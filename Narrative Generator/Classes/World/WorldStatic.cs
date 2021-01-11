using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class WorldStatic : ICloneable
    {
        private List<LocationStatic> locations; // List of locations.
        private int turn = 0;

        public object Clone()
        {
            var clone = new WorldStatic();

            clone.locations = locations;
            clone.turn = turn;

            return clone;
        }

        public void AddLocations(List<LocationStatic> locations)
        {
            for (int i = 0; i < locations.Count; i++)
            {
                AddLocation(locations[i]);
            }
        }

        public void AddLocation(LocationStatic newLocation)
        {
            locations.Add(newLocation);
        }

        public List<LocationStatic> GetLocations()
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
    }
}