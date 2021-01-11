using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class AgentAngryAt : AgentProperty, ICloneable
    {
        private bool angry;
        private AgentStateStatic objectOfAngry;

        public AgentAngryAt() {}

        public AgentAngryAt(bool angry, AgentStateStatic objectOfAngry)
        {
            this.angry = angry;
            this.objectOfAngry = objectOfAngry;
        }

        public object Clone()
        {
            var clone = new AgentAngryAt();

            clone.angry = angry;
            clone.objectOfAngry = (AgentStateStatic)objectOfAngry.Clone();

            return clone;
        }

        public void AngryOn()
        {
            angry = true;
        }

        public void AngryOff()
        {
            angry = false;
        }

        public void SetObjectOfAngry(AgentStateStatic target)
        {
            objectOfAngry = target;
        }

        public AgentStateStatic GetObjectOfAngry()
        {
            return objectOfAngry;
        }

        public bool AngryCheckAtAgent(AgentStateStatic agent)
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
