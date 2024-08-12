# Employee Tracking System Project

## Project Purpose

**Employee Tracking System Project** is a platform designed to efficiently manage tasks and monitor employee performance in the workplace. This system includes three types of users with different levels of authority:

1. **Manager**:
   - Can assign tasks to employees and monitor their progress.
   - Can approve or reject employee leave requests.
   - Can create announcements.

2. **Employee**:
   - Can accept or decline assigned tasks.
   - Can complete tasks and provide comments on them.
   - Can track their own tasks.
   - Can send emails to other employees within the system.

3. **Admin**:
   - Has access to all data within the system and can modify it.
   - Can manage personnel, add/remove units, handle logs, create meal menus, and perform various administrative tasks.
   - Can create announcements like managers.

**Note:** There can only be one Admin in the system, and each unit can only have one manager.

## Features

- **Task Management**: Tasks assigned by managers can be accepted and completed by employees.
- **Leave Management**: Employee leave requests can be approved or rejected by managers.
- **Announcements**: Announcements can be created by managers and admins.
- **Mail System**: Employees can send emails to each other within the system.
- **Unit and Personnel Management**: Admin can manage units and personnel.
- **Meal Menu Creation**: Admin can create daily meal menus.

## Technologies Used

- **Backend**: .NET MVC, MSSQL, SQL
- **Frontend**: Bootstrap, JavaScript, HTML, CSS, Razor
- **Charts**: chart.js

## Installation

To run the project on your local machine, follow these steps:

1. **Clone the repository:**

   ```bash
   git clone https://github.com/lionpeace/Employee-Tracking-System-Project.git
   cd Employee-Tracking-System-Project

2. **Set up the Database:**

   - Create the necessary tables in MSSQL Server.
   - Configure the database connection in the **appsettings.json** file.

3. **Install the Required Dependencies:**

   ```bash
   dotnet restore

4. **Run the Project:**

   ```bash
   dotnet run

## Screenshots


