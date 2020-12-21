using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class Agent
    {
        // Supporting components
        private FastDownward fastDownward;

        // Action components
        private Plan myCurrentPlan;
        private List<PlanAction> myAvailableActions;

        // Properties
        private string name;
        private bool alive;
        private string role;
        private Goal myGoals;
        private WorldBeliefs myBeliefsAboutWorld;
        private int initiative;
        private AgentAngryAt angryAt;
        private bool scared;
        private AgentFoundEvidence foundEvidence;
        private Location wantsToGo;
        private List<Location> exploredRooms;

        // Constructor
        public Agent(string name, bool alive, string role, Goal goals, WorldBeliefs beliefs)
        {
            fastDownward = new FastDownward();
            myCurrentPlan = new Plan();
            myAvailableActions = null;

            this.name = name;
            this.alive = alive;
            this.role = role;
            myGoals = goals;
            myBeliefsAboutWorld = beliefs;
            initiative = 0;
            angryAt = new AgentAngryAt(false, null);
            scared = false;
            foundEvidence = new AgentFoundEvidence(false, null);
            wantsToGo = null;
            exploredRooms = null;
        }

        public Agent()
        {

        }

        public void GenerateNewPDDLProblem()
        {
            throw new NotImplementedException();
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
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

        public void AssignRole(string role)
        {
            this.role = role;
        }

        public string GetRole()
        {
            return role;
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

        public void SetInitiative (int initiative)
        {
            this.initiative = initiative;
        }

        public int GetInitiative()
        {
            return initiative;
        }

        // Should depend on factors such as role, location of the agent, and so on. The system shouldn't tell the agent who the killer is.
        public void RefreshBeliefsAboutTheWorld(WorldBeliefs currentWorldState)
        {
            myBeliefsAboutWorld = currentWorldState;
        }

        public void CalculatePlan()
        {
            fastDownward.Run();

            if (fastDownward.isSuccess)
            {
                myCurrentPlan = fastDownward.GetResultPlan();
            }
        }

        public void GetAvailableActions()
        {
            ActionGenerator actionGenerator = new ActionGenerator();
            myAvailableActions = actionGenerator.GetPossibleActions(this);
        }

        public PlanAction ChooseAction()
        {
            for (int i = 0; i < myAvailableActions.Count(); i++)
            {
                if (myCurrentPlan.GetAction(0) == myAvailableActions[i])
                {
                    return myAvailableActions[i];
                }
            }

            return null;
        }

        public void SetTargetLocation(Location location)
        {
            wantsToGo = location;
        }

        public Location GetTargetLocation()
        {
            return wantsToGo;
        }

        public void ClearTargetLocation()
        {
            wantsToGo = null;
        }

        public void SetObjectOfAngry(Agent target)
        {
            angryAt.AngryOn();
            angryAt.SetObjectOfAngry(target);
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

        public void AddExploredLocation(Location location)
        {
            exploredRooms.Add(location);
        }

        public List<Location> GetExploredLocations()
        {
            return exploredRooms;
        }

        public Location GetExploredLocation(int index)
        {
            return exploredRooms[index];
        }

        public bool SearchAmongExploredLocations(Location location)
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

        public void AddEvidence(Agent criminal)
        {
            foundEvidence.IsEvidence();
            foundEvidence.SetCriminal(criminal);
        }

        public AgentFoundEvidence GetEvidenceStatus()
        {
            return foundEvidence;
        }

        public void ClearEvidence()
        {
            foundEvidence.Clear();
        }

        public bool ThinksThatSomeoneIsAngry()
        {
            for (int i = 0; i < myBeliefsAboutWorld.GetNumberOfAgents(); i++)
            {
                if (myBeliefsAboutWorld.GetAgent(i).GetObjectOfAngry().AngryCheck())
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

        public bool CheckScared()
        {
            return scared;
        }
    }
}
