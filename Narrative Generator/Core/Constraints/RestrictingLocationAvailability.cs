using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that implements constraints on movement around locations.
    /// </summary>
    public class RestrictingLocationAvailability : WorldConstraint
    {
        /// <summary>
        /// If true, sets a temporary restriction on access to the specified location.
        /// </summary>
        public bool temporaryTimeRestricting;
        /// <summary>
        /// If true, sets a permanent restriction on access to the specified location.
        /// </summary>
        public bool permanentTimeRestricting;
        /// <summary>
        /// If true, then the first specified target agent will not be able to enter the specified location until the second specified target agent is located there.
        /// </summary>
        public bool waitingAnotherAgent;
        /// <summary>
        /// If true, then prohibits re-visiting the location.
        /// </summary>
        public bool revisitBan;
        /// <summary>
        /// If true, prevents leaving the location until some other action is taken.
        /// </summary>
        public bool waitingAnotherAction;
        /// <summary>
        /// If true, then prohibits visiting the specified location.
        /// </summary>
        public bool locationsBan;
        /// <summary>
        /// If true, then prohibits entry to the location until the counter of completed quests matches the specified number.
        /// </summary>
        public bool questsCounterRestricting;
        /// <summary>
        /// If true, limits the number of times the target agent moves between target locations.
        /// </summary>
        public bool restrictionOfAccessByLocations;
        /// <summary>
        /// If true, then prohibits the specified agent from moving between locations.
        /// </summary>
        public bool doNotMove;
        /// <summary>
        /// If true, then the specified agent must visit the specified location at least once.
        /// </summary>
        public bool mustVisitLeastOne;
        /// <summary>
        /// If true, then the specified agent will be able to visit the specified location only once.
        /// </summary>
        public bool visitOnlyOneOf;
        /// <summary>
        /// List of locations to which the specified constraint applies.
        /// </summary>
        public HashSet<LocationStatic> targetLocations;
        /// <summary>
        /// List of agents to which the specified constraint applies.
        /// </summary>
        public HashSet<AgentStateStatic> targetAgents;
        /// <summary>
        /// Numeric parameter, for those constraints where it is required.
        /// </summary>
        public int value;

        /// <summary>
        /// A separate instance of the goal manager.
        /// </summary>
        private GoalManager goalManager = new GoalManager();

        /// <summary>
        /// The conditional constructor of the given constraint (there is no unconditional constructor).
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
        public RestrictingLocationAvailability(bool temporaryTimeRestricting, 
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
            this.temporaryTimeRestricting = temporaryTimeRestricting;
            this.permanentTimeRestricting = permanentTimeRestricting;
            this.waitingAnotherAgent = waitingAnotherAgent;
            this.revisitBan = revisitBan;
            this.waitingAnotherAction = waitingAnotherAction;
            this.locationsBan = locationsBan;
            this.questsCounterRestricting = questsCounterRestricting;
            this.restrictionOfAccessByLocations = restrictionOfAccessByLocations;
            this.doNotMove = doNotMove;
            this.mustVisitLeastOne = mustVisitLeastOne;
            this.visitOnlyOneOf = visitOnlyOneOf;
            this.targetLocations = targetLocations;
            this.targetAgents = targetAgents;
            this.value = value;
        }

        /// <summary>
        /// The method that changes the duration of the given constraint.
        /// </summary>
        /// <param name="newTerm">The numerical value of the new constraint period, in turns.</param>
        public void ChangeTermOfTimeRestricting (int newTerm)
        {
            this.value = newTerm;
        }

        /// <summary>
        /// Overloading a method that checks whether the conditions of the specified constraints are met.
        /// </summary>
        /// <param name="newState">The new state that will be obtained when applying the action on the current state.</param>
        /// <param name="currentState">The current state of the storyworld.</param>
        /// <param name="graph">Story graph.</param>
        /// <param name="currentAction">The action that the current agent has chosen to perform on the current turn.</param>
        /// <param name="currentNode">A graph node that stores the current state.</param>
        /// <param name="newNode">A graph node that stores the future state.</param>
        /// <returns>Returns the result of the check.</returns>
        public override bool IsSatisfied (WorldDynamic newState, 
                                          WorldDynamic currentState, 
                                          StoryGraph graph, 
                                          PlanAction currentAction, 
                                          StoryNode currentNode,
                                          StoryNode newNode)
        {
            if (temporaryTimeRestricting && value != 0)
            {
                if (targetAgents != null && targetLocations != null)
                {
                    foreach (var targetAgent in targetAgents)
                    {
                        foreach (var targetLocation in targetLocations)
                        {
                            if ((newState.GetLocation(targetLocation).Value.SearchAgent(targetAgent)
                                   && newState.GetStaticWorldPart().GetTurnNumber() <= value))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            else if (permanentTimeRestricting)
            {
                foreach (var targetAgent in targetAgents)
                {
                    foreach (var targetLocation in targetLocations)
                    {
                        if (newState.GetLocation(targetLocation).Value.SearchAgent(targetAgent)) { return false; }
                    }
                }
            }
            else if (waitingAnotherAgent)
            {
                if ((newState.GetLocation(targetLocations.ElementAt(0)).Value.SearchAgent(targetAgents.ElementAt(0))
                    && newState.GetLocation(targetLocations.ElementAt(1)).Value.SearchAgent(targetAgents.ElementAt(1))) ||
                    (newState.GetLocation(targetLocations.ElementAt(0)).Value.SearchAgent(targetAgents.ElementAt(0))
                    && newState.GetLocation(targetLocations.ElementAt(0)).Value.SearchAgent(targetAgents.ElementAt(1))) ||
                    (!newState.GetLocation(targetLocations.ElementAt(0)).Value.SearchAgent(targetAgents.ElementAt(0))
                    && newState.GetLocation(targetLocations.ElementAt(1)).Value.SearchAgent(targetAgents.ElementAt(1))))
                {
                    return true;
                }
                else { return false; }
            }
            else if (revisitBan)
            {
                foreach (var targetAgent in targetAgents)
                {
                    if ((currentAction is Move || currentAction is CounterMove)
                    && ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)currentAction.Arguments[0]).Key.Equals(targetAgent))
                    {
                        StoryNode testNode = currentNode;

                        while (testNode.GetNumberInSequence() != 0)
                        {
                            if (testNode.GetEdges().First().GetAction() is Move && currentAction is Move)
                            {
                                if (targetLocations == null && 
                                    ((Move)testNode.GetEdges().First().GetAction()).From.Key.Equals(((Move)currentAction).To.Key))
                                {
                                    return false;
                                }
                                else if (!(targetLocations == null) &&
                                    ((Move)testNode.GetEdges().First().GetAction()).From.Key.Equals(((Move)currentAction).To.Key))
                                {
                                    foreach (var location in targetLocations)
                                    {
                                        if (location.Equals(((Move)currentAction).To.Key))
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                            else if (testNode.GetEdges().First().GetAction() is Move && currentAction is CounterMove)
                            {
                                if (targetLocations == null && 
                                    ((Move)testNode.GetEdges().First().GetAction()).From.Key.Equals(((CounterMove)currentAction).To.Key))
                                {
                                    return false;
                                }
                                else if (!(targetLocations == null) &&
                                    ((Move)testNode.GetEdges().First().GetAction()).From.Key.Equals(((CounterMove)currentAction).To.Key))
                                {
                                    foreach (var location in targetLocations)
                                    {
                                        if (location.Equals(((CounterMove)currentAction).To.Key))
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                            else if (testNode.GetEdges().First().GetAction() is CounterMove && currentAction is CounterMove)
                            {
                                if (targetLocations == null && 
                                    ((CounterMove)testNode.GetEdges().First().GetAction()).From.Key.Equals(((CounterMove)currentAction).To.Key))
                                {
                                    return false;
                                }
                                else if (!(targetLocations == null) &&
                                    ((CounterMove)testNode.GetEdges().First().GetAction()).From.Key.Equals(((CounterMove)currentAction).To.Key))
                                {
                                    foreach (var location in targetLocations)
                                    {
                                        if (location.Equals(((CounterMove)currentAction).To.Key))
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                            else if (testNode.GetEdges().First().GetAction() is CounterMove && currentAction is Move)
                            {
                                if (targetLocations == null && 
                                    ((CounterMove)testNode.GetEdges().First().GetAction()).From.Key.Equals(((Move)currentAction).To.Key))
                                {
                                    return false;
                                }
                                else if (!(targetLocations == null) &&
                                    ((CounterMove)testNode.GetEdges().First().GetAction()).From.Key.Equals(((Move)currentAction).To.Key))
                                {
                                    foreach (var location in targetLocations)
                                    {
                                        if (location.Equals(((Move)currentAction).To.Key))
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }

                            testNode = testNode.GetEdges().First().GetUpperNode();
                        }
                    }
                }
            }
            else if (waitingAnotherAction)
            {
                foreach (var location in targetLocations)
                {
                    foreach (var agent in targetAgents)
                    {
                        if (currentState.SearchAgentAmongLocations(agent).Equals(location))
                        {
                            if (currentAction is Move || currentAction is CounterMove)
                            {
                                StoryNode testNode = currentNode;

                                while (testNode.GetNumberInSequence() != 0)
                                {
                                    if (((KeyValuePair<AgentStateStatic, AgentStateDynamic>)testNode.GetEdges().First().GetAction().Arguments[0]).Key.
                                           Equals(agent))
                                    {
                                        if (testNode.GetEdges().First().GetAction() is Move || testNode.GetEdges().First().GetAction() is CounterMove)
                                        {
                                            return false;
                                        }
                                        else
                                        {
                                            return true;
                                        }
                                    }

                                    testNode = testNode.GetEdges().First().GetUpperNode();
                                }
                            }
                        }
                    }
                }
            }
            else if (locationsBan)
            {
                foreach (var targetLocation in targetLocations)
                {
                    if (newState.SearchAgentAmongLocations(targetAgents.First()).Equals(targetLocation))
                    {
                        return false;
                    }
                }
            }
            else if (questsCounterRestricting)
            {
                if (newState.SearchAgentAmongLocations(targetAgents.First()).Equals(targetLocations.First())
                    && newState.GetAgentByName(targetAgents.First().GetName()).Value.GetComplitedQuestsCounter() < value)
                {
                    return false;
                }
            }
            else if (restrictionOfAccessByLocations)
            {
                foreach (var targetAgent in targetAgents)
                {
                    int falseCounter = 0;

                    foreach (var targetLocation in targetLocations)
                    {
                        if (!(newState.SearchAgentAmongLocations(targetAgent).Equals(targetLocation)))
                        {
                            falseCounter++;
                        }
                    }

                    if (falseCounter == targetLocations.Count) { return false; }
                }
            }
            else if (doNotMove)
            {
                foreach (var targetAgent in targetAgents)
                {
                    if ((currentAction is Move || currentAction is CounterMove)
                    && ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)currentAction.Arguments[0]).Key.Equals(targetAgent))
                    {
                        return false;
                    }
                }
            }
            else if (mustVisitLeastOne)
            {
                if (goalManager.ControlToAchieveGoalState(ref newNode))
                {
                    StoryNode testNode = currentNode;

                    while (testNode.GetNumberInSequence() != 0)
                    {
                        foreach (var agent in targetAgents)
                        {
                            foreach (var location in targetLocations)
                            {
                                foreach (var edge in testNode.GetEdges())
                                {
                                    if (((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.Equals(agent) 
                                        && (edge.GetAction() is Move || edge.GetAction() is CounterMove))
                                    {
                                        if (((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[1]).Key.Equals(location))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }

                        testNode = testNode.GetEdges().First().GetUpperNode();
                    }

                    return false;

                }
            }
            else if (visitOnlyOneOf)
            {
                StoryNode testNode = currentNode;

                while (testNode.GetNumberInSequence() != 0)
                {
                    foreach (var agent in targetAgents)
                    {
                        foreach (var location in targetLocations)
                        {
                            foreach (var edge in testNode.GetEdges())
                            {
                                if ((edge.GetAction() is Move || edge.GetAction() is CounterMove) 
                                    && ((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.Equals(agent)
                                    && ((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[1]).Key.Equals(location)
                                    && (currentAction is Move || currentAction is CounterMove))
                                {
                                    foreach (var location2 in targetLocations)
                                    {
                                        if (((KeyValuePair<LocationStatic, LocationDynamic>)edge.GetAction().Arguments[2]).Key.Equals(location2))
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    testNode = testNode.GetEdges().First().GetUpperNode();
                }
            }

            return true;
        }
    }
}
