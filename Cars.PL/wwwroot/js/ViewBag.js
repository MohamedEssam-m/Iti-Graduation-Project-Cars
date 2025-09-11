
    document.addEventListener("DOMContentLoaded", function () {
        const alerts = document.querySelectorAll(".toast-alert");

        alerts.forEach(alert => {
        // Auto remove after 5s
        setTimeout(() => {
            alert.style.transition = "opacity 0.5s";
            alert.style.opacity = "0";
            setTimeout(() => alert.remove(), 500);
        }, 5000);

            // Close on button click
            alert.querySelector(".close-btn").addEventListener("click", () => {
        alert.remove();
            });
        });
    });

