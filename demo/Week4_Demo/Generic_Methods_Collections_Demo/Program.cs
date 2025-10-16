using Generic_Methods_Collections_Demo.Models;
using Generic_Methods_Collections_Demo.Services;
using System;
using System.Collections.Generic;

namespace Generic_Methods_Collections_Demo
{
    /// <summary>
    /// Entry point for Warehouse Inventory Management demo.
    /// Demonstrates usage of generic classes, methods, and collections.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Generic Class in Use
            InventoryManager<Electronics> electronicsInventory = new InventoryManager<Electronics>();
            InventoryManager<Furniture> furnitureInventory = new InventoryManager<Furniture>();

            // Adding electronic products
            Electronics laptop = new Electronics { Id = 1, Name = "Laptop", Price = 55000, WarrantyYears = 2 };
            Electronics tv = new Electronics { Id = 2, Name = "Smart TV", Price = 45000, WarrantyYears = 3 };
            electronicsInventory.AddItem(laptop);
            electronicsInventory.AddItem(tv);

            // Adding furniture products
            Furniture chair = new Furniture { Id = 101, Name = "Office Chair", Price = 5000, Material = "Leather" };
            Furniture table = new Furniture { Id = 102, Name = "Dining Table", Price = 12000, Material = "Wood" };
            furnitureInventory.AddItem(chair);
            furnitureInventory.AddItem(table);

            // Displaying items
            electronicsInventory.DisplayAllItems();
            furnitureInventory.DisplayAllItems();

            // Generic Method in Use

            // List<T>
            // Use Cases:
            // - Maintaining a collection of inventory items.
            // - Managing a shopping cart in an e-commerce app.
            List<Electronics> expensiveElectronics = electronicsInventory.FindItems(e => e.Price > 50000);
            Console.WriteLine("\nElectronics priced above ₹50,000:");
            foreach (Electronics item in expensiveElectronics)
            {
                item.DisplayInfo();
            }

            // Stack<T>
            // Use Cases:
            // - Keeping track of "Recently Viewed Products" in LIFO order.
            // - Undo/Redo feature in text editors.
            Stack<Electronics> recentViews = new Stack<Electronics>();
            recentViews.Push(laptop);
            recentViews.Push(tv);
            Console.WriteLine("\n Last viewed product: " + recentViews.Peek().Name);

            // Queue<T>
            // Use Cases:
            // - Order processing (first come, first served).
            // - Printing job queue in printers.
            Queue<Electronics> orderQueue = new Queue<Electronics>();
            orderQueue.Enqueue(laptop);
            orderQueue.Enqueue(tv);
            Console.WriteLine("\n Next order to process: " + orderQueue.Peek().Name);

            // Dictionary<TKey, TValue>
            // Use Cases:
            // - Mapping product ID to product details.
            // - Caching user sessions or API responses.
            Dictionary<int, Electronics> productDirectory = new Dictionary<int, Electronics>();
            productDirectory.Add(1, laptop);
            productDirectory.Add(2, tv);
            Console.WriteLine("\n Product ID 2: " + productDirectory[2].Name);

            // HashSet<T>
            // Use Cases:
            // - Storing unique product names (avoid duplicates).
            // - Keeping unique tags or categories in a blog.
            HashSet<Electronics> uniqueProducts = new HashSet<Electronics> { laptop, tv, laptop }; // Duplicate ignored
            Console.WriteLine("\n Unique Products: " + string.Join(", ", uniqueProducts.Select(e => e.Name)));


            // Generic Utility Method

            List<string> suppliers = new List<string> { "TechWorld", "FurniCo", "ElectroMart" };
            Utilities.PrintAllItems<string>(suppliers);


            // Removing an item

            furnitureInventory.RemoveItem(101);
            furnitureInventory.DisplayAllItems();
        }
    }
}
