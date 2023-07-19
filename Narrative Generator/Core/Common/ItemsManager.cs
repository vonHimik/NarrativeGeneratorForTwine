using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// The class that controls the creation and management of items.
    /// </summary>
    public class ItemsManager
    {
        private HashSet<Item> itemsInStory = new HashSet<Item>();

        /// <summary>
        /// A method for creation a new item.
        /// </summary>
        /// <param name="name">New item name.</param>
        /// <param name="type">New item type.</param>
        /// <returns>New item.</returns>
        public static Item CreateItem (string name, ItemsTypes type)
        {
            Item newItem = new Item();
            newItem.SetItemName(name);
            newItem.SetItemType(type);
            return newItem;
        }

        /// <summary>
        /// A method that adds the specified item to the specified location.
        /// </summary>
        /// <param name="location">Target location.</param>
        /// <param name="item">Item to add.</param>
        public void AddItemInLocation (LocationDynamic location, Item item)
        {
            location.AddItem(item);
        }

        /// <summary>
        /// A method that adds the specified item to the specified agent.
        /// </summary>
        /// <param name="agent">Target agent.</param>
        /// <param name="item">Item to add.</param>
        public void AddItemToAgent (AgentStateDynamic agent, Item item)
        {
            agent.AddItem(item);
        }

        /// <summary>
        /// Adds the specified items to the list of items.
        /// </summary>
        /// <param name="items">Items to add.</param>
        public void AddItems (HashSet<Item> items)
        {
            foreach (var item in items)
            {
                itemsInStory.Add(item);
            }
        }
    }
}
