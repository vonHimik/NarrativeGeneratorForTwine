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
        public int termOfProtection = 0;

        public ConstraintAlive(bool temporaryInvulnerability, bool permanentInvulnerability)
        {
            this.temporaryInvulnerability = temporaryInvulnerability;
            this.permanentInvulnerability = permanentInvulnerability;
        }

        public ConstraintAlive(bool temporaryInvulnerability, 
                               bool permanentInvulnerability, 
                               KeyValuePair<AgentStateStatic, AgentStateDynamic> targetAgent)
        {
            this.temporaryInvulnerability = temporaryInvulnerability;
            this.permanentInvulnerability = permanentInvulnerability;
            this.targetAgent = targetAgent;
        }

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

        public override bool IsSatisfied(WorldBeliefs state)
        {
            if (temporaryInvulnerability && targetAgent.Key != null && targetAgent.Value != null && termOfProtection != 0)
            {
                return ((targetAgent.Value.GetStatus() && state.GetStaticWorldPart().GetTurnNumber() <= termOfProtection) || state.GetStaticWorldPart().GetTurnNumber() > termOfProtection);
            }

            return false;
        }
    }
}
