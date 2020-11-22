using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Narrative_Generator
{
    class FastDownward
    {
        Wrapper wrapper;
        public bool isSuccess;

        public FastDownward()
        {
            wrapper = new Wrapper();
            isSuccess = false;
        }

        public void Run()
        {
            wrapper.RunFastDownward("Detective-Domain.pddl", "Detective-Problem.pddl");
            isSuccess = true;
        }

        public Plan GetResultPlan()
        {
            Plan readedPlan = null;

            Action action = new Action();

            using (StreamReader streamReader = new StreamReader("sas_plan.txt", Encoding.Default))
            {
                string word = null;
                bool startLineReading = false;
                bool haveName = false;

                while (!streamReader.EndOfStream)
                {
                    char c = (char)streamReader.Read();

                    if (c == '(') { startLineReading = true; }
                    else if (c == ')')
                    {
                        startLineReading = false;
                        haveName = false;

                        action.AddParameter(word);
                        word = null;

                        readedPlan.AddAction(action);
                        action.Clear();
                    }
                    else if (c == ';') { break; }
                    else if (c== ' ')
                    {
                        if (startLineReading && haveName)
                        {
                            action.AddParameter(word);
                            word = null;
                        }
                        else if (startLineReading && !haveName)
                        {
                            action.SetName(word);
                            word = null;
                            haveName = true;
                        }
                    }
                    else
                    {
                        if (startLineReading)
                        {
                            word.Insert(word.Length, c.ToString());
                        }
                    }
                }
            }

            return readedPlan;
        }
    }
}
