# Dotnet API Project

This Dotnet API project was developed as a learning exercise to build RESTful APIs using the Dotnet framework. It utilizes MS SQL Server for database management, implements stored procedures for database operations, and includes an authentication controller along with an authentication helper class for password hashing, salting, and token creation. The API features a Posts controller where employees can post titles and content, and it includes a keyword search functionality. The project also incorporates the use of a mapper for data manipulation.

## Features

- **Authentication:**

  - Authentication controller for managing user authentication.
  - Auth helper class for password management and token creation.

- **Posts Controller:**

  - Allows employees to post titles and content.
  - Includes keyword search functionality.

- **Data Handling:**

  - Utilizes mapper for data manipulation.
  - Implements both Dapper and Entity Framework for the user controller.

- **Database Operations:**
  - MS SQL Server used for database management.
  - Includes stored procedures for efficient database operations.
  - Used Dynamic Parameters to prevent from SQL Injection. 

## Prerequisites

- Dotnet SDK installed on your system.
- MS SQL Server installed and running.
- Visual Studio or any preferred IDE for Dotnet development.

## Setup

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/yourusername/dotnet-api-project.git
   ```

2. **Database Setup:**
   - Execute the SQL seed script to initialize the database.
3. **Build and Run:**
   - Open the solution in your IDE.
   - Build and run the solution.
4. **API Endpoints:**
   - Once the API is running, you can explore the available endpoints using tools like Postman or Swagger.

## Usage

- Authenticate users using the provided authentication controller.
- Use the Posts controller to create, retrieve, update, and delete posts.
- Utilize keyword search functionality to find specific posts.
- Experiment with both Dapper and Entity Framework functionalities in the user controller.

## Deployment

- Deployment scripts are included in the project for easy deployment to various environments.
