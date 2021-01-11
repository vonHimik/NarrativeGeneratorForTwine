using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class LocationStatic: ICloneable
    {
        private string name;
        private List<LocationStatic> connectedLocations;

        public LocationStatic() {}

        public LocationStatic(string name)
        {
            this.name = name;
        }

        public object Clone()
        {
            var clone = new LocationStatic();

            clone.name = name;
            clone.connectedLocations = connectedLocations;

            return clone;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }
    }
}
