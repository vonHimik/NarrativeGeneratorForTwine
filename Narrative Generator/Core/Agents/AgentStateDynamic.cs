using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    public class AgentStateDynamic : IEquatable<AgentStateDynamic>, ICloneable
    {
        // Action components
        private Plan myCurrentPlan;
        private List<PlanAction> myAvailableActions;

        // Properties
        private AgentStateStatic agentInfo;
        private bool alive;
        private Goal myGoals;
        private int initiative;
        private bool scared;
        public int id; // Just for test
        private AgentAngryAt angryAt;
        private LocationStatic wantsToGo;

        // Beliefs
        private WorldContext beliefs;
        private AgentFoundEvidence foundEvidence;
        private HashSet<LocationStatic> exploredRooms;

        // Hashcode
        private bool hasHashCode;
        private int hashCode;

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public AgentStateDynamic()
        {
            agentInfo = new AgentStateStatic();
            myCurrentPlan = new Plan();
            myAvailableActions = new List<PlanAction>();
            alive = true;
            myGoals = new Goal();
            beliefs = new WorldContext();
            initiative = 0;
            angryAt = new AgentAngryAt();
            scared = false;
            foundEvidence = new AgentFoundEvidence();
            wantsToGo = new LocationStatic();
            exploredRooms = new HashSet<LocationStatic>();
            hasHashCode = false;
            hashCode = 0;
        }

        public AgentStateDynamic (AgentStateDynamic clone)
        {
            agentInfo = (AgentStateStatic)clone.agentInfo.Clone();
            myCurrentPlan = (Plan)clone.myCurrentPlan.Clone();
            myAvailableActions = new List<PlanAction>(clone.myAvailableActions);
            alive = clone.alive;
            myGoals = (Goal)clone.myGoals.Clone();
            beliefs = (WorldContext)clone.beliefs.Clone();
            initiative = clone.initiative;
            angryAt = (AgentAngryAt)clone.angryAt.Clone();
            scared = clone.scared;
            foundEvidence = (AgentFoundEvidence)clone.foundEvidence.Clone();
            if (clone.wantsToGo != null) { wantsToGo = (LocationStatic)clone.wantsToGo.Clone(); ; }
            else { wantsToGo = new LocationStatic(); }
            exploredRooms = new HashSet<LocationStatic>(clone.exploredRooms);
            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
        }

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        public AgentStateDynamic(bool alive, Goal goals, WorldContext beliefs, AgentStateStatic agentInfo)
        {
            this.agentInfo = agentInfo;

            myCurrentPlan = new Plan();
            myAvailableActions = new List<PlanAction>();

            SetStatus(alive);
            SetGoal(goals);
            SetBeliefs(beliefs);
            SetInitiative(0);

            AgentAngryAt angryAt = new AgentAngryAt(false, null);
            SetObjectOfAngry(angryAt);

            ScaredOff();

            AgentFoundEvidence foundEvidence = new AgentFoundEvidence(false, null);
            AddEvidence(foundEvidence);

            SetTargetLocation(null);

            exploredRooms = new HashSet<LocationStatic>();

            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Deep cloning of an agent.
        /// </summary>
        public object Clone()
        {
            var clone = new AgentStateDynamic();

            clone.myCurrentPlan = new Plan(myCurrentPlan);
            clone.myAvailableActions = new List<PlanAction>(myAvailableActions);
            clone.alive = alive;
            clone.myGoals = new Goal(myGoals);
            clone.beliefs = new WorldContext(beliefs);
            clone.initiative = initiative;
            if (angryAt != null && angryAt.GetObjectOfAngry() != null) { clone.angryAt = new AgentAngryAt(angryAt); }
            clone.scared = scared;
            if (foundEvidence != null && foundEvidence.GetCriminal() != null) { clone.foundEvidence = new AgentFoundEvidence(foundEvidence); }
            if (wantsToGo != null) { clone.wantsToGo = new LocationStatic(wantsToGo); }
            clone.exploredRooms = new HashSet<LocationStatic>(exploredRooms);

            return clone;
        }

        /// <summary>
        /// Generate a new PDDL file with a problem for the specified agent, based on his beliefs.
        /// </summary>
        public void GenerateNewPDDLProblem(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                           WorldDynamic currentWorldState, 
                                           bool killerCantCreatePlan = false)
        {
            string fileName = "";
            string problemName = "";
            string domainName = "";
            string objects = "";
            string init = "";
            string goal = "";

            switch (agent.Key.GetRole())
            {
                case AgentRole.USUAL:
                    fileName = "AgentProblemTEST";
                    break;
                case AgentRole.KILLER:
                    fileName = "KillerProblemTEST";
                    break;
                case AgentRole.PLAYER:
                    fileName = "PlayerProblemTEST";
                    break;
            }

            problemName = "detective-problem";
            domainName = "detective-domain";

            foreach (var location in agent.Value.GetBeliefs().GetLocationsInWorld())
            {
                objects = objects.Insert(objects.Length, location.GetName() + " ");
                init = init.Insert(init.Length, Environment.NewLine + "(ROOM " + location.GetName() + ") ");

                foreach (var connectedLocation in location.GetConnectedLocations())
                {
                    init = init.Insert(init.Length, "(connected " + location.GetName() + " " + connectedLocation.GetName() + ")");
                }
            }

            foreach (var a in agent.Value.GetBeliefs().GetAgentsInWorld())
            {
                objects = objects.Insert(objects.Length, a.GetInfo().GetName() + " ");

                switch (a.GetRole())
                {
                    case AgentRole.USUAL:
                        init = init.Insert(init.Length, Environment.NewLine + "(AGENT " + a.GetInfo().GetName() + ") ");
                        break;
                    case AgentRole.KILLER:
                        init = init.Insert(init.Length, Environment.NewLine + "(KILLER " + a.GetInfo().GetName() + ") ");
                        break;
                    case AgentRole.PLAYER:
                        if (agent.Key.GetRole() == AgentRole.PLAYER)
                        {
                            init = init.Insert(init.Length, Environment.NewLine + "(PLAYER " + a.GetInfo().GetName() + ") ");
                        }
                        else
                        {
                            init = init.Insert(init.Length, Environment.NewLine + "(AGENT " + a.GetInfo().GetName() + ") ");
                        }
                        break;
                }

                switch (a.CheckStatus())
                {
                    case true:
                        init = init.Insert(init.Length, "(alive " + a.GetInfo().GetName() + ") ");
                        break;
                    case false:
                        init = init.Insert(init.Length, "(died " + a.GetInfo().GetName() + ") ");
                        break;
                }

                switch (agent.Key.GetRole())
                {
                    case AgentRole.USUAL:
                        // An agent can claim that some other agent is in a certain location only if he has this information.
                        if (agent.Value.GetBeliefs().SearchAgentAmongLocations(a.GetInfo()) != null)
                        {
                            init = init.Insert(init.Length, "(in-room " + a.GetInfo().GetName() + " " + 
                                agent.Value.GetBeliefs().GetLocationByName(agent.Value.GetBeliefs().SearchAgentAmongLocations(a.GetInfo()).GetName())
                                .GetName() + ") ");

                            if (agent.Equals(a))
                            {
                                foreach (var loc in agent.Value.GetExploredLocations())
                                {
                                    init = init.Insert(init.Length, "(explored-room " + a.GetInfo().GetName() + " " + loc.GetName() + ")");
                                }
                            }
                        }
                        break;
                    case AgentRole.KILLER:
                        init = init.Insert(init.Length, "(in-room " + a.GetInfo().GetName () + " " +
                                agent.Value.GetBeliefs().GetLocationByName(currentWorldState.SearchAgentAmongLocations(a.GetInfo()).GetName())
                                .GetName() + ") ");
                        break;
                    case AgentRole.PLAYER:
                        init = init.Insert(init.Length, "(in-room " + a.GetInfo().GetName() + " " +
                                agent.Value.GetBeliefs().GetLocationByName(currentWorldState.SearchAgentAmongLocations(a.GetInfo()).GetName())
                                .GetName() + ") ");
                        break;
                }
            }

            if (agent.Value.GetGoal().goalTypeIsStatus)
            {
                switch (agent.Key.GetRole())
                {
                    case AgentRole.USUAL:
                        foreach (var a in agent.Value.GetGoal().GetGoalState().GetAgents())
                        {
                            if (a.Key.GetRole() == AgentRole.KILLER)
                            {
                                if (a.Key.GetName() != null && a.Key.GetName() != "")
                                {
                                    goal = goal.Insert(goal.Length, "(died " + a.Key.GetName() + ") ");
                                }
                                else
                                {
                                    Random random = new Random();
                                    int randomValue = random.Next(1, 25);

                                    if (randomValue <= 15)
                                    {
                                        goal = goal.Insert(goal.Length, "(in-room " + agent.Key.GetName() + " "
                                                   + agent.Value.GetBeliefs().GetRandomLocationWithout(agent.Value.GetMyLocation()).GetName()
                                                   + ") ");
                                    }
                                    else if (randomValue > 15 && randomValue <= 20 && !CheckIfLocationIsExplored(agent.Value.GetMyLocation()))
                                    {
                                        goal = goal.Insert(goal.Length, "(explored-room " + agent.Key.GetName() + " "
                                                   + agent.Value.GetMyLocation().GetName() + ") ");
                                    }
                                    else if (randomValue > 20 || (randomValue > 15 && CheckIfLocationIsExplored(agent.Value.GetMyLocation())))
                                    {
                                        goal = goal.Insert(goal.Length, "(talking " + agent.Key.GetName() + " " 
                                                   + currentWorldState.GetRandomAgent(agent).Key.GetName() + ") ");
                                    }
                                }
                            }
                        }
                        break;
                    case AgentRole.PLAYER:
                        foreach (var a in agent.Value.GetGoal().GetGoalState().GetAgents())
                        {
                            if (a.Key.GetRole() == AgentRole.KILLER)
                            {
                                if (a.Key.GetName() != null && a.Key.GetName() != "")
                                {
                                    goal = goal.Insert(goal.Length, "(died " + a.Key.GetName() + ") ");
                                }
                                else
                                {
                                    Random random = new Random();
                                    int randomValue = random.Next(1, 21);

                                    if (randomValue <= 15)
                                    {
                                        goal = goal.Insert(goal.Length, "(in-room " + agent.Key.GetName() + " "
                                                   + agent.Value.GetBeliefs().GetRandomLocationWithout(agent.Value.GetMyLocation()).GetName()
                                                   + ") ");
                                    }
                                    else if (randomValue > 15)
                                    {
                                        goal = goal.Insert(goal.Length, "(explored-room " + agent.Key.GetName() + " "
                                                   + agent.Value.GetMyLocation().GetName() + ") ");
                                    }
                                }
                            }
                        }
                        break;
                    case AgentRole.KILLER:
                        killerCantCreatePlan = true;
                        if (killerCantCreatePlan)
                        {
                            bool hasTarget = false;

                            while (!hasTarget)
                            {
                                //BeliefsAboutAgent a = agent.Value.GetBeliefs().GetRandomAgent();
                                //  !!! If all ordinary agents are killed, but the killer continues his move, we will get an error. !!!
                                BeliefsAboutAgent a = null;

                                foreach (var agnt in agent.Value.GetBeliefs().GetAgentsInWorld())
                                {
                                    if (agnt.CheckStatus() && (agnt.GetRole() == AgentRole.USUAL || agnt.GetRole() == AgentRole.PLAYER))
                                    {
                                        a = agnt;
                                        break;
                                    }
                                }

                                if (a.GetInfo().GetRole() == AgentRole.USUAL || a.GetInfo().GetRole() == AgentRole.PLAYER)
                                {
                                    goal = goal.Insert(goal.Length, "(died " + a.GetInfo().GetName() + ") ");
                                    hasTarget = true;
                                }
                            }
                        }
                        break;
                }
            }


            FileStream file = new FileStream(fileName + ".pddl", FileMode.Create, FileAccess.ReadWrite);
            StreamWriter streamWriter = new StreamWriter(file, Encoding.GetEncoding(1251));

            streamWriter.WriteLine("(define (problem " + problemName + ")");
            streamWriter.WriteLine("(:domain " + domainName + ")");
            streamWriter.WriteLine("(:objects " + objects + ")");
            streamWriter.WriteLine("(:init " + init + ")");
            streamWriter.WriteLine("(:goal (and " + goal + "))");
            streamWriter.WriteLine(")");

            streamWriter.Close();
        }

        /// <summary>
        /// Calculate an action plan for the agent based on PDDL files with descriptions of the domain and problem.
        /// </summary>
        /// <param name="agent"></param>
        public void CalculatePlan(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, WorldDynamic currentWorldState)
        {
            // We create an instance of the FastDownward class to interact with the planner.
            FastDownward fastDownward = new FastDownward();

            // We initialize variables containing the names of the Domain and Problem PDDL files.
            string domainFileName = null;
            string problemFileName = null;

            // We clear the current plan (we re-calculate it, not add to it).
            myCurrentPlan.Clear();

            // The agent's role affects what actions are available to him and what goals he pursues, therefore, the role determines what
            //    what PDDL files need to be processed.
            switch (agent.Key.GetRole())
            {
                case AgentRole.USUAL:
                    domainFileName = "AgentDomainTEST";
                    problemFileName = "AgentProblemTEST";
                    break;
                case AgentRole.KILLER:
                    domainFileName = "KillerDomainTEST";
                    problemFileName = "KillerProblemTEST";
                    break;
                case AgentRole.PLAYER:
                    domainFileName = "PlayerDomainTEST";
                    problemFileName = "PlayerProblemTEST";
                    break;
            }

            // We launch the planner, specifying the names of the files with the corresponding Domain and Problem.
            fastDownward.Run(domainFileName, problemFileName);

            // In the event that the planner successfully completed its work.
            if (fastDownward.isSuccess)
            {
                // Then we try to extract the plan from the file created by the planner.
                fastDownward.GetResultPlan(ref myCurrentPlan);

                // If, for some reason, it was not possible to extract the plan, then recursively try to generate a new file with the Problem
                //    and fetch the plan again.
                if (!myCurrentPlan.planReceived)
                {
                    GenerateNewPDDLProblem(agent, currentWorldState, true);
                    CalculatePlan(agent, currentWorldState);
                }
            }
            // If the planner completed its work with an error, then recursively try to generate a new file with the Problem
            //    and fetch the plan again.
            else
            {
                GenerateNewPDDLProblem(agent, currentWorldState);
                CalculatePlan(agent, currentWorldState);
            }
        }

        /// <summary>
        /// Populates the list of actions available to the agent.
        /// </summary>
        public void ReceiveAvailableActions(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            ActionGenerator actionGenerator = new ActionGenerator();
            myAvailableActions = actionGenerator.GetAvailableActions(agent);
        }

        public List<PlanAction> GetAvailableActions()
        {
            return myAvailableActions;
        }

        /// <summary>
        /// Correlates the list of actions available to the agent with the first action in its action plan, and returns the first match or null.
        /// </summary>
        public PlanAction ChooseAction()
        {
            // We go through all the actions available to the agent.
            for (int i = 0; i < myAvailableActions.Count(); i++)
            {
                // If one of the available actions matches the first action in the plan.
                if (myCurrentPlan.GetAction(0).GetType() == myAvailableActions[i].GetType())
                {
                    if (myCurrentPlan.GetAction(0).Arguments.Count() != 0)
                    {
                        Move testMoveAction = new Move();

                        if (myCurrentPlan.GetAction(0).GetType() == testMoveAction.GetType())
                        {
                            return myCurrentPlan.GetAction(0);
                        }
                    }
                    else
                    {
                        // Then select it.
                        return myAvailableActions[i];
                    }
                }
            }

            // If none of the available actions coincided with the first in the plan, then return null.
            return null;
        }

        public void SetStatus(bool status)
        {
            alive = status;
            UpdateHashCode();
        }

        public bool GetStatus()
        {
            return alive;
        }

        public void Die()
        {
            alive = false;
            UpdateHashCode();
        }

        public void SetGoal(Goal goal)
        {
            myGoals = (Goal)goal.Clone();
        }

        public Goal GetGoal()
        {
            return myGoals;
        }

        public void SetBeliefs(WorldContext beliefs)
        {
            this.beliefs = beliefs;
        }

        public WorldContext GetBeliefs()
        {
            return beliefs;
        }

        public void SetInitiative(int initiative)
        {
            this.initiative = initiative;
        }

        public int GetInitiative()
        {
            return initiative;
        }

        /// <summary>
        /// Updates the agent's beliefs about the location where he is.
        /// </summary>
        public void RefreshBeliefsAboutTheWorld(WorldDynamic currentWorldState, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            // Before clearing the information, remember the location in which the agent is located.
            //LocationStatic agentIsHereLoc = agent.Value.GetBeliefs().SearchAgentAmongLocations(agent.Key);
            LocationStatic agentIsHereLoc = currentWorldState.GetLocationByName(currentWorldState.SearchAgentAmongLocations(agent.Key).GetName()).Key;

            // We clear the information about the location in which the agent is located, in his beliefs.
            agent.Value.GetBeliefs().GetAgentByName(agent.Key.GetName()).ClearLocation();

            // We find the same location in the "real" world. We go through the agents in it. We are looking for agents 
            //    with the same names in the agent's beliefs. We add them to the location (in his beliefs) where he (in his belief) is.
            foreach (var agent1 in currentWorldState.GetLocationByName(agentIsHereLoc.GetName()).Value.GetAgents())
            {
                foreach (var agent2 in agent.Value.GetBeliefs().GetAgentsInWorld())
                {
                    if (agent1.Key.GetName() == agent2.GetInfo().GetName())
                    {
                        agent.Value.GetBeliefs().GetAgentByName(agent2.GetInfo().GetName()).SetLocation(agentIsHereLoc);

                        if (!agent2.CheckStatus())
                        {
                            foreach (var a in currentWorldState.GetAgents())
                            {
                                a.Value.GetBeliefs().GetAgentByName(agent2.GetInfo().GetName()).Dead();
                            }
                        }

                        break;
                    }
                }
            }
        }

        public void SetTargetLocation(LocationStatic location)
        {
            wantsToGo = location;
            UpdateHashCode();
        }

        public LocationStatic GetTargetLocation()
        {
            return wantsToGo;
        }

        public void ClearTargetLocation()
        {
            wantsToGo = null;
            UpdateHashCode();
        }

        public void SetObjectOfAngry(AgentStateStatic target)
        {
            angryAt.AngryOn();
            angryAt.SetObjectOfAngry(target);
            UpdateHashCode();
        }

        public void SetObjectOfAngry(AgentAngryAt angryAt)
        {
            this.angryAt = angryAt;
            UpdateHashCode();
        }

        public AgentAngryAt GetObjectOfAngry()
        {
            return angryAt;
        }

        public void CalmDown()
        {
            angryAt.AngryOff();
            angryAt.SetObjectOfAngry(null);
            UpdateHashCode();
        }

        public void AddExploredLocation(LocationStatic location)
        {
            foreach (var loc in exploredRooms)
            {
                if (loc.GetName() == location.GetName())
                {
                    return;
                }
            }

            exploredRooms.Add(location);
            UpdateHashCode();
        }

        public void AddExploredLocations(HashSet<LocationStatic> locations)
        {
            exploredRooms = locations;
        }

        public HashSet<LocationStatic> GetExploredLocations()
        {
            return exploredRooms;
        }

        public LocationStatic GetExploredLocation(int index)
        {
            return exploredRooms.ElementAt(index);
        }

        public bool CheckIfLocationIsExplored(LocationStatic location)
        {
            foreach(var loc in exploredRooms)
            {
                if (loc.Equals(location)) { return true; }
            }

            return false;
        }

        public bool SearchAmongExploredLocations(LocationStatic location)
        {
            for (int i = 0; i < exploredRooms.Count(); i++)
            {
                if (exploredRooms.ElementAt(i) == location)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddEvidence(AgentStateStatic criminal)
        {
            foundEvidence.IsEvidence();
            foundEvidence.SetCriminal(criminal);
            UpdateHashCode();
        }

        public void AddEvidence(AgentFoundEvidence foundEvidence)
        {
            this.foundEvidence = foundEvidence;
            UpdateHashCode();
        }

        public AgentFoundEvidence GetEvidenceStatus()
        {
            return foundEvidence;
        }

        public void ClearEvidence()
        {
            foundEvidence.Clear();
            UpdateHashCode();
        }

        /// <summary>
        /// Checks if the agent knows that one of the other agents is angry.
        /// </summary>
        public bool ThinksThatSomeoneIsAngry()
        {
            foreach (var a in beliefs.GetAgentsInWorld())
            {
                if (a.GetObjectOfAngry().AngryCheck())
                {
                    return true;
                }
            }

            return false;
        }

        public void ScaredOn()
        {
            scared = true;
            UpdateHashCode();
        }

        public void ScaredOff()
        {
            scared = false;
            UpdateHashCode();
        }

        /// <summary>
        /// Checks if the agent is scared.
        /// </summary>
        public bool CheckScared()
        {
            return scared;
        }

        public bool GetPlanStatus()
        {
            return myCurrentPlan.planReceived;
        }

        public Plan GetPlan()
        {
            return myCurrentPlan;
        }

        public void SetAgentInfo(AgentStateStatic agentInfo)
        {
            this.agentInfo = agentInfo;
            UpdateHashCode();
        }

        public AgentStateStatic GetAgentInfo()
        {
            return agentInfo;
        }

        public LocationStatic GetMyLocation()
        {
            foreach (var location in GetBeliefs().GetLocationsInWorld())
            {
                if (location.GetName() == beliefs.GetMyLocation().GetName())
                {
                    return location;
                }
            }

            throw new KeyNotFoundException();
        }

        public bool Equals (AgentStateDynamic other)
        {
            if (this.GetHashCode() == other.GetHashCode()) { return true; }
            else { return false; }
        }

        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            agentInfo.ClearHashCode();
            hashcode = hashcode * 42 + agentInfo.GetHashCode();
            hashcode = hashcode * 42 + alive.GetHashCode();
            hashCode = hashcode * 42 + scared.GetHashCode();
            if (angryAt != null)
            {
                angryAt.ClearHashCode();
                hashcode = hashcode * 42 + angryAt.GetHashCode();
            }
            if (wantsToGo != null)
            {
                wantsToGo.ClearHashCode();
                hashcode = hashcode * 42 + wantsToGo.GetHashCode();
            }
            if (foundEvidence != null)
            {
                foundEvidence.ClearHashCode();
                hashcode = hashcode * 42 + foundEvidence.GetHashCode();
            }
            if (exploredRooms != null)
            {
                foreach (var explRoom in exploredRooms)
                {
                    explRoom.ClearHashCode();
                    hashcode = hashcode * 42 + explRoom.GetHashCode();
                }
            }

            hashCode = hashcode;
            hasHashCode = true;

            return hashcode;
        }

        public void ClearHashCode()
        {
            hasHashCode = false;
            hashCode = 0;
        }

        public void UpdateHashCode()
        {
            ClearHashCode();
            GetHashCode();
        }
    }
}