// Custom password validation rule using Regex
$.validator.addMethod(
    "strongPassword",
    function (value) {
      return /^(?=.*\d)(?=.*[!@#$%^&*]).{8,}$/.test(value);
    },
    "Password must be at least 8 characters long and include a number and a special character"
  );
  
  // Apply validation rules
  $("#registerForm").validate({
    rules: {
      name: {
        required: true
      },
      email: {
        required: true,
        email: true
      },
      password: {
        required: true,
        strongPassword: true
      }
    },
    messages: {
        name: {
          required: "Name is required"
        },
        email: {
          required: "Email is required",
          email: "Enter a valid email address"
        },
        password: {
          required: "Password is required"
        }
      },
    errorPlacement: function (error, element) {
      error.insertAfter(element);
    }
  });
  