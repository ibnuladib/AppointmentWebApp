# Baymax Care

Welcome to Baymax Care, a comprehensive doctor's appointment system designed to facilitate seamless interactions between patients and healthcare providers. Our application streamlines the appointment scheduling process, ensuring that patients have easy access to medical care while allowing doctors to manage their schedules effectively.

## Features

### 1. Patient Booking System
- **User-Friendly Interface**: Patients can easily navigate the application to find and book appointments with their preferred doctors.
- **Available Time Slots**: Patients can view the available time slots for each doctor, enabling them to select a time that fits their schedule.
- **Appointment Confirmation**: Once a patient books an appointment, they receive a confirmation notification via email or in-app, ensuring clarity and peace of mind.
- **Cancellation and Rescheduling**: Patients can cancel or reschedule their appointments easily, offering flexibility in managing their healthcare.
- **Medical History Submission**: Patients can submit their medical history during the booking process, allowing doctors to review important information ahead of the appointment.

### 2. Doctor Dashboard
- **Appointment Management**: Doctors can view all their scheduled appointments in a user-friendly dashboard, allowing them to prepare for their patient visits.
- **Patient Details**: Doctors have access to essential patient information before appointments, including medical history and previous consultations, facilitating personalized care.
- **Time Slot Management**: Doctors can set their available time slots, making it easy to manage their schedules and availability for patients.
- **Performance Tracking**: The dashboard includes statistics on patient visits, reviews, and overall performance, helping doctors assess their service quality and improve where necessary.
- **Consultation Fees Management**: Doctors can set and update their consultation fees, allowing for better financial planning and transparency for patients.

### 3. User Role Management
- **Admin Role**: Administrators can manage the overall system, including adding or removing doctors, overseeing appointments, and handling user accounts. Admins have full access to all data and can generate reports on system usage.
- **Doctor Role**: Doctors can manage their profiles, update their availability, and access patient appointments. They can also respond to patient reviews, enhancing communication with patients.
- **Patient Role**: Patients can create accounts, book appointments, and leave reviews for their doctors. They can view their appointment history, manage their profile information, and view their consultation fees.

### 4. Review and Feedback System
- **Patient Reviews**: After appointments, patients can leave reviews and ratings for their doctors, providing valuable feedback that can help improve service quality.
- **Rating System**: The application includes a rating system from 1 to 5 stars, allowing patients to express their level of satisfaction clearly.
- **Visibility of Reviews**: Doctors can view their reviews, enabling them to respond to feedback and improve their services based on patient insights.
- **Aggregate Ratings**: The system calculates an average rating for each doctor based on patient reviews, helping future patients make informed decisions.

### 5. Payment Integration
- **Secure Transactions**: The system supports various payment methods, ensuring secure financial transactions for booked appointments.
- **Transaction History**: Patients can view their transaction history, including details of payments made for consultations and any outstanding balances.
- **Financial Reports for Doctors**: Doctors can access reports on their earnings, helping them understand their financial performance within the platform.
- **Payment Status Tracking**: The system allows patients to track the payment status of their appointments (Paid, Unpaid, or Pending).

### 6. Notifications and Reminders
- **Email Notifications**: Patients and doctors receive email notifications for appointment confirmations, cancellations, and reminders.
- **In-App Notifications**: Users can view notifications directly within the app for a seamless experience.

### 7. Responsive Design
- **Mobile Compatibility**: The application is designed to be responsive, ensuring that patients and doctors can access it from various devices, including smartphones and tablets.
- **Intuitive User Experience**: The layout and navigation are optimized for ease of use, making it simple for all users to find the information they need quickly.

### 8. Analytics Dashboard
- **Admin Analytics**: Admins can view analytics related to user registrations, appointment bookings, and overall system performance.
- **Doctor Analytics**: Doctors have access to personal analytics, including the number of patients seen, average ratings, and financial performance over time.

## MVC Architecture

Model-View-Controller (MVC) is a software architectural pattern commonly used in web applications. It separates the application into three interconnected components:

- **Model**: Represents the application's data and business logic. In Baymax Care, this includes models like `ApplicationUser`, `Appointment`, `Review`, and `Transaction`, which define how data is stored and interacted with.
- **View**: Represents the user interface and displays the data from the model to the user. Views handle the presentation layer, allowing users to interact with the application.
- **Controller**: Acts as an intermediary between the Model and View. It processes user inputs, manipulates the model, and updates the view accordingly. Controllers handle the application's logic and direct the flow of data.

## Configuration with `appsettings.json`

The `appsettings.json` file is a configuration file used in ASP.NET Core applications. It allows you to manage application settings, including database connection strings, logging configurations, and various application-specific settings.

### Key Sections of `appsettings.json`

- **Connection Strings**: This section contains the connection string to your database. You will need to update this string to match your database configuration:
    ```json
    "ConnectionStrings": {
    "ApplicationDbContextConnection": "Server=(localdb)\\mssqllocaldb;Database=AppointmentWebAppDemo;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True"
  }
    ```
- **Logging**: This section defines how logging is handled within your application. You can adjust the log levels for different categories to control the verbosity of logging output:
    ```json
    "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"

  }
    ```
- **Application Settings**: You can add custom settings for your application, such as feature flags or external service configurations.

## Getting Started

To get a copy of the project up and running on your local machine for development and testing purposes, follow these steps:

### Prerequisites
- .NET SDK (version XX.X)
- Visual Studio or Visual Studio Code

### Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/ibnuladib/AppointmentWebApp.git
    cd AppointmentWebApp
    ```
2. Restore the dependencies:
    ```bash
    dotnet restore
    ```
3. Update the connection string in the `appsettings.json` file to match your database configuration.
    ```json
    "ConnectionStrings": {
    "ApplicationDbContextConnection": "Server=(localdb)\\mssqllocaldb;Database=AppointmentWebAppDemo;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True"
  }
    ```
4. Apply the database migrations:

    **Using .NET CLI:**
    ```bash
    dotnet ef migrations add InitialCreate
    dotnet ef database update
    ```

    **Using Visual Studio:**
    1. Open the **Package Manager Console** from **Tools > NuGet Package Manager > Package Manager Console**.
    2. Run the following command to add a migration:
        ```powershell
        Add-Migration InitialCreate
        ```
    3. Run the following command to update the database:
        ```powershell
        Update-Database
        ```

5. Run the application:
    ```bash
    dotnet run
    ```

6. Access the application at `http://localhost:5000`.

## Usage

- **For Patients**: Register or log in, then navigate to the "Search" section to find doctors and book appointments. Review your appointment history and leave feedback for your doctors.
- **For Doctors**: Log in to your account to view your appointment schedule, manage your availability, and respond to patient reviews. Track your performance and earnings through the analytics dashboard.

## Contributing

Contributions are welcome! If you have suggestions for improvements or new features, please fork the repository and submit a pull request.
## Acknowledgements

- Thanks to the contributors and the community for their support.
- Special thanks to resources that helped in the development of this project.
