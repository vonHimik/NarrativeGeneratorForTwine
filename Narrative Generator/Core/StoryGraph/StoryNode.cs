using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class StoryNode : IEquatable<StoryNode>, ICloneable
    {
        // Story state.
        private WorldDynamic worldState;
        
        // Active player = true, active agent = false.
        private bool playerIsActive;

        // If the player does not move, then one of the agents move - which?
        private KeyValuePair<AgentStateStatic, AgentStateDynamic> activeAgent;

        private HashSet<StoryNode> links;
        private HashSet<Edge> edges;

        private bool hasHashCode;
        private int hashCode;

        private int numberInSequence;

        public bool goalState = false;
        public bool skiped = false;
        public bool counteract = false;

        public StoryNode()
        {
            links = new HashSet<StoryNode>();
            edges = new HashSet<Edge>();

            hasHashCode = false;
            hashCode = 0;
        }

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
            clone.hasHashCode = true;
            clone.GetHashCode();

            return clone;
        }

        public void SetWorldState(WorldDynamic worldState)
        {
            this.worldState = worldState;
            UpdateHashCode();
        }

        public WorldDynamic GetWorldState()
        {
            return worldState;
        }

        public void SetActivePlayer (bool playerIsActive)
        {
            this.playerIsActive = playerIsActive;
        }

        public bool GetActivePlayer()
        {
            return playerIsActive;
        }

        public void SetActiveAgent(KeyValuePair<AgentStateStatic, AgentStateDynamic> activeAgent)
        {
            this.activeAgent = activeAgent;
        }

        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetActiveAgent()
        {
            return activeAgent;
        }

        public void AddLinkToNode(ref StoryNode node)
        {
            links.Add(node);
        }

        public StoryNode GetLinkToNode(int index)
        {
            return links.ElementAt(index);
        }

        public StoryNode GetLinkToNode(StoryNode node)
        {
            foreach (var l in links)
            {
                if (node.Equals(l)) { return l; }
            }

            throw new KeyNotFoundException();
        }

        public HashSet<StoryNode> GetLinks()
        {
            return links;
        }

        public void DeleteLink(StoryNode linkedNode)
        {
            linkedNode.GetLinks().Remove(this);
            links.Remove(linkedNode);
        }

        public bool ConnectedWith(StoryNode anotherNode)
        {
            bool result = false;

            foreach (var linkedNode in links)
            {
                if (linkedNode.Equals(anotherNode))
                {
                    result = true;
                }
            }

            return result;
        }

        public void AddEdge(Edge edge)
        {
            edges.Add(edge);
        }

        public Edge GetEdge(int index)
        {
            return edges.ElementAt(index);
        }

        public Edge GetEdge (Edge edge)
        {
            foreach (var e in edges)
            {
                if (edge.Equals(e)) { return e; }
            }

            throw new KeyNotFoundException();
        }

        public HashSet<Edge> GetEdges()
        {
            return edges;
        }

        public void RemoveEdge (Edge edge)
        {
            edges.Remove(edge);
        }

        public override bool Equals (object obj)
        {
            return Equals(obj as StoryNode);
        }

        public bool Equals(StoryNode anotherNode)
        {
            bool equal = false;

            this.UpdateHashCode();
            anotherNode.UpdateHashCode();

            if (anotherNode == null) { equal = false; }
            if (GetHashCode() == anotherNode.GetHashCode()) { equal = true; }

            return equal;
        }

        public int GetNumberInSequence()
        {
            return numberInSequence;
        }

        public void SetNumberInSequence(int newNumber)
        {
            numberInSequence = newNumber;
        }

        public bool isChildren(StoryNode probablyParentNode)
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

        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

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

        public void ClearHashCode()
        {
            hasHashCode = false;
            hashCode = 0;
        }

        public void UpdateHashCode()
        {
            ClearHashCode();
            GetHashCode();
        }
    }
}
