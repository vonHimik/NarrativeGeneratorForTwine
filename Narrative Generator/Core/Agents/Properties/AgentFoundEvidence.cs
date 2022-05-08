using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    public class AgentFoundEvidence : AgentProperty, ICloneable, IEquatable<AgentFoundEvidence>
    {
        private bool foundEvidence;
        private AgentStateStatic evidenceAgainst;

        private bool hasHashCode;
        private int hashCode;

        public AgentFoundEvidence()
        {
            foundEvidence = false;
            evidenceAgainst = null;
            hasHashCode = false;
            hashCode = 0;
        }

        public AgentFoundEvidence (AgentFoundEvidence clone)
        {
            foundEvidence = clone.foundEvidence;
            if (clone.evidenceAgainst != null) { evidenceAgainst = (AgentStateStatic)clone.evidenceAgainst.Clone(); }
            else { evidenceAgainst = new AgentStateStatic(); }
            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
        }

        public AgentFoundEvidence(bool foundEvidence, AgentStateStatic evidenceAgainst)
        {
            this.foundEvidence = foundEvidence;
            this.evidenceAgainst = evidenceAgainst;
            hasHashCode = false;
            hashCode = 0;
        }

        public object Clone()
        {
            var clone = new AgentFoundEvidence();

            clone.foundEvidence = foundEvidence;
            if (evidenceAgainst != null) { clone.evidenceAgainst = new AgentStateStatic(evidenceAgainst); }

            return clone;
        }

        public void IsEvidence()
        {
            foundEvidence = true;
            UpdateHashCode();
        }

        public void NoEvidence()
        {
            foundEvidence = false;
            UpdateHashCode();
        }

        public bool CheckEvidence() { return foundEvidence; }

        public void SetCriminal(AgentStateStatic criminal)
        {
            evidenceAgainst = criminal;
            UpdateHashCode();
        }

        public AgentStateStatic GetCriminal() { return evidenceAgainst; }

        public void Clear()
        {
            foundEvidence = false;
            evidenceAgainst = null;
            UpdateHashCode();
        }

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

        public void ClearHashCode()
        {
            hasHashCode = false;
            hashCode = 0;
        }

        public void UpdateHashCode()
        {
            ClearHashCode();
            GetHashCode();
        }
    }
}
