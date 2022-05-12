using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class RestrictingLocationAvailability : WorldConstraint
    {
        public bool temporaryTimeRestricting;
        public bool permanentTimeRestricting;
        public bool waitingAnotherAgent;
        public bool revisitBan;
        public bool waitingAnotherAction;
        public bool locationsBan;
        public bool questsCounterRestricting;
        public bool restrictionOfAccessByLocations;
        public bool doNotMove;
        public HashSet<LocationStatic> targetLocations;
        public HashSet<AgentStateStatic> targetAgents;
        public int value;

        public RestrictingLocationAvailability(bool temporaryTimeRestricting, 
                                               bool permanentTimeRestricting,
                                               bool waitingAnotherAgent,
                                               bool revisitBan,
                                               bool waitingAnotherAction,
                                               bool locationsBan,
                                               bool questsCounterRestricting,
                                               bool restrictionOfAccessByLocations,
                                               bool doNotMove,
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
            this.targetLocations = targetLocations;
            this.targetAgents = targetAgents;
            this.value = value;
        }

        public void ChangeTermOfTimeRestricting (int newTerm)
        {
            this.value = newTerm;
        }

        public override bool IsSatisfied (WorldDynamic newState, 
                                          WorldDynamic currentState, 
                                          StoryGraph graph, 
                                          PlanAction currentAction, 
                                          StoryNode currentNode)
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
                                if (((Move)testNode.GetEdges().First().GetAction()).From.Key.Equals(((Move)currentAction).To.Key))
                                {
                                    return false;
                                }
                            }
                            else if (testNode.GetEdges().First().GetAction() is Move && currentAction is CounterMove)
                            {
                                if (((Move)testNode.GetEdges().First().GetAction()).From.Key.Equals(((CounterMove)currentAction).To.Key))
                                {
                                    return false;
                                }
                            }
                            else if (testNode.GetEdges().First().GetAction() is CounterMove && currentAction is CounterMove)
                            {
                                if (((CounterMove)testNode.GetEdges().First().GetAction()).From.Key.Equals(((CounterMove)currentAction).To.Key))
                                {
                                    return false;
                                }
                            }
                            else if (testNode.GetEdges().First().GetAction() is CounterMove && currentAction is Move)
                            {
                                if (((CounterMove)testNode.GetEdges().First().GetAction()).From.Key.Equals(((Move)currentAction).To.Key))
                                {
                                    return false;
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

            return true;
        }
    }
}
