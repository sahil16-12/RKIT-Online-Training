import { Person } from "./Person.js";

export class Employee extends Person {
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
