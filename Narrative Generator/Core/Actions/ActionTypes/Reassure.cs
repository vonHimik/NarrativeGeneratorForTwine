using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// The class that implements the agent's action: "Reassure"
    /// </summary>
    [Serializable]
    class Reassure : PlanAction
    {
        /// <summary>
        /// An agent that is a participant in the action.
        /// </summary>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent1
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[0];
            }
        }

        /// <summary>
        /// An agent that is a participant in the action.
        /// </summary>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent2
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[1];
            }
        }

        /// <summary>
        /// An agent that is a participant in the action.
        /// </summary>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent3
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[2];
            }
        }

        /// <summary>
        /// The agent is an indirect participant in the action.
        /// </summary>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Killer
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[3];
            }
        }

        /// <summary>
        /// The location that is the target of the action.
        /// </summary>
        public KeyValuePair<LocationStatic, LocationDynamic> Location
        {
            get
            {
                return (KeyValuePair<LocationStatic, LocationDynamic>)Arguments[4];
            }
        }

        /// <summary>
        /// A constructor based only on the state of the story world.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public Reassure (WorldDynamic state) { DefineDescription(state); }

        /// <summary>
        /// Constructor for the action instance.
        /// </summary>
        /// <param name="args">The arguments to be passed to the action.</param>
        public Reassure(params Object[] args) : base (args) { }

        /// <summary>
        /// Constructor method with parameters.
        /// </summary>
        /// <param name="agent1">The agent is a participant in the action.</param>
        /// <param name="agent2">The agent is a participant in the action.</param>
        /// <param name="agent3">The agent is a participant in the action.</param>
        /// <param name="killer">The agent is an indirect participant in the action.</param>
        /// <param name="location">The location that is the target of the action.</param>
        public Reassure(ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent1, 
                        ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent2, 
                        ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent3, 
                        ref KeyValuePair<AgentStateStatic, AgentStateDynamic> killer, 
                        ref KeyValuePair<LocationStatic, LocationDynamic> location)
        {
            Arguments.Add(agent1);
            Arguments.Add(agent2);
            Arguments.Add(agent3);
            Arguments.Add(killer);
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
            return state.GetStaticWorldPart().Equals(Setting.DefaultDemo) && (agent.Key.GetRole().Equals(AgentRole.USUAL) || agent.Key.GetRole().Equals(AgentRole.PLAYER))
                && agent.Value.GetStatus() && agent.Value.ThinksThatSomeoneIsAngry();
        }

        /// <summary>
        /// A method that checks preconditions for an action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        /// <returns>The result of the precondition check.</returns>
        public override bool CheckPreconditions (WorldDynamic state)
        {
            return Agent1.Key.GetRole() == AgentRole.USUAL && Agent1.Value.GetStatus() 
                      && Agent2.Key.GetRole() == AgentRole.USUAL && Agent2.Value.GetStatus()
                      && Agent3.Key.GetRole() == AgentRole.USUAL 
                      && Killer.Key.GetRole() == AgentRole.ANTAGONIST 
                      && Location.Value.SearchAgent(Agent1.Key) && Location.Value.SearchAgent(Agent2.Key)
                      && (Agent1.Value.GetObjectOfAngryComponent().AngryCheckAtAgent(Agent3.Key) || Agent1.Value.GetObjectOfAngryComponent().AngryCheckAtAgent(Killer.Key))
                      && !Agent1.Value.GetBeliefs().GetAgentByRole(AgentRole.ANTAGONIST).Equals(Killer);
        }

        /// <summary>
        /// A method that changes the passed world state according to the effects of the action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public override void ApplyEffects (ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent1 = state.GetAgentByName(Agent1.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent2 = state.GetAgentByName(Agent2.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent3 = state.GetAgentByName(Agent3.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateKiller = state.GetAgentByName(Killer.Key.GetName());

            stateAgent1.Value.ClearTempStates();
            stateAgent2.Value.ClearTempStates();
            stateAgent3.Value.ClearTempStates();
            stateKiller.Value.ClearTempStates();

            stateAgent1.Value.CalmDown();

            stateAgent2.Value.DecreaseTimeToMove();
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
