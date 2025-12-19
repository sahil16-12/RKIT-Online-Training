// Define "Class" using constructor function
function Car(brand, color) {
  // Instance properties (each car has its own)
  this.brand = brand;
  this.color = color;
}

// Define methods on prototype (shared among all objects)
Car.prototype.start = function () {
  console.log(`${this.brand} started!`);
};

Car.prototype.showDetails = function () {
  console.log(`Brand: ${this.brand}, Color: ${this.color}`);
};

// Add static property and static method
Car.category = "Vehicle"; // static property

Car.compare = function (carA, carB) {
  return carA.brand === carB.brand ? "Same brand!" : "Different brands.";
};

// Buttons for testing
document.getElementById("btn-create").addEventListener("click", () => {
  console.clear();
  console.log("Creating car objects using prototype...");

  const car1 = new Car("Tesla", "Red");
  const car2 = new Car("BMW", "Black");

  car1.start(); // Tesla started!
  car1.showDetails(); // Brand: Tesla, Color: Red
  car2.start(); // BMW started!
  car2.showDetails(); // Brand: BMW, Color: Black

  console.log("Category:", Car.category, "(static property)"); // Vehicle
});

document.getElementById("btn-static").addEventListener("click", () => {
  console.clear();
  console.log("Using static method (compare)...");

  const carA = new Car("Tesla", "Blue");
  const carB = new Car("Tesla", "Grey");
  const carC = new Car("Ford", "Green");

  console.log(Car.compare(carA, carB)); // Same brand!
  console.log(Car.compare(carA, carC)); // Different brands.
});
