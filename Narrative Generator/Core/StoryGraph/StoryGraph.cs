using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class that is a representation of a narrative graph, where nodes are states of the world and edges are actions (inside nodes).
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

        /// <summary>
        /// This method searches specified node among all nodes that make up the storygraph.
        /// </summary>
        /// <param name="node">The node to be found.</param>
        /// <returns>True if the node is in the graph, false if it is not.</returns>
        public bool FindNode (StoryNode node) { return nodes.Contains(node); }

        /// <summary>
        /// Returns a list of nodes in the storygraph.
        /// </summary>
        /// <returns>All nodes in the storygraph.</returns>
        public HashSet<StoryNode> GetNodes() { return nodes; }

        /// <summary>
        /// Returns the root node of the storygraph.
        /// </summary>
        /// <returns>Root node.</returns>
        public StoryNode GetRoot() { return root; }

        /// <summary>
        /// Sets the root node for the storygraph.
        /// </summary>
        /// <param name="newRoot">The node that will become the new root.</param>
        public void SetRoot(StoryNode newRoot)
        {
            root = newRoot;
            AddNode(newRoot);
        }

        /// <summary>
        /// Returns the last node from the list of nodes in the storygraph.
        /// </summary>
        /// <returns>Last node in the storygraph.</returns>
        public StoryNode GetLastNode() { return nodes.Last(); }

        /// <summary>
        /// Returns the specified node from the storygraph.
        /// </summary>
        /// <param name="index">The index of the requested node.</param>
        /// <returns>Specified node from the storygraph.</returns>
        public StoryNode GetNode (int index) { return nodes.ElementAt(index); }

        /// <summary>
        /// Returns the specified node from the storygraph.
        /// </summary>
        /// <param name="node">Parameters of the node to get.</param>
        /// <returns>Specified node from the storygraph.</returns>
        public StoryNode GetNode (StoryNode node)
        {
            foreach (var n in nodes) { if (node.Equals(n)) { return n; } }
            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Connects two specified nodes with an edge.
        /// </summary>
        /// <param name="action">An action that triggers a transition from one state to another and is written to the edge.</param>
        /// <param name="firstNode">First connected node, current state.</param>
        /// <param name="secondNode">Second connected node, new state.</param>
        /// <param name="duplicate">An indication that the second node already exists in the graph (it does not need to be created).</param>
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
        }

        /// <summary>
        /// This method creates a node with the specified parameters, which will then be deleted.
        /// </summary>
        /// <param name="currentState">The current state of the storyworld.</param>
        /// <param name="action">The action that the current agent has chosen to perform on the current turn.</param>
        /// <param name="agent">The acting agent that performs the action.</param>
        /// <param name="currentNode">A graph node that stores the current state.</param>
        /// <param name="globalNodeNumber">The number of the last created node.</param>
        /// <param name="succsessControl">Indicates whether the action was successful or not.</param>
        /// <returns>Constructed node.</returns>
        public StoryNode CreateTestNode(WorldDynamic currentState,
                                        PlanAction action,
                                        KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                                        StoryNode currentNode,
                                        int globalNodeNumber,
                                        bool succsessControl)
        {
            WorldDynamic worldForTest = (WorldDynamic)currentState.Clone();
            if (!succsessControl) { action.Fail(ref worldForTest); }
            else { action.ApplyEffects(ref worldForTest); }
            worldForTest.UpdateHashCode();

            StoryNode testNode = new StoryNode();
            testNode.SetWorldState((WorldDynamic)worldForTest.Clone());
            testNode.SetActiveAgent(testNode.GetWorldState().GetAgentByName(agent.Key.GetName()));

            // We take the last node from the list of all nodes and assign whether the player is active and which of the agents was active on this turn.
            if (agent.Key.GetRole() == AgentRole.PLAYER) { testNode.SetActivePlayer(true); }
            else { testNode.SetActivePlayer(false); }

            testNode.SetNumberInSequence(globalNodeNumber + 1);

            return testNode;
        }

        /// <summary>
        /// Create a new node for the storygraph and inserts it.
        /// </summary>
        /// <param name="action">The action that the current agent has chosen to perform on the current turn.</param>
        /// <param name="agent">The acting agent that performs the action.</param>
        /// <param name="currentState">The current state of the storyworld.</param>
        /// <param name="currentNode">A graph node that stores the current state.</param>
        /// <param name="globalNodeNumber">The number of the last created node.</param>
        /// <param name="succsessControl">Indicates whether the action was successful or not.</param>
        /// <param name="counteract">Indicates whether the action was a counter-reaction or a normal action.</param>
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

            // Add a new node to the graph.
            AddNode(newNode);
        }

        /// <summary>
        /// Create a root node for the storygraph and inserts it.
        /// </summary>
        /// <param name="action">The action that the current agent has chosen to perform on the current turn.</param>
        /// <param name="agent">The acting agent that performs the action.</param>
        /// <param name="currentState">The current state of the storyworld.</param>
        /// <param name="currentNode">A graph node that stores the current state.</param>
        /// <param name="globalNodeNumber">The number of the last created node.</param>
        /// <param name="succsessControl">Indicates whether the action was successful or not.</param>
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

            globalNodeNumber++;
            currentNode.SetNumberInSequence(globalNodeNumber);
        }

        /// <summary>
        /// Delete the specified test node.
        /// </summary>
        /// <param name="testNode">Link to node.</param>
        public void DeleteTestNode (ref StoryNode testNode)
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

        /// <summary>
        /// Checking for the presence of the specified node in the graph.
        /// </summary>
        /// <param name="checkedNode">The looking node.</param>
        /// <returns>True if the specified node is in the graph, false otherwise.</returns>
        public bool NodeExistenceControl (StoryNode checkedNode)
        {
            foreach (var node in GetNodes())
            {
                if (TwoNodesComparison(node, checkedNode)) { return true; }
            }

            return false;
        }

        /// <summary>
        /// Method for comparing two nodes with each other.
        /// </summary>
        /// <param name="nodeOne">The first node to compare.</param>
        /// <param name="nodeTwo">The second node to compare.</param>
        /// <returns>True if the nodes are the same, false otherwise.</returns>
        public bool TwoNodesComparison (StoryNode nodeOne, StoryNode nodeTwo)
        {
            if (nodeOne.Equals(nodeTwo)) { return true; }
            return false;
        }

        /// <summary>
        /// Connects two nodes if both already exist and are in the graph.
        /// </summary>
        /// <param name="currentState">The current state of the storyworld.</param>
        /// <param name="action">The action that the current agent has chosen to perform on the current turn.</param>
        /// <param name="agent">The acting agent that performs the action.</param>
        /// <param name="currentNode">A graph node that stores the current state.</param>
        /// <param name="globalNodeNumber">The number of the last created node.</param>
        /// <param name="queue">Reference to the queue of nodes to be processed.</param>
        /// <param name="succsessControl">Indicates whether the action was successful or not.</param>
        /// <param name="skip">A reference to a variable that will indicate if the creation of a new node should be skipped.</param>
        public void DuplicateNodeConnecting (WorldDynamic currentState, 
                                             PlanAction action, 
                                             KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                                             StoryNode currentNode,
                                             int globalNodeNumber,
                                             ref Queue<StoryNode> queue,
                                             bool succsessControl,
                                             ref bool skip)
        {
            StoryNode testNode = CreateTestNode(currentState, action, agent, currentNode, globalNodeNumber, succsessControl);
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
