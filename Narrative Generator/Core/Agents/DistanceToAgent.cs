using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A structure representing the distance to the agent (from the one to whom it belongs).
    /// </summary>
    public struct DistanceToAgent
    {
        /// <summary>
        /// Numeric expression of distance (number of transitions between locations).
        /// </summary>
        public int Distance { get; set; }
        /// <summary>
        /// The agent to which the distance is calculated.
        /// </summary>
        public AgentStateStatic AgentInfo { get; set; }
    }

    /// <summary>
    /// A structure representing a list of distances to several agents, with the possibility of sorting in ascending order.
    /// </summary>
    public struct ListOfDistanceToAgent
    {
        /// <summary>
        /// List of objects representing distance.
        /// </summary>
        HashSet<DistanceToAgent> list;
        /// <summary>
        /// Initialization marker.
        /// </summary>
        bool init;

        /// <summary>
        /// Initializing the list with a starting value.
        /// </summary>
        public void Initialization () { init = true;  list = new HashSet<DistanceToAgent>(); }

        /// <summary>
        /// Method for sorting the list in ascending order of distance.
        /// </summary>
        public void Sort ()
        {
            HashSet<DistanceToAgent> tempList = new HashSet<DistanceToAgent>();

            int counter = 0;

            while (tempList.Count != list.Count)
            {
                foreach (var entry in list)
                {
                    if (entry.Distance == counter)
                    {
                        tempList.Add(entry);
                    }
                }

                counter++;
            }

            list = tempList;
            tempList = null;
        }

        /// <summary>
        /// A method that creates a new instance of the distance object.
        /// </summary>
        /// <param name="agentInfo">Information about the target agent.</param>
        /// <param name="distance">Distance to the agent.</param>
        /// <returns>Instance of the distance object</returns>
        public DistanceToAgent Create (AgentStateStatic agentInfo, int distance)
        {
            DistanceToAgent newDistance = new DistanceToAgent();
            newDistance.AgentInfo = agentInfo;
            newDistance.Distance = distance;
            return newDistance;
        }

        /// <summary>
        /// A method that adds an existing distance object to the list.
        /// </summary>
        /// <param name="newObject">A method that adds a distance object to the list.</param>
        public void Add (DistanceToAgent newObject)
        {
            if (!init) { Initialization(); }
            list.Add(newObject);
            Sort();
        }

        /// <summary>
        /// A method that creates a new distance object and adds it to the list.
        /// </summary>
        /// <param name="agentInfo">Information about the target agent.</param>
        /// <param name="distance">Distance to the agent.</param>
        public void Add (AgentStateStatic agentInfo, int distance)
        {
            DistanceToAgent newDistance = Create(agentInfo, distance);
            Add(newDistance);
        }

        /// <summary>
        /// Method that returns information about the nearest agent.
        /// </summary>
        /// <returns>The first distance object in the list.</returns>
        public DistanceToAgent GetFist ()
        {
            return list.First();
        }
    }
}
