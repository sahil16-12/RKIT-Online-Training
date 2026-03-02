$(document).ready(function() {

    // Simulate an asynchronous task that resolves or rejects
    function asyncTask(shouldSucceed) {
      // Create a Deferred object (producer)
      var deferred = $.Deferred();
  
      $("#status").text("Task started...");
  
      // Simulate delay (like AJAX, timeout, etc.)
      setTimeout(function() {
        if (shouldSucceed) {
          deferred.resolve("Task completed successfully!");
        } else {
          deferred.reject("Task encountered an error!");
        }
      }, 2000);
  
      // Return only the Promise (consumer)
      return deferred.promise();
    }
  
    // When start button is clicked
    $("#btnStart").click(function() {
      $("#status, #result").text("");
  
      // Get a promise from the asyncTask
      var promise = asyncTask(Math.random() > 0.5);
  
      promise
        .done(function(message) {
          $("#status").text("SUCCESS:");
          $("#result").text(message);
        })
        .fail(function(msg) {
          $("#status").text("ERROR:");
          $("#result").text(msg);
        })
        .always(function() {
          console.log("Task settled (either success or failure).");
        });
    });
  
  });
  