# SmartRx Frontend - Development Guide

## 📋 Table of Contents
- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Available Scripts](#available-scripts)
- [Project Structure](#project-structure)
- [Development Guidelines](#development-guidelines)
- [Environment Configuration](#environment-configuration)
- [API Integration](#api-integration)
- [Styling Guidelines](#styling-guidelines)
- [Docker Development](#docker-development)
- [Troubleshooting](#troubleshooting)

---

## 🚀 Overview

SmartRx Frontend is a modern healthcare management application built with React. It provides comprehensive patient management, prescription handling, and medical record management features.

**Project Name:** rx-dev  
**Version:** 0.1.0  
**Package Manager:** Yarn 4.9.1  

---

## 🛠 Tech Stack

### Core Technologies
- **React** 19.0.0 - UI Library
- **React Router DOM** 7.2.0 - Routing
- **React Bootstrap** 2.10.9 - UI Components
- **Bootstrap** 5.3.3 - CSS Framework

### Key Libraries
- **Axios** 1.8.4 - HTTP Client
- **React Toastify** 11.0.5 - Notifications
- **Recharts** 2.15.1 - Data Visualization
- **React PDF** 9.2.1 - PDF Handling
- **Tesseract.js** 6.0.1 - OCR Functionality
- **JWT Decode** 4.0.0 - Authentication
- **Framer Motion** 12.6.3 - Animations
- **CropperJS** 2.0.0 - Image Cropping
- **React DatePicker** 8.2.1 - Date Selection

### Development Tools
- **ESLint** - Code Linting
- **Prettier** 3.6.2 - Code Formatting
- **http-proxy-middleware** 3.0.5 - API Proxy

---

## ✅ Prerequisites

Before you begin, ensure you have the following installed:

- **Node.js**: v20.19.1 or higher
- **Yarn**: v4.9.1 (managed via Corepack)
- **Git**: Latest version

### Enabling Yarn 4.9.1

```bash
corepack enable
corepack prepare yarn@4.9.1 --activate
```

---

## 📦 Installation

### 1. Clone the Repository

```bash
git clone <repository-url>
cd smartrx-revamp/src/frontend
```

### 2. Clean Install (Recommended)

If you encounter issues with existing dependencies:

```bash
# Remove existing node_modules
rm -rf node_modules

# Clear yarn cache (optional)
yarn cache clean

# Install dependencies
yarn install
```

### 3. Standard Installation

```bash
yarn install
```

---

## 🎯 Available Scripts

### Development

```bash
# Start development server (HTTPS on port 3001)
yarn start

# Start development server in LAN mode
yarn start:lan
```

**Default Development URL:** `https://0.0.0.0:3001`

### Build & Deployment

```bash
# Create production build
yarn build

# Copy build to backend (IIS deployment)
yarn copy-to-backend

# Build and deploy to backend server
yarn deploy
```

### Testing

```bash
# Run tests in watch mode
yarn test

# Run tests with coverage
yarn test --coverage
```

### Other Commands

```bash
# Eject from Create React App (⚠️ irreversible)
yarn eject
```

---

## 📁 Project Structure

```
frontend/
├── public/                      # Static assets
│   ├── img/                     # Public images
│   ├── index.html              # HTML template
│   └── manifest.json           # PWA manifest
│
├── src/
│   ├── assets/                  # Project assets
│   │   ├── css/                # Global styles
│   │   ├── font/               # Custom fonts
│   │   └── img/                # Images and SVGs
│   │
│   ├── components/              # React components
│   │   ├── BrowseRx/           # Patient browsing & management
│   │   ├── Dashboard/          # Dashboard components
│   │   ├── Login/              # Authentication
│   │   ├── Profile/            # User profile
│   │   ├── SmartRxInsider/     # Core medical features
│   │   └── static/             # Reusable UI components
│   │
│   ├── config/                  # Configuration files
│   │   └── config.js           # App configuration
│   │
│   ├── constants/               # Application constants
│   │   ├── apiEndpoints.js     # API endpoint definitions
│   │   ├── authorities.js      # User authorities/roles
│   │   └── constants.js        # General constants
│   │
│   ├── contexts/                # React contexts
│   │   ├── AuthoritiesContext.js
│   │   ├── FolderContext.js
│   │   ├── PreviewContext.js
│   │   └── UserContext.js
│   │
│   ├── hooks/                   # Custom React hooks
│   │   ├── useApiServiceCall.js
│   │   ├── useFetchData.js
│   │   ├── useErrorHandler.js
│   │   └── ...
│   │
│   ├── pages/                   # Page components
│   │   ├── MainLayout/
│   │   └── NotFoundPage/
│   │
│   ├── routes/                  # Route guards
│   │   ├── GuestRoute.js
│   │   └── ProtectedRoute.js
│   │
│   ├── services/                # API services
│   │   ├── useApiClients.js
│   │   ├── useApiService.js
│   │   └── useAuthService.js
│   │
│   ├── utils/                   # Utility functions
│   │   ├── dateTimeFormatterUtils.js
│   │   ├── errorHandling.js
│   │   └── ...
│   │
│   ├── App.js                   # Main App component
│   ├── index.js                 # Entry point
│   └── setupProxy.js            # Development proxy config
│
├── schemas/                     # JSON schemas
│   └── package-schema.json
│
├── Dockerfile                   # Docker configuration
├── package.json                 # Dependencies & scripts
├── webpack.config.js            # Webpack configuration
└── README.MD                    # This file
```

---

## 💻 Development Guidelines

### Code Style

1. **Use Prettier** for code formatting
   ```bash
   # Format code
   npx prettier --write "src/**/*.{js,jsx,css}"
   ```

2. **Follow ESLint rules** - especially React Hooks rules

3. **File Naming Conventions:**
   - Components: `PascalCase.js` (e.g., `PatientProfileList.js`)
   - Utilities: `camelCase.js` (e.g., `dateTimeFormatterUtils.js`)
   - Styles: `ComponentName.css`

### Component Structure

```jsx
// 1. Imports
import React, { useState, useEffect } from 'react';
import './ComponentName.css';

// 2. Component Definition
const ComponentName = ({ prop1, prop2 }) => {
  // 3. Hooks
  const [state, setState] = useState(null);

  // 4. Effects
  useEffect(() => {
    // Effect logic
  }, []);

  // 5. Event Handlers
  const handleClick = () => {
    // Handler logic
  };

  // 6. Render
  return (
    <div className="component-name">
      {/* Component JSX */}
    </div>
  );
};

export default ComponentName;
```

### Custom Hooks Usage

The project includes several custom hooks for common operations:

- `useApiServiceCall` - API service calls
- `useFetchData` - Data fetching
- `useErrorHandler` - Error handling
- `useCustomCSS` - Dynamic styling
- `useModal` - Modal management
- `useLocalStorage` - Local storage operations

Example:
```jsx
import { useFetchData } from '../../hooks/useFetchData';

const MyComponent = () => {
  const { data, loading, error } = useFetchData('/api/endpoint');
  // Component logic
};
```

---

## 🔧 Environment Configuration

### Environment Variables

Create a `.env` file in the root directory:

```env
# API Configuration
REACT_APP_API_BASE_URL=https://your-api-url.com
REACT_APP_ENV=development

# HTTPS Configuration
HTTPS=true
HOST=0.0.0.0
PORT=3001

# Other configurations
REACT_APP_VERSION=$npm_package_version
```

### Configuration File

Edit `src/config/config.js` for application-specific settings:

```javascript
const config = {
  apiBaseUrl: process.env.REACT_APP_API_BASE_URL,
  environment: process.env.REACT_APP_ENV,
  // Other configuration options
};
```

---

## 🌐 API Integration

### Proxy Configuration

The development server uses `setupProxy.js` for API proxy configuration to avoid CORS issues.

```javascript
// src/setupProxy.js
const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function(app) {
  app.use(
    '/api',
    createProxyMiddleware({
      target: 'http://localhost:5000',
      changeOrigin: true,
    })
  );
};
```

### API Service Usage

```javascript
import { useApiService } from '../../services/useApiService';

const MyComponent = () => {
  const apiService = useApiService();
  
  const fetchData = async () => {
    try {
      const response = await apiService.get('/endpoint');
      // Handle response
    } catch (error) {
      // Handle error
    }
  };
};
```

---

## 🎨 Styling Guidelines

### CSS Organization

1. **Component-specific styles**: Create a `.css` file alongside each component
2. **Global styles**: Place in `src/assets/css/`
3. **CSS naming**: Use kebab-case for class names

### Custom Fonts

The project includes several font families:
- **Georama** - Variable font
- **Inter** - Variable font
- **Plus Jakarta Sans** - Variable font
- **Urbanist** - Variable font
- **ABeeZee** - Regular & Italic
- **Source Sans Pro** - Multiple weights

Import fonts in your CSS:
```css
@font-face {
  font-family: 'CustomFont';
  src: url('../../assets/font/path-to-font.ttf');
}
```

### Bootstrap Customization

The project uses Bootstrap 5.3.3 with custom theming. Override Bootstrap variables in your component CSS or global styles.

---

## 🐳 Docker Development

### Build Docker Image

```bash
docker build -t smartrx-frontend .
```

### Run Docker Container

```bash
docker run -p 4200:4200 smartrx-frontend
```

### Docker Configuration Details

- **Base Image**: node:20.19.1-slim
- **Working Directory**: /app
- **Exposed Port**: 4200
- **Package Manager**: Yarn 4.9.1 (via Corepack)

---

## 🔍 Troubleshooting

### Common Issues

#### 1. Yarn Version Mismatch

```bash
# Enable corepack and set correct yarn version
corepack enable
corepack prepare yarn@4.9.1 --activate
```

#### 2. SSL/HTTPS Certificate Issues

If you encounter HTTPS certificate warnings in development:
- Accept the self-signed certificate in your browser
- Or disable HTTPS in package.json by removing `HTTPS=true`

#### 3. Port Already in Use

```bash
# Find and kill process on port 3001
# Linux/Mac:
lsof -ti:3001 | xargs kill -9

# Windows:
netstat -ano | findstr :3001
taskkill /PID <PID> /F
```

#### 4. Module Not Found Errors

```bash
# Clear cache and reinstall
rm -rf node_modules yarn.lock
yarn cache clean
yarn install
```

#### 5. React Version Conflicts

The project uses React 19. Ensure all React-related packages are compatible with React 19.

### Debugging

1. **React Developer Tools**: Install browser extension
2. **Console Logging**: Use `console.log()` for debugging
3. **React Error Boundaries**: Implemented in the app for error catching
4. **Network Tab**: Monitor API calls in browser DevTools

---

## 📚 Additional Resources

### Important Files to Review

- `src/App.js` - Main application component
- `src/routes/` - Routing configuration
- `src/constants/apiEndpoints.js` - API endpoint definitions
- `src/contexts/` - Global state management

### Useful Commands

```bash
# Add a new package
yarn add package-name

# Add a dev dependency
yarn add package-name --dev

# Remove a package
yarn remove package-name

# Update all packages
yarn upgrade

# Check for outdated packages
yarn outdated
```

---

## 👥 Contributing

1. Create a feature branch from the development branch
2. Follow the coding guidelines
3. Write meaningful commit messages
4. Test your changes thoroughly
5. Submit a pull request with a clear description

---

## 📝 Notes

- The project uses React Scripts 5.0.1 (Create React App)
- HTTPS is enabled by default for development
- The app runs on all network interfaces (0.0.0.0) for LAN access
- Default port is 3001 (configurable via environment variables)
- For WAN access port 5000

---

## 🔗 Related Documentation

- [React Documentation](https://react.dev/)
- [React Router](https://reactrouter.com/)
- [Bootstrap Documentation](https://getbootstrap.com/)
- [Yarn Documentation](https://yarnpkg.com/)

---

**Last Updated:** October 2025  
**Maintained by:** SmartRx Development Team
