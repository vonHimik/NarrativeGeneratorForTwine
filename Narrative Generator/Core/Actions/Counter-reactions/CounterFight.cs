using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// The class that implements the agent's action: "Counter Fight"
    /// </summary>
    [Serializable]
    class CounterFight : PlanAction
    {
        /// <summary>
        /// Agent which applies the action.
        /// </summary>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent1
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[0];
            }
        }

        /// <summary>
        /// Agent who is the target of the action.
        /// </summary>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent2
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[1];
            }
        }

        /// <summary>
        /// The location that is the target of the action.
        /// </summary>
        public KeyValuePair<LocationStatic, LocationDynamic> Location
        {
            get
            {
                return (KeyValuePair<LocationStatic, LocationDynamic>)Arguments[2];
            }
        }

        /// <summary>
        /// The original action that the agent tried to take but was replaced by this one.
        /// </summary>
        public KeyValuePair<string, PlanAction> OriginalAction
        {
            get
            {
                return (KeyValuePair<string, PlanAction>)Arguments[3];
            }
        }

        /// <summary>
        /// A constructor based only on the state of the story world.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public CounterFight (WorldDynamic state) { DefineDescription(state); }

        /// <summary>
        /// Constructor for the action instance.
        /// </summary>
        /// <param name="args">The arguments to be passed to the action.</param>
        public CounterFight(params Object[] args) : base(args) { }

        /// <summary>
        /// Constructor method with parameters.
        /// </summary>
        /// <param name="agent1">Agent which applies the action.</param>
        /// <param name="agent2">Agent who is the target of the action.</param>
        /// <param name="location">The location that is the target of the action.</param>
        /// <param name="originalAction">The original action that the agent tried to take but was replaced by this one.</param>
        public CounterFight(ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent1,
                            ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent2,
                            ref KeyValuePair<LocationStatic, LocationDynamic> location,
                            string originalAction)
        {
            Arguments.Add(agent1);
            Arguments.Add(agent2);
            Arguments.Add(location);
            Arguments.Add(originalAction);
        }

        /// <summary>
        /// A method that creates a description of the action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public override void DefineDescription (WorldDynamic state)
        {
            desc = GetType().ToString().Remove(0, 20);
        }

        /// <summary>
        /// A method that checks preconditions for an action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        /// <returns>The result of the precondition check.</returns>
        public override bool CheckPreconditions(WorldDynamic state)
        {
            return Agent1.Key.GetRole() == AgentRole.USUAL && Agent1.Value.GetStatus()
                      && Agent2.Key.GetRole() == AgentRole.ANTAGONIST && Agent2.Value.GetStatus()
                      && Location.Value.SearchAgent(Agent1.Key) && Location.Value.SearchAgent(Agent2.Key)
                      && Agent1.Value.GetObjectOfAngryComponent().AngryCheckAtAgent(Agent2.Key);
        }

        /// <summary>
        /// A method that changes the passed world state according to the effects of the action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public override void ApplyEffects(ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent1.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent2 = state.GetAgentByName(Agent2.Key.GetName());

            stateAgent.Value.ClearTempStates();
            stateAgent2.Value.ClearTempStates();

            stateAgent2.Value.SetStatus(false);

            stateAgent.Value.DecreaseTimeToMove();
        }

        /// <summary>
        /// A method that implements the action's failure effect.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public override void Fail(ref WorldDynamic state) { fail = true; }

        /// <summary>
        /// In counter-actions, returns the name of the action that caused the system to react.
        /// </summary>
        /// <returns>A couple from the action and separately its name.</returns>
        public override KeyValuePair<string, PlanAction> ReturnOriginal()
        {
            return OriginalAction;
        }
    }
}
