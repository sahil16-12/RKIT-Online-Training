using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamic_type_demo.Services
{
    /// <summary>
    /// Demonstrates the usage of 'dynamic' for runtime-bound operations and highlights its limitations.
    /// </summary>
    public class DynamicOperationService
    {
        /// <summary>
        /// Performs dynamic mathematical operations and shows how runtime errors occur if invalid members are accessed.
        /// </summary>
        public void PerformDynamicOperations()
        {
            Console.WriteLine(">>> Example 2: Using 'dynamic' for flexible operations\n");

            dynamic number1 = 10;
            dynamic number2 = 5;

            int result = number1 + number2;
            Console.WriteLine($"Addition (dynamic): {number1} + {number2} = {result}");

            dynamic text = "Hello";
            Console.WriteLine($"Text value: {text}");

            // Demonstrating runtime error (limitation)
            try
            {
                // 'text.Add' method doesn't exist, so this will compile fine but fail at runtime.
                text.Add("World");
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex)
            {
                Console.WriteLine("\n Runtime Error:");
                Console.WriteLine("Attempted to call a method that doesn't exist on the object.");
                Console.WriteLine($"Error Details: {ex.Message}");
            }

            Console.WriteLine("\nLimitation shown: Dynamic operations skip compile-time checking,");
            Console.WriteLine("so errors only appear when the program runs.");
        }
    }
}
