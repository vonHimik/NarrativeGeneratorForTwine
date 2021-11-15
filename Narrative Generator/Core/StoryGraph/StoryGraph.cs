using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that is a representation of a history graph, where nodes are states of the world and edges are actions.
    /// </summary>
    class StoryGraph
    {
        // Link to the root node.
        private StoryNode root;

        // List of nodes.
        private HashSet<StoryNode> nodes;

        /// <summary>
        /// Constructor method for story graph, no parameters.
        /// </summary>
        public StoryGraph()
        {
            root = new StoryNode();
            nodes = new HashSet<StoryNode>();
        }

        /// <summary>
        /// Adds a node to the list of nodes in the story graph.
        /// </summary>
        /// <param name="newNode"></param>
        public void AddNode(StoryNode newNode)
        {
            nodes.Add(newNode);
        }

        public bool FindNode(StoryNode node)
        {
            return nodes.Contains(node);
        }

        /// <summary>
        /// Returns a list of nodes in the story graph.
        /// </summary>
        public HashSet<StoryNode> GetNodes()
        {
            return nodes;
        }

        /// <summary>
        /// Returns the root node of the story graph.
        /// </summary>
        public StoryNode GetRoot()
        {
            return root;
        }

        public void SetRoot(StoryNode newRoot)
        {
            root = newRoot;
            AddNode(newRoot);
        }

        /// <summary>
        /// Returns the last node from the list of nodes in the story graph.
        /// </summary>
        public StoryNode GetLastNode()
        {
            return nodes.Last();
        }

        public StoryNode GetNode(int index)
        {
            return nodes.ElementAt(index);
        }

        public StoryNode GetNode(StoryNode node)
        {
            foreach (var n in nodes)
            {
                if (node.Equals(n)) { return n; }
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstNode">Current node</param>
        /// <param name="secondNode">New node</param>
        public void ConnectionTwoNodes(PlanAction action, StoryNode firstNode, StoryNode secondNode, bool duplicate)
        {
            bool full = false;

            if (duplicate)
            {
                if (firstNode.Equals(secondNode))
                {
                    foreach (var edge in firstNode.GetEdges())
                    {
                        foreach (var edge2 in secondNode.GetEdges())
                        {
                            if (edge.GetLowerNode() == null && edge2.GetLowerNode() == null)
                            {
                                edge.SetLowerNode(ref secondNode);
                                edge2.SetLowerNode(ref firstNode);

                                return;
                            }
                        }
                    }
                }
            }

            if (firstNode.GetEdges().Count != 0)
            {
                full = true;

                foreach (var edge in firstNode.GetEdges())
                {
                    if (edge.GetLowerNode() == null)
                    {
                        edge.SetLowerNode(ref secondNode);

                        secondNode.AddEdge(edge);

                        full = false;

                        if (duplicate)
                        {
                            firstNode.AddLinkToNode(ref secondNode);
                            secondNode.AddLinkToNode(ref firstNode);
                            return;
                        }

                        break;
                    }
                }
            }

            if (full)
            {
                Edge oldEdge = new Edge();

                //oldEdge.SetAction(firstNode.GetEdges().Last().GetAction());
                oldEdge.SetAction(action);

                oldEdge.SetUpperNode(ref firstNode);
                oldEdge.SetLowerNode(ref secondNode);

                firstNode.AddEdge(oldEdge);
                secondNode.AddEdge(oldEdge);

                firstNode.AddLinkToNode(ref secondNode);
                secondNode.AddLinkToNode(ref firstNode);

                return;
            }

            // Create an empty new edge.
            Edge newEdge = new Edge();

            // We adjust the edge - assign its action and indicate the nodes that it connects.
            newEdge.SetAction(action);
            newEdge.SetUpperNode(ref secondNode);
            if (duplicate) { newEdge.SetLowerNode(ref firstNode); }

            firstNode.AddLinkToNode(ref secondNode);
            secondNode.AddLinkToNode(ref firstNode);

            secondNode.AddEdge(newEdge);
            if (duplicate) firstNode.AddEdge(newEdge);
        }

        public StoryNode CreateTestNode(WorldDynamic currentState,
                                        PlanAction action,
                                        KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                                        StoryNode currentNode,
                                        bool connection,
                                        int globalNodeNumber,
                                        bool fail)
        {
            WorldDynamic worldForTest = (WorldDynamic)currentState.Clone();
            if (fail) { action.Fail(ref worldForTest); }
            else { action.ApplyEffects(ref worldForTest); }

            StoryNode testNode = new StoryNode();
            testNode.SetWorldState(worldForTest);

            // Create a clone of the agent.
            KeyValuePair<AgentStateStatic, AgentStateDynamic> newAgent =
                new KeyValuePair<AgentStateStatic, AgentStateDynamic>((AgentStateStatic)agent.Key.Clone(), (AgentStateDynamic)agent.Value.Clone());

            // We take the last node from the list of all nodes and assign whether the player is active and which of the agents was active on this turn.
            if (newAgent.Key.GetRole() == AgentRole.PLAYER) { testNode.SetActivePlayer(true); }
            else { testNode.SetActivePlayer(false); }

            testNode.SetActiveAgent(newAgent);

            if (connection) { ConnectionTwoNodes(action, currentNode, testNode, false); }

            testNode.SetNumberInSequence(globalNodeNumber + 1);

            return testNode;
        }

        /// <summary>
        /// Create a new node for the story graph and inserts it.
        /// </summary>
        public void CreateNewNode(PlanAction action,
                                  KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                                  WorldDynamic currentState,
                                  StoryNode currentNode,
                                  ref int globalNodeNumber,
                                  bool fail)
        {
            WorldDynamic newState = (WorldDynamic)currentState.Clone();
            if (fail) { action.Fail(ref newState); }
            else { action.ApplyEffects(ref newState); }

            // Create an empty new node.
            StoryNode newNode = new StoryNode();

            if (agent.Key.GetRole() == AgentRole.PLAYER) { newNode.SetActivePlayer(true); }
            else { newNode.SetActivePlayer(false); }

            newNode.SetActiveAgent(agent);

            // We assign the state of the world (transferred) to the new node.
            newNode.SetWorldState((WorldDynamic)newState.Clone());

            ConnectionTwoNodes(action, currentNode, newNode, false);

            globalNodeNumber++;
            newNode.SetNumberInSequence(globalNodeNumber);

            // Add a new node to the graph.
            AddNode(newNode);
        }

        public void CreateRootNode(PlanAction action,
                                   KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                                   WorldDynamic currentState,
                                   StoryNode currentNode,
                                   ref int globalNodeNumber, 
                                   bool fail)
        {
            WorldDynamic newState = (WorldDynamic)currentState.Clone();
            if (fail) { action.Fail(ref newState); }
            else { action.ApplyEffects(ref newState); }

            if (agent.Key.GetRole() == AgentRole.PLAYER) { currentNode.SetActivePlayer(true); }
            else { currentNode.SetActivePlayer(false); }

            // We assign the state of the world (transferred) to the new node.
            currentNode.SetWorldState((WorldDynamic)newState.Clone());

            Edge newEdge = new Edge();

            // We adjust the edge - assign its action and indicate the nodes that it connects.
            newEdge.SetAction(action);
            newEdge.SetUpperNode(ref currentNode);

            currentNode.AddEdge(newEdge);

            globalNodeNumber++;
            currentNode.SetNumberInSequence(globalNodeNumber);
        }

        public void DeleteTestNode(ref StoryNode testNode)
        {
            foreach (var edge in testNode.GetEdges().ToList())
            {
                edge.ClearEdge();
            }

            foreach (var link in testNode.GetLinks().ToList())
            {
                testNode.DeleteLink(link);
            }
        }

        public bool NodeExistenceControl(StoryNode checkedNode)
        {
            foreach (var node in GetNodes())
            {
                if (TwoNodesComparison(node, checkedNode)) { return true; }
            }

            return false;
        }

        public bool TwoNodesComparison(StoryNode nodeOne, StoryNode nodeTwo)
        {
            if (nodeOne.Equals(nodeTwo)) { return true; }
            return false;
        }

        public void DuplicateNodeConnecting(WorldDynamic currentState, 
                                            PlanAction action, 
                                            KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                            StoryNode currentNode,
                                            ref bool skip,
                                            int globalNodeNumber,
                                            ref Queue<StoryNode> queue)
        {
            StoryNode testNode = CreateTestNode(currentState, action, agent, currentNode, false, globalNodeNumber, false);

            if (!testNode.Equals(currentNode))
            {
                foreach (var checkedNode in nodes)
                {
                    if (TwoNodesComparison(testNode, checkedNode))
                    {
                        DeleteTestNode(ref testNode);
                        ConnectionTwoNodes(action, currentNode, checkedNode, true);
                        //queue.Enqueue(checkedNode);
                        break;
                    }
                }
            }
            else
            {
                skip = true;
            }
        }
    }
}
