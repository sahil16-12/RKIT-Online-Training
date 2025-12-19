export class Calculator {
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
