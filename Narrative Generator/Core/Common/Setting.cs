using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// An enumerator to define the setting of the generated story.
    /// </summary>
    public enum Setting
    {
        /// <summary>
        /// The setting of the Dragon Age fantasy universe.
        /// </summary>
        DragonAge,
        /// <summary>
        /// The setting embodies the standard attributes of the fantasy genre, but not any specific one.
        /// </summary>
        GenericFantasy,
        /// <summary>
        /// The setting of a detective story in the spirit of Agatha Christie's story "Ten Little Indians".
        /// </summary>
        Detective,
        /// <summary>
        /// Service setting for debugging.
        /// </summary>
        DefaultDemo,
    }

    /// <summary>
    /// A class that facilitates interaction with settings and their use.
    /// </summary>
    public static class SettingUtils
    {
        /// <summary>
        /// A method that returns the name of the specified setting.
        /// </summary>
        /// <param name="setting">The setting whose name is the desired to get.</param>
        /// <returns>Setting name.</returns>
        public static string GetName (Setting setting)
        {
            return Enum.GetName(typeof(Setting), setting);
        }

        /// <summary>
        /// A method that returns a setting that matches the passed name.
        /// </summary>
        /// <param name="settingName">The name of the setting to get.</param>
        /// <returns>The value of the setting that matches the passed name</returns>
        public static Setting GetEnum (string settingName)
        {
            if (settingName == "dragonAge") return Setting.DragonAge;
            if (settingName == "genericFantasy") return Setting.GenericFantasy;
            if (settingName == "datective") return Setting.Detective;
            if (settingName == "defaultDemo") return Setting.DefaultDemo;
            throw new Exception("UNRECOGNIZED SETTING NAME: " + settingName);
        }
    }
}
