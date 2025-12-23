// Helper: Apply theme to body
function applyTheme(theme) {
  if (theme === "dark") {
    document.body.style.backgroundColor = "#222";
    document.body.style.color = "#f5f5f5";
  } else {
    // default light theme
    document.body.style.backgroundColor = "#ffffff";
    document.body.style.color = "#000000";
  }
}
// 1) Username with localStorage
const usernameInput = document.getElementById("username-input");
const saveUsernameBtn = document.getElementById("save-username-btn");
const clearUsernameBtn = document.getElementById("clear-username-btn");
const greetingText = document.getElementById("greeting-text");
// On page load, check if we already have a stored username
const storedName = localStorage.getItem("username");
if (storedName !== null) {
  // If name exists, show a friendly greeting
  greetingText.textContent = "Welcome back, " + storedName + " 👋";
}
// When user clicks "Save Name"
saveUsernameBtn.addEventListener("click", function () {
  const name = usernameInput.value.trim();
  if (name === "") {
    greetingText.textContent = "Please enter a non-empty name.";
    return;
  }
  // Save in localStorage
  localStorage.setItem("username", name);
  greetingText.textContent =
    "Saved! Hello, " + name + " 😊 (try refreshing the page)";
});
// When user clicks "Clear Stored Name"
clearUsernameBtn.addEventListener("click", function () {
  localStorage.removeItem("username");
  greetingText.textContent = "Stored name cleared. No name stored yet.";
  usernameInput.value = "";
});
// 2) Theme preference with localStorage
const toggleThemeBtn = document.getElementById("toggle-theme-btn");
const themeStatus = document.getElementById("theme-status");
// On page load, check stored theme
let currentTheme = localStorage.getItem("theme");
if (!currentTheme) {
  // If nothing stored, default to "light"
  currentTheme = "light";
}
// Apply theme on initial load
applyTheme(currentTheme);
themeStatus.textContent = "Current theme: " + currentTheme.toUpperCase();
// On button click, toggle between light and dark
toggleThemeBtn.addEventListener("click", function () {
  // Flip theme
  currentTheme = currentTheme === "light" ? "dark" : "light";
  // Save new theme in localStorage
  localStorage.setItem("theme", currentTheme);
  // Apply
  applyTheme(currentTheme);
  themeStatus.textContent = "Current theme: " + currentTheme.toUpperCase();
});
// 3) Click counter with sessionStorage
const sessionClickBtn = document.getElementById("session-click-btn");
const sessionCountText = document.getElementById("session-count-text");
// Get existing count, or start at 0
let clickCount = Number(sessionStorage.getItem("clickCount") || 0);
// Show initial value
sessionCountText.textContent =
  "You have clicked " + clickCount + " times in this tab.";
// On button click, increase and store
sessionClickBtn.addEventListener("click", function () {
  clickCount++;
  sessionStorage.setItem("clickCount", String(clickCount));
  sessionCountText.textContent =
    "You have clicked " + clickCount + " times in this tab.";
});
