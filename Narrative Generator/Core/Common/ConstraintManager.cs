using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class ConstraintManager
    {
        private List<WorldConstraint> constraints = new List<WorldConstraint>();

        /// <summary>
        /// Adds the specified constraint to the constraint list.
        /// </summary>
        public void AddConstraint (WorldConstraint constraint) { constraints.Add(constraint); }

        public List<WorldConstraint> GetConstraints() { return constraints; }

        public void SetConstraints (List<WorldConstraint> constraints) { this.constraints = constraints; }

        public void CreateRestrictingLocationConstraint (bool temporaryTimeRestricting,
                                                         bool permanentTimeRestricting,
                                                         bool waitingAnotherAgent,
                                                         bool revisitBan,
                                                         bool waitingAnotherAction,
                                                         bool locationsBan,
                                                         bool questsCounterRestricting,
                                                         bool restrictionOfAccessByLocations,
                                                         bool doNotMove,
                                                         bool mustVisitLeastOne,
                                                         bool visitOnlyOneOf,
                                                         HashSet<LocationStatic> targetLocations,
                                                         HashSet<AgentStateStatic> targetAgents,
                                                         int value)
        {
            RestrictingLocationAvailability newConstraint = new RestrictingLocationAvailability(temporaryTimeRestricting, permanentTimeRestricting, waitingAnotherAgent,
                                                                 revisitBan, waitingAnotherAction, locationsBan, questsCounterRestricting, restrictionOfAccessByLocations,
                                                                 doNotMove, mustVisitLeastOne, visitOnlyOneOf, targetLocations, targetAgents, value);

            AddConstraint(newConstraint);
        }

        public void CreateActionRestrictingConstraint (bool mutuallyExclusiveActions,
                                                       bool onlyOneFire,
                                                       bool onlyOnceInLocation,
                                                       PlanAction mainAction,
                                                       List<PlanAction> targetActions,
                                                       int timer)
        {
            ActionsRestricting newConstraint = new ActionsRestricting(mutuallyExclusiveActions, onlyOneFire, onlyOnceInLocation, mainAction, targetActions, timer);
            AddConstraint(newConstraint);
        }

        public void CreateAliveConstraint (bool temporaryInvulnerability,
                                           bool permanentInvulnerability,
                                           bool endIfDied,
                                           AgentStateStatic targetAgent,
                                           int termOfProtection)
        {
            ConstraintAlive newConstraint = new ConstraintAlive(temporaryInvulnerability, permanentInvulnerability, endIfDied, targetAgent, termOfProtection);
            AddConstraint(newConstraint);
        }
    }
}
