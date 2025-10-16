
using Abstract_Interface_Sealed_Demo.Models;
/// <summary>
/// Create Car instance
/// </summary>
Car car = new Car("MH12AB1234", "Toyota", 180, 5);
car.Start();
car.DisplayInfo();
car.Stop();

Console.WriteLine();

///<summary>
///Create Bike instance
///</summary> 
Bike bike = new Bike("MH14XY5678", "Yamaha", 160, "Sports");
bike.Start();
bike.DisplayInfo();
bike.Stop();

Console.WriteLine();