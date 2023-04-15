using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that implements the assignment of parameters to actions according to the CSP methodology.
    /// </summary>
    class CSP_Module
    {
        /// <summary>
        /// A method that assigns parameters to an action.
        /// </summary>
        /// <param name="action">The action to which the parameters are assigned.</param>
        /// <param name="currentState">The current world state.</param>
        /// <param name="initiator">The agent performing the action.</param>
        /// <param name="note">Text to display on the main screen.</param>
        /// <returns>True if the assignment was successful, false otherwise.</returns>
        public bool AssignVariables (ref PlanAction action, WorldDynamic currentState, KeyValuePair<AgentStateStatic, AgentStateDynamic> initiator, ref TextBox note)
        {
            note.Text = "ASSIGN VARIABLES FOR ACTION";

            if (action is Entrap || action is CounterEntrap)
            {
                foreach (var agent in currentState.GetAgents())
                {
                    if ((agent.Key.GetRole() == AgentRole.USUAL || agent.Key.GetRole() == AgentRole.PLAYER) && agent.Value.GetStatus() 
                        && !currentState.GetLocation(currentState.SearchAgentAmongLocations(agent.Key)).Value.SearchAgent(initiator.Key) 
                        && !agent.Equals(initiator))
                    {
                        action.Arguments.Add(agent);
                        action.Arguments.Add(initiator); 
                        action.Arguments.Add(currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName()));
                        return true;
                    }
                }
                return false;
            }
            else if (action is Fight || action is CounterFight)
            {
                foreach (var agent in currentState.GetAgents())
                {
                    if (agent.Value.GetStatus() && !agent.Equals(initiator)
                        && currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)).Value.SearchAgent(agent.Key) 
                        && initiator.Value.GetObjectOfAngryComponent().GetObjectOfAngry().Equals(agent.Key))
                    {
                        action.Arguments.Add(initiator);
                        action.Arguments.Add(agent);
                        action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
                        return true;
                    }
                }
                return false;
            }
            else if (action is InvestigateRoom || action is CounterInvestigateRoom)
            {
                foreach (var killer in currentState.GetAgents())
                {
                    if (killer.Key.GetRole() == AgentRole.ANTAGONIST && killer.Value.GetStatus())
                    {
                        action.Arguments.Add(initiator);
                        action.Arguments.Add(killer);
                        action.Arguments.Add(currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName()));
                        return true;
                    }
                }
                return false;
            }
            else if (action is Kill || action is CounterKill)
            {
                foreach (var agent in currentState.GetAgents())
                {
                    if ((agent.Key.GetRole() == AgentRole.USUAL || agent.Key.GetRole() == AgentRole.PLAYER) && agent.Value.GetStatus() 
                        && currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)).Value.SearchAgent(agent.Key))
                    {
                        action.Arguments.Add(agent);
                        action.Arguments.Add(initiator);
                        action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
                        return true;
                    }
                }
                return false;
            }
            else if (action is Move || action is CounterMove)
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
                    return true;
                }
                else
                {
                    action.Arguments.Add(initiator);
                    action.Arguments.Add(currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName()));

                    if (initiator.Value.GetTargetLocation() != null && initiator.Value.GetTargetLocation().GetName() != "" &&
                        initiator.Key.GetRole() != AgentRole.PLAYER /*&&
                        currentState.SearchAgentAmongLocations(initiator.Key).ConnectionChecking(initiator.Value.GetTargetLocation())*/
                        && !currentState.GetStaticWorldPart().GetSetting().Equals(Setting.DragonAge))
                    {
                        action.Arguments.Add(currentState.GetLocationByName(initiator.Value.GetTargetLocation().GetName()));
                        return true;
                    }
                    else if ((currentState.GetStaticWorldPart().GetSetting().Equals(Setting.Detective)
                          || currentState.GetStaticWorldPart().GetSetting().Equals(Setting.DefaultDemo))
                          && currentState.GetStaticWorldPart().GetConnectionStatus())
                    {
                        KeyValuePair<LocationStatic, LocationDynamic> randLoc = currentState.
                          GetRandomConnectedLocation(currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName()));

                        if (currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName()).
                            Key.ConnectionChecking(randLoc.Key))
                        {
                            action.Arguments.Add(randLoc);
                            return true;
                        }
                    }
                    else
                    {
                        KeyValuePair<LocationStatic, LocationDynamic> randLoc = currentState.
                          GetRandomLocationWithout(currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName()));

                        action.Arguments.Add(randLoc);
                        return true;
                    }
                }
                return false;
            }
            else if (action is NeutralizeKiller || action is CounterNeutralizeKiller)
            {
                foreach (var killer in currentState.GetAgents())
                {
                    if (killer.Key.GetRole() == AgentRole.ANTAGONIST && killer.Value.GetStatus())
                    {
                        action.Arguments.Add(initiator);
                        action.Arguments.Add(killer);
                        action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
                        return true;
                    }
                }
                return false;
            }
            else if (action is Reassure || action is CounterReassure)
            {
                foreach (var agent in currentState.GetAgents())
                {
                    if (agent.Key.GetRole() == AgentRole.USUAL && agent.Value.GetStatus()
                        && currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)).Value.SearchAgent(agent.Key)
                        && !agent.Key.Equals(initiator.Key) && agent.Value.AngryCheck())
                    {
                        action.Arguments.Add(agent);
                        action.Arguments.Add(initiator);
                        action.Arguments.Add(currentState.GetAgentByName(agent.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName()));
                        break;
                    }
                }

                foreach (var killer in currentState.GetAgents())
                {
                    if (killer.Key.GetRole() == AgentRole.ANTAGONIST && killer.Value.GetStatus())
                    {
                        action.Arguments.Add(killer);
                        break;
                    }
                }

                action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
                return true;
            }
            else if (action is Run || action is CounterRun)
            {
                action.Arguments.Add(initiator);
                action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
                action.Arguments.Add(currentState.
                        GetRandomLocationWithout(currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName())));
                return true;
            }
            else if (action is NothingToDo)
            {
                action.Arguments.Add(initiator);
            }
            else if (action is TellAboutASuspicious || action is CounterTellAboutASuspicious)
            {
                foreach (var agent in currentState.GetAgents())
                {
                    if (agent.Key.GetRole() == AgentRole.USUAL && agent.Value.GetStatus() 
                        && currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)).Value.SearchAgent(agent.Key))
                    {
                        action.Arguments.Add(agent);
                        break;
                    }
                }

                action.Arguments.Add(initiator);
                action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
                action.Arguments.Add(currentState.GetRandomLocationWithout
                    (currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName())));
                return true;
            }
            else if (action is Talk || action is CounterTalk)
            {
                action.Arguments.Add(initiator);

                foreach (var agent in currentState.GetAgents())
                {
                    if (agent.Value.GetStatus()
                        && currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)).Value.SearchAgent(agent.Key)
                        && agent.Key.GetName() != initiator.Key.GetName())
                    {
                        action.Arguments.Add(agent);
                        return true;
                    }
                }

                return false;
            }
            else if (action is ToBeAWitness)
            {
                action.Arguments.Add(initiator);

                foreach (var victim in currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)).Value.GetAgents())
                {
                    if (!victim.Value.GetStatus())
                    {
                        action.Arguments.Add(victim);
                        break;
                    }
                }

                foreach (var killer in currentState.GetAgents())
                {
                    if (killer.Key.GetRole() == AgentRole.ANTAGONIST && killer.Value.GetStatus())
                    {
                        action.Arguments.Add(killer);
                        break;
                    }
                }

                action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
            }
            else if (action is HelpElfs)
            {
                action.Arguments.Add(initiator);
                action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
            }
            else if (action is HelpWerewolves)
            {
                action.Arguments.Add(initiator);
                action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
            }
            else if (action is HelpMages)
            {
                action.Arguments.Add(initiator);
                action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
            }
            else if (action is HelpTemplars)
            {
                action.Arguments.Add(initiator);
                action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
            }
            else if (action is HelpPrinceBelen)
            {
                action.Arguments.Add(initiator);
                action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
            }
            else if (action is HelpLordHarrowmont)
            {
                action.Arguments.Add(initiator);
                action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
            }
            else if (action is CompleteQuest)
            {
                action.Arguments.Add(initiator);
                action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
            }

            return false;
        }

        /// <summary>
        /// Method for assigning parameters to several actions of the same type.
        /// </summary>
        /// <param name="action">The action to which the parameters are assigned.</param>
        /// <param name="currentState">The current world state.</param>
        /// <param name="initiator">The agent performing the action.</param>
        /// <param name="note">Text to display on the main screen.</param>
        /// <returns>True if the assignment was successful, false otherwise.</returns>
        public List<PlanAction> MassiveAssignVariables (ref PlanAction action, 
                                                        WorldDynamic currentState, 
                                                        KeyValuePair<AgentStateStatic, AgentStateDynamic> initiator,
                                                        ref TextBox note)
        {
            note.Text = "MASSIVE ASSIGN VARIABLES FOR ACTIONS";

            List<PlanAction> actions = new List<PlanAction>();

            if (action is Move)
            {
                int locationsCount = 0;

                if (currentState.GetStaticWorldPart().GetConnectionStatus())
                {
                    locationsCount =
                       currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName()).Key.GetConnectedLocations().Count;
                }
                else if (!currentState.GetStaticWorldPart().GetConnectionStatus())
                {
                    locationsCount = currentState.GetLocations().Count - 1;
                }

                List<Move> moveArr = new List<Move>();

                for (int i = 0; i < locationsCount; i++)
                {
                    Move move = new Move(currentState);
                    moveArr.Add(move);
                }

                for (int i = 0, j = 0; i < locationsCount; i++, j++)
                {
                    if (currentState.GetStaticWorldPart().GetConnectionStatus())
                    {
                        moveArr[i].Arguments.Add(initiator);
                        moveArr[i].Arguments.Add(currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName()));
                        moveArr[i].Arguments.Add(currentState.GetLocationByName(currentState.GetLocationByName(
                            currentState.SearchAgentAmongLocations(initiator.Key).GetName()).Key.GetConnectedLocationFromIndex(i).GetName()));
                    }
                    else if (!currentState.GetStaticWorldPart().GetConnectionStatus())
                    {
                        if (!currentState.GetLocationByIndex(j).Key.Equals(currentState.SearchAgentAmongLocations(initiator.Key)))
                        {
                            moveArr[i].Arguments.Add(initiator);
                            moveArr[i].Arguments.Add(currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName()));
                            moveArr[i].Arguments.Add(currentState.GetLocationByIndex(j));
                        }
                        else { i--; }
                    }
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
