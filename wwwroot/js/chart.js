﻿<script>
    var ctx = document.getElementById('appointmentsChart').getContext('2d');

    var baseGreyColor = 'rgba(128, 128, 128, 1)';

    var greyOpacities = [0.9, 0.9, 0.9, 0.9, 0.9, 0.9, 0.9];

    var backgroundColors = greyOpacities.map(opacity => `rgba(128, 128, 128, ${opacity})`);

    // Use full opacity for the border color
    var borderColors = backgroundColors.map(color => color.replace(/0\.\d+/g, '1'));

    var appointmentsChart = new Chart(ctx, {
        type: 'bar',
    data: {
        labels: @Html.Raw(Json.Serialize(Model.Select(ac => ac.Date.ToString("MM/dd")).ToList())), // Date Labels
    datasets: [{
        label: 'Total Appointments',
                data: @Html.Raw(Json.Serialize(Model.Select(ac => ac.Count).ToList())),                 // Appointments Data
    borderColor: borderColors,
    backgroundColor: backgroundColors,
    borderWidth: 2
            }]
        },
    options: {
        scales: {
        x: {
        ticks: {
        autoSkip: false,  // Ensure labels are not skipped
    maxRotation: 0,   // Force labels to display horizontally
    minRotation: 0,
                    }
                },
    y: {
        beginAtZero: true
                }
            },
    plugins: {
        legend: {
        display: true,      // Show the legend if needed
    position: 'top'
                },
    tooltip: {
        enabled: true       // Enable tooltips
                }
            }
        }
    });
</script>