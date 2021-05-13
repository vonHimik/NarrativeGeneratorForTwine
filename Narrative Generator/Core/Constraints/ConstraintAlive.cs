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
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> targetAgent;
        public int termOfProtection;

        public ConstraintAlive(bool temporaryInvulnerability, 
                               bool permanentInvulnerability, 
                               KeyValuePair<AgentStateStatic, AgentStateDynamic> targetAgent, 
                               int termOfProtection)
        {
            this.temporaryInvulnerability = temporaryInvulnerability;
            this.permanentInvulnerability = permanentInvulnerability;
            this.targetAgent = targetAgent;
            this.termOfProtection = termOfProtection;
        }

        public void ChangeTermOfProtection(int newTerm)
        {
            this.termOfProtection = newTerm;
        }

        public override bool IsSatisfied(WorldDynamic state)
        {
            if (temporaryInvulnerability && !permanentInvulnerability && targetAgent.Key != null && targetAgent.Value != null && termOfProtection != 0)
            {
                return ((targetAgent.Value.GetStatus() && state.GetStaticWorldPart().GetTurnNumber() <= termOfProtection) || state.GetStaticWorldPart().GetTurnNumber() > termOfProtection);
            }
            else if (permanentInvulnerability && !temporaryInvulnerability && targetAgent.Key != null && targetAgent.Value != null)
            {
                return (targetAgent.Value.GetStatus());
            }

            return false;
        }
    }
}
