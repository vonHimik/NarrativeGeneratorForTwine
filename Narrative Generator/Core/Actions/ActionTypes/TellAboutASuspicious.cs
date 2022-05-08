using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class TellAboutASuspicious : PlanAction
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

        public KeyValuePair<LocationStatic, LocationDynamic> Location1
        {
            get
            {
                return (KeyValuePair<LocationStatic, LocationDynamic>)Arguments[2];
            }
        }

        public KeyValuePair<LocationStatic, LocationDynamic> Location2
        {
            get
            {
                return (KeyValuePair<LocationStatic, LocationDynamic>)Arguments[3];
            }
        }

        public TellAboutASuspicious (params Object[] args) : base (args) { }

        public TellAboutASuspicious (ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                     ref KeyValuePair<AgentStateStatic, AgentStateDynamic> killer, 
                                     ref KeyValuePair<LocationStatic, LocationDynamic> location1, 
                                     ref KeyValuePair<LocationStatic, LocationDynamic> location2)
        {
            Arguments.Add(agent);
            Arguments.Add(killer);
            Arguments.Add(location1);
            Arguments.Add(location2);
        }

        public bool PreCheckPrecondition (WorldDynamic state, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            return agent.Key.GetRole().Equals(AgentRole.KILLER) && agent.Value.GetStatus();
        }

        public override bool CheckPreconditions (WorldDynamic state)
        {
            return Agent.Key.GetRole() == AgentRole.USUAL && Agent.Value.GetStatus() 
                      && Killer.Key.GetRole() == AgentRole.KILLER && Killer.Value.GetStatus()
                      && Location1.Value.SearchAgent(Agent.Key) && Location1.Value.SearchAgent(Killer.Key) && !Location1.Equals(Location2);
        }

        public override void ApplyEffects (ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateKiller = state.GetAgentByName(Killer.Key.GetName());
            KeyValuePair<LocationStatic, LocationDynamic> stateLocation2 = state.GetLocationByName(Location2.Key.GetName());

            stateAgent.Value.ClearTempStates();
            stateKiller.Value.ClearTempStates();

            stateAgent.Value.SetTargetLocation (stateLocation2.Key);
        }

        public override void Fail (ref WorldDynamic state) { fail = true; }
    }
}
