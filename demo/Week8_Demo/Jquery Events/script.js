let keyboardBlocked = false;

// GLOBAL KEYBOARD HANDLER 
$(document).on("keydown", function (e) {

  // UNLOCK
  if (e.key === "u") {
    keyboardBlocked = false;
    console.log("Keyboard unblocked");
    return;
  }

  // LOCK
  if (e.key === "b") {
    keyboardBlocked = true;
    console.log("Keyboard blocked");
    return false;
  }

  // BLOCK EVERYTHING ELSE
  if (keyboardBlocked) {
    console.log("Keyboard blocked");
    return false;
  }
  // WHEN PRESSED 'a' ALERT BOX WILL BE SHOWN
  if(e.key === "a"){
    alert("You pressed the 'a' key!");
  }
});

// ---------------- OTHER EVENTS ----------------

// Button click
$("#btnClick").on("click", function () {
  $("#log").text("Button was clicked by user!");
});

// Programmatic click
$("#btnProg").on("click", function () {
  $("#btnClick").trigger("click");
});

// Typing input
$("#textInput").on("keyup", function () {
  $("#log").text("You typed: " + $(this).val());
});

// Custom event
$(document).on("hello", function (event, msg) {
  alert(msg);
});

$("#btnCustom").on("click", function () {
  $(document).trigger("hello", ["Hello from custom event!"]);
});
