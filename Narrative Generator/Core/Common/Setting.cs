using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    public enum Setting
    {
        Fantasy,
        GenericFantasy,
        Detective,
        DefaultDemo,
    }

    public static class SettingUtils
    {
        public static string GetName (Setting setting)
        {
            return Enum.GetName(typeof(Setting), setting);
        }

        public static Setting GetEnum (string settingName)
        {
            if (settingName == "fantasy") return Setting.Fantasy;
            if (settingName == "datective") return Setting.Detective;
            if (settingName == "defaultDemo") return Setting.DefaultDemo;
            throw new Exception("UNRECOGNIZED SETTING NAME: " + settingName);
        }
    }
}
