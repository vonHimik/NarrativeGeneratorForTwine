using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    [Serializable]
    public class WantToEntrap : AgentProperty, ICloneable, IEquatable<WantToEntrap>
    {
        private bool entraping;
        private AgentStateStatic whom;
        private LocationStatic where;

        private bool hasHashCode;
        private int hashCode;

        public WantToEntrap()
        {
            entraping = false;
            whom = new AgentStateStatic();
            where = new LocationStatic();
            hasHashCode = false;
            hashCode = 0;
        }

        public WantToEntrap (WantToEntrap clone)
        {
            entraping = clone.entraping;
            if (clone.whom != null) { whom = (AgentStateStatic)clone.whom.Clone(); }
            else { whom = new AgentStateStatic(); }
            if (clone.where != null) { where = (LocationStatic)clone.where.Clone(); }
            else { where = new LocationStatic(); }
            hasHashCode = clone.hasHashCode;
            hashCode = clone.hashCode;
        }

        public WantToEntrap (bool entraping, AgentStateStatic whom, LocationStatic where)
        {
            this.entraping = entraping;
            this.whom = whom;
            this.where = where;
            hasHashCode = false;
            hashCode = 0;
        }

        public object Clone()
        {
            var clone = new WantToEntrap();

            clone.entraping = entraping;
            if (whom != null) { clone.whom = new AgentStateStatic(whom); }
            if (where != null) { clone.where = new LocationStatic(where); }

            return clone;
        }

        public bool Equals (WantToEntrap anotherWantToEntrap)
        {
            if (anotherWantToEntrap == null) { return false; }

            bool entrapingEquals = (entraping == anotherWantToEntrap.entraping);
            bool entrapingReferenceEquals = object.ReferenceEquals(entraping, anotherWantToEntrap.entraping);

            bool whomEquals;
            bool whomReferenceEquals;
            if (whom == null && anotherWantToEntrap.whom == null)
            {
                whomEquals = true;
                whomReferenceEquals = true;
            }
            else if ((whom == null && anotherWantToEntrap.whom != null) || (whom != null && anotherWantToEntrap.whom == null))
            {
                whomEquals = false;
                whomReferenceEquals = false;
            }
            else
            {
                whomEquals = whom.Equals(anotherWantToEntrap.whom);
                whomReferenceEquals = object.ReferenceEquals(whom, anotherWantToEntrap.whom);
            }

            bool whereEquals;
            bool whereReferenceEquals;
            if (where == null && anotherWantToEntrap.where == null)
            {
                whereEquals = true;
                whereReferenceEquals = true;
            }
            else if ((where == null && anotherWantToEntrap.where != null) || (where != null && anotherWantToEntrap.where == null))
            {
                whereEquals = false;
                whereReferenceEquals = false;
            }
            else
            {
                whereEquals = where.Equals(anotherWantToEntrap.where);
                whereReferenceEquals = object.ReferenceEquals(where, anotherWantToEntrap.where);
            }

            bool entrapingGlobal = entrapingEquals || entrapingReferenceEquals;
            bool whomGlobal = whomEquals || whomReferenceEquals;
            bool whereGlobal = whereEquals || whereReferenceEquals;

            bool equal = entrapingGlobal && whomGlobal && whereGlobal;

            return equal;
        }

        public void EntrapingStart()
        {
            entraping = true;
            UpdateHashCode();
        }

        public void EntrapingEnd()
        {
            entraping = false;
            UpdateHashCode();
        }

        public void SetVictim (AgentStateStatic victim)
        {
            whom = victim;
            UpdateHashCode();
        }

        public AgentStateStatic GetVictim() { return whom; }

        public void SetLocation (LocationStatic location)
        {
            where = location;
            UpdateHashCode();
        }

        public LocationStatic GetLocation() { return where; }

        public void SetVictimAndLocation (AgentStateStatic victim, LocationStatic location)
        {
            SetVictim(victim);
            SetLocation(location);
        }

        public bool EntrapCheck() { return entraping; }

        public bool EntrapingCheckAtAgent (AgentStateStatic agent)
        {
            if (entraping && agent == whom) { return true; }
            else { return false; }
        }

        public bool EntrapingCheckAtLocation (LocationStatic location)
        {
            if (entraping && location == where) { return true; }
            else { return false; }
        }

        //////////////////////
        /* HASHCODE SECTION */
        //////////////////////

        public override int GetHashCode()
        {
            if (hasHashCode && hashCode != 0) { return hashCode; }

            int hashcode = 18;

            hashcode = hashcode * 42 + entraping.GetHashCode();
            if (whom != null)
            {
                whom.ClearHashCode();
                hashcode = hashcode * 42 + whom.GetHashCode();
            }
            if (where != null)
            {
                where.ClearHashCode();
                hashcode = hashcode * 42 + where.GetHashCode();
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
