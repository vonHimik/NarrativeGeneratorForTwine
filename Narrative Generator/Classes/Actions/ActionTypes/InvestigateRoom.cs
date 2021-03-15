using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class InvestigateRoom : PlanAction
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

        public InvestigateRoom(params Object[] args) : base(args) { }

        public InvestigateRoom(ref KeyValuePair<AgentStateStatic, AgentStateDynamic> agent, 
                               ref KeyValuePair<AgentStateStatic, AgentStateDynamic> killer, 
                               ref KeyValuePair<LocationStatic, LocationDynamic> location)
        {
            Arguments.Add(agent);
            Arguments.Add(killer);
            Arguments.Add(location);
        }

        public override bool CheckPreconditions(WorldDynamic state)
        {
            return Agent.Key.GetRole() == AgentRole.USUAL && Agent.Value.GetStatus() 
                      && Killer.Key.GetRole() == AgentRole.KILLER && Killer.Value.GetStatus()
                      && Location.Value.SearchAgent(Agent.Key) 
                      && !Agent.Value.SearchAmongExploredLocations(Location.Key) && Location.Value.CheckEvidence();
        }

        public override void ApplyEffects(ref WorldDynamic state)
        {
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateAgent = state.GetAgentByName(Agent.Key.GetName());
            KeyValuePair<AgentStateStatic, AgentStateDynamic> stateKiller = state.GetAgentByName(Killer.Key.GetName());
            KeyValuePair<LocationStatic, LocationDynamic> stateLocation = state.GetLocationByName(Location.Key.GetName());

            stateAgent.Value.AddEvidence(stateKiller.Key);
            stateAgent.Value.GetBeliefs().GetAgentByName(stateKiller.Key.GetName()).Key.AssignRole(AgentRole.KILLER);
            stateAgent.Value.SetObjectOfAngry(stateKiller.Key);
            stateAgent.Value.AddExploredLocation(stateLocation.Key);
        }
    }
}
