using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class BeliefsAboutAgent : ICloneable
    {
        private AgentStateStatic info;
        private AgentRole role;
        private bool isAlive;
        private LocationStatic inLocation;
        private AgentAngryAt angryAt;

        public BeliefsAboutAgent()
        {
            info = new AgentStateStatic();
            role = AgentRole.USUAL;
            isAlive = true;
            inLocation = new LocationStatic();
            angryAt = new AgentAngryAt();
        }

        public BeliefsAboutAgent (BeliefsAboutAgent clone)
        {
            info = (AgentStateStatic)clone.info.Clone();
            role = clone.role;
            isAlive = clone.isAlive;
            inLocation = (LocationStatic)clone.inLocation.Clone();
            angryAt = (AgentAngryAt)clone.angryAt.Clone();
        }

        public BeliefsAboutAgent(AgentStateStatic info, AgentRole role, bool isAlive, LocationStatic inLocation, AgentAngryAt angryAt)
        {
            this.info = info;
            this.role = role;
            this.isAlive = isAlive;
            this.inLocation = inLocation;
            this.angryAt = angryAt;
        }

        public object Clone()
        {
            var clone = new BeliefsAboutAgent();

            clone.info = new AgentStateStatic(info);
            clone.role = role;
            clone.isAlive = isAlive;
            if (inLocation != null) { clone.inLocation = new LocationStatic(inLocation); }
            clone.angryAt = new AgentAngryAt(angryAt);

            return clone;
        }

        public AgentStateStatic GetInfo()
        {
            return info;
        }

        public void SetInfo(AgentStateStatic info)
        {
            this.info = info;
        }

        public bool CheckStatus()
        {
            return isAlive;
        }

        public void Dead()
        {
            isAlive = false;
        }

        public AgentRole GetRole()
        {
            return role;
        }

        public void AssignRole(AgentRole role)
        {
            this.role = role;
        }

        public LocationStatic GetLocation()
        {
            return inLocation;
        }

        public void SetLocation(LocationStatic location)
        {
            inLocation = location;
        }

        public void ClearLocation()
        {
            inLocation = null;
        }

        public AgentAngryAt GetObjectOfAngry()
        {
            return angryAt;
        }

        public void SetObjectOfAngry(AgentAngryAt objectOfAngry)
        {
            angryAt = objectOfAngry;
        }
    }

    public class WorldContext : ICloneable
    {
        private LocationStatic myLocation;
        private HashSet<AgentStateStatic> anotherAgentsInMyLocation;
        private HashSet<BeliefsAboutAgent> agentsInWorld;
        private HashSet<LocationStatic> locationsInWorld;

        public WorldContext()
        {
            myLocation = new LocationStatic();
            anotherAgentsInMyLocation = new HashSet<AgentStateStatic>();
            agentsInWorld = new HashSet<BeliefsAboutAgent>();
            locationsInWorld = new HashSet<LocationStatic>();
        }

        public WorldContext (WorldContext clone)
        {
            if (clone.myLocation != null) { myLocation = (LocationStatic)clone.myLocation.Clone(); }
            else { myLocation = new LocationStatic(); }
            anotherAgentsInMyLocation = new HashSet<AgentStateStatic>(clone.anotherAgentsInMyLocation);
            agentsInWorld = new HashSet<BeliefsAboutAgent>(clone.agentsInWorld);
            locationsInWorld = new HashSet<LocationStatic>(clone.locationsInWorld);
        }

        public object Clone()
        {
            var clone = new WorldContext();

            if (myLocation != null) { clone.myLocation = new LocationStatic(myLocation); }
            clone.anotherAgentsInMyLocation = new HashSet<AgentStateStatic>(
                anotherAgentsInMyLocation.Select(entry => new AgentStateStatic(entry)).ToHashSet());
            clone.agentsInWorld = new HashSet<BeliefsAboutAgent>(agentsInWorld.Select(entry => new BeliefsAboutAgent(entry)).ToHashSet());
            clone.locationsInWorld = new HashSet<LocationStatic>(locationsInWorld.Select(entry => new LocationStatic(entry)).ToHashSet());

            return clone;
        }

        public void SetMyLocation(LocationStatic location)
        {
            myLocation = location;
        }

        public LocationStatic GetMyLocation()
        {
            return myLocation;
        }

        public void ClearMyLocation()
        {
            myLocation = null;
        }

        public void AddAgentInMyLocation(AgentStateStatic agent)
        {
            anotherAgentsInMyLocation.Add(agent);
        }

        public void AddAgentsInMyLocation(HashSet<AgentStateStatic> agents)
        {
            foreach (var agent in agents)
            {
                anotherAgentsInMyLocation.Add(agent);
            }
        }

        public void SetListAnotherAgentsInMyLocation(HashSet<AgentStateStatic> agents)
        {
            anotherAgentsInMyLocation = agents;
        }

        public AgentStateStatic GetSomeAgentInMyLocation(int index)
        {
            return anotherAgentsInMyLocation.ElementAt(index);
        }

        public HashSet<AgentStateStatic> GetAgentsInMyLocation()
        {
            return anotherAgentsInMyLocation;
        }

        public void AddAgentInBeliefs(BeliefsAboutAgent agent)
        {
            agentsInWorld.Add(agent);
        }

        public void AddAgentInBeliefs(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, AgentRole role)
        {
            BeliefsAboutAgent newAgent = new BeliefsAboutAgent(agent.Key, role, agent.Value.GetStatus(), agent.Value.GetMyLocation(), 
                                                                  agent.Value.GetObjectOfAngry());
            agentsInWorld.Add(newAgent);
        }

        public void AddAgentsInWorld(HashSet<BeliefsAboutAgent> agents)
        {
            foreach (var agent in agents)
            {
                AddAgentInBeliefs(agent);
            }
        }

        public void SetAgentsInWorld(HashSet<BeliefsAboutAgent> agents)
        {
            agentsInWorld = agents;
        }

        public KeyValuePair<AgentStateStatic, AgentRole> GetAgentByRole(AgentRole role)
        {
            foreach (var agent in agentsInWorld)
            {
                if (agent.GetRole() == role)
                {
                    return new KeyValuePair<AgentStateStatic, AgentRole>(agent.GetInfo(), agent.GetRole());
                }
            }

            throw new KeyNotFoundException();
        }

        public BeliefsAboutAgent GetAgentByName(string name)
        {
            foreach (var agent in agentsInWorld)
            {
                if (agent.GetInfo().GetName() == name)
                {
                    return agent;
                }
            }

            throw new KeyNotFoundException();
        }

        public HashSet<BeliefsAboutAgent> GetAgentsInWorld()
        {
            return agentsInWorld;
        }

        public BeliefsAboutAgent GetRandomAgent()
        {
            Random random = new Random();

            int randomIndex = random.Next(agentsInWorld.Count);

            return agentsInWorld.ElementAt(randomIndex);
        }

        public void AddLocationInWorld(LocationStatic location)
        {
            locationsInWorld.Add(location);
        }

        public void AddAgentsInWorld(HashSet<LocationStatic> locations)
        {
            foreach (var location in locations)
            {
                locationsInWorld.Add(location);
            }
        }

        public void SetLocationsInWorld(HashSet<LocationStatic> locations)
        {
            locationsInWorld = locations;
        }

        public void SetLocationsInWorld(Dictionary<LocationStatic, LocationDynamic> locations)
        {
            HashSet<LocationStatic> locationsList = new HashSet<LocationStatic>();

            foreach (var loc in locations)
            {
                locationsList.Add(loc.Key);
            }

            locationsInWorld = locationsList;
        }

        public HashSet<LocationStatic> GetLocationsInWorld()
        {
            return locationsInWorld;
        }

        public LocationStatic SearchAgentAmongLocations(AgentStateStatic agent)
        {
            foreach (var a in agentsInWorld)
            {
                if (a.GetInfo() == agent)
                {
                    return a.GetLocation();
                }
            }

            return null;
        }

        public LocationStatic GetLocationByName(string name)
        {
            foreach (var location in locationsInWorld)
            {
                if (location.GetName() == name)
                {
                    return location;
                }
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// A method that returns a random location, excluding the specified one.
        /// </summary>
        /// <param name="excludedLocation"></param>
        public LocationStatic GetRandomLocationWithout(LocationStatic excludedLocation)
        {
            // Create an instance of the Random Number Generator.
            Random random = new Random();

            // Create a list in which we write down the names of all locations.
            List<string> locationsNames = new List<string>();

            // In the loop we go through all the locations.
            foreach (var location in locationsInWorld)
            {
                // If the name of the location in question does not match the name of the excluded location.
                if (location.GetName() != excludedLocation.GetName())
                {
                    // By adding their names to the previously created list.
                    locationsNames.Add(location.GetName());
                }
            }

            // We get the index - a random number, in the range from 0 to the number of locations minus one (due to the indexing of arrays from 0).
            int index = random.Next(locationsNames.Count());

            // We get the name of a randomly selected location (by a randomly generated index), search for it by name and return it.
            return GetLocationByName(locationsNames[index]);
        }
    }
}
