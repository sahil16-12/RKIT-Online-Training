/*
  Assignment 7.2
  Topic: DOM Manipulation, Events, Bubbling
*/

const bulb = document.querySelector("#bulb");
const button = document.querySelector("#toggleBtn");
const body = document.querySelector("body");
const clickInfo = document.querySelector("#clickInfo");

let isOn = false;
let bodyClicks = 0;

// Body click listener (demonstrates bubbling)
body.addEventListener("click", () => {
  bodyClicks++;
  clickInfo.textContent = `Body Clicks: ${bodyClicks}`;
});

// Button click
button.addEventListener("click", (e) => {
  // Stop event bubbling to body
  e.stopPropagation();

  isOn = !isOn;

  if (isOn) {
    bulb.style.backgroundColor = "yellow";
    button.textContent = "Turn Off";
  } else {
    bulb.style.backgroundColor = "gray";
    button.textContent = "Turn On";
  }
});
