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

            using (StreamReader streamReader = new StreamReader("sas_plan.txt", Encoding.Default))
            {
                string line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    
                }
            }

            return readedPlan;
        }
    }
}
