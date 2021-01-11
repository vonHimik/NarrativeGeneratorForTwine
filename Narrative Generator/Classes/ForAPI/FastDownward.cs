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

        public void Run(string domainFileName, string problemFileName)
        {
            wrapper.RunFastDownward(domainFileName + ".pddl", problemFileName + ".pddl");
            isSuccess = true;
        }

        public Plan GetResultPlan()
        {
            Plan readedPlan = null;
            string actionName = null;

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

                        //action.AddParameter(word);
                        word = null;

                        readedPlan.AddAction(actionName);
                        actionName = null; ;
                    }
                    else if (c == ';') { break; }
                    else if (c== ' ')
                    {
                        if (startLineReading && haveName)
                        {
                            //action.AddParameter(word);
                            word = null;
                        }
                        else if (startLineReading && !haveName)
                        {
                            actionName = word;
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
