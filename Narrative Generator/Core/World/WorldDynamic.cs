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
    [Serializable]
    public class WorldDynamic : ICloneable
    {
        private WorldStatic world;
        private Dictionary<LocationStatic, LocationDynamic> currentStateOfLocations;
        private Dictionary<AgentStateStatic, AgentStateDynamic> agents;  // Agents (include agents states).
        private HashSet<Goal> goalStates;                                // List of goal state(s).

        public WorldDynamic()
        {
            world = new WorldStatic();
            currentStateOfLocations = new Dictionary<LocationStatic, LocationDynamic>();
            agents = new Dictionary<AgentStateStatic, AgentStateDynamic>();
            goalStates = new HashSet<Goal>();
            
        }

        public object Clone()
        {
            var clone = new WorldDynamic();

            clone.world = (WorldStatic)world.Clone();

            clone.currentStateOfLocations = currentStateOfLocations.ToDictionary(entry => (LocationStatic)entry.Key.Clone(), 
                                                                                 entry => (LocationDynamic)entry.Value.Clone());

            clone.agents = agents.ToDictionary(entry => (AgentStateStatic)entry.Key.Clone(),
                                               entry => (AgentStateDynamic)entry.Value.Clone());

            /*object agentsCopy = FromBinary(ToBinary(agents));
            clone.agents = (Dictionary<AgentStateStatic, AgentStateDynamic>)agentsCopy;
            agentsCopy = null;*/

            //clone.agents = agents;

            clone.goalStates = goalStates;

            return clone;
        }

        public Byte[] ToBinary(Dictionary<AgentStateStatic, AgentStateDynamic> cloneDictionary)
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

        public object FromBinary(Byte[] buffer)
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

        public void ClearLocations()
        {
            foreach (var location in currentStateOfLocations)
            {
                location.Value.ClearLocation();
            }
        }

        public void AddAgents(Dictionary<AgentStateStatic, AgentStateDynamic> agents)
        {
            agents.ToList().ForEach(x => this.agents.Add(x.Key, x.Value));
        }

        public void AddAgent(AgentStateStatic newAgentStatic, AgentStateDynamic newAgentStateDynamic)
        {
            agents.Add(newAgentStatic, newAgentStateDynamic);
        }

        public void AddAgent(AgentRole role, bool status)
        {
            AgentStateStatic newAgentStateStatic = new AgentStateStatic();
            AgentStateDynamic newAgentStateDynamic = new AgentStateDynamic();

            newAgentStateStatic.AssignRole(role);
            newAgentStateDynamic.SetStatus(status);

            agents.Add(newAgentStateStatic, newAgentStateDynamic);

            // Очистка
            newAgentStateStatic = null;
            newAgentStateDynamic = null;
            GC.Collect();
        }

        /// <summary>
        /// Add the agent to the existing collection of agents using only the specified role and name.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="name"></param>
        public void AddAgent(AgentRole role, string name)
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

            // Очистка
            newAgentStateStatic = null;
            newAgentStateDynamic = null;
            GC.Collect();
        }

        public void AddEmptyAgent()
        {
            AgentStateStatic newAgentStateStatic = new AgentStateStatic();
            AgentStateDynamic newAgentStateDynamic = new AgentStateDynamic();
            agents.Add(newAgentStateStatic, newAgentStateDynamic);

            // Очистка
            newAgentStateStatic = null;
            newAgentStateDynamic = null;
            GC.Collect();
        }

        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetFirstAgent()
        {
            return agents.First();
        }

        public Dictionary<AgentStateStatic, AgentStateDynamic> GetAgents()
        {
            return agents;
        }

        /// <summary>
        /// Returns the first founded agent with the specified role.
        /// </summary>
        /// <param name="role"></param>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetAgentByRole(AgentRole role)
        {
            foreach (var agent in agents.Where(a => a.Key.GetRole().Equals(role)))
            {
                return agent;
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Returns the first founded agent with the specified name.
        /// </summary>
        /// <param name="name"></param>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetAgentByName(string name)
        {
            foreach (var agent in agents.Where(a => a.Key.GetName().Equals(name)))
            {
                return agent;
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// A method that returns a random agent.
        /// </summary>

        // The old method. Longer runnable than new, according to my estimates.
        /*public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetRandomAgent()
        {
            Random random = new Random();
            List<string> agentsNames = new List<string>();

            foreach (var agent in agents)
            {
                if (agent.Value.GetStatus())
                {
                    agentsNames.Add(agent.Key.GetName());
                }
            }

            int index = random.Next(agentsNames.Count() - 1);
            return GetAgentByName(agentsNames[index]);
        }*/

        // The new method, I think, is faster than the old one.
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetRandomAgent()
        {
            Random random = new Random();
            int maxNumberOfAgents = agents.Count();
            int randomIndex = random.Next(maxNumberOfAgents);
            int counter = 0;

            foreach (var agent in agents)
            {
                if (counter == randomIndex)
                {
                    return agent;
                }
                else
                {
                    counter++;
                }
            }

            throw new KeyNotFoundException();
        }

        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetRandomAgent(KeyValuePair<AgentStateStatic, AgentStateDynamic> initiator)
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

        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetRandomAgent(Dictionary<AgentStateStatic, AgentStateDynamic> initiators)
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

        public int GetNumberOfAgents()
        {
            return agents.Count();
        }

        /// <summary>
        /// Sorts the agents in the dictionary according to their initiative. CORRECT WORKING IS NOT GUARANTEED.
        /// </summary>
        public void OrderAgentsByInitiative()
        {
            agents = agents.OrderBy(x => x.Value.GetInitiative()).ToDictionary(x => x.Key, x => x.Value);
        }

        public void AddLocations(Dictionary<LocationStatic, LocationDynamic> locations)
        {
            //locations.ToList().ForEach(x => currentStateOfLocations.Add(x.Key, x.Value));

            foreach (var location in locations)
            {
                LocationStatic sPrefab = (LocationStatic)location.Key.Clone();
                LocationDynamic dPrefab = (LocationDynamic)location.Value.Clone();
                currentStateOfLocations.Add(sPrefab, dPrefab);

                // Очистка
                sPrefab = null;
                dPrefab = null;
                GC.Collect();
            }
        }

        public LocationDynamic GetLocation(LocationStatic locationKey)
        {
            return currentStateOfLocations[locationKey];
        }

        public KeyValuePair<LocationStatic, LocationDynamic> GetLocationByName(string name)
        {
            foreach (var location in currentStateOfLocations)
            {
                if (name.ToLower() == location.Key.GetName())
                {
                    return location;
                }
            }

            throw new KeyNotFoundException();
        }

        public Dictionary<LocationStatic, LocationDynamic> GetLocations()
        {
            return currentStateOfLocations;
        }

        public Dictionary<LocationStatic, LocationDynamic> CloneLocations()
        {
            Dictionary<LocationStatic, LocationDynamic> newLocations = new Dictionary<LocationStatic, LocationDynamic>();

            foreach (var x in currentStateOfLocations)
            {
                var newStatic = x.Key.Clone();
                var newDynamic = x.Value.Clone();
                newLocations.Add((LocationStatic)newStatic, (LocationDynamic)newDynamic);

                // Очистка
                newStatic = null;
                newDynamic = null;
                GC.Collect();
            }

            return newLocations;
        }

        /// <summary>
        /// A method that returns a random location.
        /// </summary>
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
        /// <param name="excludedLocation"></param>
        public KeyValuePair<LocationStatic, LocationDynamic> GetRandomLocationWithout(KeyValuePair<LocationStatic, LocationDynamic> excludedLocation)
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

        public KeyValuePair<LocationStatic, LocationDynamic> GetRandomConnectedLocation(KeyValuePair<LocationStatic, LocationDynamic> checkedLocation)
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

        public bool LocationIsEmpty(KeyValuePair<LocationStatic, LocationDynamic> location)
        {
            if (location.Value.GetAgents().Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddAgentIntoLocation(KeyValuePair<LocationStatic, LocationDynamic> location, 
                                         KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            AgentStateStatic sNewAgent = (AgentStateStatic)agent.Key.Clone();
            AgentStateDynamic dNewAgent = (AgentStateDynamic)agent.Value.Clone();
            KeyValuePair<AgentStateStatic, AgentStateDynamic> newAgent = new KeyValuePair<AgentStateStatic, AgentStateDynamic> (sNewAgent, dNewAgent);
            location.Value.AddAgent(newAgent);

            // Очистка
            sNewAgent = null;
            dNewAgent = null;
            GC.Collect();
        }

        /// <summary>
        /// Returns the static part (name) of the location where the searched agent is located.
        /// </summary>
        /// <param name="agent"></param>
        public LocationStatic SearchAgentAmongLocations(AgentStateStatic agent)
        {
            foreach (var location in currentStateOfLocations)
            {
                if (location.Value.SearchAgent(agent))
                {
                    return location.Key;
                }
            }

            return null;
        }

        public WorldStatic GetStaticWorldPart()
        {
            return world;
        }

        public void SetStaticWorldPart(WorldStatic world)
        {
            this.world = (WorldStatic)world.Clone();
        }
    }
}