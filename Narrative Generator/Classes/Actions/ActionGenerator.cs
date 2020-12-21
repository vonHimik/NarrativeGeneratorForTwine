using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class ActionGenerator
    {
        public List<PlanAction> GetPossibleActions (Agent agent)
        {
            List<PlanAction> result = new List<PlanAction>();
            Entrap.GetPossibleActions(agent, result);
            Fight.GetPossibleActions(agent, result);
            InvestigateRoom.GetPossibleActions(agent, result);
            Move.GetPossibleActions(agent, result);
            Kill.GetPossibleActions(agent, result);
            NeutralizeKiller.GetPossibleActions(agent, result);
            NothingToDo.GetPossibleActions(agent, result);
            Reassure.GetPossibleActions(agent, result);
            Run.GetPossibleActions(agent, result);
            TellAboutASuspicious.GetPossibleActions(agent, result);
            return result;
        }
    }
}
