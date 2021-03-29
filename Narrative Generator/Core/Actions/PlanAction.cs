using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public abstract class PlanAction
    {
        public List<Object> Arguments = new List<object>();
        public bool success = false;
        public bool fail = false;

        public PlanAction() {}

        public PlanAction(params Object[] args)
        {
            foreach (Object arg in args)
            {
                Arguments.Add(arg);
            }
        }

        public abstract bool CheckPreconditions (WorldDynamic state);

        public abstract void ApplyEffects (ref WorldDynamic state);

        public bool TakeAction(WorldDynamic state)
        {
            if (CheckPreconditions(state))
            {
                ApplyEffects(ref state);
                return true;
            }

            return false;
        }

        public void Fail()
        {
            fail = true;
        }
    }
}
