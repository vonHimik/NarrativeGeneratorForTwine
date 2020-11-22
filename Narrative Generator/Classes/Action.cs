using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class Action
    {
        private string name;
        private List<String> parameters;                  // Temporary plug.
        List<Precondition> preconditions;
        List<Effect> effects;

        public void Clear()
        {
            name = null;
            parameters.Clear();
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }

        public void AddParameter(string parameter)
        {
            parameters.Add(parameter);
        }
    }
}
