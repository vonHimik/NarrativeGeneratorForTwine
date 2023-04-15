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
    public class LocationStatic: ICloneable, IEquatable<LocationStatic>
    {
        // Static properties of the location.
        private string name;
        private HashSet<LocationStatic> connectedLocations;

        // Hashcode
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

        /// <summary>
        /// Constructor with parameters of the LocationStatic, which creates a new instance of the LocationStatic based on the passed clone.
        /// </summary>
        /// <param name="clone">A LocationStatic instance that will serve as the basis for creating a new instance.</param>
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
        /// <param name="name">The name of the location to be created.</param>
        public LocationStatic (string name)
        {
            // We assign the specified name to the location.
            this.name = name;

            // We initialize the list of pointers to locations associated with this location.
            connectedLocations = new HashSet<LocationStatic>();

            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        ///  A method for cloning a location.
        /// </summary>
        /// <returns>A new instance that is a copy of the current one.</returns>
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
        /// <param name="name">New location name.</param>
        public void SetName (string name)
        {
            this.name = name;
            UpdateHashCode();
        }

        /// <summary>
        /// Method that returns the name of the location.
        /// </summary>
        /// <returns>Location name.</returns>
        public string GetName() { return name; }

        /// <summary>
        /// Checks if there is a path (connection) between this location and the specified location.
        /// </summary>
        /// <param name="checkedLocation">Checked location.</param>
        /// <returns>True if yes, otherwise false.</returns>
        public bool ConnectionChecking (LocationStatic checkedLocation)
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

        /// <summary>
        /// Returns a list of all locations that have a path from this location (they are connected).
        /// </summary>
        /// <returns>List of all locations that have a path from this location.</returns>
        public HashSet<LocationStatic> GetConnectedLocations() { return connectedLocations; }

        /// <summary>
        /// Returns a specific location from a list of all locations that have a path from that location (they are connected).
        /// </summary>
        /// <param name="index">The index of the required location.</param>
        /// <returns>Specific location from a list of all locations that have a path from that location.</returns>
        public LocationStatic GetConnectedLocationFromIndex (int index) { return connectedLocations.ElementAt(index); }

        /// <summary>
        /// Returns a random location from a list of all locations that have a path from this location (they are connected).
        /// </summary>
        /// <returns>Random location from a list of all locations that have a path from this location.</returns>
        public LocationStatic GetRandomConnectedLocation()
        {
            // Create an instance of the Random Number Generator.
            Random random = new Random();

            int index = random.Next(connectedLocations.Count());

            return connectedLocations.ElementAt(index);
        }

        /// <summary>
        /// Adds a path (connection) between this location and the specified location.
        /// </summary>
        /// <param name="location">Information about the connected location.</param>
        public void AddConnection (LocationStatic location)
        {
            connectedLocations.Add(location);
            UpdateHashCode();
        }

        /// <summary>
        /// Removes information about all paths (connections) from this location to other locations.
        /// </summary>
        public void ClearAllConnections()
        {
            connectedLocations.Clear();
            UpdateHashCode();
        }

        /// <summary>
        /// Method for comparing two LocationStatic instance.
        /// </summary>
        /// <param name="anotherLocation">Another LocationStatic instance, for comparison.</param>
        /// <returns>True if both instance are the same, false otherwise.</returns>
        public bool Equals (LocationStatic anotherLocation)
        {
            if (anotherLocation == null) { return false; }

            bool nameEquals = (name == anotherLocation.name);
            bool nameReferenceEquals = object.ReferenceEquals(name, anotherLocation.name);

            bool connectedLocationsEquals = true;
            bool connectedLocationsReferenceEquals = true;
            if (connectedLocations.Count == anotherLocation.connectedLocations.Count)
            {
                for (int i = 0; i < connectedLocations.Count; i++)
                {
                    if (connectedLocations.ElementAt(i).name != anotherLocation.connectedLocations.ElementAt(i).name)
                    {
                        connectedLocationsEquals = false;
                    }
                    if (!object.ReferenceEquals(connectedLocations.ElementAt(i), anotherLocation.connectedLocations.ElementAt(i)))
                    {
                        connectedLocationsReferenceEquals = false;
                    }
                }
            }
            else
            {
                connectedLocationsEquals = false;
                connectedLocationsReferenceEquals = false;
            }

            bool nameGlobal = nameEquals || nameReferenceEquals;
            bool connectedLocationsGlobal = connectedLocationsEquals || connectedLocationsReferenceEquals;

            bool equal = nameGlobal && connectedLocationsGlobal;

            return equal;
        }


        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

        /// <summary>
        /// Calculates and returns the hash code of this instance of the LocationStatic.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            hashcode = hashcode * 42 + name.GetHashCode();

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
