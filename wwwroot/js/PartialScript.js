<>
<script>
    function toggleViewComment(doctorId) {
        const viewComment = document.getElementById('viewComment');
        const ratingForm = document.getElementById('ratingForm');

        if (viewComment.style.display === 'none') {
            fetch(`/Review/GetRecentComments?doctorId=${doctorId}`)
                .then(response => response.text())
                .then(htmlContent => {
                    document.getElementById('commentContent').innerHTML = htmlContent;
                    viewComment.style.display = 'block';
                    ratingForm.style.display = 'none';
                })
                .catch(error => {
                    console.error('Error loading comments:', error);
                    document.getElementById('commentContent').innerHTML = '<p>Error loading comments.</p>';
                });
        } else {
            viewComment.style.display = 'none';
        }
    }

    const doctorRatingComponent = (() => {
        const doctorId = '@doctorId';
        const patientId = '@patientId';
        let selectedRating = 0;

        const rateDoctorBtn = document.getElementById('rateDoctorBtn');
        const ratingForm = document.getElementById('ratingForm');
        const starRating = document.getElementById('starRating');
        const commentInput = document.getElementById('commentInput');
        const submitRatingBtn = document.getElementById('submitRatingBtn');
        const messageAlert = document.getElementById('messageAlert');

        function init() {
            createStars();
            attachEventListeners();
        }

        function createStars() {
            for (let i = 1; i <= 5; i++) {
                const star = document.createElement('span');
                star.innerHTML = '&#9733;';
                star.className = 'star';
                star.style.fontSize = '24px';
                star.style.color = 'gray';
                star.style.cursor = 'pointer';
                star.dataset.value = i;
                starRating.appendChild(star);
            }
        }

        function attachEventListeners() {
            rateDoctorBtn.addEventListener('click', toggleRatingForm);
            starRating.addEventListener('click', handleStarClick);
            submitRatingBtn.addEventListener('click', submitRating);
        }

        function toggleRatingForm() {
            ratingForm.style.display = ratingForm.style.display === 'none' ? 'block' : 'none';
            rateDoctorBtn.textContent = ratingForm.style.display === 'none' ? 'Rate Doctor' : 'Cancel';
            document.getElementById('viewComment').style.display = 'none'; // Hide comment view
        }

        function handleStarClick(event) {
            if (event.target.classList.contains('star')) {
                selectedRating = parseInt(event.target.dataset.value);
                updateStars();
            }
        }

        function updateStars() {
            starRating.querySelectorAll('.star').forEach(star => {
                star.style.color = parseInt(star.dataset.value) <= selectedRating ? 'gold' : 'gray';
            });
        }

        function submitRating() {
            if (selectedRating === 0) {
                showMessage('Please select a rating.', 'alert-warning');
                return;
            }

            if (!doctorId || !patientId) {
                showMessage('Doctor or patient ID missing.', 'alert-warning');
                return;
            }

            const review = {
                DoctorId: doctorId,
                PatientId: patientId,
                Rating: selectedRating,
                Comment: commentInput.value,
            };

            fetch('/Review/Create', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value},
                body: JSON.stringify(review)
            })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    showMessage('Thank you for your review!', 'alert-success');
                    setTimeout(() => {
                        window.location.reload();
                    }, 3000); // Wait for 3 seconds before reloading
                    resetForm();
                } else {
                    showMessage('Error submitting review: ' + data.message, 'alert-danger');
                }
            })
            .catch(error => {
                console.error('Error submitting review:', error);
                showMessage('Error submitting your review. Please try again.', 'alert-danger');
            });
        }

        function showMessage(message, className) {
            messageAlert.textContent = message;
            messageAlert.className = `alert ${className} mt-2`;
            messageAlert.style.display = 'block';
        }

        function resetForm() {
            selectedRating = 0;
            updateStars();
            commentInput.value = '';
            toggleRatingForm();
        }

        return {
            init
        };
    })();

    document.addEventListener('DOMContentLoaded', doctorRatingComponent.init);
</script>

<style>
    .star {
        transition: color 0.2s;
    }

    .star:hover {
        color: gold;
    }

    .custom-margin-right {
        margin-right: 15px; /* Adjust the value as needed */
    }
</style>
</>