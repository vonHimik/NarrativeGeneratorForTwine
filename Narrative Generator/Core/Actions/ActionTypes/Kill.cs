using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class Kill : PlanAction
    {
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[0];
            }
        }

        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Killer
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[1];
            }
        }

        public KeyValuePair<LocationStatic, LocationDynamic> Location
        {
            get
            {
                return (KeyValuePair<LocationStatic, LocationDynamic>)Arguments[2];
            }
        }

        public Kill (WorldDynamic state) { DefineDescription(state); }

        public Kill (params Object[] args) : base (args) { }

        public Kill (ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                     ref KeyValuePair<AgentStateStatic, AgentStateDynamic> killer, 
                     ref KeyValuePair<LocationStatic, LocationDynamic> location)
        {
            Arguments.Add(agent);
            Arguments.Add(killer);
            Arguments.Add(location);
        }

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

        public bool PreCheckPrecondition (WorldDynamic state, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            return (agent.Key.GetRole().Equals(AgentRole.ANTAGONIST) || agent.Key.GetRole().Equals(AgentRole.ENEMY)) && agent.Value.GetStatus();
        }

        public override bool CheckPreconditions (WorldDynamic state)
        {
            return Agent.Key.GetRole() == AgentRole.USUAL && Agent.Value.GetStatus()
                   && (Killer.Key.GetRole() == AgentRole.ANTAGONIST || Killer.Key.GetRole() == AgentRole.ENEMY) && Killer.Value.GetStatus()
                   && Location.Value.SearchAgent(Agent.Key) && Location.Value.SearchAgent(Killer.Key) && Location.Value.CountAgents() == 2;
        }

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

        public override void Fail (ref WorldDynamic state)
        {
            fail = true;

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
    }
}
