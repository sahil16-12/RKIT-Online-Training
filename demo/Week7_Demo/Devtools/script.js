// 1️⃣ Console demonstration
document.getElementById("btn-log").addEventListener("click", () => {
  console.log(" Hello! This message is from console.log()");
  console.info("This is an info message");
  console.warn(" This is a warning message");
  console.error(" This is an error message");
});
// 2️⃣ Simulate a JS error
document.getElementById("btn-error").addEventListener("click", () => {
  console.log("About to simulate an error...");
  // This will cause an intentional error
  nonExistentFunction();
});
// 3️⃣ Fetch API demonstration (Network tab)
document.getElementById("btn-fetch").addEventListener("click", async () => {
  console.log("Fetching data from JSONPlaceholder...");
  const response = await fetch("https://jsonplaceholder.typicode.com/users");
  const data = await response.json();
  console.log("Data fetched:", data);
  alert("Data fetched! Check Network tab for this request.");
});
