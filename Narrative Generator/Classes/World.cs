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

        private List<Location> locations;                  // List of locations.
        private List<Agent> agents;                        // Agents (include agents states).
        List<Goal> goalStates;                             // List of goal state(s). 

        static Action agentMove = new Action("agent_move", true, false, false);
        static Action kill = new Action("kill", false, false, true);
        List<Action> allActionsInGame = new List<Action>() // List of all actions.
        {
            agentMove, kill
        };

        private int turn = 0;

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

        public Agent GetAgentByRole(string role)
        {
            for (int i = 0; i < agents.Count(); i++)
            {
                if (agents[i].GetRole() == role)
                {
                    return agents[i];
                }
            }

            return null;
        }

        public int GetNumberOfAgents()
        {
            return agents.Count();
        }

        public List<Action> GetAllActions()
        {
            return allActionsInGame;
        }

        public Action GetAction (int index)
        {
            return allActionsInGame[index];
        }

        public int CountActions()
        {
            return allActionsInGame.Count();
        }

        public int GetTurnNumber()
        {
            return turn;
        }

        public void IncreaseTurnNumber()
        {
            turn++;
        }

        public void OrderByInitiative()
        {
            agents = agents.OrderBy(x => x.GetInitiative()).ToList();
        }
    }
}
