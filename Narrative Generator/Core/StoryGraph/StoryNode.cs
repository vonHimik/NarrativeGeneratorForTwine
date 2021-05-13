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

        public StoryNode()
        {
            links = new HashSet<StoryNode>();
            edges = new HashSet<Edge>();
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

        public HashSet<StoryNode> GetLinks()
        {
            return links;
        }

        public void AddEdge(Edge edge)
        {
            edges.Add(edge);
        }

        public Edge GetEdge(int index)
        {
            return edges.ElementAt(index);
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

            return this.GetActivePlayer().Equals(anotherNode.GetActivePlayer()) &&
            (
                object.ReferenceEquals(this.GetWorldState(), anotherNode.GetWorldState()) ||
                (this.GetWorldState() != null &&
                worldState.Equals(anotherNode.GetWorldState()))
            ) 
            &&
            (
                object.ReferenceEquals(this.GetActiveAgent(), anotherNode.GetActiveAgent()) ||
                (this.GetActiveAgent().Key != null && this.GetActiveAgent().Value != null &&
                this.GetActiveAgent().Equals(anotherNode.GetActiveAgent()))
            ) 
            &&
            (
                object.ReferenceEquals(this.GetLinks(), anotherNode.GetLinks()) ||
                (this.GetLinks() != null &&
                this.GetLinks().Equals(anotherNode.GetLinks()))
            )
            &&
            (
                object.ReferenceEquals(this.GetEdges(), anotherNode.GetEdges()) ||
                (this.GetEdges() != null &&
                this.GetEdges().Equals(anotherNode.GetEdges()))
            );
        }
    }
}
