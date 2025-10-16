using System;
using System.Collections.Generic;

namespace Generic_Methods_Collections_Demo.Services
{
    /// <summary>
    /// Static class demonstrating a generic utility method.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Generic method that prints all items in any collection type.
        /// Demonstrates how generic methods can work on multiple collection types.
        /// </summary>
        /// <typeparam name="T">Type of elements in the collection.</typeparam>
        /// <param name="collection">Collection to print items from.</param>
        public static void PrintAllItems<T>(IEnumerable<T> collection)
        {
            Console.WriteLine("\n--- Printing Collection Items ---");
            foreach (T item in collection)
            {
                Console.WriteLine(item);
            }
        }
    }
}
