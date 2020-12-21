using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class AgentFoundEvidence : AgentProperty
    {
        private bool foundEvidence;
        private Agent evidenceAgainst;

        public AgentFoundEvidence(bool foundEvidence, Agent evidenceAgainst)
        {
            this.foundEvidence = foundEvidence;
            this.evidenceAgainst = evidenceAgainst;
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

        public void SetCriminal(Agent criminal)
        {
            evidenceAgainst = criminal;
        }

        public Agent GetCriminal()
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
