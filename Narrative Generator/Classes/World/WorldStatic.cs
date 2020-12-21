using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class WorldStatic
    {
        private List<Location> locations;                     // List of locations.
        private int turn = 0;

        public void AddLocations(List<Location> locations)
        {
            for (int i = 0; i < locations.Count; i++)
            {
                AddLocation(locations[i]);
            }
        }

        public void AddLocation(Location newLocation)
        {
            locations.Add(newLocation);
        }

        public List<Location> GetLocations()
        {
            return locations;
        }

        public int GetTurnNumber()
        {
            return turn;
        }

        public void IncreaseTurnNumber()
        {
            turn++;
        }

        public Location SearchAgentAmongLocations(Agent agent)
        {
            for (int i = 0; i < locations.Count(); i++)
            {
                if (locations[i].SearchAgent(agent))
                {
                    return locations[i];
                }
            }

            return null;
        }
    }
}