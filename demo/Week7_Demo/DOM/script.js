// Selecting elements
// Using getElementById: grab elements by their IDs
const pageTitle = document.getElementById("page-title");
const mainBox = document.getElementById("main-box");
const textPara = document.getElementById("text-para");
// Using querySelector: first element that matches a CSS selector
const infoPara = document.querySelector(".info-para"); // first element with class "info-para"
// Also select the buttons
const btnChangeText = document.getElementById("btn-change-text");
const btnHighlight = document.getElementById("btn-highlight");
const btnChangeStyle = document.getElementById("btn-change-style");
const btnToggleInfo = document.getElementById("btn-toggle-info");
pageTitle.innerHTML = "DOM Manipulation Demo (JS Updated Title)";
// Button 1: Change Text using innerHTML
btnChangeText.addEventListener("click", function () {
  // We are changing the HTML inside the paragraph.
  // Note: We are adding <strong> tag here to show that innerHTML can contain HTML.
  textPara.innerHTML = "Text changed! <strong>This is now bold.</strong>";
});
// Button 2: Toggle Highlight using classList
btnHighlight.addEventListener("click", function () {
  // Here we just toggle a CSS class that we defined in <style>.
  // This is cleaner than writing many style lines in JavaScript.
  textPara.classList.toggle("highlight");
});
// Button 3: Change Box Style using .style
btnChangeStyle.addEventListener("click", function () {
  // We directly change inline styles using JS.
  // This is good for quick, one-time modifications.
  // Change background color, text color, and border
  mainBox.style.backgroundColor = "#f0f8ff"; // light bluish
  mainBox.style.color = "#333";
  mainBox.style.borderColor = "#007bff";
  mainBox.style.boxShadow = "0 0 10px rgba(0,0,0,0.2)";
});
// Button 4: Show / Hide Info Paragraph using querySelector
btnToggleInfo.addEventListener("click", function () {
  // We used querySelector above to select the ".info-para" element.
  // Now we will toggle the "hidden" class to show/hide it.
  infoPara.classList.toggle("hidden");
});
