using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that implements a dynamic (frequently changed) part of the state of the storyworld that stores information about the current state of locations and agents.
    /// </summary>
    [Serializable]
    public class WorldDynamic : ICloneable, IEquatable<WorldDynamic>
    {
        /// <summary>
        /// An immutable part of information about the world
        /// </summary>
        private WorldStatic world;

        /// <summary>
        /// A set of locations in the world, information about them and their contents
        /// </summary>
        private Dictionary<LocationStatic, LocationDynamic> currentStateOfLocations;

        /// <summary>
        /// Agents in the world, immutable and up-to-date information
        /// </summary>
        private Dictionary<AgentStateStatic, AgentStateDynamic> agents;

        // Plot variables / TO DO: put it in a separate file and make a mechanism for reading from there
        // Dragon Age
        /// <summary>
        /// Information about reaching the story event in the story (progress point) - Help Mages.
        /// </summary>
        public bool helpMages;
        /// <summary>
        /// Information about reaching the story event in the story (progress point) - Help Templars.
        /// </summary>
        public bool helpTemplars;
        /// <summary>
        /// Information about reaching the story event in the story (progress point) - Help Elfs.
        /// </summary>
        public bool helpElfs;
        /// <summary>
        /// Information about reaching the story event in the story (progress point) - Help Werewolves.
        /// </summary>
        public bool helpWerewolves;
        /// <summary>
        /// Information about reaching the story event in the story (progress point) - Help Prince Belen.
        /// </summary>
        public bool helpPrineBelen;
        /// <summary>
        /// Information about reaching the story event in the story (progress point) - Help Lord Harrowmont.
        /// </summary>
        public bool helpLordHarrowmont;

        // Hashcode
        private bool hasHashCode;
        private int hashCode;

        /// <summary>
        /// Constructor without parameters.
        /// </summary>
        public WorldDynamic()
        {
            world = new WorldStatic();
            currentStateOfLocations = new Dictionary<LocationStatic, LocationDynamic>();
            agents = new Dictionary<AgentStateStatic, AgentStateDynamic>();

            helpMages = false;
            helpTemplars = false;
            helpElfs = false;
            helpWerewolves = false;
            helpPrineBelen = false;
            helpLordHarrowmont = false;

            hasHashCode = false;
            hashCode = 0;           
        }

        /// <summary>
        /// Constructor with parameters of the WorldDynamic, which creates a new instance of the WorldDynamic based on the passed clone.
        /// </summary>
        /// <param name="clone">A WorldDynamic instance that will serve as the basis for creating a new instance.</param>
        public WorldDynamic (WorldDynamic clone)
        {
            world = (WorldStatic)clone.world.Clone();

            currentStateOfLocations = new Dictionary<LocationStatic, LocationDynamic>(
                                               clone.currentStateOfLocations.ToDictionary(entry => (LocationStatic)entry.Key.Clone(),
                                                                                          entry => (LocationDynamic)entry.Value.Clone()));

            agents = new Dictionary<AgentStateStatic, AgentStateDynamic>(clone.agents.ToDictionary(entry => (AgentStateStatic)entry.Key.Clone(),
                                                                                                   entry => (AgentStateDynamic)entry.Value.Clone()));

            helpMages = clone.helpMages;
            helpTemplars = clone.helpTemplars;
            helpElfs = clone.helpElfs;
            helpWerewolves = clone.helpWerewolves;
            helpPrineBelen = clone.helpPrineBelen;
            helpLordHarrowmont = clone.helpLordHarrowmont;

            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
        }

        /// <summary>
        /// Method for cloning an WorldDynamic instance.
        /// </summary>
        /// <returns>A new instance that is a copy of the current one.</returns>
        public object Clone()
        {
            var clone = new WorldDynamic();
            clone.world = new WorldStatic(world);

            clone.agents = new Dictionary<AgentStateStatic, AgentStateDynamic>(agents.ToDictionary(entry => new AgentStateStatic(entry.Key),
                                                                                                   entry => new AgentStateDynamic(entry.Value)));

            clone.currentStateOfLocations = new Dictionary<LocationStatic, LocationDynamic>(
                                               currentStateOfLocations.ToDictionary(entry => new LocationStatic(entry.Key),
                                                                                    entry => new LocationDynamic(entry.Value)));

            foreach (var agent in clone.agents)
            {
                foreach (var location in clone.currentStateOfLocations)
                {
                    foreach (var anotherAgent in location.Value.GetAgents())
                    {
                        if (agent.Key.GetName() == anotherAgent.Key.GetName() && agent.Key.GetRole() == anotherAgent.Key.GetRole())
                        {
                            location.Value.RemoveAgent(anotherAgent);
                            location.Value.AddAgent(agent);
                        }
                    }
                }
            }

            clone.helpMages = helpMages;
            clone.helpTemplars = helpTemplars;
            clone.helpElfs = helpElfs;
            clone.helpWerewolves = helpWerewolves;
            clone.helpPrineBelen = helpPrineBelen;
            clone.helpLordHarrowmont = helpLordHarrowmont;

            return clone;
        }

        /// <summary>
        /// Method for serializing the list of agents when cloning, converting them into an array of bytes.
        /// </summary>
        /// <param name="cloneDictionary">Cloned agent list.</param>
        /// <returns>Array of bytes.</returns>
        public Byte[] ToBinary (Dictionary<AgentStateStatic, AgentStateDynamic> cloneDictionary)
        {
            MemoryStream memoryStream = null;
            Byte[] byteArray = null;

            try
            {
                BinaryFormatter serializer = new BinaryFormatter();
                memoryStream = new MemoryStream();
                serializer.Serialize(memoryStream, cloneDictionary);
                memoryStream.Seek(0, SeekOrigin.Begin);
                byteArray = memoryStream.ToArray();
            }
            catch (Exception unexpected)
            {
                Trace.Fail(unexpected.Message);
                throw;
            }
            finally
            {
                if (memoryStream != null) { memoryStream.Close(); }
            }

            return byteArray;
        }

        /// <summary>
        /// Method for deserializing binary objects.
        /// </summary>
        /// <param name="buffer">Array of bytes.</param>
        /// <returns>Deserialized object.</returns>
        public object FromBinary (Byte[] buffer)
        {
            MemoryStream memoryStream = null;
            object deserializedObject = null;

            try
            {
                BinaryFormatter serializer = new BinaryFormatter();
                memoryStream = new MemoryStream();
                memoryStream.Write(buffer, 0, buffer.Length);
                memoryStream.Position = 0;
                deserializedObject = serializer.Deserialize(memoryStream);
            }
            finally
            {
                buffer = null;
                if (memoryStream != null) { memoryStream.Close(); }
            }

            return deserializedObject;
        }

        /// <summary>
        /// Iterates through all instances of locations in the list of locations and clears them of stored information about the agents located in them.
        /// </summary>
        public void ClearLocations()
        {
            foreach (var location in currentStateOfLocations)
            {
                location.Value.ClearLocation();
            }

            UpdateHashCode();
        }

        /// <summary>
        /// Adds to the list of agents those agents that have been passed.
        /// </summary>
        /// <param name="agents">List of agents to add.</param>
        public void AddAgents (Dictionary<AgentStateStatic, AgentStateDynamic> agents)
        {
            agents.ToList().ForEach(x => this.agents.Add(x.Key, x.Value));
        }

        /// <summary>
        /// Adds the passed agent to the list of agents.
        /// </summary>
        /// <param name="newAgentStatic">The static part of the state of the agent being added.</param>
        /// <param name="newAgentStateDynamic">The dynamic part of the state of the agent being added.</param>
        public void AddAgent (AgentStateStatic newAgentStatic, AgentStateDynamic newAgentStateDynamic)
        {
            agents.Add(newAgentStatic, newAgentStateDynamic);
            UpdateHashCode();
        }

        /// <summary>
        /// Adds the specified agent to the specified location.
        /// </summary>
        /// <param name="agent">The agent to add.</param>
        /// <param name="location">The location to add the agent.</param>
        public void AddAgent (KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, KeyValuePair<LocationStatic, LocationDynamic> location)
        {
            agents.Add(agent.Key, agent.Value);
            location.Value.AddAgent(agent);
            UpdateHashCode();
        }

        /// <summary>
        /// Adding an agent, basic information about it is specified in separate parameters.
        /// </summary>
        /// <param name="role">The role of the added agent.</param>
        /// <param name="status">The status (alive or dead) of the agent being added.</param>
        /// <param name="name">The name of the agent to add.</param>
        public void AddAgent (AgentRole role, bool status, string name)
        {
            AgentStateStatic newAgentStateStatic = new AgentStateStatic();
            AgentStateDynamic newAgentStateDynamic = new AgentStateDynamic();

            newAgentStateStatic.AssignRole(role);
            newAgentStateStatic.SetName(name);
            newAgentStateDynamic.SetStatus(status);

            agents.Add(newAgentStateStatic, newAgentStateDynamic);

            UpdateHashCode();
        }

        /// <summary>
        /// Add an agent without specifying its name.
        /// </summary>
        /// <param name="role">The role of the added agent.</param>
        /// <param name="status">The status (alive or dead) of the agent being added.</param>
        public void AddAgent (AgentRole role, bool status)
        {
            AgentStateStatic newAgentStateStatic = new AgentStateStatic();
            AgentStateDynamic newAgentStateDynamic = new AgentStateDynamic();

            newAgentStateStatic.AssignRole(role);
            newAgentStateDynamic.SetStatus(status);

            agents.Add(newAgentStateStatic, newAgentStateDynamic);

            UpdateHashCode();
        }

        /// <summary>
        /// Add the agent to the existing collection of agents using only the specified role and name.
        /// </summary>
        /// <param name="role">The role of the added agent.</param>
        /// <param name="name">The name of the agent to add.</param>
        public void AddAgent (AgentRole role, string name)
        {
            // Create empty instances of the static and dynamic parts of the agent.
            AgentStateStatic newAgentStateStatic = new AgentStateStatic();
            AgentStateDynamic newAgentStateDynamic = new AgentStateDynamic();

            // Assign the role and name of the static part.
            newAgentStateStatic.AssignRole(role);
            newAgentStateStatic.SetName(name);

            // We give the dynamic part a link to the static part.
            newAgentStateDynamic.SetAgentInfo(newAgentStateStatic);

            // We combine both parts into one and add to the collection.
            agents.Add(newAgentStateStatic, newAgentStateDynamic);

            UpdateHashCode();
        }

        /// <summary>
        /// Adds an "empty" agent, without passing any information about it.
        /// </summary>
        public void AddEmptyAgent()
        {
            AgentStateStatic newAgentStateStatic = new AgentStateStatic();
            AgentStateDynamic newAgentStateDynamic = new AgentStateDynamic();
            agents.Add(newAgentStateStatic, newAgentStateDynamic);

            UpdateHashCode();
        }

        /// <summary>
        /// Returns the first agent from the list of agents.
        /// </summary>
        /// <returns>Required agent.</returns>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetFirstAgent() { return agents.First(); }

        /// <summary>
        /// Returns a list of all agents.
        /// </summary>
        /// <returns>List of agents.</returns>
        public Dictionary<AgentStateStatic, AgentStateDynamic> GetAgents() { return agents; }

        /// <summary>
        /// Returns the first founded agent with the specified role.
        /// </summary>
        /// <param name="role">The role of the required agent.</param>
        /// <returns>Required agent.</returns>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetAgentByRole (AgentRole role)
        {
            foreach (var agent in agents.Where(a => a.Key.GetRole().Equals(role))) { return agent; }
            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Returns the first founded agent with the specified name.
        /// </summary>
        /// <param name="name">The name of the required agent.</param>
        /// <returns>Required agent.</returns>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetAgentByName (string name)
        {
            foreach (var agent in agents.Where(a => a.Key.GetName().Equals(name))) { return agent; }
            throw new KeyNotFoundException();
        }

        /// <summary>
        ///  A method that returns a random agent. (faster)
        /// </summary>
        /// <returns>Random agent.</returns>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetRandomAgent()
        {
            Random random = new Random();
            int maxNumberOfAgents = agents.Count();
            int randomIndex = random.Next(maxNumberOfAgents);
            int counter = 0;

            foreach (var agent in agents)
            {
                if (counter == randomIndex) { return agent; }
                else { counter++; }
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// A method that returns a random agent from the list of agents, except for the agent that initiates the call to this method.
        /// </summary>
        /// <param name="initiator">Agent that initiates the call to this method.</param>
        /// <returns>Random agent.</returns>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetRandomAgent (KeyValuePair<AgentStateStatic, AgentStateDynamic> initiator)
        {
            Random random = new Random();
            List<string> agentsNames = new List<string>();

            foreach (var agent in agents)
            {
                if (agent.Value.GetStatus() && agent.Key.GetName() != initiator.Key.GetName())
                {
                    agentsNames.Add(agent.Key.GetName());
                }
            }

            int index = random.Next(agentsNames.Count() - 1);
            return GetAgentByName(agentsNames[index]);
        }

        /// <summary>
        /// A method that returns a random agent from the list of agents in the location with the agent that will call this method, except for himself.
        /// </summary>
        /// <param name="initiator">Agent that initiates the call to this method.</param>
        /// <returns>Random agent.</returns>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetRandomAgentInMyLocation (KeyValuePair<AgentStateStatic, AgentStateDynamic> initiator)
        {
            Random random = new Random();
            List<string> agentsNames = new List<string>();

            foreach (var agent in GetLocationByName(initiator.Value.GetBeliefs().GetMyLocation().GetName()).Value.GetAgents())
            {
                if (agent.Value.GetStatus() && agent.Key.GetName() != initiator.Key.GetName())
                {
                    agentsNames.Add(agent.Key.GetName());
                }
            }

            int index = random.Next(agentsNames.Count() - 1);
            return GetAgentByName(agentsNames[index]);
        }

        /// <summary>
        /// A method that returns a random agent from the list of agents, except for the agents that initiates the call to this method.
        /// </summary>
        /// <param name="initiators">Agents that initiates the call to this method.</param>
        /// <returns>Random agent.</returns>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetRandomAgent (Dictionary<AgentStateStatic, AgentStateDynamic> initiators)
        {
            Random random = new Random();
            List<string> agentsNames = new List<string>();

            foreach (var agent in agents)
            {
                foreach (var initiator in initiators)
                {
                    if (agent.Value.GetStatus() && agent.Key.GetName() != initiator.Key.GetName())
                    {
                        agentsNames.Add(agent.Key.GetName());
                    }
                }
            }

            int index = random.Next(agentsNames.Count() - 1);
            return GetAgentByName(agentsNames[index]);
        }

        /// <summary>
        /// Returns the agent with the specified index.
        /// </summary>
        /// <param name="index">The index of the required agent.</param>
        /// <returns>Required agent.</returns>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetAgentByIndex (int index) { return agents.ElementAt(index); }

        /// <summary>
        /// Returns the index of the specified agent in the list of agents.
        /// </summary>
        /// <param name="agent">Information about the agent whose index is necessary to find out.</param>
        /// <returns>Numeric index value of the specified agent.</returns>
        public int GetIndexOfAgent (KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            int index = -1;

            foreach (var a in agents)
            {
                index++;
                if (a.GetHashCode() == agent.GetHashCode()) { return index; }
            }

            index = -1;
            return index;
        }

        /// <summary>
        /// Returns the number of agents in the list of agents.
        /// </summary>
        /// <returns>Numeric value for the number of agents in the agent list.</returns>
        public int GetNumberOfAgents() { return agents.Count(); }

        /// <summary>
        /// Sorts the agents in the dictionary according to their initiative. CORRECT WORKING IS NOT GUARANTEED.
        /// </summary>
        public void OrderAgentsByInitiative()
        {
            agents = agents.OrderBy(x => x.Value.GetInitiative()).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Adds the specified locations to the list of locations.
        /// </summary>
        /// <param name="locations">List of locations to add.</param>
        public void AddLocations (Dictionary<LocationStatic, LocationDynamic> locations)
        {
            foreach (var location in locations)
            {
                LocationStatic sPrefab = (LocationStatic)location.Key.Clone();
                LocationDynamic dPrefab = (LocationDynamic)location.Value.Clone();
                currentStateOfLocations.Add(sPrefab, dPrefab);

                // Clear
                sPrefab = null;
                dPrefab = null;
                GC.Collect();
            }

            UpdateHashCode();
        }

        /// <summary>
        /// Returns a location from the list of locations if the key (the static part of the location state) matches.
        /// </summary>
        /// <param name="locationKey">Static part of required location.</param>
        /// <returns>Required location.</returns>
        public KeyValuePair<LocationStatic, LocationDynamic> GetLocation (LocationStatic locationKey)
        {
            foreach (var location in currentStateOfLocations)
            {
                if (location.Key.Equals(locationKey)) { return location; }
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Returns a location from the list of locations if the specified name matches the name of the location.
        /// </summary>
        /// <param name="name">Name of required location.</param>
        /// <returns>Required location.</returns>
        public KeyValuePair<LocationStatic, LocationDynamic> GetLocationByName (string name)
        {
            foreach (var location in currentStateOfLocations)
            {
                if (name.ToLower() == location.Key.GetName().ToLower()) { return location; }
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Returns a location from the list of locations according to the specified index.
        /// </summary>
        /// <param name="index">Index of required location.</param>
        /// <returns>Required location.</returns>
        public KeyValuePair<LocationStatic, LocationDynamic> GetLocationByIndex (int index) { return currentStateOfLocations.ElementAt(index); }

        /// <summary>
        /// Returns a list of all locations.
        /// </summary>
        /// <returns>List of locations.</returns>
        public Dictionary<LocationStatic, LocationDynamic> GetLocations() { return currentStateOfLocations; }

        /// <summary>
        /// Creates a copy of the current location list instance.
        /// </summary>
        /// <returns>List of locations.</returns>
        public Dictionary<LocationStatic, LocationDynamic> CloneLocations()
        {
            Dictionary<LocationStatic, LocationDynamic> newLocations = new Dictionary<LocationStatic, LocationDynamic>();

            foreach (var x in currentStateOfLocations)
            {
                var newStatic = x.Key.Clone();
                var newDynamic = x.Value.Clone();
                newLocations.Add((LocationStatic)newStatic, (LocationDynamic)newDynamic);

                // Clear
                newStatic = null;
                newDynamic = null;
                GC.Collect();
            }

            return newLocations;
        }

        /// <summary>
        /// A method that returns a random location.
        /// </summary>
        /// <returns>Random location.</returns>
        public KeyValuePair<LocationStatic, LocationDynamic> GetRandomLocation()
        {
            // Create an instance of the Random Number Generator.
            Random random = new Random();

            // Create a list in which we write down the names of all locations.
            List<string> locationsNames = new List<string>();

            // In the loop we go through all the locations.
            foreach (var location in currentStateOfLocations)
            {
                // By adding their names to the previously created list.
                locationsNames.Add(location.Key.GetName());
            }

            // We get the index - a random number, in the range from 0 to the number of locations minus one (due to the indexing of arrays from 0).
            int index = random.Next(locationsNames.Count());

            // We get the name of a randomly selected location (by a randomly generated index), search for it by name and return it.
            return GetLocationByName(locationsNames[index]);
        }

        /// <summary>
        /// A method that returns a random location, excluding the specified one.
        /// </summary>
        /// <param name="excludedLocation">Excluded location/</param>
        /// <returns>Random location.</returns>
        public KeyValuePair<LocationStatic, LocationDynamic> GetRandomLocationWithout (KeyValuePair<LocationStatic, LocationDynamic> excludedLocation)
        {
            // Create an instance of the Random Number Generator.
            Random random = new Random();

            // Create a list in which we write down the names of all locations.
            List<string> locationsNames = new List<string>();

            // In the loop we go through all the locations.
            foreach (var location in currentStateOfLocations)
            {
                // If the name of the location in question does not match the name of the excluded location.
                if (location.Key.GetName() != excludedLocation.Key.GetName())
                {
                    // By adding their names to the previously created list.
                    locationsNames.Add(location.Key.GetName());
                }
            }

            // We get the index - a random number, in the range from 0 to the number of locations minus one (due to the indexing of arrays from 0).
            int index = random.Next(locationsNames.Count());

            // We get the name of a randomly selected location (by a randomly generated index), search for it by name and return it.
            return GetLocationByName(locationsNames[index]);
        }

        /// <summary>
        /// Returns a random location where there are no agents.
        /// </summary>
        /// <returns>Random location without agents.</returns>
        public KeyValuePair<LocationStatic, LocationDynamic> GetRandomEmptyLocation()
        {
            Random random = new Random();
            List<string> locationsNames = new List<string>();

            foreach (var location in currentStateOfLocations)
            {
                if (LocationIsEmpty(location))
                {
                    locationsNames.Add(location.Key.GetName());
                }
            }

            int index = random.Next(locationsNames.Count() - 1);
            return GetLocationByName(locationsNames[index]);
        }

        /// <summary>
        /// Returns a random location that has a path (connection) to the specified location.
        /// </summary>
        /// <param name="checkedLocation">Information about the checked location.</param>
        /// <returns>Random location.</returns>
        public KeyValuePair<LocationStatic, LocationDynamic> GetRandomConnectedLocation (KeyValuePair<LocationStatic, LocationDynamic> checkedLocation)
        {
            Random rand = new Random();
            int randomIndex = rand.Next(0, checkedLocation.Key.GetConnectedLocations().Count);

            if (checkedLocation.Key.GetConnectedLocations() != null)
            {
                HashSet<LocationStatic> connectedLocationList = checkedLocation.Key.GetConnectedLocations();

                return GetLocationByName(connectedLocationList.ElementAt(randomIndex).GetName());
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Checks if there are no agents in the specified location.
        /// </summary>
        /// <param name="location">Information about the checked location.</param>
        /// <returns>True if there are no agents in the checked location, false otherwise.</returns>
        public bool LocationIsEmpty (KeyValuePair<LocationStatic, LocationDynamic> location)
        {
            if (location.Value.GetAgents().Count() == 0) { return true; }
            else { return false; }
        }

        /// <summary>
        /// Adds the specified agent to the specified location.
        /// </summary>
        /// <param name="location">Information about the agent to be added.</param>
        /// <param name="agent">The location where need to add an agent.</param>
        public void AddAgentIntoLocation (KeyValuePair<LocationStatic, LocationDynamic> location, 
                                          KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            AgentStateStatic sNewAgent = (AgentStateStatic)agent.Key.Clone();
            AgentStateDynamic dNewAgent = (AgentStateDynamic)agent.Value.Clone();
            KeyValuePair<AgentStateStatic, AgentStateDynamic> newAgent = new KeyValuePair<AgentStateStatic, AgentStateDynamic> (sNewAgent, dNewAgent);
            location.Value.AddAgent(newAgent);

            UpdateHashCode();

            // Clear
            sNewAgent = null;
            dNewAgent = null;
            GC.Collect();
        }

        /// <summary>
        /// Returns the static part (name) of the location where the searched agent is located.
        /// </summary>
        /// <param name="agent">Information about the searched agent.</param>
        /// <returns>Required location.</returns>
        public LocationStatic SearchAgentAmongLocations (AgentStateStatic agent)
        {
            foreach (var location in currentStateOfLocations)
            {
                if (location.Value.SearchAgent(agent)) { return location.Key; }
            }

            return null;
        }

        /// <summary>
        /// Searches for the location where the agent with the specified name is located and returns information about it, up to the first match.
        /// </summary>
        /// <param name="name">Name of the searched agent.</param>
        /// <returns>Required location.</returns>
        public LocationStatic SearchAgentAmongLocationsByName (string name)
        {
            foreach (var location in currentStateOfLocations)
            {
                if (location.Value.SearchAgentByName(name)) { return location.Key; }
            }

            return null;
        }

        /// <summary>
        /// Searches for the location where the agent with the specified role is located and returns information about it, up to the first match.
        /// </summary>
        /// <param name="role">Role of the searched agent.</param>
        /// <returns>Required location.</returns>
        public LocationStatic SearchAgentAmongLocationsByRole (AgentRole role)
        {
            foreach (var location in currentStateOfLocations)
            {
                if (location.Value.SearchAgentByRole(role)) { return location.Key; }
            }

            return null;
        }

        /// <summary>
        /// Adds a component with a static part of the state of this storyworld.
        /// </summary>
        /// <param name="world">Component with a static part of the state of this storyworld.</param>
        public void SetStaticWorldPart (WorldStatic world)
        {
            this.world = (WorldStatic)world.Clone();
            UpdateHashCode();
        }

        public void SecondlyGetNearestAgent (ref ListOfDistanceToAgent list, LocationStatic startingLocation, int counter, ref HashSet<LocationStatic> locationsList)
        {
            foreach (var location in currentStateOfLocations)
            {
                if (location.Key.Equals(startingLocation))
                {
                    locationsList.Add(location.Key);

                    foreach (var agent in location.Value.GetAgents())
                    {
                        if ((agent.Key.GetRole().Equals(AgentRole.USUAL) || agent.Key.GetRole().Equals(AgentRole.PLAYER)) && agent.Value.GetStatus())
                        {
                            list.Add(agent.Key, counter);
                        }
                    }

                    foreach (var neighboringLocation in location.Key.GetConnectedLocations())
                    {
                        if (!locationsList.Contains(neighboringLocation))
                        {
                            SecondlyGetNearestAgent(ref list, neighboringLocation, counter++, ref locationsList);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method that returns the closest agent, relative to the location where it was called.
        /// </summary>
        /// <param name="startingLocation">Location where the search starts</param>
        /// <param name="locationsList">List of visited locations</param>
        /// <param name="counter">Distance counter</param>
        /// <returns></returns>
        public DistanceToAgent GetNearestAgentTo (LocationStatic startingLocation, HashSet<LocationStatic> locationsList = null, int counter = 0)
        {
            ListOfDistanceToAgent listOfAgents = new ListOfDistanceToAgent();

            if (locationsList == null) { locationsList = new HashSet<LocationStatic>(); }

            foreach (var location in currentStateOfLocations)
            {
                if (location.Key.Equals(startingLocation))
                {
                    locationsList.Add(location.Key);

                    foreach (var agent in location.Value.GetAgents())
                    {
                        if ((agent.Key.GetRole().Equals(AgentRole.USUAL) || agent.Key.GetRole().Equals(AgentRole.PLAYER)) && agent.Value.GetStatus())
                        {
                            listOfAgents.Add(agent.Key, counter);
                            return listOfAgents.GetFist();
                        }
                    }

                    foreach (var neighboringLocation in location.Key.GetConnectedLocations())
                    {
                        if (!locationsList.Contains(neighboringLocation))
                        {
                            SecondlyGetNearestAgent(ref listOfAgents, neighboringLocation, counter++, ref locationsList);
                        }
                    }

                    return listOfAgents.GetFist();
                }
            }

            // This is where we get if there are no agents in the location.
            return listOfAgents.GetFist();
        }

        /// <summary>
        /// Returns a component with the static part of the state of this storyworld.
        /// </summary>
        /// <returns>Component with a static part of the state of this storyworld.</returns>
        public WorldStatic GetStaticWorldPart() { return world; }

        /// <summary>
        /// Method for comparing two WorldDynamic instance.
        /// </summary>
        /// <param name="anotherWorld">Another WorldDynamic instance, for comparison.</param>
        /// <returns>True if both instance are the same, false otherwise.</returns>
        public bool Equals (WorldDynamic anotherWorld)
        {
            if (anotherWorld == null) { return false; }

            bool worldStateEquals = world.Equals(anotherWorld.GetStaticWorldPart());
            bool worldStateReferenceEquals = object.ReferenceEquals(GetStaticWorldPart(), anotherWorld.GetStaticWorldPart());

            bool locationsEquals = true;
            bool locationsReferenceEquals = true;

            for (int i = 0; i < currentStateOfLocations.Count; i++)
            {
                if (!currentStateOfLocations.Keys.ElementAt(i).Equals(anotherWorld.currentStateOfLocations.Keys.ElementAt(i))
                    || !currentStateOfLocations.Values.ElementAt(i).Equals(anotherWorld.currentStateOfLocations.Values.ElementAt(i)))
                {
                    locationsEquals = false;
                }
                if (!object.ReferenceEquals(currentStateOfLocations.Keys.ElementAt(i), anotherWorld.currentStateOfLocations.Keys.ElementAt(i))
                    || !object.ReferenceEquals(currentStateOfLocations.Values.ElementAt(i), anotherWorld.currentStateOfLocations.Values.ElementAt(i)))
                {
                    locationsReferenceEquals = false;
                }
            }

            bool agentsReferenceEquals = true;
            bool agentsEquals = true;
            if (agents.Count == anotherWorld.agents.Count)
            {
                for (int i = 0; i < agents.Count; i++)
                {
                    if (!agents.Keys.ElementAt(i).Equals(anotherWorld.agents.Keys.ElementAt(i))
                        || !agents.Values.ElementAt(i).Equals(anotherWorld.agents.Values.ElementAt(i)))
                    {
                        agentsEquals = false;
                    }
                    if (!object.ReferenceEquals(agents.Keys.ElementAt(i), anotherWorld.agents.Keys.ElementAt(i))
                        || !object.ReferenceEquals(agents.Values.ElementAt(i), anotherWorld.agents.Values.ElementAt(i)))
                    {
                        agentsReferenceEquals = false;
                    }
                }
            }
            else
            {
                agentsEquals = false;
                agentsReferenceEquals = false;
            }

            bool helpMagesEquals = helpMages.Equals(anotherWorld.helpMages);
            bool helpTemplarsEquals = helpTemplars.Equals(anotherWorld.helpTemplars);
            bool helpElfsEquals = helpElfs.Equals(anotherWorld.helpElfs);
            bool helpWerewolvesEquals = helpWerewolves.Equals(anotherWorld.helpWerewolves);
            bool helpPrinceBelenEquals = helpPrineBelen.Equals(anotherWorld.helpPrineBelen);
            bool helpLordHarrowmontEquals = helpLordHarrowmont.Equals(anotherWorld.helpLordHarrowmont);

            bool worldStateGlobal = worldStateReferenceEquals || worldStateEquals;
            bool locationsGlobal = locationsReferenceEquals || locationsEquals;
            bool agentsGlobal = agentsReferenceEquals || agentsEquals;

            bool equals = worldStateGlobal && locationsGlobal && agentsGlobal && helpMagesEquals && helpTemplarsEquals && helpElfsEquals && helpWerewolvesEquals
                && helpPrinceBelenEquals && helpLordHarrowmontEquals;

            return equals;
        }

        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

        /// <summary>
        /// Calculates and returns the hash code of this instance of the WorldDynamic.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            foreach (var csol in currentStateOfLocations)
            {
                csol.Value.ClearHashCode();
                hashcode = hashcode * 42 + csol.Value.GetHashCode() + csol.Key.GetHashCode();
            }

            hashcode = hashcode + helpMages.GetHashCode();
            hashcode = hashcode + helpTemplars.GetHashCode();
            hashcode = hashcode + helpElfs.GetHashCode();
            hashcode = hashcode + helpWerewolves.GetHashCode();
            hashcode = hashcode + helpPrineBelen.GetHashCode();
            hashcode = hashcode + helpLordHarrowmont.GetHashCode();

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