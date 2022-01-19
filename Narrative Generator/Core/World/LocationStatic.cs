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
    [Serializable]
    public class LocationStatic: ICloneable/*, IEquatable<LocationStatic>*/
    {
        // Static properties of the location.
        private string name;
        private HashSet<LocationStatic> connectedLocations;
        public int id; // Just for test.

        private bool hasHashCode;
        private int hashCode;

        /// <summary>
        /// Method constructor for the static part of the location.
        /// </summary>
        public LocationStatic()
        {
            // We initialize the variable storing the name of the location.
            name = "";

            // We initialize the list of pointers to locations associated with this location.
            connectedLocations = new HashSet<LocationStatic>();

            hasHashCode = false;
            hashCode = 0;
        }

        public LocationStatic (LocationStatic clone)
        {
            name = clone.name;
            connectedLocations = new HashSet<LocationStatic>(clone.connectedLocations);
            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
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
            connectedLocations = new HashSet<LocationStatic>();

            hasHashCode = false;
            hashCode = 0;
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
            clone.connectedLocations = new HashSet<LocationStatic>(connectedLocations);

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
            UpdateHashCode();
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

        public HashSet<LocationStatic> GetConnectedLocations()
        {
            return connectedLocations;
        }

        public LocationStatic GetConnectedLocationsFromIndex(int index)
        {
            return connectedLocations.ElementAt(index);
        }

        public void AddConnection(LocationStatic location)
        {
            connectedLocations.Add(location);
            UpdateHashCode();
        }

        public void ClearAllConnections()
        {
            connectedLocations.Clear();
            UpdateHashCode();
        }

        public bool Equals(LocationStatic other)
        {
            if (hashCode == other.hashCode) { return true; }
            else { return false; }
        }


        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            hashcode = hashcode * 42 + name.GetHashCode();
            foreach (var location in connectedLocations)
            {
                //location.ClearHashCode();
                hashcode = hashcode * 42 + location.GetHashCode();
            }

            hashCode = hashcode;
            hasHashCode = true;

            return hashcode;
        }

        public void ClearHashCode()
        {
            hasHashCode = false;
            hashCode = 0;
        }

        public void UpdateHashCode()
        {
            ClearHashCode();
            GetHashCode();
        }
    }
}
