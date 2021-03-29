using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class AgentStateDynamic : ICloneable
    {
        private AgentStateStatic agentInfo;

        // Action components
        private Plan myCurrentPlan;
        private List<PlanAction> myAvailableActions;

        // Agent properties
        private bool alive;
        private Goal myGoals;
        private WorldDynamic myBeliefsAboutWorld;
        private int initiative;
        private AgentAngryAt angryAt;
        private bool scared;
        private AgentFoundEvidence foundEvidence;
        private LocationStatic wantsToGo;
        private List<LocationStatic> exploredRooms;
        public int id;

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
            myBeliefsAboutWorld = new WorldDynamic();
            initiative = 0;
            angryAt = new AgentAngryAt();
            scared = false;
            foundEvidence = new AgentFoundEvidence();
            wantsToGo = new LocationStatic();
            exploredRooms = new List<LocationStatic>();
            id = -1;
        }

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="alive"></param>
        /// <param name="goals"></param>
        /// <param name="beliefs"></param>
        public AgentStateDynamic(bool alive, Goal goals, WorldDynamic beliefs, AgentStateStatic agentInfo)
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

            exploredRooms = new List<LocationStatic>();

            Random rand = new Random();
            id = rand.Next(100);
        }

        /// <summary>
        /// Deep cloning of an agent.
        /// </summary>
        public object Clone()
        {
            var clone = new AgentStateDynamic();

            clone.myCurrentPlan = (Plan)myCurrentPlan.Clone();
            clone.myAvailableActions = myAvailableActions;
            clone.alive = alive;
            clone.myGoals = (Goal)myGoals.Clone();
            clone.myBeliefsAboutWorld = (WorldDynamic)myBeliefsAboutWorld.Clone();
            clone.initiative = initiative;
            if (angryAt != null && angryAt.GetObjectOfAngry() != null) { clone.angryAt = (AgentAngryAt)angryAt.Clone(); }
            clone.scared = scared;
            if (foundEvidence != null && foundEvidence.GetCriminal() != null) { clone.foundEvidence = (AgentFoundEvidence)foundEvidence.Clone(); }
            if (wantsToGo != null) { clone.wantsToGo = (LocationStatic)wantsToGo.Clone(); }
            clone.exploredRooms = exploredRooms;

            return clone;
        }

        /// <summary>
        /// Generate a new PDDL file with a problem for the specified agent, based on his beliefs.
        /// </summary>
        /// <param name="agent"></param>
        public void GenerateNewPDDLProblem(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, WorldDynamic currentWorldState)
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
            }

            problemName = "detective-problem";
            domainName = "detective-domain";

            foreach (var location in agent.Value.GetBeliefs().GetLocations())
            {
                objects = objects.Insert(objects.Length, location.Key.GetName() + " ");
                init = init.Insert(init.Length, "(ROOM " + location.Key.GetName() + ") ");

                foreach (var connectedLocation in location.Key.GetConnectedLocations())
                {
                    init = init.Insert(init.Length, "(connected " + location.Key.GetName() + " " + connectedLocation.GetName() + ")");
                }
            }

            foreach (var a in agent.Value.GetBeliefs().GetAgents())
            {
                objects = objects.Insert(objects.Length, a.Key.GetName() + " ");

                switch (a.Key.GetRole())
                {
                    case AgentRole.USUAL:
                        init = init.Insert(init.Length, "(AGENT " + a.Key.GetName() + ") ");
                        break;
                    case AgentRole.KILLER:
                        init = init.Insert(init.Length, "(KILLER " + a.Key.GetName() + ") ");
                        break;
                }

                switch (a.Value.GetStatus())
                {
                    case true:
                        init = init.Insert(init.Length, "(alive " + a.Key.GetName() + ") ");
                        break;
                    case false:
                        init = init.Insert(init.Length, "(died " + a.Key.GetName() + ") ");
                        break;
                }

                switch (agent.Key.GetRole ())
                {
                    case AgentRole.USUAL:
                        // An agent can claim that some other agent is in a certain location only if he has this information.
                        if (agent.Value.GetBeliefs().SearchAgentAmongLocations(a.Key) != null)
                        {
                            init = init.Insert(init.Length, "(in-room " + a.Key.GetName () + " " + 
                                agent.Value.GetBeliefs().GetLocationByName(agent.Value.GetBeliefs().SearchAgentAmongLocations(a.Key).GetName())
                                .Key.GetName() + ") ");
                        }
                        break;
                    case AgentRole.KILLER:
                        init = init.Insert(init.Length, "(in-room " + a.Key.GetName () + " " +
                                agent.Value.GetBeliefs().GetLocationByName(currentWorldState.SearchAgentAmongLocations(a.Key).GetName())
                                .Key.GetName() + ") ");
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
                                    int randomValue = random.Next(1, 21);

                                    if (randomValue <= 15)
                                    {
                                        goal = goal.Insert(goal.Length, "(in-room " + agent.Key.GetName() + " "
                                                   + agent.Value.GetBeliefs().GetRandomLocationWithout(agent.Value.GetMyLocation()).Key.GetName()
                                                   + ") ");
                                    }
                                    else if (randomValue > 15)
                                    {
                                        goal = goal.Insert(goal.Length, "(explored-room " + agent.Key.GetName() + " "
                                                   + agent.Value.GetMyLocation().Key.GetName() + ") ");
                                    }
                                }
                            }
                        }
                        break;
                    case AgentRole.KILLER:
                        foreach (var a in agent.Value.GetBeliefs().GetAgents())
                        {
                            if (a.Key.GetRole() == AgentRole.USUAL)
                            {
                                goal = goal.Insert(goal.Length, "(died " + a.Key.GetName() + ") ");
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
                    GenerateNewPDDLProblem(agent, currentWorldState);
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
        /// <param name="agent"></param>
        public void GetAvailableActions(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            ActionGenerator actionGenerator = new ActionGenerator();
            myAvailableActions = actionGenerator.GetPossibleActions(agent);
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
        }

        public bool GetStatus()
        {
            return alive;
        }

        public void Die()
        {
            alive = false;
        }

        public void SetGoal(Goal goal)
        {
            myGoals = (Goal)goal.Clone();
        }

        public Goal GetGoal()
        {
            return myGoals;
        }

        public void SetBeliefs(WorldDynamic beliefs)
        {
            myBeliefsAboutWorld = (WorldDynamic)beliefs.Clone();
        }

        public WorldDynamic GetBeliefs()
        {
            return myBeliefsAboutWorld;
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
        /// <param name="currentWorldState"></param>
        /// <param name="agent"></param>
        public void RefreshBeliefsAboutTheWorld(WorldDynamic currentWorldState, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            // Before clearing the information, remember the location in which the agent is located.
            LocationStatic agentIsHereLoc = myBeliefsAboutWorld.SearchAgentAmongLocations(agent.Key);

            // We clear the information about the location in which the agent is located, in his beliefs.
            myBeliefsAboutWorld.GetLocation(myBeliefsAboutWorld.SearchAgentAmongLocations(agent.Key)).ClearLocation(currentWorldState);

            // We find the same location in the "real" world. We go through the agents in it. We are looking for agents 
            //    with the same names in the agent's beliefs. We add them to the location (in his beliefs) where he (in his belief) is.
            foreach (var agent1 in currentWorldState.GetLocationByName(agentIsHereLoc.GetName()).Value.GetAgents())
            {
                foreach (var agent2 in myBeliefsAboutWorld.GetAgents())
                {
                    if (agent1.Key.GetName() == agent2.Key.GetName())
                    {
                        myBeliefsAboutWorld.GetLocation(agentIsHereLoc).AddAgent(agent2);

                        if (!agent2.Value.GetStatus())
                        {
                            foreach (var a in currentWorldState.GetAgents())
                            {
                                a.Value.GetBeliefs().GetAgentByName(agent2.Key.GetName()).Value.SetStatus(false);
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
        }

        public LocationStatic GetTargetLocation()
        {
            return wantsToGo;
        }

        public void ClearTargetLocation()
        {
            wantsToGo = null;
        }

        public void SetObjectOfAngry(AgentStateStatic target)
        {
            angryAt.AngryOn();
            angryAt.SetObjectOfAngry(target);
        }

        public void SetObjectOfAngry(AgentAngryAt angryAt)
        {
            this.angryAt = angryAt;
        }

        public AgentAngryAt GetObjectOfAngry()
        {
            return angryAt;
        }

        public void CalmDown()
        {
            angryAt.AngryOff();
            angryAt.SetObjectOfAngry(null);
        }

        public void AddExploredLocation(LocationStatic location)
        {
            exploredRooms.Add(location);
        }

        public void AddExploredLocations(List<LocationStatic> locations)
        {
            exploredRooms = locations;
        }

        public List<LocationStatic> GetExploredLocations()
        {
            return exploredRooms;
        }

        public LocationStatic GetExploredLocation(int index)
        {
            return exploredRooms[index];
        }

        public bool SearchAmongExploredLocations(LocationStatic location)
        {
            for (int i = 0; i < exploredRooms.Count(); i++)
            {
                if (exploredRooms[i] == location)
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
        }

        public void AddEvidence(AgentFoundEvidence foundEvidence)
        {
            this.foundEvidence = foundEvidence;
        }

        public AgentFoundEvidence GetEvidenceStatus()
        {
            return foundEvidence;
        }

        public void ClearEvidence()
        {
            foundEvidence.Clear();
        }

        /// <summary>
        /// Checks if the agent knows that one of the other agents is angry.
        /// </summary>
        public bool ThinksThatSomeoneIsAngry()
        {
            foreach (var a in myBeliefsAboutWorld.GetAgents())
            {
                if (a.Value.GetObjectOfAngry().AngryCheck())
                {
                    return true;
                }
            }

            return false;
        }

        public void ScaredOn()
        {
            scared = true;
        }

        public void ScaredOff()
        {
            scared = false;
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

        public void SetAgentInfo(AgentStateStatic agentInfo)
        {
            this.agentInfo = agentInfo;
        }

        public AgentStateStatic GetAgentInfo()
        {
            return agentInfo;
        }

        public KeyValuePair<LocationStatic, LocationDynamic> GetMyLocation()
        {
            foreach (var location in GetBeliefs().GetLocations())
            {
                if (location.Value.SearchAgent(GetAgentInfo()))
                {
                    return location;
                }
            }

            throw new KeyNotFoundException();
        }
    }
}