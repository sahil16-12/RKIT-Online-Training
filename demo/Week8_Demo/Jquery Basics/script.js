// Ensure the DOM is fully loaded
$(document).ready(function() {

  // 1) Change HTML/Text
  $("#btnText").click(function() {
    $("#content").html("Hello! Content changed by jQuery!");
  });

  // 2) Change CSS style
  $("#btnStyle").click(function() {
    $("#content").css("color", "blue");
    $(".box").addClass("highlight"); // add CSS class
  });

  // 3) Hide element
  $("#btnHide").click(function() {
    $(".box").hide(); // hide all elements with class .box
  });

  // 4) Get value from input
  $("#btnShowVal").click(function() {
    // val() gets current value
    alert("You typed: " + $("#userInput").val());
  });

});