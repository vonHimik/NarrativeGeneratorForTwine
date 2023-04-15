using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that implements the edges between the nodes of the storygraph, which store the action serving as a transition trigger between them.
    /// </summary>
    public class Edge: ICloneable
    {
        /// <summary>
        /// The node from which the oriented edge originates. Is the parent of the lower node.
        /// </summary>
        private StoryNode upperNode;
        /// <summary>
        /// The node into which the oriented edge comes. It is a child node of the upper node.
        /// </summary>
        private StoryNode lowerNode;
        /// <summary>
        /// An action that creates a lower node from a upper node.
        /// </summary>
        private PlanAction action;

        /// <summary>
        /// A method that implements edge cloning.
        /// </summary>
        /// <returns>Returns a clone of the current edge object.</returns>
        public object Clone()
        {
            var clone = new Edge();

            clone.upperNode = (StoryNode)upperNode.Clone();
            clone.lowerNode = (StoryNode)lowerNode.Clone();
            clone.action = action;

            return clone;
        }

        /// <summary>
        /// This method allows assign a node for the top vertex of an edge (outgoing, earlier in time).
        /// </summary>
        /// <param name="node">The node that will be assigned to the top slot of this edge.</param>
        public void SetUpperNode (ref StoryNode node) { upperNode = node; }

        /// <summary>
        /// This method allows assign a node for the top vertex of an edge (outgoing, earlier in time).
        /// </summary>
        /// <param name="node">The node that will be assigned to the top slot of this edge.</param>
        public void SetUpperNode (StoryNode node) { upperNode = node; }

        /// <summary>
        /// This method clears the top vertex of an edge from a connected node.
        /// </summary>
        public void ClearUpperNode()
        {
            upperNode.GetEdges().Remove(this);
            upperNode = null;
        }

        /// <summary>
        /// This method returns the node attached to the top vertex of this edge.
        /// </summary>
        /// <returns>Returns the node attached to the top vertex of edge.</returns>
        public StoryNode GetUpperNode() { return upperNode; }

        /// <summary>
        /// This method allows assign a node to the bottom vertex of an edge (incoming, later in time).
        /// </summary>
        /// <param name="node">The node that will be assigned to the bottom slot of this edge.</param>
        public void SetLowerNode (ref StoryNode node) { lowerNode = node; }

        /// <summary>
        /// This method allows assign a node to the bottom vertex of an edge (incoming, later in time).
        /// </summary>
        /// <param name="node">The node that will be assigned to the bottom slot of this edge.</param>
        public void SetLowerNode (StoryNode node) { lowerNode = node; }

        /// <summary>
        /// This method clears the bottom vertex of an edge from a connected node.
        /// </summary>
        public void ClearLowerNode()
        {
            if (lowerNode != null)
            {
                lowerNode.GetEdges().Remove(this);
                lowerNode = null;
            }
        }

        /// <summary>
        /// This method returns the node attached to the bottom vertex of this edge.
        /// </summary>
        /// <returns>Returns the node attached to the bottom vertex of edge.</returns>
        public StoryNode GetLowerNode() { return lowerNode; }

        /// <summary>
        /// This method allows assign an action to this edge.
        /// </summary>
        /// <param name="action">The action to be assigned to this edge.</param>
        public void SetAction (PlanAction action) { this.action = action; }

        /// <summary>
        /// This method removes the action attached to this edge.
        /// </summary>
        public void ClearAction() { action = null; }

        /// <summary>
        /// This method returns the action attached to this edge.
        /// </summary>
        /// <returns>Returns the action attached to edge</returns>
        public PlanAction GetAction() { return action; }

        /// <summary>
        /// This method completely clears the edge, removing both nodes attached to it and the action attached to it too.
        /// </summary>
        public void ClearEdge()
        {
            ClearAction();
            ClearUpperNode();
            ClearLowerNode();
        }
    }
}
