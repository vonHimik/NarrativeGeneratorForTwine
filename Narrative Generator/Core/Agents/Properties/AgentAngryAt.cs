using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class representing the state of the agent's anger and the object of this anger.
    /// </summary>
    [Serializable]
    public class AgentAngryAt : AgentProperty, ICloneable, IEquatable<AgentAngryAt>
    {
        // AgentAngryAt components
        private bool angry;
        private AgentStateStatic objectOfAngry;

        // Hashcode
        private bool hasHashCode;
        private int hashCode;

        /// <summary>
        /// Constructor without parameters.
        /// </summary>
        public AgentAngryAt()
        {
            angry = false;
            objectOfAngry = new AgentStateStatic();
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Constructor with parameters of the AgentAngryAt, which creates a new instance of the AgentAngryAt based on the passed clone.
        /// </summary>
        /// <param name="clone">An AgentAngryAt instance that will serve as the basis for creating a new instance.</param>
        public AgentAngryAt (AgentAngryAt clone)
        {
            angry = clone.angry;
            if (clone.objectOfAngry != null) { objectOfAngry = (AgentStateStatic)clone.objectOfAngry.Clone(); }
            else { objectOfAngry = new AgentStateStatic(); }
            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
        }

        /// <summary>
        /// A parameterized constructor that takes the agent's anger status and information about the agent it is angry with.
        /// </summary>
        /// <param name="angry">Agent's anger status.</param>
        /// <param name="objectOfAngry">Information about the agent this agent is angry with.</param>
        public AgentAngryAt (bool angry, AgentStateStatic objectOfAngry)
        {
            this.angry = angry;
            this.objectOfAngry = objectOfAngry;
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Method for cloning an AgentAngryAt instance.
        /// </summary>
        /// <returns>A new instance that is a copy of the current one.</returns>
        public object Clone()
        {
            var clone = new AgentAngryAt();

            clone.angry = angry;
            if (objectOfAngry != null) { clone.objectOfAngry = new AgentStateStatic(objectOfAngry); }

            return clone;
        }

        /// <summary>
        /// Sets the angry state to true.
        /// </summary>
        public void AngryOn()
        {
            angry = true;
            UpdateHashCode();
        }

        /// <summary>
        /// Sets the angry state to false.
        /// </summary>
        public void AngryOff()
        {
            angry = false;
            UpdateHashCode();
        }

        /// <summary>
        /// Assigns an agent that this agent is angry with.
        /// </summary>
        /// <param name="target">Information about the agent this agent is angry with.</param>
        public void SetObjectOfAngry (AgentStateStatic target)
        {
            objectOfAngry = target;
            AngryOn();
            UpdateHashCode();
        }

        /// <summary>
        /// Returns information about the agent that this agent is angry with.
        /// </summary>
        /// <returns>Information about the agent that this agent is angry with.</returns>
        public AgentStateStatic GetObjectOfAngry() { return objectOfAngry; }

        /// <summary>
        /// Checks if the specified agent is the agent that this agent is angry with.
        /// </summary>
        /// <param name="agent">Information about the checked agent.</param>
        /// <returns>True if yes, otherwise false.</returns>
        public bool AngryCheckAtAgent (AgentStateStatic agent)
        {
            if (angry && agent == objectOfAngry) { return true; }
            else { return false; }
        }

        /// <summary>
        /// Checks the angry state of this agent.
        /// </summary>
        /// <returns>True if this agent is angry, false otherwise.</returns>
        public bool AngryCheck() { return angry; }

        /// <summary>
        /// Method for comparing two AgentAngryAt instance.
        /// </summary>
        /// <param name="anotherAngryAt">Another AgentAngryAt instance, for comparison.</param>
        /// <returns>True if both instance are the same, false otherwise.</returns>
        public bool Equals (AgentAngryAt anotherAngryAt)
        {
            if (anotherAngryAt == null) { return false; }

            bool angryEquals = (angry == anotherAngryAt.angry);
            bool angryReferenceEquals = object.ReferenceEquals(angry, anotherAngryAt.angry);

            bool objectOfAngryEquals;
            bool objectOfAngryReferenceEquals;
            if (objectOfAngry == null && anotherAngryAt.objectOfAngry == null)
            {
                objectOfAngryEquals = true;
                objectOfAngryReferenceEquals = true;
            }
            else if  ((objectOfAngry == null && anotherAngryAt.objectOfAngry != null) || objectOfAngry == null && anotherAngryAt.objectOfAngry != null)
            {
                objectOfAngryEquals = false;
                objectOfAngryReferenceEquals = false;
            }
            else
            {
                objectOfAngryEquals = objectOfAngry.Equals(anotherAngryAt.objectOfAngry);
                objectOfAngryReferenceEquals = object.ReferenceEquals(objectOfAngry, anotherAngryAt.objectOfAngry);
            }

            bool angryGlobal = angryEquals || angryReferenceEquals;
            bool objectOfAngryGlobal = objectOfAngryEquals || objectOfAngryReferenceEquals;

            bool equal = angryGlobal && objectOfAngryGlobal;

            return equal;
        }

        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

        /// <summary>
        /// Calculates and returns the hash code of this instance of the AgentAngryAt.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            hashcode = hashcode * 42 + angry.GetHashCode();
            if (objectOfAngry != null)
            {
                objectOfAngry.ClearHashCode();
                hashcode = hashcode * 42 + objectOfAngry.GetHashCode();
            }

            hashCode = hashcode;
            hasHashCode = true;

            return hashcode;
        }

        /// <summary>
        /// Clears the current hash code value.
        /// </summary>
        public void ClearHashCode()
        {
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Updates (refresh) the current hash code value.
        /// </summary>
        public void UpdateHashCode()
        {
            ClearHashCode();
            GetHashCode();
        }
    }
}
