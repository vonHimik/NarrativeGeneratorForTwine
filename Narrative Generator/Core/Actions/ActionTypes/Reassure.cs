using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class Reassure : PlanAction
    {
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent1
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[0];
            }
        }

        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent2
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[1];
            }
        }

        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent3
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[2];
            }
        }

        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Killer
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[3];
            }
        }

        public KeyValuePair<LocationStatic, LocationDynamic> Location
        {
            get
            {
                return (KeyValuePair<LocationStatic, LocationDynamic>)Arguments[4];
            }
        }

        public Reassure(params Object[] args) : base(args) { }

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

        public override bool CheckPreconditions(WorldDynamic state)
        {
            return Agent1.Key.GetRole() == AgentRole.USUAL && Agent1.Value.GetStatus() 
                      && Agent2.Key.GetRole() == AgentRole.USUAL && Agent2.Value.GetStatus()
                      && Agent3.Key.GetRole() == AgentRole.USUAL 
                      && Killer.Key.GetRole() == AgentRole.KILLER 
                      && Location.Value.SearchAgent(Agent1.Key) && Location.Value.SearchAgent(Agent2.Key)
                      && (Agent1.Value.GetObjectOfAngry().AngryCheckAtAgent(Agent3.Key) || Agent1.Value.GetObjectOfAngry().AngryCheckAtAgent(Killer.Key))
                      && !Agent1.Value.GetBeliefs().GetAgentByRole(AgentRole.KILLER).Equals(Killer);
        }

        public override void ApplyEffects(ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent1 = state.GetAgentByName(Agent1.Key.GetName());

            stateAgent1.Value.CalmDown();
        }
    }
}
