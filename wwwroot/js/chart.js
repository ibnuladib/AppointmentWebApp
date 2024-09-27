﻿    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script>
            var ctx = document.getElementById('appointmentsChart').getContext('2d');
        var appointmentsChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: @Html.Raw(Json.Serialize(Model.Select(ac => ac.Date.ToString("MM/dd")).ToList())), // Corrected Date Format
                datasets: [{
                    label: 'Total Appointments',
                    data: @Html.Raw(Json.Serialize(Model.Select(ac => ac.Count).ToList())),
                    borderColor: 'rgba(75, 192, 192, 1)',
                    backgroundColor: 'rgba(75, 192, 192, 0.2)',
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    x: {
                        ticks: {
                            autoSkip: false, // Ensure labels are not skipped
                            maxRotation: 0,  // Force labels to display horizontally
                            minRotation: 0,  // Force labels to display horizontally
                        }
                    },
                    y: {
                        beginAtZero: true
                    }
                },
                plugins: {
                    legend: {
                        display: true, // Show the legend if needed
                        position: 'top'
                    },
                    tooltip: {
                        enabled: true
                    }
                }
            }
        });
    </script>