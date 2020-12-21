﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class StoryNode
    {
        private WorldBeliefs worldState;  // Story state.
        private bool activePlayer; // Active player = true, active agent = false.
        private Agent activeAgent; // If the player does not move, then one of the agents move - which?

        private StoryNode parent;
        private List<StoryNode> childrens;
        private List<Edge> edgesToChildrens;

        public void SetWorldState(WorldBeliefs worldState)
        {
            this.worldState = worldState;
        }

        public WorldBeliefs GetWorldState()
        {
            return worldState;
        }

        public void SetActivePlayer (bool activePlayer)
        {
            this.activePlayer = activePlayer;
        }

        public bool GetActivePlayer()
        {
            return activePlayer;
        }

        public void SetActiveAgent(Agent activeAgent)
        {
            this.activeAgent = activeAgent;
        }

        public Agent GetActiveAgent()
        {
            return activeAgent;
        }

        public void SetParentNode(StoryNode parent)
        {
            this.parent = parent;
        }

        public StoryNode GetParentNode()
        {
            return parent;
        }

        public void AddChildrenNode(StoryNode node)
        {
            childrens.Add(node);
        }

        public StoryNode GetChildrenNode(int index)
        {
            return childrens[index];
        }

        public void AddEdge(Edge edge)
        {
            edgesToChildrens.Add(edge);
        }

        public Edge GetEdge(int index)
        {
            return edgesToChildrens[index];
        }
    }
}
