using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class World
    {
        StoryDomain domain;          // PDDL domain.
        List<Goal> goalStates;       // List of goal state(s). // Or it must be StoryNode?
        List<CSPVariable> variables; // Variables for CSP model. 
        List<Agent> agents;          // Agents (include agents states).
    }
}
