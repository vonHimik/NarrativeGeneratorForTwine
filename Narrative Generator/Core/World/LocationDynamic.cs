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
    public class LocationDynamic : ICloneable, IEquatable<LocationDynamic>
    {
        // Link to the static part of the location.
        private LocationStatic locationInfo;

        // List of agents in the location.
        private Dictionary<AgentStateStatic, AgentStateDynamic> agentsAtLocations;

        private HashSet<Item> itemsInLocation = new HashSet<Item>();

        // A flag indicating whether the location contains evidence or not.
        private bool containEvidence;

        // Hashcode
        private bool hasHashCode;
        private int hashCode;

        /// <summary>
        /// Constructor method for the dynamic part of the location, without parameters.
        /// </summary>
        public LocationDynamic()
        {
            locationInfo = new LocationStatic();
            agentsAtLocations = new Dictionary<AgentStateStatic, AgentStateDynamic>();
            itemsInLocation = new HashSet<Item>();
            containEvidence = false;
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Constructor with parameters of the LocationDynamic, which creates a new instance of the LocationDynamic based on the passed clone.
        /// </summary>
        /// <param name="clone">A LocationDynamic instance that will serve as the basis for creating a new instance.</param>
        public LocationDynamic (LocationDynamic clone)
        {
            locationInfo = (LocationStatic)clone.locationInfo.Clone();
            agentsAtLocations = new Dictionary<AgentStateStatic, AgentStateDynamic>(clone.agentsAtLocations);
            containEvidence = clone.containEvidence;
            itemsInLocation = clone.itemsInLocation;
            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
        }

        /// <summary>
        /// Constructor method for the dynamic part of the location, the value of the flag about the presence of evidence is used as a parameter.
        /// </summary>
        /// <param name="containEvidence">Value of the flag about the presence of evidence.</param>
        public LocationDynamic (bool containEvidence)
        {
            locationInfo = new LocationStatic();
            agentsAtLocations = new Dictionary<AgentStateStatic, AgentStateDynamic>();
            this.containEvidence = containEvidence;
            itemsInLocation = new HashSet<Item>();
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Constructor method for the dynamic part of the location,
        ///    as a parameter using the values for the flag about the presence of evidence and a link to the static part of the location.
        /// </summary>
        /// <param name="containEvidence">Value of the flag about the presence of evidence.</param>
        /// <param name="locationInfo">Information about the created location.</param>
        public LocationDynamic (bool containEvidence, LocationStatic locationInfo)
        {
            this.locationInfo = locationInfo;
            agentsAtLocations = new Dictionary<AgentStateStatic, AgentStateDynamic>();
            this.containEvidence = containEvidence;
            itemsInLocation = new HashSet<Item>();
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Returns a clone of the dynamic part of the location that called this method.
        /// </summary>
        /// <returns>A new instance that is a copy of the current one.</returns>
        public object Clone()
        {
            // We create an empty instance of the dynamic part of the location.
            var clone = new LocationDynamic();

            clone.agentsAtLocations = new Dictionary<AgentStateStatic, AgentStateDynamic>(agentsAtLocations);

            // Copy the flag value.
            clone.containEvidence = containEvidence;

            // We return the clone.
            return clone;
        }

        /// <summary>
        /// Adds an agent to the list of agents located in this location.
        /// </summary>
        /// <param name="agent">Information about the added agent.</param>
        public void AddAgent (KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            foreach (var a in agentsAtLocations)
            {
                if (a.Key.GetName().Equals(agent.Key.GetName())) { return; }
            }

            agentsAtLocations.Add(agent.Key, agent.Value);

            UpdateHashCode();
        }

        /// <summary>
        /// Adds a set of agents to the list of agents located in this location.
        /// </summary>
        /// <param name="agents">The set of agents to add to this location.</param>
        public void AddAgents (Dictionary<AgentStateStatic, AgentStateDynamic> agents)
        {
            agentsAtLocations = agentsAtLocations.Concat(agents).ToDictionary(x => x.Key, x => x.Value);
            UpdateHashCode();
        }

        /// <summary>
        /// Returns the specified agent from the list of agents in the location.
        /// </summary>
        /// <param name="agent">Information about the required agent.</param>
        /// <returns>Required agent.</returns>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetAgent (KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
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
            foreach (var agent in agentsAtLocations) { agents.Add(agent.Key, agent.Value); }

            // We return this dictionary.
            return agents;
        }

        /// <summary>
        /// Removes the specified agent from the list of agents in the given location, returning true on success and false on failure.
        /// </summary>
        /// <param name="agent">Information about the agent to be removed.</param>
        public bool RemoveAgent (KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
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

            UpdateHashCode();

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

            UpdateHashCode();
        }

        /// <summary>
        /// Searches for the specified agent in the list of agents located in the given location, 
        ///    returns true on success, and returns false on failure.
        /// </summary>
        /// <param name="agent">Information about searched agent.</param>
        /// <returns>True on success, false on failure.</returns>
        public bool SearchAgent (AgentStateStatic agent)
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
        /// Searches for an agent in this location according to the specified name, returns with the first match.
        /// </summary>
        /// <param name="name">The name of the agent being searched for.</param>
        /// <returns>True if the agent with that name is in the location, false otherwise.</returns>
        public bool SearchAgentByName (string name)
        {
            // We go through all the agents in the list of agents located in this location.
            foreach (var a in agentsAtLocations)
            {
                // We compare the name of the agent with the name of the desired agent.
                if (a.Key.GetName() == name)
                {
                    // Return true if the search is successful.
                    return true;
                }
            }

            // Otherwise, we return false.
            return false;
        }

        /// <summary>
        /// Searches for an agent in this location according to the specified role, returns with the first match.
        /// </summary>
        /// <param name="role">The role of the agent being searched for.</param>
        /// <returns>True if the agent with that role is in the location, false otherwise.</returns>
        public bool SearchAgentByRole (AgentRole role)
        {
            // We go through all the agents in the list of agents located in this location.
            foreach (var a in agentsAtLocations)
            {
                // We compare the name of the agent with the name of the desired agent.
                if (a.Key.GetRole() == role)
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
        /// <returns>Number of agents located in this location.</returns>
        public int CountAgents() { return agentsAtLocations.Count(); }

        /// <summary>
        /// Returns whether there is evidence in the given location or not.
        /// </summary>
        /// <returns>True if the location has a evidence, otherwise not.</returns>
        public bool CheckEvidence() { return containEvidence; }

        /// <summary>
        /// Sets a link to the specified static part of this location.
        /// </summary>
        /// <param name="locationInfo">The static part of this location.</param>
        public void SetLocationInfo (LocationStatic locationInfo)
        {
            this.locationInfo = locationInfo;
            UpdateHashCode();
        }

        /// <summary>
        /// Returns the static part of the this location.
        /// </summary>
        /// <returns>The static part of this location.</returns>
        public LocationStatic GetLocationInfo() { return locationInfo; }

        /// <summary>
        /// Removes from the list of all agents located in this location whose status is False (they are dead).
        /// </summary>
        public void RemoveDiedAgents()
        {
            for (int i = 0; i < agentsAtLocations.Count; i++)
            {
                var agent = agentsAtLocations.ElementAt(i);

                if (!agent.Value.GetStatus())
                {
                    this.agentsAtLocations.Remove(agent.Key);
                    i--;
                }
            }
        }

        /// <summary>
        /// Returns the number of living agents in this location (whose status is True).
        /// </summary>
        /// <returns>The numerical value of the number of living agents in this location.</returns>
        public int CountAliveAgents()
        {
            int counter = 0;

            foreach (var agent in agentsAtLocations)
            {
                if (agent.Value.GetStatus()) { counter++; }
            }

            return counter;
        }

        /// <summary>
        /// Checks if there are agents in the given location whose role is Usual or Player.
        /// </summary>
        /// <returns>True if at least one of the agents with the specified role is in this location, false otherwise.</returns>
        public bool PlayerOrUsualIsHere()
        {
            foreach (var agent in agentsAtLocations)
            {
                if (agent.Key.GetRole().Equals(AgentRole.USUAL) || agent.Key.GetRole().Equals(AgentRole.PLAYER)) { return true; }
            }

            return false;
        }

        /// <summary>
        /// Returns the number of dead agents (whose status is False) in this location.
        /// </summary>
        /// <returns>The numerical value of the number of dead agents in this location.</returns>
        public int CountDeadAgents()
        {
            int counter = 0;

            foreach (var agent in agentsAtLocations)
            {
                if (!agent.Value.GetStatus()) { counter++; }
            }

            return counter;
        }

        /// <summary>
        /// Adds the specified item to the list of items in this location.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void AddItem (Item item)
        {
            itemsInLocation.Add(item);
        }

        /// <summary>
        /// Adds the set specified items to the list of items in this location.
        /// </summary>
        /// <param name="items">Items to add.</param>
        public void AddItems (HashSet<Item> items)
        {
            foreach (var item in items)
            {
                itemsInLocation.Add(item);
            }
        }

        /// <summary>
        /// Returns the item with the specified name if it is in this location.
        /// </summary>
        /// <param name="itemName">The name of the item being searched.</param>
        /// <returns>Item from location with matching name.</returns>
        public Item GetItemByName (string itemName)
        {
            foreach (var item in itemsInLocation)
            {
                if (item.GetItemName().Equals(itemName)) { return item; }
            }

            throw new MissingMemberException();
        }

        /// <summary>
        /// Returns items with the specified type if they are in this location.
        /// </summary>
        /// <param name="type">The type of items being searched.</param>
        /// <returns>Items from location with matching type.</returns>
        public HashSet<Item> GetItemsByType (ItemsTypes type)
        {
            HashSet<Item> items = new HashSet<Item>();

            foreach (var item in itemsInLocation)
            {
                if (item.GetItemType().Equals(type)) { items.Add(item); }
            }

            return items;
        }

        /// <summary>
        /// Returns a list of items in this location.
        /// </summary>
        /// <returns>The set of items.</returns>
        public HashSet<Item> GetItems()
        {
            return itemsInLocation;
        }

        /// <summary>
        /// Checks if this location contains an item with the specified name.
        /// </summary>
        /// <param name="itemName">The name of the item being checked.</param>
        /// <returns>True if the location contains the specified item, false otherwise.</returns>
        public bool ItemCheck (string itemName)
        {
            foreach (var item in itemsInLocation)
            {
                if (item.GetItemName().Equals(itemName)) { return true; }
            }

            return false;
        }

        /// <summary>
        /// Checks if this location contains an item with the specified type.
        /// </summary>
        /// <param name="itemType">The type of the item being checked.</param>
        /// <returns>True if the location contains the specified item, false otherwise.</returns>
        public bool ItemCheck (ItemsTypes itemType)
        {
            foreach (var item in itemsInLocation)
            {
                if (item.GetItemType().Equals(itemType)) { return true; }
            }

            return false;
        }

        /// <summary>
        /// Removes an item with the specified name from the list of items in this location (if it has one).
        /// </summary>
        /// <param name="itemName">The name of the item to be removed.</param>
        public void RemoveItem (string itemName)
        {
            for (int i = 0; i < itemsInLocation.Count; i++)
            {
                if (itemsInLocation.ElementAt(i).GetItemName().Equals(itemName)) { RemoveItem(itemsInLocation.ElementAt(i)); }
            }
        }

        /// <summary>
        /// Removes all items with the specified type from the list of items in this location.
        /// </summary>
        /// <param name="item">The type of items to be removed.</param>
        public void RemoveItem (Item item)
        {
            itemsInLocation.Remove(item);
        }

        /// <summary>
        /// Method for comparing two LocationDynamic instance.
        /// </summary>
        /// <param name="anotherLocation">Another LocationDynamic instance, for comparison.</param>
        /// <returns>True if both instance are the same, false otherwise.</returns>
        public bool Equals (LocationDynamic anotherLocation)
        {
            if (anotherLocation == null) { return false; }

            bool locationInfoEquals = locationInfo.Equals(anotherLocation.locationInfo);
            bool locationInfoReferenceEquals = object.ReferenceEquals(locationInfo, anotherLocation.locationInfo);

            bool agentsAtLocationEquals = true;
            bool agentsAtLocationReferenceEquals = true;
            if (agentsAtLocations.Count == anotherLocation.agentsAtLocations.Count)
            {
                for (int i = 0; i < agentsAtLocations.Count; i++)
                {
                    if (!agentsAtLocations.Keys.ElementAt(i).Equals(anotherLocation.agentsAtLocations.Keys.ElementAt(i)) ||
                        !agentsAtLocations.Values.ElementAt(i).Equals(anotherLocation.agentsAtLocations.Values.ElementAt(i)))
                    {
                        agentsAtLocationEquals = false;
                    }
                    if (!object.ReferenceEquals(agentsAtLocations.Keys.ElementAt(i), anotherLocation.agentsAtLocations.Keys.ElementAt(i)) ||
                        !object.ReferenceEquals(agentsAtLocations.Values.ElementAt(i), anotherLocation.agentsAtLocations.Values.ElementAt(i)))
                    {
                        agentsAtLocationReferenceEquals = false;
                    }
                }
            }
            else
            {
                agentsAtLocationEquals = false;
                agentsAtLocationReferenceEquals = false;
            }

            bool containEvidenceEquals = (containEvidence == anotherLocation.containEvidence);
            bool containEvidenceReferenceEquals = object.ReferenceEquals(containEvidence, anotherLocation.containEvidence);

            bool locationInfoGlobal = locationInfoEquals || locationInfoReferenceEquals;
            bool agentsAtLocationGlobal = agentsAtLocationEquals || agentsAtLocationReferenceEquals;
            bool containEvidenceGlobal = containEvidenceEquals || containEvidenceReferenceEquals;

            bool equal = locationInfoGlobal && agentsAtLocationGlobal && containEvidenceGlobal;

            return equal;
        }

        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

        /// <summary>
        /// Calculates and returns the hash code of this instance of the LocationDynamic.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            foreach (var agent in agentsAtLocations)
            {
                agent.Key.ClearHashCode();
                agent.Value.ClearHashCode();
                hashcode = hashcode * 42 + (agent.Key.GetHashCode() + agent.Value.GetHashCode());
            }

            hashcode = hashcode * 42 + containEvidence.GetHashCode();

            hashCode = hashcode;
            hasHashCode = true;

            return hashcode;
        }

        /// <summary>
        /// Clears the current hash code value.
        /// </summary>
        public void ClearHashCode()
        {
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Updates (refresh) the current hash code value.
        /// </summary>
        public void UpdateHashCode()
        {
            ClearHashCode();
            GetHashCode();
        }
    }
}
