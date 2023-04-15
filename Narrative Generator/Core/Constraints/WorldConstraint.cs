using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// The base class for implementing the constraints imposed on the story.
    /// </summary>
    public abstract class WorldConstraint
    {
        /// <summary>
        /// A method that checks whether the specified world state satisfies constraints.
        /// </summary>
        /// <param name="newState">The new state that will be obtained when applying the action on the current state.</param>
        /// <param name="currentState">The current state of the storyworld.</param>
        /// <param name="graph">Story graph, which is a collection of nodes connected by oriented edges.</param>
        /// <param name="currentAction">The action currently performed by the agent, and whose influence on changing the world is being checked.</param>
        /// <param name="currentNode">A node that stores the current state.</param>
        /// <param name="newNode">A node that stores the future world state, resulting from applying the current action on the current node.</param>
        /// <param name="succsessControl">A variable into which the overridden result (succes or fail) of an action can be written.</param>
        /// <returns>Result of the check.</returns>
        public abstract bool IsSatisfied (WorldDynamic newState, 
                                          WorldDynamic currentState, 
                                          StoryGraph graph, 
                                          ref PlanAction currentAction, 
                                          StoryNode currentNode,
                                          StoryNode newNode,
                                          ref bool succsessControl);
    }
}
