// 1) Basic click event
$("#btnClick").on("click", function () {
  $("#log").text("Button was clicked by user!");
});

// 2) Programmatically trigger click
$("#btnProg").on("click", function () {
  $("#btnClick").trigger("click");
});

// 3) Keyup event (typing)
$("#textInput").on("keyup", function () {
  $("#log").text("You typed: " + $(this).val());
});

// 4) Custom logic example
$(document).on("hello", function (event, msg) {
  $("#log").text("Custom event received: " + msg);
});

$("#btnCustom").on("click", function () {
  $(document).trigger("hello", ["Hello from custom event!"]);
});
