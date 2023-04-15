using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// The class that implements the agent's action: "Kill"
    /// </summary>
    [Serializable]
    class Kill : PlanAction
    {
        /// <summary>
        /// Agent who is the target of the action.
        /// </summary>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[0];
            }
        }

        /// <summary>
        /// Agent which applies the action.
        /// </summary>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Killer
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
        public Kill (WorldDynamic state) { DefineDescription(state); }

        /// <summary>
        /// Constructor for the action instance.
        /// </summary>
        /// <param name="args">The arguments to be passed to the action.</param>
        public Kill (params Object[] args) : base (args) { }

        /// <summary>
        /// Constructor method with parameters.
        /// </summary>
        /// <param name="agent">Agent who is the target of the action.</param>
        /// <param name="killer">Agent which applies the action.</param>
        /// <param name="location">The location that is the target of the action.</param>
        public Kill (ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                     ref KeyValuePair<AgentStateStatic, AgentStateDynamic> killer, 
                     ref KeyValuePair<LocationStatic, LocationDynamic> location)
        {
            Arguments.Add(agent);
            Arguments.Add(killer);
            Arguments.Add(location);
        }

        /// <summary>
        /// A method that creates a description of the action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public override void DefineDescription (WorldDynamic state)
        {
            if (state.GetStaticWorldPart().GetSetting().Equals(Setting.Detective) && state.GetStaticWorldPart().GetUniqWaysToKillStatus() 
                   && Arguments.Count > 0 && Arguments[0] != null)
            {
                switch (Agent.Key.GetName())
                {
                    case "Player": desc = "Poison with potassium cyanide"; break;
                    case "JohnGordonMacArthur": desc = "Poison with sleeping pills"; break;
                    case "ThomasRogers": desc = "Kill with a hit to the head"; break;
                    case "EmilyCarolineBrent": desc = "Kill by hacking with an ax"; break;
                }
            }
            else
            {
                desc = GetType().ToString().Remove(0, 20);
            }
        }

        /// <summary>
        /// A method that checks the most basic preconditions for an action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        /// <param name="agent">Agent which applies the action.</param>
        /// <returns>The result of the precondition check.</returns>
        public bool PreCheckPrecondition (WorldDynamic state, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            return (agent.Key.GetRole().Equals(AgentRole.ANTAGONIST) || agent.Key.GetRole().Equals(AgentRole.ENEMY)) && agent.Value.GetStatus();
        }

        /// <summary>
        /// A method that checks preconditions for an action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        /// <returns>The result of the precondition check.</returns>
        public override bool CheckPreconditions (WorldDynamic state)
        {
            return Agent.Key.GetRole() == AgentRole.USUAL && Agent.Value.GetStatus()
                   && (Killer.Key.GetRole() == AgentRole.ANTAGONIST || Killer.Key.GetRole() == AgentRole.ENEMY) && Killer.Value.GetStatus()
                   && Location.Value.SearchAgent(Agent.Key) && Location.Value.SearchAgent(Killer.Key) && Location.Value.CountAgents() == 2;
        }

        /// <summary>
        /// A method that changes the passed world state according to the effects of the action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public override void ApplyEffects (ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateKiller = state.GetAgentByName(Killer.Key.GetName());

            stateAgent.Value.ClearTempStates();
            stateKiller.Value.ClearTempStates();

            stateAgent.Value.SetStatus(false);
            stateAgent.Value.GetBeliefs().GetAgentByName(stateAgent.Key.GetName()).Dead();
            stateKiller.Value.GetBeliefs().GetAgentByName(stateAgent.Key.GetName()).Dead();

            state.GetLocationByName(state.SearchAgentAmongLocations(stateAgent.Key).GetName()).Value.GetAgent(stateAgent).Value.Die();
            //state.GetLocationByName(state.SearchAgentAmongLocations(stateAgent.Key).GetName()).Value.RemoveDiedAgents();

            stateKiller.Value.DecreaseTimeToMove();

            DefineDescription(state);
        }

        /// <summary>
        /// A method that implements the action's failure effect.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public override void Fail (ref WorldDynamic state)
        {
            fail = true;
            success = false;

            if (state.GetStaticWorldPart().GetRandomBattlesResultsStatus())
            {
                KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent.Key.GetName());
                KeyValuePair<AgentStateStatic, AgentStateDynamic> stateKiller = state.GetAgentByName(Killer.Key.GetName());

                stateAgent.Value.ClearTempStates();
                stateKiller.Value.ClearTempStates();

                stateKiller.Value.SetStatus(false);
                stateKiller.Value.GetBeliefs().GetAgentByName(stateKiller.Key.GetName()).Dead();
                stateAgent.Value.GetBeliefs().GetAgentByName(stateKiller.Key.GetName()).Dead();

                state.GetLocationByName(state.SearchAgentAmongLocations(stateKiller.Key).GetName()).Value.GetAgent(stateKiller).Value.Die();

                DefineDescription(state);
            }
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
