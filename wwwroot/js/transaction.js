<script>
    function openPopup() {
        document.getElementById("popup-bg").style.display = "flex";
        }

    function closePopup() {
        document.getElementById("popup-bg").style.display = "none";
        }

    function showPaymentDetails() {
            const paymentType = document.getElementById("payment-type").value;

    // Hide all payment detail sections
    document.getElementById("mobile-banking-details").style.display = "none";
    document.getElementById("card-details").style.display = "none";

    // Show relevant details based on the selected payment type
    if (paymentType === "mobile-banking") {
        document.getElementById("mobile-banking-details").style.display = "block";
            } else if (paymentType === "card") {
        document.getElementById("card-details").style.display = "block";
            }
        }
    const transactionId = '@Model.Id';
    function submitPayment(event) {
        event.preventDefault();  // Prevent form submission

    const transaction = {
        Status: 'Paid'
            };

    const url = `/Transactions/UpdateTransactionStatus?Id=${transactionId}`;

    fetch(url, {
        method: 'PUT',
    headers: {
        'Content-Type': 'application/json',
    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
    body: JSON.stringify(transaction)
            })
                .then(response => response.text())  // Use `text()` to parse the response as string
                .then(data => {
                    if (data === "Transaction status updated successfully") {
        closePopup();  // Close the payment popup
    document.getElementById("success-bg").style.display = "flex";  // Show success message

                        // Set a short timeout to allow the user to see the success message, then reload the page
                        setTimeout(() => {
        location.reload();  // Reload the page to reflect updated status
                        }, 1000);  // Adjust the timeout as necessary
                    } else {
        alert("Failed to process payment. Please try again.");
                    }
                })
                .catch(error => console.error("Error updating transaction status:", error));
        }






    function closeSuccessPopup() {
        document.getElementById("success-bg").style.display = "none"; // Close success message
        }
</script>