using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class Constraint
    {
        // Agents constraints section
        public bool temporaryInvulnerability;
        public bool permanentInvulnerability;

        public Agent targetAgent = null;
        public int termOfProtection = 0;

        public Constraint(bool temporaryInvulnerability, bool permanentInvulnerability)
        {
            this.temporaryInvulnerability = temporaryInvulnerability;
            this.permanentInvulnerability = permanentInvulnerability;
        }

        public Constraint(bool temporaryInvulnerability, bool permanentInvulnerability, Agent targetAgent)
        {
            this.temporaryInvulnerability = temporaryInvulnerability;
            this.permanentInvulnerability = permanentInvulnerability;
            this.targetAgent = targetAgent;
        }

        public Constraint(bool temporaryInvulnerability, bool permanentInvulnerability, Agent targetAgent, int termOfProtection)
        {
            this.temporaryInvulnerability = temporaryInvulnerability;
            this.permanentInvulnerability = permanentInvulnerability;
            this.targetAgent = targetAgent;
            this.termOfProtection = termOfProtection;
        }

        public bool Control(World state)
        {
            if (temporaryInvulnerability && targetAgent != null && termOfProtection != 0)
            {
                if ((targetAgent.GetStatus() && state.GetTurnNumber() <= termOfProtection) || state.GetTurnNumber() > termOfProtection)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}
