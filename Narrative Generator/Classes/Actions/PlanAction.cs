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
        public bool success;

        public PlanAction()
        {

        }

        public PlanAction(params Object[] args)
        {
            foreach (Object arg in args)
            {
                Arguments.Add(arg);
            }
        }

        public abstract bool CheckPreconditions (WorldBeliefs state);

        public abstract void ApplyEffects (WorldBeliefs state);

        public bool TakeAction(WorldBeliefs state)
        {
            if (CheckPreconditions(state))
            {
                ApplyEffects(state);
                return true;
            }

            return false;
        }

        // A placeholder to add to the action the ability to apply the effects that occur when it fails.
        // At the moment, only the effects of a successful application of an action are described.
        /*public abstract void Fail()
        {

        }*/
    }
}
