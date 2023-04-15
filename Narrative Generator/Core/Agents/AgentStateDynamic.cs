using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Narrative_Generator
{
    /// <summary>
    /// The class that implements the agent, its behavior and the information it stores. The dynamic part, which includes values that change during the development of story.
    /// </summary>
    [Serializable]
    public class AgentStateDynamic : IEquatable<AgentStateDynamic>, ICloneable
    {
        // Action components
        /// <summary>
        /// The agent's action plan.
        /// </summary>
        private Plan myCurrentPlan;
        /// <summary>
        /// List of actions for the agent to perform at the current moment.
        /// </summary>
        private List<PlanAction> myAvailableActions;

        // Properties
        /// <summary>
        /// Information about the agent that does not change over time.
        /// </summary>
        private AgentStateStatic agentInfo;
        /// <summary>
        /// The current status of the agent.
        /// </summary>
        private bool alive;
        /// <summary>
        /// The agent's goals.
        /// </summary>
        private Goal myGoals;
        /// <summary>
        /// The value of the agent's initiative, which determines the order in which agents perform actions.
        /// </summary>
        private int initiative;
        /// <summary>
        /// Agent behavior modifier: whether the agent is in a state of fear.
        /// </summary>
        private bool scared;
        /// <summary>
        /// Agent behavior modifier: whether the agent is angry with another agent.
        /// </summary>
        private AgentAngryAt angryAt;
        /// <summary>
        /// Agent behavior modifier: whether the agent has the location he wants to go to.
        /// </summary>
        private LocationStatic wantsToGo;
        /// <summary>
        /// Agent behavior modifier: whether the agent intends to entrap another agent into a trap.
        /// </summary>
        private WantToEntrap wantToEntrap;
        /// <summary>
        /// Agent behavior modifier: whether this agent is currently talking to another agent.
        /// </summary>
        private TalkingWith talkingWith;
        /// <summary>
        /// The counter of turns in which the agent was idle.
        /// </summary>
        private int skipedTurns;
        /// <summary>
        /// The counter of the remaining turns until the agent can move.
        /// </summary>
        private int timeToMove;
        /// <summary>
        /// The counter of quests completed by the agent.
        /// </summary>
        private int complitedQuestsCounter;

        // Beliefs
        /// <summary>
        /// Representation of the agent's beliefs about the surrounding world.
        /// </summary>
        private WorldContext beliefs;
        /// <summary>
        /// Information about the evidence found by the agent.
        /// </summary>
        private AgentFoundEvidence foundEvidence;
        /// <summary>
        /// List of locations investigated by the agent (in search of evidences).
        /// </summary>
        private HashSet<LocationStatic> exploredRooms;

        // Hashcode
        /// <summary>
        /// An indicator of whether a hashcode has been generated for this component.
        /// </summary>
        private bool hasHashCode;
        /// <summary>
        /// The hashcode of this component.
        /// </summary>
        private int hashCode;

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public AgentStateDynamic()
        {
            agentInfo = new AgentStateStatic();
            myCurrentPlan = new Plan();
            myAvailableActions = new List<PlanAction>();
            alive = true;
            myGoals = new Goal();
            beliefs = new WorldContext();
            initiative = 0;
            angryAt = new AgentAngryAt();
            scared = false;
            foundEvidence = new AgentFoundEvidence();
            wantsToGo = new LocationStatic();
            exploredRooms = new HashSet<LocationStatic>();
            wantToEntrap = new WantToEntrap();
            talkingWith = new TalkingWith();
            hasHashCode = false;
            hashCode = 0;
            skipedTurns = 0;
            timeToMove = 0; // 2
            complitedQuestsCounter = 0;
        }

        /// <summary>
        /// Constructor with parameters of the dynamic part of the agent, which creates a new instance of the agent based on the passed clone.
        /// </summary>
        /// <param name="clone">An agent instance that will serve as the basis for creating a new instance.</param>
        public AgentStateDynamic (AgentStateDynamic clone)
        {
            agentInfo = (AgentStateStatic)clone.agentInfo.Clone();
            myCurrentPlan = (Plan)clone.myCurrentPlan.Clone();
            myAvailableActions = new List<PlanAction>(clone.myAvailableActions);
            alive = clone.alive;
            myGoals = (Goal)clone.myGoals.Clone();
            beliefs = (WorldContext)clone.beliefs.Clone();
            initiative = clone.initiative;
            angryAt = (AgentAngryAt)clone.angryAt.Clone();
            scared = clone.scared;
            foundEvidence = (AgentFoundEvidence)clone.foundEvidence.Clone();
            if (clone.wantsToGo != null) { wantsToGo = (LocationStatic)clone.wantsToGo.Clone();  }
            else { wantsToGo = new LocationStatic(); }
            exploredRooms = new HashSet<LocationStatic>(clone.exploredRooms);
            wantToEntrap = (WantToEntrap)clone.wantToEntrap.Clone();
            talkingWith = (TalkingWith)clone.talkingWith.Clone();
            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
            skipedTurns = clone.skipedTurns;
            timeToMove = clone.timeToMove;
            complitedQuestsCounter = clone.complitedQuestsCounter;
        }

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        /// <param name="alive">The current status of the agent.</param>
        /// <param name="goals">The agent's goals.</param>
        /// <param name="beliefs">Representation of the agent's beliefs about the surrounding world.</param>
        /// <param name="agentInfo">Information about the agent that does not change over time.</param>
        public AgentStateDynamic (bool alive, Goal goals, WorldContext beliefs, AgentStateStatic agentInfo)
        {
            this.agentInfo = agentInfo;

            myCurrentPlan = new Plan();
            myAvailableActions = new List<PlanAction>();

            SetStatus(alive);
            SetGoal(goals);
            SetBeliefs(beliefs);
            SetInitiative(0);

            AgentAngryAt angryAt = new AgentAngryAt(false, null);
            SetObjectOfAngry(angryAt);

            ScaredOff();

            AgentFoundEvidence foundEvidence = new AgentFoundEvidence(false, null);
            AddEvidence(foundEvidence);

            WantToEntrap wantToEntrap = new WantToEntrap(false, null, null);
            SetEntrap(wantToEntrap);

            TalkingWith talkingWith = new TalkingWith(false, null);
            SetTalking(talkingWith);

            SetTargetLocation(null);

            exploredRooms = new HashSet<LocationStatic>();

            hasHashCode = false;
            hashCode = 0;
            skipedTurns = 0;
            timeToMove = 0;
            complitedQuestsCounter = 0;
        }

        /// <summary>
        /// Deep cloning of an agent.
        /// </summary>
        /// <returns>A new instance that is a copy of the current one.</returns>
        public object Clone()
        {
            var clone = new AgentStateDynamic();

            clone.myCurrentPlan = new Plan(myCurrentPlan);
            clone.myAvailableActions = new List<PlanAction>(myAvailableActions);
            clone.alive = alive;
            clone.myGoals = new Goal(myGoals);
            clone.beliefs = new WorldContext(beliefs);
            clone.initiative = initiative;
            if (angryAt != null && angryAt.GetObjectOfAngry() != null) { clone.angryAt = new AgentAngryAt(angryAt); }
            clone.scared = scared;
            if (foundEvidence != null && foundEvidence.GetCriminal() != null) { clone.foundEvidence = new AgentFoundEvidence(foundEvidence); }
            if (wantsToGo != null) { clone.wantsToGo = new LocationStatic(wantsToGo); }
            clone.exploredRooms = new HashSet<LocationStatic>(exploredRooms);
            if (wantToEntrap != null && wantToEntrap.GetVictim() != null && wantToEntrap.GetLocation() != null)
            {
                clone.wantToEntrap = new WantToEntrap(wantToEntrap);
            }
            if (talkingWith != null && talkingWith.GetInterlocutor() != null) { clone.talkingWith = new TalkingWith(talkingWith); }
            clone.skipedTurns = skipedTurns;
            clone.timeToMove = timeToMove;
            clone.complitedQuestsCounter = complitedQuestsCounter;

            return clone;
        }

        /// <summary>
        /// Calculate an action plan for the agent based on PDDL files with descriptions of the domain and problem.
        /// </summary>
        /// <param name="agent">The acting agent that performs the action.</param>
        /// <param name="currentWorldState">The current state of the storyworld.</param>
        /// <param name="note">Text to display on the main screen.</param>
        public void CalculatePlan (KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, WorldDynamic currentWorldState, ref TextBox note)
        {
            note.Text = "AGENT'S PLAN CALCULATING";

            // We create an instance of the FastDownward class to interact with the planner.
            FastDownward fastDownward = new FastDownward();

            PDDL_Module pddlModule = new PDDL_Module(currentWorldState.GetStaticWorldPart().GetSetting(), currentWorldState.GetStaticWorldPart().GetConnectionStatus(), 
                                                       currentWorldState.GetAgents().Count, currentWorldState.GetStaticWorldPart().GetCanFindEvidence());
 
            // We initialize variables containing the names of the Domain and Problem PDDL files.
            string domainFileName = null;
            string problemFileName = null;

            // We clear the current plan (we re-calculate it, not add to it).
            myCurrentPlan.Clear();

            // The agent's role affects what actions are available to him and what goals he pursues, therefore, the role determines what
            //    what PDDL files need to be processed.
            domainFileName = agent.Key.GetRole().ToString() + "Domain";
            problemFileName = agent.Key.GetRole().ToString() + "Problem";

            // We launch the planner, specifying the names of the files with the corresponding Domain and Problem.
            fastDownward.Run(domainFileName, problemFileName, ref note);

            // In the event that the planner successfully completed its work.
            if (fastDownward.isSuccess)
            {
                // Then we try to extract the plan from the file created by the planner.
                fastDownward.GetResultPlan(ref myCurrentPlan, currentWorldState, ref note);

                // If, for some reason, it was not possible to extract the plan, then recursively try to generate a new file with the Problem
                //    and fetch the plan again.
                if (!myCurrentPlan.planReceived)
                {
                    pddlModule.GeneratePDDLProblem(agent, currentWorldState, ref note);
                    CalculatePlan(agent, currentWorldState, ref note);
                }
            }
            // If the planner completed its work with an error, then recursively try to generate a new file with the Problem
            //    and fetch the plan again.
            else
            {
                pddlModule.GeneratePDDLProblem(agent, currentWorldState, ref note);
                CalculatePlan(agent, currentWorldState, ref note);
            }
        }

        /// <summary>
        /// Populates the list of actions available to the agent.
        /// </summary>
        /// <param name="agent">The acting agent that performs the action.</param>
        /// <param name="currentState">The current state of the storyworld.</param>
        /// <param name="note">Text to display on the main screen.</param>
        public void ReceiveAvailableActions (KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, WorldDynamic currentState, ref TextBox note)
        {
            note.Text = "LIST OF ACTIONS AVAILABLE FOR AGENT RECEIVING";

            ActionGenerator actionGenerator = new ActionGenerator();
            myAvailableActions = actionGenerator.GetAvailableActions(agent, currentState);
        }

        /// <summary>
        /// A method that returns a list of currently available actions for the agent.
        /// </summary>
        /// <returns>List of action instances.</returns>
        public List<PlanAction> GetAvailableActions()
        {
            return myAvailableActions;
        }

        /// <summary>
        /// Correlates the list of actions available to the agent with the first action in its action plan, and returns the first match or null.
        /// </summary>
        public PlanAction ChooseAction()
        {
            // We go through all the actions available to the agent.
            for (int i = 0; i < myAvailableActions.Count(); i++)
            {
                // If one of the available actions matches the first action in the plan.
                if (myCurrentPlan.GetAction(0).GetType() == myAvailableActions[i].GetType())
                {
                    if (myCurrentPlan.GetAction(0).Arguments.Count() != 0)
                    {
                        Move testMoveAction = new Move();

                        if (myCurrentPlan.GetAction(0).GetType() == testMoveAction.GetType())
                        {
                            return myCurrentPlan.GetAction(0);
                        }
                    }
                    else
                    {
                        // Then select it.
                        return myAvailableActions[i];
                    }
                }
            }

            // If none of the available actions coincided with the first in the plan, then return null.
            return null;
        }

        /// <summary>
        /// The method that sets the agent's status.
        /// </summary>
        /// <param name="status">Status value (alive - true / dead - false)</param>
        public void SetStatus (bool status)
        {
            alive = status;
            UpdateHashCode();
        }

        /// <summary>
        /// Method that returns the value of the agent's status.
        /// </summary>
        /// <returns>Alive - true / dead - false</returns>
        public bool GetStatus() { return alive; }

        /// <summary>
        /// The method that sets the status value to false (dead).
        /// </summary>
        public void Die()
        {
            alive = false;
            UpdateHashCode();
        }

        /// <summary>
        /// A method for setting a goal for an agent.
        /// </summary>
        /// <param name="goal">An instance of the goal world states descryption that will be copied into the agent's beliefs about the goal states.</param>
        public void SetGoal (Goal goal) { myGoals = (Goal)goal.Clone(); }

        /// <summary>
        /// Returns an object of the "Goal" class that stores information about goal states for this agent.
        /// </summary>
        /// <returns>Information about goal states for this agent</returns>
        public Goal GetGoal() { return myGoals; }

        /// <summary>
        /// A method that allows to establish the agent's beliefs about the storyworld (environment).
        /// </summary>
        /// <param name="beliefs">An object that stores information about the agent's beliefs about the storyworld.</param>
        public void SetBeliefs (WorldContext beliefs) { this.beliefs = beliefs; }

        /// <summary>
        /// The method allows to receive information about the agent's beliefs about the storyworld (environment).
        /// </summary>
        /// <returns>An object that stores information about the agent's beliefs about the storyworld.</returns>
        public WorldContext GetBeliefs() { return beliefs; }

        /// <summary>
        /// A method that allows to set value of the initiative of this agent.
        /// </summary>
        /// <param name="initiative">The new numerical value of the initiative.</param>
        public void SetInitiative (int initiative) { this.initiative = initiative; }

        /// <summary>
        /// A method that allows to get the current value of the agent's initiative.
        /// </summary>
        /// <returns>Numerical value of the initiative.</returns>
        public int GetInitiative() { return initiative; }

        /// <summary>
        /// Updates the agent's beliefs about the location where he is.
        /// </summary>
        /// <param name="agent">The acting agent that performs the action.</param>
        /// <param name="currentWorldState">The current state of the storyworld.</param>
        /// <param name="note">Text to display on the main screen.</param>
        public void RefreshBeliefsAboutTheWorld (KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, WorldDynamic currentWorldState, ref TextBox note)
        {
            note.Text = "AGENT BELIEFS REFRESH";

            // Before clearing the information, remember the location in which the agent is located.
            //LocationStatic agentIsHereLoc = agent.Value.GetBeliefs().SearchAgentAmongLocations(agent.Key);
            LocationStatic agentIsHereLoc = currentWorldState.GetLocationByName(currentWorldState.SearchAgentAmongLocations(agent.Key).GetName()).Key;

            // We clear the information about the location in which the agent is located, in his beliefs.
            agent.Value.GetBeliefs().GetAgentByName(agent.Key.GetName()).ClearLocation();

            // We find the same location in the "real" world. We go through the agents in it. We are looking for agents 
            //    with the same names in the agent's beliefs. We add them to the location (in his beliefs) where he (in his belief) is.
            foreach (var agent1 in currentWorldState.GetLocationByName(agentIsHereLoc.GetName()).Value.GetAgents())
            {
                foreach (var agent2 in agent.Value.GetBeliefs().GetAgentsInWorld())
                {
                    if (agent1.Key.GetName() == agent2.GetInfo().GetName())
                    {
                        agent.Value.GetBeliefs().GetAgentByName(agent2.GetInfo().GetName()).SetLocation(agentIsHereLoc);

                        if (!agent2.CheckStatus())
                        {
                            foreach (var a in currentWorldState.GetAgents())
                            {
                                a.Value.GetBeliefs().GetAgentByName(agent2.GetInfo().GetName()).Dead();
                            }
                        }

                        break;
                    }
                }
            }

            foreach (var agent1 in agent.Value.GetBeliefs().GetAgentsInWorld())
            {
                if (agent1.GetLocation().GetName() == agentIsHereLoc.GetName() 
                    && currentWorldState.GetAgentByName(agent1.GetInfo().GetName()).Value.GetMyLocation().GetName() != agentIsHereLoc.GetName()
                    && agent.Key.GetName() != agent1.GetInfo().GetName())
                {
                    if (currentWorldState.GetStaticWorldPart().GetConnectionStatus())
                    {
                        agent1.SetLocation(currentWorldState.GetLocationByName(agentIsHereLoc.GetName()).Key.GetRandomConnectedLocation());
                    }
                    else
                    {
                        agent1.SetLocation(currentWorldState.GetRandomLocationWithout(currentWorldState.GetLocationByName(agentIsHereLoc.GetName())).Key);
                    }
                }
            }
        }

        /// <summary>
        /// A method that allows to set the target location for the agent (the one he wants to go to).
        /// </summary>
        /// <param name="location">Information about the target location.</param>
        public void SetTargetLocation (LocationStatic location)
        {
            wantsToGo = location;
            UpdateHashCode();
        }

        /// <summary>
        /// A method that returns information about the target location of this agent.
        /// </summary>
        /// <returns>Information about the target location of this agent.</returns>
        public LocationStatic GetTargetLocation() { return wantsToGo; }

        /// <summary>
        /// A method that allows to clear information about the target location of this agent (delete it).
        /// </summary>
        public void ClearTargetLocation()
        {
            wantsToGo = null;
            UpdateHashCode();
        }

        /// <summary>
        /// A method to find out if a target location exists for this agent.
        /// </summary>
        /// <returns>True if exists, false otherwise.</returns>
        public bool CheckTargetLocation()
        {
            if (wantsToGo.GetName() == "") { return false; }
            else { return true; }
        }

        /// <summary>
        /// A method that allows to set the agent with which this agent will be angry.
        /// </summary>
        /// <param name="target">Information about the target agent.</param>
        public void SetObjectOfAngry (AgentStateStatic target)
        {
            angryAt.AngryOn();
            angryAt.SetObjectOfAngry(target);
            UpdateHashCode();
        }

        /// <summary>
        /// A method that allows to set the agent with which this agent will be angry.
        /// </summary>
        /// <param name="angryAt">An object that stores information that the agent is angry with someone.</param>
        public void SetObjectOfAngry (AgentAngryAt angryAt)
        {
            this.angryAt = angryAt;
            UpdateHashCode();
        }

        /// <summary>
        /// Returns an object containing information that the agent is angry with someone.
        /// </summary>
        /// <returns>Object containing information that the agent is angry with someone</returns>
        public AgentAngryAt GetObjectOfAngryComponent() { return angryAt; }

        /// <summary>
        /// Checking if this agent is angry with any other agent.
        /// </summary>
        /// <returns>Truth if angry, otherwise false.</returns>
        public bool AngryCheck()
        {
            if (angryAt != null) { return angryAt.AngryCheck(); }
            return false;
        }

        /// <summary>
        /// A method that allows an agent to start a conversation with another agent and remember this fact (while the conversation is in progress).
        /// </summary>
        /// <param name="interlocutor">Information about the interlocutor agent.</param>
        public void SetInterlocutor (AgentStateStatic interlocutor)
        {
            talkingWith.TalkingStart();
            talkingWith.SetInterlocutor(interlocutor);
            UpdateHashCode();
        }

        /// <summary>
        /// A method that allows to assign an object that stores information about whether the agent is talking now and who his interlocutor is.
        /// </summary>
        /// <param name="talkingWith">Object that stores information about whether the agent is talking now and who his interlocutor is</param>
        public void SetTalking (TalkingWith talkingWith)
        {
            this.talkingWith = talkingWith;
            UpdateHashCode();
        }

        /// <summary>
        /// A method that allows to start a conversation between two specified agents.
        /// </summary>
        /// <param name="agent1">First agent.</param>
        /// <param name="agent2">Second agent.</param>
        public void SetTalking (KeyValuePair<AgentStateStatic, AgentStateDynamic> agent1, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent2)
        {
            agent1.Value.SetInterlocutor(agent2);
            agent2.Value.SetInterlocutor(agent1);
        }

        /// <summary>
        /// A method that allows an agent to start a conversation with another agent and remember this fact (while the conversation is in progress).
        /// </summary>
        /// <param name="interlocutor">Interlocutor agent.</param>
        public void SetInterlocutor (KeyValuePair<AgentStateStatic, AgentStateDynamic> interlocutor)
        {
            talkingWith.TalkingStart();
            talkingWith.SetInterlocutor(interlocutor.Key);
            UpdateHashCode();
        }

        /// <summary>
        /// A method that allows to get an object that stores information about whether the agent is currently talking and if so, with whom.
        /// </summary>
        /// <returns>Object that stores information about whether the agent is currently talking and if so, with whom</returns>
        public TalkingWith GetTalking() { return talkingWith; }

        /// <summary>
        /// Method for checking if this agent is talking to some other agent.
        /// </summary>
        /// <returns>True if yes, otherwise false</returns>
        public bool CheckTalking() { return talkingWith.TalkingCheck(); }

        /// <summary>
        /// A method to clear information about this agent's conversation.
        /// </summary>
        public void ClearTalking() { talkingWith.Clear(); }

        /// <summary>
        /// A method that allows to assign this agent an object that stores information about whether he wants to entrap someone.
        /// </summary>
        /// <param name="wantToEntrap">Object that stores information about whether agent wants to entrap someone.</param>
        public void SetEntrap (WantToEntrap wantToEntrap)
        {
            this.wantToEntrap = wantToEntrap;
            UpdateHashCode();
        }

        /// <summary>
        /// A method that allows this agent to start entrap the specified agent at the specified location.
        /// </summary>
        /// <param name="victim">Information about the agent that this agent wants to entrap to another location.</param>
        /// <param name="location">Information about the location this agent wants to entrap the specified agent.</param>
        public void SetEntrap (AgentStateStatic victim, LocationStatic location)
        {
            wantToEntrap.EntrapingStart();
            wantToEntrap.SetVictimAndLocation(victim, location);
            UpdateHashCode();
        }

        /// <summary>
        /// A method that allows you to get an object that stores the beliefs of this agent about whether he wants to entrap someone to another location, 
        ///    and if so, who and where.
        /// </summary>
        /// <returns>Object that stores the beliefs of this agent about whether he wants to entrap someone to another location, and if so, who and where.</returns>
        public WantToEntrap GetEntrap() { return wantToEntrap; }

        /// <summary>
        /// A method that checks if this agent wants to entrap some other agent to some location.
        /// </summary>
        /// <returns>True if yes, otherwise false.</returns>
        public bool CheckEntrap() { return wantToEntrap.EntrapCheck(); }

        /// <summary>
        /// A method that allows to clear the information that the this agent is angry with some other agent (delete it).
        /// </summary>
        public void CalmDown()
        {
            angryAt.AngryOff();
            angryAt.SetObjectOfAngry(null);
            UpdateHashCode();
        }

        /// <summary>
        /// A method that allows to add information about the specified location to the list of explored locations.
        /// </summary>
        /// <param name="location">Information about the location.</param>
        public void AddExploredLocation (LocationStatic location)
        {
            foreach (var loc in exploredRooms)
            {
                if (loc.GetName() == location.GetName()) { return; }
            }

            exploredRooms.Add(location);
            UpdateHashCode();
        }

        /// <summary>
        /// A method that allows to add information about the specified locations to the list of explored locations.
        /// </summary>
        /// <param name="locations">List of locations.</param>
        public void AddExploredLocations (HashSet<LocationStatic> locations) {  exploredRooms = locations; }

        /// <summary>
        /// Returns a list of locations explored by this agent.
        /// </summary>
        /// <returns>List of explored locations.</returns>
        public HashSet<LocationStatic> GetExploredLocations() { return exploredRooms; }

        /// <summary>
        /// Returns information about a location from the list of locations explored by this agent, according to the specified index.
        /// </summary>
        /// <param name="index">The index of the required location.</param>
        /// <returns>Information about the specified location.</returns>
        public LocationStatic GetExploredLocation (int index) { return exploredRooms.ElementAt(index); }

        /// <summary>
        /// Checks if the specified location has been explored by this agent.
        /// </summary>
        /// <param name="location">Information about the checked location.</param>
        /// <returns>True if yes, otherwise no.</returns>
        public bool CheckIfLocationIsExplored (LocationStatic location)
        {
            foreach (var loc in exploredRooms) { if (loc.Equals(location)) { return true; } }
            return false;
        }

        /// <summary>
        /// A method to give this agent evidence against the specified agent.
        /// </summary>
        /// <param name="criminal">Information about the agent against whom the evidence will be added.</param>
        public void AddEvidence (AgentStateStatic criminal)
        {
            foundEvidence.IsEvidence();
            foundEvidence.SetCriminal(criminal);
            UpdateHashCode();
        }

        /// <summary>
        /// A method that allows to assign to this agent an object that stores information about the presence of evidence against other agents.
        /// </summary>
        /// <param name="foundEvidence">Object that stores information about the presence of evidence against other agents.</param>
        public void AddEvidence (AgentFoundEvidence foundEvidence)
        {
            this.foundEvidence = foundEvidence;
            UpdateHashCode();
        }

        /// <summary>
        /// A method that allows to return information about the presence of evidence against other agents.
        /// </summary>
        /// <returns>Object that stores information about the presence of evidence against other agents.</returns>
        public AgentFoundEvidence GetEvidenceStatus() { return foundEvidence; }

        /// <summary>
        /// Clears information about evidence against other agents from this agent (deletes it).
        /// </summary>
        public void ClearEvidence()
        {
            foundEvidence.Clear();
            UpdateHashCode();
        }

        /// <summary>
        /// Checks if the agent knows that one of the other agents is angry.
        /// </summary>
        /// <returns>True if yes, otherwise false.</returns>
        public bool ThinksThatSomeoneIsAngry()
        {
            foreach (var a in beliefs.GetAgentsInWorld())
            {
                if (a.GetObjectOfAngry().AngryCheck()) { return true; }
            }

            return false;
        }

        /// <summary>
        /// Puts the agent in the "Scared" state.
        /// </summary>
        public void ScaredOn()
        {
            scared = true;
            UpdateHashCode();
        }

        /// <summary>
        /// Get out the agent from the "Scared" state.
        /// </summary>
        public void ScaredOff()
        {
            scared = false;
            UpdateHashCode();
        }

        /// <summary>
        /// Checks if the agent is scared.
        /// </summary>
        /// <returns>True if yes, otherwise false.</returns>
        public bool CheckScared() { return scared; }

        /// <summary>
        /// Returns information about whether the agent found the plan or not.
        /// </summary>
        /// <returns>True if yes, otherwise false.</returns>
        public bool GetPlanStatus() { return myCurrentPlan.planReceived; }

        /// <summary>
        /// Returns the action plan for this agent.
        /// </summary>
        /// <returns>Plan instance.</returns>
        public Plan GetPlan() { return myCurrentPlan; }

        /// <summary>
        /// Assigns static (unchanging) information about this agent.
        /// </summary>
        /// <param name="agentInfo">An instance of static (unchangeable) information about this agent.</param>
        public void SetAgentInfo (AgentStateStatic agentInfo)
        {
            this.agentInfo = agentInfo;
            UpdateHashCode();
        }

        /// <summary>
        /// Returns static (unchangeable) information about this agent.
        /// </summary>
        /// <returns>Information about this agent.</returns>
        public AgentStateStatic GetAgentInfo() { return agentInfo; }

        /// <summary>
        /// Returns information about the location in which this agent is located (according to his beliefs).
        /// </summary>
        /// <returns>Information about location.</returns>
        public LocationStatic GetMyLocation()
        {
            foreach (var location in GetBeliefs().GetLocationsInWorld())
            {
                if (location.GetName() == beliefs.GetMyLocation().GetName()) { return location; }
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Removes all temporary states of the agent.
        /// </summary>
        public void ClearTempStates() { this.ClearTalking(); }

        /// <summary>
        /// Sets the number of moves skipped by the agent (in which he did nothing).
        /// </summary>
        /// <param name="value">Number of moves skipped by the agent.</param>
        public void SetSkippedTurns (int value) { skipedTurns = value; }


        /// <summary>
        /// Returns the number of moves skipped by the agent (in which he did nothing).
        /// </summary>
        /// <returns>Number of moves skipped by the agent.</returns>
        public int GetSkipedTurns() { return skipedTurns; }

        /// <summary>
        /// Increases the number of moves skipped by the agent (in which he did nothing) by one.
        /// </summary>
        public void IncreaseSkipedTurns() { skipedTurns++; }

        /// <summary>
        /// Sets the delay before moving to another location, in turns.
        /// </summary>
        /// <param name="value">Value of delay, in turns.</param>
        public void SetTimeToMove (int value) { timeToMove = value; }

        /// <summary>
        /// Returns the delay before moving to another location, in turns.
        /// </summary>
        /// <returns>Value of delay, in turns.</returns>
        public int GetTimeToMove() { return timeToMove; }

        /// <summary>
        /// Decrease the delay before moving to another location, in turns, by one.
        /// </summary>
        public void DecreaseTimeToMove() { if (timeToMove > 0) { timeToMove--; } }

        /// <summary>
        /// Sets the number of quests completed by this agent.
        /// </summary>
        /// <param name="value">New value of counter of quests completed by this agent.</param>
        public void SetComplitedQuestsCounter (int value) { complitedQuestsCounter = value; }

        /// <summary>
        /// Returns the number of quests completed by this agent.
        /// </summary>
        /// <returns>Value of counter of quests completed by this agent.</returns>
        public int GetComplitedQuestsCounter() { return complitedQuestsCounter; }

        /// <summary>
        /// Increases the counter of quests completed by this agent by one.
        /// </summary>
        public void CompleteQuest() { complitedQuestsCounter++; }

        /// <summary>
        /// Method for comparing two dynamic parts of agent states.
        /// </summary>
        /// <param name="anotherState">Another dynamic state of the agent, for comparison.</param>
        /// <returns>True if both states are the same, false otherwise.</returns>
        public bool Equals (AgentStateDynamic anotherState)
        {
            if (anotherState == null) { return false; }

            bool agentInfoEquals = agentInfo.Equals(anotherState.agentInfo);
            bool agentInfoReferenceEquals = object.ReferenceEquals(agentInfo, anotherState.agentInfo);

            bool statusEquals = (alive == anotherState.alive);
            bool statusReferenceEquals = object.ReferenceEquals(alive, anotherState.alive);

            //bool goalsEquals = myGoals.Equals(anotherState.myGoals);
            //bool goalsReferenceEquals = object.ReferenceEquals(myGoals, anotherState.myGoals);

            bool initiativeEquals = (initiative == anotherState.initiative);
            bool initiativeReferenceEquals = object.ReferenceEquals(initiative, anotherState.initiative);

            bool scaredEquals = (scared == anotherState.scared);
            bool scaredReferenceEquals = object.ReferenceEquals(scared, anotherState.scared);

            bool angryAtEquals = angryAt.Equals(anotherState.angryAt);
            bool angryAtReferenceEquals = object.ReferenceEquals(angryAt, anotherState.angryAt);

            bool wantsToGoEquals;
            bool wantsToGoReferenceEquals;
            if (wantsToGo == null && anotherState.wantsToGo == null)
            {
                wantsToGoEquals = true;
                wantsToGoReferenceEquals = true;
            }
            else if ((wantsToGo == null && anotherState.wantsToGo != null) || (wantsToGo != null && anotherState.wantsToGo == null))
            {
                wantsToGoEquals = false;
                wantsToGoReferenceEquals = false;
            }
            else
            {
                wantsToGoEquals = wantsToGo.Equals(anotherState.wantsToGo);
                wantsToGoReferenceEquals = object.ReferenceEquals(wantsToGo, anotherState.wantsToGo);
            }

            bool wantToEntrapEquals = wantToEntrap.Equals(anotherState.wantToEntrap);
            bool wantToEntrapReferenceEquals = object.ReferenceEquals(wantToEntrap, anotherState.wantToEntrap);

            bool talkingWithEquals = talkingWith.Equals(anotherState.talkingWith);
            bool talkingWithReferenceEquals = object.ReferenceEquals(talkingWith, anotherState.talkingWith);

            bool skipedTurnsEquals = (skipedTurns == anotherState.skipedTurns);
            bool skipedTurnsReferenceEquals = object.ReferenceEquals(skipedTurns, anotherState.skipedTurns);

            bool beliefsEquals = beliefs.Equals(anotherState.beliefs);
            bool beliefsReferenceEquals = object.ReferenceEquals(beliefs, anotherState.beliefs);

            bool foundEvidenceEquals = foundEvidence.Equals(anotherState.foundEvidence);
            bool foundEvidenceReferenceEquals = object.ReferenceEquals(foundEvidence, anotherState.foundEvidence);

            bool exploredRoomsEquals = true;
            bool exploredRoomsReferenceEquals = true;
            if (exploredRooms.Count == anotherState.exploredRooms.Count)
            {
                for (int i = 0; i < exploredRooms.Count; i++)
                {
                    if (!exploredRooms.ElementAt(i).Equals(anotherState.exploredRooms.ElementAt(i)))
                    {
                        exploredRoomsEquals = false;
                    }
                    if (!object.ReferenceEquals(exploredRooms.ElementAt(i), anotherState.exploredRooms.ElementAt(i)))
                    {
                        exploredRoomsReferenceEquals = false;
                    }
                }
            }
            else
            {
                exploredRoomsEquals = false;
                exploredRoomsReferenceEquals = false;
            }

            bool complitedQuestsCounterEquals = complitedQuestsCounter.Equals(anotherState.complitedQuestsCounter);

            bool agentInfoGlobal = agentInfoEquals || agentInfoReferenceEquals;
            bool statusGlobal = statusEquals || statusReferenceEquals;
            //bool goalsGlobal = goalsEquals || goalsReferenceEquals;
            bool initiativeGlobal = initiativeEquals || initiativeReferenceEquals;
            bool scaredGlobal = scaredEquals || scaredReferenceEquals;
            bool angryAtGlobal = angryAtEquals || angryAtReferenceEquals;
            bool wantToGoGlobal = wantsToGoEquals || wantsToGoReferenceEquals;
            bool wantToEntrapGlobal = wantToEntrapEquals || wantToEntrapReferenceEquals;
            bool talkingWithGlobal = talkingWithEquals || talkingWithReferenceEquals;
            bool skipedTurnsGlobal = skipedTurnsEquals || skipedTurnsReferenceEquals;
            bool beliefsGlobal = beliefsEquals || beliefsReferenceEquals;
            bool foundEvidenceGlobal = foundEvidenceEquals || foundEvidenceReferenceEquals;
            bool exploredRoomsGlobal = exploredRoomsEquals || exploredRoomsReferenceEquals;

            bool equal = agentInfoGlobal && statusGlobal /*&& goalsGlobal*/ && initiativeGlobal && scaredGlobal && angryAtGlobal && wantToGoGlobal
                && wantToEntrapGlobal && talkingWithGlobal && skipedTurnsGlobal && beliefsGlobal && foundEvidenceGlobal && exploredRoomsGlobal
                && complitedQuestsCounterEquals;

            return equal;
        }

        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

        /// <summary>
        /// Calculates and returns the hash code of this instance of the dynamic part of the agent's state.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            agentInfo.ClearHashCode();
            hashcode = hashcode * 42 + agentInfo.GetHashCode();
            hashcode = hashcode * 42 + alive.GetHashCode();
            hashCode = hashcode * 42 + scared.GetHashCode();
            if (angryAt != null)
            {
                angryAt.ClearHashCode();
                hashcode = hashcode * 42 + angryAt.GetHashCode();
            }
            if (wantsToGo != null)
            {
                wantsToGo.ClearHashCode();
                hashcode = hashcode * 42 + wantsToGo.GetHashCode();
            }
            if (foundEvidence != null)
            {
                foundEvidence.ClearHashCode();
                hashcode = hashcode * 42 + foundEvidence.GetHashCode();
            }
            if (exploredRooms != null)
            {
                foreach (var explRoom in exploredRooms)
                {
                    explRoom.ClearHashCode();
                    hashcode = hashcode * 42 + explRoom.GetHashCode();
                }
            }
            if (wantToEntrap != null)
            {
                wantToEntrap.ClearHashCode();
                hashcode = hashcode * 42 + wantToEntrap.GetHashCode();
            }
            if (talkingWith != null)
            {
                talkingWith.ClearHashCode();
                hashcode = hashcode * 42 + talkingWith.GetHashCode();
            }
            hashcode = hashcode + skipedTurns;

            hashcode = hashcode + complitedQuestsCounter;

            hashCode = hashcode;
            hasHashCode = true;

            return hashcode;
        }

        /// <summary>
        /// Clears the current hash code value.
        /// </summary>
        public void ClearHashCode()
        {
            hasHashCode = false;
            hashCode = 0;
        }

        /// <summary>
        /// Updates (refresh) the current hash code value.
        /// </summary>
        public void UpdateHashCode()
        {
            ClearHashCode();
            GetHashCode();
        }
    }
}