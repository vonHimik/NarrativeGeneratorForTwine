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

        public Kill (params Object[] args) : base (args) { }

        public Kill (ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                     ref KeyValuePair<AgentStateStatic, AgentStateDynamic> killer, 
                     ref KeyValuePair<LocationStatic, LocationDynamic> location)
        {
            Arguments.Add(agent);
            Arguments.Add(killer);
            Arguments.Add(location);
        }

        public bool PreCheckPrecondition (WorldDynamic state, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            return (agent.Key.GetRole().Equals(AgentRole.KILLER) || agent.Key.GetRole().Equals(AgentRole.BOSS)) && agent.Value.GetStatus();
        }

        public override bool CheckPreconditions (WorldDynamic state)
        {
            return Agent.Key.GetRole() == AgentRole.USUAL && Agent.Value.GetStatus()
                   && (Killer.Key.GetRole() == AgentRole.KILLER || Killer.Key.GetRole() == AgentRole.BOSS) && Killer.Value.GetStatus()
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
        }

        public override void Fail (ref WorldDynamic state) { fail = true; }
    }
}
