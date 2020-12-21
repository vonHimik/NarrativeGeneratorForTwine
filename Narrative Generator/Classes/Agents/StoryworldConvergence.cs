using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class StoryworldConvergence
    {
        private List<WorldConstraint> constraints;
        StoryGraph globalGraph;
        List<Goal> allGoalStates;

        public void CreateGlobalGraph(StoryNode rootNode)
        {
            throw new NotImplementedException();
        }

        public bool ControlToAchieveGoalState()
        {
            throw new NotImplementedException();
        }

        public void AddConstraint(WorldConstraint constraint)
        {
            constraints.Add(constraint);
        }

        public bool ConstraintsControl(PlanAction action)
        {
            throw new NotImplementedException();
        }

        public void ActionRequest(Agent agent, ref StoryGraph currentGraph, ref WorldBeliefs currentState)
        {
            agent.RefreshBeliefsAboutTheWorld(currentState);
            agent.GenerateNewPDDLProblem();
            agent.CalculatePlan();
            agent.GetAvailableActions();
            PlanAction receivedAction = agent.ChooseAction();

            if (receivedAction != null)
            {
                ActionControl(receivedAction, currentGraph, agent, currentState);
            }
            else
            {
                SkipTurn();
            }
        }

        public void ActionControl(PlanAction action, StoryGraph currentGraph, Agent agent, WorldBeliefs currentState)
        {
            if (ConstraintsControl(action) && DeadEndsControl(action) && CyclesControl(action))
            {
                ApplyAction(action, currentGraph, agent, currentState);
            }
            else if (!ConstraintsControl(action) && DeadEndsControl(action) && CyclesControl(action))
            {
                ActionCounteract(action, currentGraph, currentState);
            }
            else
            {
                SkipTurn();
            }
        }

        public void ApplyAction(PlanAction action, StoryGraph currentGraph, Agent agent, WorldBeliefs currentState)
        {
            if (ProbabilityCalculating(action)) // We apply a successful option to perform an action
            {
                action.ApplyEffects(currentState);
            }
            else // We apply an unsuccessful option to perform an action
            {
                //action.Fail();
            }

            CreateNewNode(action, currentGraph, agent, currentState);
        }

        public bool DeadEndsControl(PlanAction action)
        {
            throw new NotImplementedException();
        }

        public bool CyclesControl(PlanAction action)
        {
            throw new NotImplementedException();
        }

        public void ActionCounteract(PlanAction action, StoryGraph currentGraph, WorldBeliefs currentState)
        {
            throw new NotImplementedException();
        }

        public bool ProbabilityCalculating(PlanAction action)
        {
            throw new NotImplementedException();
        }

        public void SkipTurn()
        {
            throw new NotImplementedException();
        }

        public void CreateNewNode(PlanAction action, StoryGraph currentGraph, Agent agent, WorldBeliefs newState)
        {
            StoryNode newNode = new StoryNode();
            Edge newEdge = new Edge();

            newNode.SetActivePlayer(false);
            newNode.SetActiveAgent(agent);
            newNode.SetWorldState(newState);
            newNode.SetParentNode(currentGraph.GetNodes().Last());
            currentGraph.GetNodes().Last().AddChildrenNode(newNode);
            newEdge.SetAction(action);
            newEdge.SetUpperNode(currentGraph.GetNodes().Last());
            newEdge.SetLowerNode(newNode);

            currentGraph.AddNode(newNode);
            currentGraph.AddEdge(newEdge);
        }
    }
}
