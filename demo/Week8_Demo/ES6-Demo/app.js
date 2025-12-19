// Import all classes
import { Person } from "./Person.js";
import { Employee } from "./Employee.js";
import { Calculator } from "./Calculator.js";
import { MathUtil } from "./MathUtil.js";
import { Car } from "./Car.js";
import { Bike } from "./Bike.js";

// Event Listeners for Demo Buttons

document.getElementById("btn-basic").addEventListener("click", () => {
  console.clear();
  const p = new Person("Shahil", 22);
  p.introduce();
});

document.getElementById("btn-inherit").addEventListener("click", () => {
  console.clear();
  const e = new Employee("Shahil", 22, "Software Engineer");
  e.introduce(); // overridden method
});

document.getElementById("btn-overload").addEventListener("click", () => {
  console.clear();
  const calc = new Calculator();
  console.log("add(2, 3) =", calc.add(2, 3));
  console.log("add(2, 3, 4) =", calc.add(2, 3, 4));
});

document.getElementById("btn-static").addEventListener("click", () => {
  console.clear();
  console.log("PI =", MathUtil.PI);
  console.log("Circle area (r=5) =", MathUtil.circleArea(5));
});

document.getElementById("btn-interface").addEventListener("click", () => {
  console.clear();
  const car = new Car("Tesla");
  const bike = new Bike("Yamaha");

  car.drive();
  bike.drive();
});
