using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class Edge
    {
        private StoryNode upperNode;
        private StoryNode lowerNode;
        private PlanAction action;

        public void SetUpperNode(StoryNode node)
        {
            upperNode = node;
        }

        public StoryNode GetUpperNode()
        {
            return upperNode;
        }

        public void SetLowerNode(StoryNode node)
        {
            lowerNode = node;
        }

        public StoryNode GetLowerNode()
        {
            return lowerNode;
        }

        public void SetAction(PlanAction action)
        {
            this.action = action;
        }

        public PlanAction GetAction()
        {
            return action;
        }
    }
}
