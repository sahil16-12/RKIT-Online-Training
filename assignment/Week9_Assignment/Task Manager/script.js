$(document).ready(function () {

  const API_URL = "https://jsonplaceholder.typicode.com/todos";

  /* -------------------- SHOW / HIDE SPINNER -------------------- */
  function showSpinner() {
    $("#spinner").show();
  }

  function hideSpinner() {
    $("#spinner").hide();
  }

  /* -------------------- GET : FETCH TODOS -------------------- */
  function loadTasks() {
    showSpinner();

    $.ajax({
      url: API_URL + "?_limit=5",
      method: "GET",
      success: function (tasks) {
        $("#taskTableBody").empty();

        tasks.forEach(task => {
          addRowToTable(task);
        });
      },
      complete: function () {
        hideSpinner();
      }
    });
  }

  /* -------------------- ADD ROW TO TABLE -------------------- */
  function addRowToTable(task) {
    let row = `
      <tr data-id="${task.id}">
        <td>${task.id}</td>
        <td class="${task.completed ? 'completed' : ''}">${task.title}</td>
        <td>${task.completed ? "Done" : "Pending"}</td>
        <td>
          <button class="updateBtn">Update</button>
          <button class="deleteBtn">Delete</button>
        </td>
      </tr>
    `;
    $("#taskTableBody").append(row);
  }

  /* -------------------- POST : ADD TASK -------------------- */
  $("#addTaskBtn").click(function () {
    let taskTitle = $("#newTask").val().trim();
    if (!taskTitle) return;

    showSpinner();

    $.ajax({
      url: API_URL,
      method: "POST",
      data: JSON.stringify({
        title: taskTitle,
        completed: false
      }),
      contentType: "application/json",
      success: function (newTask) {
        addRowToTable(newTask);
        $("#newTask").val("");
      },
      complete: function () {
        hideSpinner();
      }
    });
  });

  /* -------------------- DELETE : REMOVE TASK -------------------- */
  $("#taskTableBody").on("click", ".deleteBtn", function () {
    let row = $(this).closest("tr");
    let id = row.data("id");

    showSpinner();

    $.ajax({
      url: API_URL + "/" + id,
      method: "DELETE",
      success: function () {
        row.remove();
      },
      complete: function () {
        hideSpinner();
      }
    });
  });

  /* -------------------- PUT : UPDATE TASK -------------------- */
  $("#taskTableBody").on("click", ".updateBtn", function () {
    let row = $(this).closest("tr");
    let id = row.data("id");
    let taskCell = row.find("td:eq(1)");
    let statusCell = row.find("td:eq(2)");

    let updatedTitle = prompt("Edit task:", taskCell.text());
    if (!updatedTitle) return;

    showSpinner();

    $.ajax({
      url: API_URL + "/" + id,
      method: "PUT",
      data: JSON.stringify({
        id: id,
        title: updatedTitle,
        completed: true
      }),
      contentType: "application/json",
      success: function () {
        taskCell.text(updatedTitle).addClass("completed");
        statusCell.text("Done");
      },
      complete: function () {
        hideSpinner();
      }
    });
  });

  /* -------------------- INITIAL LOAD -------------------- */
  loadTasks();

});