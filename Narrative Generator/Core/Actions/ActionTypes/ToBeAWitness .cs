using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class ToBeAWitness : PlanAction
    {
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[0];
            }
        }

        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Victim
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[1];
            }
        }

        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Killer
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[2];
            }
        }

        public KeyValuePair<LocationStatic, LocationDynamic> Location
        {
            get
            {
                return (KeyValuePair<LocationStatic, LocationDynamic>)Arguments[3];
            }
        }

        public ToBeAWitness (WorldDynamic state) { DefineDescription(state); }

        public ToBeAWitness (params Object[] args) : base(args) { }

        public ToBeAWitness (ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent,
                             ref KeyValuePair<AgentStateStatic, AgentStateDynamic> victim,
                             ref KeyValuePair<AgentStateStatic, AgentStateDynamic> killer,
                             ref KeyValuePair<LocationStatic, LocationDynamic> location)
        {
            Arguments.Add(agent);
            Arguments.Add(victim);
            Arguments.Add(killer);
            Arguments.Add(location);
        }

        public override void DefineDescription (WorldDynamic state)
        {
            desc = GetType().ToString().Remove(0, 20);
        }

        public bool PreCheckPrecondition(WorldDynamic state, KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            return agent.Key.GetRole() == AgentRole.USUAL && agent.Value.GetStatus() 
                      && state.GetLocation(state.SearchAgentAmongLocations(agent.Key)).Value.CountDeadAgents() >= 1
                      && state.GetLocation(state.SearchAgentAmongLocations(agent.Key)).
                            Equals(state.GetLocation(state.SearchAgentAmongLocations(state.GetAgentByRole(AgentRole.ANTAGONIST).Key)));
        }

        public override bool CheckPreconditions(WorldDynamic state)
        {
            return Agent.Key.GetRole() == AgentRole.USUAL && Agent.Value.GetStatus()
                      && (Victim.Key.GetRole() == AgentRole.USUAL || Victim.Key.GetRole() == AgentRole.PLAYER) &&!Victim.Value.GetStatus()
                      && !Agent.Equals(Victim)
                      && Killer.Key.GetRole() == AgentRole.ANTAGONIST && Killer.Value.GetStatus()
                      && Location.Value.SearchAgent(Agent.Key) && Location.Value.SearchAgent(Victim.Key) && Location.Value.SearchAgent(Killer.Key);
        }

        public override void ApplyEffects(ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateVictim = state.GetAgentByName(Victim.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateKiller = state.GetAgentByName(Killer.Key.GetName());
            KeyValuePair<LocationStatic, LocationDynamic> stateLocation = state.GetLocationByName(Location.Key.GetName());

            stateAgent.Value.ClearTempStates();

            stateAgent.Value.AddEvidence(stateKiller.Key);
            stateAgent.Value.GetBeliefs().GetAgentByName(stateKiller.Key.GetName()).AssignRole(AgentRole.ANTAGONIST);
            stateAgent.Value.SetObjectOfAngry(stateKiller.Key);

            stateAgent.Value.DecreaseTimeToMove();
        }

        public override void Fail(ref WorldDynamic state) { fail = true; }
    }
}
