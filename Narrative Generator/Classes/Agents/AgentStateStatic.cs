using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public class AgentStateStatic : ICloneable
    {
        private string name;
        private AgentRole role;

        public AgentStateStatic() {}

        public AgentStateStatic(string name, AgentRole role)
        {
            SetName(name);
            AssignRole(role);
        }

        public object Clone()
        {
            var clone = new AgentStateStatic();

            clone.name = name;
            clone.role = role;

            return clone;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public string GetName()
        {
            return name;
        }

        public void AssignRole(AgentRole role)
        {
            this.role = role;
        }

        public void AssignRole(string role)
        {
            this.role = AgentRoleUtils.GetEnum(role);
        }

        public AgentRole GetRole()
        {
            return role;
        }
    }
}
