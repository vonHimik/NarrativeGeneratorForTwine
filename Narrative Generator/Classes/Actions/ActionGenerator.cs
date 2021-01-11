using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class ActionGenerator
    {
        public static readonly ActionGenerator INSTANCE = new ActionGenerator();

        public List<PlanAction> GetPossibleActions(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            List<PlanAction> result = new List<PlanAction>();

            if (agent.Value.GetStatus())
            {
                Move move = new Move();
                result.Add(move);

                NothingToDo nothingToDo = new NothingToDo();
                result.Add(nothingToDo);

                if (agent.Value.ThinksThatSomeoneIsAngry())
                {
                    Reassure reassure = new Reassure();
                    result.Add(reassure);
                }

                if (agent.Value.CheckScared())
                {
                    Run run = new Run();
                    result.Add(run);
                }

                if (agent.Key.GetRole() == AgentRole.KILLER)
                {
                    Kill kill = new Kill();
                    result.Add(kill);

                    Entrap entrap = new Entrap();
                    result.Add(entrap);

                    TellAboutASuspicious tellAboutASuspicious = new TellAboutASuspicious();
                    result.Add(tellAboutASuspicious);
                }

                if (agent.Key.GetRole() == AgentRole.USUAL)
                {
                    if (agent.Value.GetObjectOfAngry().AngryCheck())
                    {
                        Fight fight = new Fight();
                        result.Add(fight);

                        if (agent.Value.GetEvidenceStatus().CheckEvidence())
                        {
                            NeutralizeKiller neutralizeKiller = new NeutralizeKiller();
                            result.Add(neutralizeKiller);
                        }
                    }

                    if (!agent.Value.SearchAmongExploredLocations(agent.Value.GetBeliefs().SearchAgentAmongLocations(agent.Key)))
                    {
                        InvestigateRoom investigateRoom = new InvestigateRoom();
                        result.Add(investigateRoom);
                    }
                }
            }

            return result;
        }
    }
}