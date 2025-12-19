/*
  Assignment 8.1
  Topic: Date, Math, String methods
*/

// ---------- DIGITAL CLOCK ----------
const clockEl = document.querySelector("#clock");

function updateClock() {
  const now = new Date();
  clockEl.textContent = now.toLocaleTimeString();
}

// Update every second
setInterval(updateClock, 1000);
updateClock();

// ---------- RANDOM USER ID ----------
const userIdEl = document.querySelector("#userId");

// Random number between 1000 and 9999
const userId = Math.floor(Math.random() * 9000) + 1000;
userIdEl.textContent = `Generated User ID: ${userId}`;

// ---------- STRING FORMATTING ----------
const messyString = " heLLO woRLD ";

function formatString(str) {
  const trimmed = str.trim().toLowerCase();
  return trimmed
    .split(" ")
    .map(
      word => word.charAt(0).toUpperCase() + word.slice(1)
    )
    .join(" ");
}

document.querySelector("#formattedText").textContent =
  formatString(messyString);
