// BASIC "CLASS" USING CONSTRUCTOR FUNCTION

function Person(name, age) {
  // Instance properties
  this.name = name;
  this.age = age;
}

// Prototype method
Person.prototype.introduce = function () {
  console.log("Hi, I am " + this.name + ", age " + this.age + ".");
};

// INHERITANCE + METHOD OVERRIDING

function Employee(name, age, role) {
  // Call parent constructor
  Person.call(this, name, age);
  this.role = role;
}

// Set up inheritance
Employee.prototype = Object.create(Person.prototype);
Employee.prototype.constructor = Employee;

// Override parent method
Employee.prototype.introduce = function () {
  console.log(
    "Hi, I am " +
      this.name +
      ", working as a " +
      this.role +
      ", age " +
      this.age +
      "."
  );
};

//  METHOD OVERLOADING 


function Calculator() {}

Calculator.prototype.add = function () {

  if (arguments.length === 2) {
    return arguments[0] + arguments[1];
  }
  if (arguments.length === 3) {
    return arguments[0] + arguments[1] + arguments[2];
  }
  return 0;
};

// STATIC PROPERTIES & METHODS

function MathUtil() {}

// Static property
MathUtil.PI = 3.14159;

// Static method
MathUtil.circleArea = function (radius) {
  return MathUtil.PI * radius * radius;
};


// INTERFACE SIMULATION USING PROTOTYPE
function Drivable() {}

Drivable.prototype.drive = function () {
  throw new Error("drive() method must be implemented");
};

// Car implements Drivable
function Car(brand) {
  this.brand = brand;
}

Car.prototype = Object.create(Drivable.prototype);
Car.prototype.constructor = Car;

Car.prototype.drive = function () {
  console.log(this.brand + " car is driving");
};

// Bike implements Drivable
function Bike(brand) {
  this.brand = brand;
}

Bike.prototype = Object.create(Drivable.prototype);
Bike.prototype.constructor = Bike;

Bike.prototype.drive = function () {
  console.log(this.brand + " bike is driving");
};

// Event Listeners for Demo Buttons
document.getElementById("btn-basic").addEventListener("click", function () {
  console.clear();
  var p = new Person("Shahil", 22);
  p.introduce();
});

document.getElementById("btn-inherit").addEventListener("click", function () {
  console.clear();
  var e = new Employee("Shahil", 22, "Software Engineer");
  e.introduce(); // overridden method
});

document.getElementById("btn-overload").addEventListener("click", function () {
  console.clear();
  var calc = new Calculator();
  console.log("add(2, 3) =", calc.add(2, 3));
  console.log("add(2, 3, 4) =", calc.add(2, 3, 4));
});

document.getElementById("btn-static").addEventListener("click", function () {
  console.clear();
  console.log("PI =", MathUtil.PI);
  console.log("Circle area (r=5) =", MathUtil.circleArea(5));
});

document.getElementById("btn-interface").addEventListener("click", function () {
  console.clear();
  var car = new Car("Tesla");
  var bike = new Bike("Yamaha");

  // Polymorphism: same method, different behavior
  var vehicles = [car, bike];
  vehicles.forEach(function (v) {
    v.drive();
  });
});
