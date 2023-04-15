using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that implements the knowledge and memory of an agent regarding the evidence found on other agents.
    /// </summary>
    [Serializable]
    public class AgentFoundEvidence : AgentProperty, ICloneable, IEquatable<AgentFoundEvidence>
    {
        // AgentFoundEvidence components
        /// <summary>
        /// An indicator of whether this agent behavior modifier is active.
        /// </summary>
        private bool foundEvidence;
        /// <summary>
        /// Another agent that is the target of this agent behavior modifier.
        /// </summary>
        private AgentStateStatic evidenceAgainst;

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
        public AgentFoundEvidence()
        {
            foundEvidence = false;
            evidenceAgainst = null;
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Constructor with parameters of the AgentFoundEvidence, which creates a new instance of the AgentFoundEvidence based on the passed clone.
        /// </summary>
        /// <param name="clone">An AgentFoundEvidence instance that will serve as the basis for creating a new instance.</param>
        public AgentFoundEvidence (AgentFoundEvidence clone)
        {
            foundEvidence = clone.foundEvidence;
            if (clone.evidenceAgainst != null) { evidenceAgainst = (AgentStateStatic)clone.evidenceAgainst.Clone(); }
            else { evidenceAgainst = new AgentStateStatic(); }
            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
        }

        /// <summary>
        /// A parameterized constructor that takes an indicator that a evidence has been found and information about the agent it points to.
        /// </summary>
        /// <param name="foundEvidence">Indicator that a evidence has been found.</param>
        /// <param name="evidenceAgainst">Information about the agent pointed to by the evidence.</param>
        public AgentFoundEvidence (bool foundEvidence, AgentStateStatic evidenceAgainst)
        {
            this.foundEvidence = foundEvidence;
            this.evidenceAgainst = evidenceAgainst;
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Method for cloning an AgentFoundEvidence instance.
        /// </summary>
        /// <returns>A new instance that is a copy of the current one.</returns>
        public object Clone()
        {
            var clone = new AgentFoundEvidence();

            clone.foundEvidence = foundEvidence;
            if (evidenceAgainst != null) { clone.evidenceAgainst = new AgentStateStatic(evidenceAgainst); }

            return clone;
        }

        /// <summary>
        /// Sets the status of clue detection to True.
        /// </summary>
        public void IsEvidence()
        {
            foundEvidence = true;
            UpdateHashCode();
        }

        /// <summary>
        /// Sets the status of clue detection to False.
        /// </summary>
        public void NoEvidence()
        {
            foundEvidence = false;
            UpdateHashCode();
        }

        /// <summary>
        /// Checks the status of finding evidence by this agent.
        /// </summary>
        /// <returns>True if evidence is found, false otherwise.</returns>
        public bool CheckEvidence() { return foundEvidence; }

        /// <summary>
        /// Sets information about the agent against which the evidence points.
        /// </summary>
        /// <param name="criminal">Information about the agent against which the evidence points</param>
        public void SetCriminal(AgentStateStatic criminal)
        {
            evidenceAgainst = criminal;
            UpdateHashCode();
        }

        /// <summary>
        /// Returns information about the agent against which the evidence points.
        /// </summary>
        /// <returns>Information about the agent against which the evidence points.</returns>
        public AgentStateStatic GetCriminal() { return evidenceAgainst; }

        /// <summary>
        /// Clears all information about the found evidence and the agent to which the evidence points (deletes it).
        /// </summary>
        public void Clear()
        {
            foundEvidence = false;
            evidenceAgainst = null;
            UpdateHashCode();
        }

        /// <summary>
        /// Method for comparing two AgentFoundEvidence instance.
        /// </summary>
        /// <param name="anotherAgentFoundEvidence">Another AgentFoundEvidence instance, for comparison.</param>
        /// <returns>True if both instance are the same, false otherwise.</returns>
        public bool Equals(AgentFoundEvidence anotherAgentFoundEvidence)
        {
            if (anotherAgentFoundEvidence == null) { return false; }

            bool foundEvidenceEquals = foundEvidence.Equals(anotherAgentFoundEvidence.foundEvidence);
            bool foundEvidenceReferenceEquals = object.ReferenceEquals(foundEvidence, anotherAgentFoundEvidence.foundEvidence);

            bool evidenceAgainstEquals;
            bool evidenceAgainstReferenceEquals;
            if (evidenceAgainst == null && anotherAgentFoundEvidence.evidenceAgainst == null)
            {
                evidenceAgainstEquals = true;
                evidenceAgainstReferenceEquals = true;
            }
            else if (evidenceAgainst == null && anotherAgentFoundEvidence.evidenceAgainst != null)
            {
                evidenceAgainstEquals = false;
                evidenceAgainstReferenceEquals = false;
            }
            else
            {
                evidenceAgainstEquals = evidenceAgainst.Equals(anotherAgentFoundEvidence.evidenceAgainst);
                evidenceAgainstReferenceEquals = object.ReferenceEquals(evidenceAgainst, anotherAgentFoundEvidence.evidenceAgainst);
            }

            bool foundEvidenceGlobal = foundEvidenceEquals || foundEvidenceReferenceEquals;
            bool evidenceAgainstGlobal = evidenceAgainstEquals || evidenceAgainstReferenceEquals;

            bool equal = foundEvidenceGlobal && evidenceAgainstGlobal;

            return equal;
        }

        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

        /// <summary>
        /// Calculates and returns the hash code of this instance of the AgentFoundEvidence.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            hashcode = hashcode * 42 + foundEvidence.GetHashCode();
            if (evidenceAgainst != null)
            {
                evidenceAgainst.ClearHashCode();
                hashcode = hashcode * 42 + evidenceAgainst.GetHashCode();
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
