using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// This class stores static (rarely changed) properties of a location and describes methods for interacting with them.
    /// </summary>
    public class LocationStatic: ICloneable
    {
        // Static properties of the location.
        private string name;
        private List<LocationStatic> connectedLocations;
        public int id;

        /// <summary>
        /// Method constructor for the static part of the location.
        /// </summary>
        public LocationStatic()
        {
            // We initialize the variable storing the name of the location.
            name = "";

            // We initialize the list of pointers to locations associated with this location.
            connectedLocations = new List<LocationStatic>();

            // We generate a random identification number for the location.
            Random rand = new Random();
            id = rand.Next(1000);
        }

        /// <summary>
        /// Method-constructor of the static part of the location, using the location name as a parameter.
        /// </summary>
        /// <param name="name"></param>
        public LocationStatic(string name)
        {
            // We assign the specified name to the location.
            this.name = name;

            // We initialize the list of pointers to locations associated with this location.
            connectedLocations = new List<LocationStatic>();

            // We generate a random identification number for the location.
            Random rand = new Random();
            id = rand.Next(1000);
        }

        /// <summary>
        /// A method for cloning a location.
        /// </summary>
        public object Clone()
        {
            // Preparing an empty clone.
            var clone = new LocationStatic();

            // Copy the data of the instance of the static part of the location into it.
            clone.name = name;
            clone.connectedLocations = connectedLocations;

            // We return the clone.
            return clone;
        }

        /// <summary>
        /// The method that sets the name of the location.
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Method that returns the name of the location.
        /// </summary>
        public string GetName()
        {
            return name;
        }

        public bool ConnectionChecking(LocationStatic checkedLocation)
        {
            if (connectedLocations != null)
            {
                foreach (var location in connectedLocations)
                {
                    if (location.GetName().Equals(checkedLocation.GetName())) { return true; }
                }
            }

            return false;
        }

        public List<LocationStatic> GetConnectedLocations()
        {
            return connectedLocations;
        }

        public void AddConnection(LocationStatic location)
        {
            connectedLocations.Add(location);
        }
    }
}
