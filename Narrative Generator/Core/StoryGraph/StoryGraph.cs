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
    public class StoryGraph
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
        public void AddNode (StoryNode newNode) { nodes.Add(newNode); }

        public bool FindNode (StoryNode node) { return nodes.Contains(node); }

        /// <summary>
        /// Returns a list of nodes in the story graph.
        /// </summary>
        public HashSet<StoryNode> GetNodes() { return nodes; }

        /// <summary>
        /// Returns the root node of the story graph.
        /// </summary>
        public StoryNode GetRoot() { return root; }

        public void SetRoot(StoryNode newRoot)
        {
            root = newRoot;
            AddNode(newRoot);
        }

        /// <summary>
        /// Returns the last node from the list of nodes in the story graph.
        /// </summary>
        public StoryNode GetLastNode() { return nodes.Last(); }

        public StoryNode GetNode (int index) { return nodes.ElementAt(index); }

        public StoryNode GetNode (StoryNode node)
        {
            foreach (var n in nodes) { if (node.Equals(n)) { return n; } }
            throw new KeyNotFoundException();
        }

        public void ConnectionTwoNodes (PlanAction action, StoryNode firstNode, StoryNode secondNode, bool duplicate)
        {

            Edge newEdge = new Edge();

            newEdge.SetAction(action);

            newEdge.SetUpperNode(ref firstNode);
            newEdge.SetLowerNode(ref secondNode);

            firstNode.AddEdge(newEdge);
            secondNode.AddEdge(newEdge);

            firstNode.AddLinkToNode(ref secondNode);
            secondNode.AddLinkToNode(ref firstNode);

            if (firstNode.GetEdges().Count != firstNode.GetLinks().Count)
            {
                bool test = true;
            }
        }

        public StoryNode CreateTestNode(WorldDynamic currentState,
                                        PlanAction action,
                                        KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                                        StoryNode currentNode,
                                        bool connection,
                                        int globalNodeNumber,
                                        bool succsessControl)
        {
            WorldDynamic worldForTest = (WorldDynamic)currentState.Clone();
            if (!succsessControl) { action.Fail(ref worldForTest); }
            else { action.ApplyEffects(ref worldForTest); }
            worldForTest.UpdateHashCode();

            StoryNode testNode = new StoryNode();
            //if (counteract) { testNode.counteract = true; }
            //testNode.SetWorldState(worldForTest);
            testNode.SetWorldState((WorldDynamic)worldForTest.Clone());

            // Create a clone of the agent.
            //KeyValuePair<AgentStateStatic, AgentStateDynamic> newAgent =
            //    new KeyValuePair<AgentStateStatic, AgentStateDynamic>((AgentStateStatic)agent.Key.Clone(), (AgentStateDynamic)agent.Value.Clone());
            testNode.SetActiveAgent(testNode.GetWorldState().GetAgentByName(agent.Key.GetName()));

            // We take the last node from the list of all nodes and assign whether the player is active and which of the agents was active on this turn.
            if (agent.Key.GetRole() == AgentRole.PLAYER) { testNode.SetActivePlayer(true); }
            else { testNode.SetActivePlayer(false); }

            //testNode.SetActiveAgent(newAgent);

            /*if (connection)
            {
                ConnectionTwoNodes(action, currentNode, testNode, false);
            }*/

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
                                  bool succsessControl,
                                  bool counteract)
        {
            WorldDynamic newState = (WorldDynamic)currentState.Clone();
            if (!succsessControl) { action.Fail(ref newState); }
            else { action.ApplyEffects(ref newState); }
            newState.UpdateHashCode();

            // Create an empty new node.
            StoryNode newNode = new StoryNode();
            if (counteract) { newNode.counteract = true; }

            // We assign the state of the world (transferred) to the new node.
            newNode.SetWorldState((WorldDynamic)newState.Clone());

            newNode.SetActiveAgent(newNode.GetWorldState().GetAgentByName(agent.Key.GetName()));
            if (agent.Key.GetRole() == AgentRole.PLAYER) { newNode.SetActivePlayer(true); }
            else { newNode.SetActivePlayer(false); }

            ConnectionTwoNodes(action, currentNode, newNode, false);

            globalNodeNumber++;
            newNode.SetNumberInSequence(globalNodeNumber);

            if (nodes.Contains(newNode))
            {
                bool test = true;
            }

            // Add a new node to the graph.
            AddNode(newNode);
        }

        public void CreateRootNode(PlanAction action,
                                   KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                                   WorldDynamic currentState,
                                   StoryNode currentNode,
                                   ref int globalNodeNumber, 
                                   bool succsessControl)
        {
            WorldDynamic newState = (WorldDynamic)currentState.Clone();
            if (!succsessControl) { action.Fail(ref newState); }
            else { action.ApplyEffects(ref newState); }

            if (agent.Key.GetRole() == AgentRole.PLAYER) { currentNode.SetActivePlayer(true); }
            else { currentNode.SetActivePlayer(false); }

            // We assign the state of the world (transferred) to the new node.
            currentNode.SetWorldState((WorldDynamic)newState.Clone());

            //Edge newEdge = new Edge();

            // We adjust the edge - assign its action and indicate the nodes that it connects.
            //newEdge.SetAction(action);
            //newEdge.SetUpperNode(ref currentNode);

            //currentNode.AddEdge(newEdge);

            globalNodeNumber++;
            currentNode.SetNumberInSequence(globalNodeNumber);
        }

        public void CreateEndNode() { }

        public void DeleteTestNode(ref StoryNode testNode)
        {
            foreach (var edge in testNode.GetEdges().ToList())
            {
                edge.GetUpperNode().RemoveEdge(edge);
                edge.GetLowerNode().RemoveEdge(edge);
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
                                            int globalNodeNumber,
                                            ref Queue<StoryNode> queue,
                                            bool succsessControl,
                                            ref bool skip)
        {
            StoryNode testNode = CreateTestNode(currentState, action, agent, currentNode, false, globalNodeNumber, succsessControl);
            testNode.UpdateHashCode();

            if (!testNode.Equals(currentNode))
            {
                foreach (var checkedNode in nodes)
                {
                    if (TwoNodesComparison(testNode, checkedNode) && !currentNode.ConnectedWith(checkedNode))
                    {
                        DeleteTestNode(ref testNode);
                        ConnectionTwoNodes(action, currentNode, checkedNode, true);
                        break;
                    }
                }
            }
            else
            {
                DeleteTestNode(ref testNode);
                skip = true;
            }
        }
    }
}
