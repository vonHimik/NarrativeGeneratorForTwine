using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    public class WorldStatic : ICloneable, IEquatable<WorldStatic>
    {
        private HashSet<LocationStatic> locations; // List of locations.
        private int turn;

        private bool hasHashCode;
        private int hashCode;

        public WorldStatic()
        {
            locations = new HashSet<LocationStatic>();
            turn = 0;
            hasHashCode = false;
            hashCode = 0;
        }

        public WorldStatic(WorldStatic clone)
        {
            locations = new HashSet<LocationStatic>(clone.locations);
            turn = clone.turn;
            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
        }

        public object Clone()
        {
            var clone = new WorldStatic();

            clone.locations = new HashSet<LocationStatic>(locations);
            clone.turn = turn;

            return clone;
        }

        public void AddLocations(List<LocationStatic> locations)
        {
            foreach (var location in locations)
            {
                AddLocation(location);
            }

            UpdateHashCode();
        }

        public void AddLocation(LocationStatic newLocation)
        {
            locations.Add(newLocation);
            UpdateHashCode();
        }

        public HashSet<LocationStatic> GetLocations()
        {
            return locations;
        }

        public LocationStatic GetLocation(string name)
        {
            foreach (var location in locations)
            {
                if (location.GetName() == name)
                {
                    return location;
                }
            }

            throw new MissingMemberException();
        }

        public int GetTurnNumber()
        {
            return turn;
        }

        public void IncreaseTurnNumber()
        {
            turn++;
        }

        public bool Equals(WorldStatic anotherWorld)
        {
            bool locationsReferenceEquals = object.ReferenceEquals(GetLocations(), anotherWorld.GetLocations());
            bool locationsEquals = GetLocations().Equals(anotherWorld.GetLocations());

            bool turnEquals = turn.Equals(anotherWorld.GetTurnNumber());

            bool locationsGlobal = locationsReferenceEquals || locationsEquals;

            bool equals = locationsGlobal && turnEquals;

            return equals;
        }

        /* HASHCODE SECTION */

        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            foreach (var location in locations)
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