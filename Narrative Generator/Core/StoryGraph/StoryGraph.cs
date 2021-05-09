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
        private StoryNode root;

        // List of nodes.
        private HashSet<StoryNode> nodes;

        /// <summary>
        /// Constructor method for story graph, no parameters.
        /// </summary>
        public StoryGraph()
        {
            root = new StoryNode();
            nodes = new HashSet<StoryNode>();
        }

        public void DFS(StoryNode root)
        {
            Stack<StoryNode> q = new Stack<StoryNode>();
            HashSet<StoryNode> vis = new HashSet<StoryNode>();
            q.Push(root);
            vis.Add(root);
            // DoSomething(null, root); STEP
            while (q.Count > 0)
            {
                StoryNode current = q.Pop();

                foreach (StoryNode next in current.GetLinks())
                {
                    if (vis.Contains(next)) continue; // Контроль циклов
                    q.Push(next);
                    vis.Add(next);
                    // DoSomething(current, next); STEP
                }
            }
        }

        /// <summary>
        /// Adds a node to the list of nodes in the story graph.
        /// </summary>
        /// <param name="newNode"></param>
        public void AddNode(StoryNode newNode)
        {
            nodes.Add(newNode);
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
            return root;
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

        public StoryNode GetNode(StoryNode node)
        {
            foreach (var n in nodes)
            {
                if (node.Equals(n)) { return n; }
            }

            throw new KeyNotFoundException();
        }
    }
}
