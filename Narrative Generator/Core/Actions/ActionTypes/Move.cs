using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// The class that implements the agent's action: "Move"
    /// </summary>
    [Serializable]
    class Move : PlanAction
    {
        /// <summary>
        /// Agent which applies the action.
        /// </summary>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[0];
            }
        }

        /// <summary>
        /// The location that is the target of the action (1).
        /// </summary>
        public KeyValuePair<LocationStatic, LocationDynamic> From
        {
            get
            {
                return (KeyValuePair<LocationStatic, LocationDynamic>)Arguments[1];
            }
        }

        /// <summary>
        /// The location that is the target of the action (2).
        /// </summary>
        public KeyValuePair<LocationStatic, LocationDynamic> To
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
        public Move (WorldDynamic state) { DefineDescription(state); }

        /// <summary>
        /// Constructor for the action instance.
        /// </summary>
        /// <param name="args">The arguments to be passed to the action.</param>
        public Move (params Object[] args) : base (args) {}

        /// <summary>
        /// Constructor method with parameters.
        /// </summary>
        /// <param name="agent">Agent which applies the action.</param>
        /// <param name="from">The location (1) that is the target of the action.</param>
        /// <param name="to">The location (2) that is the target of the action.</param>
        public Move (ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                     ref KeyValuePair<LocationStatic, LocationDynamic> from, 
                     ref KeyValuePair<LocationStatic, LocationDynamic> to)
        {
            Arguments.Add(agent);
            Arguments.Add(from);
            Arguments.Add(to);
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
        public bool PreCheckPrecondition(WorldDynamic state, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            return agent.Value.GetStatus() && (agent.Key.GetRole().Equals(AgentRole.ANTAGONIST) || agent.Key.GetRole().Equals(AgentRole.PLAYER)
                    || agent.Key.GetRole().Equals(AgentRole.ENEMY))
                    || (agent.Key.GetRole().Equals(AgentRole.USUAL) && agent.Value.GetTargetLocation() != null);
        }

        /// <summary>
        /// A method that checks preconditions for an action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        /// <returns>The result of the precondition check.</returns>
        public override bool CheckPreconditions (WorldDynamic state)
        {
            return Agent.Value.GetStatus() && From.Value.SearchAgent(Agent.Key) && !To.Value.SearchAgent(Agent.Key);
        }

        /// <summary>
        /// A method that changes the passed world state according to the effects of the action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public override void ApplyEffects (ref WorldDynamic state)
        {
            KeyValuePair<LocationStatic, LocationDynamic> stateFrom = state.GetLocationByName(From.Key.GetName());
            KeyValuePair<LocationStatic, LocationDynamic> stateTo = state.GetLocationByName(To.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent.Key.GetName());

            stateAgent.Value.ClearTempStates();

            stateFrom.Value.RemoveAgent(stateAgent);
            stateAgent.Value.GetBeliefs().GetAgentByName(stateAgent.Key.GetName()).ClearLocation();
            stateAgent.Value.GetBeliefs().ClearMyLocation();

            stateTo.Value.AddAgent(stateAgent);
            stateAgent.Value.GetBeliefs().GetAgentByName(stateAgent.Key.GetName()).
                SetLocation(stateAgent.Value.GetBeliefs().GetLocationByName(To.Key.GetName()));
            stateAgent.Value.GetBeliefs().SetMyLocation(stateAgent.Value.GetBeliefs().GetLocationByName(To.Key.GetName()));

            if (stateTo.Key.Equals(stateAgent.Value.GetTargetLocation()))
            {
                stateAgent.Value.ClearTargetLocation();
            }

            stateAgent.Value.SetTimeToMove(1);
        }

        /// <summary>
        /// A method that implements the action's failure effect.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public override void Fail (ref WorldDynamic state) { fail = true; }

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

