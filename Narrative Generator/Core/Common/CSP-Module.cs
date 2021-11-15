using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class CSP_Module
    {
        public void AssignVariables(ref PlanAction action, WorldDynamic currentState, KeyValuePair<AgentStateStatic, AgentStateDynamic> initiator)
        {
            if (action is Entrap)
            {
                foreach (var agent in currentState.GetAgents())
                {
                    if ((agent.Key.GetRole() == AgentRole.USUAL || agent.Key.GetRole() == AgentRole.PLAYER) && agent.Value.GetStatus() 
                        && !currentState.GetLocation(currentState.SearchAgentAmongLocations(agent.Key)).SearchAgent(initiator.Key))
                    {
                        action.Arguments.Add(agent);
                        action.Arguments.Add(initiator); 
                        action.Arguments.Add(currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName()));
                        break;
                    }
                }
            }
            else if (action is Fight)
            {
                foreach (var agent in currentState.GetAgents())
                {
                    if ((agent.Key.GetRole() == AgentRole.USUAL || agent.Key.GetRole() == AgentRole.PLAYER) && agent.Value.GetStatus() 
                        && currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)).SearchAgent(agent.Key))
                    {
                        action.Arguments.Add(initiator);
                        action.Arguments.Add(agent);
                        action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
                        break;
                    }
                }
            }
            else if (action is InvestigateRoom)
            {
                foreach (var killer in currentState.GetAgents())
                {
                    if (killer.Key.GetRole() == AgentRole.KILLER && killer.Value.GetStatus())
                    {
                        action.Arguments.Add(initiator);
                        action.Arguments.Add(killer);
                        action.Arguments.Add(currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName()));
                        break;
                    }
                }
            }
            else if (action is Kill)
            {
                foreach (var agent in currentState.GetAgents())
                {
                    if ((agent.Key.GetRole() == AgentRole.USUAL || agent.Key.GetRole() == AgentRole.PLAYER) && agent.Value.GetStatus() 
                        && currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)).SearchAgent(agent.Key))
                    {
                        action.Arguments.Add(agent);
                        action.Arguments.Add(initiator);
                        action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
                        break;
                    }
                }
            }
            else if (action is Move)
            {
                if (action.Arguments.Count() != 0)
                {
                    List<string> arguments = new List<string>();

                    foreach (var argument in action.Arguments)
                    {
                        arguments.Add((string)argument);
                    }

                    action.Arguments.Clear();

                    action.Arguments.Add(initiator);
                    action.Arguments.Add(currentState.GetLocationByName(arguments[1]));
                    action.Arguments.Add(currentState.GetLocationByName(arguments[2]));
                }
                else
                {
                    action.Arguments.Add(initiator);
                    action.Arguments.Add(currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName()));

                    if (initiator.Value.GetTargetLocation() != null && initiator.Key.GetRole() != AgentRole.PLAYER &&
                        currentState.SearchAgentAmongLocations(initiator.Key).ConnectionChecking(initiator.Value.GetTargetLocation()))
                    {
                        action.Arguments.Add(currentState.GetLocationByName(initiator.Value.GetTargetLocation().GetName()));
                    }
                    else
                    {
                        KeyValuePair<LocationStatic, LocationDynamic> randLoc = currentState.
                          GetRandomConnectedLocation(currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName()));

                        if (currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName()).
                            Key.ConnectionChecking(randLoc.Key))
                        {
                            action.Arguments.Add(randLoc);
                        }
                    }
                }
            }
            else if (action is NeutralizeKiller)
            {
                foreach (var killer in currentState.GetAgents())
                {
                    if (killer.Key.GetRole() == AgentRole.KILLER && killer.Value.GetStatus())
                    {
                        action.Arguments.Add(initiator);
                        action.Arguments.Add(killer);
                        action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
                        break;
                    }
                }
            }
            else if (action is NothingToDo)
            {
                action.Arguments.Add(initiator);
            }
            else if (action is Reassure)
            {
                Dictionary<AgentStateStatic, AgentStateDynamic> excludedAgents = new Dictionary<AgentStateStatic, AgentStateDynamic>();
                excludedAgents.Add(initiator.Key, initiator.Value);

                action.Arguments.Add(initiator);

                foreach (var agent in currentState.GetAgents())
                {
                    if (agent.Key.GetRole() == AgentRole.USUAL && agent.Value.GetStatus()
                        && currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)).SearchAgent(agent.Key))
                    {
                        excludedAgents.Add(agent.Key, agent.Value);

                        action.Arguments.Add(agent);
                        break;
                    }
                }

                action.Arguments.Add(currentState.GetRandomAgent(excludedAgents));

                foreach (var killer in currentState.GetAgents())
                {
                    if (killer.Key.GetRole() == AgentRole.KILLER && killer.Value.GetStatus())
                    {
                        action.Arguments.Add(killer);
                        break;
                    }
                }

                action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
            }
            else if (action is Run)
            {
                action.Arguments.Add(initiator);
                action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
                action.Arguments.Add(currentState.
                        GetRandomLocationWithout(currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName())));
            }
            else if (action is TellAboutASuspicious)
            {
                foreach (var agent in currentState.GetAgents())
                {
                    if (agent.Key.GetRole() == AgentRole.USUAL && agent.Value.GetStatus() 
                        && currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)).SearchAgent(agent.Key))
                    {
                        action.Arguments.Add(agent);
                        break;
                    }
                }

                action.Arguments.Add(initiator);
                action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
                action.Arguments.Add(currentState.GetRandomLocationWithout
                    (currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName())));
            }
            else if (action is Talk)
            {
                action.Arguments.Add(initiator);

                foreach (var agent in currentState.GetAgents())
                {
                    if (agent.Key.GetRole() == AgentRole.USUAL && agent.Value.GetStatus()
                        && currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)).SearchAgent(agent.Key))
                    {
                        action.Arguments.Add(agent);
                        break;
                    }
                }

                // TO DO: Need to expand !!!
                action.Arguments.Add("topic");
            }
        }

        public List<PlanAction> MassiveAssignVariables(ref PlanAction action, 
                                           WorldDynamic currentState, 
                                           KeyValuePair<AgentStateStatic, AgentStateDynamic> initiator)
        {
            List<PlanAction> actions = new List<PlanAction>();

            if (action is Move)
            {
                int locationsCount = 
                    currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName()).Key.GetConnectedLocations().Count;

                List<Move> moveArr = new List<Move>();

                for (int i = 0; i < locationsCount; i++)
                {
                    Move move = new Move();
                    moveArr.Add(move);
                }

                for (int i = 0; i < locationsCount; i++)
                {
                    moveArr[i].Arguments.Add(initiator);
                    moveArr[i].Arguments.Add(currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName()));
                    moveArr[i].Arguments.Add(currentState.GetLocationByName
                        (currentState.GetLocationByName(
                            currentState.SearchAgentAmongLocations(initiator.Key).GetName()).Key.GetConnectedLocationsFromIndex(i).GetName()));
                }

                foreach (var a in moveArr)
                {
                    actions.Add(a);
                }
            }

            return actions;
        }
    }
}
