$(document).ready(function() {

    // Callback with hide()
    $("#btnHide").click(function() {
      $(".output").text("");
      $("#box").hide(1000, function() {
        $(".output").append("Hide animation completed\n");
      });
    });
  
    // Callback with slideUp() and slideDown()
    $("#btnSlide").click(function() {
      $(".output").text("");
      $("#box").slideUp(800, function() {
        $(".output").append("SlideUp completed — now sliding down...\n");
  
        // Callback inside callback
        $(this).slideDown(800, function() {
          $(".output").append("SlideDown completed\n");
        });
      });
    });
  
    // Using a Callback list
    var cbList = $.Callbacks();
    cbList.add(function(msg) {
      $(".output").append("Callback 1: " + msg + "\n");
    });
    cbList.add(function(msg) {
      $(".output").append("Callback 2: " + msg + "\n");
    });
  
    // Fire callbacks
    $("#btnHide").click(function() {
      cbList.fire("Callbacks fired!");
    });
  
  });
  