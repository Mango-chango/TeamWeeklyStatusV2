# Team Weekly Status Backend

This project serves as the backend layer of the Team Weekly Status application, providing RESTful APIs for managing weekly status reports, team management, and authentication. The application is built using **ASP.NET Core in .NET 8** and follows the principles of **Clean Architecture** to ensure a scalable, maintainable, and testable codebase.

## Table of Contents

- [Development Setup](#development-setup)
- [Available Scripts](#available-scripts)
- [Environment Variables](#environment-variables)
- [Database Configuration](#database-configuration)
- [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Development Setup

### Prerrequisites

Before you begin, ensure you have met the following requirements:

- **.NET SDK** (version 8.0 or higher)
- **SQL Server** or another supported database
- **Visual Studio** or **Visual Studio Code**
- **Entity Framework Core CLI** (for migrations)

### Getting Started

To get started with the project, follow these steps:

1. **Clone the repository:**

   ```sh
   git clone https://github.com/linogvallejo/TeamWeeklyStatusV2.git
   cd TeamWeeklyStatusV2
   cd (TeamWeeklyStatus.Api|TeamWeeklyStatus.Application|TeamWeeklyStatus.Core|TeamWeeklyStatus.Infrastructure|TeamWeeklyStatus.Tests)

   ```

2. **Set up environment variables:**

   - Create a `appsettings.json` file in the root directory.
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

## Environment Variables

The application uses configuration settings from `appsettings.json`. Key settings include:

- **ConnectionStrings**:

  ```json
  "ConnectionStrings": {
    "AzureSqlConnection": "Data Source=mangochango.database.windows.net;Initial Catalog=team-weekly-status;User ID=linovallejo;Password=your_password;Connect Timeout=60;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
  }
  ```

- **GoogleClientId**: "91039693581-hprbpbenb5fjgm5ccq73d72cpu1o4ptf.apps.googleusercontent.com"

- **GoogleClientSecret**: "your_client_secret"

  ```json
  "Notifications": {
    "Configuration": {
      "SmtpServer": "smtp.gmail.com",
      "SmtpPort": 587,
      "SenderEmail": "lgarcia@mangochango.com",
      "SenderName": "Weekly Status Police",
      "Password": "your_password",
      "SlackWebhookUrl": "https://hooks.slack.com/services/T06G5J1J2/B06G5J1J2/XXXXXXXXXXXXXXXXXXXXXXXX",
      "SlackChannel": "#team-weekly-status"
    },
    "Templates": {
      "Email": {
        "PostWeeklyStatus": "Hey {0}! It's time to post your weekly status.",
        "PostWeeklyStatusSubject": "Weekly Status Update",
        "SendWeeklyStatusReport": "Hey {0}! You have been chosen to provide the team's weekly report to the client this week; kindly do so as soon as possible. Kudos to you Chango!",
        "SendWeeklyStatusReportSubject": "Weekly Status Report"
      },
      "Slack": {
        "PostWeeklyStatus": "Hey {0}! It's time to post your weekly status.",
        "SendWeeklyStatus": "Hey {0}! You have been chosen to provide the team's weekly report to the client this week; kindly do so as soon as possible. Kudos to you Chango!"
      }
    }    
  }
  ```

## Database Configuration

The application uses **Entity Framework Core** for data access. Ensure that your connection string in `appsettings.json` is correctly set up to connect to your database.

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

  Located in `TeamWeeklyStatus.Application.Tests`. Run using:

  ```sh
  dotnet test TeamWeeklyStatus.Application.Tests
  ```

  Located in `TeamWeeklyStatus.WebApi.Tests`. Run using:

  ```sh
  dotnet test TeamWeeklyStatus.WebApi.Tests
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

- **Project Link**: [https://github.com/linogvallejo/TeamWeeklyStatusV2.git](https://github.com/linogvallejo/TeamWeeklyStatusV2.git)
- **Author**: Lino Garcia Vallejo
- **Email**: [lgarcia@mangochango.com](mailto:lgarcia@mangochango.com)

---
