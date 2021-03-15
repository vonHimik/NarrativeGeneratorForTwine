using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class LocationDynamic : ICloneable
    {
        private LocationStatic locationInfo;
        private Dictionary<AgentStateStatic, AgentStateDynamic> agentsAtLocations;
        private bool containEvidence;
        int id;

        public LocationDynamic()
        {
            locationInfo = new LocationStatic();
            agentsAtLocations = new Dictionary<AgentStateStatic, AgentStateDynamic>();
            containEvidence = false;

            Random rand = new Random();
            id = rand.Next(1000);
        }

        public LocationDynamic(bool containEvidence)
        {
            locationInfo = new LocationStatic();
            agentsAtLocations = new Dictionary<AgentStateStatic, AgentStateDynamic>();
            this.containEvidence = containEvidence;
        }

        public LocationDynamic(bool containEvidence, LocationStatic locationInfo)
        {
            this.locationInfo = locationInfo;
            agentsAtLocations = new Dictionary<AgentStateStatic, AgentStateDynamic>();
            this.containEvidence = containEvidence;
        }

        public object Clone()
        {
            var clone = new LocationDynamic();

            foreach (var agent in agentsAtLocations)
            {
                AgentStateStatic sTemp = (AgentStateStatic)agent.Key.Clone();
                AgentStateDynamic dTemp = (AgentStateDynamic)agent.Value.Clone();
                clone.agentsAtLocations.Add(sTemp, dTemp);
            }

            //clone.agentsAtLocations = agentsAtLocations;
            clone.containEvidence = containEvidence;

            return clone;
        } 

        public void AddAgent(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            AgentStateStatic sPrefab = (AgentStateStatic)agent.Key.Clone();
            AgentStateDynamic dPrefab = (AgentStateDynamic)agent.Value.Clone();
            agentsAtLocations.Add(sPrefab, dPrefab);
        }

        public void AddAgents(Dictionary<AgentStateStatic, AgentStateDynamic> agents)
        {
            agentsAtLocations = agentsAtLocations.Concat(agents).ToDictionary(x => x.Key, x => x.Value);
        }

        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetAgent(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            foreach (var a in agentsAtLocations)
            {
                if (a.Key.GetName() == agent.Key.GetName())
                {
                    return a;
                }
            }

            throw new KeyNotFoundException();
        }

        public Dictionary<AgentStateStatic, AgentStateDynamic> GetAgents()
        {
            Dictionary<AgentStateStatic, AgentStateDynamic> agents = new Dictionary<AgentStateStatic, AgentStateDynamic>();

            foreach (var agent in agentsAtLocations)
            {
                agents.Add(agent.Key, agent.Value);
            }

            return agents;
        }

        public bool RemoveAgent(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            foreach (var a in agentsAtLocations)
            {
                if (a.Key.GetName() == agent.Key.GetName())
                {
                    if (agentsAtLocations.Remove(a.Key)) { return true; }
                    else { return false; }
                }
            }

            return false;
        }

        public void ClearLocation(WorldDynamic currentWorldState)
        {
            foreach (var agent in agentsAtLocations.ToArray())
            {
                agentsAtLocations.Remove(agent.Key);
            }
        }

        public bool SearchAgent(AgentStateStatic agent)
        {
            foreach (var a in agentsAtLocations)
            {
                if (a.Key.GetName().Equals(agent.GetName()))
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

        public void SetLocationInfo(LocationStatic locationInfo)
        {
            this.locationInfo = locationInfo;
        }

        public LocationStatic GetLocationInfo()
        {
            return locationInfo;
        }
    }
}
