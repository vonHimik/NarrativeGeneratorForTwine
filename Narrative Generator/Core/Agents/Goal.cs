using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that implements the agent's belifs about the goal state of the storyworld.
    /// </summary>
    [Serializable]
    public class Goal : ICloneable, IEquatable<Goal>
    {
        // Goal components
        /// <summary>
        /// Goal type marker.
        /// </summary>
        private List<GoalTypes> goalTypes;
        /// <summary>
        /// Goal state of the world.
        /// </summary>
        private WorldDynamic goalState;

        // Hashcode
        /// <summary>
        /// An indicator of whether a hashcode has been generated for this component.
        /// </summary>
        private bool hasHashCode;
        /// <summary>
        /// The hashcode of this component.
        /// </summary>
        private int hashCode;

        /// <summary>
        /// Constructor without parameters.
        /// </summary>
        public Goal()
        {
            goalTypes = new List<GoalTypes>();
            goalState = new WorldDynamic();
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// The conditional constructor of the goal, which creates a new instance of the goal based on the passed clone.
        /// </summary>
        /// <param name="clone">A goal instance that will serve as the basis for creating a new instance.</param>
        public Goal (Goal clone)
        {
            goalTypes = clone.goalTypes;
            goalState = (WorldDynamic)clone.goalState.Clone();
            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
        }

        /// <summary>
        /// A constructor with parameters that takes the values of its type and the target state to construct a new instance of the goal.
        /// </summary>
        /// <param name="goalTypes">The type(s) of the goal to construct.</param>
        /// <param name="goalState">Goal state of the storyworld of the constructed target.</param>
        public Goal (List<GoalTypes> goalTypes, WorldDynamic goalState)
        {
            this.goalTypes = goalTypes;
            this.goalState = goalState;
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Constructor with parameters if the goal type is specified and the goal state is created.
        /// </summary>
        /// <param name="goalType">Type of creating goal.</param>
        /// <param name="goalState">Target state of created goal.</param>
        public Goal (GoalTypes goalType, WorldDynamic goalState)
        {
            if (goalTypes == null || goalTypes.Count == 0) { this.goalTypes = new List<GoalTypes>(); }
            else { goalTypes.Add(goalType); }

            this.goalState = goalState;
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Method for cloning a goal instance.
        /// </summary>
        /// <returns>A new instance that is a copy of the current one.</returns>
        public object Clone()
        {
            var clone = new Goal();

            clone.goalTypes = goalTypes;
            clone.goalState = new WorldDynamic(goalState);

            return clone;
        }

        /// <summary>
        /// Returns the type of this instance of the goal.
        /// </summary>
        /// <returns>Type of this instance of the goal.</returns>
        public List<GoalTypes> GetGoalType() { return goalTypes; }

        /// <summary>
        /// Returns the goal state of storyworld of this instance of the goal.
        /// </summary>
        /// <returns>Goal state of this instance of the goal.</returns>
        public WorldDynamic GetGoalState() { return goalState; }

        /// <summary>
        /// Adds a goal type.
        /// </summary>
        /// <param name="newGoalType">Goal type to add.</param>
        public void AddGoalType (GoalTypes newGoalType)
        {
            goalTypes.Add(newGoalType);
        }

        /// <summary>
        /// Method for comparing two goal instance.
        /// </summary>
        /// <param name="anotherGoal">Another goal instance, for comparison.</param>
        /// <returns>True if both states are the same, false otherwise.</returns>
        public bool Equals (Goal anotherGoal)
        {
            if (anotherGoal == null) { return false; }

            bool goalTypeEquals = (goalTypes.Equals(anotherGoal.goalTypes));
            bool goalTypeReferenceEquals = object.ReferenceEquals(goalTypes, anotherGoal.goalTypes);

            bool goalStateEquals = goalState.Equals(anotherGoal.goalState);
            bool goalStateReferenceEquals = object.ReferenceEquals(goalState, anotherGoal.goalState);

            bool goalTypeGlobal = goalTypeEquals || goalTypeReferenceEquals;
            bool goalStateGlobal = goalStateEquals || goalStateReferenceEquals;

            bool equal = goalTypeGlobal && goalStateGlobal;

            return equal;
        }

        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

        /// <summary>
        /// Calculates and returns the hash code of this instance of the goal.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            hashcode = hashcode * 42 + goalTypes.GetHashCode();
            hashcode = hashcode * 42 + goalState.GetHashCode();

            hashCode = hashcode;
            hasHashCode = true;

            return hashcode;
        }
    }
}
