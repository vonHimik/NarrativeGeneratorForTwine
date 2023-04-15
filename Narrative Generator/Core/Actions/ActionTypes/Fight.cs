using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// The class that implements the agent's action: "Fight"
    /// </summary>
    [Serializable]
    class Fight : PlanAction
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
        /// A constructor based only on the state of the story world.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public Fight (WorldDynamic state) { DefineDescription(state); }

        /// <summary>
        /// Constructor for the action instance.
        /// </summary>
        /// <param name="args">The arguments to be passed to the action.</param>
        public Fight (params Object[] args) : base (args) { }

        /// <summary>
        /// Constructor method with parameters.
        /// </summary>
        /// <param name="agent1">Agent which applies the action.</param>
        /// <param name="agent2">Agent who is the target of the action.</param>
        /// <param name="location">The location that is the target of the action.</param>
        public Fight (ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent1, 
                      ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent2, 
                      ref KeyValuePair<LocationStatic, LocationDynamic> location)
        {
            Arguments.Add(agent1);
            Arguments.Add(agent2);
            Arguments.Add(location);
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
        /// A method that checks the most basic preconditions for an action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        /// <param name="agent">Agent which applies the action.</param>
        /// <returns>The result of the precondition check.</returns>
        public bool PreCheckPrecondition (WorldDynamic state, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            return /*(agent.Key.GetRole() == AgentRole.USUAL || agent.Key.GetRole() == AgentRole.PLAYER) &&*/ agent.Value.GetStatus()
                    && agent.Value.GetObjectOfAngryComponent().AngryCheck()
                    && state.GetAgentByName(agent.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName()).Value.GetStatus()
                    && state.GetLocationByName(agent.Value.GetBeliefs().GetMyLocation().GetName()).Key.
                       Equals(state.GetAgentByName(agent.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName()).Value.GetBeliefs().GetMyLocation())
                    && !state.GetStaticWorldPart().GetSetting().Equals(Setting.Detective);
        }

        /// <summary>
        /// A method that checks preconditions for an action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        /// <returns>The result of the precondition check.</returns>
        public override bool CheckPreconditions (WorldDynamic state)
        {
            return    (Agent1.Key.GetRole() == AgentRole.USUAL || Agent1.Key.GetRole() == AgentRole.PLAYER) && Agent1.Value.GetStatus() 
                      && Agent2.Value.GetStatus()
                      && Location.Value.SearchAgent(Agent1.Key) && Location.Value.SearchAgent(Agent2.Key) 
                      && Agent1.Value.GetObjectOfAngryComponent().AngryCheckAtAgent(Agent2.Key);
        }

        /// <summary>
        /// A method that changes the passed world state according to the effects of the action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public override void ApplyEffects (ref WorldDynamic state)
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
        public override void Fail (ref WorldDynamic state)
        {
            fail = true;
            success = false;

            //if (state.GetStaticWorldPart().GetRandomBattlesResultsStatus())
            //{
                KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent1.Key.GetName());
                KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent2 = state.GetAgentByName(Agent2.Key.GetName());

                stateAgent.Value.ClearTempStates();
                stateAgent2.Value.ClearTempStates();

                stateAgent.Value.SetStatus(false);
            //}
        }

        /// <summary>
        /// In counter-actions, returns the name of the action that caused the system to react.
        /// </summary>
        /// <returns>A couple from the action and separately its name.</returns>
        public override KeyValuePair<string, PlanAction> ReturnOriginal()
        {
            throw new NotImplementedException();
        }
    }
}
