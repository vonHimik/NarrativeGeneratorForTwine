using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    public class TalkingWith : AgentProperty, ICloneable, IEquatable<TalkingWith>
    {
        private bool talking;
        private AgentStateStatic interlocutor;

        private bool hasHashCode;
        private int hashCode;

        public TalkingWith()
        {
            talking = false;
            interlocutor = new AgentStateStatic();
            hasHashCode = false;
            hashCode = 0;
        }

        public TalkingWith (TalkingWith clone)
        {
            talking = clone.talking;
            if (clone.interlocutor != null) { interlocutor = (AgentStateStatic)clone.interlocutor.Clone(); }
            else { interlocutor = new AgentStateStatic(); }
            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
        }

        public TalkingWith (bool talking, AgentStateStatic interlocutor)
        {
            this.talking = talking;
            this.interlocutor = interlocutor;
            hasHashCode = false;
            hashCode = 0;
        }

        public object Clone()
        {
            var clone = new TalkingWith();

            clone.talking = talking;
            if (interlocutor != null) { clone.interlocutor = new AgentStateStatic(interlocutor); }

            return clone;
        }

        public bool Equals (TalkingWith anotherTalkingWith)
        {
            if (anotherTalkingWith == null) { return false; }

            bool talkingEquals = (talking == anotherTalkingWith.talking);
            bool talkingReferenceEquals = object.ReferenceEquals(talking, anotherTalkingWith.talking);

            bool interlocutorEquals;
            bool interlocutorReferenceEquals;
            if (interlocutor == null && anotherTalkingWith.interlocutor == null)
            {
                interlocutorEquals = true;
                interlocutorReferenceEquals = true;
            }
            else
            {
                interlocutorEquals = interlocutor.Equals(anotherTalkingWith.interlocutor);
                interlocutorReferenceEquals = object.ReferenceEquals(interlocutor, anotherTalkingWith.interlocutor);
            }

            bool talkingGlobal = talkingEquals || talkingReferenceEquals;
            bool interlocutorGlobal = interlocutorEquals || interlocutorReferenceEquals;

            bool equal = talkingGlobal && interlocutorGlobal;

            return equal;
        }

        public void TalkingStart()
        {
            talking = true;
            UpdateHashCode();
        }

        public void TalkingEnd()
        {
            talking = false;
            UpdateHashCode();
        }

        public void SetInterlocutor (AgentStateStatic interlocutor)
        {
            this.interlocutor = interlocutor;
            UpdateHashCode();
        }

        public AgentStateStatic GetInterlocutor()
        {
            return interlocutor;
        }

        public bool TalkingCheck()
        {
            return talking;
        }

        public void Clear()
        {
            this.interlocutor = null;
            UpdateHashCode();
        }

        public bool TalkingCheckAtAgent (AgentStateStatic agent)
        {
            if (talking && agent == interlocutor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            hashcode = hashcode * 42 + talking.GetHashCode();
            if (interlocutor != null)
            {
                interlocutor.ClearHashCode();
                hashcode = hashcode * 42 + interlocutor.GetHashCode();
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
