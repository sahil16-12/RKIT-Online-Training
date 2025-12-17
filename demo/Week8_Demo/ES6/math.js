// Named exports
export const PI = 3.14159;

export function add(a, b) {
  return a + b;
}

export function multiply(a, b) {
  return a * b;
}

// Default export (optional main function)
export default function calculateCircleArea(radius) {
  return PI * radius * radius;
}


function helper() {
  function internal() {
    console.log(" This is an internal helper function.");
  }
  internal();
}

helper();

let a = helper;

a();