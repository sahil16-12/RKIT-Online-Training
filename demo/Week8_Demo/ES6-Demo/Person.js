export class Person {
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
