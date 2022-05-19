﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class Edge: ICloneable
    {
        private StoryNode upperNode;
        private StoryNode lowerNode;
        private PlanAction action;

        public object Clone()
        {
            var clone = new Edge();

            clone.upperNode = (StoryNode)upperNode.Clone();
            clone.lowerNode = (StoryNode)lowerNode.Clone();
            clone.action = action;

            return clone;
        }

        public void SetUpperNode (ref StoryNode node) { upperNode = node; }

        public void ClearUpperNode()
        {
            upperNode.GetEdges().Remove(this);
            upperNode = null;
        }

        public StoryNode GetUpperNode() { return upperNode; }

        public void SetLowerNode (ref StoryNode node) { lowerNode = node; }

        public void ClearLowerNode()
        {
            if (lowerNode != null)
            {
                lowerNode.GetEdges().Remove(this);
                lowerNode = null;
            }
        }

        public StoryNode GetLowerNode() { return lowerNode; }

        public void SetAction (PlanAction action) { this.action = action; }

        public void ClearAction() { action = null; }

        public PlanAction GetAction() { return action; }

        public void ClearEdge()
        {
            ClearAction();
            ClearUpperNode();
            ClearLowerNode();
        }
    }
}
