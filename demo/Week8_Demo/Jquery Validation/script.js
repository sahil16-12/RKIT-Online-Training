// When DOM ready, attach validation
$(document).ready(function () {
  $.validator.addMethod("noNumbers", function(value, element) {
  return this.optional(element) || /^[A-Za-z ]+$/.test(value);
}, "Name should not contain numbers");

$("#signupForm").validate({
  rules: {
    username: {
      required: true,
      // remote: {
      //   url: "/api/check-username",
      //   type: "post",
      //   data: {
      //     username: () => $("#username").val()
      //   }
      // }
    },
    email: { required: true, email: true },
    password: { required: true, minlength: 8 },
    otherDetails: {
      required: function() {
        return $("#username").val() === "other";
      }
    },
    name: { noNumbers: true }
  },
  messages: {
    username: { remote: "User exists, choose another" }
  },
  errorPlacement: function(error, element) {
    error.insertAfter(element);
  }
});

});
