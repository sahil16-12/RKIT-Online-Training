// VARIABLES + DATA TYPES

let Name = "Sahil"; 
let age = 20;        
let isStudent = true;
const country = "India"; 

// FUNCTION 
function introducePerson(personName, personAge) {
  console.log("Hi! My name is " + personName + " and I am " + personAge + " years old.");
}

// Calling the function
introducePerson(Name, age);

// ARROW FUNCTION
const square = (num) => {
  return num * num; 
};

console.log("Square of 5 is:", square(5));

// LOOPS

// FOR LOOP 
console.log("Counting from 1 to 5:");
for (let i = 1; i <= 5; i++) {
  console.log(i);
}

// WHILE LOOP 
let battery = 3;
console.log("Phone battery draining:");
while (battery > 0) {
  console.log("Battery at " + battery + "%");
  battery--;
}

// FOR...OF LOOP 
let fruits = ["Mango", "Banana", "Apple"];

console.log("List of fruits:");
for (let fruit of fruits) {
  console.log("→ " + fruit);
}
