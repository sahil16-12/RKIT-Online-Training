  $(document).ready(function () {



      $("#userForm").on("submit", function (event) {

        // Prevent form submission
        event.preventDefault();

        // Capture form data using serializeArray()
        let formDataArray = $(this).serializeArray();

        // Convert array into clean JSON object
        let jsonObject = {};

        $.each(formDataArray, function (index, field) {

          // Handle multiple checkbox values
          if (jsonObject[field.name]) {

            if (!Array.isArray(jsonObject[field.name])) {
              jsonObject[field.name] = [jsonObject[field.name]];
            }

            jsonObject[field.name].push(field.value);

          } else {
            jsonObject[field.name] = field.value;
          }
        });

        // Convert JSON object to formatted JSON string
        let jsonString = JSON.stringify(jsonObject, null,2);

        // Display JSON in <pre>
        $("#output").text(jsonString);

      });

    });