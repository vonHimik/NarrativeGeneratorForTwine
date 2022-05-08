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
        private Setting setting;

        private bool hasHashCode;
        private int hashCode;

        public WorldStatic()
        {
            locations = new HashSet<LocationStatic>();
            turn = 0;
            setting = Setting.DefaultDemo;
            hasHashCode = false;
            hashCode = 0;
        }

        public WorldStatic(WorldStatic clone)
        {
            locations = new HashSet<LocationStatic>(clone.locations);
            turn = clone.turn;
            setting = clone.setting;
            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
        }

        public object Clone()
        {
            var clone = new WorldStatic();

            clone.locations = new HashSet<LocationStatic>(locations);
            clone.turn = turn;
            clone.setting = setting;

            return clone;
        }

        public void AddLocations (List<LocationStatic> locations)
        {
            foreach (var location in locations) { AddLocation(location); }

            UpdateHashCode();
        }

        public void AddLocation(LocationStatic newLocation)
        {
            locations.Add(newLocation);
            UpdateHashCode();
        }

        public HashSet<LocationStatic> GetLocations() { return locations; }

        public LocationStatic GetLocation (string name)
        {
            foreach (var location in locations)
            {
                if (location.GetName() == name) { return location; }
            }

            throw new MissingMemberException();
        }

        public int GetTurnNumber() { return turn; }

        public void IncreaseTurnNumber() { turn++; }

        public void SetSetting (Setting setting) { this.setting = setting; }

        public Setting GetSetting() { return setting; }

        public bool Equals(WorldStatic anotherWorld)
        {
            if (anotherWorld == null) { return false; }

            bool locationsEquals = true;
            bool locationsReferenceEquals = true;

            for (int i = 0; i < locations.Count; i++)
            {
                if (!locations.ElementAt(i).Equals(anotherWorld.locations.ElementAt(i)))
                {
                    locationsEquals = false;
                }
                if (!object.ReferenceEquals(locations.ElementAt(i), anotherWorld.locations.ElementAt(i)))
                {
                    locationsReferenceEquals = false;
                }
            }

            bool turnEquals = turn.Equals(anotherWorld.GetTurnNumber());
            bool turnReferenceEquals = object.ReferenceEquals(turn, anotherWorld.turn);

            bool settingEquals = setting.Equals(anotherWorld.setting);
            bool settingReferenceEquals = object.ReferenceEquals(setting, anotherWorld.setting);

            bool locationsGlobal = locationsReferenceEquals || locationsEquals;
            bool turnGlobal = turnEquals || turnReferenceEquals;
            bool settingGlobal = settingEquals || settingReferenceEquals;

            bool equal = locationsGlobal /*&& turnGlobal*/ && settingGlobal;

            return equal;
        }

        /* HASHCODE SECTION */

        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            foreach (var location in locations)
            {
                hashcode = hashcode * 42 + location.GetHashCode();
            }

            hashcode = hashcode * 42 + setting.GetHashCode();

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