const logDiv = document.getElementById("log");
function log(msg) {
  console.log(msg);
  logDiv.textContent += msg + "\n";
}
// Fake API helper: returns a promise that resolves/rejects after a delay
function fakeApi(name, delay, shouldFail = false) {
  return new Promise((resolve, reject) => {
    setTimeout(() => {
      if (shouldFail) {
        reject(name + " failed after " + delay + "ms");
      } else {
        resolve(name + " success after " + delay + "ms");
      }
    }, delay);
  });
}
// Basic Promise example
document.getElementById("btn-basic").addEventListener("click", () => {
  logDiv.textContent = "";
  log("Starting basic promise...");
  const shouldFail = Math.random() < 0.5;
  const p = fakeApi("BasicRequest", 1000, shouldFail);
  p.then((result) => {
    log("Promise fulfilled: " + result);
  })
    .catch((error) => {
      log("Promise rejected: " + error);
    })
    .finally(() => {
      log("Basic promise finished (then/catch done).");
    });
});
// Promise.all example
document.getElementById("btn-all").addEventListener("click", () => {
  logDiv.textContent = "";
  log("Starting Promise.all...");
  const p1 = fakeApi("Request1", 1000, true);
  const p2 = fakeApi("Request2", 1500);
  Promise.all([p1, p2])
    .then((results) => {
      log("Promise.all fulfilled with results:");
      results.forEach((res, index) => log("   [" + index + "] " + res));
    })
    .catch((error) => {
      log("Promise.all rejected: " + error);
    });
});
// Promise.race example
document.getElementById("btn-race").addEventListener("click", () => {
  logDiv.textContent = "";
  log("Starting Promise.race...");
  const p1 = fakeApi("FastRequest", 800);
  const p2 = fakeApi("SlowRequest", 2000);
  Promise.race([p1, p2])
    .then((winner) => {
      log("Promise.race winner (fulfilled): " + winner);
    })
    .catch((error) => {
      log("Promise.race winner (rejected): " + error);
    });
});
// Promise.allSettled example
document.getElementById("btn-allsettled").addEventListener("click", () => {
  logDiv.textContent = "";
  log("Starting Promise.allSettled...");
  const p1 = fakeApi("Req1", 1000);
  const p2 = fakeApi("Req2", 1200, true);
  const p3 = fakeApi("Req3", 800);
  Promise.allSettled([p1, p2, p3]).then((results) => {
    log("📦 Promise.allSettled results:");
    results.forEach((res, index) => {
      if (res.status === "fulfilled") {
        log(`   [${index}] fulfilled → ${res.value}`);
      } else {
        log(`   [${index}] rejected → ${res.reason}`);
      }
    });
  });
});
