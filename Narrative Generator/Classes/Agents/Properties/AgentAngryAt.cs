using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class AgentAngryAt : AgentProperty
    {
        private bool angry;
        private Agent objectOfAngry;

        public AgentAngryAt(bool angry, Agent objectOfAngry)
        {
            this.angry = angry;
            this.objectOfAngry = objectOfAngry;
        }

        public void AngryOn()
        {
            angry = true;
        }

        public void AngryOff()
        {
            angry = false;
        }

        public void SetObjectOfAngry(Agent target)
        {
            objectOfAngry = target;
        }

        public Agent GetObjectOfAngry()
        {
            return objectOfAngry;
        }

        public bool AngryCheckAtAgent(Agent agent)
        {
            if (angry && agent == objectOfAngry)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AngryCheck()
        {
            return angry;
        }
    }
}
