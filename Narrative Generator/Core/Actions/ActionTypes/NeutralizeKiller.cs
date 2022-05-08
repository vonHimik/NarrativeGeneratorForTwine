using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class NeutralizeKiller : PlanAction
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

        public NeutralizeKiller (params Object[] args) : base (args) { }

        public NeutralizeKiller (ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                 ref KeyValuePair<AgentStateStatic, AgentStateDynamic> killer, 
                                 ref LocationStatic location)
        {
            Arguments.Add(agent);
            Arguments.Add(killer);
            Arguments.Add(location);
        }

        public bool PreCheckPrecondition (WorldDynamic state, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            return (agent.Key.GetRole() == AgentRole.USUAL || agent.Key.GetRole() == AgentRole.PLAYER) && agent.Value.GetStatus()
                && agent.Value.GetObjectOfAngryComponent().AngryCheck()
                && agent.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetRole() == AgentRole.KILLER
                && state.GetAgentByName(agent.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName()).Value.GetStatus()
                && state.GetLocationByName(agent.Value.GetBeliefs().GetMyLocation().GetName()).
                Equals(state.GetLocationByName(state.GetAgentByName(agent.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName()).Value.GetBeliefs().GetMyLocation().GetName()));
        }

        public override bool CheckPreconditions (WorldDynamic state)
        {
            return Agent.Key.GetRole() == AgentRole.USUAL && Agent.Value.GetStatus() 
                      && Killer.Key.GetRole() == AgentRole.KILLER && Killer.Value.GetStatus()
                      && Location.Value.SearchAgent(Agent.Key) && Location.Value.SearchAgent(Killer.Key) 
                      && Agent.Value.GetObjectOfAngryComponent().AngryCheckAtAgent(Killer.Key);
        }

        public override void ApplyEffects (ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateKiller = state.GetAgentByName(Killer.Key.GetName());

            stateAgent.Value.ClearTempStates();
            stateKiller.Value.ClearTempStates();

            stateKiller.Value.SetStatus(false);
        }

        public override void Fail (ref WorldDynamic state) { fail = true; }
    }
}
