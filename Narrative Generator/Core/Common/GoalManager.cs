using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class GoalManager
    {
        public bool KillAntagonistOrAllEnemis { get; set; }
        public bool ReachGoalLocation { get; set; }
        public bool GetImportantItem { get; set; }

        private List<GoalTypes> goalTypes = new List<GoalTypes>();

        public void Initialization()
        {
            if (KillAntagonistOrAllEnemis) { goalTypes.Add(GoalTypes.STATUS); }
            if (ReachGoalLocation) { goalTypes.Add(GoalTypes.LOCATION); }
            if (GetImportantItem) { goalTypes.Add(GoalTypes.POSSESSION); }
        }

        /// <summary>
        /// A method that creates a set of goals to pass to agents.
        /// </summary>
        public List<Goal> CreateGoalSet (List<AgentRole> roles)
        {
            // We create an empty list of goals.
            List<Goal> goals = new List<Goal>();

            foreach (var goalType in goalTypes)
            {
                switch (goalType)
                {
                    case GoalTypes.STATUS:
                        foreach (var role in roles)
                        {
                            // We create an empty state of the world, which we will later transform into the goal state.
                            WorldDynamic newGoalState = new WorldDynamic();

                            goals.Add(new Goal(GoalTypes.STATUS, newGoalState));
                        }
                        break;
                    case GoalTypes.LOCATION:
                        foreach (var role in roles)
                        {
                            // We create an empty state of the world, which we will later transform into the goal state.
                            WorldDynamic newGoalState = new WorldDynamic();

                            if (role.Equals(AgentRole.PLAYER))
                            {
                                goals.Add(new Goal(GoalTypes.LOCATION, newGoalState));
                            }
                            else
                            {
                                goals.Add(new Goal(GoalTypes.STATUS, newGoalState));
                            }
                        }
                        break;
                    case GoalTypes.POSSESSION:
                        foreach (var role in roles)
                        {
                            // We create an empty state of the world, which we will later transform into the goal state.
                            WorldDynamic newGoalState = new WorldDynamic();

                            if (role.Equals(AgentRole.PLAYER))
                            {
                                goals.Add(new Goal(GoalTypes.POSSESSION, newGoalState));
                            }
                            else
                            {
                                goals.Add(new Goal(GoalTypes.STATUS, newGoalState));
                            }
                        }
                        break;
                }
            }

            return goals;
        }

        public void AssignGoalsToAgents (ref WorldDynamic state)
        {
            Dictionary<AgentStateStatic, AgentStateDynamic> agents = state.GetAgents();

            // Go through all the agents in the world.
            foreach (var agent in agents)
            {
                switch (agent.Key.GetRole())
                {
                    case AgentRole.PLAYER:
                        switch (agent.Value.GetGoal().GetGoalType())
                        {
                            case GoalTypes.STATUS:
                                agent.Value.GetGoal().GetGoalState().AddAgent(AgentRole.ANTAGONIST, false);

                                if ((state.GetStaticWorldPart().GetSetting().Equals(Setting.DragonAge) ||
                                     state.GetStaticWorldPart().GetSetting().Equals(Setting.GenericFantasy)))
                                {
                                    agent.Value.SetObjectOfAngry(state.GetAgentByRole(AgentRole.ANTAGONIST).Key);
                                }
                                break;
                            case GoalTypes.LOCATION:
                                state.GetAgentByRole(AgentRole.PLAYER).Value.GetGoal().GetGoalState().GetLocations().Last().Value.AddAgent(agent);
                                break;
                            case GoalTypes.POSSESSION: /* Not Implemented yet */ break;
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
        public bool ControlToAchieveGoalState (ref StoryNode currentNode)
        {
            List<Goal> allGoalStates = new List<Goal>();

            // Collects goals from all agents and adds them to the goal list.
            foreach (var agent in currentNode.GetWorldState().GetAgents())
            {
                allGoalStates.Add(agent.Value.GetGoal());
            }

            foreach (var goal in allGoalStates)
            {
                bool antagonistPresent = false;
                int antagonistsCounter = 0;
                int enemyCounter = 0;
                int killedEnemyCointer = 0;

                foreach (var agent in currentNode.GetWorldState().GetAgents())
                {
                    if (agent.Key.GetRole().Equals(AgentRole.ANTAGONIST)) { antagonistPresent = true; ; }
                }

                foreach (var agent in currentNode.GetWorldState().GetAgents())
                {
                    if (agent.Key.GetRole().Equals(AgentRole.ANTAGONIST) || agent.Key.GetRole().Equals(AgentRole.ENEMY)) { antagonistsCounter++; }
                }

                foreach (var agent in currentNode.GetWorldState().GetAgents())
                {
                    if (agent.Key.GetRole().Equals(AgentRole.ENEMY)) { enemyCounter++; }
                }

                switch (goal.GetGoalType())
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
                            currentNode.goalState = true;
                            return true;
                        }
                        if (killerDied)
                        {
                            currentNode.goalState = true;
                            return true;
                        }
                        if (playerDied)
                        {
                            currentNode.goalState = true;
                            return true;
                        }
                        if (!antagonistPresent && killedEnemyCointer.Equals(antagonistsCounter))
                        {
                            currentNode.goalState = true;
                            return true;
                        }
                        break;
                    case GoalTypes.LOCATION: break;
                    case GoalTypes.POSSESSION: break;
                }
            }

            return false;
        }
    }
}