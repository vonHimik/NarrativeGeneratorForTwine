using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    class CounterInvestigateRoom : PlanAction
    {
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Agent
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[0];
            }
        }

        public KeyValuePair<AgentStateStatic, AgentStateDynamic> Killer
        {
            get
            {
                return (KeyValuePair<AgentStateStatic, AgentStateDynamic>)Arguments[1];
            }
        }

        public KeyValuePair<LocationStatic, LocationDynamic> Location
        {
            get
            {
                return (KeyValuePair<LocationStatic, LocationDynamic>)Arguments[2];
            }
        }

        public string OriginalAction
        {
            get
            {
                return (string)Arguments[3];
            }
        }

        public CounterInvestigateRoom (WorldDynamic state) { DefineDescription(state); }

        public CounterInvestigateRoom(params Object[] args) : base(args) { }

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

        public override void DefineDescription (WorldDynamic state)
        {
            desc = GetType().ToString().Remove(0, 20);
        }

        public override bool CheckPreconditions(WorldDynamic state)
        {
            return Agent.Key.GetRole() == AgentRole.USUAL && Agent.Value.GetStatus()
                      && Killer.Key.GetRole() == AgentRole.ANTAGONIST && Killer.Value.GetStatus()
                      && Location.Value.SearchAgent(Agent.Key)
                      && !Agent.Value.CheckIfLocationIsExplored(Location.Key) && Location.Value.CheckEvidence();
        }

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

        public override void Fail(ref WorldDynamic state)
        {
            fail = true;

            //KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent.Key.GetName());
            //KeyValuePair<LocationStatic, LocationDynamic> stateLocation = state.GetLocationByName(Location.Key.GetName());
            //stateAgent.Value.AddExploredLocation(stateLocation.Key);
        }
    }
}

