using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class WorldBeliefs
    {
        private WorldStatic world;

        private List<Agent> agents;                        // Agents (include agents states).
        List<Goal> goalStates;                             // List of goal state(s). 

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

        public Agent GetAgentByName(string name)
        {
            for (int i = 0; i < agents.Count(); i++)
            {
                if (agents[i].GetName() == name)
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

        public void OrderByInitiative()
        {
            agents = agents.OrderBy(x => x.GetInitiative()).ToList();
        }

        public WorldStatic GetStaticWorldPart()
        {
            return world;
        }

        public void SetStaticWorldPart(WorldStatic world)
        {
            this.world = world;
        }
    }
}