using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class StoryGraph
    {
        public StoryNode startNode; // Root
        List<StoryNode> nodes;
        List<Edge> edges;

        public void AddNode() // State
        {
            // nodes.Add();
        }

        public void AddEdge() // Action
        {
            // edges.Add();
        }

        public void ExpandNode()
        {
            AddNode();
            AddEdge();
        }

        public void CyclesControl()
        {

        }
    }
}
