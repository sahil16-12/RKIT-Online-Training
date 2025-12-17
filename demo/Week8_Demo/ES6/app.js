// Importing modules
import greetUser from "./greet.js";
import calculateCircleArea, { PI, add, multiply } from "./math.js";

// Using imports
greetUser("Shahil");

console.log("PI =", PI);
console.log("5 + 10 =", add(5, 10));
console.log("5 × 10 =", multiply(5, 10));
console.log("Area of circle (r=5):", calculateCircleArea(5));

// Async/Await example
async function fetchUsers() {
  console.log(" Fetching users...");
  try {
    const response = await fetch("https://jsonplaceholder.typicode.com/users");
    const users = await response.json();
    console.log(" Users fetched successfully:");
    console.table(users);
  } catch (error) {
    console.error(" Error fetching users:", error);
  }
}

fetchUsers();

// Class example
class Car {
  static category = "Vehicle"; // static property
  brand = "tesla";
  constructor(brand, color) {
    this.brand = brand;
    this.color = color;
  }

  start() {
    console.log(` ${this.brand} (${this.color}) started!`);
  }

  static info() {
    console.log(` All cars are categorized as: ${this.category}`);
  }
}

const car1 = new Car("Tesla", "Red");
car1.start();
Car.info();
