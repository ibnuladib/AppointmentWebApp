# Baymax Care

Welcome to Baymax Care, a comprehensive doctor's appointment system designed to facilitate seamless interactions between patients and healthcare providers. Our application streamlines the appointment scheduling process, ensuring that patients have easy access to medical care while allowing doctors to manage their schedules effectively.
Demonstration video: https://www.youtube.com/watch?v=tRVjEe338gE

## CaseStudy
In our country, the healthcare system is quite inefficient as it wastes time as at times patients need to come to the clinic or hospital to book an appointment and wait for it, in other scenarios it also wastes resources too as clerks are needed to manage appointments, including making calls or receiving calls in queries to doctors or appointments. So, our goal was to make the process entirely online and make booking more efficient by reducing the time, effort and resources it takes to book an appointment. 

In our system, Patients can sign up with all their necessary details and are authenticated using their email address. They can give their past medical records and details and update their profile. They can search for doctors and see their past reviews. Once they find the one they want to book an appointment with. They can then create an appointment at the available time slots. A transaction responding to the appointment will be created. The Patient can view the transaction and pay it. The doctor will receive a notification concerning the Appointment and can prepare for it via looking through the Patient’s medical history. 
An Administrator will manage all records of Patients, Doctors and Reviews to moderate through it when deemed necessary. 
All these actions will be done through a user-friendly interface. 

## Technoloy Used
- C#
- ASP.NET CORE MVC
- Libraries: Identity, Signal R
- SQL
- JavaScript
- Bootstrap CSS, html
 
## Features

### 1. Patient Booking System
- **User-Friendly Interface**: Patients can easily navigate the application to find and book appointments with their preferred doctors.
- **Available Time Slots**: Patients can view the available time slots for each doctor, enabling them to select a time that fits their schedule.
- **Appointment Confirmation**: Once a patient books an appointment, they receive a confirmation notification via email or in-app, ensuring clarity and peace of mind.
- **Patient Dashboard**: Patients have a Dashboard to track their appointments and transactions status.

### 2. Doctor Dashboard
- **Appointment Management**: Doctors can view all their scheduled appointments in a user-friendly dashboard, allowing them to prepare for their patient visits.
- **Patient Details**: Doctors have access to essential patient information before appointments, including medical history and previous consultations, facilitating personalized care.
- **Time Slot Management**: Doctors can set their available time slots with shifts, making it easy to manage their schedules and availability for patients.
- **Performance Tracking**: The dashboard includes statistics on patient visits, reviews, and overall performance, helping doctors assess their service quality and improve where necessary.
- **Consultation Fees Management**: Doctors can set and update their consultation fees, allowing for better financial planning and transparency for patients.

### 3. User Role Management
- **Admin Role**: Administrators can manage the overall system, including adding or removing doctors, overseeing appointments, and handling user accounts. Admins have full access to all data and can generate reports on system usage.
- **Doctor Role**: Doctors can manage their profiles, update their availability, and access patient appointments. They can also respond to patient reviews, enhancing communication with patients.
- **Patient Role**: Patients can create accounts, book appointments, and leave reviews for their doctors. They can view their appointment history, manage their profile information, and view their consultation fees.

### 4. Review and Feedback System
- **Patient Reviews**: After appointments, patients can leave comments and ratings for their doctors, providing valuable feedback that can help improve service quality.
- **Rating System**: A rating system from 1 to 5 stars is there to allow patients to express their level of satisfaction clearly which can be updated later.
- **Visibility of Reviews**: Doctors can view their reviews, enabling them to respond to feedback and improve their services based on patient insights.
- **Aggregate Ratings**: The system calculates an average rating for each doctor based on patient reviews, helping future patients make informed decisions.

### 5. Payment Integration
- **Different Payment Methods**: The system supports various payment methods, ensuring secure financial transactions for booked appointments.
- **Transaction History**: Patients can view their transaction history, including details of payments made for consultations and any outstanding balances.
- **Financial Reports for Doctors**: Doctors can access reports on their earnings, helping them understand their financial performance within the platform.
- **Payment Status Tracking**: The system allows patients to track the payment status of their appointments (Paid or UnPaid).

### 6. Notifications and Log
- **In-App Notifications**: Users can view notifications directly within the app for a seamless experience.
- **Admin Log**: Admins will have a log page to check the most recent updates.

### 8. Analytics Dashboard
- **Admin Analytics**: Admins can view analytics related to user payments, appointment bookings, and overall hospital/clinic performance.
- **Doctor Analytics**: Doctors have access to personal analytics, including the number of patients seen, average ratings, and financial performance over time.

## MVC Architecture

Model-View-Controller (MVC) is a software architectural pattern commonly used in web applications similar to Monolythic Architecture. It separates the application into three interconnected components:
![image](https://github.com/user-attachments/assets/1a96750d-37d8-40dc-8540-b76ac5e1589b)  ![image](https://github.com/user-attachments/assets/c0776718-20d6-446f-aa76-304793662833)


- **Model**: Represents the application's data and business logic. In Baymax Care, this includes models like `ApplicationUser`, `Appointment`, `Review`, and `Transaction`, which define how data is stored and interacted with.
- **View**: Represents the user interface and displays the data from the model to the user. Views handle the presentation layer, allowing users to interact with the application.
- **Controller**: Acts as an intermediary between the Model and View. It processes user inputs, manipulates the model, and updates the view accordingly. Controllers handle the application's logic and direct the flow of data.

## Database Schemas
![image](https://github.com/user-attachments/assets/c2cbfeb9-f802-4080-a733-5aa4e6d71f69)

## Permissions Chart

![image](https://github.com/user-attachments/assets/1be3bf29-2fe8-417f-9184-609f6882cb36)

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

    
    **SQL Script:**
    There is also a SQL script file given in the folder SQL, with which you can initialize the local database

5. Run the application:
    ```bash
    dotnet run
    ```

6. Access the application at `http://localhost:[Port Number]`.

7. An Admin page is made by default; email - admin@admin.com and password - Test1234,

## Usage

- **For Patients**: Register or log in, then navigate to the "Search" section to find doctors and book appointments. Review your appointment history and leave feedback for your doctors.
- **For Doctors**: Log in to your account to view your appointment schedule, manage your availability, and respond to patient reviews. Track your performance and earnings through the analytics dashboard.

## Screenshots
![image](https://github.com/user-attachments/assets/508b7561-f5b4-4dc7-93c0-fa2416421df8)
![image](https://github.com/user-attachments/assets/ba5780ee-ed53-47b0-bf0c-5859db276d31)
![image](https://github.com/user-attachments/assets/1a279b1b-9d9c-426b-a05b-17ccc84b6c67)
![image](https://github.com/user-attachments/assets/b548d193-9dcf-48fc-8aaf-23f1fa6dc170)
![image](https://github.com/user-attachments/assets/734a3bd1-0920-41d6-a88c-47eaae4227aa)
![image](https://github.com/user-attachments/assets/440425b2-5bb2-4afb-af49-4199a8c708c5)
![image](https://github.com/user-attachments/assets/52723d7c-ed30-4280-8da2-a60badf3c178)
![image](https://github.com/user-attachments/assets/bfb19be9-91ca-4032-a9e0-6dc16cff360c)
![image](https://github.com/user-attachments/assets/d0b12fe2-076e-469a-a286-442badacc028)




## Contributing

Contributions are welcome! If you have suggestions for improvements or new features, please fork the repository and submit a pull request.
## Acknowledgements

- Thanks to the contributors and the community for their support.
- Special thanks to resources that helped in the development of this project.
