using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace Dynamic_type_demo.Services
{
    /// <summary>
    /// Demonstrates how the 'dynamic' keyword can be used for flexible JSON handling
    /// when the structure of data is not known at compile time.
    /// </summary>
    public class JsonDynamicService
    {
        /// <summary>
        /// Reads a JSON string, deserializes it dynamically, and accesses its properties without predefined classes.
        /// </summary>
        public void DisplayDynamicJsonData()
        {
            Console.WriteLine(">>> Example 1: Using 'dynamic' with JSON data\n");

            string jsonString = "{\"Name\":\"Sahil\",\"Age\":22,\"City\":\"Anand\"}";

            // Deserialize JSON into a dynamic object
            dynamic? jsonData = JsonConvert.DeserializeObject<dynamic>(jsonString);

            Console.WriteLine("Accessing JSON properties dynamically:");
            Console.WriteLine($"Name: {jsonData.Name}");
            Console.WriteLine($"Age: {jsonData.Age}");
            Console.WriteLine($"City: {jsonData.City}");

            Console.WriteLine("\nThis shows how 'dynamic' helps when the structure of JSON is not fixed.");
        }
    }
}
