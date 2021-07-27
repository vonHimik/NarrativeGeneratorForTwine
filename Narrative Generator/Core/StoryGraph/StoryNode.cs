using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class StoryNode : IEquatable<StoryNode>
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

        public int numberInSequence;

        public StoryNode()
        {
            links = new HashSet<StoryNode>();
            edges = new HashSet<Edge>();

            hasHashCode = false;
            hashCode = 0;
        }

        public void SetWorldState(WorldDynamic worldState)
        {
            this.worldState = worldState;
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

        public void AddEdge(Edge edge)
        {
            edges.Add(edge);
        }

        public Edge GetEdge(int index)
        {
            return edges.ElementAt(index);
        }

        public Edge GetEdge(Edge edge)
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

        public override bool Equals(object obj)
        {
            return Equals(obj as StoryNode);
        }

        public bool Equals(StoryNode anotherNode)
        {
            if (anotherNode == null) { return false; }

            if (GetHashCode() == anotherNode.GetHashCode()) { return true; }

            //bool equals = 

            bool activePlayerCheckEquals = this.GetActivePlayer().Equals(anotherNode.GetActivePlayer());

            bool worldStateReferenceEquals = object.ReferenceEquals(GetWorldState(), anotherNode.GetWorldState());
            bool worldStateEquals = worldState.Equals(anotherNode.GetWorldState());

            bool activeAgentReferenceEquals = object.ReferenceEquals(GetActiveAgent(), anotherNode.GetActiveAgent());
            bool activeAgentEquals = GetActiveAgent().Equals(anotherNode.GetActiveAgent());


            bool stateEquals = activePlayerCheckEquals && (worldStateReferenceEquals || worldStateEquals);
            /*this.GetActivePlayer().Equals(anotherNode.GetActivePlayer()) &&
        (
            object.ReferenceEquals(GetWorldState(), anotherNode.GetWorldState()) ||
            worldState.Equals(anotherNode.GetWorldState())
        );*/
            /*&&*/
            bool activeAgentEqualsGlobal = activeAgentReferenceEquals || activeAgentEquals;
            /*(
                object.ReferenceEquals(GetActiveAgent(), anotherNode.GetActiveAgent()) ||
                GetActiveAgent().Equals(anotherNode.GetActiveAgent())
            ) */
            /*&&
            (
                object.ReferenceEquals(this.GetLinks(), anotherNode.GetLinks()) ||
                (this.GetLinks() != null &&
                this.GetLinks().Equals(anotherNode.GetLinks()))
            )*/
            /*&&
            (
                object.ReferenceEquals(this.GetEdges(), anotherNode.GetEdges()) ||
                (this.GetEdges() != null &&
                this.GetEdges().Equals(anotherNode.GetEdges()))
            )*/;

            bool equals = stateEquals && activeAgentEqualsGlobal;

            return equals;
        }

        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            hashcode = hashcode * 42 + worldState.GetHashCode();
            hashcode = hashcode * 42 + playerIsActive.GetHashCode();
            hashcode = hashcode * 42 + activeAgent.GetHashCode();
            //hashcode = hashcode * 42 + links.GetHashCode();
            //hashcode = hashcode * 42 + edges.GetHashCode();

            hashCode = hashcode;
            hasHashCode = true;

            return hashcode;
        }
    }
}
