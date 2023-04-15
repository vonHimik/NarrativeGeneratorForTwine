using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that implements one agent's beliefs about another agent.
    /// </summary>
    public class BeliefsAboutAgent : ICloneable, IEquatable<BeliefsAboutAgent>
    {
        // Information about another agent.
        /// <summary>
        /// Immutable information about the agent.
        /// </summary>
        private AgentStateStatic info;
        /// <summary>
        /// The role of the agent.
        /// </summary>
        private AgentRole role;
        /// <summary>
        /// Agent status.
        /// </summary>
        private bool isAlive;
        /// <summary>
        /// Estimated location of the agent.
        /// </summary>
        private LocationStatic inLocation;
        /// <summary>
        /// The assumption that the other agent is angry.
        /// </summary>
        private AgentAngryAt angryAt;

        /// <summary>
        /// Constructor without parameters.
        /// </summary>
        public BeliefsAboutAgent()
        {
            info = new AgentStateStatic();
            role = AgentRole.USUAL;
            isAlive = true;
            inLocation = new LocationStatic();
            angryAt = new AgentAngryAt();
        }

        /// <summary>
        /// Constructor with parameters of the BeliefsAboutAgent, which creates a new instance of the BeliefsAboutAgent based on the passed clone.
        /// </summary>
        /// <param name="clone">A BeliefsAboutAgent instance that will serve as the basis for creating a new instance.</param>
        public BeliefsAboutAgent (BeliefsAboutAgent clone)
        {
            info = (AgentStateStatic)clone.info.Clone();
            role = clone.role;
            isAlive = clone.isAlive;
            inLocation = (LocationStatic)clone.inLocation.Clone();
            angryAt = (AgentAngryAt)clone.angryAt.Clone();
        }

        /// <summary>
        /// A parameterized constructor that receives as input all the necessary information to create a new full instance of beliefs about another agent.
        /// </summary>
        /// <param name="info">Information about agent.</param>
        /// <param name="role">The role of the agent.</param>
        /// <param name="isAlive">The status of the agent.</param>
        /// <param name="inLocation">Agent location.</param>
        /// <param name="angryAt">Information about whether the agent is angry.</param>
        public BeliefsAboutAgent (AgentStateStatic info, AgentRole role, bool isAlive, LocationStatic inLocation, AgentAngryAt angryAt)
        {
            this.info = info;
            this.role = role;
            this.isAlive = isAlive;
            this.inLocation = inLocation;
            this.angryAt = angryAt;
        }

        /// <summary>
        /// Method for cloning an BeliefsAboutAgent instance.
        /// </summary>
        /// <returns>A new instance that is a copy of the current one.</returns>
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

        /// <summary>
        /// Returns information about the agent.
        /// </summary>
        /// <returns>Information about the agent.</returns>
        public AgentStateStatic GetInfo() { return info; }

        /// <summary>
        /// Sets agent information.
        /// </summary>
        /// <param name="info">Information about the agent.</param>
        public void SetInfo (AgentStateStatic info) { this.info = info; }

        /// <summary>
        /// Checks the status of an agent.
        /// </summary>
        /// <returns>True if alive, false otherwise.</returns>
        public bool CheckStatus() { return isAlive; }

        /// <summary>
        /// Switches the agent's status to False (dead).
        /// </summary>
        public void Dead() { isAlive = false; }

        /// <summary>
        /// Returns the role of the agent.
        /// </summary>
        /// <returns>Role of the agent.</returns>
        public AgentRole GetRole() { return role; }

        /// <summary>
        /// Assigns the specified role to the agent.
        /// </summary>
        /// <param name="role">Role of the agent.</param>
        public void AssignRole (AgentRole role)
        {
            this.role = role;
        }

        /// <summary>
        /// Returns information about the location where the agent is located.
        /// </summary>
        /// <returns>Information about the location where the agent is located.</returns>
        public LocationStatic GetLocation() { return inLocation; }

        /// <summary>
        /// Sets the location where the agent is located.
        /// </summary>
        /// <param name="location">Information about the location where the agent is located.</param>
        public void SetLocation (LocationStatic location) { inLocation = location; }

        /// <summary>
        /// Clears information about the location where the agent is located.
        /// </summary>
        public void ClearLocation() { inLocation = null; }

        /// <summary>
        /// Returns information about whether the agent is angry, and if so, at whom.
        /// </summary>
        /// <returns>Information about whether the agent is angry, and if so, at whom.</returns>
        public AgentAngryAt GetObjectOfAngry() { return angryAt; }

        /// <summary>
        /// Sets information about whether the agent is angry, and if so, at whom.
        /// </summary>
        /// <param name="objectOfAngry">Information about whether the agent is angry, and if so, at whom.</param>
        public void SetObjectOfAngry(AgentAngryAt objectOfAngry) { angryAt = objectOfAngry; }

        /// <summary>
        /// Method for comparing two BeliefsAboutAgent instance.
        /// </summary>
        /// <param name="anotherBeliefsAboutAgent">Another BeliefsAboutAgent instance, for comparison.</param>
        /// <returns>True if both instance are the same, false otherwise.</returns>
        public bool Equals(BeliefsAboutAgent anotherBeliefsAboutAgent)
        {
            if (anotherBeliefsAboutAgent == null) { return false; }

            bool infoEquals = info.Equals(anotherBeliefsAboutAgent.info);
            bool infoReferenceEquals = object.ReferenceEquals(info, anotherBeliefsAboutAgent.info);

            bool roleEquals = (role == anotherBeliefsAboutAgent.role);
            bool roleReferenceEquals = object.ReferenceEquals(role, anotherBeliefsAboutAgent.role);

            bool statusEquals = (isAlive == anotherBeliefsAboutAgent.isAlive);
            bool statusReferenceEquals = object.ReferenceEquals(isAlive, anotherBeliefsAboutAgent.isAlive);

            bool inLocationEquals = inLocation.Equals(anotherBeliefsAboutAgent.inLocation);
            bool inLocationReferenceEquals = object.ReferenceEquals(inLocation, anotherBeliefsAboutAgent.inLocation);

            bool angryAtEquals = angryAt.Equals(anotherBeliefsAboutAgent.angryAt);
            bool angryAtReferenceEquals = object.ReferenceEquals(angryAt, anotherBeliefsAboutAgent.angryAt);

            bool infoGlobal = infoEquals || infoReferenceEquals;
            bool roleGlobal = roleEquals || roleReferenceEquals;
            bool statusGlobal = statusEquals || statusReferenceEquals;
            bool inLocationGlobal = inLocationEquals || inLocationReferenceEquals;
            bool angryAtGlobal = angryAtEquals || angryAtReferenceEquals;

            bool equal = infoGlobal && roleGlobal && statusGlobal && inLocationGlobal && angryAtGlobal;

            return equal;
        }
    }

    /// <summary>
    /// A class that implements the agent's beliefs about the surrounding storyworld (environment).
    /// </summary>
    public class WorldContext : ICloneable, IEquatable<WorldContext>
    {
        // The main parameters of the belief about the storyworld (environment).
        private LocationStatic myLocation;
        private HashSet<AgentStateStatic> anotherAgentsInMyLocation;
        private HashSet<BeliefsAboutAgent> agentsInWorld;
        private HashSet<LocationStatic> locationsInWorld;

        /// <summary>
        /// Constructor without parameters.
        /// </summary>
        public WorldContext()
        {
            myLocation = new LocationStatic();
            anotherAgentsInMyLocation = new HashSet<AgentStateStatic>();
            agentsInWorld = new HashSet<BeliefsAboutAgent>();
            locationsInWorld = new HashSet<LocationStatic>();
        }

        /// <summary>
        /// Constructor with parameters of the WorldContext, which creates a new instance of the WorldContext based on the passed clone.
        /// </summary>
        /// <param name="clone">A WorldContext instance that will serve as the basis for creating a new instance.</param>
        public WorldContext (WorldContext clone)
        {
            if (clone.myLocation != null) { myLocation = (LocationStatic)clone.myLocation.Clone(); }
            else { myLocation = new LocationStatic(); }
            anotherAgentsInMyLocation = new HashSet<AgentStateStatic>(clone.anotherAgentsInMyLocation);
            agentsInWorld = new HashSet<BeliefsAboutAgent>(clone.agentsInWorld);
            locationsInWorld = new HashSet<LocationStatic>(clone.locationsInWorld);
        }

        /// <summary>
        /// Method for cloning an WorldContext instance.
        /// </summary>
        /// <returns>A new instance that is a copy of the current one.</returns>
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

        /// <summary>
        /// Set the location where the agent thinks he is.
        /// </summary>
        /// <param name="location">Location where the agent thinks he is.</param>
        public void SetMyLocation (LocationStatic location) { myLocation = location; }

        /// <summary>
        /// Returns the location where the agent thinks he is.
        /// </summary>
        /// <returns>Location where the agent thinks he is</returns>
        public LocationStatic GetMyLocation() { return myLocation; }

        /// <summary>
        /// Clears the agent's belief about where Location he is.
        /// </summary>
        public void ClearMyLocation() { myLocation = null; }

        /// <summary>
        /// Adds a belief about the presence of another agent in the location where this agent is located.
        /// </summary>
        /// <param name="agent">Information about the added agent.</param>
        public void AddAgentInMyLocation (AgentStateStatic agent) { anotherAgentsInMyLocation.Add(agent); }

        /// <summary>
        /// Adds a beliefs about the presence of another agents in the location where this agent is located.
        /// </summary>
        /// <param name="agents">Information about the added agents.</param>
        public void AddAgentsInMyLocation (HashSet<AgentStateStatic> agents)
        {
            foreach (var agent in agents)
            {
                anotherAgentsInMyLocation.Add(agent);
            }
        }

        /// <summary>
        /// Adds a beliefs about the presence of another agents in the location where this agent is located.
        /// </summary>
        /// <param name="agents">Information (ready-made list) about the added agents.</param>
        public void SetListAnotherAgentsInMyLocation (HashSet<AgentStateStatic> agents) { anotherAgentsInMyLocation = agents; }

        /// <summary>
        /// Returns information about an agent that this agent believes is in the same location as him.
        /// </summary>
        /// <param name="index">The index of the searched agent.</param>
        /// <returns>Information about searched agent.</returns>
        public AgentStateStatic GetSomeAgentInMyLocation (int index) { return anotherAgentsInMyLocation.ElementAt(index); }

        /// <summary>
        /// Returns information about all agents that this agent believes are in the same location as him.
        /// </summary>
        /// <returns>List of agents.</returns>
        public HashSet<AgentStateStatic> GetAgentsInMyLocation() { return anotherAgentsInMyLocation; }

        /// <summary>
        /// Add (new) beliefs about another agent to this agent.
        /// </summary>
        /// <param name="agent">Added information about the agent.</param>
        public void AddAgentInBeliefs (BeliefsAboutAgent agent) { agentsInWorld.Add(agent); }

        /// <summary>
        /// Add (new) beliefs about another agent to this agent.
        /// </summary>
        /// <param name="agent">Information about added agent.</param>
        /// <param name="role">Role of added agent.</param>
        public void AddAgentInBeliefs (KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, AgentRole role)
        {
            BeliefsAboutAgent newAgent = new BeliefsAboutAgent(agent.Key, role, agent.Value.GetStatus(), agent.Value.GetMyLocation(), 
                                                                  agent.Value.GetObjectOfAngryComponent());
            agentsInWorld.Add(newAgent);
        }

        /// <summary>
        /// Iterates through the provided list of agents and adds information about them to that agent's beliefs.
        /// </summary>
        /// <param name="agents">List of beliefs about agents.</param>
        public void AddAgentsInWorld (HashSet<BeliefsAboutAgent> agents)
        {
            foreach (var agent in agents)
            {
                AddAgentInBeliefs(agent);
            }
        }

        /// <summary>
        /// Sets new beliefs about agents passed to this agent.
        /// </summary>
        /// <param name="agents">List of beliefs about agents.</param>
        public void SetAgentsInWorld (HashSet<BeliefsAboutAgent> agents) { agentsInWorld = agents; }

        /// <summary>
        /// Searches this agent's beliefs for information about an agent with the specified role and returns the first match.
        /// </summary>
        /// <param name="role">Role of the searched agent.</param>
        /// <returns>Information about an agent with the specified role.</returns>
        public KeyValuePair<AgentStateStatic, AgentRole> GetAgentByRole (AgentRole role)
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

        /// <summary>
        /// Searches this agent's beliefs for information about the agent with the specified name and returns the first match.
        /// </summary>
        /// <param name="name">Name of the searched agent</param>
        /// <returns>Information about an agent with the specified name.</returns>
        public BeliefsAboutAgent GetAgentByName (string name)
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

        /// <summary>
        /// Returns this agent's set of beliefs about other agents.
        /// </summary>
        /// <returns>Set of beliefs about other agents.</returns>
        public HashSet<BeliefsAboutAgent> GetAgentsInWorld() { return agentsInWorld; }

        /// <summary>
        /// Returns the agent's beliefs about a randomly selected agent.
        /// </summary>
        /// <returns>Information about an agent.</returns>
        public BeliefsAboutAgent GetRandomAgent()
        {
            Random random = new Random();

            int randomIndex = random.Next(agentsInWorld.Count);

            return agentsInWorld.ElementAt(randomIndex);
        }

        /// <summary>
        /// Adds to this agent the belief that the specified location exists.
        /// </summary>
        /// <param name="location">Target location.</param>
        public void AddLocationInWorld (LocationStatic location) { locationsInWorld.Add(location); }

        /// <summary>
        /// Iterates through the provided list of locations and adds information about them to that agent's beliefs.
        /// </summary>
        /// <param name="locations">List of beliefs about locations.</param>
        public void AddLocationsInWorld (HashSet<LocationStatic> locations)
        {
            foreach (var location in locations)
            {
                locationsInWorld.Add(location);
            }
        }

        /// <summary>
        /// Sets new beliefs about locations passed to this agent.
        /// </summary>
        /// <param name="locations">List of beliefs about locations.</param>
        public void SetLocationsInWorld (HashSet<LocationStatic> locations) { locationsInWorld = locations; }

        /// <summary>
        /// Sets new beliefs about locations passed to this agent.
        /// </summary>
        /// <param name="locations">List of beliefs about locations.</param>
        public void SetLocationsInWorld (Dictionary<LocationStatic, LocationDynamic> locations)
        {
            HashSet<LocationStatic> locationsList = new HashSet<LocationStatic>();

            foreach (var loc in locations)
            {
                locationsList.Add(loc.Key);
            }

            locationsInWorld = locationsList;
        }

        /// <summary>
        /// Returns this agent's set of beliefs about locations.
        /// </summary>
        /// <returns>Set of beliefs about locations.</returns>
        public HashSet<LocationStatic> GetLocationsInWorld() { return locationsInWorld; }

        /// <summary>
        /// Searches for the specified agent among the locations and returns the location in which he is, according to the belief of this agent.
        /// </summary>
        /// <param name="agent">Information about the agent being searched.</param>
        /// <returns>Information about the desired location.</returns>
        public LocationStatic SearchAgentAmongLocations (AgentStateStatic agent)
        {
            foreach (var a in agentsInWorld)
            {
                if (a.GetInfo() == agent) { return a.GetLocation(); }
            }

            return null;
        }

        /// <summary>
        /// Searches this agent's beliefs for information about the location with the specified name and returns the first match.
        /// </summary>
        /// <param name="name">Name of searched location.</param>
        /// <returns>Information about searched location.</returns>
        public LocationStatic GetLocationByName (string name)
        {
            foreach (var location in locationsInWorld)
            {
                if (location.GetName() == name) { return location; }
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// A method that returns a random location, excluding the specified one.
        /// </summary>
        /// <param name="excludedLocation">The location to be excluded from the search.</param>
        /// <returns>Information about a random location.</returns>
        public LocationStatic GetRandomLocationWithout (LocationStatic excludedLocation)
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

        /// <summary>
        /// Method for comparing two WantToEntrap instance.
        /// </summary>
        /// <param name="anotherWorldContext">Another WorldContext instance, for comparison.</param>
        /// <returns>True if both instance are the same, false otherwise.</returns>
        public bool Equals (WorldContext anotherWorldContext)
        {
            if (anotherWorldContext == null) { return false; }

            bool myLocationEquals = myLocation.Equals(anotherWorldContext.myLocation);
            bool myLocationReferenceEquals = object.ReferenceEquals(myLocation, anotherWorldContext.myLocation);

            bool anotherAgentsInMyLocationEquals = true;
            bool anotherAgentsInMyLocationReferenceEquals = true;

            for (int i = 0; i <anotherAgentsInMyLocation.Count; i++)
            {
                if (!anotherAgentsInMyLocation.ElementAt(i).Equals(anotherWorldContext.anotherAgentsInMyLocation.ElementAt(i)))
                {
                    anotherAgentsInMyLocationEquals = false;
                }
                if (!object.ReferenceEquals(anotherAgentsInMyLocation.ElementAt(i), anotherWorldContext.anotherAgentsInMyLocation.ElementAt(i)))
                {
                    anotherAgentsInMyLocationReferenceEquals = false;
                }
            }

            bool agentsInWorldEquals = true;
            bool agentsInWorldReferenceEquals = true;

            for (int i = 0; i < agentsInWorld.Count; i++)
            {
                if (!agentsInWorld.ElementAt(i).Equals(anotherWorldContext.agentsInWorld.ElementAt(i)))
                {
                    agentsInWorldEquals = false;
                }
                if (!object.ReferenceEquals(agentsInWorld.ElementAt(i), anotherWorldContext.agentsInWorld.ElementAt(i)))
                {
                    agentsInWorldReferenceEquals = false;
                }
            }

            bool locationsInWorldEquals = true;
            bool locationsInWorldReferenceEquals = true;

            for (int i = 0; i < locationsInWorld.Count; i++)
            {
                if (!locationsInWorld.ElementAt(i).Equals(anotherWorldContext.locationsInWorld.ElementAt(i)))
                {
                    locationsInWorldEquals = false;
                }
                if (!object.ReferenceEquals(locationsInWorld.ElementAt(i), anotherWorldContext.locationsInWorld.ElementAt(i)))
                {
                    locationsInWorldReferenceEquals = false;
                }
            }

            bool myLocationGlobal = myLocationEquals || myLocationReferenceEquals;
            bool anotherAgentsInMyLocationGlobal = anotherAgentsInMyLocationEquals || anotherAgentsInMyLocationReferenceEquals;
            bool agentsInWorldGlobal = agentsInWorldEquals || agentsInWorldReferenceEquals;
            bool locationsInWorldGlobal = locationsInWorldEquals || locationsInWorldReferenceEquals;

            bool equal = myLocationGlobal && anotherAgentsInMyLocationGlobal && agentsInWorldGlobal && locationsInWorldGlobal;

            return equal;
        }
    }
}
