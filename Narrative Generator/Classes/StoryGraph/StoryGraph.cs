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
        List<StoryNode> nodes;      // Last in this list = current.
        List<Edge> edges;

        public void AddNode(StoryNode newNode) // State
        {
            nodes.Add(newNode);
        }

        public void AddEdge(Edge newEdge) // Action
        {
            edges.Add(newEdge);
        }

        public void ExpandNode(StoryworldConvergence storyworldConvergence)
        {
            throw new NotImplementedException();
        }

        public List<StoryNode> GetNodes()
        {
            return nodes;
        }

        public StoryNode GetLastNode()
        {
            return nodes.Last();
        }
    }
}
