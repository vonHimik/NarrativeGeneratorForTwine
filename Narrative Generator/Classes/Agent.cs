using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class Agent
    {
        private FastDownward fastDownward;

        private Plan myCurrentPlan;
        private List<Action> myAvailableActions;

        private string name;
        private bool alive;
        private string role;
        private Goal myGoals;
        private World myBeliefsAboutWorld;
        private int initiative;

        public Agent(string name, bool alive, string role, Goal goals, World beliefs)
        {
            fastDownward = new FastDownward();
            myCurrentPlan = new Plan();
            myAvailableActions = new List<Action>();

            this.name = name;
            this.alive = alive;
            this.role = role;
            myGoals = goals;
            myBeliefsAboutWorld = beliefs;
            initiative = 0;
        }

        public Agent()
        {

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

        public void SetBeliefs(World beliefs)
        {
            myBeliefsAboutWorld = beliefs;
        }

        public World GetBeliefs()
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

        public void RefreshBeliefsAboutTheWorld(World currentWorldState) // Should depend on factors such as role, location of the agent, and so on. 
                                                                         // The system shouldn't tell the agent who the killer is.
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

        }

        public void ChooseAction(List<Action> availableActions)
        {
            CalculatePlan();
        }

        public void SendAction(Action choosedAction)
        {

        }
    }
}
