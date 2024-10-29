# Team Weekly Status Frontend

Welcome to the **Team Weekly Status Frontend** application! This project is the frontend layer of the Team Weekly Status system, built with **React**, **TypeScript**, and **Vite**. It provides a user-friendly interface for managing weekly status reports, across multiple teams, and administrative tasks.

## Table of Contents

- [Features](#features)
- [Demo](#demo)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [HTTPS Configuration](#https-configuration)
- [Available Scripts](#available-scripts)
- [Project Structure](#project-structure)
- [Technologies Used](#technologies-used)
- [Routing](#routing)
- [State Management](#state-management)
- [API Interaction](#api-interaction)
- [Environment Variables](#environment-variables)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

## Features

- **User Authentication**: Secure login and logout functionality.
- **Weekly Status Management**: Create, edit, and view weekly status reports.
- **Team Selection**: Choose and switch between different teams.
- **Administrative Tasks**: Manage users, teams, and permissions (admin only).
- **Rich Text Editing**: Utilize CKEditor for a rich text editing experience.
- **Responsive Design**: Mobile-friendly layouts and components.
- **Error Handling**: User-friendly error messages and notifications.
- **Performance Optimization**: Lazy loading and code splitting.
- **HTTPS Support**: Runs over HTTPS for secure communication.
- **API Integration**: Interacts with the backend API using Axios.


## Prerequisites

Before you begin, ensure you have met the following requirements:

- **Node.js** (version 18 or higher)
- **npm** (version 6 or higher)
- **Vite**: Installed globally (`npm install -g vite`)
- **Access to the Backend API**: Ensure the backend API is running and accessible.
- **HTTPS Certificates**: SSL certificate and key files (`reactapp.pem` and `reactapp.key`)

## Getting Started

To get started with the project, follow these steps:

1. **Clone the repository:**

   ```sh
   git clone https://github.com/linogvallejo/TeamWeeklyStatusV2.git
   cd team-weekly-status-frontend
   ```

2. **Install the dependencies:**

   ```sh
   npm install
   ```

3. **Set up environment variables:**

   - Create a `.env` file in the root directory.
   - Add the necessary environment variables (see [Environment Variables](#environment-variables)).

4. **Configure HTTPS:**

   - Place your SSL certificate files (`reactapp.pem` and `reactapp.key`) in the following directory:

     - **Windows:** `%APPDATA%\ASP.NET\https`
     - **macOS/Linux:** `$HOME/.aspnet/https`

     > **Note:** You can also specify a different directory by configuring `aspnetcore-https.js` and `vite.config.js`. See [HTTPS Configuration](#https-configuration) for details.

5. **Start the development server:**

   ```sh
   npm run dev
   ```

6. **Open your browser:**

   - Navigate to `https://localhost:5173` to view the application.

## HTTPS Configuration

The application is configured to run over **HTTPS** for secure communication with the backend. Follow these steps to set up HTTPS:

### **1. Obtain SSL Certificates**

- **Development Certificates:** You can generate self-signed certificates for development purposes.
- **Production Certificates:** Obtain valid SSL certificates from a trusted Certificate Authority.

### **2. Place Certificate Files**

- Place the `reactapp.pem` (certificate) and `reactapp.key` (private key) files in the following directory:

  - **Windows:**

    ```
    %APPDATA%\ASP.NET\https
    ```

  - **macOS/Linux:**

    ```
    $HOME/.aspnet/https
    ```

### **3. Configure Custom Certificate Directory (Optional)**

If you prefer to store your certificates in a different directory, you'll need to update the paths in both `aspnetcore-https.js` and `vite.config.js`.

- **Update `aspnetcore-https.js`:**

  ```javascript
  const certificate = {
    cert: fs.readFileSync('path/to/your/reactapp.pem'),
    key: fs.readFileSync('path/to/your/reactapp.key'),
  };
  ```

- **Update `vite.config.js`:**

  ```javascript
  export default defineConfig({
    // ...
    server: {
      https: {
        cert: fs.readFileSync('path/to/your/reactapp.pem'),
        key: fs.readFileSync('path/to/your/reactapp.key'),
      },
    },
  });
  ```

### **4. Trust the Certificate (Development Only)**

- **macOS:**

  - Open `Keychain Access`.
  - Import the certificate and set it to "Always Trust".

- **Windows:**

  - Open the certificate file.
  - Install it into the "Trusted Root Certification Authorities" store.

### **5. Verify HTTPS Setup**

- Start the development server:

  ```sh
  npm run dev
  ```

- Navigate to `https://localhost:5173` and ensure there are no SSL warnings.

## Available Scripts

In the project directory, you can run:

- **`npm run dev`**: Starts the development server with hot module replacement at `https://localhost:5173`.
- **`npm run build`**: Builds the app for production in the `dist` folder.
- **`npm run preview`**: Previews the production build locally.
- **`npm run lint`**: Runs ESLint to check for linting errors.
- **`npm run format`**: Formats the code using Prettier.
- **`npm run test`**: Runs unit tests using your configured testing library.

## Project Structure

```
team-weekly-status-frontend/
├── public/                     # Static assets
├── src/
│   ├── assets/                 # Images, icons, etc.
│   ├── components/             # Reusable components
│   ├── pages/                  # Page components for routes
│   ├── services/               # API calls and service functions
│   ├── store/                  # State management (e.g., Zustand store)
│   ├── routing/                # Route definitions and guards
│   ├── types/                  # TypeScript type definitions
│   ├── utils/                  # Utility functions and helpers
│   ├── styles/                 # Global and component-specific styles
│   ├── App.tsx                 # Root component
│   ├── main.tsx                # Entry point
│   └── index.css               # Global styles
├── .env                        # Environment variables
├── .gitignore                  # Git ignore rules
├── index.html                  # HTML template
├── package.json                # Project dependencies and scripts
├── tsconfig.json               # TypeScript configuration
├── vite.config.js              # Vite configuration
└── README.md                   # Project documentation
```

## Technologies Used

- **React**: JavaScript library for building user interfaces.
- **TypeScript**: Typed superset of JavaScript.
- **Vite**: Fast development build tool.
- **React Router**: Client-side routing.
- **Zustand**: State management library.
- **React Bootstrap**: UI components.
- **CKEditor 5**: Rich text editor.
- **Axios**: Promise-based HTTP client for API requests.
- **ESLint**: Linting utility.
- **Prettier**: Code formatter.

## Routing

The routing for the application is defined in [`src/routing/routes.tsx`](src/routing/routes.tsx). It uses `react-router-dom` for client-side routing and includes private routes for authenticated users.

- **Public Routes**: Accessible without authentication (e.g., Login).
- **Private Routes**: Accessible only after authentication (e.g., Dashboard, Weekly Status).
- **Admin Routes**: Accessible only to users with admin privileges.

## State Management

The application uses **Zustand** for state management, providing a simple and scalable way to manage global state.

- **User Store**: Manages user authentication and profile information.
- **Team Store**: Manages team selection and related data.
- **Status Store**: Manages weekly status reports.

## API Interaction

The application interacts with the backend API to fetch and update data. It uses **Axios** for making HTTP requests.

- **API Service**: Located in `src/services/apiHelper.ts`, it defines methods for making API calls.
- **Base URL**: Configured using environment variables (see [Environment Variables](#environment-variables)).
- **Endpoints**: Organized by feature (e.g., `/Member/GetAll`, `/WeeklyStatus/Add`).
- **Error Handling**: API responses are handled with appropriate error checking and user feedback.

### **Example API Call**

```typescript
import axios from 'axios';

export const fetchUserMembers = async () => {
  try {
    const response = await axios.get('/Member/GetAll');
    return response.data;
  } catch (error) {
    throw new Error('Failed to fetch members.');
  }
};
```

### **Configuring Axios**

Axios is configured to use the base URL from environment variables:

```typescript
import axios from 'axios';

axios.defaults.baseURL = import.meta.env.VITE_API_BASE_URL;

// Include credentials if needed
axios.defaults.withCredentials = true;
```

## Environment Variables

The project uses a `.env` file to manage environment variables. Create a `.env` file in the root directory and add the necessary variables:

```dotenv
VITE_API_BASE_URL=https://localhost:5001/api
VITE_ANOTHER_VARIABLE=your_value
```

- **`VITE_API_BASE_URL`**: The base URL for the backend API, including the protocol (`https`).
- **`VITE_SOME_OTHER_VAR`**: Additional variables as needed.

> **Note:** All Vite environment variables must be prefixed with `VITE_`.

## Contributing

Contributions are welcome! Please follow these steps:

1. **Fork the repository**.

2. **Create a new branch**:

   ```sh
   git checkout -b feature/your-feature-name
   ```

3. **Make your changes**.

4. **Commit your changes**:

   ```sh
   git commit -m "Add some feature"
   ```

5. **Push to the branch**:

   ```sh
   git push origin feature/your-feature-name
   ```

6. **Open a Pull Request**.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more details.

## Contact

- **Project Link**: [https://github.com/linogvallejo/TeamWeeklyStatusV2](https://github.com/linogvallejo/TeamWeeklyStatusV2)
- **Author**: Lino Garcia Vallejo
- **Email**: [lgarcia@mangochango.com](mailto:lgarcia@mangochango.com)

---
