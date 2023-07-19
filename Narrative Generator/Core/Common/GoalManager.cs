using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that manages the goals of agents.
    /// </summary>
    class GoalManager
    {
        /// <summary>
        /// Active goal type marker: goals based on agent statuses.
        /// </summary>
        public bool KillAntagonistOrAllEnemis { get; set; }
        /// <summary>
        /// Active goal type marker: goals based on agents being in specific locations.
        /// </summary>
        public bool ReachGoalLocation { get; set; }
        /// <summary>
        /// Active goal type marker: goals based on agent ownership of certain items.
        /// </summary>
        public bool GetImportantItem { get; set; }

        /// <summary>
        /// Storage of individual instances of active goal types.
        /// </summary>
        private List<GoalTypes> goalTypes = new List<GoalTypes>();

        private string targetLocationName;
        private List<string> targetItemsNames;

        /// <summary>
        /// A method that adds goals that are marked as active to the goal type storage.
        /// </summary>
        public void Initialization (string targetLocationName, List<string> targetItemsNames)
        {
            if (KillAntagonistOrAllEnemis) { goalTypes.Add(GoalTypes.STATUS); }
            if (ReachGoalLocation) { goalTypes.Add(GoalTypes.LOCATION); }
            if (GetImportantItem) { goalTypes.Add(GoalTypes.POSSESSION); }
            this.targetLocationName = targetLocationName;
            this.targetItemsNames = targetItemsNames;
        }

        /// <summary>
        /// A method that creates a set of goals to pass to agents.
        /// </summary>
        /// <param name="roles">Roles of agents existing in the world.</param>
        /// <returns>Goals set.</returns>
        public List<Goal> CreateGoalSet (List<AgentRole> roles)
        {
            // We create an empty list of goals.
            List<Goal> goals = new List<Goal>();

            foreach (var role in roles)
            {
                bool statusGoalTypeIsSelected = false;
                Goal newGoal = new Goal();

                foreach (var goalType in goalTypes)
                {
                    if (goalType.Equals(GoalTypes.STATUS))
                    {
                        statusGoalTypeIsSelected = true;

                        newGoal.AddGoalType(GoalTypes.STATUS);
                    }
                    if (goalType.Equals(GoalTypes.LOCATION))
                    {
                        if (role.Equals(AgentRole.PLAYER))
                        {
                            newGoal.AddGoalType(GoalTypes.LOCATION);
                        }
                        else
                        {
                            if (!statusGoalTypeIsSelected) { newGoal.AddGoalType(GoalTypes.STATUS); statusGoalTypeIsSelected = true; }
                        }
                    }
                    if (goalType.Equals(GoalTypes.POSSESSION))
                    {
                        if (role.Equals(AgentRole.PLAYER))
                        {
                            newGoal.AddGoalType(GoalTypes.POSSESSION);
                        }
                        else
                        {
                            if (!statusGoalTypeIsSelected) { newGoal.AddGoalType(GoalTypes.STATUS); statusGoalTypeIsSelected = true; }
                        }
                    }
                }

                goals.Add(newGoal);
            }

            return goals;
        }

        /// <summary>
        /// A method that assigns goals to agents based on their role.
        /// </summary>
        /// <param name="state">The current world state.</param>
        public void AssignGoalsToAgents (ref WorldDynamic state)
        {
            Dictionary<AgentStateStatic, AgentStateDynamic> agents = state.GetAgents();

            // Go through all the agents in the world.
            foreach (var agent in agents)
            {
                switch (agent.Key.GetRole())
                {
                    case AgentRole.PLAYER:

                        foreach (var goalType in agent.Value.GetGoal().GetGoalType())
                        {
                            if (goalType.Equals(GoalTypes.STATUS))
                            {
                                agent.Value.GetGoal().GetGoalState().AddAgent(AgentRole.ANTAGONIST, false);

                                if ((state.GetStaticWorldPart().GetSetting().Equals(Setting.DragonAge) ||
                                     state.GetStaticWorldPart().GetSetting().Equals(Setting.GenericFantasy)))
                                {
                                    agent.Value.SetObjectOfAngry(state.GetAgentByRole(AgentRole.ANTAGONIST).Key);
                                }
                            }
                            if (goalType.Equals(GoalTypes.LOCATION))
                            {
                                LocationStatic newStatic = new LocationStatic();
                                LocationDynamic newDynamic = new LocationDynamic();
                                KeyValuePair<LocationStatic, LocationDynamic> newLocation = new KeyValuePair<LocationStatic, LocationDynamic>(newStatic, newDynamic);

                                agent.Value.GetGoal().GetGoalState().AddLocation(newLocation, targetLocationName);

                                foreach (var location in agent.Value.GetGoal().GetGoalState().GetLocations())
                                {
                                    if (location.Key.GetName().Equals(targetLocationName))
                                    {
                                        location.Value.AddAgent(agent);
                                    }
                                }
                            }
                            if (goalType.Equals(GoalTypes.POSSESSION))
                            {
                                agent.Value.GetGoal().GetGoalState().
                                    AddAgent((AgentStateStatic)agent.Key.Clone(), (AgentStateDynamic)agent.Value.Clone());

                                foreach (var item in targetItemsNames)
                                {
                                    agent.Value.GetGoal().GetGoalState().GetAgentByRole(AgentRole.PLAYER).Value.
                                        AddItem(ItemsManager.CreateItem(item, ItemsTypesUtils.GetEnum(item.ToLower())));
                                }
                            }
                        }

                        break;

                    case AgentRole.ANTAGONIST:

                        agent.Value.GetGoal().GetGoalState().AddAgent(AgentRole.PLAYER, false);

                        if ((state.GetStaticWorldPart().GetSetting().Equals(Setting.DragonAge) ||
                             state.GetStaticWorldPart().GetSetting().Equals(Setting.GenericFantasy)))
                        {
                            agent.Value.SetObjectOfAngry(state.GetAgentByRole(AgentRole.PLAYER).Key);
                        }

                        foreach (var anotherAgent in state.GetAgents())
                        {
                            if (anotherAgent.Key.GetRole().Equals(AgentRole.USUAL))
                            {
                                agent.Value.GetGoal().GetGoalState().AddAgent(AgentRole.USUAL, false, anotherAgent.Key.GetName());
                            }
                        }

                        break;

                    case AgentRole.USUAL:

                        agent.Value.GetGoal().GetGoalState().AddAgent(AgentRole.ANTAGONIST, false);

                        Random random = new Random();
                        int r = random.Next(1);

                        if (state.GetStaticWorldPart().GetSetting().Equals(Setting.Detective) && state.GetStaticWorldPart().GetAgentsHasUniqGoals() && r==1)
                        {
                            agent.Value.GetGoal().GetGoalState().AddAgent(state.GetRandomAgent().Key.GetRole(), false, state.GetRandomAgent().Key.GetName());
                        }
                        else if (state.GetStaticWorldPart().GetSetting().Equals(Setting.Detective) && state.GetStaticWorldPart().GetAgentsHasUniqGoals())
                        {
                            //state.GetAgentByName(agent.Key.GetName()).Value.GetGoal().GetGoalState().GetRandomLocation().Value.AddAgent(agent);
                            KeyValuePair<LocationStatic, LocationDynamic> randomLocation = state.GetRandomLocation();
                            LocationStatic rLS = (LocationStatic)randomLocation.Key.Clone(); LocationDynamic rLD = (LocationDynamic)randomLocation.Value.Clone();
                            rLD.AddAgent(agent);
                            state.GetAgentByName(agent.Key.GetName()).Value.GetGoal().GetGoalState().GetLocations().Add(rLS, rLD);
                        }

                        break;

                    case AgentRole.ENEMY:

                        agent.Value.GetGoal().GetGoalState().AddAgent(AgentRole.PLAYER, false);

                        if ((state.GetStaticWorldPart().GetSetting().Equals(Setting.DragonAge) ||
                             state.GetStaticWorldPart().GetSetting().Equals(Setting.GenericFantasy)))
                        {
                            agent.Value.SetObjectOfAngry(state.GetAgentByRole(AgentRole.PLAYER).Key);
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Checks the achievement of any of the goal conditions (in state).
        /// </summary>
        /// <param name="currentNode">A node that stores the current world state.</param>
        /// <returns>True if the world state matches the goal state, otherwise false.</returns>
        public bool ControlToAchieveGoalState (ref StoryNode currentNode)
        {
            List<Goal> allGoalStates = new List<Goal>();
            int goalHit = 0;
            bool hasAllItems = false;

            // Collects goals from all agents and adds them to the goal list.
            foreach (var Agent in currentNode.GetWorldState().GetAgents())
            {
                bool antagonistPresent = false;
                int antagonistsCounter = 0;
                int enemyCounter = 0;
                int killedEnemyCointer = 0;

                List<string> tempTargetItemsList = new List<string>();
                foreach (var item in targetItemsNames)
                {
                    tempTargetItemsList.Add(item);
                }

                List<string> tempAgentItemsList = new List<string>();
                foreach (var item in Agent.Value.GetItems())
                {
                    tempAgentItemsList.Add(item.GetItemName());
                }

                foreach (var agent in currentNode.GetWorldState().GetAgents())
                {
                    if (agent.Key.GetRole().Equals(AgentRole.ANTAGONIST)) { antagonistPresent = true; }
                }

                foreach (var agent in currentNode.GetWorldState().GetAgents())
                {
                    if (agent.Key.GetRole().Equals(AgentRole.ANTAGONIST) 
                        || agent.Key.GetRole().Equals(AgentRole.ENEMY)) { antagonistsCounter++; }
                }

                foreach (var agent in currentNode.GetWorldState().GetAgents())
                {
                    if (agent.Key.GetRole().Equals(AgentRole.ENEMY)) { enemyCounter++; }
                }

                foreach (var goalType in Agent.Value.GetGoal().GetGoalType())
                {
                    switch (goalType)
                    {
                        case GoalTypes.STATUS:

                            int killCounter = 0;
                            bool killerDied = false;
                            bool playerDied = false;

                            foreach (var agent in currentNode.GetWorldState().GetAgents())
                            {
                                switch (agent.Key.GetRole())
                                {
                                    case AgentRole.USUAL: if (!agent.Value.GetStatus()) { killCounter++; } break;
                                    case AgentRole.PLAYER: if (!agent.Value.GetStatus()) { playerDied = true; } break;
                                    case AgentRole.ANTAGONIST: if (!agent.Value.GetStatus()) { killerDied = true; } break;
                                    case AgentRole.ENEMY: if (!agent.Value.GetStatus()) { killedEnemyCointer++; } break;
                                }
                            }

                            if (killCounter == currentNode.GetWorldState().GetAgents().Count - antagonistsCounter)
                            {
                                goalHit++;
                            }
                            else if (killerDied && !(Agent.Key.GetRole().Equals(AgentRole.ANTAGONIST) || Agent.Key.GetRole().Equals(AgentRole.ENEMY)))
                            {
                                goalHit++;
                            }
                            else if (playerDied && !Agent.Key.GetRole().Equals(AgentRole.PLAYER))
                            {
                                goalHit++;
                            }
                            else if (!antagonistPresent && killedEnemyCointer.Equals(antagonistsCounter))
                            {
                                goalHit++;
                            }

                            break;

                        case GoalTypes.LOCATION:

                            foreach (var location in currentNode.GetWorldState().GetLocations())
                            {
                                if (location.Key.GetName().Equals(targetLocationName))
                                {
                                    foreach (var agent in location.Value.GetAgents())
                                    {
                                        if (agent.Key.GetRole().Equals(AgentRole.PLAYER))
                                        {
                                            goalHit++;
                                        }
                                    }
                                }
                            }

                            break;

                        case GoalTypes.POSSESSION:
                            for (int i = 0; i < tempTargetItemsList.Count; i++)
                            {
                                for (int j = 0; j < tempAgentItemsList.Count; j++)
                                {
                                    if (tempAgentItemsList[i].Equals(tempAgentItemsList[j]))
                                    {
                                        tempTargetItemsList.RemoveAt(i);
                                        tempAgentItemsList.RemoveAt(j); j--;

                                        if (tempTargetItemsList.Count == 0)
                                        {
                                            break;
                                        }
                                    }
                                }

                                i--;

                                if (tempTargetItemsList.Count == 0 || tempAgentItemsList.Count == 0)
                                {
                                    break;
                                }
                            }

                            if (tempTargetItemsList.Count == 0) { hasAllItems = true; }

                            break;
                    }
                }

                if (GetImportantItem && hasAllItems)
                {
                    goalHit++;
                }

                if (Agent.Value.GetGoal().GetGoalType().Count == goalHit)
                {
                    currentNode.goalState = true;
                    return true;
                }
            }

            return false;
        }
    }
}