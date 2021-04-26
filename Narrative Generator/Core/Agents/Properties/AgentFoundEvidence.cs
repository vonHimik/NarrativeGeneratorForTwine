using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    public class AgentFoundEvidence : AgentProperty, ICloneable
    {
        private bool foundEvidence;
        private AgentStateStatic evidenceAgainst;

        public AgentFoundEvidence() {}

        public AgentFoundEvidence(bool foundEvidence, AgentStateStatic evidenceAgainst)
        {
            this.foundEvidence = foundEvidence;
            this.evidenceAgainst = evidenceAgainst;
        }

        public object Clone()
        {
            var clone = new AgentFoundEvidence();

            clone.foundEvidence = foundEvidence;
            clone.evidenceAgainst = (AgentStateStatic)evidenceAgainst.Clone();

            return clone;
        }

        public void IsEvidence()
        {
            foundEvidence = true;
        }

        public void NoEvidence()
        {
            foundEvidence = false;
        }

        public bool CheckEvidence()
        {
            return foundEvidence;
        }

        public void SetCriminal(AgentStateStatic criminal)
        {
            evidenceAgainst = criminal;
        }

        public AgentStateStatic GetCriminal()
        {
            return evidenceAgainst;
        }

        public void Clear()
        {
            foundEvidence = false;
            evidenceAgainst = null;
        }
    }
}
