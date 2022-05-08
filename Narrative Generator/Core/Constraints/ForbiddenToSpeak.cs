using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class ForbiddenToSpeak: WorldConstraint
    {
        public bool temporaryRestricting;
        public bool permanentRestricting;
        public Dictionary<AgentStateStatic, AgentStateDynamic> targetAgents;
        public int termOfRestricting;

        public ForbiddenToSpeak(bool temporaryRestricting,
                                bool permanentRestricting,
                                Dictionary<AgentStateStatic, AgentStateDynamic> targetAgents,
                                int termOfRestricting)
        {
            this.temporaryRestricting = temporaryRestricting;
            this.permanentRestricting = permanentRestricting;
            this.targetAgents = targetAgents;
            this.termOfRestricting = termOfRestricting;
        }

        public void ChangeTermOfRestricting (int newTerm)
        {
            this.termOfRestricting = newTerm;
        }

        public override bool IsSatisfied (WorldDynamic newState, 
                                          WorldDynamic currentState, 
                                          StoryGraph graph, 
                                          PlanAction currentAction, 
                                          StoryNode currentNode)
        {
            foreach (var targetAgent in targetAgents)
            {
                if (temporaryRestricting && !permanentRestricting && targetAgent.Key != null && targetAgent.Value != null && termOfRestricting != 0)
                {
                    if (targetAgent.Value.GetPlan().GetAction(0) is Talk && newState.GetStaticWorldPart().GetTurnNumber() <= termOfRestricting)
                    {
                        return false;
                    }
                }
                else if (permanentRestricting && !temporaryRestricting && targetAgent.Key != null && targetAgent.Value != null)
                {
                    if (targetAgent.Value.GetPlan().GetAction(0) is Talk) { return false; }
                }
            }

            return true;
        }
    }
}
