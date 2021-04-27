using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that is a representation of a history graph, where nodes are states of the world and edges are actions.
    /// </summary>
    class StoryGraph
    {
        // Link to the root node.
        private StoryNode startNode;

        // List of nodes.
        private HashSet<StoryNode> nodes;

        // List of edges.
        private HashSet<Edge> edges;

        /// <summary>
        /// Constructor method for story graph, no parameters.
        /// </summary>
        public StoryGraph()
        {
            startNode = new StoryNode();
            nodes = new HashSet<StoryNode>();
            edges = new HashSet<Edge>();
        }

        /// <summary>
        /// Adds a node to the list of nodes in the story graph.
        /// </summary>
        /// <param name="newNode"></param>
        public void AddNode(StoryNode newNode)
        {
            nodes.Add(newNode);
        }

        /// <summary>
        /// Adds a edge to the story graph edges list.
        /// </summary>
        /// <param name="newEdge"></param>
        public void AddEdge(Edge newEdge)
        {
            edges.Add(newEdge);
        }

        // TODO
        public void ExpandNode(StoryworldConvergence storyworldConvergence)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a list of nodes in the story graph.
        /// </summary>
        public HashSet<StoryNode> GetNodes()
        {
            return nodes;
        }

        /// <summary>
        /// Returns the root node of the story graph.
        /// </summary>
        public StoryNode GetRoot()
        {
            return startNode;
        }

        /// <summary>
        /// Returns the last node from the list of nodes in the story graph.
        /// </summary>
        public StoryNode GetLastNode()
        {
            return nodes.Last();
        }

        public StoryNode GetNode(int index)
        {
            return nodes.ElementAt(index);
        }
    }
}
