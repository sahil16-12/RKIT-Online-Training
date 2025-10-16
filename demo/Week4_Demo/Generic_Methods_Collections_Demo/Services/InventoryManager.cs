using System;
using System.Collections.Generic;
using Generic_Methods_Collections_Demo.Models;

namespace Generic_Methods_Collections_Demo.Services
{
    /// <summary>
    /// Generic inventory manager class that can handle different types of products.
    /// T represents the type of item (e.g., Electronics, Furniture).
    /// </summary>
    /// <typeparam name="T">Type of item stored in inventory (must inherit from Product).</typeparam>
    public class InventoryManager<T> where T : Product
    {
        /// <summary>
        /// List to hold all items of type T.
        /// </summary>
        private List<T> items;

        /// <summary>
        /// Constructor initializes the collection.
        /// </summary>
        public InventoryManager()
        {
            items = new List<T>();
        }

        /// <summary>
        /// Adds an item of type T to the inventory.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void AddItem(T item)
        {
            items.Add(item);
            Console.WriteLine($"Item '{item.Name}' added to inventory.");
        }

        /// <summary>
        /// Removes an item from the inventory based on ID.
        /// </summary>
        /// <param name="id">The ID of the item to remove.</param>
        public void RemoveItem(int id)
        {
            T foundItem = items.Find(x => x.Id == id);
            if (foundItem != null)
            {
                items.Remove(foundItem);
                Console.WriteLine($"Item '{foundItem.Name}' removed from inventory.");
            }
            else
            {
                Console.WriteLine($"Item with ID {id} not found.");
            }
        }

        /// <summary>
        /// Displays all items currently in the inventory.
        /// </summary>
        public void DisplayAllItems()
        {
            Console.WriteLine("\n=== Inventory Items ===");
            foreach (T item in items)
            {
                item.DisplayInfo();
            }
        }

        /// <summary>
        /// Uses a generic method to find items by a specific condition.
        /// Demonstrates generic method with Func delegate.
        /// </summary>
        /// <param name="predicate">Condition to filter items.</param>
        /// <returns>List of matching items.</returns>
        public List<T> FindItems(Predicate<T> predicate)
        {
            List<T> results = new List<T>();
            foreach (T item in items)
            {
                if (predicate(item))
                {
                    results.Add(item);
                }
            }
            return results;
        }
    }
}
