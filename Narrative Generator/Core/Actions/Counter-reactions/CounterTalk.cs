using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// The class that implements the agent's action: "Counter Talk"
    /// </summary>
    [Serializable]
    class CounterTalk : PlanAction
    {
        /// <summary>
        /// An agent that is a participant in the action.
        /// </summary>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent1
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[0];
            }
        }

        /// <summary>
        /// An agent that is a participant in the action.
        /// </summary>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent2
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[1];
            }
        }

        /// <summary>
        /// The original action that the agent tried to take but was replaced by this one.
        /// </summary>
        public KeyValuePair<string, PlanAction> OriginalAction
        {
            get
            {
                return (KeyValuePair<string, PlanAction>)Arguments[2];
            }
        }

        /// <summary>
        /// A constructor based only on the state of the story world.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public CounterTalk (WorldDynamic state) { DefineDescription(state); }

        /// <summary>
        /// Constructor for the action instance.
        /// </summary>
        /// <param name="args">The arguments to be passed to the action.</param>
        public CounterTalk (params Object[] args) : base(args) { }

        /// <summary>
        /// Constructor method with parameters.
        /// </summary>
        /// <param name="agent1">An agent that is a participant in the action.</param>
        /// <param name="agent2">An agent that is a participant in the action.</param>
        /// <param name="originalAction">The original action that the agent tried to take but was replaced by this one.</param>
        public CounterTalk (ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent1,
                            ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent2,
                            string originalAction)
        {
            Arguments.Add(agent1);
            Arguments.Add(agent2);
            Arguments.Add(originalAction);
        }

        /// <summary>
        /// A method that creates a description of the action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public override void DefineDescription (WorldDynamic state)
        {
            desc = GetType().ToString().Remove(0, 20);
        }

        /// <summary>
        /// A method that checks preconditions for an action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        /// <returns>The result of the precondition check.</returns>
        public override bool CheckPreconditions(WorldDynamic state)
        {
            return Agent1.Value.GetStatus() && Agent2.Value.GetStatus() /*&& !(Agent1.Key.GetName() == Agent2.Key.GetName())*/
                   && /*(Agent1.Value.GetMyLocation().Equals(Agent2.Value.GetMyLocation()))*/
                   Agent1.Value.GetMyLocation().GetName() == Agent2.Value.GetMyLocation().GetName();
        }

        /// <summary>
        /// A method that changes the passed world state according to the effects of the action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public override void ApplyEffects (ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent1 = state.GetAgentByName(Agent1.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent2 = state.GetAgentByName(Agent2.Key.GetName());

            stateAgent1.Value.ClearTempStates();
            stateAgent2.Value.ClearTempStates();

            stateAgent1.Value.SetInterlocutor(stateAgent2);
            stateAgent2.Value.SetInterlocutor(stateAgent1);

            if (stateAgent1.Value.GetObjectOfAngryComponent().AngryCheck() && stateAgent1.Value.GetEvidenceStatus().CheckEvidence() &&
                stateAgent2.Key.GetRole() != AgentRole.ANTAGONIST)
            {
                stateAgent2.Value.AddEvidence(stateAgent1.Value.GetObjectOfAngryComponent().GetObjectOfAngry());
                stateAgent2.Value.GetBeliefs().GetAgentByName(
                    stateAgent1.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName()).AssignRole(AgentRole.ANTAGONIST);
                stateAgent2.Value.SetObjectOfAngry(stateAgent1.Value.GetObjectOfAngryComponent());
            }
            else if (stateAgent2.Value.GetObjectOfAngryComponent().AngryCheck() && stateAgent2.Value.GetEvidenceStatus().CheckEvidence() &&
                     stateAgent1.Key.GetRole() != AgentRole.ANTAGONIST)
            {
                stateAgent1.Value.AddEvidence(stateAgent2.Value.GetObjectOfAngryComponent().GetObjectOfAngry());
                stateAgent1.Value.GetBeliefs().GetAgentByName(
                    stateAgent2.Value.GetObjectOfAngryComponent().GetObjectOfAngry().GetName()).AssignRole(AgentRole.ANTAGONIST);
                stateAgent1.Value.SetObjectOfAngry(stateAgent2.Value.GetObjectOfAngryComponent());
            }

            stateAgent1.Value.DecreaseTimeToMove();
        }

        /// <summary>
        /// A method that implements the action's failure effect.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public override void Fail (ref WorldDynamic state) { fail = true; }

        /// <summary>
        /// In counter-actions, returns the name of the action that caused the system to react.
        /// </summary>
        /// <returns>A couple from the action and separately its name.</returns>
        public override KeyValuePair<string, PlanAction> ReturnOriginal()
        {
            return OriginalAction;
        }
    }
}
