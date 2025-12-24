    $(document).ready(function () {
  // Helper to log output
  function log(msg) {
    $("#output").append(msg + "\n");
  }

  // 1) $.each() — iterate array & object
  $("#btnEach").click(function () {
    $("#output").text(""); // clear
    var arr = [10, 20, 30];
    log("Array each:");
    $.each(arr, function (i, val) {
      log("index " + i + ": " + val);
    });

    var obj = {a: 1, b: 2};
    log("Object each:");
    $.each(obj, function (key, val) {
      log(key + " => " + val);
    });
  });

  // 2) $.map() — make new array
  $("#btnMap").click(function () {
    $("#output").text("");
    var numbers = [1, 2, 3, 4];
    var doubled = $.map(numbers, function (n) {
      return n * 2;
    });
    log("Original: " + numbers);
    log("Doubled: " + doubled);
  });

  // 3) $.grep() — filter array
  $("#btnGrep").click(function () {
    $("#output").text("");
    var nums = [2, 5, 8, 10, 3];
    var result = $.grep(nums, function (n) {
      return n > 5; // keep >5
    });
    log("Filtered (>5): " + result);
  });

  // 4) $.merge() — combine two arrays
  $("#btnMerge").click(function () {
    $("#output").text("");
    var a1 = ["a", "b"];
    var a2 = ["c", "d"];
    var copy = $.merge([], a1); // copy
    $.merge(copy, a2);
    log("Merged: " + copy);
  });

  // 5) $.extend() — merge objects
  $("#btnExtend").click(function () {
    $("#output").text("");
    var o1 = {x: 1, y: 2};
    var o2 = {y: 20, z: 30};
    var merged = $.extend({}, o1, o2); // do not modify originals
    log("o1: " + JSON.stringify(o1));
    log("o2: " + JSON.stringify(o2));
    log("Merged: " + JSON.stringify(merged));
  });
});
