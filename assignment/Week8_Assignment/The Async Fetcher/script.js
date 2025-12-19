/*
  Assignment 8.4
  Topic: Async/Await, Promises, Fetch API
*/

const loadBtn = document.querySelector("#loadBtn");
const userList = document.querySelector("#userList");

async function loadUsers() {
  try {
    const response = await fetch(
      "https://jsonplaceholder.typicode.com/users"
    );

    if (!response.ok) {
      throw new Error("Failed to fetch data");
    }

    const users = await response.json();

    // Clear previous list
    userList.innerHTML = "";

    users.forEach(user => {
      const li = document.createElement("li");
      li.textContent = user.name;
      userList.appendChild(li);
    });

  } catch (error) {
    userList.innerHTML = "<li>Unable to load users</li>";
    console.error(error);
  }
}

loadBtn.addEventListener("click", loadUsers);
