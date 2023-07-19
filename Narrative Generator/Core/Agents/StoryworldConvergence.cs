using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that implements the process of performing an action at all its stages: planning, checking, applying effects.
    /// </summary>
    class StoryworldConvergence
    {
        /// <summary>
        /// List of constraints imposed on the world of this story.
        /// </summary>
        private List<WorldConstraint> constraints;

        /// <summary>
        /// Constructor without parameters.
        /// </summary>
        public StoryworldConvergence() { constraints = new List<WorldConstraint>(); }
        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="constraints">List of constraints.</param>
        public StoryworldConvergence (List<WorldConstraint> constraints) { this.constraints = constraints; }

        /// <summary>
        /// Adds the specified constraint to the constraint list.
        /// </summary>
        /// <param name="constraint">The constraint instance to add.</param>
        public void AddConstraint (WorldConstraint constraint) { constraints.Add(constraint); }

        /// <summary>
        /// A method that replaces the list of constraints with the specified one.
        /// </summary>
        /// <param name="constraints">New list of constraints.</param>
        public void SetConstraints (List<WorldConstraint> constraints) { this.constraints = constraints; }

        /// <summary>
        /// Checking whether the application of an action would violate the established constraints.
        /// </summary>
        /// <param name="currentGraph">Generated story graph.</param>
        /// <param name="currentState">The current state of the story world.</param>
        /// <param name="action">The action currently performed by the agent.</param>
        /// <param name="succsessControl">A variable expressing the success or failure of an action.</param>
        /// <param name="currentNode">The graph node currently being considered.</param>
        /// <param name="note">Text to display on the main screen.</param>
        /// <returns>True if the check was successful, false otherwise.</returns>
        public bool ConstraintsControl (StoryGraph currentGraph, 
                                        WorldDynamic currentState, 
                                        PlanAction action, 
                                        ref bool succsessControl, 
                                        StoryNode currentNode,
                                        ref TextBox note)
        {
            note.Text = "CONSTRAINTS CONTROL";

            WorldDynamic worldForTest = (WorldDynamic)currentState.Clone();
            if (!succsessControl)
            {
                action.Fail(ref worldForTest);
            }
            else { action.ApplyEffects(ref worldForTest); }

            StoryNode testNode = new StoryNode();
            testNode.SetWorldState((WorldDynamic)worldForTest.Clone());

            foreach (var constraint in constraints)
            {
                if (!constraint.IsSatisfied(worldForTest, currentState, currentGraph, ref action, currentNode, testNode, ref succsessControl))
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
        /// <param name="agent">The agent is currently performing the action.</param>
        /// <param name="currentGraph">Generated story graph.</param>
        /// <param name="currentState">The current state of the story world.</param>
        /// <param name="currentNode">The graph node currently being considered.</param>
        /// <param name="root">A marker that is considering the root node.</param>
        /// <param name="globalNodeNumber">Last assigned node number.</param>
        /// <param name="queue">The queue of nodes to be considered.</param>
        /// <param name="note">Text to display on the main screen.</param>
        /// <param name="findEvidenceShance">The agent's chance to find a evidence when performing the action to find it.</param>
        public void ActionRequest(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                  ref StoryGraph currentGraph, 
                                  ref WorldDynamic currentState,
                                  StoryNode currentNode,
                                  bool root,
                                  ref int globalNodeNumber,
                                  ref Queue<StoryNode> queue,
                                  ref TextBox note,
                                  int findEvidenceShance)
        {
            CSP_Module cspModule = new CSP_Module();
            PDDL_Module pddlModule = new PDDL_Module(currentState.GetStaticWorldPart().GetSetting(), currentState.GetStaticWorldPart().GetConnectionStatus(),
                                                       currentState.GetAgents().Count, currentState.GetStaticWorldPart().GetCanFindEvidence());

            if (agent.Key.GetRole().Equals(AgentRole.PLAYER))
            {
                agent.Value.RefreshBeliefsAboutTheWorld(agent, currentState, ref note);
                pddlModule.GeneratePDDLProblem(agent, currentState, ref note);
                agent.Value.ReceiveAvailableActions(agent, currentState, ref note);

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
                                                ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
                                break;
                            case "Fight":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                              ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
                                break;
                            case "InvestigateRoom":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root, 
                                              ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
                                break;
                            case "NeutralizeKiller":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                              ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
                                break;
                            case "NothingToDo":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                              ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
                                break;
                            case "Reassure":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                              ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
                                break;
                            case "Talk":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                              ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
                                break;
                            case "HelpElfs":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                    ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
                                break;
                            case "HelpWerewolves":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                    ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
                                break;
                            case "HelpMages":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                    ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
                                break;
                            case "HelpTemplars":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                    ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
                                break;
                            case "HelpPrinceBelen":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                    ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
                                break;
                            case "HelpLordHarrowmont":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                    ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
                                break;
                            case "CompleteQuest":
                                SingleAVandAC(ref receivedAction, currentState, agent, cspModule, currentGraph, currentNode, root,
                                   ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
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
                agent.Value.RefreshBeliefsAboutTheWorld(agent, currentState, ref note);
                pddlModule.GeneratePDDLProblem(agent, currentState, ref note);
                agent.Value.CalculatePlan(agent, currentState, ref note);
                agent.Value.ReceiveAvailableActions(agent, currentState, ref note);

                PlanAction receivedAction = agent.Value.ChooseAction();

                if (receivedAction != null)
                {
                    cspModule.AssignVariables(ref receivedAction, currentState, agent, ref note);

                    ActionControl(receivedAction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue, ref note, findEvidenceShance);

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

        /// <summary>
        /// A method that assigns variables (parameters) and checks the constraints of one specific action.
        /// </summary>
        /// <param name="receivedAction">Action under consideration.</param>
        /// <param name="currentState">The current state of the story world.</param>
        /// <param name="agent">The agent is currently performing the action.</param>
        /// <param name="cspModule">An instance of a class that implements the assignment of parameters to actions according to the CSP methodology.</param>
        /// <param name="currentGraph">Generated story graph.</param>
        /// <param name="currentNode">The graph node currently being considered.</param>
        /// <param name="root">A marker that is considering the root node.</param>
        /// <param name="globalNodeNumber">Last assigned node number.</param>
        /// <param name="queue">The queue of nodes to be considered.</param>
        /// <param name="note">Text to display on the main screen.</param>
        /// <param name="findEvidenceShance">The agent's chance to find a evidence when performing the action to find it.</param>
        public void SingleAVandAC(ref PlanAction receivedAction, 
                                  WorldDynamic currentState, 
                                  KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                  CSP_Module cspModule, 
                                  StoryGraph currentGraph,
                                  StoryNode currentNode,
                                  bool root,
                                  ref int globalNodeNumber,
                                  ref Queue<StoryNode> queue,
                                  ref TextBox note,
                                  int findEvidenceShance)
        {
            cspModule.AssignVariables(ref receivedAction, currentState, agent, ref note);

            ActionControl(receivedAction, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue, ref note, findEvidenceShance);

            // Cleaning
            currentNode = null;
            GC.Collect();
        }

        /// <summary>
        /// A method that assigns variables (parameters) and checks the constraints of several actions of the same type.
        /// </summary>
        /// <param name="receivedAction">Action under consideration.</param>
        /// <param name="currentState">The current state of the story world.</param>
        /// <param name="agent">The agent is currently performing the action.</param>
        /// <param name="cspModule">An instance of a class that implements the assignment of parameters to actions according to the CSP methodology.</param>
        /// <param name="currentGraph">Generated story graph.</param>
        /// <param name="currentNode">The graph node currently being considered.</param>
        /// <param name="root">A marker that is considering the root node.</param>
        /// <param name="globalNodeNumber">Last assigned node number.</param>
        /// <param name="queue">The queue of nodes to be considered.</param>
        /// <param name="note">Text to display on the main screen.</param>
        /// <param name="findEvidenceShance">The agent's chance to find a evidence when performing the action to find it.</param>
        public void MultiAVandAC(ref PlanAction receivedAction, 
                                 WorldDynamic currentState, 
                                 KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                                 CSP_Module cspModule, 
                                 StoryGraph currentGraph,
                                 StoryNode currentNode,
                                 bool root,
                                 ref int globalNodeNumber,
                                 ref Queue<StoryNode> queue,
                                 ref TextBox note,
                                 int findEvidenceShance)
        {
            List<PlanAction> actionsList = cspModule.MassiveAssignVariables(ref receivedAction, currentState, agent, ref note);

            AgentStateStatic sCurrentAgent = (AgentStateStatic)agent.Key.Clone();
            AgentStateDynamic dCurrentAgent = (AgentStateDynamic)agent.Value.Clone();
            KeyValuePair<AgentStateStatic, AgentStateDynamic> currentAgent = 
                                                                  new KeyValuePair<AgentStateStatic, AgentStateDynamic>(sCurrentAgent, dCurrentAgent);

            WorldDynamic statePrefab = (WorldDynamic)currentState.Clone();


            foreach (var a in actionsList)
            {
                ActionControl(a, currentGraph, currentAgent, statePrefab, currentNode, root, ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
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
        /// <param name="action">Action under consideration.</param>
        /// <param name="currentGraph">Generated story graph.</param>
        /// <param name="agent">The agent is currently performing the action.</param>
        /// <param name="currentState">The current state of the story world.</param>
        /// <param name="currentNode">The graph node currently being considered.</param>
        /// <param name="root">A marker that is considering the root node.</param>
        /// <param name="globalNodeNumber">Last assigned node number.</param>
        /// <param name="queue">The queue of nodes to be considered.</param>
        /// <param name="note">Text to display on the main screen.</param>
        /// <param name="findEvidenceShance">The agent's chance to find a evidence when performing the action to find it.</param>
        public void ActionControl(PlanAction action, 
                                  StoryGraph currentGraph, 
                                  KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                  WorldDynamic currentState,
                                  StoryNode currentNode,
                                  bool root,
                                  ref int globalNodeNumber,
                                  ref Queue<StoryNode> queue,
                                  ref TextBox note,
                                  int findEvidenceShance)
        {
            note.Text = "ACTION CONTROL";

            bool succsessControl = ProbabilityCalculating(action, currentState, findEvidenceShance);

            action.success = succsessControl;
            action.fail = !succsessControl;

            bool constraintsControl = ConstraintsControl(currentGraph, currentState, action, ref succsessControl, currentNode, ref note);
            bool deadEndsControl = DeadEndsControl(action, currentState, agent, succsessControl, ref note);
            bool duplicateControl = DuplicateControl(currentState, action, currentGraph, agent, currentNode, globalNodeNumber, succsessControl, ref note);
            bool cyclesControl = CyclesControl(currentState, action, currentGraph, agent, currentNode, duplicateControl, globalNodeNumber, succsessControl, ref note);
            
            int nodecounter = currentGraph.GetNodes().Count;
            int edgecounter = currentNode.GetEdges().Count;

            if (!constraintsControl && agent.Key.GetRole().Equals(AgentRole.PLAYER)) { }
            else if (constraintsControl && deadEndsControl && cyclesControl && duplicateControl)
            {
                // If all checks are passed, then we apply the action.
                ApplyAction(action, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, succsessControl, false, ref note);
            }
            else if (!constraintsControl && deadEndsControl && cyclesControl && duplicateControl)
            {
                // If the action violates the constraints, then convergence will not apply it, but will apply its counter-reaction.
                ActionCounteract(action, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
            }
            else if (!duplicateControl && cyclesControl)
            {
                bool skip = false;

                // connection current node --> finded node
                currentGraph.DuplicateNodeConnecting(currentState, action, agent, currentNode, globalNodeNumber, ref queue, succsessControl, ref skip, ref note);

                if (skip)
                {
                    ActionCounteract(action, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
                }
            }
            else
            {
                ActionCounteract(action, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue, ref note, findEvidenceShance);
            }
        }

        /// <summary>
        /// The probability of success of the action is calculated, and if successful, it is applied.
        /// </summary>
        /// <param name="action">Action under consideration.</param>
        /// <param name="currentGraph">Generated story graph.</param>
        /// <param name="agent">The agent is currently performing the action.</param>
        /// <param name="currentState">The current state of the story world.</param>
        /// <param name="currentNode">The graph node currently being considered.</param>
        /// <param name="root">A marker that is considering the root node.</param>
        /// <param name="globalNodeNumber">Last assigned node number.</param>
        /// <param name="succsessControl">A variable expressing the success or failure of an action.</param>
        /// <param name="counteract">A marker for whether a normal action or a counter-reaction is being considered.</param>
        /// <param name="note">Text to display on the main screen.</param>
        public void ApplyAction(PlanAction action, 
                                StoryGraph currentGraph, 
                                KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                WorldDynamic currentState,
                                StoryNode currentNode,
                                bool root,
                                ref int globalNodeNumber,
                                bool succsessControl,
                                bool counteract,
                                ref TextBox note)
        {
            note.Text = "APPLY ACTION";

            // We apply a successful/unsuccessful option to perform an action.
            if (root) { currentGraph.CreateRootNode(action, agent, currentState, currentNode, ref globalNodeNumber, succsessControl, ref note); }
            else { currentGraph.CreateNewNode(action, agent, currentState, currentNode, ref globalNodeNumber, succsessControl, counteract, ref note); }
        }

        /// <summary>
        /// A method that checks whether the effects of the action under test will result in a dead end in the graph.
        /// </summary>
        /// <param name="action">Action under consideration.</param>
        /// <param name="currentState">The current state of the story world.</param>
        /// <param name="player">The agent representing the player.</param>
        /// <param name="succsessControl">A variable expressing the success or failure of an action.</param>
        /// <param name="note">Text to display on the main screen.</param>
        /// <returns>True if the check was successful (not dead end), false otherwise.</returns>
        public bool DeadEndsControl(PlanAction action, 
                                    WorldDynamic currentState, 
                                    KeyValuePair<AgentStateStatic, AgentStateDynamic> player, 
                                    bool succsessControl,
                                    ref TextBox note)
        {
            note.Text = "DEADENDS CONTROL";

            if (player.Key.GetRole() == AgentRole.PLAYER)
            {
                WorldDynamic worldForTest = (WorldDynamic)currentState.Clone();
                if (!succsessControl) { action.Fail(ref worldForTest); }
                else { action.ApplyEffects(ref worldForTest); }

                worldForTest.GetAgentByRole(AgentRole.PLAYER).Value.CalculatePlan(worldForTest.GetAgentByRole(AgentRole.PLAYER), worldForTest, ref note);

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

        /// <summary>
        /// A method that checks whether the effects of the checked action will lead to the creation of a node that already exists in the graph.
        /// </summary>
        /// <param name="currentState">The current state of the story world.</param>
        /// <param name="action">Action under consideration.</param>
        /// <param name="currentGraph">Generated story graph.</param>
        /// <param name="agent">The agent is currently performing the action.</param>
        /// <param name="currentNode">The graph node currently being considered.</param>
        /// <param name="globalNodeNumber">Last assigned node number.</param>
        /// <param name="succsessControl">A variable expressing the success or failure of an action.</param>
        /// <param name="note">Text to display on the main screen.</param>
        /// <returns>True if the check was successful (not duplicate), false otherwise.</returns>
        public bool DuplicateControl(WorldDynamic currentState,
                                     PlanAction action,
                                     StoryGraph currentGraph,
                                     KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                                     StoryNode currentNode,
                                     int globalNodeNumber,
                                     bool succsessControl,
                                     ref TextBox note)
        {
            note.Text = "DUPLICATE CONTROL";

            StoryNode testNode = currentGraph.CreateTestNode(currentState, action, agent, currentNode, globalNodeNumber, succsessControl);
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

        /// <summary>
        /// A method that checks whether the effects of the action under test will cause cycles in the graph.
        /// </summary>
        /// <param name="currentState">The current state of the story world.</param>
        /// <param name="action">Action under consideration.</param>
        /// <param name="currentGraph">Generated story graph.</param>
        /// <param name="agent">The agent is currently performing the action.</param>
        /// <param name="currentNode">The graph node currently being considered.</param>
        /// <param name="duplicated">A marker that the previous check detected the creation of a duplicate node.</param>
        /// <param name="globalNodeNumber">Last assigned node number.</param>
        /// <param name="succsessControl">A variable expressing the success or failure of an action.</param>
        /// <param name="note">Text to display on the main screen.</param>
        /// <returns>True if the check was successful (not cycles), false otherwise.</returns>
        public bool CyclesControl(WorldDynamic currentState, 
                                  PlanAction action, 
                                  StoryGraph currentGraph, 
                                  KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                  StoryNode currentNode,
                                  bool duplicated,
                                  int globalNodeNumber,
                                  bool succsessControl,
                                  ref TextBox note)
        {
            note.Text = "CYCLES CONTROL";

            bool result = false;

            // We create a test node similar to the one we are going to add to the graph as a result of the current action.
            StoryNode testNode = currentGraph.CreateTestNode(currentState, action, agent, currentNode, globalNodeNumber, succsessControl);

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

            result = TarjanAlgStep(currentGraph.GetRoot(), ref colors, duplicated, duplicatedNode);

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

        /// <summary>
        /// A method that implements an algorithm that checks for the presence of a cycle in a graph.
        /// </summary>
        /// <param name="checkedNode">The node under consideration.</param>
        /// <param name="colors">An array corresponding in size to the number of nodes.</param>
        /// <param name="duplicated">A marker that the previous check detected the creation of a duplicate node.</param>
        /// <param name="duplicatedNode">The node that should be duplicated as a result of applying the effects of the action (an edge will be added instead).</param>
        /// <returns>True if the check was successful (not cycles), false otherwise.</returns>
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

                if (colors[nextNode.GetNumberInSequence()] == "grey" && (nextNode.GetNumberInSequence() < checkedNode.GetNumberInSequence()))
                {
                    if (checkedNode.isChild(nextNode) && duplicated)
                    {
                        continue;
                    }
                    else if (!duplicated && checkedNode.isChild(nextNode) && !duplicatedNode.Equals(nextNode))
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

        /// <summary>
        /// A method that implements the system's response to an action that should not be applied.
        /// </summary>
        /// <param name="action">Action under consideration.</param>
        /// <param name="currentGraph">Generated story graph.</param>
        /// <param name="agent">The agent is currently performing the action.</param>
        /// <param name="currentState">The current state of the story world.</param>
        /// <param name="currentNode">The graph node currently being considered.</param>
        /// <param name="root">A marker that is considering the root node.</param>
        /// <param name="globalNodeNumber">Last assigned node number.</param>
        /// <param name="queue">The queue of nodes to be considered.</param>
        /// <param name="note">Text to display on the main screen.</param>
        /// <param name="findEvidenceShance">The agent's chance to find a evidence when performing the action to find it.</param>
        public void ActionCounteract(PlanAction action, 
                                     StoryGraph currentGraph, 
                                     KeyValuePair<AgentStateStatic,AgentStateDynamic> agent, 
                                     WorldDynamic currentState, 
                                     StoryNode currentNode,
                                     bool root,
                                     ref int globalNodeNumber,
                                     ref Queue<StoryNode> queue,
                                     ref TextBox note,
                                     int findEvidenceShance)
        {
            note.Text = "COUNTERACTION GENERATING";

            CSP_Module cspModule = new CSP_Module();

             bool stageOne_NewNode = true;
             bool stageTwo_ConnectedNode = false;
             bool counterreactionFound = false;

            bool skip = false;

            string currentActionStr = action.GetType().ToString().Remove(0, 20);
            KeyValuePair<string, PlanAction> currentAction = new KeyValuePair<string, PlanAction>(currentActionStr, action);

             while (!counterreactionFound)
             {
                 PlanAction counterreactionTalk = new CounterTalk(currentState);
                 bool assignVariables = cspModule.AssignVariables(ref counterreactionTalk, currentState, agent, ref note);
                 counterreactionTalk.Arguments.Add(currentAction);

                 if (assignVariables && counterreactionTalk.CheckPreconditions(currentState))
                 {
                     bool constractionAndDeadEndAndCicle = false;
                     bool duplicate = false;

                     CounterreactionControl(counterreactionTalk, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                            ref constractionAndDeadEndAndCicle, ref duplicate, ref note, findEvidenceShance);

                     if (stageOne_NewNode)
                     {
                         if (constractionAndDeadEndAndCicle && duplicate)
                         {
                             ApplyAction(counterreactionTalk, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true, ref note);
                             counterreactionFound = true;
                         }
                     }
                     else if (stageTwo_ConnectedNode)
                     {
                         if (constractionAndDeadEndAndCicle && !duplicate)
                         {
                             currentGraph.DuplicateNodeConnecting(currentState, counterreactionTalk, agent, currentNode, globalNodeNumber, ref queue, true, ref skip, ref note);
                             counterreactionFound = true;
                         }
                     }
                 }

                 if (!counterreactionFound)
                 {
                     PlanAction counterreactionEntrap = new CounterEntrap(currentState);
                     assignVariables = cspModule.AssignVariables(ref counterreactionEntrap, currentState, agent, ref note);
                     counterreactionEntrap.Arguments.Add(currentAction);

                     if (assignVariables && counterreactionEntrap.CheckPreconditions(currentState))
                     {
                         bool constractionAndDeadEndAndCicle = false;
                         bool duplicate = false;

                         CounterreactionControl(counterreactionEntrap, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                                ref constractionAndDeadEndAndCicle, ref duplicate, ref note, findEvidenceShance);

                         if (stageOne_NewNode)
                         {
                             if (constractionAndDeadEndAndCicle && duplicate)
                             {
                                 ApplyAction(counterreactionEntrap, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true, ref note);
                                 counterreactionFound = true;
                             }
                         }
                         else if (stageTwo_ConnectedNode)
                         {
                             if (constractionAndDeadEndAndCicle && !duplicate)
                             {
                                 currentGraph.DuplicateNodeConnecting(currentState, counterreactionEntrap, agent, currentNode, globalNodeNumber, ref queue, true, ref skip, ref note);
                                 counterreactionFound = true;
                             }
                         }
                     }
                }

                if (!counterreactionFound)
                {
                    PlanAction counterreactionKill = new CounterKill(currentState);
                    assignVariables = cspModule.AssignVariables(ref counterreactionKill, currentState, agent, ref note);
                    counterreactionKill.Arguments.Add(currentAction);

                    if (assignVariables && counterreactionKill.CheckPreconditions(currentState))
                    {
                        bool constractionAndDeadEndAndCicle = false;
                        bool duplicate = false;

                        CounterreactionControl(counterreactionKill, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                               ref constractionAndDeadEndAndCicle, ref duplicate, ref note, findEvidenceShance);

                        if (stageOne_NewNode)
                        {
                            if (constractionAndDeadEndAndCicle && duplicate)
                            {
                                ApplyAction(counterreactionKill, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true, ref note);
                                counterreactionFound = true;
                            }
                        }
                        else if (stageTwo_ConnectedNode)
                        {
                            if (constractionAndDeadEndAndCicle && !duplicate)
                            {
                                currentGraph.DuplicateNodeConnecting(currentState, counterreactionKill, agent, currentNode, globalNodeNumber, ref queue, true, ref skip, ref note);
                                counterreactionFound = true;
                            }
                        }
                    }
                }

                if (!counterreactionFound)
                {
                     PlanAction counterreactionIR = new InvestigateRoom(currentState);
                     assignVariables = cspModule.AssignVariables(ref counterreactionIR, currentState, agent, ref note);
                     counterreactionIR.Arguments.Add(currentAction);

                     if (assignVariables && counterreactionIR.CheckPreconditions(currentState))
                     {
                         bool constractionAndDeadEndAndCicle = false;
                         bool duplicate = false;

                         CounterreactionControl(counterreactionIR, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                                ref constractionAndDeadEndAndCicle, ref duplicate, ref note, findEvidenceShance);

                         if (stageOne_NewNode)
                         {
                             if (constractionAndDeadEndAndCicle && duplicate)
                             {
                                 ApplyAction(counterreactionIR, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true, ref note);
                                 counterreactionFound = true;
                             }
                         }
                         else if (stageTwo_ConnectedNode)
                         {
                             if (constractionAndDeadEndAndCicle && !duplicate)
                             {
                                 currentGraph.DuplicateNodeConnecting(currentState, counterreactionIR, agent, currentNode, globalNodeNumber, ref queue, true, ref skip, ref note);
                                 counterreactionFound = true;
                             }
                         }
                     }
                 }

                 if (!counterreactionFound)
                 {
                     PlanAction counterreactionNK = new CounterNeutralizeKiller(currentState);
                     assignVariables = cspModule.AssignVariables(ref counterreactionNK, currentState, agent, ref note);
                     counterreactionNK.Arguments.Add(currentAction);

                     if (assignVariables && counterreactionNK.CheckPreconditions(currentState))
                     {
                         bool constractionAndDeadEndAndCicle = false;
                         bool duplicate = false;

                         CounterreactionControl(counterreactionNK, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                                ref constractionAndDeadEndAndCicle, ref duplicate, ref note, findEvidenceShance);

                         if (stageOne_NewNode)
                         {
                             if (constractionAndDeadEndAndCicle && duplicate)
                             {
                                 ApplyAction(counterreactionNK, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true, ref note);
                                 counterreactionFound = true;
                             }
                         }
                         else if (stageTwo_ConnectedNode)
                         {
                             if (constractionAndDeadEndAndCicle && !duplicate)
                             {
                                 currentGraph.DuplicateNodeConnecting(currentState, counterreactionNK, agent, currentNode, globalNodeNumber, ref queue, true, ref skip, ref note);
                                 counterreactionFound = true;
                             }
                         }
                     }
                 }

                 if (!counterreactionFound)
                 {
                     PlanAction counterreactionTalkAboutSuspicious = new CounterTellAboutASuspicious(currentState);
                     assignVariables = cspModule.AssignVariables(ref counterreactionTalkAboutSuspicious, currentState, agent, ref note);
                     counterreactionTalkAboutSuspicious.Arguments.Add(currentAction);

                     if (assignVariables && counterreactionTalkAboutSuspicious.CheckPreconditions(currentState))
                     {
                         bool constractionAndDeadEndAndCicle = false;
                         bool duplicate = false;

                         CounterreactionControl(counterreactionTalkAboutSuspicious, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                                ref constractionAndDeadEndAndCicle, ref duplicate, ref note, findEvidenceShance);

                         if (stageOne_NewNode)
                         {
                             if (constractionAndDeadEndAndCicle && duplicate)
                             {
                                 ApplyAction(counterreactionTalkAboutSuspicious, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true, ref note);
                                 counterreactionFound = true;
                             }
                         }
                         else if (stageTwo_ConnectedNode)
                         {
                             if (constractionAndDeadEndAndCicle && !duplicate)
                             {
                                 currentGraph.DuplicateNodeConnecting(currentState, counterreactionTalkAboutSuspicious, agent, currentNode, globalNodeNumber, ref queue, true, ref skip, ref note);
                                 counterreactionFound = true;
                             }
                         }
                     }
                 }

                 if (!counterreactionFound)
                 {
                     PlanAction counterreactionFight = new CounterFight(currentState);
                     assignVariables = cspModule.AssignVariables(ref counterreactionFight, currentState, agent, ref note);
                     counterreactionFight.Arguments.Add(currentAction);

                     if (assignVariables && counterreactionFight.CheckPreconditions(currentState))
                     {
                         bool constractionAndDeadEndAndCicle = false;
                         bool duplicate = false;

                         CounterreactionControl(counterreactionFight, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                                ref constractionAndDeadEndAndCicle, ref duplicate, ref note, findEvidenceShance);

                         if (stageOne_NewNode)
                         {
                             if (constractionAndDeadEndAndCicle && duplicate)
                             {
                                 ApplyAction(counterreactionFight, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true, ref note);
                                 counterreactionFound = true;
                             }
                         }
                         else if (stageTwo_ConnectedNode)
                         {
                             if (constractionAndDeadEndAndCicle && !duplicate)
                             {
                                 currentGraph.DuplicateNodeConnecting(currentState, counterreactionFight, agent, currentNode, globalNodeNumber, ref queue, true, ref skip, ref note);
                                 counterreactionFound = true;
                             }
                         }
                     }
                 }

                 if (!counterreactionFound)
                 {
                     PlanAction counterreactionReassure = new CounterReassure(currentState);
                     assignVariables = cspModule.AssignVariables(ref counterreactionReassure, currentState, agent, ref note);
                     counterreactionReassure.Arguments.Add(currentAction);

                     if (assignVariables && counterreactionReassure.CheckPreconditions(currentState))
                     {
                         bool constractionAndDeadEndAndCicle = false;
                         bool duplicate = false;

                         CounterreactionControl(counterreactionReassure, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                                ref constractionAndDeadEndAndCicle, ref duplicate, ref note, findEvidenceShance);

                         if (stageOne_NewNode)
                         {
                             if (constractionAndDeadEndAndCicle && duplicate)
                             {
                                 ApplyAction(counterreactionReassure, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true, ref note);
                                 counterreactionFound = true;
                             }
                         }
                         else if (stageTwo_ConnectedNode)
                         {
                             if (constractionAndDeadEndAndCicle && !duplicate)
                             {
                                 currentGraph.DuplicateNodeConnecting(currentState, counterreactionReassure, agent, currentNode, globalNodeNumber, ref queue, true, ref skip, ref note);
                                 counterreactionFound = true;
                             }
                         }
                     }
                 }

                 if (!counterreactionFound)
                 {
                     PlanAction counterreactionRun = new CounterRun(currentState);
                     assignVariables = cspModule.AssignVariables(ref counterreactionRun, currentState, agent, ref note);
                     counterreactionRun.Arguments.Add(currentAction);

                     if (assignVariables && counterreactionRun.CheckPreconditions(currentState))
                     {
                         bool constractionAndDeadEndAndCicle = false;
                         bool duplicate = false;

                         CounterreactionControl(counterreactionRun, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                                ref constractionAndDeadEndAndCicle, ref duplicate, ref note, findEvidenceShance);

                         if (stageOne_NewNode)
                         {
                             if (constractionAndDeadEndAndCicle && duplicate)
                             {
                                 ApplyAction(counterreactionRun, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true, ref note);
                                 counterreactionFound = true;
                             }
                         }
                         else if (stageTwo_ConnectedNode)
                         {
                             if (constractionAndDeadEndAndCicle && !duplicate)
                             {
                                 currentGraph.DuplicateNodeConnecting(currentState, counterreactionRun, agent, currentNode, globalNodeNumber, ref queue, true, ref skip, ref note);
                                 counterreactionFound = true;
                             }
                         }
                     }
                 }

                 if (!counterreactionFound)
                 {
                     PlanAction counterreactionMove = new CounterMove(currentState);
                     assignVariables = cspModule.AssignVariables(ref counterreactionMove, currentState, agent, ref note);
                     counterreactionMove.Arguments.Add(currentAction);

                     if (assignVariables && counterreactionMove.CheckPreconditions(currentState))
                     {
                         bool constractionAndDeadEndAndCicle = false;
                         bool duplicate = false;

                         CounterreactionControl(counterreactionMove, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                                ref constractionAndDeadEndAndCicle, ref duplicate, ref note, findEvidenceShance);

                         if (stageOne_NewNode)
                         {
                             if (constractionAndDeadEndAndCicle && duplicate)
                             {
                                 ApplyAction(counterreactionMove, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true, ref note);
                                 counterreactionFound = true;
                             }
                         }
                         else if (stageTwo_ConnectedNode)
                         {
                             if (constractionAndDeadEndAndCicle && !duplicate)
                             {
                                 currentGraph.DuplicateNodeConnecting(currentState, counterreactionMove, agent, currentNode, globalNodeNumber, ref queue, true, ref skip, ref note);
                                 counterreactionFound = true;
                             }
                         }
                     }
                 }

                 if (stageTwo_ConnectedNode /*&& !counterreactionFound*/)
                {
                    PlanAction counterreactionSkip = new NothingToDo(currentState);
                    counterreactionSkip.Arguments.Add(agent);

                    bool constractionAndDeadEndAndCicle = false;
                    bool duplicate = false;

                    CounterreactionControl(counterreactionSkip, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, ref queue,
                                           ref constractionAndDeadEndAndCicle, ref duplicate, ref note, findEvidenceShance);

                    if (constractionAndDeadEndAndCicle && duplicate)
                    {
                        ApplyAction(counterreactionSkip, currentGraph, agent, currentState, currentNode, root, ref globalNodeNumber, true, true, ref note);
                        counterreactionFound = true;
                    }
                    else if (constractionAndDeadEndAndCicle && !duplicate)
                    {
                        currentGraph.DuplicateNodeConnecting(currentState, counterreactionSkip, agent, currentNode, globalNodeNumber, ref queue, true, ref skip, ref note);
                        counterreactionFound = true;
                    }
                }

                if (stageOne_NewNode)
                {
                    stageOne_NewNode = false;
                    stageTwo_ConnectedNode = true;
                }
                else if (stageTwo_ConnectedNode)
                {
                    stageOne_NewNode = true;
                    stageTwo_ConnectedNode = false;
                }
            }
            
        }

        /// <summary>
        /// Calculation of the probability of success of the action.
        /// </summary>
        /// <param name="action">Action under consideration.</param>
        /// <param name="currentState">The current state of the story world.</param>
        /// <param name="findEvidenceShance">The agent's chance to find a evidence when performing the action to find it.</param>
        /// <returns>True if the calculation completed without errors, false otherwise.</returns>
        public bool ProbabilityCalculating (PlanAction action, WorldDynamic currentState, int findEvidenceShance)
        {
            Random random = new Random();
            int probability = random.Next(0, 100);
            int threshold = 0;

            if (action is Entrap) { threshold = 100; }
            else if (action is CounterEntrap) { threshold = 100; }
            else if (action is Fight)
            {
                if (currentState.GetStaticWorldPart().GetRandomBattlesResultsStatus()) { threshold = 75; }
                else { threshold = 100; }
            }
            else if (action is CounterFight) { threshold = 100; }
            else if (action is InvestigateRoom || action is CounterInvestigateRoom) { threshold = findEvidenceShance; }
            else if (action is Kill || action is CounterKill)
            {
                if (currentState.GetStaticWorldPart().GetRandomBattlesResultsStatus()) { threshold = 35; }
                else { threshold = 100; }
            }
            else if (action is Move || action is CounterMove) { threshold = 100; }
            else if (action is NeutralizeKiller || action is CounterNeutralizeKiller) { threshold = 100; }
            else if (action is NothingToDo) { threshold = 100; }
            else if (action is Reassure) { threshold = 100; }
            else if (action is CounterReassure) { threshold = 100; }
            else if (action is Run || action is CounterRun) { threshold = 100; }
            else if (action is TellAboutASuspicious) { threshold = 100; }
            else if (action is CounterTellAboutASuspicious) { threshold = 100; }
            else if (action is Talk || action is CounterTalk) { threshold = 100; }
            else if (action is ToBeAWitness) { threshold = 100; }
            else if (action is HelpElfs) { threshold = 100; }
            else if (action is HelpWerewolves) { threshold = 100; }
            else if (action is HelpMages) { threshold = 100; }
            else if (action is HelpTemplars) { threshold = 100; }
            else if (action is HelpPrinceBelen) { threshold = 100; }
            else if (action is HelpLordHarrowmont) { threshold = 100; }
            else if (action is CompleteQuest) { threshold = 100; }

            if (probability <= threshold) { return true; }
            else { return false; }
        }

        /// <summary>
        /// To skip a turn (action), an action "NothingToDo" is created and applied.
        /// </summary>
        /// <param name="currentState">The current state of the story world.</param>
        public void SkipTurn (WorldDynamic currentState)
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
                                            ref bool controlTwo,
                                            ref TextBox note,
                                            int findEvidenceShance)
        {
            note.Text = "COUNTERREACTION CONTROL";

            bool succsessControl = ProbabilityCalculating(action, currentState, findEvidenceShance);

            action.success = succsessControl;
            action.fail = !succsessControl;

            bool constraintsControl = ConstraintsControl(currentGraph, currentState, action, ref succsessControl, currentNode, ref note);
            bool deadEndsControl = DeadEndsControl(action, currentState, agent, succsessControl, ref note);
            bool duplicateControl = DuplicateControl(currentState, action, currentGraph, agent, currentNode, globalNodeNumber, succsessControl, ref note);
            bool cyclesControl = CyclesControl(currentState, action, currentGraph, agent, currentNode, duplicateControl, globalNodeNumber, succsessControl, ref note);

            controlOne = constraintsControl & deadEndsControl & cyclesControl;
            controlTwo = duplicateControl;
        }
    }
}