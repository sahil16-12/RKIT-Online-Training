// Listen for message from main thread
onmessage = function (event) {
  if (event.data === "start") {
    let sum = 0;

    // Heavy computation
    for (let i = 0; i < 5_000_000_000; i++) {
      sum += i;
    }

    // Send result back
    postMessage(sum);
  }
};
