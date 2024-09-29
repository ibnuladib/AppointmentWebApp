<script>
    $(document).ready(function () {
            var dateInput = $('#appointmentDate');
    var availableTimesInput = $('#AvailableTimes');
    var startTimeInput = $('#startTimeInput');
    var startTimeDisplay = $('#startTimeDisplay');
    var dateOfAppointmentInput = $('#dateOfAppointmentInput');
    // Fetch available times for the selected date
    function fetchAvailableTimes() {
                var selectedDate = dateInput.val();
    var doctorId = $('#DoctorId').val();

    if (selectedDate) {
        $.ajax({
            url: '@Url.Action("GetAvailableTimes", "Appointments")',
            type: 'GET',
            data: {
                appointmentDate: selectedDate,
                doctorId: doctorId
            },
            success: function (data) {
                availableTimesInput.empty();
                if (data.length > 0) {
                    $.each(data, function (index, timeString) {
                        availableTimesInput.append($('<option></option>').val(timeString).html(timeString));
                    });
                } else {
                    availableTimesInput.append('<option>No available times</option>');
                }
            },
            error: function () {
                alert('Error retrieving times');
            }
        });
                } else {
        availableTimesInput.empty();
    availableTimesInput.append('<option>Select a date first</option>');
                }
            }

    // Update start time display and hidden input when a time is selected
    availableTimesInput.change(function () {
                var selectedTime = availableTimesInput.val();
    var selectedDate = dateInput.val();

    if (selectedTime && selectedDate) {
                    var appointmentDate = new Date(selectedDate);
    var timeParts = selectedTime.split(' ');
    var time = timeParts[0].split(':');
    var hours = parseInt(time[0]);
    var minutes = parseInt(time[1]);

    if (timeParts[1] === 'PM' && hours !== 12) hours += 12;
    if (timeParts[1] === 'AM' && hours === 12) hours = 0;

    appointmentDate.setHours(hours, minutes);

    // Format DateOfAppointment (date only)
    var formattedDate = appointmentDate.toISOString().split('T')[0];
    dateOfAppointmentInput.val(formattedDate);

    // Format StartTime (time only, as TimeSpan)
    var timeSpan = String(hours).padStart(2, '0') + ':' +
    String(minutes).padStart(2, '0') + ':00';
    startTimeInput.val(timeSpan);

    // Update display (optional)
    startTimeDisplay.val(appointmentDate.toLocaleString('en-GB', {hour12: false }));
                }
            });

    // Handle Create button click
    $('#submitButton').click(function () {
                var appointmentData = {
        DoctorId: $('#DoctorId').val(),
    PatientId: $('#PatientId').val(),
    DateOfAppointment: dateOfAppointmentInput.val(),
    StartTime: startTimeInput.val(),
    Symptoms: $('#Symptoms').val(),
    Amount: $('#totalAmount').val(),
                };

    $.ajax({
        url: '@Url.Action("CreateNew", "Appointments")',
    type: 'POST',
    contentType: 'application/json',
    data: JSON.stringify(appointmentData),
    success: function (response) {
        alert('Appointment created successfully!');
    window.location.href = '@Url.Action("Index", "Appointments")';
                    },
    error: function (xhr, status, error) {
        alert('Failed to create appointment.');
                    }
                });
            });

    dateInput.change(function () {
        fetchAvailableTimes();
            });
        });
</script>