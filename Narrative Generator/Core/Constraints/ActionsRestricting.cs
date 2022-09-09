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
        public bool onlyOnceInLocation;
        public PlanAction mainAction;
        public List<PlanAction> targetActions;
        public int timer;

        public ActionsRestricting (bool mutuallyExclusiveActions, 
                                   bool onlyOneFire, 
                                   bool onlyOnceInLocation,
                                   PlanAction mainAction, 
                                   List<PlanAction> targetActions, 
                                   int timer)
        {
            this.mutuallyExclusiveActions = mutuallyExclusiveActions;
            this.onlyOneFire = onlyOneFire;
            this.onlyOnceInLocation = onlyOnceInLocation;
            this.mainAction = mainAction;
            this.targetActions = targetActions;
            this.timer = timer;

        }

        public override bool IsSatisfied (WorldDynamic newState, 
                                          WorldDynamic currentState, 
                                          StoryGraph graph, 
                                          PlanAction currentAction, 
                                          StoryNode currentNode,
                                          StoryNode newNode)
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
            if (onlyOnceInLocation)
            {
                if (currentAction.GetType().Equals(mainAction.GetType()))
                {
                    StoryNode testNode = currentNode;

                    while (testNode.GetNumberInSequence() != 0)
                    {
                        foreach (var edge in testNode.GetEdges())
                        {
                            if (edge.GetLowerNode().Equals(testNode) && edge.GetAction().GetType().Equals(mainAction.GetType())
                                && ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).
                                Value.GetBeliefs().GetMyLocation().Equals(
                                    ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)currentAction.Arguments[0]).Value.GetMyLocation()))
                            {
                                return false;
                            }
                        }

                        testNode = testNode.GetEdges().First().GetUpperNode();
                    }
                }
            }

            return true;
        }
    }
}
