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
        /// Method that checks whether the conditions of the specified constraints are met.
        /// </summary>
        /// <param name="newState">The new state that will be obtained when applying the action on the current state.</param>
        /// <param name="currentState">The current state of the storyworld.</param>
        /// <param name="graph">Story graph.</param>
        /// <param name="currentAction">The action that the current agent has chosen to perform on the current turn.</param>
        /// <param name="currentNode">A graph node that stores the current state.</param>
        /// <param name="newNode">A graph node that stores the future state.</param>
        /// <returns>Returns the result of the check.</returns>
        public abstract bool IsSatisfied (WorldDynamic newState, 
                                          WorldDynamic currentState, 
                                          StoryGraph graph, 
                                          PlanAction currentAction, 
                                          StoryNode currentNode,
                                          StoryNode newNode);
    }
}
