using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class EnvironmentIsNotConduciveToTalk : PlanAction
    {
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Speaker
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[0];
            }
        }

        public EnvironmentIsNotConduciveToTalk(params Object[] args) : base(args) { }

        public EnvironmentIsNotConduciveToTalk(ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            Arguments.Add(agent);
        }

        public override bool CheckPreconditions(WorldDynamic state) { return true; }

        public override void ApplyEffects(ref WorldDynamic state) { }
    }
}
