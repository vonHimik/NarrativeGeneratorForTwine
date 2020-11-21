using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class World
    {
        StoryDomain domain;
        List<Goal> goalStates;
        List<CSPVariable> variables;
        List<Agent> agents;
    }
}
