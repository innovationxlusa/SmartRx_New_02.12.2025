# JWT Token Testing Guide for Postman

This guide explains how to obtain and use JWT tokens from the SmartRx backend API for testing in Postman.

## Overview

The SmartRx backend uses JWT (JSON Web Token) authentication with two types of tokens:
1. **Access Token** - Short-lived (120 minutes default) for API requests
2. **Refresh Token** - Long-lived (7 days) for getting new access tokens

## Authentication Endpoints

### 0. Generate Token Endpoint (Quick Testing - Development Only)

**Endpoint:** `POST /api/Auth/generate-token`

**Purpose:** Quick token generation for testing purposes. This bypasses authentication and generates tokens directly for a given UserId.

**⚠️ Development Use Only:** This endpoint should NOT be used in production. It's designed for quick Postman testing.

**Request Headers:**
```
Content-Type: application/json
```

**Request Body:**
```json
{
  "userId": 1
}
```

**Available User IDs:**
- `1` = Super Admin
- `2` = Admin
- `3` = Entry User

**Example Response:**
```json
{
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "base64-encrypted-random-string",
    "accessTokenExpiry": "2025-01-28T10:30:00.000Z",
    "refreshTokenExpiry": "2025-02-04T08:30:00.000Z",
    "tokenType": "Bearer"
  },
  "statusCode": 200,
  "status": "Success",
  "message": "Token generated successfully"
}
```

---

### 1. Login Endpoint (Get Access & Refresh Tokens)

**Endpoint:** `POST /api/Auth/Login`

**Purpose:** Authenticate with username and password to get both access and refresh tokens.

**Request Headers:**
```
Content-Type: application/json
```

**Request Body:**
```json
{
  "userName": "your-username",
  "authType": 1,
  "password": "your-password"
}
```

**AuthType Values:**
- `1` = Email authentication (requires password)
- `2` = Mobile authentication (OTP-based)

**Example Response:**
```json
{
  "data": {
    "userId": 1,
    "authType": 1,
    "otp": "password-hash",
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "base64-encrypted-random-string",
    "accessTokenExpiry": "2025-01-28T10:30:00.000Z",
    "refreshTokenExpiry": "2025-02-04T08:30:00.000Z",
    "tokenType": "Bearer"
  },
  "statusCode": 200,
  "status": "Success",
  "message": "User found successfully"
}
```

---

### 2. Refresh Token Endpoint

**Endpoint:** `POST /api/Auth/refresh-token`

**Purpose:** Get a new access token when the current one expires.

**Request Headers:**
```
Content-Type: application/json
```

**Request Body:**
```json
{
  "accessToken": "expired-access-token",
  "refreshToken": "valid-refresh-token"
}
```

**Example Response:**
```json
{
  "data": {
    "accessToken": "new-access-token",
    "refreshToken": "new-refresh-token",
    "accessTokenExpiry": "2025-01-28T12:30:00.000Z",
    "refreshTokenExpiry": "2025-02-04T10:30:00.000Z",
    "tokenType": "Bearer"
  },
  "statusCode": 200,
  "status": "Success",
  "message": "Token refreshed successfully"
}
```

---

### 3. Revoke Token Endpoint

**Endpoint:** `POST /api/Auth/revoke-token`

**Purpose:** Revoke a specific refresh token (logout from specific device).

**Request Body:**
```json
{
  "refreshToken": "refresh-token-to-revoke"
}
```

---

### 4. Revoke All User Tokens Endpoint

**Endpoint:** `POST /api/Auth/revoke-all-user-tokens`

**Purpose:** Revoke all tokens for a user (logout from all devices).

**Request Body:**
```json
{
  "userId": 1
}
```

---

## Step-by-Step Postman Setup

### Step 1: Get Your Token

1. Open Postman and create a new request
2. Set method to `POST`
3. Set URL: `http://localhost:5270/api/Auth/Login` (adjust host/port as needed)
4. Go to **Headers** tab, add:
   - Key: `Content-Type`, Value: `application/json`
5. Go to **Body** tab, select **raw** and **JSON**, enter:
   ```json
   {
     "userName": "superadmin",
     "authType": 1,
     "password": "1234"
   }
   ```
6. Click **Send**
7. Copy the `accessToken` from the response

### Step 2: Use Token in Protected Endpoints

1. Create a new request for any protected endpoint (e.g., `GET /api/User`)
2. Go to **Headers** tab
3. Add authorization header:
   - Key: `Authorization`
   - Value: `Bearer YOUR_ACCESS_TOKEN_HERE`
   - Replace `YOUR_ACCESS_TOKEN_HERE` with the actual token from Step 1
4. Click **Send**

### Step 3: Set Up Postman Environment (Recommended)

For easier token management:

1. Click **Environments** in left sidebar
2. Click **+** to create new environment (name it "SmartRx Dev")
3. Add variables:
   - `base_url` = `http://localhost:5270`
   - `access_token` = (leave empty initially)
   - `refresh_token` = (leave empty initially)
4. Set active environment to "SmartRx Dev"
5. Update your requests:
   - URL: `{{base_url}}/api/Auth/Login`
   - In Login response, add a script in **Tests** tab:
     ```javascript
     if (pm.response.code === 200) {
         var jsonData = pm.response.json();
         pm.environment.set("access_token", jsonData.data.accessToken);
         pm.environment.set("refresh_token", jsonData.data.refreshToken);
     }
     ```
   - In protected requests, set Authorization header to: `Bearer {{access_token}}`

---

## JWT Token Details

### Access Token Claims

The access token contains these claims:
- `sub`: "AUTHENTICATION"
- `jti`: Unique token ID
- `name`: Username
- `UserId`: User ID
- `FirstName`: User's first name
- `LastName`: User's last name
- `PrimaryFolderId`: User's primary folder ID
- `PrimaryFolderHeirarchy`: Folder hierarchy level
- `PrimaryFolderParentFolderId`: Parent folder ID
- `role`: User roles (may have multiple)

### Token Configuration

Current settings (in `appsettings.json`):
- **Secret Key**: Configured in appsettings
- **Issuer**: `smartrx-app`
- **Audience**: `smartrx-users`
- **Access Token Expiry**: 120 minutes (2 hours)
- **Refresh Token Expiry**: 7 days

---

## Common Issues & Solutions

### Issue: "401 Unauthorized"
**Solution**: 
- Check that the token is not expired
- Ensure you're using `Bearer ` prefix before the token
- Verify the token is complete (not truncated)
- Use refresh token endpoint to get a new access token

### Issue: "Token has been revoked"
**Solution**: 
- The refresh token has been manually revoked or expired
- Login again to get new tokens

### Issue: "Invalid User!"
**Solution**:
- Check username and password are correct
- Ensure the user exists and is active in the database
- For AuthType 2 (Mobile), ensure proper OTP flow

---

## Testing Flow Example

### Quick Testing Flow (Using Generate Token)
```http
### 0. Generate token directly (fastest for testing)
POST {{base_url}}/api/Auth/generate-token
Content-Type: application/json

{
  "userId": 1
}

### Use the access token in protected endpoints
GET {{base_url}}/api/User
Authorization: Bearer {{access_token}}
```

### Full Authentication Flow (Production-like)
```http
### 1. Login to get tokens
POST {{base_url}}/api/Auth/Login
Content-Type: application/json

{
  "userName": "superadmin",
  "authType": 1,
  "password": "1234"
}

### 2. Use access token in protected endpoint
GET {{base_url}}/api/User
Authorization: Bearer {{access_token}}

### 3. Refresh token when access token expires
POST {{base_url}}/api/Auth/refresh-token
Content-Type: application/json

{
  "accessToken": "{{access_token}}",
  "refreshToken": "{{refresh_token}}"
}

### 4. Logout (revoke specific token)
POST {{base_url}}/api/Auth/revoke-token
Content-Type: application/json

{
  "refreshToken": "{{refresh_token}}"
}

### 5. Logout from all devices
POST {{base_url}}/api/Auth/revoke-all-user-tokens
Content-Type: application/json

{
  "userId": 1
}
```

---

## Security Notes

1. **Never commit tokens** to version control
2. **Access tokens expire** after 2 hours for security
3. **Always use HTTPS** in production
4. **Refresh tokens** should be stored securely (e.g., httpOnly cookies in browsers)
5. **Revoke tokens** when logout happens

---

## Test Users

The following test users are available in the database by default:

| Username | Password | Role | Description |
|----------|----------|------|-------------|
| superadmin | 1234 | Super Admin | Full system access |
| admin | 1234 | Admin | General admin access |
| entryuser | 1234 | Entry User | Data entry access |

## Database Notes

Make sure you have valid users in your database. To check/create users:

1. Connect to your SQL Server database (SmartRxDB2 or SmartRxDB3)
2. Check the `Security_PMSUsers` table for existing users
3. Users must have:
   - Active status
   - Valid username and password
   - Assigned roles via `UserRoles` table

---

## Need Help?

If you encounter issues:
1. Check the backend logs for detailed error messages
2. Verify database connection and user exists
3. Ensure the backend is running on the correct port
4. Check appsettings.json for JWT configuration

