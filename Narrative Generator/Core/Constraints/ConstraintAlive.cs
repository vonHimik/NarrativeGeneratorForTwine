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
        public AgentStateStatic targetAgent;
        public int termOfProtection;

        public ConstraintAlive(bool temporaryInvulnerability, 
                               bool permanentInvulnerability, 
                               AgentStateStatic targetAgent, 
                               int termOfProtection)
        {
            this.temporaryInvulnerability = temporaryInvulnerability;
            this.permanentInvulnerability = permanentInvulnerability;
            this.targetAgent = targetAgent;
            this.termOfProtection = termOfProtection;
        }

        public void ChangeTermOfProtection (int newTerm)
        {
            this.termOfProtection = newTerm;
        }

        public override bool IsSatisfied (WorldDynamic newState, 
                                          WorldDynamic currentState, 
                                          StoryGraph graph, 
                                          PlanAction currentAction, 
                                          StoryNode currentNode,
                                          StoryNode newNode)
        {
            if (temporaryInvulnerability && !permanentInvulnerability && targetAgent != null && termOfProtection != 0)
            {
                return ((newState.GetAgentByName(targetAgent.GetName()).Value.GetStatus() 
                        && newState.GetStaticWorldPart().GetTurnNumber() <= termOfProtection) 
                        || newState.GetStaticWorldPart().GetTurnNumber() > termOfProtection);

            }
            else if (permanentInvulnerability && !temporaryInvulnerability && targetAgent != null)
            {
                return (newState.GetAgentByName(targetAgent.GetName()).Value.GetStatus());
            }

            return true;
        }
    }
}
