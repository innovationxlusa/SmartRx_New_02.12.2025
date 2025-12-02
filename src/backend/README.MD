# SmartRx Backend API

## Overview
SmartRx Backend is a comprehensive healthcare management system built with .NET 8.0, Entity Framework Core, and CQRS pattern using MediatR. The system manages patient profiles, prescriptions, doctor profiles, rewards, and medical records.

## Technology Stack
- **.NET 8.0** - Framework
- **Entity Framework Core** - ORM with Lazy Loading
- **SQL Server** - Database
- **MediatR** - CQRS pattern implementation
- **JWT Authentication** - Secure API access

## Project Structure

```
PMSBackend/
├── PMSBackend.API/              # API Controllers and Presentation Layer
├── PMSBackend.Application/      # Application Logic (Commands, Queries, DTOs)
├── PMSBackend.Databases/        # Data Access Layer (Repositories, DbContext)
├── PMSBackend.Domain/           # Domain Entities and Interfaces
└── PMSLibrary.Infrastructure/   # Infrastructure Services
```

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022 or VS Code

### Installation & Running

In VS Code:
1. **Navigate to the backend folder:**
   ```bash
   cd path\to\smartrx-revamp\src\backend\PMSBackend
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Update database connection string in `appsettings.json`:**
   ```json
   {
     "ConnectionStrings": {
       "PMSDBConnection": "your-connection-string-here"
     }
   }
   ```

4. **Build the solution:**
   ```bash
   dotnet build
   ```

5. **Run the application:**
   ```bash
   dotnet run
   ```

6. **Access the API:**
   - HTTP: `http://localhost:7000`

In Visual Studio:
    Backend run as a Backend Developer
    Install Git in windows
    Open Git bash
    Git checkout from link- https://github.com/togetherGithub/smartrx-revamp.git
    or get the link from git https
    Install visual studio latest version
    Go to path "\smartrx-revamp\src\backend" and right click on .sln file>Open(Direct open) or Open with>Select Visual Studio version
    Another way:
        Open Visual Studio
        Select File>Open>Select the same path ("\smartrx-revamp\src\backend")



## API Modules

### 1. Authentication & Authorization
- **POST** `/api/Auth/Login` - User login with JWT token generation
- **POST** `/api/Auth/VerifyOtp` - OTP verification
- **POST** `/api/Auth/VerifyPassword` - Password verification

### 2. User Management
- **POST** `/api/User/CreateUser` - Create new user
- **PUT** `/api/User/EditUserProfile` - Update user profile
- **GET** `/api/User/GetUserDetails/{id}` - Get user by ID
- **GET** `/api/User/GetAllUsers` - Get all users with pagination
- **POST** `/api/User/AssignExternalUsersToRole` - Assign roles to users

### 3. Patient Profile Management
- **POST** `/api/PatientProfile/CreatePatientProfile` - Create patient profile
- **PUT** `/api/PatientProfile/EditPatientProfile` - Update patient profile
- **GET** `/api/PatientProfile/GetPatientProfileById/{id}` - Get patient by ID
- **GET** `/api/PatientProfile/GetAllPatientProfilesByUserId` - Get all patients with pagination, search, and filtering

### 4. Doctor Profile Management
- **GET** `/api/Doctor/GetDoctorProfilesByUserId` - Get doctors with pagination
- **GET** `/api/Doctor/GetDoctorProfileById/{id}` - Get doctor by ID

### 5. Prescription Management
- **POST** `/api/PrescriptionUpload/UploadPrescription` - Upload prescription files
- **GET** `/api/BrowseRx/GetBrowseRx` - Browse prescription folders and files
- **POST** `/api/Folder/CreateFolder` - Create folder
- **PUT** `/api/Folder/UpdateFolder` - Update folder
- **DELETE** `/api/Folder/DeleteFolder/{id}` - Delete folder

### 6. SmartRx Insider
- **GET** `/api/SmartRxInsider/GetSmartRxMainInsider` - Get complete SmartRx prescription details
- **GET** `/api/SmartRxInsider/GetAllSmartRxWithVitalsByUserId` - Get SmartRx records with vitals
- **PUT** `/api/SmartRxInsider/ChangeSmartRxDoctorReview` - Update doctor review
- **PUT** `/api/SmartRxInsider/EditSmartRxMedicineWishlist` - Update medicine wishlist
- **PUT** `/api/SmartRxInsider/EditSmartRxInvestigationWishlist` - Update investigation wishlist

### 7. Vital Management
- **POST** `/api/Vital/AddSmartRxVital` - Add vital sign
- **PUT** `/api/Vital/EditSmartRxVital` - Update vital sign
- **DELETE** `/api/Vital/DeleteSmartRxVital` - Delete vital sign
- **GET** `/api/Vital/GetAllVitalByVitalName` - Get vitals by name

### 8. Reward System (NEW)

#### Configuration_Reward APIs
- **POST** `/api/Reward/CreateReward` - Create reward configuration
- **PUT** `/api/Reward/UpdateReward` - Update reward configuration
- **DELETE** `/api/Reward/DeleteReward/{id}` - Delete reward
- **GET** `/api/Reward/GetRewardById/{id}` - Get reward by ID
- **GET** `/api/Reward/GetAllRewards` - Get all rewards with pagination

#### Configuration_RewardBadge APIs
- **POST** `/api/RewardBadge/CreateRewardBadge` - Create reward badge
- **PUT** `/api/RewardBadge/UpdateRewardBadge` - Update reward badge
- **DELETE** `/api/RewardBadge/DeleteRewardBadge/{id}` - Delete badge
- **GET** `/api/RewardBadge/GetRewardBadgeById/{id}` - Get badge by ID
- **GET** `/api/RewardBadge/GetAllRewardBadges` - Get all badges with pagination

#### SmartRx_PatientReward APIs
- **POST** `/api/PatientReward/CreatePatientReward` - Create patient reward entry
- **PUT** `/api/PatientReward/UpdatePatientReward` - Update patient reward
- **DELETE** `/api/PatientReward/DeletePatientReward/{id}` - Delete patient reward
- **GET** `/api/PatientReward/GetPatientRewardsByUserIdAndPatientId` - Get rewards with pagination
- **GET** `/api/PatientReward/GetPatientRewardsSummary` - Get reward summary totals

### 9. Dashboard
- **GET** `/api/Dashboard/GetDashboardData` - Get dashboard statistics

### 10. Other Expense Management
- **POST** `/api/SmartRxOtherExpense/AddSmartRxOtherExpense` - Add expense
- **PUT** `/api/SmartRxOtherExpense/EditSmartRxOtherExpense` - Update expense
- **DELETE** `/api/SmartRxOtherExpense/DeleteSmartRxOtherExpense` - Delete expense

## Key Features

### CQRS Pattern
- **Commands**: Create, Update, Delete operations
- **Queries**: Read operations with complex filtering
- **MediatR**: Decoupled request/response handling

### Validation
- Multi-layer validation (Handler → Repository)
- Duplicate name/heading checks
- Length constraints
- Business rule validation
- Proper HTTP status codes (200, 404, 417, 500)

### Search & Filtering
- Multi-column search support
- Gender search (Male/1, Female/2)
- Blood group search (A+/1, A-/2, B+/3, B-/4, AB+/5, AB-/6, O+/7, O-/8)
- Age and date of birth search
- Vital name and value search
- Case-insensitive searching

### Pagination & Sorting
- Configurable page size and page number
- Multi-field sorting support
- Sort direction (asc/desc)
- Total record count

### Data Transfer Objects (DTOs)
- Clean separation between entities and API contracts
- Consistent response structure with `ApiResponseResult`
- Wrapper DTOs for validation responses

## Database Configuration

### Connection String Encryption
The system uses encrypted connection strings stored in Azure Key Vault or local configuration.

### Migrations
Entity Framework migrations are located in `PMSBackend.Databases/Migrations/`

To apply migrations:
```bash
dotnet ef database update --project PMSBackend.Databases
```

## Authentication

### JWT Token APIs

The system uses JWT authentication with access and refresh tokens:

#### Generate Token (Quick Testing - Development Only)
**POST** `/api/Auth/generate-token`
```json
{
  "userId": 1
}
```
⚠️ Development use only - generates token directly without authentication

#### Get Token (Login)
**POST** `/api/Auth/Login`
```json
{
  "userName": "superadmin",
  "authType": 1,
  "password": "1234"
}
```
Response includes both `accessToken` and `refreshToken`.

#### Refresh Token
**POST** `/api/Auth/refresh-token` - Get new access token when expired

#### Revoke Token
**POST** `/api/Auth/revoke-token` - Logout from specific device

#### Revoke All User Tokens
**POST** `/api/Auth/revoke-all-user-tokens` - Logout from all devices

### Test Users
| Username | Password | Role |
|----------|----------|------|
| superadmin | 1234 | Super Admin |
| admin | 1234 | Admin |
| entryuser | 1234 | Entry User |

### JWT Configuration
Configure JWT settings in `appsettings.json`:
```json
{
  "JwtSettings": {
    "SecretKey": "your-secret-key",
    "Issuer": "smartrx-app",
    "Audience": "smartrx-users",
    "ExpiryMinutes": 120,
    "RefreshTokenExpiryDays": 7
  }
}
```

### Authorization
Most endpoints require JWT Bearer token:
```
Authorization: Bearer {your-access-token}
```

### Postman Testing
For detailed JWT testing instructions, see [POSTMAN_JWT_TESTING_GUIDE.md](POSTMAN_JWT_TESTING_GUIDE.md)

HTTP requests examples are available in `PMSBackend/PMSBackend.http`

## CORS Configuration
Configured origins in `Program.cs`:
- `https://localhost:3001`
- Additional configured IPs for development/production

## File Storage
- **Prescription Files**: `/Files` directory
- **Profile Photos**: `/Photos` directory
- Static file serving enabled

## Error Handling

### Global Exception Middleware
The application has a global exception middleware (`Common/ExceptionMiddleware.cs`) that catches and handles unhandled exceptions. It:
- **Shows specific error details** - Returns exact exception type, message, inner exceptions, and stack trace
- **Default to 500** - All unknown exceptions default to HTTP 500 Internal Server Error
- **Custom ErrorCode support** - All custom exceptions support ErrorCode property for specific status codes
- **Maps exceptions to HTTP codes** - Automatically maps exception types to appropriate HTTP status codes
- **Development vs Production** - Detailed debugging info in development, user-friendly messages in production
- **Comprehensive logging** - All exceptions logged with full context including type, message, inner exception, and stack trace
- **User-friendly messages** - Production environment shows clean, safe error messages

### Error Handling Pattern
Controllers should use try-catch blocks to handle specific errors gracefully. 

**Documentation:**
- [EXCEPTION_HANDLING_GUIDE.md](EXCEPTION_HANDLING_GUIDE.md) - Complete guide on how exception handling works
- [ERROR_HANDLING_SUMMARY.md](ERROR_HANDLING_SUMMARY.md) - Current implementation status and recommendations

### Status
- Controllers with proper error handling: 7/18 (39%)
- Controllers needing improvements: 11/18 (61%)
- Repository layer: ✅ All repositories have try-catch with logging (re-throw pattern)
- Handler layer: ✅ Uses re-throw pattern (appropriate)
- Global exception middleware: ✅ Enhanced with specific error codes and detailed messages

### Recently Fixed:
- ✅ RefreshTokenRepository - Comprehensive error handling with logging
- ✅ TokenGenerator - Comprehensive error handling and logging
- ✅ ExceptionMiddleware - Specific error details with default to 500
- ✅ All custom exception classes - Support for ErrorCode property

## API Response Format

### Success Response
```json
{
  "data": { ... },
  "statusCode": 200,
  "status": "Success",
  "message": "Operation completed successfully"
}
```

### Error Response
```json
{
  "data": null,
  "statusCode": 404,
  "status": "Failed",
  "message": "Resource not found"
}
```

## Development Notes

### Code Organization
- **Controllers**: Thin layer, handles HTTP concerns only
- **Handlers**: Business logic and validation
- **Repositories**: Data access only
- **Entities**: Domain models with navigation properties

### Naming Conventions
- Commands: `{Action}{Entity}Command` (e.g., `CreateRewardCommand`)
- Queries: `Get{Entity}Query` (e.g., `GetRewardByIdQuery`)
- DTOs: `{Entity}DTO` (e.g., `RewardDTO`)
- Repositories: `I{Entity}Repository` / `{Entity}Repository`

### Best Practices
- All validation in handlers, not controllers
- DTOs for all API responses
- Proper async/await usage
- Cancellation token support
- Null safety checks
- Duplicate prevention

## Testing
Access UI at `http://localhost:7000` for interactive API testing.

## Logging
- Console logging enabled
- Minimum log level: Debug
- EF Core sensitive data logging enabled in development

## Support
For issues or questions, contact the development team.

## Version
- **API Version**: 1.0
- **.NET Version**: 8.0
- **Last Updated**: October 2025
