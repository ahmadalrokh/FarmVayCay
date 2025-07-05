
    document.addEventListener("DOMContentLoaded", function () {
        const images = document.querySelectorAll(".slider-image");
    const prevButton = document.querySelector(".prev");
    const nextButton = document.querySelector(".next");
    let currentIndex = 0;

    function updateSlider() {
        images.forEach((img, index) => {
            img.style.display = index === currentIndex ? "block" : "none";
        });
        }

    prevButton.addEventListener("click", function () {
        currentIndex = (currentIndex - 1 + images.length) % images.length;
    updateSlider();
        });

    nextButton.addEventListener("click", function () {
        currentIndex = (currentIndex + 1) % images.length;
    updateSlider();
        });

    updateSlider(); 
    });

 