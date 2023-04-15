using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that implements the agent's belief that he is in a conversation with some other agent and stores information about this fact and about the interlocutor.
    /// </summary>
    [Serializable]
    public class TalkingWith : AgentProperty, ICloneable, IEquatable<TalkingWith>
    {
        // TalkingWith components
        /// <summary>
        /// An indicator of whether this agent behavior modifier is active.
        /// </summary>
        private bool talking;
        /// <summary>
        /// Another agent that is the target of this agent behavior modifier.
        /// </summary>
        private AgentStateStatic interlocutor;

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
        public TalkingWith()
        {
            talking = false;
            interlocutor = new AgentStateStatic();
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Constructor with parameters of the TalkingWith, which creates a new instance of the TalkingWith based on the passed clone.
        /// </summary>
        /// <param name="clone">A TalkingWith instance that will serve as the basis for creating a new instance.</param>
        public TalkingWith (TalkingWith clone)
        {
            talking = clone.talking;
            if (clone.interlocutor != null) { interlocutor = (AgentStateStatic)clone.interlocutor.Clone(); }
            else { interlocutor = new AgentStateStatic(); }
            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
        }

        /// <summary>
        /// A parameterized constructor that takes an indicator of whether the agent is currently talking and information about the agent with whom he is talking.
        /// </summary>
        /// <param name="talking">An indicator that the agent is in a conversation with another agent.</param>
        /// <param name="interlocutor">Information about the interlocutor agent.</param>
        public TalkingWith (bool talking, AgentStateStatic interlocutor)
        {
            this.talking = talking;
            this.interlocutor = interlocutor;
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Method for cloning an TalkingWith instance.
        /// </summary>
        /// <returns>A new instance that is a copy of the current one.</returns>
        public object Clone()
        {
            var clone = new TalkingWith();

            clone.talking = talking;
            if (interlocutor != null) { clone.interlocutor = new AgentStateStatic(interlocutor); }

            return clone;
        }

        /// <summary>
        /// Sets the status of the conversation to True.
        /// </summary>
        public void TalkingStart()
        {
            talking = true;
            UpdateHashCode();
        }

        /// <summary>
        /// Sets the status of the conversation to False.
        /// </summary>
        public void TalkingEnd()
        {
            talking = false;
            UpdateHashCode();
        }

        /// <summary>
        /// Sets the agent of the interlocutor for this agent.
        /// </summary>
        /// <param name="interlocutor">Information about the interlocutor agent.</param>
        public void SetInterlocutor (AgentStateStatic interlocutor)
        {
            this.interlocutor = interlocutor;
            UpdateHashCode();
        }

        /// <summary>
        /// Returns information about the agent of the interlocutor.
        /// </summary>
        /// <returns>Information about the agent of the interlocutor</returns>
        public AgentStateStatic GetInterlocutor() { return interlocutor; }

        /// <summary>
        /// Returns the status of the conversation.
        /// </summary>
        /// <returns>True if this agent is talking to another agent, false otherwise.</returns>
        public bool TalkingCheck() { return talking; }

        /// <summary>
        /// Clears all information about whether this agent is talking and with whom (deletes it).
        /// </summary>
        public void Clear()
        {
            this.interlocutor = null;
            UpdateHashCode();
        }

        /// <summary>
        /// Checks if the specified agent is a interlocutor of this agent.
        /// </summary>
        /// <param name="agent">Information about the checked agent.</param>
        /// <returns>True if yes, otherwise false.</returns>
        public bool TalkingCheckAtAgent (AgentStateStatic agent)
        {
            if (talking && agent == interlocutor) { return true; }
            else { return false; }
        }

        /// <summary>
        /// Method for comparing two TalkingWith instance.
        /// </summary> 
        /// <param name="anotherTalkingWith">Another TalkingWith instance, for comparison.</param>
        /// <returns>True if both instance are the same, false otherwise.</returns>
        public bool Equals(TalkingWith anotherTalkingWith)
        {
            if (anotherTalkingWith == null) { return false; }

            bool talkingEquals = (talking == anotherTalkingWith.talking);
            bool talkingReferenceEquals = object.ReferenceEquals(talking, anotherTalkingWith.talking);

            bool interlocutorEquals;
            bool interlocutorReferenceEquals;
            if (interlocutor == null && anotherTalkingWith.interlocutor == null)
            {
                interlocutorEquals = true;
                interlocutorReferenceEquals = true;
            }
            else if ((interlocutor != null && anotherTalkingWith.interlocutor == null)
                || (interlocutor == null && anotherTalkingWith.interlocutor != null))
            {
                interlocutorEquals = false;
                interlocutorReferenceEquals = false;
            }
            else
            {
                interlocutorEquals = interlocutor.Equals(anotherTalkingWith.interlocutor);
                interlocutorReferenceEquals = object.ReferenceEquals(interlocutor, anotherTalkingWith.interlocutor);
            }

            bool talkingGlobal = talkingEquals || talkingReferenceEquals;
            bool interlocutorGlobal = interlocutorEquals || interlocutorReferenceEquals;

            bool equal = talkingGlobal && interlocutorGlobal;

            return equal;
        }

        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

        /// <summary>
        /// Calculates and returns the hash code of this instance of the TalkingWith.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            hashcode = hashcode * 42 + talking.GetHashCode();
            if (interlocutor != null)
            {
                interlocutor.ClearHashCode();
                hashcode = hashcode * 42 + interlocutor.GetHashCode();
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
