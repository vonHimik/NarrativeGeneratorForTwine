using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that implements a dynamic (often changeable) part of a location.
    /// </summary>
    [Serializable]
    public class LocationDynamic : ICloneable
    {
        // Link to the static part of the location.
        private LocationStatic locationInfo;

        // List of agents in the location.
        private Dictionary<AgentStateStatic, AgentStateDynamic> agentsAtLocations;

        // A flag indicating whether the location contains evidence or not.
        private bool containEvidence;

        // Numeric identifier for the location.
        int id;

        /// <summary>
        /// Constructor method for the dynamic part of the location, without parameters.
        /// </summary>
        public LocationDynamic()
        {
            locationInfo = new LocationStatic();
            agentsAtLocations = new Dictionary<AgentStateStatic, AgentStateDynamic>();
            containEvidence = false;

            // We assign a random integer ID.
            Random rand = new Random();
            id = rand.Next(1000);
        }

        /// <summary>
        /// Constructor method for the dynamic part of the location, the value of the flag about the presence of evidence is used as a parameter.
        /// </summary>
        /// <param name="containEvidence"></param>
        public LocationDynamic(bool containEvidence)
        {
            locationInfo = new LocationStatic();
            agentsAtLocations = new Dictionary<AgentStateStatic, AgentStateDynamic>();
            this.containEvidence = containEvidence;
        }

        /// <summary>
        /// Constructor method for the dynamic part of the location,
        /// as a parameter using the values for the flag about the presence of evidence and a link to the static part of the location.
        /// </summary>
        /// <param name="containEvidence"></param>
        /// <param name="locationInfo"></param>
        public LocationDynamic(bool containEvidence, LocationStatic locationInfo)
        {
            this.locationInfo = locationInfo;
            agentsAtLocations = new Dictionary<AgentStateStatic, AgentStateDynamic>();
            this.containEvidence = containEvidence;
        }

        /// <summary>
        /// Returns a clone of the dynamic part of the location that called this method.
        /// </summary>
        public object Clone()
        {
            // We create an empty instance of the dynamic part of the location.
            var clone = new LocationDynamic();

            // We go through each agent from the list of those in the location, separately clone their static and dynamic parts,
            // and then we pass them to the clone (collecting them into one whole).
            foreach (var agent in this.agentsAtLocations)
            {
                AgentStateStatic sTemp = (AgentStateStatic)agent.Key.Clone();
                AgentStateDynamic dTemp = (AgentStateDynamic)agent.Value.Clone();
                clone.agentsAtLocations.Add(sTemp, dTemp);

                // Очистка
                sTemp = null;
                dTemp = null;
                GC.Collect();
            }

            // Copy the flag value.
            clone.containEvidence = containEvidence;

            // We return the clone.
            return clone;
        }

        /// <summary>
        /// Adds an agent to the list of agents located in this location.
        /// </summary>
        /// <param name="agent"></param>
        public void AddAgent(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            foreach (var a in agentsAtLocations)
            {
                if (a.Key.GetName().Equals(agent.Key.GetName()))
                {
                    return;
                }
            }

            AgentStateStatic sPrefab = (AgentStateStatic)agent.Key.Clone();
            AgentStateDynamic dPrefab = (AgentStateDynamic)agent.Value.Clone();
            agentsAtLocations.Add(sPrefab, dPrefab);

            // Очистка
            sPrefab = null;
            dPrefab = null;
            GC.Collect();
        }

        /// <summary>
        /// Adds a set of agents to the list of agents located in this location.
        /// </summary>
        /// <param name="agents"></param>
        public void AddAgents(Dictionary<AgentStateStatic, AgentStateDynamic> agents)
        {
            agentsAtLocations = agentsAtLocations.Concat(agents).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Returns the specified agent from the list of agents in the location.
        /// </summary>
        /// <param name="agent"></param>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetAgent(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            // We go through all the agents in the location.
            foreach (var a in agentsAtLocations)
            {
                // Compare the name of the agent being checked with the name of the agent being searched for.
                if (a.Key.GetName() == agent.Key.GetName())
                {
                    // Return the agent on match.
                    return a;
                }
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Returns a list of agents located in the given location.
        /// </summary>
        public Dictionary<AgentStateStatic, AgentStateDynamic> GetAgents()
        {
            // Create a new empty dictionary.
            Dictionary<AgentStateStatic, AgentStateDynamic> agents = new Dictionary<AgentStateStatic, AgentStateDynamic>();

            // We go through the list of agents located in this location and add them to the newly created dictionary.
            foreach (var agent in agentsAtLocations)
            {
                agents.Add(agent.Key, agent.Value);
            }

            // We return this dictionary.
            return agents;
        }

        /// <summary>
        /// Removes the specified agent from the list of agents in the given location, returning true on success and false on failure.
        /// </summary>
        /// <param name="agent"></param>
        public bool RemoveAgent(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            // We go through all the agents from the list of agents located in this location.
            foreach (var a in agentsAtLocations)
            {
                // We compare the name of the agent with the name of the desired agent.
                if (a.Key.GetName() == agent.Key.GetName())
                {
                    // If there is a match, remove the given agent and return true.
                    if (agentsAtLocations.Remove(a.Key)) { return true; }
                    // If we cannot find such an agent in the list, then we return false.
                    else { return false; }
                }
            }

            return false;
        }

        /// <summary>
        /// Clears the list of agents in the location.
        /// </summary>
        public void ClearLocation()
        {
            foreach (var agent in agentsAtLocations.ToArray())
            {
                agentsAtLocations.Remove(agent.Key);
            }
        }

        /// <summary>
        /// Searches for the specified agent in the list of agents located in the given location, 
        ///    returns true on success, and returns false on failure.
        /// </summary>
        /// <param name="agent"></param>
        public bool SearchAgent(AgentStateStatic agent)
        {
            // We go through all the agents in the list of agents located in this location.
            foreach (var a in agentsAtLocations)
            {
                // We compare the name of the agent with the name of the desired agent.
                if (a.Key.GetName().Equals(agent.GetName()))
                {
                    // Return true if the search is successful.
                    return true;
                }
            }

            // Otherwise, we return false.
            return false;
        }

        /// <summary>
        /// Returns the number of agents located in this location.
        /// </summary>
        public int CountAgents()
        {
            return agentsAtLocations.Count();
        }

        /// <summary>
        /// Returns whether there is evidence in the given location or not.
        /// </summary>
        public bool CheckEvidence()
        {
            return containEvidence;
        }

        /// <summary>
        /// Sets a link to the specified static part of the location.
        /// </summary>
        /// <param name="locationInfo"></param>
        public void SetLocationInfo(LocationStatic locationInfo)
        {
            this.locationInfo = locationInfo;
        }

        /// <summary>
        /// Returns the static part of the given location.
        /// </summary>
        public LocationStatic GetLocationInfo()
        {
            return locationInfo;
        }
    }
}
