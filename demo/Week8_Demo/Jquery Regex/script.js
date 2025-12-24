$(document).ready(function () {
  // Patterns
  const regexLetter = /^[a-zA-Z]+$/; // letters only
  const regexEmail = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$/i; // valid email
  const regexPhone = /^\d{10}$/; // exactly 10 digits

  // On form submit
  $("#form").on("submit", function (e) {
    e.preventDefault(); // stop default submission

    let errors = [];

    const user = $("#username").val();
    const email = $("#email").val();
    const phone = $("#phone").val();

    // Validate username
    if (!regexLetter.test(user)) {
      errors.push("Username: letters only.");
    }

    // Validate email
    if (!regexEmail.test(email)) {
      errors.push("Email: enter valid address.");
    }

    // Validate phone
    if (!regexPhone.test(phone)) {
      errors.push("Phone: enter 10 digits.");
    }

    // Show errors or success
    if (errors.length > 0) {
      $("#messages").html("<p id='errorList'>" + errors.join("<br>") + "</p>");
    } else {
      $("#messages").html("<p>All fields are valid!</p>");
    }
  });
});
