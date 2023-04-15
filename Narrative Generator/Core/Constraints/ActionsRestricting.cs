using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A type of constraint that restricts agents from performing actions.
    /// </summary>
    class ActionsRestricting : WorldConstraint
    {
        /// <summary>
        /// Marker to the type of constraint to apply: actions that cannot be performed sequentially.
        /// </summary>
        public bool mutuallyExclusiveActions;
        /// <summary>
        /// Marker to the type of constraint to apply: an action can be performed only once.
        /// </summary>
        public bool onlyOneFire;
        /// <summary>
        /// Marker to the type of constraint to apply: in the specified location, the specified action can be performed only once.
        /// </summary>
        public bool onlyOnceInLocation;
        /// <summary>
        /// The main variable that stores the action with which the main manipulations are carried out.
        /// </summary>
        public PlanAction mainAction;
        /// <summary>
        /// A variable that stores additional actions.
        /// </summary>
        public List<PlanAction> targetActions;
        /// <summary>
        /// Timer value required for some types of restrictions.
        /// </summary>
        public int timer;

        /// <summary>
        /// Constructor method.
        /// </summary>
        /// <param name="mutuallyExclusiveActions">If true, then actions that cannot be performed sequentially.</param>
        /// <param name="onlyOneFire">If true, then an action can be performed only once.</param>
        /// <param name="onlyOnceInLocation">If true, then in the specified location, the specified action can be performed only once.</param>
        /// <param name="mainAction">The main variable that stores the action with which the main manipulations are carried out.</param>
        /// <param name="targetActions">A variable that stores additional actions.</param>
        /// <param name="timer">Timer value required for some types of restrictions.</param>
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

        /// <summary>
        /// A method that checks whether the specified world state satisfies constraints.
        /// </summary>
        /// <param name="newState">The new state that will be obtained when applying the action on the current state.</param>
        /// <param name="currentState">The current state of the storyworld.</param>
        /// <param name="graph">Story graph, which is a collection of nodes connected by oriented edges.</param>
        /// <param name="currentAction">The action currently performed by the agent, and whose influence on changing the world is being checked.</param>
        /// <param name="currentNode">A node that stores the current world state.</param>
        /// <param name="newNode">A node that stores the future world state, resulting from applying the current action on the current node.</param>
        /// <param name="succsessControl">A variable into which the overridden result (succes or fail) of an action can be written.</param>
        /// <returns>Result of the check.</returns>
        public override bool IsSatisfied (WorldDynamic newState, 
                                          WorldDynamic currentState, 
                                          StoryGraph graph, 
                                          ref PlanAction currentAction, 
                                          StoryNode currentNode,
                                          StoryNode newNode,
                                          ref bool succsessControl)
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
