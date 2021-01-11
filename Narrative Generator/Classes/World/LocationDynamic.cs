using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class LocationDynamic
    {
        private Dictionary<KeyValuePair<AgentStateStatic, AgentStateDynamic>, LocationStatic> agentsAtLocations;
        private bool containEvidence;

        public LocationDynamic() {}

        public LocationDynamic(bool containEvidence)
        {
            this.containEvidence = containEvidence;
        }

        public void AddAgent(LocationStatic location, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            agentsAtLocations.Add(agent, location);
        }

        public void AddAgents(Dictionary<KeyValuePair<AgentStateStatic, AgentStateDynamic>, LocationStatic> agents)
        {
            agentsAtLocations = agentsAtLocations.Concat(agents).ToDictionary(x => x.Key, x => x.Value);
        }

        public Dictionary<AgentStateStatic, AgentStateDynamic> GetAgents()
        {
            Dictionary<AgentStateStatic, AgentStateDynamic> agents = new Dictionary<AgentStateStatic, AgentStateDynamic>();

            foreach (var agent in agentsAtLocations)
            {
                agents.Add(agent.Key.Key, agent.Key.Value);
            }

            return agents;
        }

        public void RemoveAgent(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            agentsAtLocations.Remove(agent);
        }

        public void ClearLocation()
        {
            foreach (var agent in agentsAtLocations)
            {
                agentsAtLocations.Remove(agent.Key);
            }
        }

        public bool SearchAgent(AgentStateStatic agent)
        {
            foreach (var a in agentsAtLocations)
            {
                if (a.Key.Key.GetName().Equals(agent.GetName()))
                {
                    return true;
                }
            }

            return false;
        }

        public int CountAgents()
        {
            return agentsAtLocations.Count();
        }

        public bool CheckEvidence()
        {
            return containEvidence;
        }
    }
}
