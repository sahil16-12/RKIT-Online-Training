const logBox = document.getElementById("log");
function log(msg) {
  console.log(msg);
  logBox.textContent += msg + "\n";
}
document.getElementById("start-btn").addEventListener("click", () => {
  logBox.textContent = ""; // clear logs
  log("Creating a Promise...");
  // Create a new Promise
  const demoPromise = new Promise((resolve, reject) => {
    log("Inside executor: work started (Promise is now PENDING)");
    // Simulate async work using setTimeout
    setTimeout(() => {
      const success = Math.random() > 0.5; // random true/false
      if (success) {
        resolve("Promise fulfilled successfully!");
      } else {
        reject("Promise rejected (something went wrong).");
      }
    }, 2000); // 2 seconds delay
  });
  log("Immediately after creation, Promise is PENDING");
  console.log(demoPromise);
  // Attach .then and .catch handlers
  demoPromise
    .then((value) => {
      log("THEN called → Promise RESOLVED with value:");
      log("    " + value);
    })
    .catch((error) => {
      log("CATCH called → Promise REJECTED with reason:");
      log("    " + error);
    })
    .finally(() => {
      log("FINALLY called → Promise is now SETTLED (done).");
    });
  // Extra log after attaching handlers
  log("Waiting for 2 seconds... (Promise still pending)");
});
