using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    class Plan
    {
        private List<PlanAction> actions;

        public void AddAction(string actionName)
        {
            if (actionName.Contains("move"))
            {
                Move move = new Move();
                actions.Add(move);
            }
            if (actionName.Contains("kill"))
            {
                Kill kill = new Kill();
                actions.Add(kill);
            }
            if (actionName == "entrap")
            {
                Entrap entrap = new Entrap();
                actions.Add(entrap);
            }
            if (actionName == "fight")
            {
                Fight fight = new Fight();
                actions.Add(fight);
            }
            if (actionName == "investigateroom")
            {
                InvestigateRoom investigateRoom = new InvestigateRoom();
                actions.Add(investigateRoom);
            }
            if (actionName == "neutralizekiller")
            {
                NeutralizeKiller neutralizeKiller = new NeutralizeKiller();
                actions.Add(neutralizeKiller);
            }
            if (actionName == "nothingtodo")
            {
                NothingToDo nothingToDo = new NothingToDo();
                actions.Add(nothingToDo);
            }
            if (actionName == "reassure")
            {
                Reassure reassure = new Reassure();
                actions.Add(reassure);
            }
            if (actionName == "run")
            {
                Run run = new Run();
                actions.Add(run);
            }
            if (actionName == "tellaboutasuspicious")
            {
                TellAboutASuspicious tellAboutASuspicious = new TellAboutASuspicious();
                actions.Add(tellAboutASuspicious);
            }
        }

        public PlanAction GetAction(int index)
        {
            return actions[index];
        }
    }
}
