using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class WorldBeliefs : ICloneable
    {
        private WorldStatic world;
        private Dictionary<LocationStatic, LocationDynamic> currentStateOfLocations;
        private Dictionary<AgentStateStatic, AgentStateDynamic> agents;               // Agents (include agents states).
        private List<Goal> goalStates;                                                // List of goal state(s). 

        public object Clone()
        {
            var clone = new WorldBeliefs();

            clone.world = (WorldStatic)world.Clone();
            clone.currentStateOfLocations = currentStateOfLocations;
            clone.agents = agents;
            clone.goalStates = goalStates;

            return clone;
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
        }

        public void AddAgent(AgentRole role, string name)
        {
            AgentStateStatic newAgentStateStatic = new AgentStateStatic();
            AgentStateDynamic newAgentStateDynamic = new AgentStateDynamic();

            newAgentStateStatic.AssignRole(role);
            newAgentStateStatic.SetName(name);

            agents.Add(newAgentStateStatic, newAgentStateDynamic);
        }

        public void AddEmptyAgent()
        {
            AgentStateStatic newAgentStateStatic = new AgentStateStatic();
            AgentStateDynamic newAgentStateDynamic = new AgentStateDynamic();
            agents.Add(newAgentStateStatic, newAgentStateDynamic);
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

        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetRandomAgent()
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
        public void OrderByInitiative()
        {
            agents = agents.OrderBy(x => x.Value.GetInitiative()).ToDictionary(x => x.Key, x => x.Value);
        }

        public void AddLocations(Dictionary<LocationStatic, LocationDynamic> locations)
        {
            locations.ToList().ForEach(x => currentStateOfLocations.Add(x.Key, x.Value));
        }

        public LocationDynamic GetLocation(LocationStatic locationKey)
        {
            return currentStateOfLocations[locationKey];
        }

        public KeyValuePair<LocationStatic, LocationDynamic> GetLocationByName(string name)
        {
            foreach (var location in currentStateOfLocations)
            {
                if (name == location.Key.GetName())
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

        public KeyValuePair<LocationStatic, LocationDynamic> GetRandomLocation()
        {
            Random random = new Random();
            List<string> locationsNames = new List<string>();

            foreach (var location in currentStateOfLocations)
            {
                locationsNames.Add(location.Key.GetName());
            }

            int index = random.Next(locationsNames.Count() - 1);
            return GetLocationByName(locationsNames[index]);
        }

        public KeyValuePair<LocationStatic, LocationDynamic> GetRandomLocation(KeyValuePair<LocationStatic, LocationDynamic> startLocation)
        {
            Random random = new Random();
            List<string> locationsNames = new List<string>();

            foreach (var location in currentStateOfLocations)
            {
                if (location.Key.GetName() != startLocation.Key.GetName())
                {
                    locationsNames.Add(location.Key.GetName());
                }
            }

            int index = random.Next(locationsNames.Count() - 1);
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
            location.Value.AddAgent(location.Key, agent);
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
            this.world = world;
        }
    }
}