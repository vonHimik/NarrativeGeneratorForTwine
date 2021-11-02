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

        public NeutralizeKiller(params Object[] args) : base(args) { }

        public NeutralizeKiller(ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                ref KeyValuePair<AgentStateStatic, AgentStateDynamic> killer, 
                                ref LocationStatic location)
        {
            Arguments.Add(agent);
            Arguments.Add(killer);
            Arguments.Add(location);
        }

        public override bool CheckPreconditions(WorldDynamic state)
        {
            return Agent.Key.GetRole() == AgentRole.USUAL && Agent.Value.GetStatus() 
                      && Killer.Key.GetRole() == AgentRole.KILLER && Killer.Value.GetStatus()
                      && Location.Value.SearchAgent(Agent.Key) && Location.Value.SearchAgent(Killer.Key) 
                      && Agent.Value.GetObjectOfAngry().AngryCheckAtAgent(Killer.Key);
        }

        public override void ApplyEffects(ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateKiller = state.GetAgentByName(Killer.Key.GetName());

            stateKiller.Value.SetStatus(false);
        }

        public override void Fail(ref WorldDynamic state) { fail = true; }
    }
}
