using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    public class AgentAngryAt : AgentProperty, ICloneable, IEquatable<AgentAngryAt>
    {
        private bool angry;
        private AgentStateStatic objectOfAngry;

        private bool hasHashCode;
        private int hashCode;

        public AgentAngryAt()
        {
            angry = false;
            objectOfAngry = new AgentStateStatic();
            hasHashCode = false;
            hashCode = 0;
        }

        public AgentAngryAt (AgentAngryAt clone)
        {
            angry = clone.angry;
            if (clone.objectOfAngry != null) { objectOfAngry = (AgentStateStatic)clone.objectOfAngry.Clone(); }
            else { objectOfAngry = new AgentStateStatic(); }
            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
        }

        public AgentAngryAt(bool angry, AgentStateStatic objectOfAngry)
        {
            this.angry = angry;
            this.objectOfAngry = objectOfAngry;
            hasHashCode = false;
            hashCode = 0;
        }

        public object Clone()
        {
            var clone = new AgentAngryAt();

            clone.angry = angry;
            if (objectOfAngry != null) { clone.objectOfAngry = new AgentStateStatic(objectOfAngry); }

            return clone;
        }

        public void AngryOn()
        {
            angry = true;
            UpdateHashCode();
        }

        public void AngryOff()
        {
            angry = false;
            UpdateHashCode();
        }

        public void SetObjectOfAngry(AgentStateStatic target)
        {
            objectOfAngry = target;
            UpdateHashCode();
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

        public bool Equals(AgentAngryAt anotherAngryAt)
        {
            if (anotherAngryAt == null) { return false; }

            bool angryEquals = (angry == anotherAngryAt.angry);
            bool angryReferenceEquals = object.ReferenceEquals(angry, anotherAngryAt.angry);

            bool objectOfAngryEquals;
            bool objectOfAngryReferenceEquals;
            if (objectOfAngry == null && anotherAngryAt.objectOfAngry == null)
            {
                objectOfAngryEquals = true;
                objectOfAngryReferenceEquals = true;
            }
            else
            {
                objectOfAngryEquals = objectOfAngry.Equals(anotherAngryAt.objectOfAngry);
                objectOfAngryReferenceEquals = object.ReferenceEquals(objectOfAngry, anotherAngryAt.objectOfAngry);
            }

            bool angryGlobal = angryEquals || angryReferenceEquals;
            bool objectOfAngryGlobal = objectOfAngryEquals || objectOfAngryReferenceEquals;

            bool equal = angryGlobal && objectOfAngryGlobal;

            return equal;
        }

        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            hashcode = hashcode * 42 + angry.GetHashCode();
            if (objectOfAngry != null)
            {
                objectOfAngry.ClearHashCode();
                hashcode = hashcode * 42 + objectOfAngry.GetHashCode();
            }

            hashCode = hashcode;
            hasHashCode = true;

            return hashcode;
        }

        public void ClearHashCode()
        {
            hasHashCode = false;
            hashCode = 0;
        }

        public void UpdateHashCode()
        {
            ClearHashCode();
            GetHashCode();
        }
    }
}
