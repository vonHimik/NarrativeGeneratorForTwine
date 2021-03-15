using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class StoryGraph
    {
        private StoryNode startNode; // Root
        private List<StoryNode> nodes;      // Last in this list = current.
        private List<Edge> edges;

        public StoryGraph()
        {
            startNode = new StoryNode();
            nodes = new List<StoryNode>();
            edges = new List<Edge>();
        }

        public void AddNode(StoryNode newNode) // State
        {
            nodes.Add(newNode);
        }

        public void AddEdge(Edge newEdge) // Action
        {
            edges.Add(newEdge);
        }

        // TODO
        public void ExpandNode(StoryworldConvergence storyworldConvergence)
        {
            throw new NotImplementedException();
        }

        public List<StoryNode> GetNodes()
        {
            return nodes;
        }

        public StoryNode GetRoot()
        {
            return startNode;
        }

        public StoryNode GetLastNode()
        {
            return nodes.Last();
        }
    }
}
