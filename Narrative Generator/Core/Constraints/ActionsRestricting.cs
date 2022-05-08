using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class ActionsRestricting : WorldConstraint
    {
        public bool mutuallyExclusiveActions;
        public bool onlyOneFire;
        public PlanAction mainAction;
        public List<PlanAction> targetActions;
        public int timer;

        public ActionsRestricting (bool mutuallyExclusiveActions, bool onlyOneFire, PlanAction mainAction, List<PlanAction> targetActions, int timer)
        {
            this.mutuallyExclusiveActions = mutuallyExclusiveActions;
            this.onlyOneFire = onlyOneFire;
            this.mainAction = mainAction;
            this.targetActions = targetActions;
            this.timer = timer;

        }

        public override bool IsSatisfied (WorldDynamic newState, 
                                          WorldDynamic currentState, 
                                          StoryGraph graph, 
                                          PlanAction currentAction, 
                                          StoryNode currentNode)
        {
            if (mutuallyExclusiveActions)
            {
                StoryNode testNode = currentNode;

                while (testNode.GetNumberInSequence() != 0)
                {
                    if (testNode.GetEdge(0).GetAction().GetType().Equals(mainAction.GetType()))
                    {
                        foreach (var action in targetActions)
                        {
                            if (action.GetType().Equals(currentAction.GetType())) { return false; }
                        }
                    }

                    testNode = testNode.GetEdge(0).GetUpperNode();
                }
            }
            if (onlyOneFire)
            {
                if (mainAction.GetType().Equals(currentAction.GetType()))
                {
                    StoryNode testNode = currentNode;

                    while (testNode.GetNumberInSequence() != 0)
                    {
                        if (testNode.GetEdge(0).GetAction().GetType().Equals(mainAction.GetType()))
                        {
                            return false;
                        }

                        testNode = testNode.GetEdge(0).GetUpperNode();
                    }
                }
            }

            return true;
        }
    }
}
