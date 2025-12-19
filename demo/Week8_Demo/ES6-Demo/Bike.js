import { Drivable } from "./Drivable.js";

export class Bike extends Drivable {
  constructor(brand) {
    super();
    this.brand = brand;
  }

  drive() {
    console.log(`${this.brand} bike is driving`);
  }
}
