/*
  Assignment 7.1
  Topic: Variables, Functions, Loops, Data Types
  Goal: Practice pure JavaScript logic (no UI)
*/

// Step 1: Array of student scores
const scores = [45, 78, 92, 30, 65];

// Step 2: Function to calculate pass/fail
function calculateStatus(score) {
  // Strict type checking
  if (typeof score !== "number") {
    return "Invalid Score";
  }

  return score > 50 ? "Pass" : "Fail";
}

// Step 3: Loop through scores and log status
scores.forEach((score, index) => {
  const status = calculateStatus(score);
  console.log(`Student ${index + 1}: Score = ${score}, Status = ${status}`);
});
