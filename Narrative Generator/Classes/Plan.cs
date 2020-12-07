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

        public void AddAction(string actionName)
        {
            Action newAction = new Action(actionName, false, false, false);
            actions.Add(newAction);
        }

        public Action GetAction(int index)
        {
            return actions[index];
        }
    }
}
