using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class StoryworldConvergence
    {
        private List<WorldConstraint> constraints;
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
                if (!constraint.IsSatisfied(worldForTest))
                {
                    // Cleaning
                    worldForTest = null;
                    GC.Collect();

                    // Return result
                    return false;
                }
            }

            // Cleaning
            worldForTest = null;
            GC.Collect();

            // Return result
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
                                  ref WorldDynamic currentState,
                                  StoryNode currentNode,
                                  bool root,
                                  ref int globalNodeNumber)
        {
            CSP_Module cspModule = new CSP_Module();

            /*if (agent.Key.GetRole().Equals(AgentRole.PLAYER))
            {
                agent.Value.RefreshBeliefsAboutTheWorld(currentState, agent);
                agent.Value.GenerateNewPDDLProblem(agent, currentState);
                agent.Value.ReceiveAvailableActions(agent);

                List<PlanAction> receivedActions = agent.Value.GetAvailableActions();

                for (int i = 0; i < receivedActions.Count; i++)
                {
                    PlanAction receivedAction = receivedActions[i];

                    if (receivedAction != null)
                    {
                        switch (receivedAction.GetType().ToString().Remove(0, 20))
                        {
                            case "Move":
                                MultiAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode);
                                break;
                            case "Fight": // Not relevant yet
                                break;
                            case "InvestigateRoom":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode);
                                break;
                            case "NeutralizeKiller": // Not relevant yet
                                break;
                            case "NothingToDo": SkipTurn(currentState);
                                break;
                            case "Reassure": // Not relevant yet
                                break;
                            case "Run": // Not relevant yet
                                break;
                        }

                        // Cleaning
                        receivedAction = null;
                        GC.Collect();
                    }
                    else
                    {
                        SkipTurn(currentState);
                    }
                }

                // Cleaning
                receivedActions = null;
                GC.Collect();
            }
            else
            {*/
                agent.Value.RefreshBeliefsAboutTheWorld(currentState, agent);
                agent.Value.GenerateNewPDDLProblem(agent, currentState);
                agent.Value.CalculatePlan(agent, currentState);
                agent.Value.ReceiveAvailableActions(agent);

                PlanAction receivedAction = agent.Value.ChooseAction();

                if (receivedAction != null)
                {
                    cspModule.AssignVariables(ref receivedAction, currentState, agent);

                    ActionControl(receivedAction, currentGraph, agent, currentState, currentNode, false, root, ref globalNodeNumber);

                    // Cleaning
                    receivedAction = null;
                    currentNode = null;
                    GC.Collect();
                }
                else
                {
                    SkipTurn(currentState);
                }
            //}
        }

        public void SingleAVandAC(ref PlanAction receivedAction, 
                                  WorldDynamic currentState, 
                                  KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                  CSP_Module cspModule, 
                                  StoryGraph currentGraph,
                                  StoryNode currentNode,
                                  bool root,
                                  ref int globalNodeNumber)
        {
            cspModule.AssignVariables(ref receivedAction, currentState, agent);

            ActionControl(receivedAction, currentGraph, agent, currentState, currentNode, false, root, ref globalNodeNumber);

            // Cleaning
            currentNode = null;
            GC.Collect();
        }

        public void MultiAVandAC(ref PlanAction receivedAction, 
                                 WorldDynamic currentState, 
                                 KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                                 CSP_Module cspModule, 
                                 StoryGraph currentGraph,
                                 StoryNode currentNode,
                                 bool root,
                                 int globalNodeNumber)
        {
            List<PlanAction> actionsList = cspModule.MassiveAssignVariables(ref receivedAction, currentState, agent);

            AgentStateStatic sCurrentAgent = (AgentStateStatic)agent.Key.Clone();
            AgentStateDynamic dCurrentAgent = (AgentStateDynamic)agent.Value.Clone();
            KeyValuePair<AgentStateStatic, AgentStateDynamic> currentAgent = 
                                                                  new KeyValuePair<AgentStateStatic, AgentStateDynamic>(sCurrentAgent, dCurrentAgent);

            WorldDynamic statePrefab = (WorldDynamic)currentState.Clone();

            foreach (var a in actionsList)
            {
                ActionControl(a, currentGraph, currentAgent, statePrefab, currentNode, true, root, ref globalNodeNumber);
            }

            // Cleaning
            actionsList = null;
            currentNode = null;
            statePrefab = null;
            GC.Collect();
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
                                  WorldDynamic currentState,
                                  StoryNode currentNode,
                                  bool duplication,
                                  bool root,
                                  ref int globalNodeNumber)
        {
            bool constraintsControl = ConstraintsControl(currentState, action);
            bool deadEndsControl = DeadEndsControl(action, currentState, agent);
            bool duplicateControl = DuplicateControl(currentState, action, currentGraph, agent, currentNode);
            bool cyclesControl = CyclesControl(currentState, action, currentGraph, agent, currentNode);

            if (constraintsControl && deadEndsControl && cyclesControl && duplicateControl)
            {
                // If all checks are passed, then we apply the action.
                ApplyAction(action, currentGraph, agent, currentState, currentNode, duplication, root, ref globalNodeNumber);
            }
            else if (!constraintsControl && deadEndsControl && cyclesControl && duplicateControl)
            {
                // If the action violates the constraints, then convergence will not apply it, but will apply its counter-reaction.
                ActionCounteract(action, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber);
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
                                WorldDynamic currentState,
                                StoryNode currentNode,
                                bool duplication,
                                bool root,
                                ref int globalNodeNumber)
        {
            // We apply a successful option to perform an action.
            if (ProbabilityCalculating(action))
            {
                if (duplication)
                {
                    WorldDynamic duplicatedState = (WorldDynamic)currentState.Clone();
                    action.ApplyEffects(ref duplicatedState);

                    if (root) { currentGraph.CreateRootNode(action, agent, duplicatedState, currentNode, ref globalNodeNumber); }
                    else { currentGraph.CreateNewNode(action, agent, duplicatedState, currentNode, ref globalNodeNumber); }
                }
                else
                {
                    action.ApplyEffects(ref currentState);

                    if (root) { currentGraph.CreateRootNode(action, agent, currentState, currentNode, ref globalNodeNumber); }
                    else { currentGraph.CreateNewNode(action, agent, currentState, currentNode, ref globalNodeNumber); }
                }
            }
            // We apply an unsuccessful option to perform an action.
            else
            {
                action.Fail();

                if (!root) { currentGraph.CreateNewNode(action, agent, currentState, currentNode, ref globalNodeNumber); }
                else { currentGraph.CreateRootNode(action, agent, currentState, currentNode, ref globalNodeNumber); }
            }
        }

        public bool DeadEndsControl(PlanAction action, WorldDynamic currentState, KeyValuePair<AgentStateStatic, AgentStateDynamic> player)
        {
            if (player.Key.GetRole() == AgentRole.PLAYER)
            {
                WorldDynamic worldForTest = (WorldDynamic)currentState.Clone();
                action.ApplyEffects(ref worldForTest);
                worldForTest.GetAgentByRole(AgentRole.PLAYER).Value.CalculatePlan(worldForTest.GetAgentByRole(AgentRole.PLAYER), worldForTest);

                if (worldForTest.GetAgentByRole(AgentRole.PLAYER).Value.GetPlanStatus())
                {
                    // Cleaning
                    worldForTest = null;
                    GC.Collect();

                    // Return result
                    return true;
                }
                else
                {
                    // Cleaning
                    worldForTest = null;
                    GC.Collect();

                    // Return result
                    return false;
                }
            }
            else { return true; }
        }

        public bool DuplicateControl(WorldDynamic currentState,
                                     PlanAction action,
                                     StoryGraph currentGraph,
                                     KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                                     StoryNode currentNode)
        {
            StoryNode testNode = currentGraph.CreateTestNode(currentState, action, agent, currentNode, false);

            Stack<StoryNode> stack = new Stack<StoryNode>();
            HashSet<StoryNode> visitedNodes = new HashSet<StoryNode>();

            stack.Push(currentGraph.GetRoot());
            visitedNodes.Add(currentGraph.GetRoot());

            while (stack.Count > 0)
            {
                StoryNode checkedNode = stack.Pop();

                if (currentGraph.TwoNodesComparison(testNode, checkedNode))
                {
                    currentGraph.ConnectionTwoNodes(action, currentNode, checkedNode);
                    currentGraph.DeleteTestNode(testNode);
                    return false;
                }

                foreach (StoryNode nextNode in checkedNode.GetLinks())
                {
                    if (visitedNodes.Contains(nextNode)) continue;

                    stack.Push(nextNode);
                    visitedNodes.Add(nextNode);
                }
            }

            currentGraph.DeleteTestNode(testNode);
            return true;
        }

        public bool CyclesControl(WorldDynamic currentState, 
                                  PlanAction action, 
                                  StoryGraph currentGraph, 
                                  KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                  StoryNode currentNode)
        {
            StoryNode testNode = currentGraph.CreateTestNode(currentState, action, agent, currentNode, true);

            Stack<StoryNode> stack = new Stack<StoryNode>();
            HashSet<StoryNode> visitedNodes = new HashSet<StoryNode>();

            StoryNode oldNode = new StoryNode();

            stack.Push(testNode);
            visitedNodes.Add(testNode);

            while (stack.Count > 0)
            {
                StoryNode checkedNode = stack.Pop();

                foreach (StoryNode nextNode in checkedNode.GetLinks())
                {
                    if (visitedNodes.Contains(nextNode))
                    {
                        if (nextNode.Equals(oldNode)) { continue; }
                        else
                        {
                            currentGraph.DeleteTestNode(testNode);
                            return false;
                        }
                    };

                    stack.Push(nextNode);
                    visitedNodes.Add(nextNode);
                }

                oldNode = checkedNode;
            }

            currentGraph.DeleteTestNode(testNode);
            return true;
        }

        public void ActionCounteract(PlanAction action, 
                                     StoryGraph currentGraph, 
                                     KeyValuePair<AgentStateStatic,AgentStateDynamic> agent, 
                                     WorldDynamic currentState, 
                                     StoryNode currentNode,
                                     bool root,
                                     ref int globalNodeNumber)
        {
            // TODO: Depending on the action, convergence has at least one way to counter it.
            //       To do this, counter-actions must also be formalized as subclasses of the "PlanAction" class.

            if (action is Entrap)
            {

            }
            else if (action is Fight)
            {
                MiraculousSalvation counterreaction = new MiraculousSalvation();
                counterreaction.Arguments.Add(action.Arguments[1]);
                ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, false, root, ref globalNodeNumber);
            }
            // There is no restriction on this action.
            else if (action is InvestigateRoom) {}
            else if (action is Kill)
            {
                MiraculousSalvation counterreaction = new MiraculousSalvation();
                counterreaction.Arguments.Add(action.Arguments[0]);
                ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, false, root, ref globalNodeNumber);
            }
            else if (action is Move)
            {
                UnexpectedObstacle counterreaction = new UnexpectedObstacle();
                counterreaction.Arguments.Add(action.Arguments[0]);
                ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, false, root, ref globalNodeNumber);
            }
            // At the moment, this is the only action that can violate the constraints.
            else if (action is NeutralizeKiller)
            {
                MiraculousSalvation counterreaction = new MiraculousSalvation();
                counterreaction.Arguments.Add(action.Arguments[1]);
                ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, false, root, ref globalNodeNumber);
            }
            // There is no restriction on this action.
            else if (action is NothingToDo) {}
            else if (action is Reassure)
            {
                AngerIsInTheAir counterreaction = new AngerIsInTheAir();
                counterreaction.Arguments.Add(action.Arguments[0]);
                ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, false, root, ref globalNodeNumber);
            }
            else if (action is Run)
            {
                UnexpectedObstacle counterreaction = new UnexpectedObstacle();
                counterreaction.Arguments.Add(action.Arguments[0]);
                ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, false, root, ref globalNodeNumber);
            }
            else if (action is TellAboutASuspicious)
            {
                EnvironmentIsNotConduciveToTalk counterreaction = new EnvironmentIsNotConduciveToTalk();
                counterreaction.Arguments.Add(action.Arguments[1]);
                ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, false, root, ref globalNodeNumber);
            }
            else if (action is Talk)
            {
                EnvironmentIsNotConduciveToTalk counterreaction = new EnvironmentIsNotConduciveToTalk();
                counterreaction.Arguments.Add(action.Arguments[0]);
                ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, false, root, ref globalNodeNumber);
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

            if (action is Entrap) { threshold = 80; }
            else if (action is Fight) { threshold = 75; }
            else if (action is InvestigateRoom) { threshold = 20; }
            else if (action is Kill) { threshold = 100; }
            else if (action is Move) { threshold = 100; }
            else if (action is NeutralizeKiller) { threshold = 100; }
            else if (action is NothingToDo) { threshold = 100; }
            else if (action is Reassure) { threshold = 80; }
            else if (action is Run) { threshold = 100; }
            else if (action is TellAboutASuspicious) { threshold = 80; }

            if (probability <= threshold) { return true; }
            else { return false; }
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
    }
}