# Exception Handling Guide

This guide explains how exception handling works throughout the SmartRx backend application.

## 🎯 Overview

The application implements a comprehensive exception handling system that:
- ✅ Shows specific error details including exception type and message
- ✅ Defaults to HTTP 500 for unknown exceptions
- ✅ Supports custom ErrorCode property in all custom exceptions
- ✅ Provides different error detail levels for Development vs Production
- ✅ Logs all exceptions with full context

## 🔧 How It Works

### 1. Global Exception Middleware

The `ExceptionMiddleware` catches all unhandled exceptions and processes them:

```csharp
// Located in: PMSBackend/Common/ExceptionMiddleware.cs
```

**Flow:**
1. Exception is raised anywhere in the application
2. Caught by `ExceptionMiddleware`
3. Exception type determined and mapped to appropriate HTTP status code
4. Detailed error response built
5. Comprehensive logging performed
6. Error response returned to client

### 2. Exception Type to HTTP Status Code Mapping

| Exception Type | HTTP Status Code | Default |
|----------------|------------------|---------|
| `NotFoundException` | 404 Not Found | If ErrorCode > 0, uses custom code |
| `BadRequestException` | 400 Bad Request | If ErrorCode > 0, uses custom code |
| `ConflictException` | 409 Conflict | If ErrorCode > 0, uses custom code |
| `UnprocessableEntityException` | 422 Unprocessable Entity | If ErrorCode > 0, uses custom code |
| `ForbiddenAccessException` | 403 Forbidden | If ErrorCode > 0, uses custom code |
| `ValidationException` | 400 Bad Request | - |
| `UnauthorizedAccessException` | 401 Unauthorized | - |
| `ArgumentException` | 400 Bad Request | - |
| `ArgumentNullException` | 400 Bad Request | - |
| `KeyNotFoundException` | 404 Not Found | - |
| `InvalidOperationException` | 409 Conflict | - |
| `TimeoutException` | 408 Request Timeout | - |
| `NotImplementedException` | 501 Not Implemented | - |
| `OperationCanceledException` | 499 Client Closed Request | - |
| `SqlException` | 500 Internal Server Error | - |
| `DbUpdateException` | 500 Internal Server Error | - |
| **Unknown Exception** | **500 Internal Server Error** | **Default** |

### 3. Error Response Format

#### Development Environment
```json
{
  "data": null,
  "statusCode": 500,
  "status": "Failed",
  "message": "User not found [Exception Type: InvalidOperationException] | Inner Exception: NullReferenceException - Object reference not set | Source: PMSBackend.Databases | Method: GetDetailsByIdAsync",
  "stackTrace": "at PMSBackend.Databases.Repositories...",
  "hRResult": -2146233079
}
```

#### Production Environment
```json
{
  "data": null,
  "statusCode": 500,
  "status": "Failed",
  "message": "An internal server error occurred. Please contact the system administrator.",
  "stackTrace": null,
  "hRResult": -2146233079
}
```

## 📝 Custom Exception Classes

All custom exception classes now support the `ErrorCode` property:

### NotFoundException
```csharp
throw new NotFoundException("User not found", 404);
throw new NotFoundException("Entity", userId); // Uses default 404
```

### BadRequestException
```csharp
throw new BadRequestException("Invalid input", 400);
throw new BadRequestException("Validation failed"); // Uses default 400
```

### ConflictException
```csharp
throw new ConflictException("User already exists", 409);
throw new ConflictException("Resource conflict"); // Uses default 409
```

### UnprocessableEntityException
```csharp
throw new UnprocessableEntityException("Invalid business logic", 422);
throw new UnprocessableEntityException("Cannot process"); // Uses default 422
```

### ForbiddenAccessException
```csharp
throw new ForbiddenAccessException("Access denied", 403);
throw new ForbiddenAccessException(); // Uses default 403 with message
```

### ValidationException
```csharp
// Automatically maps validation errors dictionary
throw new ValidationException(validationFailures);
// Returns 400 with Errors dictionary in response
```

## 🔨 Usage Examples

### Example 1: Throw Custom Exception with Specific Error Code
```csharp
public async Task<User> GetUserAsync(long userId)
{
    var user = await _userRepository.GetDetailsByIdAsync(userId);
    if (user == null)
    {
        throw new NotFoundException($"User with ID {userId} not found", 404);
    }
    return user;
}
```

### Example 2: Throw Business Logic Exception
```csharp
public async Task<bool> CreateUserAsync(User user)
{
    var existingUser = await _userRepository.GetByEmailAsync(user.Email);
    if (existingUser != null)
    {
        throw new ConflictException("User with this email already exists", 409);
    }
    // ... create user
}
```

### Example 3: Throw Validation Exception
```csharp
var validator = new CreateUserValidator();
var validationResult = validator.Validate(userDto);

if (!validationResult.IsValid)
{
    throw new ValidationException(validationResult.Errors);
}
```

### Example 4: Let Exception Bubble Up to Middleware
```csharp
public async Task<T> AddAsync(T entity)
{
    try
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error adding entity");
        throw; // Let middleware handle it
    }
}
```

## 📊 Status Code Details

### Success Status Codes
- `200 OK` - Request successful
- `201 Created` - Resource created successfully
- `202 Accepted` - Request accepted for processing

### Client Error Status Codes (4xx)
- `400 Bad Request` - Invalid request format or validation failure
- `401 Unauthorized` - Authentication required
- `403 Forbidden` - Access denied
- `404 Not Found` - Resource not found
- `408 Request Timeout` - Request took too long
- `409 Conflict` - Resource conflict (e.g., duplicate)
- `422 Unprocessable Entity` - Request valid but cannot be processed

### Server Error Status Codes (5xx)
- `500 Internal Server Error` - **Default for unknown exceptions**
- `501 Not Implemented` - Feature not yet implemented
- `499 Client Closed Request` - Request cancelled by client

## 🛠️ Best Practices

### 1. Use Custom Exceptions for Business Logic
```csharp
// ✅ Good
if (user == null)
    throw new NotFoundException("User not found", 404);

// ❌ Avoid
if (user == null)
    throw new Exception("User not found");
```

### 2. Let Repository/Hafndler Exceptions Bubble Up
```csharp
// ✅ Good
public async Task<User> GetUser(long id)
{
    try
    {
        return await _repository.GetById(id);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error getting user");
        throw; // Let middleware handle it
    }
}

// ❌ Avoid
public async Task<User> GetUser(long id)
{
    try
    {
        return await _repository.GetById(id);
    }
    catch (Exception ex)
    {
        return null; // Don't swallow exceptions
    }
}
```

### 3. Use Appropriate Exception Types
```csharp
// ✅ Use NotFoundException for missing resources
throw new NotFoundException("Patient not found", 404);

// ✅ Use BadRequestException for invalid input
throw new BadRequestException("Email format invalid", 400);

// ✅ Use ConflictException for duplicates
throw new ConflictException("User already exists", 409);

// ✅ Use UnprocessableEntityException for business logic violations
throw new UnprocessableEntityException("Patient age invalid", 422);
```

### 4. Always Log Before Throwing
```csharp
// ✅ Good
catch (Exception ex)
{
    _logger.LogError(ex, "Error processing user {UserId}", userId);
    throw;
}

// ❌ Avoid
catch (Exception ex)
{
    throw; // No logging
}
```

### 5. Provide Meaningful Error Messages
```csharp
// ✅ Good
throw new NotFoundException($"Patient with ID {patientId} not found in database", 404);

// ❌ Avoid
throw new NotFoundException("Not found", 404);
```

## 🔍 Error Debugging

### Development Environment
- Full exception details including type, message, inner exception, stack trace
- Source code location and method name
- Inner stack trace for nested exceptions

### Production Environment
- User-friendly error messages
- No stack traces or sensitive information exposed
- Generic message for 500 errors: "An internal server error occurred"

## 📈 Logging

All exceptions are logged with comprehensive details:

```
Unhandled exception occurred | Type: InvalidOperationException | Message: User not found | Inner Exception: None | Stack Trace: at PMSBackend...
```

### Log Levels
- `LogInformation` - Successful operations
- `LogWarning` - Non-critical issues, missing data
- `LogError` - Exceptions and errors
- `LogCritical` - System failures

## 🎯 Summary

The exception handling system ensures:
1. **Specific error details** shown to developers and logged
2. **Default to 500** for unknown exceptions
3. **Custom ErrorCode** support for flexibility
4. **Environment-specific** error detail levels
5. **Comprehensive logging** for debugging
6. **User-friendly messages** in production

## 📚 Related Documentation

- [ERROR_HANDLING_SUMMARY.md](ERROR_HANDLING_SUMMARY.md) - Detailed error handling implementation status
- [README.MD](README.MD) - Project overview and error handling status

