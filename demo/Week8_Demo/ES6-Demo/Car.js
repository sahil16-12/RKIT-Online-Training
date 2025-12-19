import { Drivable } from "./Drivable.js";

export class Car extends Drivable {
  constructor(brand) {
    super();
    this.brand = brand;
  }

  drive() {
    console.log(`${this.brand} car is driving`);
  }
}
