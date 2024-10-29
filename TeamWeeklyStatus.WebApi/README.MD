# Team Weekly Status Backend

Welcome to the **Team Weekly Status Backend** application! This project serves as the backend layer of the Team Weekly Status application, providing RESTful APIs for managing weekly status reports, team management, and authentication. The application is built using **ASP.NET Core** and follows the principles of **Clean Architecture** to ensure a scalable, maintainable, and testable codebase.

## Table of Contents

- [Features](#features)
- [Clean Architecture](#clean-architecture)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Available Scripts](#available-scripts)
- [Project Structure](#project-structure)
- [Technologies Used](#technologies-used)
- [Environment Variables](#environment-variables)
- [Database Configuration](#database-configuration)
- [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Features

- **User Authentication**: Secure login and token-based authentication.
- **Weekly Status Management**: APIs to create, read, update, and delete weekly status reports.
- **Team Management**: Manage teams and assign users to teams.
- **Administrative Functions**: Admin-level APIs for user and team management.
- **Role-Based Access Control**: Secure endpoints based on user roles.
- **RESTful API Design**: Follows REST conventions for API design.
- **Clean Architecture**: Organized codebase promoting separation of concerns.
- **Error Handling**: Consistent error responses and logging.

## Clean Architecture

This application follows the **Clean Architecture** principles proposed by Robert C. Martin (Uncle Bob). The main objectives are:

- **Independence of Frameworks**: The architecture does not depend on the existence of specific frameworks or libraries. This allows you to use frameworks as tools rather than forcing your application to fit the framework.
- **Testability**: Business rules can be tested without the UI, database, web server, or any other external element.
- **UI Independence**: The UI can change easily without affecting the core logic.
- **Database Independence**: You can swap out databases without changing the business rules.
- **External Agency Independence**: The business rules are not bound to external services.

### Layers

1. **Core (Domain)**: Contains business entities and domain logic.
2. **Application**: Contains application services and interfaces.
3. **Infrastructure**: Contains data access implementations and external services.
4. **Presentation**: Contains the API controllers and presentation logic.

This separation ensures a clear division of responsibilities, making the application more maintainable and scalable.

## Prerequisites

Before you begin, ensure you have met the following requirements:

- **.NET SDK** (version 5.0 or higher)
- **SQL Server** or another supported database
- **Visual Studio** or **Visual Studio Code**
- **Entity Framework Core CLI** (for migrations)
- **Access to the frontend application** (for full-stack integration)

## Getting Started

To get started with the project, follow these steps:

1. **Clone the repository:**

   ```sh
   git clone https://github.com/your-repo/team-weekly-status-backend.git
   cd team-weekly-status-backend
   ```

2. **Set up environment variables:**

   - Create a `appsettings.Development.json` file in the root directory.
   - Add the necessary configuration settings (see [Environment Variables](#environment-variables)).

3. **Restore NuGet packages:**

   ```sh
   dotnet restore
   ```

4. **Apply database migrations:**

   ```sh
   dotnet ef database update
   ```

5. **Run the application:**

   ```sh
   dotnet run
   ```

6. **API Documentation:**

   - The API uses **Swagger** for documentation.
   - Navigate to `https://localhost:5001/swagger` to view the API documentation.

## Available Scripts

In the project directory, you can run:

- **`dotnet build`**: Builds the project and all of its dependencies.
- **`dotnet run`**: Runs the application.
- **`dotnet test`**: Runs the tests for the application.
- **`dotnet ef migrations add <MigrationName>`**: Adds a new database migration.
- **`dotnet ef database update`**: Updates the database to the latest migration.

## Project Structure

```
team-weekly-status-backend/
├── src/
│   ├── TeamWeeklyStatus.Api/             # Presentation layer (API)
│   ├── TeamWeeklyStatus.Application/     # Application layer
│   ├── TeamWeeklyStatus.Core/            # Core domain layer
│   ├── TeamWeeklyStatus.Infrastructure/  # Infrastructure layer
│   └── TeamWeeklyStatus.Tests/           # Test projects
├── tests/
│   ├── TeamWeeklyStatus.UnitTests/       # Unit tests
│   └── TeamWeeklyStatus.IntegrationTests/# Integration tests
├── appsettings.json                      # Configuration settings
├── appsettings.Development.json          # Development-specific settings
├── Program.cs                            # Entry point of the application
├── Startup.cs                            # Configuration and middleware
├── README.md                             # Project documentation
└── ...                                   # Other configuration and solution files
```

## Technologies Used

- **ASP.NET Core**: Framework for building web applications and APIs.
- **Entity Framework Core**: Object-relational mapper (ORM) for data access.
- **SQL Server**: Primary database (can be swapped due to Clean Architecture).
- **AutoMapper**: Object-object mapping.
- **FluentValidation**: Validation of models.
- **JWT Bearer Authentication**: Secure token-based authentication.
- **Swagger**: API documentation.
- **XUnit**: Testing framework.
- **Moq**: Mocking framework for unit tests.
- **Serilog**: Logging library.

## Environment Variables

The application uses configuration settings from `appsettings.json` and `appsettings.Development.json`. Key settings include:

- **ConnectionStrings**:

  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TeamWeeklyStatusDb;User Id=your_user;Password=your_password;"
  }
  ```

- **JWT Settings**:

  ```json
  "JwtSettings": {
    "Secret": "YourSuperSecretKey",
    "Issuer": "TeamWeeklyStatus",
    "Audience": "TeamWeeklyStatusUsers",
    "ExpiryMinutes": 60
  }
  ```

- **Logging**:

  ```json
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
  ```

## Database Configuration

The application uses **Entity Framework Core** for data access. Ensure that your connection string in `appsettings.json` or `appsettings.Development.json` is correctly set up to connect to your database.

### Applying Migrations

To apply database migrations and update your database schema:

```sh
dotnet ef database update
```

### Creating a New Migration

If you make changes to your data models and need to create a new migration:

```sh
dotnet ef migrations add YourMigrationName
```

## Testing

The project includes both unit tests and integration tests to ensure code quality.

- **Unit Tests**:

  Located in `tests/TeamWeeklyStatus.UnitTests`. Run using:

  ```sh
  dotnet test tests/TeamWeeklyStatus.UnitTests
  ```

- **Integration Tests**:

  Located in `tests/TeamWeeklyStatus.IntegrationTests`. Run using:

  ```sh
  dotnet test tests/TeamWeeklyStatus.IntegrationTests
  ```

## Contributing

Contributions are welcome! Please follow these steps:

1. **Fork the repository**.

2. **Create a new branch**:

   ```sh
   git checkout -b feature/your-feature-name
   ```

3. **Make your changes**.

4. **Run tests** to ensure nothing is broken:

   ```sh
   dotnet test
   ```

5. **Commit your changes**:

   ```sh
   git commit -m "Add some feature"
   ```

6. **Push to the branch**:

   ```sh
   git push origin feature/your-feature-name
   ```

7. **Open a Pull Request**.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Contact

- **Project Link**: [https://github.com/your-repo/team-weekly-status-backend](https://github.com/your-repo/team-weekly-status-backend)
- **Author**: Your Name
- **Email**: [youremail@example.com](mailto:youremail@example.com)

---

Feel free to reach out if you have any questions or need assistance setting up the project. Happy coding!