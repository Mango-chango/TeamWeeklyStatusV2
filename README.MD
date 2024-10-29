# Team Weekly Status

Welcome to the **Team Weekly Status ** application! This project provides a full-stack solution for managing weekly status reports for teams. The application consists of two main parts: the **backend** (API) and the **frontend** (client application). The backend is built using **ASP.NET Core** and follows the principles of **Clean Architecture** to ensure a scalable, maintainable, and testable codebase. The frontend is built using **ReactJs** and **TypeScript** to provide a modern and responsive user interface.  
The frontend is a separate project and can be found in the `team-weekly-status-front` directory. The two projects are designed to work together to provide a seamless user experience. The frontend application consumes the backend APIs to perform CRUD operations on weekly status reports, manage teams, and user authentication. The backend provides secure endpoints for managing weekly status reports, team management, and user authentication. The application is designed to be flexible and extensible, allowing for easy integration with other systems and services. The application is built using the latest technologies and best practices to ensure a high-quality user experience. 


## Table of Contents

- [Features](#features)
- [Clean Architecture](#clean-architecture)
- [Prerequisites](#prerequisites)
- [Solution Structure](#solution-structure)
- [Technologies Used](#technologies-used)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Features

- **User Authentication**: Secure login through Google authentication.
- **Weekly Status Management**: APIs to create, read, update, and delete weekly status reports.
- **Team Management**: Manage teams and assign users to teams.
- **Administrative Functions**: Admin-level APIs and UI for user and team management.
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

- **.NET SDK** (version 8.0 or higher)
- **SQL Server** or another supported database
- **Visual Studio** or **Visual Studio Code**
- **Entity Framework Core CLI** (for migrations)
- **Access to the frontend application** (for full-stack integration)

## Solution Structure

```
TeamWeeklyStatusV2/
├── src/
│   ├── team-weekly-status-front/         # Frontend (ReactJs)
│   ├── TeamWeeklyStatus.WebApi/          # Presentation layer (API)
│   ├── TeamWeeklyStatus.Application/     # Application layer
│   ├── TeamWeeklyStatus.Domain/          # Core domain layer
│   ├── TeamWeeklyStatus.Infrastructure/  # Infrastructure layer
│   └── TeamWeeklyStatus.sln              # Solution file
├── tests/
│   ├── TeamWeeklyStatus.UnitTests/       # Unit tests
│   └── TeamWeeklyStatus.IntegrationTests/# Integration tests
├── appsettings.json                      # Configuration settings
├── Program.cs                            # Entry point of the application
├── README.md                             # This file
```

## Technologies Used

- **ReactJs**: Frontend library for building user interfaces.
- **ASP.NET Core**: Framework for building web applications and APIs.
- **Entity Framework Core**: Object-relational mapper (ORM) for data access.
- **SQL Server**: Primary database (can be swapped due to Clean Architecture).
- **Swagger**: API documentation.
- **XUnit**: Testing framework.
- **Moq**: Mocking framework for unit tests.

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

- **Project Link**: [https://github.com/linogvallejo/TeamWeeklyStatusV2](https://github.com/linogvallejo/TeamWeeklyStatusV2)
- **Author**: Lino Garcia Vallejo
- **Email**: [lgarcia@mangochango.com](mailto:lgarcia@mangochango.com)

---

Feel free to reach out if you have any questions or need assistance setting up the project. Happy coding!