let count = 0;

// Increment button click
$("#incrementBtn").on("click", function () {
  count++;
  $("#counter").text(count);
  logMessage("Increment button clicked");

  // Programmatic trigger when count hits 10
  if (count === 10) {
    logMessage("Counter reached 10 → auto reset triggered");
    $("#resetBtn").trigger("click");
  }
});

// Reset button click
$("#resetBtn").on("click", function () {
  count = 0;
  $("#counter").text(count);
  logMessage("Reset button clicked");
});

// Custom logger
function logMessage(message) {
  $("#logArea").append(`<div>${message}</div>`);
  $("#logArea").scrollTop($("#logArea")[0].scrollHeight);
}
