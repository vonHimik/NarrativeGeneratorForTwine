using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// Enumerator for item types.
    /// </summary>
    public enum ItemsTypes
    {
        /// <summary>
        /// A value indicating the item's type as "evidence".
        /// </summary>
        EVIDENCE,
        /// <summary>
        /// A value indicating the item's type as "weapon".
        /// </summary>
        WEAPON,
        /// <summary>
        /// A value indicating the item's type as "armor".
        /// </summary>
        ARMOR
    }

    /// <summary>
    /// A class that facilitates interaction with item types and their use.
    /// </summary>
    public static class ItemsTypesUtils
    {
        /// <summary>
        /// A method that returns the name of the specified type of item.
        /// </summary>
        /// <param name="type">The type whose name is the desired to get.</param>
        /// <returns>Type name.</returns>
        public static string GetName (ItemsTypes type)
        {
            return Enum.GetName(typeof(ItemsTypes), type);
        }

        /// <summary>
        /// A method that returns a type that matches the passed name.
        /// </summary>
        /// <param name="name">The name of the type to get.</param>
        /// <returns>The value of the type that matches the passed name.</returns>
        public static ItemsTypes GetEnum (string name)
        {
            if (name == "evidence") return ItemsTypes.EVIDENCE;
            if (name == "weapon") return ItemsTypes.WEAPON;
            if (name == "armor") return ItemsTypes.ARMOR;
            throw new Exception("UNRECOGNIZED ITEM TYPE: " + name);
        }
    }
}
