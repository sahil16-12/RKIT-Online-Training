// Helper function to log messages in a div
function logMessage(element, message) {
  element.textContent = message;
  console.log(message);
}
const tempbutton = document.getElementById("tempbutton");
tempbutton.onclick = function () {
  console.log("This is another way to set onclick using JS property.");
};
tempbutton.addEventListener("click", function () {
  console.log("This is an addEventListener on the same button. Both will run.");
});
// 1) onclick examples
const jsOnclickBtn = document.getElementById("js-onclick-btn");
// Here we use the onclick PROPERTY in JS (not inline HTML).
jsOnclickBtn.onclick = function () {
  alert("JS onclick: Button clicked!");
};
// If we do this again, it will overwrite the previous function:
// jsOnclickBtn.onclick = function () {
//   alert("This overwrote the previous onclick handler!");
// };
// 2) onchange examples
const usernameInput = document.getElementById("username-input");
const languageSelect = document.getElementById("language-select");
const changeLog = document.getElementById("change-log");
// When user changes the text and then clicks somewhere else
usernameInput.onchange = function () {
  const value = usernameInput.value.trim();
  if (value === "") {
    logMessage(changeLog, "Username cleared or empty.");
  } else {
    logMessage(changeLog, "Username changed to: " + value);
  }
};
// When user selects a different option in dropdown
languageSelect.onchange = function () {
  const value = languageSelect.value;
  if (value === "") {
    logMessage(changeLog, "No language selected.");
  } else if (value === "js") {
    logMessage(changeLog, "You selected JavaScript.");
  } else if (value === "py") {
    logMessage(changeLog, "You selected Python.");
  } else if (value === "java") {
    logMessage(changeLog, "You selected Java.");
  }
};
// 3) addEventListener example
const multiListenerBtn = document.getElementById("multi-listener-btn");
const listenerLog = document.getElementById("listener-log");
// First listener
multiListenerBtn.addEventListener("click", function () {
  logMessage(listenerLog, "First listener: Button clicked.");
});
// Second listener (yes, both will run)
multiListenerBtn.addEventListener("click", function () {
  console.log("Second listener: Doing another action.");
  // We'll not overwrite the first one, both will execute.
});
// You could even add a third listener if you want:
function thirdListener() {
  console.log("Third listener: This could be removed later.");
}
multiListenerBtn.addEventListener("click", thirdListener);
// Example: removing a listener after 5 seconds
setTimeout(() => {
  multiListenerBtn.removeEventListener("click", thirdListener);
  console.log("Third listener removed after 5 seconds.");
}, 5000);
// 4) Event Bubbling example
const outer = document.getElementById("outer");
const inner = document.getElementById("inner");
const bubbleButton = document.getElementById("bubble-button");
const bubbleLog = document.getElementById("bubble-log");
// Click on outer box
outer.addEventListener("click", function () {
  logMessage(bubbleLog, "Outer DIV handler triggered (bubbled up here).");
});
// Click on inner box
inner.addEventListener("click", function () {
  logMessage(bubbleLog, "Inner DIV handler triggered.");
  // Note: we are NOT stopping propagation here,
  // so the event will continue bubbling to outer.
});
// Click on button
bubbleButton.addEventListener("click", function (event) {
  logMessage(bubbleLog, "Button handler triggered.");
  // Uncomment this line to stop the event from reaching inner/outer:
  event.stopPropagation();
  // stopImmediatePropagation in action -> It will stop bubbling and also other event listeners of same type on same element
  // event.stopImmediatePropagation();
});
