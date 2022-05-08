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
                /*foreach (var node in graph.GetNodes())
                {
                    foreach (var edge in node.GetEdges())
                    {
                        if (edge.GetAction() is Move && ((Move)edge.GetAction()).Agent.Key.Equals(targetAgents.First()))
                        {
                            if (((Move)edge.GetAction()).From.Key.Equals(state.SearchAgentAmongLocations(targetAgents.First())))
                            {
                                return false;
                            }
                        }
                    }
                }*/

                if (currentAction is Move || currentAction is CounterMove)
                {
                    StoryNode testNode = currentNode;

                    while (testNode.GetNumberInSequence() != 0)
                    {
                        if (testNode.GetEdges().First().GetAction() is Move || testNode.GetEdges().First().GetAction() is CounterMove)
                        {
                            if (((Move)testNode.GetEdges().First().GetAction()).From.Key.Equals(((Move)currentAction).To.Key))
                            {
                                return false;
                            }
                        }

                        testNode = testNode.GetEdges().First().GetUpperNode();
                    }
                }
            }
            else if (waitingAnotherAction)
            {
                /*HashSet<StoryNode> newNodesList = graph.GetNodes().Reverse().ToHashSet();

                foreach (var node in newNodesList)
                {
                    HashSet<Edge> newEdgesList = node.GetEdges().Reverse().ToHashSet();

                    foreach (var edge in newEdgesList)
                    {
                        if (((KeyValuePair<AgentStateStatic, AgentStateDynamic>)edge.GetAction().Arguments[0]).Key.Equals(targetAgents.First()))
                        {
                            if (edge.GetAction() is Move)
                            {
                                return ((Move)edge.GetAction()).To.Key.Equals(state.SearchAgentAmongLocations(targetAgents.First()));
                            }
                        }
                    }
                }*/

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
                                           Equals(agent)) // Agent in action = Player
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

            return true;
        }
    }
}
