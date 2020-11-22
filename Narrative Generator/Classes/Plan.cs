using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class Plan
    {
        private List<Action> actions;

        public void AddAction(Action action)
        {
            actions.Add(action);
        }

        public Action GetAction(int index)
        {
            return actions[index];
        }
    }
}
