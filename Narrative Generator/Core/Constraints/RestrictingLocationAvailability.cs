using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class RestrictingLocationAvailability : WorldConstraint
    {
        public bool temporaryRestricting;
        public bool permanentRestricting;
        public KeyValuePair<LocationStatic, LocationDynamic> targetLocation;
        public Dictionary<AgentStateStatic, AgentStateDynamic> targetAgents;
        public int termOfRestricting;

        public RestrictingLocationAvailability(bool temporaryRestricting, 
                                               bool permanentRestricting, 
                                               KeyValuePair<LocationStatic, LocationDynamic> targetLocation,
                                               Dictionary<AgentStateStatic, AgentStateDynamic> targetAgents,
                                               int termOfRestricting)
        {
            this.temporaryRestricting = temporaryRestricting;
            this.permanentRestricting = permanentRestricting;
            this.targetLocation = targetLocation;
            this.targetAgents = targetAgents;
            this.termOfRestricting = termOfRestricting;
        }

        public void ChangeTermOfRestricting(int newTerm)
        {
            this.termOfRestricting = newTerm;
        }

        public override bool IsSatisfied(WorldDynamic state)
        {
            foreach (var targetAgent in targetAgents)
            {
                if (temporaryRestricting && !permanentRestricting && targetAgent.Key != null && targetAgent.Value != null && termOfRestricting != 0 
                                         && targetLocation.Key != null && targetLocation.Value != null)
                {
                    if ((targetLocation.Value.SearchAgent(targetAgent.Key) && state.GetStaticWorldPart().GetTurnNumber() <= termOfRestricting))
                    {
                        return false;
                    }
                }
                else if (permanentRestricting && !temporaryRestricting && targetAgent.Key != null && targetAgent.Value != null 
                                              && targetLocation.Key != null && targetLocation.Value != null)
                {
                    if (targetLocation.Value.SearchAgent(targetAgent.Key)) { return false; }
                }
            }

            return true;
        }
    }
}
