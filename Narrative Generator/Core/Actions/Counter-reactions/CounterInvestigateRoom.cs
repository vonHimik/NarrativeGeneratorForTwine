﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// The class that implements the agent's action: "Counter Investigate Room"
    /// </summary>
    [Serializable]
    class CounterInvestigateRoom : PlanAction
    {
        /// <summary>
        /// Agent which applies the action.
        /// </summary>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[0];
            }
        }

        /// <summary>
        /// Agent who is the target of the action.
        /// </summary>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Killer
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[1];
            }
        }

        /// <summary>
        /// The location that is the target of the action.
        /// </summary>
        public KeyValuePair<LocationStatic, LocationDynamic> Location
        {
            get
            {
                return (KeyValuePair<LocationStatic, LocationDynamic>)Arguments[2];
            }
        }

        /// <summary>
        /// The original action that the agent tried to take but was replaced by this one.
        /// </summary>
        public KeyValuePair<string, PlanAction> OriginalAction
        {
            get
            {
                return (KeyValuePair<string, PlanAction>)Arguments[3];
            }
        }

        /// <summary>
        /// A constructor based only on the state of the story world.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public CounterInvestigateRoom (WorldDynamic state) { DefineDescription(state); }

        /// <summary>
        /// Constructor for the action instance.
        /// </summary>
        /// <param name="args">The arguments to be passed to the action.</param>
        public CounterInvestigateRoom(params Object[] args) : base(args) { }

        /// <summary>
        /// Constructor method with parameters.
        /// </summary>
        /// <param name="agent">Agent which applies the action.</param>
        /// <param name="killer">Agent who is the target of the action.</param>
        /// <param name="location">The location that is the target of the action.</param>
        /// <param name="originalAction">The original action that the agent tried to take but was replaced by this one.</param>
        public CounterInvestigateRoom(ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                                      ref KeyValuePair<AgentStateStatic, AgentStateDynamic> killer,
                                      ref KeyValuePair<LocationStatic, LocationDynamic> location,
                                      string originalAction)
        {
            Arguments.Add(agent);
            Arguments.Add(killer);
            Arguments.Add(location);
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
            return Agent.Key.GetRole() == AgentRole.USUAL && Agent.Value.GetStatus()
                      && Killer.Key.GetRole() == AgentRole.ANTAGONIST && Killer.Value.GetStatus()
                      && Location.Value.SearchAgent(Agent.Key)
                      && !Agent.Value.CheckIfLocationIsExplored(Location.Key) && Location.Value.CheckEvidence();
        }

        /// <summary>
        /// A method that changes the passed world state according to the effects of the action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public override void ApplyEffects(ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgentClone = state.GetLocationByName(Location.Key.GetName()).Value.GetAgent(Agent);
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateKiller = state.GetAgentByName(Killer.Key.GetName());
            KeyValuePair<LocationStatic, LocationDynamic> stateLocation = state.GetLocationByName(Location.Key.GetName());

            stateAgent.Value.ClearTempStates();
            stateKiller.Value.ClearTempStates();

            stateAgent.Value.AddEvidence(stateKiller.Key);
            stateAgent.Value.GetBeliefs().GetAgentByName(stateKiller.Key.GetName()).AssignRole(AgentRole.ANTAGONIST);
            stateAgent.Value.SetObjectOfAngry(stateKiller.Key);
            stateAgent.Value.AddExploredLocation(stateLocation.Key);

            stateAgent.Value.DecreaseTimeToMove();
        }

        /// <summary>
        /// A method that implements the action's failure effect.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public override void Fail(ref WorldDynamic state)
        {
            fail = true;

            //KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent.Key.GetName());
            //KeyValuePair<LocationStatic, LocationDynamic> stateLocation = state.GetLocationByName(Location.Key.GetName());
            //stateAgent.Value.AddExploredLocation(stateLocation.Key);
        }

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

