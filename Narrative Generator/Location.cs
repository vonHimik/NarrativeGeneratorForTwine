using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class Location
    {
        public string name;
        List<Agent> containedAgents;
        List<Location> connectedLocations;

        public Location (string name)
        {
            this.name = name;
        }
    }
}
