// Helper functions for cookies
// Set a cookie with optional daysToExpire
function setCookie(name, value, daysToExpire) {
  let cookieStr = name + "=" + encodeURIComponent(value) + "; path=/";
  if (daysToExpire) {
    const date = new Date();
    date.setTime(date.getTime() + daysToExpire * 24 * 60 * 60 * 1000);
    cookieStr += "; expires=" + date.toUTCString();
  }
  document.cookie = cookieStr;
}
// Get a cookie by name
function getCookie(name) {
  const cookies = document.cookie.split(";");
  for (let c of cookies) {
    const [key, value] = c.trim().split("=");
    if (key === name) {
      return decodeURIComponent(value || "");
    }
  }
  return null; // not found
}
// Delete a cookie by setting its expiry in the past
function deleteCookie(name) {
  document.cookie = name + "=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/";
}
// 1) Remember username with cookies
const usernameInput = document.getElementById("username-input");
const saveNameBtn = document.getElementById("save-name-btn");
const deleteNameBtn = document.getElementById("delete-name-btn");
const greetingText = document.getElementById("greeting-text");
// On page load, check if username cookie exists
const storedName = getCookie("demo_username");
if (storedName) {
  greetingText.textContent = "Welcome back, " + storedName + " 👋";
  usernameInput.value = storedName;
} else {
  greetingText.textContent = "No name cookie set yet.";
}
// When user clicks "Save Name in Cookie"
saveNameBtn.addEventListener("click", function () {
  const name = usernameInput.value.trim();
  if (name === "") {
    greetingText.textContent = "Please enter a valid name.";
    return;
  }
  // Let's make the cookie valid for 7 days
  setCookie("demo_username", name, 7);
  greetingText.textContent =
    "Name saved in cookie! Refresh the page to see it remembered.";
});
// When user clicks "Delete Name Cookie"
deleteNameBtn.addEventListener("click", function () {
  deleteCookie("demo_username");
  greetingText.textContent = "Name cookie deleted.";
  usernameInput.value = "";
});
// 2) Visit counter using cookies
const visitCountText = document.getElementById("visit-count-text");
const resetVisitsBtn = document.getElementById("reset-visits-btn");
// Get current count from cookie
let visitCount = parseInt(getCookie("demo_visits") || "0", 10);
// Increase count for this visit
visitCount += 1;
setCookie("demo_visits", String(visitCount), 30); // remember for 30 days
// Show message
visitCountText.textContent =
  "You have visited/reloaded this page " +
  visitCount +
  " times (tracked using a cookie).";
// Reset visits button
resetVisitsBtn.addEventListener("click", function () {
  deleteCookie("demo_visits");
  visitCountText.textContent =
    "Visit count cookie reset. Refresh page to restart counting.";
});
// 3) Clear all demo cookies
const clearAllCookiesBtn = document.getElementById("clear-all-cookies-btn");
clearAllCookiesBtn.addEventListener("click", function () {
  deleteCookie("demo_username");
  deleteCookie("demo_visits");
  alert("Demo cookies cleared (username & visits).");
  greetingText.textContent = "No name cookie set yet.";
  usernameInput.value = "";
  visitCountText.textContent = "Visit count cleared. Refresh page to restart.";
});
