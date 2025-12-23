const container = document.getElementById("cards-container");
// Create demo cards with fake images
for (let i = 1; i <= 10; i++) {
  const card = document.createElement("div");
  card.className = "card";
  const title = document.createElement("h3");
  title.textContent = "Card " + i;
  const img = document.createElement("img");
  img.src =
    "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///ywAAAAAAQABAAACAUwAOw=="; // 1x1 transparent gif
  img.classList.add("lazy-image");
  img.dataset.src = `https://via.placeholder.com/400x250.png?text=Image+${i}`;
  const text = document.createElement("p");
  text.textContent = "This image will load only when you scroll to this card.";
  card.appendChild(title);
  card.appendChild(img);
  card.appendChild(text);
  container.appendChild(card);
}
// Lazy loading logic using IntersectionObserver
const lazyImages = document.querySelectorAll("img.lazy-image");
const observer = new IntersectionObserver(
  (entries, obs) => {
    entries.forEach((entry) => {
      if (entry.isIntersecting) {
        const img = entry.target;
        img.src = img.dataset.src;
        console.log("Loading image:", img.dataset.src);
        img.classList.remove("lazy-image");
        obs.unobserve(img);
      }
    });
  },
  {
    rootMargin: "100px",
    threshold: 1,
  }
);
lazyImages.forEach((img) => observer.observe(img));
