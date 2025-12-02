# JWT Authentication with Refresh Token - Implementation Guide

## Overview
This project implements a complete JWT (JSON Web Token) authentication system with refresh token support. The system automatically uses environment-specific configuration files for Development, Staging, and Production environments.

---

## Environment Configuration

### How It Works
The application automatically loads the correct `appsettings.json` file based on the environment:

1. **Base Configuration**: `appsettings.json` (always loaded first)
2. **Environment-Specific**: `appsettings.{Environment}.json` (overrides base settings)
3. **Environment Variables**: Highest priority (overrides all file settings)

### Setting the Environment

#### Development (Local)
```bash
# Windows PowerShell
$env:ASPNETCORE_ENVIRONMENT="Development"

# Windows CMD
set ASPNETCORE_ENVIRONMENT=Development

# Linux/Mac
export ASPNETCORE_ENVIRONMENT=Development
```

#### Staging
```bash
# Windows PowerShell
$env:ASPNETCORE_ENVIRONMENT="Staging"

# Windows CMD
set ASPNETCORE_ENVIRONMENT=Staging

# Linux/Mac
export ASPNETCORE_ENVIRONMENT=Staging
```

#### Production
```bash
# Windows PowerShell
$env:ASPNETCORE_ENVIRONMENT="Production"

# Windows CMD
set ASPNETCORE_ENVIRONMENT=Production

# Linux/Mac
export ASPNETCORE_ENVIRONMENT=Production
```

---

## JWT Settings by Environment

### Development (`appsettings.Development.json`)
- **Issuer**: `smartrx-app-dev`
- **Audience**: `smartrx-users-dev`
- **Access Token Expiry**: 120 minutes (2 hours)
- **Refresh Token Expiry**: 7 days

### Staging (`appsettings.Staging.json`)
- **Issuer**: `smartrx-app-staging`
- **Audience**: `smartrx-users-staging`
- **Access Token Expiry**: 90 minutes
- **Refresh Token Expiry**: 14 days

### Production (`appsettings.Production.json`)
- **Issuer**: `smartrx-app-production`
- **Audience**: `smartrx-users-production`
- **Access Token Expiry**: 60 minutes (1 hour)
- **Refresh Token Expiry**: 30 days

### Default (`appsettings.json`)
- **Issuer**: `smartrx-app`
- **Audience**: `smartrx-users`
- **Access Token Expiry**: 120 minutes (2 hours)
- **Refresh Token Expiry**: 7 days

---

## API Endpoints

### 1. Login (Get Tokens)
**Endpoint**: `POST /api/Auth/Login`

**Request Body**:
```json
{
  "userName": "user@example.com",
  "authType": 1,
  "password": "YourPassword123"
}
```

**Response**:
```json
{
  "statusCode": 200,
  "status": "Success",
  "message": "User found successfully",
  "data": {
    "userId": 123,
    "authType": 1,
    "otp": "...",
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "base64EncodedRefreshToken...",
    "accessTokenExpiry": "2025-10-28T14:30:00Z",
    "refreshTokenExpiry": "2025-11-04T12:30:00Z",
    "tokenType": "Bearer"
  }
}
```

### 2. Refresh Token
**Endpoint**: `POST /api/Auth/refresh-token`

**Request Body**:
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "base64EncodedRefreshToken..."
}
```

**Response**:
```json
{
  "statusCode": 200,
  "status": "Success",
  "message": "Token refreshed successfully",
  "data": {
    "accessToken": "newAccessToken...",
    "refreshToken": "newRefreshToken...",
    "accessTokenExpiry": "2025-10-28T14:30:00Z",
    "refreshTokenExpiry": "2025-11-04T12:30:00Z",
    "tokenType": "Bearer"
  }
}
```

### 3. Revoke Token (Logout)
**Endpoint**: `POST /api/Auth/revoke-token`

**Request Body**:
```json
{
  "refreshToken": "base64EncodedRefreshToken..."
}
```

**Response**:
```json
{
  "statusCode": 200,
  "status": "Success",
  "message": "Token revoked successfully",
  "data": true
}
```

### 4. Verify OTP
**Endpoint**: `POST /api/Auth/verify-otp`

**Request Body**:
```json
{
  "userId": 123,
  "otp": "12345678"
}
```

---

## Using JWT Tokens in API Calls

### Authorization Header
For all protected endpoints (all endpoints except `/api/Auth/*`), include the access token in the Authorization header:

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Example using cURL
```bash
curl -X GET "https://your-api.com/api/User/GetAll" \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN_HERE"
```

### Example using JavaScript/Fetch
```javascript
fetch('https://your-api.com/api/User/GetAll', {
  method: 'GET',
  headers: {
    'Authorization': `Bearer ${accessToken}`,
    'Content-Type': 'application/json'
  }
})
.then(response => response.json())
.then(data => console.log(data));
```

---

## Token Refresh Flow

### When to Refresh
- Refresh the access token **before** it expires (e.g., 5 minutes before expiry)
- If you receive a `401 Unauthorized` response, attempt to refresh the token

### Recommended Client-Side Flow
```javascript
async function makeAuthenticatedRequest(url, options) {
  // Check if access token is expired or about to expire
  if (isTokenExpiringSoon(accessToken)) {
    try {
      const refreshResponse = await fetch('/api/Auth/refresh-token', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          accessToken: accessToken,
          refreshToken: refreshToken
        })
      });
      
      if (refreshResponse.ok) {
        const newTokens = await refreshResponse.json();
        // Update stored tokens
        accessToken = newTokens.data.accessToken;
        refreshToken = newTokens.data.refreshToken;
      } else {
        // Refresh failed, redirect to login
        redirectToLogin();
        return;
      }
    } catch (error) {
      redirectToLogin();
      return;
    }
  }
  
  // Make the actual request with current/refreshed token
  return fetch(url, {
    ...options,
    headers: {
      ...options.headers,
      'Authorization': `Bearer ${accessToken}`
    }
  });
}
```

---

## Security Features

### 1. Token Storage (Database)
- Refresh tokens are stored in the `Security_RefreshToken` table
- Each token is tied to a specific JWT ID (jti claim)
- Tokens can be marked as used or revoked

### 2. Token Validation
- **Access Token**: Validated on every API request
- **Refresh Token**: Must match stored token in database
- **One-Time Use**: Refresh tokens are marked as "used" after being exchanged
- **Expiry Checks**: Both token types have expiration validation

### 3. Token Revocation
- Logout revokes the refresh token
- Admin can revoke all tokens for a user
- Revoked tokens cannot be used for token refresh

### 4. Environment-Specific Keys
- Each environment uses a different secret key
- Production key should be stored securely (Azure Key Vault, AWS Secrets Manager, etc.)

---

## Protected Endpoints

All controllers except `AuthController` require authentication:

- ✅ **Protected**: UserController, PatientProfileController, DoctorController, VitalController, RoleController, etc.
- ⚠️ **Public** (Anonymous): AuthController (Login, Verify OTP, Refresh Token, Revoke Token)

---

## Database Schema

### Security_RefreshToken Table
```sql
CREATE TABLE Security_RefreshToken (
    Id BIGINT PRIMARY KEY IDENTITY,
    UserId BIGINT NOT NULL,
    Token NVARCHAR(500) NOT NULL,
    JwtId NVARCHAR(500) NOT NULL,
    IsUsed BIT NOT NULL DEFAULT 0,
    IsRevoked BIT NOT NULL DEFAULT 0,
    CreatedDate DATETIME2 NOT NULL,
    ExpiryDate DATETIME2 NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Security_PMSUser(Id) ON DELETE CASCADE
);
```

---

## Testing the Implementation

### 1. Test Login
```bash
curl -X POST "http://localhost:7000/api/Auth/Login" \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "testuser",
    "authType": 1,
    "password": "TestPassword123"
  }'
```

### 2. Test Protected Endpoint
```bash
curl -X GET "http://localhost:7000/api/User/GetAll" \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN"
```

### 3. Test Token Refresh
```bash
curl -X POST "http://localhost:7000/api/Auth/refresh-token" \
  -H "Content-Type: application/json" \
  -d '{
    "accessToken": "YOUR_OLD_ACCESS_TOKEN",
    "refreshToken": "YOUR_REFRESH_TOKEN"
  }'
```

### 4. Test Token Revocation
```bash
curl -X POST "http://localhost:7000/api/Auth/revoke-token" \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "YOUR_REFRESH_TOKEN"
  }'
```

---

## Troubleshooting

### Issue: "401 Unauthorized" on Protected Endpoints
**Solution**: Ensure you're including the `Authorization: Bearer {token}` header

### Issue: Token Refresh Fails
**Solution**: 
- Check if refresh token has expired
- Verify token hasn't been used or revoked
- Ensure access token and refresh token pair match

### Issue: Wrong JWT Configuration Loaded
**Solution**: 
- Check `ASPNETCORE_ENVIRONMENT` variable
- Verify the correct `appsettings.{Environment}.json` file exists
- Check console output on startup for environment info

### Issue: "Invalid token" Error
**Solution**:
- Token may have expired
- Token may have been revoked
- Secret key mismatch (check environment)
- Issuer/Audience mismatch

---

## Best Practices

1. **Store Tokens Securely**: Use httpOnly cookies or secure storage on client-side
2. **Short Access Token Lifetime**: Keep access tokens short-lived (15-60 minutes)
3. **Long Refresh Token Lifetime**: Refresh tokens can be longer (7-30 days)
4. **Automatic Token Refresh**: Implement automatic refresh before expiry
5. **Logout = Revoke**: Always revoke refresh tokens on logout
6. **HTTPS Only**: Use HTTPS in production
7. **Rate Limiting**: Implement rate limiting on auth endpoints
8. **Monitor Failed Attempts**: Log and monitor failed authentication attempts

---

## Environment Variables (Optional Override)

You can override any setting using environment variables:

```bash
# Override JWT settings
export JwtSettings__SecretKey="your-custom-secret-key"
export JwtSettings__Issuer="custom-issuer"
export JwtSettings__Audience="custom-audience"
export JwtSettings__ExpiryMinutes="60"
export JwtSettings__RefreshTokenExpiryDays="30"
```

---

## Migration Command

To create the RefreshToken table in the database:

```bash
cd PMSBackend.Databases
dotnet ef migrations add AddRefreshTokenEntity
dotnet ef database update
```

Or run the application and it will auto-migrate on startup (if configured).

---

## Support

For issues or questions:
1. Check the console output on application startup
2. Verify environment is set correctly
3. Check JWT configuration in the appropriate appsettings file
4. Review logs for authentication errors

