$(document).ready(function() {

  // Your unique CrudCrud base URL
  const baseURL = "https://crudcrud.com/api/83b4603c2a4b454ea6a1ffc2f5ab09f1/students";

  let editId = null; // will hold the ID during edit mode

  // Read (GET) — Load all students
  function fetchStudents() {
    $("#studentList").empty();
    $.ajax({
      url: baseURL,
      type: "GET",
      success: function(students) {
        // Loop and append each student
        students.forEach(student => {
          $("#studentList").append(
            `<li class="student-item" data-id="${student._id}"
                data-name="${student.name}"
                data-age="${student.age}"
                data-class="${student.class}">
               ${student.name}, Age: ${student.age}, Class: ${student.class}
               <button class="editBtn">Edit</button>
               <button class="deleteBtn">Delete</button>
             </li>`
          );
        });
      },
      error: function() {
        alert("Failed to fetch students.");
      }
    });
  }

  // Create (POST) — Add a student
  $("#createBtn").click(function() {
    const studentData = {
      name: $("#studentName").val(),
      age: $("#studentAge").val(),
      class: $("#studentClass").val()
    };

    $.ajax({
      url: baseURL,
      type: "POST",
      contentType: "application/json",
      data: JSON.stringify(studentData),
      success: function() {
        fetchStudents();
        clearForm();
      },
      error: function() {
        alert("Failed to create student.");
      }
    });
  });

  // Delete (DELETE)
  $("#studentList").on("click", ".deleteBtn", function() {
    const id = $(this).closest("li").data("id");
    $.ajax({
      url: baseURL + "/" + id,
      type: "DELETE",
      success: function() {
        fetchStudents();
      },
      error: function() {
        alert("Failed to delete student.");
      }
    });
  });

  // Setup form for Update
  $("#studentList").on("click", ".editBtn", function() {
    const li = $(this).closest("li");

    editId = li.data("id");
    const name = li.data("name");
    const age = li.data("age");
    const cls = li.data("class");

    $("#studentName").val(name);
    $("#studentAge").val(age);
    $("#studentClass").val(cls);

    $("#createBtn").prop("disabled", true);
    $("#updateBtn").prop("disabled", false);
    $("#cancelUpdateBtn").show();
  });

  // Update (PUT)
  $("#updateBtn").click(function() {
    const updatedData = {
      name: $("#studentName").val(),
      age: $("#studentAge").val(),
      class: $("#studentClass").val()
    };

    $.ajax({
      url: baseURL + "/" + editId,
      type: "PUT",
      contentType: "application/json",
      data: JSON.stringify(updatedData),
      success: function() {
        fetchStudents();
        clearForm();
      },
      error: function() {
        alert("Failed to update student.");
      }
    });
  });

  // Cancel Update
  $("#cancelUpdateBtn").click(function() {
    clearForm();
  });

  // Reset form
  function clearForm() {
    $("#studentName").val("");
    $("#studentAge").val("");
    $("#studentClass").val("");

    $("#createBtn").prop("disabled", false);
    $("#updateBtn").prop("disabled", true);
    $("#cancelUpdateBtn").hide();

    editId = null;
  }

  // Initial page load: read students
  fetchStudents();

});
