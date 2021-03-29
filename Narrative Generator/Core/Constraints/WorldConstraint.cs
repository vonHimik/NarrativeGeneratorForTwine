using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public abstract class WorldConstraint
    {
        public abstract bool IsSatisfied (WorldDynamic state);
    }
}
