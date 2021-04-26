using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class Talk : PlanAction
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

        public string Topic
        {
            get
            {
                return (string)Arguments[2];
            }
        }

        public Talk(params Object[] args) : base(args) { }

        public Talk  (ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent1,
                      ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent2,
                      ref string topic)
        {
            Arguments.Add(agent1);
            Arguments.Add(agent2);
            Arguments.Add(topic);
        }

        public override bool CheckPreconditions(WorldDynamic state)
        {
            return Agent1.Key.GetRole() == AgentRole.USUAL && Agent1.Value.GetStatus()
                   && Agent2.Key.GetRole() == AgentRole.USUAL && Agent2.Value.GetStatus()
                   && (Agent1.Value.GetMyLocation() == Agent2.Value.GetMyLocation());
        }

        public override void ApplyEffects(ref WorldDynamic state) { }
    }
}
