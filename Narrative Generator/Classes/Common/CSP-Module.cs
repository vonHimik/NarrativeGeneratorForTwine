using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class CSP_Module
    {
        public void AssignVariables(ref PlanAction action, WorldBeliefs currentState, KeyValuePair<AgentStateStatic, AgentStateDynamic> initiator)
        {
            if (action is Entrap)
            {
                foreach (var agent in currentState.GetAgents())
                {
                    if (agent.Key.GetRole() == AgentRole.USUAL && agent.Value.GetStatus() 
                        && !currentState.GetLocation(currentState.SearchAgentAmongLocations(agent.Key)).SearchAgent(initiator.Key))
                    {
                        action.Arguments.Add(agent);
                        action.Arguments.Add(initiator); 
                        action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
                        break;
                    }
                }
            }
            else if (action is Fight)
            {
                foreach (var agent in currentState.GetAgents())
                {
                    if (agent.Key.GetRole() == AgentRole.USUAL && agent.Value.GetStatus() 
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
                        action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));
                        break;
                    }
                }
            }
            else if (action is Kill)
            {
                foreach (var agent in currentState.GetAgents())
                {
                    if (agent.Key.GetRole() == AgentRole.USUAL && agent.Value.GetStatus() 
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
                action.Arguments.Add(initiator);
                action.Arguments.Add(currentState.GetLocation(currentState.SearchAgentAmongLocations(initiator.Key)));

                if (initiator.Value.GetTargetLocation() != null)
                {
                    action.Arguments.Add(currentState.GetLocationByName(initiator.Value.GetTargetLocation().GetName()));
                }
                else
                {
                    action.Arguments.Add(currentState.
                        GetRandomLocation(currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName())));
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
                        GetRandomLocation(currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName())));
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
                action.Arguments.Add(currentState.GetRandomLocation
                    (currentState.GetLocationByName(currentState.SearchAgentAmongLocations(initiator.Key).GetName())));
            }
        }
    }
}
