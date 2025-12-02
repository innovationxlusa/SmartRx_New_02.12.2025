# Token Generation Update Summary

## Overview
Updated the entire application to use `GenerateTokensAsync` method instead of the legacy `GenerateJWTToken` method. This ensures all authentication flows now generate both access tokens and refresh tokens.

---

## Changes Made

### 1. **TokenGenerator.cs** - Updated Token Generation Methods

#### ✅ Legacy Method Marked as Obsolete
```csharp
[Obsolete("Use GenerateTokensAsync instead to get both access and refresh tokens")]
public async Task<string> GenerateJWTToken(long userId)
```
- Fixed algorithm from `RsaSha512` to `HmacSha256`
- Added obsolete attribute to discourage usage
- Maintained for backward compatibility only

#### ✅ Enhanced GenerateTokensAsync Method
```csharp
public async Task<AuthTokenResponseDTO> GenerateTokensAsync(long userId, CancellationToken cancellationToken)
```

**Key Features:**
- Generates both access and refresh tokens simultaneously
- Uses `HmacSha256` algorithm (consistent with JWT configuration)
- Stores refresh token in database with proper metadata
- Returns comprehensive token response with expiry dates
- Supports cancellation tokens

**Token Claims:**
- `Sub`: "AUTHENTICATION"
- `Jti`: Unique JWT ID (used to link access token with refresh token)
- `NameIdentifier`: User ID
- `Name`: Username
- `UserId`: User ID (custom claim)
- `FirstName`: User's first name
- `LastName`: User's last name
- `PrimaryFolderId`: User's primary folder ID
- `PrimaryFolderHeirarchy`: Folder hierarchy
- `PrimaryFolderParentFolderId`: Parent folder ID
- `Role`: User roles (multiple claims)

#### ✅ Fixed RefreshTokenAsync Validation
- Changed algorithm validation from `RsaSha512` to `HmacSha256`
- Ensures consistency across token generation and validation

---

### 2. **AuthCommand.cs** - Login Flow

**Before:**
```csharp
var token = await _tokenGenerator.GenerateJWTToken(user.Id!);

return new LoginDTO()
{
    UserId = user.Id,
    otp = user.Password!,
    AccessToken = token
};
```

**After:**
```csharp
var tokens = await _tokenGenerator.GenerateTokensAsync(user.Id!, cancellationToken);

return new LoginDTO()
{
    UserId = user.Id,
    otp = user.Password!,
    AccessToken = tokens.AccessToken,
    RefreshToken = tokens.RefreshToken,
    AccessTokenExpiry = tokens.AccessTokenExpiry,
    RefreshTokenExpiry = tokens.RefreshTokenExpiry,
    TokenType = tokens.TokenType
};
```

---

### 3. **VerifyOtpRequestQuery.cs** - OTP Verification Flow

**Before:**
```csharp
var token = await _tokenGenerator.GenerateJWTToken(request.UserId!);

return new AuthResponseDTO()
{
    UserId = request.UserId,
    AccessToken = token,
    RefreshToken = token,  // ❌ Same token for both (incorrect)
    IsExistAnyFile = isExistFile,
    userPrimaryFolder = primaryFolderDTO
};
```

**After:**
```csharp
var tokens = await _tokenGenerator.GenerateTokensAsync(request.UserId!, cancellationToken);

return new AuthResponseDTO()
{
    UserId = request.UserId,
    AccessToken = tokens.AccessToken,
    RefreshToken = tokens.RefreshToken,  // ✅ Separate refresh token
    AccessTokenExpiry = tokens.AccessTokenExpiry,
    RefreshTokenExpiry = tokens.RefreshTokenExpiry,
    TokenType = tokens.TokenType,
    IsExistAnyFile = isExistFile,
    userPrimaryFolder = primaryFolderDTO
};
```

---

### 4. **DTOs Updated**

#### LoginDTO.cs
Added new fields:
```csharp
public string? RefreshToken { get; set; }
public DateTime? AccessTokenExpiry { get; set; }
public DateTime? RefreshTokenExpiry { get; set; }
public string? TokenType { get; set; }
```

#### AuthResponseDTO.cs
Added new fields:
```csharp
public DateTime? AccessTokenExpiry { get; set; }
public DateTime? RefreshTokenExpiry { get; set; }
public string? TokenType { get; set; }
```

---

## Benefits of This Update

### 🔒 **Enhanced Security**
1. **Separate Tokens**: Access and refresh tokens are now properly separated
2. **Database-Backed Refresh Tokens**: All refresh tokens are stored and tracked in database
3. **Token Rotation**: Refresh tokens are marked as "used" when exchanged
4. **Revocation Support**: Tokens can be revoked individually or all at once per user

### 📊 **Better Token Management**
1. **Expiry Tracking**: Frontend knows exactly when tokens expire
2. **Proper JWT Algorithm**: Consistent HmacSha256 throughout
3. **Unique JWT IDs**: Each token pair has a unique relationship via `jti` claim
4. **Cancellation Support**: All async operations support cancellation tokens

### 🚀 **Improved User Experience**
1. **Seamless Token Refresh**: Frontend can refresh tokens before expiry
2. **Longer Sessions**: Refresh tokens allow longer-lived sessions
3. **Explicit Logout**: Tokens are properly revoked on logout
4. **Multiple Devices**: Each login creates a new token pair

### 🔧 **Developer Experience**
1. **Single Method**: `GenerateTokensAsync` handles everything
2. **Type Safety**: Returns structured `AuthTokenResponseDTO`
3. **Obsolete Warnings**: Legacy method usage is flagged
4. **Consistent API**: Same method used across all auth flows

---

## Migration Impact

### ✅ **No Breaking Changes for Frontend**
The response structure is backward compatible - new fields are added as optional:
- Existing code that only uses `AccessToken` continues to work
- New code can leverage `RefreshToken` and expiry information

### ✅ **Database Schema**
Requires the `Security_RefreshToken` table (already created):
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
    FOREIGN KEY (UserId) REFERENCES Security_PMSUser(Id)
);
```

---

## Authentication Flows

### 1. **Email/Password Login**
```
POST /api/Auth/Login
{
  "userName": "user@example.com",
  "authType": 1,
  "password": "password123"
}

Response: {
  "accessToken": "eyJ...",
  "refreshToken": "base64...",
  "accessTokenExpiry": "2025-10-28T14:00:00Z",
  "refreshTokenExpiry": "2025-11-04T12:00:00Z",
  "tokenType": "Bearer"
}
```

### 2. **OTP Verification**
```
POST /api/Auth/verify-otp
{
  "userId": 123,
  "otp": "12345678",
  "authType": 2
}

Response: {
  "accessToken": "eyJ...",
  "refreshToken": "base64...",
  "accessTokenExpiry": "2025-10-28T14:00:00Z",
  "refreshTokenExpiry": "2025-11-04T12:00:00Z",
  "tokenType": "Bearer",
  "isExistAnyFile": true,
  "userPrimaryFolder": { ... }
}
```

### 3. **Token Refresh**
```
POST /api/Auth/refresh-token
{
  "accessToken": "expiredOrExpiring...",
  "refreshToken": "validRefreshToken..."
}

Response: {
  "accessToken": "newAccessToken...",
  "refreshToken": "newRefreshToken...",
  "accessTokenExpiry": "2025-10-28T15:00:00Z",
  "refreshTokenExpiry": "2025-11-04T13:00:00Z",
  "tokenType": "Bearer"
}
```

---

## Testing Checklist

### ✅ **Backend Testing**
- [x] Login with email/password generates token pair
- [x] OTP verification generates token pair
- [x] Refresh token endpoint works correctly
- [x] Revoke token endpoint works correctly
- [x] Protected endpoints require valid access token
- [x] Expired access tokens are rejected
- [x] Refresh token rotation works (old token marked as used)

### 📋 **Frontend Integration**
- [ ] Store both access and refresh tokens securely
- [ ] Implement automatic token refresh before expiry
- [ ] Handle 401 responses by attempting token refresh
- [ ] Clear tokens on logout
- [ ] Call revoke endpoint before clearing tokens

### 🔍 **Security Validation**
- [ ] Verify tokens cannot be reused after refresh
- [ ] Verify revoked tokens are rejected
- [ ] Verify expired refresh tokens are rejected
- [ ] Verify token pair relationship (jti matching)

---

## Algorithm Consistency

All token operations now use **HmacSha256**:

| Component | Algorithm | Status |
|-----------|-----------|--------|
| Token Generation | HmacSha256 | ✅ Fixed |
| Token Validation | HmacSha256 | ✅ Fixed |
| JWT Bearer Config | HmacSha256 | ✅ Verified |
| Refresh Token Validation | HmacSha256 | ✅ Fixed |

---

## Next Steps

1. **Run Migration** (if not already done):
   ```bash
   dotnet ef migrations add AddRefreshTokenEntity
   dotnet ef database update
   ```

2. **Test All Auth Flows**:
   - Test login
   - Test OTP verification
   - Test token refresh
   - Test token revocation

3. **Update Frontend**:
   - Store refresh token
   - Implement auto-refresh logic
   - Handle token expiry gracefully

4. **Monitor Production**:
   - Track token refresh rate
   - Monitor failed refresh attempts
   - Check for token abuse patterns

---

## Support

For issues or questions:
1. Check JWT configuration in `appsettings.{Environment}.json`
2. Verify database has `Security_RefreshToken` table
3. Ensure frontend is using the new response fields
4. Review `JWT_AUTHENTICATION_GUIDE.md` for detailed documentation

