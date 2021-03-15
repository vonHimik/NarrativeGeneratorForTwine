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
        //private StoryGraph globalGraph;
        private List<Goal> allGoalStates;

        public StoryworldConvergence()
        {
            constraints = new List<WorldConstraint>();
            allGoalStates = new List<Goal>();
        }

        /// <summary>
        /// Collects goals from all agents and adds them to the goal list.
        /// </summary>
        /// <param name="currentState"></param>
        public void ExtractGoals(WorldDynamic currentState)
        {
            foreach (var agent in currentState.GetAgents())
            {
                allGoalStates.Add(agent.Value.GetGoal());
            }
        }

        /// <summary>
        /// Checks the achievement of any of the goal conditions (in state).
        /// </summary>
        /// <param name="currentWorldState"></param>
        public bool ControlToAchieveGoalState(WorldDynamic currentWorldState)
        {
            foreach (var goal in allGoalStates)
            {
                if (goal.goalTypeIsStatus)
                {
                    int counter = 0;

                    foreach (var agent1 in currentWorldState.GetAgents())
                    {
                        foreach (var agent2 in goal.GetGoalState().GetAgents())
                        {

                            if (agent1.Key.GetRole() == agent2.Key.GetRole() && agent1.Value.GetStatus() == agent2.Value.GetStatus())
                            {
                                counter++;

                                if (counter >= goal.GetGoalState().GetAgents().Count())
                                {
                                    return true;
                                }

                                break;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Adds the specified constraint to the constraint list.
        /// </summary>
        /// <param name="constraint"></param>
        public void AddConstraint(WorldConstraint constraint)
        {
            constraints.Add(constraint);
        }

        /// <summary>
        /// Checking whether the application of an action would violate the established constraints.
        /// </summary>
        /// <param name="currentState"></param>
        /// <param name="action"></param>
        public bool ConstraintsControl(WorldDynamic currentState, PlanAction action)
        {
            WorldDynamic worldForTest = (WorldDynamic)currentState.Clone();
            action.ApplyEffects(ref worldForTest);

            foreach (var constraint in constraints)
            {
                if (!constraint.IsSatisfied(worldForTest)) { return false; }
            }

            return true;
        }

        /// <summary>
        /// The agent updates his beliefs, calculates a plan, chooses an action, assigns variables to it, and sends it for further control.
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="currentGraph"></param>
        /// <param name="currentState"></param>
        public void ActionRequest(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                  ref StoryGraph currentGraph, 
                                  ref WorldDynamic currentState)
        {
            CSP_Module cspModule = new CSP_Module();

            agent.Value.RefreshBeliefsAboutTheWorld(currentState, agent);
            agent.Value.GenerateNewPDDLProblem(agent, currentState);
            agent.Value.CalculatePlan(agent, currentState);
            agent.Value.GetAvailableActions(agent);
            PlanAction receivedAction = agent.Value.ChooseAction();

            if (receivedAction != null)
            {
                cspModule.AssignVariables(ref receivedAction, currentState, agent);
                ActionControl(receivedAction, currentGraph, agent, currentState);
            }
            else
            {
                SkipTurn(currentState);
            }
        }

        /// <summary>
        /// Checking the action for violation of the established constraints and the reachability of the goal state (control of cycles and deadends).
        /// </summary>
        /// <param name="action"></param>
        /// <param name="currentGraph"></param>
        /// <param name="agent"></param>
        /// <param name="currentState"></param>
        public void ActionControl(PlanAction action, 
                                  StoryGraph currentGraph, 
                                  KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                  WorldDynamic currentState)
        {
            bool constraintsControl = ConstraintsControl(currentState, action);
            bool deadEndsControl = DeadEndsControl(action, currentState, agent);
            bool cyclesControl = CyclesControl(currentState, action, currentGraph);

            if (constraintsControl && deadEndsControl && cyclesControl)
            {
                // If all checks are passed, then we apply the action.
                ApplyAction(action, currentGraph, agent, currentState);
            }
            else if (!constraintsControl && deadEndsControl && cyclesControl)
            {
                // If the action violates the constraints, then convergence will not apply it, but will apply its counter-reaction.
                ActionCounteract(action, currentGraph, agent, currentState);
            }
            else
            {
                // If the application of an action leads to a cycle or deadend, then it will not be applied, and the turn will go to the next agent.
                SkipTurn(currentState);
            }
        }

        /// <summary>
        /// The probability of success of the action is calculated, and if successful, it is applied.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="currentGraph"></param>
        /// <param name="agent"></param>
        /// <param name="currentState"></param>
        public void ApplyAction(PlanAction action, 
                                StoryGraph currentGraph, 
                                KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                WorldDynamic currentState)
        {
            if (ProbabilityCalculating(action)) // We apply a successful option to perform an action
            {
                action.ApplyEffects(ref currentState);
                CreateNewNode(action, currentGraph, agent, currentState);
            }
            else // We apply an unsuccessful option to perform an action
            {
                action.Fail();
                // TO DO - Should I create a node for failed actions?
            }
        }

        public bool DeadEndsControl(PlanAction action, WorldDynamic currentState, KeyValuePair<AgentStateStatic, AgentStateDynamic> player)
        {
            if (player.Key.GetRole() == AgentRole.PLAYER)
            {
                WorldDynamic worldForTest = (WorldDynamic)currentState.Clone();
                action.ApplyEffects(ref worldForTest);
                worldForTest.GetAgentByRole(AgentRole.PLAYER).Value.CalculatePlan(worldForTest.GetAgentByRole(AgentRole.PLAYER), worldForTest);

                if (worldForTest.GetAgentByRole(AgentRole.PLAYER).Value.GetPlanStatus()) { return true; }
                else { return false; }
            }
            else { return true; }
        }

        public bool CyclesControl(WorldDynamic currentState, PlanAction action, StoryGraph currentGraph)
        {
            StoryNode testNode = CreateTestNode(currentState, action, currentGraph);
            
            if (NodeExistenceControl(testNode, currentGraph)) { return false; }
            else { return true; }
        }

        public void ActionCounteract(PlanAction action, StoryGraph currentGraph, KeyValuePair<AgentStateStatic,AgentStateDynamic> agent, 
                                     WorldDynamic currentState)
        {
            // TODO: Depending on the action, convergence has at least one way to counter it.
            //       To do this, counter-actions must also be formalized as subclasses of the "PlanAction" class.

            if (action is Entrap)
            {

            }
            else if (action is Fight)
            {

            }
            else if (action is InvestigateRoom)
            {

            }
            else if (action is Kill)
            {

            }
            else if (action is Move)
            {

            }
            // At the moment, this is the only action that can violate the constraints.
            else if (action is NeutralizeKiller)
            {
                MiraculousSalvation counterreaction = new MiraculousSalvation();
                counterreaction.Arguments.Add(action.Arguments[1]);
                ApplyAction(counterreaction, currentGraph, agent, currentState);
            }
            else if (action is NothingToDo)
            {

            }
            else if (action is Reassure)
            {

            }
            else if (action is Run)
            {

            }
            else if (action is TellAboutASuspicious)
            {

            }
        }

        /// <summary>
        /// Calculation of the probability of success of the action.
        /// </summary>
        /// <param name="action"></param>
        public bool ProbabilityCalculating(PlanAction action)
        {
            Random random = new Random();
            int probability = random.Next(0, 100);
            int threshold = 0;

            if (action is Entrap)
            {
                threshold = 80;
            }
            else if (action is Fight)
            {
                threshold = 75;
            }
            else if (action is InvestigateRoom)
            {
                threshold = 20;
            }
            else if (action is Kill)
            {
                threshold = 100;
            }
            else if (action is Move)
            {
                threshold = 100;
            }
            else if (action is NeutralizeKiller)
            {
                threshold = 100;
            }
            else if (action is NothingToDo)
            {
                threshold = 100;
            }
            else if (action is Reassure)
            {
                threshold = 80;
            }
            else if (action is Run)
            {
                threshold = 100;
            }
            else if (action is TellAboutASuspicious)
            {
                threshold = 80;
            }

            if (probability <= threshold)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// To skip a turn (action), an action "NothingToDo" is created and applied.
        /// </summary>
        /// <param name="currentState"></param>
        public void SkipTurn(WorldDynamic currentState)
        {
            NothingToDo skipAction = new NothingToDo();
            skipAction.ApplyEffects(ref currentState);
        }

        /// <summary>
        /// Create a new node for the story graph and inserts it.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="currentGraph"></param>
        /// <param name="agent"></param>
        /// <param name="newState"></param>
        public void CreateNewNode(PlanAction action, 
                                  StoryGraph currentGraph, 
                                  KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                  WorldDynamic newState)
        {
            StoryNode newNode = new StoryNode();
            Edge newEdge = new Edge();
            KeyValuePair<AgentStateStatic, AgentStateDynamic> newAgent = 
                new KeyValuePair<AgentStateStatic, AgentStateDynamic>((AgentStateStatic)agent.Key.Clone(), (AgentStateDynamic)agent.Value.Clone());

            currentGraph.GetLastNode().SetActivePlayer(false);
            currentGraph.GetLastNode().SetActiveAgent(newAgent);

            newNode.SetWorldState((WorldDynamic)newState.Clone());
            newNode.SetParentNode(currentGraph.GetLastNode());
            currentGraph.GetLastNode().AddChildrenNode(ref newNode);

            newEdge.SetAction(action);
            newEdge.SetUpperNode(currentGraph.GetLastNode());
            newEdge.SetLowerNode(ref newNode);

            currentGraph.GetLastNode().AddEdge(newEdge);
            currentGraph.AddNode(newNode);
            currentGraph.AddEdge(newEdge);
        }

        public StoryNode CreateTestNode(WorldDynamic currentState, PlanAction action, StoryGraph currentGraph)
        {
            WorldDynamic worldForTest = (WorldDynamic)currentState.Clone();
            action.ApplyEffects(ref worldForTest);

            StoryNode testNode = new StoryNode();
            testNode.SetWorldState(worldForTest);
            testNode.SetParentNode(currentGraph.GetLastNode());

            return testNode;
        }

        public bool NodeExistenceControl(StoryNode checkedNode, StoryGraph currentGraph)
        {
            foreach (var node in currentGraph.GetNodes())
            {
                if (TwoNodesComparison(node, checkedNode))
                {
                    return true;
                }
            }

            return false;
        }

        public bool TwoNodesComparison(StoryNode nodeOne, StoryNode nodeTwo)
        {
            if (nodeOne.GetWorldState().Equals(nodeTwo.GetWorldState()) && 
                nodeOne.GetActiveAgent().Equals(nodeTwo.GetActiveAgent()) &&
                nodeOne.GetActivePlayer() == nodeTwo.GetActivePlayer() &&
                nodeOne.GetParentNode().Equals(nodeTwo.GetParentNode()))
            {
                return true;
            }

            return false;
        }
    }
}