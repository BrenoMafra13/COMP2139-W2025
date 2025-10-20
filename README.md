# 🧩 Project Management Platform

A full-featured **web application** built with **ASP.NET MVC** and **PostgreSQL**, designed to manage projects and tasks efficiently.  
The platform follows the **Model-View-Controller (MVC)** design pattern to ensure clean architecture, scalability, and maintainability.

## 🚀 Overview

This project enables teams to create, track, and manage projects and tasks in a collaborative environment.  
It includes features such as user role management, customizable profiles, and CRUD operations integrated with a PostgreSQL database.

🎥 **Demo Videos:**  
- [Task management Demo – YouTube #1](https://www.youtube.com/watch?v=ajSPwLKT3EI)  
- [User Roles and Permissions – YouTube #2](https://www.youtube.com/watch?v=NBVjLgryr_s)  
- [Project Final Version – YouTube #3](https://www.youtube.com/watch?v=be8gfvCMi5Q)

## 🧠 Features

- 🗂️ **Project & Task Management:** Create, search, and organize projects and tasks by name.  
- 👤 **User Profiles:** Editable profile with name, photo, phone, and email customization.  
- 🔐 **Role-Based Access Control (RBAC):** Admin, Manager, Executor, and Tester roles with specific permissions.  
- 💾 **Database Integration:** Full CRUD operations via PostgreSQL.  
- 🧱 **Clean Architecture:** Multiple `.cs` Controllers/Models and `.cshtml` Views for separation of concerns.  
- ⚙️ **Scalable Design:** Built with best practices for performance and maintainability.  

## 🧰 Tech Stack

| Category | Technologies |
|-----------|---------------|
| **Frontend** | HTML5, CSS3, JavaScript |
| **Backend** | C#, ASP.NET MVC |
| **Database** | PostgreSQL |
| **Architecture** | MVC Pattern |
| **Other** | Entity Framework, LINQ |

## ⚙️ Setup & Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/BrenoMafra13/COMP2139-W2025.git
   cd COMP2139-W2025
2. **Open the project in Visual Studio (2022 recommended).**

3. **Configure database connection:**
   Update the connection string in appsettings.json or Web.config with your PostgreSQL credentials.

4. **Run migrations (if applicable):**
   ```bash
    dotnet ef database update


5. **Start the project:**
    Press Ctrl + F5 or run the solution from Visual Studio.

## 🧪 Testing
- All controllers and routes tested manually using Postman.
- Verified CRUD operations, authentication, and RBAC functionality.
- Input validation and error handling tested across all key features.

## 🏗️ Architecture

**The system follows the MVC pattern:**
- Model: Represents business logic and database entities.
- View: Handles user interface and data presentation using .cshtml Razor pages.
- Controller: Coordinates requests between model and view, handling routing and input logic.

## 📚 Skills & Concepts Demonstrated
- ASP.NET MVC Framework
- PostgreSQL Integration
- Role-Based Access Control (RBAC)
- Clean Architecture & SOLID Principles
- Entity Framework & LINQ
- Full-Stack Web Development
- RESTful API Concepts
- UI/UX Design and Usability Principles

## 📄 License

This project is for educational and demonstration purposes.
© 2025 Breno Lopes Mafra – All rights reserved.

## 🔗 GitHub Repository: 
https://github.com/BrenoMafra13/COMP2139-W2025
