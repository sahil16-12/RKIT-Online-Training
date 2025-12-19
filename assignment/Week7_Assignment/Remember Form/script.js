/*
  Assignment 7.3
  Topic: LocalStorage, SessionStorage, Cookies
*/

// ---------- Local Storage ----------
const usernameInput = document.querySelector("#username");
const saveUserBtn = document.querySelector("#saveUser");

// Load saved username
usernameInput.value = localStorage.getItem("username") || "";

saveUserBtn.addEventListener("click", () => {
  localStorage.setItem("username", usernameInput.value);
});

// ---------- Session Storage ----------
const sessionBtn = document.querySelector("#sessionBtn");
const sessionCountText = document.querySelector("#sessionCount");

let sessionCount = Number(sessionStorage.getItem("clicks")) || 0;
sessionCountText.textContent = `Session Clicks: ${sessionCount}`;

sessionBtn.addEventListener("click", () => {
  sessionCount++;
  sessionStorage.setItem("clicks", sessionCount);
  sessionCountText.textContent = `Session Clicks: ${sessionCount}`;
});

// ---------- Cookies ----------
const cookieBanner = document.querySelector("#cookieBanner");
const acceptBtn = document.querySelector("#acceptCookies");

// Check cookie
if (document.cookie.includes("consent=true")) {
  cookieBanner.style.display = "none";
}

acceptBtn.addEventListener("click", () => {
  const expiry = new Date();
  expiry.setDate(expiry.getDate() + 7);

  document.cookie = `consent=true; expires=${expiry.toUTCString()}`;
  cookieBanner.style.display = "none";
});
