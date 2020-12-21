using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class ConstraintAlive : WorldConstraint
    {
        public bool temporaryInvulnerability;
        public bool permanentInvulnerability;

        public Agent targetAgent = null;
        public int termOfProtection = 0;

        public ConstraintAlive(bool temporaryInvulnerability, bool permanentInvulnerability)
        {
            this.temporaryInvulnerability = temporaryInvulnerability;
            this.permanentInvulnerability = permanentInvulnerability;
        }

        public ConstraintAlive(bool temporaryInvulnerability, bool permanentInvulnerability, Agent targetAgent)
        {
            this.temporaryInvulnerability = temporaryInvulnerability;
            this.permanentInvulnerability = permanentInvulnerability;
            this.targetAgent = targetAgent;
        }

        public ConstraintAlive(bool temporaryInvulnerability, bool permanentInvulnerability, Agent targetAgent, int termOfProtection)
        {
            this.temporaryInvulnerability = temporaryInvulnerability;
            this.permanentInvulnerability = permanentInvulnerability;
            this.targetAgent = targetAgent;
            this.termOfProtection = termOfProtection;
        }

        public override bool IsSatisfied(WorldBeliefs state)
        {
            if (temporaryInvulnerability && targetAgent != null && termOfProtection != 0)
            {
                return ((targetAgent.GetStatus() && state.GetStaticWorldPart().GetTurnNumber() <= termOfProtection) || state.GetStaticWorldPart().GetTurnNumber() > termOfProtection);
            }

            return false;
        }
    }
}
