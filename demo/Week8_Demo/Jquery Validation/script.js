// When DOM ready, attach validation
$(document).ready(function () {

  // 1) Add a custom validation rule: must be an even number
  $.validator.addMethod(
    "evenNumber",
    function (value, element) {
      if (this.optional(element)) return true;  // skip if empty
      var num = parseInt(value, 10);
      return !isNaN(num) && num % 2 === 0;
    },
    "Please enter an even number."
  );

  // 2) Attach validate() to the form
  $("#signupForm").validate({
    rules: {
      username: {
        required: true
      },
      email: {
        required: true,
        email: true
      },
      password: {
        required: true,
        minlength: 5
      },
      confirm_password: {
        required: true,
        equalTo: "#password"
      },
      evenNum: {
        required: true,
        evenNumber: true
      }
    },
    messages: {
      username: "Username is required.",
      email: {
        required: "Email required.",
        email: "Enter a valid email."
      },
      password: {
        required: "Password is required.",
        minlength: "At least 5 characters."
      },
      confirm_password: {
        equalTo: "Passwords must match."
      }
    }
  });

});
