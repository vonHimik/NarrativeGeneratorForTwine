using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class UnexpectedObstacle : PlanAction
    {
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Intruder
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[0];
            }
        }

        public UnexpectedObstacle(params Object[] args) : base(args) { }

        public UnexpectedObstacle(ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            Arguments.Add(agent);
        }

        public override bool CheckPreconditions(WorldDynamic state) { return true; }

        public override void ApplyEffects(ref WorldDynamic state) { }

        public override void Fail(ref WorldDynamic state) { fail = true; }
    }
}
