using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public enum AgentRole
    {
        USUAL,
        KILLER
    }

    public static class AgentRoleUtils
    {
        public static string GetName(AgentRole role)
        {
            return Enum.GetName(typeof(AgentRole), role);
        }

        public static AgentRole GetEnum(string name)
        {
            if (name == "usual") return AgentRole.USUAL;
            if (name == "killer") return AgentRole.KILLER;
            throw new Exception("UNRECOGNIZED AGENT ROLE: " + name);
        }
    }
}
