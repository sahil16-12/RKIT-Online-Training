using Dynamic_type_demo.Services;

Console.WriteLine("==== Dynamic Type Demonstration ====\n");

// Step 1: Demonstrate dynamic JSON parsing
JsonDynamicService jsonService = new JsonDynamicService();
jsonService.DisplayDynamicJsonData();

Console.WriteLine("\n----------------------------------\n");

// Step 2: Demonstrate dynamic operations and their runtime behavior
DynamicOperationService operationService = new DynamicOperationService();
operationService.PerformDynamicOperations();

Console.WriteLine("\n==== End of Demo ====");