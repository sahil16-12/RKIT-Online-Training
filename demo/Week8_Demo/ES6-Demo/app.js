
 // BASIC CLASS + PROPERTY DECLARATION

class Person {
  // property declaration
  name;
  age;

  constructor(name, age) {
    this.name = name;
    this.age = age;
  }

  introduce() {
    console.log(`Hi, I am ${this.name}, ${this.age} years old.`);
  }
}

// INHERITANCE + METHOD OVERRIDING

class Employee extends Person {
  role;

  constructor(name, age, role) {
    super(name, age); // call parent constructor
    this.role = role;
  }

  // Method overriding
  introduce() {
    console.log(
      `Hi, I am ${this.name}, working as a ${this.role}, age ${this.age}.`
    );
  }
}

// METHOD OVERLOADING 

class Calculator {
  add(...args) {
    // Method "overloading" simulation
    if (args.length === 2) {
      return args[0] + args[1];
    }
    if (args.length === 3) {
      return args[0] + args[1] + args[2];
    }
    return 0;
  }
}

// STATIC PROPERTIES & METHODS

class MathUtil {
  static PI = 3.14159; // static property

  static circleArea(radius) {
    return this.PI * radius * radius;
  }
}

// INTERFACE SIMULATION USING ABSTRACT CLASS

class Drivable {
  drive() {
    throw new Error("drive() method must be implemented");
  }
}

class Car extends Drivable {
  constructor(brand) {
    super();
    this.brand = brand;
  }

  drive() {
    console.log(`${this.brand} car is driving`);
  }
}

class Bike extends Drivable {
  constructor(brand) {
    super();
    this.brand = brand;
  }

  drive() {
    console.log(`${this.brand} bike is driving`);
  }
}

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
