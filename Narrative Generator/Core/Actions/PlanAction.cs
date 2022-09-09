using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that implements an action that an agent takes to change the state of the storyworld.
    /// </summary>
    [Serializable]
    public abstract class PlanAction
    {
        /// <summary>
        /// The list of arguments that the action instance will receive.
        /// </summary>
        public List<Object> Arguments = new List<object>();
        /// <summary>
        /// True if the action completed successfully.
        /// </summary>
        public bool success = false;
        /// <summary>
        /// True if the action failed.
        /// </summary>
        public bool fail = false;
        /// <summary>
        /// A variable to store the description of the action that will be displayed when it is rendered.
        /// </summary>
        public string desc = null;

        /// <summary>
        /// Constructor without parameters for the action instance.
        /// </summary>
        public PlanAction() {}

        /// <summary>
        /// Constructor for the action instance.
        /// </summary>
        /// <param name="args">The arguments to be passed to the action.</param>
        public PlanAction (params Object[] args)
        {
            foreach (Object arg in args)
            {
                Arguments.Add(arg);
            }
        }

        /// <summary>
        /// An abstract method for defining an action description.
        /// </summary>
        /// <param name="state">The current state of the storyworld.</param>
        public abstract void DefineDescription (WorldDynamic state);

        /// <summary>
        /// An abstract method that returns a description of the action.
        /// </summary>
        /// <returns>Description of the action</returns>
        public string WriteDescription() { return desc; }

        /// <summary>
        /// An abstract method that determines the fulfillment of the preconditions necessary to perform an action.
        /// </summary>
        /// <param name="state">The current state of the storyworld.</param>
        /// <returns>True if the preconditions are met, false otherwise.</returns>
        public abstract bool CheckPreconditions (WorldDynamic state);

        /// <summary>
        /// An abstract method for applying changes defined by an action to the current state of the storyworld.
        /// </summary>
        /// <param name="state">The current state of the storyworld.</param>
        public abstract void ApplyEffects (ref WorldDynamic state);

        /// <summary>
        /// An abstract method for checking the preconditions of an action and, if they are satisfied, the execution of its effects.
        /// </summary>
        /// <param name="state">The current state of the storyworld.</param>
        /// <returns>True if the preconditions are met and the action is done, false otherwise.</returns>
        public bool TakeAction (WorldDynamic state)
        {
            if (CheckPreconditions(state))
            {
                ApplyEffects(ref state);
                return true;
            }

            return false;
        }

        /// <summary>
        /// An abstract method for defining behavior if the result of an action fails.
        /// </summary>
        /// <param name="state">The current state of the storyworld.</param>
        public abstract void Fail (ref WorldDynamic state);
    }
}
