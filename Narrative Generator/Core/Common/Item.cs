using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// A class implements an item that can be stored in locations or owned by agents.
    /// </summary>
    public class Item
    {
        private string name;
        private ItemsTypes type;

        /// <summary>
        /// Sets the name of the item.
        /// </summary>
        /// <param name="name">Setted name.</param>
        public void SetItemName (string name) { this.name = name; }

        /// <summary>
        /// Returns the name of the item.
        /// </summary>
        /// <returns>Item name.</returns>
        public string GetItemName() { return name; }

        /// <summary>
        /// Sets the type of the item.
        /// </summary>
        /// <param name="type">Setted type.</param>
        public void SetItemType (ItemsTypes type) { this.type = type; }

        /// <summary>
        /// Returns the type of the item.
        /// </summary>
        /// <returns>Item type.</returns>
        public ItemsTypes GetItemType() { return type; }
    }
}
