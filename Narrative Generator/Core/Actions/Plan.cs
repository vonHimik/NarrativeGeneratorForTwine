using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    public class Plan : ICloneable
    {
        private List<PlanAction> actions;
        public bool planReceived;

        public Plan()
        {
            actions = new List<PlanAction>();
            planReceived = false;
        }

        public Plan (Plan clone)
        {
            actions = new List<PlanAction>(clone.actions);
            planReceived = clone.planReceived;
        }

        public object Clone()
        {
            var clone = new Plan();
            clone.actions = new List<PlanAction>(actions);
            clone.planReceived = planReceived;
            return clone;
        }

        public void Clear()
        {
            actions.Clear();
            planReceived = false;
        }

        public void AddAction(string actionName, List<string> parameters)
        {
            if (actionName.Contains("killer_move"))
            {
                Move move = new Move();
                
                foreach (var parameter in parameters)
                {
                    move.Arguments.Add(parameter);
                }

                actions.Add(move);
            }
            if (actionName.Contains("move") && !actionName.Contains("killer"))
            {
                Move move = new Move();
                actions.Add(move);
            }
            if (actionName.Contains("kill") && !actionName.Contains("move") && !actionName.Contains("neutralize"))
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
            if (actionName == "investigate-room")
            {
                InvestigateRoom investigateRoom = new InvestigateRoom();
                actions.Add(investigateRoom);
            }
            if (actionName == "neutralize-killer")
            {
                NeutralizeKiller neutralizeKiller = new NeutralizeKiller();
                actions.Add(neutralizeKiller);
            }
            if (actionName == "nothing-to-do")
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
            if (actionName == "tell-about-killer")
            {
                TellAboutASuspicious tellAboutASuspicious = new TellAboutASuspicious();
                actions.Add(tellAboutASuspicious);
            }
            if (actionName == "talk")
            {
                Talk talk = new Talk();
                actions.Add(talk);
            }
            if (actionName == "to-be-a-witness")
            {
                ToBeAWitness toBeAWitness = new ToBeAWitness();
                actions.Add(toBeAWitness);
            }
            if (actionName == "help-mages")
            {
                HelpMages helpMages = new HelpMages();
                actions.Add(helpMages);
            }
            if (actionName == "help-templars")
            {
                HelpTemplars helpTemplars = new HelpTemplars();
                actions.Add(helpTemplars);
            }
            if (actionName == "help-elfs")
            {
                HelpElfs helpElfs = new HelpElfs();
                actions.Add(helpElfs);
            }
            if (actionName == "help-werewolves")
            {
                HelpWerewolves helpWerewolves = new HelpWerewolves();
                actions.Add(helpWerewolves);
            }
            if (actionName == "help-prinec")
            {
                HelpPrinceBelen helpPrinceBelen = new HelpPrinceBelen();
                actions.Add(helpPrinceBelen);
            }
            if (actionName == "help-lord")
            {
                HelpLordHarrowmont helpLordHarrowmont = new HelpLordHarrowmont();
                actions.Add(helpLordHarrowmont);
            }
        }

        public PlanAction GetAction(int index)
        {
            return actions[index];
        }
    }
}
