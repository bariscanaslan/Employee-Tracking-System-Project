# Employee Tracking System Project

## WARNING! 

This project created for a Turkish employer. So, all the texts are Turkish. Most of the variables are created with a Turkish name. If you have treble with this situation please contact me anytime.
- My e-mail address: bariscanaslan@outlook.com
- My website: www.bariscanaslan.com


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

<div align="center">

### Login Page
<img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/1.png" alt="Login Page"/>

### Manager Home Page
<img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/2.png" alt="Manager Home Page"/>

### Task Assignment Page
<img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/3.png" alt="Task Assignment Page"/>

### Task Tracking Page
<img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/4.png" alt="Task Tracking Page"/>

### Manage Leave Page
<img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/5.png" alt="Manage Leave Page"/>

### Mail Inbox Page
<img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/6.png" alt="Mail Inbox Page"/>

### Send Mail Page
<img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/7.png" alt="Send Mail Page"/>

### Announcement Reading Page
<img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/8.png" alt="Announcement Reading Page"/>

### Meal Menu Page
<img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/9.png" alt="Meal Menu Page"/>

### Log Table Page
<img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/10.png" alt="Log Table Page"/>

### Unit Table Page
<img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/11.png" alt="Unit Table Page"/>

### Personnel Table Page
<img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/12.png" alt="Personnel Table Page"/>

### Employee Home Page
<img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/13.png" alt="Employee Home Page"/>

### Task Completion Page
<img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/14.png" alt="Task Completion Page"/>

### Task Tracking Page (Employee)
<img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/15.png" alt="Task Tracking Page (Employee)"/>

### Leave Request Page
<img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/16.png" alt="Leave Request Page"/>

### My Leave Page
<img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/17.png" alt="My Leave Page"/>

</div>


