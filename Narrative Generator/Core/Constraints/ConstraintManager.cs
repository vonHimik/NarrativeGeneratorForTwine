using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// The class that manages the constraints.
    /// </summary>
    class ConstraintManager
    {
        /// <summary>
        /// List of active constraints.
        /// </summary>
        private List<WorldConstraint> constraints = new List<WorldConstraint>();

        /// <summary>
        /// Adds the specified constraint to the constraint list.
        /// </summary>
        /// <param name="constraint">Specific instance of constraint.</param>
        public void AddConstraint (WorldConstraint constraint) { constraints.Add(constraint); }

        /// <summary>
        /// Returns a list of active constraints.
        /// </summary>
        /// <returns>List of active constraints.</returns>
        public List<WorldConstraint> GetConstraints() { return constraints; }

        /// <summary>
        /// Replaces the list of active constraints another specified list.
        /// </summary>
        /// <param name="constraints">List of constraints.</param>
        public void SetConstraints (List<WorldConstraint> constraints) { this.constraints = constraints; }

        /// <summary>
        /// A method for creating an instance of a constraint on the availability of locations.
        /// </summary>
        /// <param name="temporaryTimeRestricting">If true, sets a temporary restriction on access to the specified location.</param>
        /// <param name="permanentTimeRestricting">If true, sets a permanent restriction on access to the specified location.</param>
        /// <param name="waitingAnotherAgent">If true, then the first specified target agent will not be able to enter the specified location 
        ///                                      until the second specified target agent is located there.</param>
        /// <param name="revisitBan">If true, then prohibits re-visiting the location.</param>
        /// <param name="waitingAnotherAction">If true, prevents leaving the location until some other action is taken.</param>
        /// <param name="locationsBan">If true, then prohibits visiting the specified location.</param>
        /// <param name="questsCounterRestricting">If true, then prohibits entry to the location until the counter of completed quests matches the specified number.</param>
        /// <param name="restrictionOfAccessByLocations">If true, limits the number of times the target agent moves between target locations.</param>
        /// <param name="doNotMove">If true, then prohibits the specified agent from moving between locations.</param>
        /// <param name="mustVisitLeastOne">If true, then the specified agent must visit the specified location at least once.</param>
        /// <param name="visitOnlyOneOf">If true, then the specified agent will be able to visit the specified location only once.</param>
        /// <param name="targetLocations">List of locations to which the specified constraint applies.</param>
        /// <param name="targetAgents">List of agents to which the specified constraint applies.</param>
        /// <param name="value">Numeric parameter, for those constraints where it is required.</param>
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

        /// <summary>
        /// A method for creating an instance of a constraint on the possibility of performing certain actions or their result.
        /// </summary>
        /// <param name="mutuallyExclusiveActions">If true, then actions that cannot be performed sequentially.</param>
        /// <param name="onlyOneFire">If true, then an action can be performed only once.</param>
        /// <param name="onlyOnceInLocation">If true, then in the specified location, the specified action can be performed only once.</param>
        /// <param name="mainAction">The main variable that stores the action with which the main manipulations are carried out.</param>
        /// <param name="targetActions">A variable that stores additional actions.</param>
        /// <param name="timer">Timer value required for some types of restrictions.</param>
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

        /// <summary>
        /// A method for creating an instance of a constraint imposed by story on the lifetime of an agent.
        /// </summary>
        /// <param name="temporaryInvulnerability">If true, protects the agent from changing their status to deads, it is required to set a time limit.</param>
        /// <param name="permanentInvulnerability">If true, protects the agent from changing their status to dead, without time limit.</param>
        /// <param name="endIfDied">If true, then an instance of this class will monitor the state of the specified agent and return false if it dies.</param>
        /// <param name="targetAgent">Information about the agent to which this constraint will apply.</param>
        /// <param name="termOfProtection">The period for applying this restriction, in turns.</param>
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
