using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// Enumerator for goal types.
    /// </summary>
    public enum GoalTypes
    {
        /// <summary>
        /// Values for the type that defines the agent statuses as the target parameter.
        /// </summary>
        STATUS,
        /// <summary>
        /// Values for a type that defines the target parameter to be in certain locations.
        /// </summary>
        LOCATION,
        /// <summary>
        /// Values for a type that targets the possession of certain items.
        /// </summary>
        POSSESSION,
    }

    /// <summary>
    /// A class that facilitates interaction with goal types and their use.
    /// </summary>
    public static class GoalTypesUtils
    {
        /// <summary>
        /// A method that returns the name of the specified goal type.
        /// </summary>
        /// <param name="type">The goal type whose name is the desired to get.</param>
        /// <returns>Name of the specified goal type</returns>
        public static string GetName (GoalTypes type)
        {
            return Enum.GetName(typeof(GoalTypes), type);
        }

        /// <summary>
        /// A method that returns a goal types that matches the passed name.
        /// </summary>
        /// <param name="name">The name of the goal type to get.</param>
        /// <returns>The value of the goal type that matches the passed name.</returns>
        public static GoalTypes GetEnum (string name)
        {
            if (name == "status") return GoalTypes.STATUS;
            if (name == "location") return GoalTypes.LOCATION;
            if (name == "possession") return GoalTypes.POSSESSION;
            throw new Exception("UNRECOGNIZED GOAL TYPE: " + name);
        }
    }
}
