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
        // Action components
        private Plan myCurrentPlan;
        private List<PlanAction> myAvailableActions;

        private bool alive;
        private Goal myGoals;
        private WorldBeliefs myBeliefsAboutWorld;
        private int initiative;
        private AgentAngryAt angryAt;
        private bool scared;
        private AgentFoundEvidence foundEvidence;
        private LocationStatic wantsToGo;
        private List<LocationStatic> exploredRooms;

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public AgentStateDynamic() {}

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="alive"></param>
        /// <param name="goals"></param>
        /// <param name="beliefs"></param>
        public AgentStateDynamic(bool alive, Goal goals, WorldBeliefs beliefs)
        {
            myCurrentPlan = new Plan();
            myAvailableActions = null;

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

            AddExploredLocations(null);
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
            clone.myBeliefsAboutWorld = (WorldBeliefs)myBeliefsAboutWorld.Clone();
            clone.initiative = initiative;
            clone.angryAt = (AgentAngryAt)angryAt.Clone();
            clone.scared = scared;
            clone.foundEvidence = (AgentFoundEvidence)foundEvidence.Clone();
            clone.wantsToGo = (LocationStatic)wantsToGo.Clone();
            clone.exploredRooms = exploredRooms;

            return clone;
        }

        /// <summary>
        /// Generate a new PDDL file with a problem for the specified agent, based on his beliefs.
        /// </summary>
        /// <param name="agent"></param>
        public void GenerateNewPDDLProblem(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            string fileName = null;
            string problemName = null;
            string domainName = null;
            string objects = null;
            string init = null;
            string goal = null;

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
                objects.Insert(objects.Length + 1, location.Key.GetName() + " ");
                init.Insert(init.Length + 1, "(ROOM " + location.Key.GetName() + ") ");
            }

            foreach (var a in agent.Value.GetBeliefs().GetAgents())
            {
                objects.Insert(objects.Length + 1, a.Key.GetName() + " ");

                switch (a.Key.GetRole())
                {
                    case AgentRole.USUAL:
                        init.Insert(init.Length + 1, "(AGENT " + a.Key.GetName() + ") ");
                        break;
                    case AgentRole.KILLER:
                        init.Insert(init.Length + 1, "(KILLER " + a.Key.GetName() + ") ");
                        break;
                }

                switch (a.Value.GetStatus())
                {
                    case true:
                        init.Insert(init.Length + 1, "(alive " + a.Key.GetName() + ") ");
                        break;
                    case false:
                        init.Insert(init.Length + 1, "(died " + a.Key.GetName() + ") ");
                        break;
                }

                init.Insert(init.Length + 1, "(in-room " + a.Key.GetName() + " " +
                    a.Value.GetBeliefs().GetLocationByName(a.Value.GetBeliefs().SearchAgentAmongLocations(a.Key).GetName()).Key.GetName());
            }

            if (agent.Value.GetGoal().goalTypeStatus)
            {
                switch (agent.Key.GetRole())
                {
                    case AgentRole.USUAL:
                        foreach (var a in agent.Value.GetBeliefs().GetAgents())
                        {
                            if (a.Key.GetRole() == AgentRole.KILLER)
                            {
                                goal.Insert(goal.Length + 1, "(died " + a.Key.GetName() + ") ");
                            }
                        }
                        break;
                    case AgentRole.KILLER:
                        foreach (var a in agent.Value.GetBeliefs().GetAgents())
                        {
                            if (a.Key.GetRole() == AgentRole.USUAL)
                            {
                                goal.Insert(goal.Length + 1, "(died " + a.Key.GetName() + ") ");
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
        public void CalculatePlan(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            FastDownward fastDownward = new FastDownward();

            string domainFileName = null;
            string problemFileName = null;

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

            fastDownward.Run(domainFileName, problemFileName);

            if (fastDownward.isSuccess)
            {
                myCurrentPlan = fastDownward.GetResultPlan();
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
            for (int i = 0; i < myAvailableActions.Count(); i++) // We go through all the actions available to the agent.
            {
                if (myCurrentPlan.GetAction(0) == myAvailableActions[i]) // If one of the available actions matches the first action in the plan.
                {
                    return myAvailableActions[i]; // Then select it.
                }
            }

            return null; // If none of the available actions coincided with the first in the plan, then return null.
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
            myGoals = goal;
        }

        public Goal GetGoal()
        {
            return myGoals;
        }

        public void SetBeliefs(WorldBeliefs beliefs)
        {
            myBeliefsAboutWorld = beliefs;
        }

        public WorldBeliefs GetBeliefs()
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
        public void RefreshBeliefsAboutTheWorld(WorldBeliefs currentWorldState, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            // We find (in the beliefs of the agent) the location where he (in his opinion) is. We clean it up.
            myBeliefsAboutWorld.GetLocation(myBeliefsAboutWorld.SearchAgentAmongLocations(agent.Key)).ClearLocation();

            // We find the same location in the "real" world. We go through the agents in it. We are looking for agents 
            //    with the same names in the agent's beliefs. We add them to the location (in his beliefs) where he (in his belief) is.
            foreach (var agent1 in currentWorldState.GetLocation(myBeliefsAboutWorld.SearchAgentAmongLocations(agent.Key)).GetAgents())
            {
                foreach (var agent2 in myBeliefsAboutWorld.GetAgents())
                {
                    if (agent1.Key.GetName() == agent2.Key.GetName())
                    {
                        myBeliefsAboutWorld.GetLocation(myBeliefsAboutWorld.SearchAgentAmongLocations(agent.Key)).
                            AddAgent(myBeliefsAboutWorld.SearchAgentAmongLocations(agent.Key), agent2);
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
    }
}