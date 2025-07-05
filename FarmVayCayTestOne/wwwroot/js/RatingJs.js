document.addEventListener('DOMContentLoaded', function () {
    const stars = document.querySelectorAll('.star');
    const ratingInput = document.getElementById('ratingInput');
    let currentRating = 0;

    stars.forEach(star => {
        // عند الضغط على نجمة
        star.addEventListener('click', function () {
            currentRating = parseInt(this.getAttribute('data-rating'));
            ratingInput.value = currentRating;

            updateStarColors(currentRating);
        });

        // تأثير الـ hover
        star.addEventListener('mouseover', function () {
            const hoverRating = parseInt(this.getAttribute('data-rating'));
            updateStarColors(hoverRating, true);
        });

        // إزالة الـ hover عند الخروج
        star.addEventListener('mouseout', function () {
            updateStarColors(currentRating);
        });
    });

    function updateStarColors(rating, isHover = false) {
        stars.forEach(star => {
            const starRating = parseInt(star.getAttribute('data-rating'));
            if (starRating <= rating) {
                star.classList.add(isHover ? 'hover' : 'active');
                star.classList.remove(isHover ? 'active' : 'hover');
            } else {
                star.classList.remove('hover', 'active');
            }
        });
    }

    // أزرار Yes / No
    const ynButtons = document.querySelectorAll('.yn-btn');
    ynButtons.forEach(button => {
        button.addEventListener('click', function () {
            const buttonGroup = this.parentElement;
            buttonGroup.querySelectorAll('.yn-btn').forEach(btn => btn.classList.remove('selected'));
            this.classList.add('selected');
        });
    });
});
