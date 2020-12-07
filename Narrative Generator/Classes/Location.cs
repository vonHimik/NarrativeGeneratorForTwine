using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class Location
    {
        private string name;
        private List<Agent> containedAgents;
        List<Location> connectedLocations;

        public Location (string name)
        {
            this.name = name;
        }

        public void SetName (string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }

        public void AddAgent (Agent agent)
        {
            containedAgents.Add(agent);
        }

        public void AddAgents (List<Agent> agents)
        {
            containedAgents.AddRange(agents);
        }

        public void RemoveAgent(Agent agent)
        {
            containedAgents.Remove(agent);
        }

        public bool SearchForAnAgent (Agent agent)
        {
            for (int i = 0; i < containedAgents.Count(); i++)
            {
                if (containedAgents[i].GetName() == agent.GetName())
                {
                    return true;
                }
            }

            return false;
        }

        public int CountAgents()
        {
            return containedAgents.Count();
        }
    }
}
