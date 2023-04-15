using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that implements a storygraph node that stores information about a specific state of the storyworld.
    /// </summary>
    public class StoryNode : IEquatable<StoryNode>, ICloneable
    {
        /// <summary>
        /// Story state.
        /// </summary>
        private WorldDynamic worldState;

        /// <summary>
        /// Player activity indicator.
        /// </summary>
        private bool playerIsActive;

        /// <summary>
        /// The agent that created the node by its action.
        /// </summary>
        private KeyValuePair<AgentStateStatic, AgentStateDynamic> activeAgent;

        // Properties
        private HashSet<StoryNode> links;
        private HashSet<Edge> edges;
        private int numberInSequence;

        // Hashcode
        private bool hasHashCode;
        private int hashCode;

        /// <summary>
        /// Determinant of whether the node stores the goal state or not.
        /// </summary>
        public bool goalState = false;
        /// <summary>
        /// Specifies that the state stored by the node is the result of skipping an action.
        /// </summary>
        public bool skiped = false;
        /// <summary>
        /// A determinant that the state stored by the node is obtained as a result of a counter action.
        /// </summary>
        public bool counteract = false;

        /// <summary>
        /// Constructor without parameters.
        /// </summary>
        public StoryNode()
        {
            links = new HashSet<StoryNode>();
            edges = new HashSet<Edge>();

            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Method for cloning an StoryNode instance.
        /// </summary>
        /// <returns>A new instance that is a copy of the current one.</returns>
        public object Clone()
        {
            var clone = new StoryNode();

            clone.worldState = new WorldDynamic(worldState);
            clone.playerIsActive = playerIsActive;

            clone.activeAgent = 
                new KeyValuePair<AgentStateStatic, AgentStateDynamic>
                (new AgentStateStatic(activeAgent.Key), new AgentStateDynamic(activeAgent.Value));

            clone.links = new HashSet<StoryNode>(links);
            clone.edges = new HashSet<Edge>(edges);
            clone.numberInSequence = numberInSequence;

            clone.hasHashCode = true;
            clone.GetHashCode();

            return clone;
        }

        /// <summary>
        /// Sets the storyworld state stored by the node.
        /// </summary>
        /// <param name="worldState">New stored storyworld state.</param>
        public void SetWorldState (WorldDynamic worldState)
        {
            this.worldState = worldState;
            UpdateHashCode();
        }

        /// <summary>
        /// Returns the storyworld state stored by the node.
        /// </summary>
        /// <returns>Stored storyworld state.</returns>
        public WorldDynamic GetWorldState() { return worldState; }

        /// <summary>
        /// Sets whether the player takes an action in the state stored by this node or not.
        /// </summary>
        /// <param name="playerIsActive">Information about the player's activity.</param>
        public void SetActivePlayer (bool playerIsActive) { this.playerIsActive = playerIsActive; }

        /// <summary>
        /// Returns the player's activity status in the state stored by this node.
        /// </summary>
        /// <returns>True if the player is active, false otherwise.</returns>
        public bool GetActivePlayer() { return playerIsActive; }

        /// <summary>
        /// Sets the active agent to the state maintained by this node.
        /// </summary>
        /// <param name="activeAgent">Information about the agent.</param>
        public void SetActiveAgent (KeyValuePair<AgentStateStatic, AgentStateDynamic> activeAgent) { this.activeAgent = activeAgent; }

        /// <summary>
        /// Returns information about the active agent.
        /// </summary>
        /// <returns>Information about the agent.</returns>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetActiveAgent() { return activeAgent; }

        /// <summary>
        /// Adds a link between this node and the specified node.
        /// </summary>
        /// <param name="node">Link to another node.</param>
        public void AddLinkToNode (ref StoryNode node) { links.Add(node); }

        /// <summary>
        /// Returns a node from the list of connected nodes to this node.
        /// </summary>
        /// <param name="index">The index of the requested node.</param>
        /// <returns>Requested node.</returns>
        public StoryNode GetLinkToNode (int index) { return links.ElementAt(index); }

        /// <summary>
        /// If among the nodes connected to this node there is a node similar to the specified node, then it will return the discovered node from the list.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Requested node.</returns>
        public StoryNode GetLinkToNode (StoryNode node)
        {
            foreach (var link in links) { if (node.Equals(link)) { return link; } }
            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Returns a list of all nodes connected to this node.
        /// </summary>
        /// <returns>List of nodes.</returns>
        public HashSet<StoryNode> GetLinks() { return links; }

        /// <summary>
        /// If the specified node is among the list of nodes linked with this node, then the link will be deleted.
        /// </summary>
        /// <param name="linkedNode">The node to be removed.</param>
        public void DeleteLink (StoryNode linkedNode)
        {
            linkedNode.GetLinks().Remove(this);
            links.Remove(linkedNode);
        }

        /// <summary>
        /// Checks if a link has been established between this node and the specified one.
        /// </summary>
        /// <param name="anotherNode">Checked node.</param>
        /// <returns>True if yes, otherwise false.</returns>
        public bool ConnectedWith (StoryNode anotherNode)
        {
            bool result = false;

            foreach (var linkedNode in links)
            {
                if (linkedNode.Equals(anotherNode)) { result = true; }
            }

            return result;
        }

        /// <summary>
        /// Adds the specified edge to the list of edges connected to this node.
        /// </summary>
        /// <param name="edge">The edge to be added.</param>
        public void AddEdge (Edge edge) { edges.Add(edge); }

        /// <summary>
        /// Returns the specified edge from the list of edges connected to the this node.
        /// </summary>
        /// <param name="index">The index of the requested edge.</param>
        /// <returns>Required edge.</returns>
        public Edge GetEdge (int index) { return edges.ElementAt(index); }

        /// <summary>
        /// Returns the specified edge from the list of edges connected to the this node.
        /// </summary>
        /// <param name="edge">Information about the requested edge.</param>
        /// <returns>Required edge.</returns>
        public Edge GetEdge (Edge edge)
        {
            foreach (var e in edges)
            {
                if (edge.Equals(e)) { return e; }
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Returns a list of all edges connected to the this node.
        /// </summary>
        /// <returns>List of edges.</returns>
        public HashSet<Edge> GetEdges() { return edges; }

        /// <summary>
        /// Returns the last edge from a list of all edges connected to the this node.
        /// </summary>
        /// <returns>Required edge.</returns>
        public Edge GetLastEdge() { return edges.Last(); }

        /// <summary>
        /// Returns the first edge from a list of all edges connected to the this node.
        /// </summary>
        /// <returns>Required edge</returns>
        public Edge GetFirstEdge() { return edges.First(); }

        /// <summary>
        /// Removes the specified edge from the list of all edges connected to the this node.
        /// </summary>
        /// <param name="edge">Information about the removed edge.</param>
        public void RemoveEdge (Edge edge) { edges.Remove(edge); }

        /// <summary>
        /// Returns the sequence number of this node.
        /// </summary>
        /// <returns>The numeric value of the sequence number.</returns>
        public int GetNumberInSequence() { return numberInSequence; }

        /// <summary>
        /// Sets the sequence number of this node.
        /// </summary>
        /// <param name="newNumber">New numeric value of th e sequence number.</param>
        public void SetNumberInSequence (int newNumber) { numberInSequence = newNumber; }

        /// <summary>
        /// Checks if this node is a child of the specified node.
        /// </summary>
        /// <param name="probablyParentNode">Probably parent node.</param>
        /// <returns>True if yes, otherwise false.</returns>
        public bool isChild (StoryNode probablyParentNode)
        {
            bool result = false;

            foreach (var edge in this.GetEdges())
            {
                if (edge.GetUpperNode() == probablyParentNode && edge.GetLowerNode() == this)
                {
                    result = true;
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Method for comparing two StoryNode instance.
        /// </summary>
        /// <param name="anotherNode">Another StoryNode instance, for comparison.</param>
        /// <returns>True if both instance are the same, false otherwise.</returns>
        public bool Equals (StoryNode anotherNode)
        {
            if (anotherNode == null) { return false; }

            bool worldStateEquals = worldState.Equals(anotherNode.worldState);
            bool worldStateReferenceEquals = object.ReferenceEquals(worldState, anotherNode.worldState);

            bool playerIsActiveEquals = (playerIsActive == anotherNode.playerIsActive);

            bool activeAgentStaticEquals = activeAgent.Key.Equals(anotherNode.activeAgent.Key);
            bool activeAgentStaticReferenceEquals = object.ReferenceEquals(activeAgent.Key, anotherNode.activeAgent.Key);

            bool activeAgentDynamicEquals = activeAgent.Value.Equals(anotherNode.activeAgent.Value);
            bool activeAgentDynamicReferenceEquals = object.ReferenceEquals(activeAgent.Value, anotherNode.activeAgent.Value);

            bool worldStateGlobal = worldStateEquals || worldStateReferenceEquals;
            bool activeAgentStaticGlobal = activeAgentStaticEquals || activeAgentStaticReferenceEquals;
            bool activeAgentDynamicGlobal = activeAgentDynamicEquals || activeAgentDynamicReferenceEquals;

            bool equal = worldStateGlobal /*&& playerIsActiveEquals && activeAgentStaticGlobal && activeAgentDynamicGlobal*/;

            return equal;
        }

        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

        /// <summary>
        /// Calculates and returns the hash code of this instance of the StoryNode.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            worldState.ClearHashCode();
            hashcode = hashcode * 42 + worldState.GetHashCode();
            hashcode = hashcode * 42 + counteract.GetHashCode();

            hashCode = hashcode;
            hasHashCode = true;

            return hashcode;
        }

        /// <summary>
        /// Clears the current hash code value.
        /// </summary>
        public void ClearHashCode()
        {
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Updates (refresh) the current hash code value.
        /// </summary>
        public void UpdateHashCode()
        {
            ClearHashCode();
            GetHashCode();
        }
    }
}
