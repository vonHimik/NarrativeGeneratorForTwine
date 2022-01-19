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
        /// <param name="currentWorldState"></param>
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
        public void AddConstraint(WorldConstraint constraint)
        {
            constraints.Add(constraint);
        }

        /// <summary>
        /// Checking whether the application of an action would violate the established constraints.
        /// </summary>
        public bool ConstraintsControl(WorldDynamic currentState, PlanAction action, bool succsessControl)
        {
            WorldDynamic worldForTest = (WorldDynamic)currentState.Clone();
            if (!succsessControl) { action.Fail(ref worldForTest); }
            else { action.ApplyEffects(ref worldForTest); }

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
                                  ref int globalNodeNumber,
                                  ref bool skip,
                                  ref Queue<StoryNode> queue)
        {
            CSP_Module cspModule = new CSP_Module();

            if (agent.Key.GetRole().Equals(AgentRole.PLAYER))
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
                                MultiAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root, 
                                                ref globalNodeNumber, ref skip, ref queue);
                                break;
                            case "Fight": // Not relevant yet
                                break;
                            case "InvestigateRoom":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root, 
                                                 ref globalNodeNumber, ref skip, ref queue);
                                break;
                            case "NeutralizeKiller": // Not relevant yet
                                break;
                            case "NothingToDo": SkipTurn(currentState, ref skip);
                                break;
                            case "Reassure": // Not relevant yet
                                break;
                            case "Run": // Not relevant yet
                                break;
                            case "Talk": // Not relevant yet
                                break;
                        }

                        // Cleaning
                        receivedAction = null;
                        GC.Collect();
                    }
                    else
                    {
                        SkipTurn(currentState, ref skip);
                    }
                }

                // Cleaning
                receivedActions = null;
                GC.Collect();
            }
            else
            {
                agent.Value.RefreshBeliefsAboutTheWorld(currentState, agent);
                agent.Value.GenerateNewPDDLProblem(agent, currentState);
                agent.Value.CalculatePlan(agent, currentState);
                agent.Value.ReceiveAvailableActions(agent);

                PlanAction receivedAction = agent.Value.ChooseAction();

                if (receivedAction != null)
                {
                    cspModule.AssignVariables(ref receivedAction, currentState, agent);

                    ActionControl(receivedAction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref skip, ref queue);

                    // Cleaning
                    receivedAction = null;
                    currentNode = null;
                    GC.Collect();
                }
                else
                {
                    SkipTurn(currentState, ref skip);
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
                                  ref bool skip,
                                  ref Queue<StoryNode> queue)
        {
            cspModule.AssignVariables(ref receivedAction, currentState, agent);

            ActionControl(receivedAction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref skip, ref queue);

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
                                 ref bool skip,
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
                ActionControl(a, currentGraph, currentAgent, statePrefab, currentNode, root, ref globalNodeNumber, ref skip, ref queue);
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
                                  ref bool skip,
                                  ref Queue<StoryNode> queue)
        {
            bool succsessControl = ProbabilityCalculating(action);

            action.success = succsessControl;
            action.fail = !succsessControl;

            bool constraintsControl = ConstraintsControl(currentState, action, succsessControl);
            bool deadEndsControl = DeadEndsControl(action, currentState, agent, succsessControl);
            bool duplicateControl = DuplicateControl(currentState, action, currentGraph, agent, currentNode, globalNodeNumber, succsessControl);
            bool cyclesControl = CyclesControl(currentState, action, currentGraph, agent, currentNode, duplicateControl, globalNodeNumber, succsessControl);

            if (constraintsControl && deadEndsControl && cyclesControl && duplicateControl)
            {
                // If all checks are passed, then we apply the action.
                ApplyAction(action, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, succsessControl, false);
            }
            else if (!constraintsControl && deadEndsControl && cyclesControl && duplicateControl)
            {
                // If the action violates the constraints, then convergence will not apply it, but will apply its counter-reaction.
                ActionCounteract(action, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref skip, ref queue);
            }
            else if (!duplicateControl && cyclesControl)
            {
                // connection current node --> finded node
                currentGraph.DuplicateNodeConnecting(currentState, action, agent, currentNode, ref skip, globalNodeNumber, ref queue);
            }
            else
            {
                ActionCounteract(action, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref skip, ref queue);
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
                foreach (var edge in checkedNode.GetEdges())
                {
                    if (duplicated)
                    {
                        // Игнорируем в том случае, если рассматриваемый узел является родительским для проверяемого и он не является дубликатом.
                        //if (edge.GetUpperNode() == nextNode && edge.GetLowerNode() == checkedNode && !duplicatedNode.Equals(nextNode)) { continue; }
                    }
                    else
                    {
                        //if (edge.GetUpperNode() == nextNode && edge.GetLowerNode() == checkedNode) { continue; }
                    }
                }

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
                                     ref bool skip,
                                     ref Queue<StoryNode> queue)
        {
            // TODO: Depending on the action, convergence has at least one way to counter it.
            //       To do this, counter-actions must also be formalized as subclasses of the "PlanAction" class.

            if (action is Entrap)
            {
                // Если у убийцы уже есть целевая локацмя.
                if (currentState.GetAgentByName(agent.Key.GetName()).Value.GetTargetLocation() != null)
                {
                    // Если целевая локация не равна той, в которой и так находится жертва.
                    if (currentState.GetAgentByName(agent.Key.GetName()).Value.GetTargetLocation() != 
                        currentState.GetAgentByName(((KeyValuePair<AgentStateStatic, AgentStateDynamic>)action.Arguments[0]).
                                                                                                         Key.GetName()).Value.GetMyLocation()
                        )
                    {
                        // Поменять целевую локацию на новую.
                        // И применить действие.

                        ChangeOfMood counterreaction = new ChangeOfMood();
                        counterreaction.Arguments.Add(currentState.GetAgentByName(agent.Key.GetName()));
                        counterreaction.Arguments.Add(currentState.GetLocationByName(currentState.GetAgentByName(((
                            KeyValuePair<AgentStateStatic, AgentStateDynamic>)action.Arguments[0]).Key.GetName()).Value.GetMyLocation().GetName()));

                        bool controlOne = false;
                        bool controlTwo = false;

                        CounterreactionControl
                            (counterreaction, currentGraph, agent, currentState, currentNode, root,
                            ref globalNodeNumber, ref skip, ref queue, ref controlOne, ref controlTwo);

                        if (controlOne && controlTwo)
                        {
                            ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, false, false);
                        }
                        else if (controlOne && !controlTwo)
                        {
                            currentGraph.DuplicateNodeConnecting(currentState, counterreaction, agent, currentNode, ref skip, globalNodeNumber, ref queue);
                        }
                        else if (!controlOne)
                        {
                            Move newAction = new Move();
                            newAction.Arguments.Add(action.Arguments[1]);
                            newAction.Arguments.Add(action.Arguments[2]);
                            newAction.Arguments.Add(currentState.GetLocationByName(currentState.GetLocationByName(agent.Value.GetMyLocation().GetName()).
                                Key.GetConnectedLocations().ElementAt(0).GetName()));

                            ActionControl(newAction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref skip, ref queue);
                        }
                    }
                    // Если убийца уже направляется в ту локацию, где находится жертва.
                    else
                    {
                        // Совершить действие Move.
                        Move newAction = new Move();
                        // Кто
                        // Откуда
                        // Куда
                        newAction.Arguments.Add(action.Arguments[1]);
                        newAction.Arguments.Add(action.Arguments[2]);
                        newAction.Arguments.Add(currentState.GetAgentByName(agent.Key.GetName()).Value.GetTargetLocation());

                        ActionControl(newAction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref skip, ref queue);
                    }
                }
                else
                {
                    // Нужно назначить целевую локацию убийце.
                    // И применить действие.
                    ChangeOfMood counterreaction = new ChangeOfMood();
                    counterreaction.Arguments.Add(currentState.GetAgentByName(agent.Key.GetName()));
                    counterreaction.Arguments.Add(currentState.GetAgentByName(((
                        KeyValuePair<AgentStateStatic, AgentStateDynamic>)action.Arguments[0]).Key.GetName()).Value.GetMyLocation());

                    bool controlOne = false;
                    bool controlTwo = false;

                    CounterreactionControl
                        (counterreaction, currentGraph, agent, currentState, currentNode, root,
                        ref globalNodeNumber, ref skip, ref queue, ref controlOne, ref controlTwo);

                    if (controlOne && controlTwo)
                    {
                        ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, false, false);
                    }
                    else if (controlOne && !controlTwo)
                    {
                        currentGraph.DuplicateNodeConnecting(currentState, counterreaction, agent, currentNode, ref skip, globalNodeNumber, ref queue);
                    }
                    else if (!controlOne)
                    {
                        Move newAction = new Move();
                        newAction.Arguments.Add(action.Arguments[1]);
                        newAction.Arguments.Add(action.Arguments[2]);
                        newAction.Arguments.Add(agent.Value.GetMyLocation().GetConnectedLocations().ElementAt(0));

                        ActionControl(newAction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref skip, ref queue);
                    }
                }

            }
            else if (action is Fight)
            {
                MiraculousSalvation counterreaction = new MiraculousSalvation();
                counterreaction.Arguments.Add(action.Arguments[1]);
                ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, false, true);
            }
            // There is no restriction on this action.
            else if (action is InvestigateRoom)
            {
                bool test = true;
            }
            else if (action is Kill)
            {
                MiraculousSalvation counterreaction = new MiraculousSalvation();
                counterreaction.Arguments.Add(action.Arguments[0]);
                ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, false, true);
            }
            else if (action is Move)
            {
                UnexpectedObstacle counterreaction = new UnexpectedObstacle();
                counterreaction.Arguments.Add(action.Arguments[0]);
                counterreaction.Arguments.Add(action.Arguments[1]);

                KeyValuePair<LocationStatic, LocationDynamic> targetLocation = (KeyValuePair<LocationStatic, LocationDynamic>)action.Arguments[2];

                Dictionary<LocationStatic, LocationDynamic> locationAround = new Dictionary<LocationStatic, LocationDynamic>();
                foreach (var location in agent.Value.GetMyLocation().GetConnectedLocations())
                {
                    locationAround.Add(location, currentNode.GetWorldState().GetLocationByName(location.GetName()).Value);
                }

                bool controlOne = false;
                bool controlTwo = false;

                if (locationAround.Count != 0)
                {
                    foreach (var location in locationAround)
                    {
                        controlOne = false;
                        controlTwo = false;
                        Move newAction = new Move();
                        newAction.Arguments.Add(action.Arguments[0]);
                        newAction.Arguments.Add(action.Arguments[1]);
                        newAction.Arguments.Add(location);
                        CounterreactionControl
                            (newAction, currentGraph, agent, currentState, currentNode, root, 
                            ref globalNodeNumber, ref skip, ref queue, ref controlOne, ref controlTwo);

                        if (controlOne)
                        {
                            counterreaction.Arguments.Add(location);
                            break;
                        }
                    }
                }

                if (controlOne && controlTwo)
                {
                    ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true);
                }
                else if (controlOne && !controlTwo)
                {
                    currentGraph.DuplicateNodeConnecting(currentState, counterreaction, agent, currentNode, ref skip, globalNodeNumber, ref queue);
                }
                else if (locationAround.Count == 0 || !controlOne)
                {
                    if (controlTwo)
                    {
                        counterreaction.Arguments.Add(action.Arguments[2]);
                        ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true);
                    }
                    else
                    {
                        KeyValuePair<LocationStatic, LocationDynamic>  randomLocation = 
                            currentNode.GetWorldState().GetRandomLocationWithout((KeyValuePair<LocationStatic, LocationDynamic>)action.Arguments[1]);

                        Move newAction = new Move();
                        newAction.Arguments.Add(action.Arguments[0]);
                        newAction.Arguments.Add(action.Arguments[1]);
                        newAction.Arguments.Add(randomLocation);

                        bool duplicateTest = DuplicateControl(currentState, newAction, currentGraph, agent, currentNode, globalNodeNumber, true);

                        if (duplicateTest)
                        {
                            counterreaction.Arguments.Add(randomLocation);
                            ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true);
                        }
                        else
                        {
                            counterreaction.Arguments.Add(randomLocation);
                            currentGraph.DuplicateNodeConnecting(currentState, counterreaction, agent, currentNode, ref skip, globalNodeNumber, ref queue);
                        }
                    }
                }
            }
            // At the moment, this is the only action that can violate the constraints.
            else if (action is NeutralizeKiller)
            {
                MiraculousSalvation counterreaction = new MiraculousSalvation();
                counterreaction.Arguments.Add(action.Arguments[1]);
                ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, false, true);
            }
            // There is no restriction on this action.
            else if (action is NothingToDo) {}
            else if (action is Reassure)
            {
                AngerIsInTheAir counterreaction = new AngerIsInTheAir();
                counterreaction.Arguments.Add(action.Arguments[0]);
                ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, false, true);
            }
            else if (action is Run)
            {
                UnexpectedObstacle counterreaction = new UnexpectedObstacle();
                counterreaction.Arguments.Add(action.Arguments[0]);
                ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, false, true);
            }
            else if (action is TellAboutASuspicious)
            {
                EnvironmentIsNotConduciveToTalk counterreaction = new EnvironmentIsNotConduciveToTalk();
                counterreaction.Arguments.Add(action.Arguments[1]);
                ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, false, true);
            }
            else if (action is Talk)
            {
                EnvironmentIsNotConduciveToTalk counterreaction = new EnvironmentIsNotConduciveToTalk();
                counterreaction.Arguments.Add(action.Arguments[0]);
                ApplyAction(counterreaction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, false, true);
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
            else if (action is Fight) { threshold = 75; }
            else if (action is InvestigateRoom) { threshold = 100; }
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
        public void SkipTurn(WorldDynamic currentState, ref bool skip)
        {
            NothingToDo skipAction = new NothingToDo();
            skipAction.ApplyEffects(ref currentState);
            skip = true;
        }

        public void CounterreactionControl (PlanAction action,
                                            StoryGraph currentGraph,
                                            KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                                            WorldDynamic currentState,
                                            StoryNode currentNode,
                                            bool root,
                                            ref int globalNodeNumber,
                                            ref bool skip,
                                            ref Queue<StoryNode> queue,
                                            ref bool controlOne,
                                            ref bool controlTwo)
        {
            bool succsessControl = ProbabilityCalculating(action);

            action.success = succsessControl;
            action.fail = !succsessControl;

            bool constraintsControl = ConstraintsControl(currentState, action, succsessControl);
            bool deadEndsControl = DeadEndsControl(action, currentState, agent, succsessControl);
            bool duplicateControl = DuplicateControl(currentState, action, currentGraph, agent, currentNode, globalNodeNumber, succsessControl);
            bool cyclesControl = CyclesControl(currentState, action, currentGraph, agent, currentNode, duplicateControl, globalNodeNumber, succsessControl);

            controlOne = constraintsControl & deadEndsControl & cyclesControl;
            controlTwo = duplicateControl;
        }
    }
}