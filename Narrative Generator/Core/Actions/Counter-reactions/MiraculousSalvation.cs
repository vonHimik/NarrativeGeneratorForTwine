using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class MiraculousSalvation : PlanAction
    {
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Victim
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[0];
            }
        }

        public MiraculousSalvation(params Object[] args) : base(args) { }

        public MiraculousSalvation(ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            Arguments.Add(agent);
        }

        public override bool CheckPreconditions(WorldDynamic state) { return true; }

        public override void ApplyEffects(ref WorldDynamic state) {}

        public override void Fail(ref WorldDynamic state) { fail = true; }
    }
}
