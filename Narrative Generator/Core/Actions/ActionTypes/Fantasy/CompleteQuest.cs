﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// The class that implements the agent's action: "Complete Quest"
    /// </summary>
    [Serializable]
    class CompleteQuest : PlanAction
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
        /// The location where the action was applied.
        /// </summary>
        public KeyValuePair<LocationStatic, LocationDynamic> Location
        {
            get
            {
                return (KeyValuePair<LocationStatic, LocationDynamic>)Arguments[1];
            }
        }

        /// <summary>
        /// A constructor based only on the state of the story world.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public CompleteQuest (WorldDynamic state) { DefineDescription(state); }

        /// <summary>
        /// Constructor for the action instance.
        /// </summary>
        /// <param name="args">The arguments to be passed to the action.</param>
        public CompleteQuest (params Object[] args) : base(args) { }

        /// <summary>
        /// Constructor method with parameters.
        /// </summary>
        /// <param name="agent">Agent which applies the action.</param>
        /// <param name="location">The location where the action was applied.</param>
        public CompleteQuest (ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                              ref KeyValuePair<LocationStatic, LocationDynamic> location)
        {
            Arguments.Add(agent);
            Arguments.Add(location);
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
        /// A method that checks the most basic preconditions for an action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        /// <param name="agent">Agent which applies the action.</param>
        /// <returns>The result of the precondition check.</returns>
        public bool PreCheckPrecondition (WorldDynamic state, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            return state.GetStaticWorldPart().GetSetting().Equals(Setting.GenericFantasy) && agent.Key.GetRole().Equals(AgentRole.PLAYER)
                && agent.Value.GetStatus() && state.GetLocationByName(state.SearchAgentAmongLocations(agent.Key).GetName()).Value.SearchAgent(agent.Key);
        }

        /// <summary>
        /// A method that checks preconditions for an action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        /// <returns>The result of the precondition check.</returns>
        public override bool CheckPreconditions (WorldDynamic state)
        {
            return state.GetStaticWorldPart().GetSetting().Equals(Setting.GenericFantasy) && Agent.Key.GetRole().Equals(AgentRole.PLAYER)
                && Agent.Value.GetStatus() && Location.Value.SearchAgent(Agent.Key);
        }

        /// <summary>
        /// A method that changes the passed world state according to the effects of the action.
        /// </summary>
        /// <param name="state">Considered world state.</param>
        public override void ApplyEffects (ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent.Key.GetName());

            ItemsManager itemsManager = new ItemsManager();

            if (state.GetStaticWorldPart().GetSetting().Equals(Setting.GenericFantasy))
            {
                if (stateAgent.Value.GetItems().Count == 0)
                {
                    stateAgent.Value.AddItem(ItemsManager.CreateItem("Weapon", ItemsTypes.WEAPON));
                }
                else if (stateAgent.Value.GetItems().Count == 1 
                    && stateAgent.Value.GetItems().First().GetItemType().Equals(ItemsTypes.WEAPON))
                {
                    stateAgent.Value.AddItem(ItemsManager.CreateItem("Armor", ItemsTypes.ARMOR));
                }
            }

            stateAgent.Value.CompleteQuest();
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
            throw new NotImplementedException();
        }
    }
}
