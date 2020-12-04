using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class World // Story state
    {
        // The class, in general, reflects (represents) the state of the system, the interpretation of the "world". 
        // Some classes can be used him as a "real" description of the world, others - as THEIR assumption about the current state of the "world", 
        // the third - to display their goal state of the "world".

        private List<Location> locations;
        private List<Agent> agents;             // Agents (include agents states).
        List<Goal> goalStates;                  // List of goal state(s). 
        List<Action> allActionsInGame;

        // StoryDomain domain;          // PDDL domain.
        // List<CSPVariable> variables; // Variables for CSP model. 

        public void AddLocations(List<Location> locations)
        {
            for (int i = 0; i < locations.Count; i++)
            {
                AddLocation(locations[i]);
            }
        }

        public void AddLocation(Location newLocation)
        {
            locations.Add(newLocation);
        }

        public List<Location> GetLocations()
        {
            return locations;
        }

        public void AddAgents(List<Agent> agents)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                AddAgent(agents[i]);
            }
        }

        public void AddAgent(Agent newAgent)
        {
            agents.Add(newAgent);
        }

        public void AddEmptyAgent()
        {
            Agent newAgent = new Agent();
            agents.Add(newAgent);
        }

        public List<Agent> GetAgents()
        {
            return agents;
        }

        public Agent GetAgent(int index)
        {
            return agents[index];
        }

        public int GetNumberOfAgents()
        {
            return agents.Count();
        }
    }
}
