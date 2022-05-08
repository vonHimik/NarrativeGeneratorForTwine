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
        public bool ControlToAchieveGoalState(ref StoryNode currentNode)
        {
            foreach (var goal in allGoalStates)
            {
                // todo: convert to switch
                // todo: supplement the types of goals - group and specific

                if (goal.goalTypeIsStatus)
                {
                    int killCounter = 0;
                    bool killerDied = false;

                    foreach (var agent in currentNode.GetWorldState().GetAgents())
                    {
                        switch (agent.Key.GetRole())
                        {
                            case AgentRole.USUAL: if(!agent.Value.GetStatus()) { killCounter++; } break;
                            case AgentRole.PLAYER: if(!agent.Value.GetStatus()) { killCounter++; } break;
                            case AgentRole.KILLER: if(!agent.Value.GetStatus()) { killerDied = true; } break;
                            case AgentRole.BOSS: if (!agent.Value.GetStatus()) { killerDied = true; } break;
                        }
                    }

                    if (killCounter == currentNode.GetWorldState().GetAgents().Count - 1)
                    {
                        currentNode.goalState = true;
                        return true;
                    }
                    if (killerDied)
                    {
                        currentNode.goalState = true;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Adds the specified constraint to the constraint list.
        /// </summary>
        public void AddConstraint (WorldConstraint constraint) { constraints.Add(constraint); }

        /// <summary>
        /// Checking whether the application of an action would violate the established constraints.
        /// </summary>
        public bool ConstraintsControl (StoryGraph currentGraph, 
                                        WorldDynamic currentState, 
                                        PlanAction action, 
                                        bool succsessControl, 
                                        StoryNode currentNode)
        {
            WorldDynamic worldForTest = (WorldDynamic)currentState.Clone();
            if (!succsessControl) { action.Fail(ref worldForTest); }
            else { action.ApplyEffects(ref worldForTest); }

            foreach (var constraint in constraints)
            {
                if (!constraint.IsSatisfied(worldForTest, currentState, currentGraph, action, currentNode))
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
                                  ref int globalNodeNumber,
                                  ref Queue<StoryNode> queue)
        {
            CSP_Module cspModule = new CSP_Module();

            if (agent.Key.GetRole().Equals(AgentRole.PLAYER))
            {
                agent.Value.RefreshBeliefsAboutTheWorld(agent, currentState);
                agent.Value.GenerateNewPDDLProblem(agent, currentState);
                agent.Value.ReceiveAvailableActions(agent, currentState);

                List<PlanAction> receivedActions = agent.Value.GetAvailableActions();

                for (int i = 0; i < receivedActions.Count; i++)
                {
                    PlanAction receivedAction = receivedActions[i];

                    if (receivedAction != null)
                    {
                        switch (receivedAction.GetType().ToString().Remove(0, 20))
                        {
                            case "Move":
                                MultiAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root, 
                                                ref globalNodeNumber, ref queue);
                                break;
                            case "Fight":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                              ref globalNodeNumber, ref queue);
                                break;
                            case "InvestigateRoom":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root, 
                                              ref globalNodeNumber, ref queue);
                                break;
                            case "NeutralizeKiller":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                              ref globalNodeNumber, ref queue);
                                break;
                            case "NothingToDo":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                              ref globalNodeNumber, ref queue);
                                break;
                            case "Reassure":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                              ref globalNodeNumber, ref queue);
                                break;
                            case "Talk":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                              ref globalNodeNumber, ref queue);
                                break;
                            case "HelpElfs":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                    ref globalNodeNumber, ref queue);
                                break;
                            case "HelpWerewolves":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                    ref globalNodeNumber, ref queue);
                                break;
                            case "HelpMages":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                    ref globalNodeNumber, ref queue);
                                break;
                            case "HelpTemplars":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                    ref globalNodeNumber, ref queue);
                                break;
                            case "HelpPrinceBelen":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                    ref globalNodeNumber, ref queue);
                                break;
                            case "HelpLordHarrowmont":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                    ref globalNodeNumber, ref queue);
                                break;
                        }

                        // Cleaning
                        receivedAction = null;
                        GC.Collect();
                    }
                }

                // Cleaning
                receivedActions = null;
                GC.Collect();
            }
            else
            {
                agent.Value.RefreshBeliefsAboutTheWorld(agent, currentState);
                agent.Value.GenerateNewPDDLProblem(agent, currentState);
                agent.Value.CalculatePlan(agent, currentState);
                agent.Value.ReceiveAvailableActions(agent, currentState);

                PlanAction receivedAction = agent.Value.ChooseAction();

                if (receivedAction != null)
                {
                    cspModule.AssignVariables(ref receivedAction, currentState, agent);

                    ActionControl(receivedAction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue);

                    // Cleaning
                    receivedAction = null;
                    currentNode = null;
                    GC.Collect();
                }
                else
                {
                    SkipTurn(currentState);
                }
            }
        }

        public void SingleAVandAC(ref PlanAction receivedAction, 
                                  WorldDynamic currentState, 
                                  KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                  CSP_Module cspModule, 
                                  StoryGraph currentGraph,
                                  StoryNode currentNode,
                                  bool root,
                                  ref int globalNodeNumber,
                                  ref Queue<StoryNode> queue)
        {
            cspModule.AssignVariables(ref receivedAction, currentState, agent);

            ActionControl(receivedAction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue);

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
                                 ref int globalNodeNumber,
                                 ref Queue<StoryNode> queue)
        {
            List<PlanAction> actionsList = cspModule.MassiveAssignVariables(ref receivedAction, currentState, agent);

            AgentStateStatic sCurrentAgent = (AgentStateStatic)agent.Key.Clone();
            AgentStateDynamic dCurrentAgent = (AgentStateDynamic)agent.Value.Clone();
            KeyValuePair<AgentStateStatic, AgentStateDynamic> currentAgent = 
                                                                  new KeyValuePair<AgentStateStatic, AgentStateDynamic>(sCurrentAgent, dCurrentAgent);

            WorldDynamic statePrefab = (WorldDynamic)currentState.Clone();

            foreach (var a in actionsList)
            {
                ActionControl(a, currentGraph, currentAgent, statePrefab, currentNode, root, ref globalNodeNumber, ref queue);
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
        public void ActionControl(PlanAction action, 
                                  StoryGraph currentGraph, 
                                  KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                  WorldDynamic currentState,
                                  StoryNode currentNode,
                                  bool root,
                                  ref int globalNodeNumber,
                                  ref Queue<StoryNode> queue)
        {
            bool succsessControl = ProbabilityCalculating(action);

            action.success = succsessControl;
            action.fail = !succsessControl;

            bool constraintsControl = ConstraintsControl(currentGraph, currentState, action, succsessControl, currentNode);
            bool deadEndsControl = DeadEndsControl(action, currentState, agent, succsessControl);
            bool duplicateControl = DuplicateControl(currentState, action, currentGraph, agent, currentNode, globalNodeNumber, succsessControl);
            bool cyclesControl = CyclesControl(currentState, action, currentGraph, agent, currentNode, duplicateControl, globalNodeNumber, succsessControl);

            if (!constraintsControl && agent.Key.GetRole().Equals(AgentRole.PLAYER))
            {
                currentGraph.CreateEndNode();
            }
            else if (constraintsControl && deadEndsControl && cyclesControl && duplicateControl)
            {
                // If all checks are passed, then we apply the action.
                ApplyAction(action, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, succsessControl, false);
            }
            else if (!constraintsControl && deadEndsControl && cyclesControl && duplicateControl)
            {
                // If the action violates the constraints, then convergence will not apply it, but will apply its counter-reaction.
                ActionCounteract(action, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue);
            }
            else if (!duplicateControl && cyclesControl)
            {
                // connection current node --> finded node
<<<<<<< Updated upstream
                currentGraph.DuplicateNodeConnecting(currentState, action, agent, currentNode, globalNodeNumber, ref queue, succsessControl, ref skip);

                if (skip)
                {
                    ActionCounteract(action, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue);
                }
=======
                currentGraph.DuplicateNodeConnecting(currentState, action, agent, currentNode, globalNodeNumber, ref queue);
>>>>>>> Stashed changes
            }
            else
            {
                ActionCounteract(action, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue);
            }
        }

        /// <summary>
        /// The probability of success of the action is calculated, and if successful, it is applied.
        /// </summary>
        public void ApplyAction(PlanAction action, 
                                StoryGraph currentGraph, 
                                KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                WorldDynamic currentState,
                                StoryNode currentNode,
                                bool root,
                                ref int globalNodeNumber,
                                bool succsessControl,
                                bool counteract)
        {
            // We apply a successful/unsuccessful option to perform an action.
            if (root) { currentGraph.CreateRootNode(action, agent, currentState, currentNode, ref globalNodeNumber, succsessControl); }
            else { currentGraph.CreateNewNode(action, agent, currentState, currentNode, ref globalNodeNumber, succsessControl, counteract); }
        }

        public bool DeadEndsControl(PlanAction action, 
                                    WorldDynamic currentState, 
                                    KeyValuePair<AgentStateStatic, AgentStateDynamic> player, 
                                    bool succsessControl)
        {
            if (player.Key.GetRole() == AgentRole.PLAYER)
            {
                WorldDynamic worldForTest = (WorldDynamic)currentState.Clone();
                if (!succsessControl) { action.Fail(ref worldForTest); }
                else { action.ApplyEffects(ref worldForTest); }

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
                                     StoryNode currentNode,
                                     int globalNodeNumber,
                                     bool succsessControl)
        {
            StoryNode testNode = currentGraph.CreateTestNode(currentState, action, agent, currentNode, false, globalNodeNumber, succsessControl);
            testNode.UpdateHashCode();

            foreach (var checkedNode in currentGraph.GetNodes())
            {
                checkedNode.UpdateHashCode();
                if (currentGraph.TwoNodesComparison(testNode, checkedNode))
                {
                    currentGraph.DeleteTestNode(ref testNode);
                    return false;
                }
            }

            currentGraph.DeleteTestNode(ref testNode);
            return true;
        }

        public bool CyclesControl(WorldDynamic currentState, 
                                  PlanAction action, 
                                  StoryGraph currentGraph, 
                                  KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                  StoryNode currentNode,
                                  bool duplicated,
                                  int globalNodeNumber,
                                  bool succsessControl)
        {
            bool result = false;

            // We create a test node similar to the one we are going to add to the graph as a result of the current action.
            StoryNode testNode = currentGraph.CreateTestNode(currentState, action, agent, currentNode, !duplicated, globalNodeNumber, succsessControl);

            StoryNode duplicatedNode = null;
            Edge testEdge = new Edge();

            if (!duplicated)
            {
                duplicatedNode = currentGraph.GetNode(testNode);

                if (currentNode.Equals(duplicatedNode))
                {
                    return false;
                }

                testEdge.SetUpperNode(ref currentNode);
                testEdge.SetLowerNode(ref duplicatedNode);

                currentNode.AddEdge(testEdge);
                duplicatedNode.AddEdge(testEdge);

                currentNode.AddLinkToNode(ref duplicatedNode);
                duplicatedNode.AddLinkToNode(ref currentNode);
            }

            string[] colors = new string[currentGraph.GetNodes().Count + 2];
            for (int i = 0; i < currentGraph.GetNodes().Count + 2; i++) { colors[i] = "white"; }

            result = TarjanAlgStep(currentGraph.GetRoot(), ref colors, !duplicated, duplicatedNode);

            if (!duplicated)
            {
                currentNode.RemoveEdge(testEdge);
                currentNode.DeleteLink(duplicatedNode);
                duplicatedNode.RemoveEdge(testEdge);
                duplicatedNode.DeleteLink(currentNode);
                testEdge.ClearUpperNode();
                testEdge.ClearLowerNode();
                testEdge = null;
            }

            // We delete the test node and mark the loop test as passed.
            currentGraph.DeleteTestNode(ref testNode);

            return result;
        }

        public bool TarjanAlgStep(StoryNode checkedNode, ref string[] colors, bool duplicated, StoryNode duplicatedNode)
        {
            bool result = true;

            colors[checkedNode.GetNumberInSequence()] = "grey";

            foreach (StoryNode nextNode in checkedNode.GetLinks())
            {
                if (!result)
                {
                    return result;
                }

                if (colors[nextNode.GetNumberInSequence()] == "grey")
                {
                    if (duplicated)
                    {
                        if (duplicatedNode.Equals(nextNode))
                        {
                            bool test = true;
                        }
                    }

                    if (checkedNode.isChildren(nextNode) && !duplicated)
                    {
                        continue;
                    }
                    else if (duplicated && checkedNode.isChildren(nextNode) && !duplicatedNode.Equals(nextNode))
                    {
                        continue;
                    }

                    result = false;
                    return result;
                }
                else if (colors[nextNode.GetNumberInSequence()] == "black") { continue; }
                else
                {
                    result = TarjanAlgStep(nextNode, ref colors, duplicated, duplicatedNode);
                    if (!result)
                    {
                        return result;
                    }
                }
            }

            colors[checkedNode.GetNumberInSequence()] = "black";

            return result;
        }

        public void ActionCounteract(PlanAction action, 
                                     StoryGraph currentGraph, 
                                     KeyValuePair<AgentStateStatic,AgentStateDynamic> agent, 
                                     WorldDynamic currentState, 
                                     StoryNode currentNode,
                                     bool root,
                                     ref int globalNodeNumber,
                                     ref Queue<StoryNode> queue)
        {
            CSP_Module cspModule = new CSP_Module();

             bool stageOne_NewNode = true;
             bool stageTwo_ConnectedNode = false;
             bool counterreactionFound = false;

            string currentAction = action.GetType().ToString().Remove(0, 20);

             while (!counterreactionFound)
             {
                 PlanAction counterreactionTalk = new CounterTalk();
                 bool assignVariables = cspModule.AssignVariables(ref counterreactionTalk, currentState, agent);
                 counterreactionTalk.Arguments.Add(currentAction);

                 if (assignVariables && counterreactionTalk.CheckPreconditions(currentState))
                 {
                     bool constractionAndDeadEndAndCicle = false;
                     bool duplicate = false;

                     CounterreactionControl(counterreactionTalk, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                            ref constractionAndDeadEndAndCicle, ref duplicate);

                     if (stageOne_NewNode)
                     {
                         if (constractionAndDeadEndAndCicle && duplicate)
                         {
                             ApplyAction(counterreactionTalk, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true);
                             counterreactionFound = true;
                         }
                     }
                     else if (stageTwo_ConnectedNode)
                     {
                         if (constractionAndDeadEndAndCicle && !duplicate)
                         {
                             currentGraph.DuplicateNodeConnecting(currentState, counterreactionTalk, agent, currentNode, globalNodeNumber, ref queue);
                             counterreactionFound = true;
                         }
                     }
                 }

                 if (!counterreactionFound)
                 {
                     PlanAction counterreactionEntrap = new CounterEntrap();
                     assignVariables = cspModule.AssignVariables(ref counterreactionEntrap, currentState, agent);
                     counterreactionEntrap.Arguments.Add(currentAction);

                     if (assignVariables && counterreactionEntrap.CheckPreconditions(currentState))
                     {
                         bool constractionAndDeadEndAndCicle = false;
                         bool duplicate = false;

                         CounterreactionControl(counterreactionEntrap, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                                ref constractionAndDeadEndAndCicle, ref duplicate);

                         if (stageOne_NewNode)
                         {
                             if (constractionAndDeadEndAndCicle && duplicate)
                             {
                                 ApplyAction(counterreactionEntrap, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true);
                                 counterreactionFound = true;
                             }
                         }
                         else if (stageTwo_ConnectedNode)
                         {
                             if (constractionAndDeadEndAndCicle && !duplicate)
                             {
                                 currentGraph.DuplicateNodeConnecting(currentState, counterreactionEntrap, agent, currentNode, globalNodeNumber, ref queue);
                                 counterreactionFound = true;
                             }
                         }
                     }
                }

                if (!counterreactionFound)
                {
                    PlanAction counterreactionKill = new CounterKill();
                    assignVariables = cspModule.AssignVariables(ref counterreactionKill, currentState, agent);
                    counterreactionKill.Arguments.Add(currentAction);

                    if (assignVariables && counterreactionKill.CheckPreconditions(currentState))
                    {
                        bool constractionAndDeadEndAndCicle = false;
                        bool duplicate = false;

                        CounterreactionControl(counterreactionKill, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                               ref constractionAndDeadEndAndCicle, ref duplicate);

                        if (stageOne_NewNode)
                        {
                            if (constractionAndDeadEndAndCicle && duplicate)
                            {
                                ApplyAction(counterreactionKill, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true);
                                counterreactionFound = true;
                            }
                        }
                        else if (stageTwo_ConnectedNode)
                        {
                            if (constractionAndDeadEndAndCicle && !duplicate)
                            {
                                currentGraph.DuplicateNodeConnecting(currentState, counterreactionKill, agent, currentNode, globalNodeNumber, ref queue);
                                counterreactionFound = true;
                            }
                        }
                    }
                }

                if (!counterreactionFound)
                {
                     PlanAction counterreactionIR = new InvestigateRoom();
                     assignVariables = cspModule.AssignVariables(ref counterreactionIR, currentState, agent);
                     counterreactionIR.Arguments.Add(currentAction);

                     if (assignVariables && counterreactionIR.CheckPreconditions(currentState))
                     {
                         bool constractionAndDeadEndAndCicle = false;
                         bool duplicate = false;

                         CounterreactionControl(counterreactionIR, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                                ref constractionAndDeadEndAndCicle, ref duplicate);

                         if (stageOne_NewNode)
                         {
                             if (constractionAndDeadEndAndCicle && duplicate)
                             {
                                 ApplyAction(counterreactionIR, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true);
                                 counterreactionFound = true;
                             }
                         }
                         else if (stageTwo_ConnectedNode)
                         {
                             if (constractionAndDeadEndAndCicle && !duplicate)
                             {
                                 currentGraph.DuplicateNodeConnecting(currentState, counterreactionIR, agent, currentNode, globalNodeNumber, ref queue);
                                 counterreactionFound = true;
                             }
                         }
                     }
                 }

                 if (!counterreactionFound)
                 {
                     PlanAction counterreactionNK = new CounterNeutralizeKiller();
                     assignVariables = cspModule.AssignVariables(ref counterreactionNK, currentState, agent);
                     counterreactionNK.Arguments.Add(currentAction);

                     if (assignVariables && counterreactionNK.CheckPreconditions(currentState))
                     {
                         bool constractionAndDeadEndAndCicle = false;
                         bool duplicate = false;

                         CounterreactionControl(counterreactionNK, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                                ref constractionAndDeadEndAndCicle, ref duplicate);

                         if (stageOne_NewNode)
                         {
                             if (constractionAndDeadEndAndCicle && duplicate)
                             {
                                 ApplyAction(counterreactionNK, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true);
                                 counterreactionFound = true;
                             }
                         }
                         else if (stageTwo_ConnectedNode)
                         {
                             if (constractionAndDeadEndAndCicle && !duplicate)
                             {
                                 currentGraph.DuplicateNodeConnecting(currentState, counterreactionNK, agent, currentNode, globalNodeNumber, ref queue);
                                 counterreactionFound = true;
                             }
                         }
                     }
                 }

                 if (!counterreactionFound)
                 {
                     PlanAction counterreactionTalkAboutSuspicious = new CounterTellAboutASuspicious();
                     assignVariables = cspModule.AssignVariables(ref counterreactionTalkAboutSuspicious, currentState, agent);
                     counterreactionTalkAboutSuspicious.Arguments.Add(currentAction);

                     if (assignVariables && counterreactionTalkAboutSuspicious.CheckPreconditions(currentState))
                     {
                         bool constractionAndDeadEndAndCicle = false;
                         bool duplicate = false;

                         CounterreactionControl(counterreactionTalkAboutSuspicious, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                                ref constractionAndDeadEndAndCicle, ref duplicate);

                         if (stageOne_NewNode)
                         {
                             if (constractionAndDeadEndAndCicle && duplicate)
                             {
                                 ApplyAction(counterreactionTalkAboutSuspicious, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true);
                                 counterreactionFound = true;
                             }
                         }
                         else if (stageTwo_ConnectedNode)
                         {
                             if (constractionAndDeadEndAndCicle && !duplicate)
                             {
                                 currentGraph.DuplicateNodeConnecting(currentState, counterreactionTalkAboutSuspicious, agent, currentNode, globalNodeNumber, ref queue);
                                 counterreactionFound = true;
                             }
                         }
                     }
                 }

                 if (!counterreactionFound)
                 {
                     PlanAction counterreactionFight = new CounterFight();
                     assignVariables = cspModule.AssignVariables(ref counterreactionFight, currentState, agent);
                     counterreactionFight.Arguments.Add(currentAction);

                     if (assignVariables && counterreactionFight.CheckPreconditions(currentState))
                     {
                         bool constractionAndDeadEndAndCicle = false;
                         bool duplicate = false;

                         CounterreactionControl(counterreactionFight, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                                ref constractionAndDeadEndAndCicle, ref duplicate);

                         if (stageOne_NewNode)
                         {
                             if (constractionAndDeadEndAndCicle && duplicate)
                             {
                                 ApplyAction(counterreactionFight, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true);
                                 counterreactionFound = true;
                             }
                         }
                         else if (stageTwo_ConnectedNode)
                         {
                             if (constractionAndDeadEndAndCicle && !duplicate)
                             {
                                 currentGraph.DuplicateNodeConnecting(currentState, counterreactionFight, agent, currentNode, globalNodeNumber, ref queue);
                                 counterreactionFound = true;
                             }
                         }
                     }
                 }

                 if (!counterreactionFound)
                 {
                     PlanAction counterreactionReassure = new CounterReassure();
                     assignVariables = cspModule.AssignVariables(ref counterreactionReassure, currentState, agent);
                     counterreactionReassure.Arguments.Add(currentAction);

                     if (assignVariables && counterreactionReassure.CheckPreconditions(currentState))
                     {
                         bool constractionAndDeadEndAndCicle = false;
                         bool duplicate = false;

                         CounterreactionControl(counterreactionReassure, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                                ref constractionAndDeadEndAndCicle, ref duplicate);

                         if (stageOne_NewNode)
                         {
                             if (constractionAndDeadEndAndCicle && duplicate)
                             {
                                 ApplyAction(counterreactionReassure, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true);
                                 counterreactionFound = true;
                             }
                         }
                         else if (stageTwo_ConnectedNode)
                         {
                             if (constractionAndDeadEndAndCicle && !duplicate)
                             {
                                 currentGraph.DuplicateNodeConnecting(currentState, counterreactionReassure, agent, currentNode, globalNodeNumber, ref queue);
                                 counterreactionFound = true;
                             }
                         }
                     }
                 }

                 if (!counterreactionFound)
                 {
                     PlanAction counterreactionRun = new CounterRun();
                     assignVariables = cspModule.AssignVariables(ref counterreactionRun, currentState, agent);
                     counterreactionRun.Arguments.Add(currentAction);

                     if (assignVariables && counterreactionRun.CheckPreconditions(currentState))
                     {
                         bool constractionAndDeadEndAndCicle = false;
                         bool duplicate = false;

                         CounterreactionControl(counterreactionRun, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                                ref constractionAndDeadEndAndCicle, ref duplicate);

                         if (stageOne_NewNode)
                         {
                             if (constractionAndDeadEndAndCicle && duplicate)
                             {
                                 ApplyAction(counterreactionRun, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true);
                                 counterreactionFound = true;
                             }
                         }
                         else if (stageTwo_ConnectedNode)
                         {
                             if (constractionAndDeadEndAndCicle && !duplicate)
                             {
                                 currentGraph.DuplicateNodeConnecting(currentState, counterreactionRun, agent, currentNode, globalNodeNumber, ref queue);
                                 counterreactionFound = true;
                             }
                         }
                     }
                 }

                 if (!counterreactionFound)
                 {
                     PlanAction counterreactionMove = new CounterMove();
                     assignVariables = cspModule.AssignVariables(ref counterreactionMove, currentState, agent);
                     counterreactionMove.Arguments.Add(currentAction);

                     if (assignVariables && counterreactionMove.CheckPreconditions(currentState))
                     {
                         bool constractionAndDeadEndAndCicle = false;
                         bool duplicate = false;

                         CounterreactionControl(counterreactionMove, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                                ref constractionAndDeadEndAndCicle, ref duplicate);

                         if (stageOne_NewNode)
                         {
                             if (constractionAndDeadEndAndCicle && duplicate)
                             {
                                 ApplyAction(counterreactionMove, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true);
                                 counterreactionFound = true;
                             }
                         }
                         else if (stageTwo_ConnectedNode)
                         {
                             if (constractionAndDeadEndAndCicle && !duplicate)
                             {
                                 currentGraph.DuplicateNodeConnecting(currentState, counterreactionMove, agent, currentNode, globalNodeNumber, ref queue);
                                 counterreactionFound = true;
                             }
                         }
                     }
                 }

                 if (stageTwo_ConnectedNode)
                {
                    PlanAction counterreactionSkip = new NothingToDo();
                    counterreactionSkip.Arguments.Add(agent);

                    bool constractionAndDeadEndAndCicle = false;
                    bool duplicate = false;

                    CounterreactionControl(counterreactionSkip, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                           ref constractionAndDeadEndAndCicle, ref duplicate);

                    if (constractionAndDeadEndAndCicle && duplicate)
                    {
                        ApplyAction(counterreactionSkip, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true);
                        counterreactionFound = true;
                    }
                    else if (constractionAndDeadEndAndCicle && !duplicate)
                    {
                        currentGraph.DuplicateNodeConnecting(currentState, counterreactionSkip, agent, currentNode, globalNodeNumber, ref queue);
                        counterreactionFound = true;
                    }
                }

<<<<<<< Updated upstream
                if (stageOne_NewNode)
=======
                stageOne_NewNode = false;
                stageTwo_ConnectedNode = true;


                /*if (stageOne_NewNode)
>>>>>>> Stashed changes
                {
                    stageOne_NewNode = false;
                    stageTwo_ConnectedNode = true;
                }
                else if (stageTwo_ConnectedNode)
                {
                    stageOne_NewNode = true;
                    stageTwo_ConnectedNode = false;
                }*/
            }
            
        }

        /// <summary>
        /// Calculation of the probability of success of the action.
        /// </summary>
        public bool ProbabilityCalculating(PlanAction action)
        {
            Random random = new Random();
            int probability = random.Next(0, 100);
            int threshold = 0;

            if (action is Entrap) { threshold = 80; }
            else if (action is CounterEntrap) { threshold = 100; }
            else if (action is Fight) { threshold = 75; }
            else if (action is CounterFight) { threshold = 100; }
            else if (action is InvestigateRoom || action is CounterInvestigateRoom) { threshold = 60; }
            else if (action is Kill || action is CounterKill) { threshold = 100; }
            else if (action is Move || action is CounterMove) { threshold = 100; }
            else if (action is NeutralizeKiller || action is CounterNeutralizeKiller) { threshold = 100; }
            else if (action is NothingToDo) { threshold = 100; }
            else if (action is Reassure) { threshold = 80; }
            else if (action is CounterReassure) { threshold = 100; }
            else if (action is Run || action is CounterRun) { threshold = 100; }
            else if (action is TellAboutASuspicious) { threshold = 80; }
            else if (action is CounterTellAboutASuspicious) { threshold = 100; }
            else if (action is Talk || action is CounterTalk) { threshold = 100; }
            else if (action is ToBeAWitness) { threshold = 100; }
            else if (action is HelpElfs) { threshold = 100; }
            else if (action is HelpWerewolves) { threshold = 100; }
            else if (action is HelpMages) { threshold = 100; }
            else if (action is HelpTemplars) { threshold = 100; }
            else if (action is HelpPrinceBelen) { threshold = 100; }
            else if (action is HelpLordHarrowmont) { threshold = 100; }

            if (probability <= threshold) { return true; }
            else { return false; }
        }

        /// <summary>
        /// To skip a turn (action), an action "NothingToDo" is created and applied.
        /// </summary>
        public void SkipTurn(WorldDynamic currentState)
        {
            NothingToDo skipAction = new NothingToDo();
            skipAction.ApplyEffects(ref currentState);
        }

        public void CounterreactionControl (PlanAction action,
                                            StoryGraph currentGraph,
                                            KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                                            WorldDynamic currentState,
                                            StoryNode currentNode,
                                            bool root,
                                            ref int globalNodeNumber,
                                            ref Queue<StoryNode> queue,
                                            ref bool controlOne,
                                            ref bool controlTwo)
        {
            bool succsessControl = ProbabilityCalculating(action);

            action.success = succsessControl;
            action.fail = !succsessControl;

            bool constraintsControl = ConstraintsControl(currentGraph, currentState, action, succsessControl, currentNode);
            bool deadEndsControl = DeadEndsControl(action, currentState, agent, succsessControl);
            bool duplicateControl = DuplicateControl(currentState, action, currentGraph, agent, currentNode, globalNodeNumber, succsessControl);
            bool cyclesControl = CyclesControl(currentState, action, currentGraph, agent, currentNode, duplicateControl, globalNodeNumber, succsessControl);

            controlOne = constraintsControl & deadEndsControl & cyclesControl;
            controlTwo = duplicateControl;
        }
    }
}