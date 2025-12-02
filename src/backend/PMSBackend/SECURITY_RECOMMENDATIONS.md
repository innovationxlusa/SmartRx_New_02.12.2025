# JWT Authentication Security Recommendations

## 🔒 Critical Security Fixes Applied

### 1. **ClockSkew Configuration** ✅ FIXED
**Previous Issue**: ClockSkew was set to the same value as token expiry (120 minutes), effectively doubling token lifetime.

**Fix Applied**: Set to `TimeSpan.Zero` for strict expiration enforcement.

```csharp
ClockSkew = TimeSpan.Zero // Tokens expire exactly at their expiry time
```

**Why This Matters**: ClockSkew is meant to handle small time sync issues between servers (typically 0-5 minutes), not extend token lifetime. The previous configuration meant tokens were valid for up to 240 minutes (4 hours) instead of 120 minutes.

---

## 🛡️ Security Features Implemented

### 1. **Refresh Token Security**
- ✅ Refresh tokens are stored in database with proper tracking
- ✅ One-time use: Tokens are marked as "used" after exchange
- ✅ Token rotation: New token pair generated on each refresh
- ✅ Automatic revocation on suspicious activity (token reuse, JTI mismatch)
- ✅ Comprehensive logging for security monitoring

### 2. **Token Validation**
- ✅ Proper algorithm validation (HmacSha256)
- ✅ Issuer/Audience validation
- ✅ Expiry validation with zero clock skew
- ✅ JTI (JWT ID) matching between access and refresh tokens

### 3. **Enhanced Logging**
- ✅ All token refresh attempts are logged
- ✅ Failed validations are logged with reasons
- ✅ Suspicious activities trigger security alerts
- ✅ Automatic token revocation on potential security breaches

---

## 🔐 Production Security Checklist

### **Critical - Must Do Before Production**

#### 1. **Secret Key Management** 🔴
**Current State**: Secret keys are in appsettings files

**Action Required**:
```bash
# Option A: Use Environment Variables
export JwtSettings__SecretKey="your-production-secret-key-here-minimum-64-characters-recommended"

# Option B: Use Azure Key Vault (Recommended)
# Add to Program.cs:
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{keyVaultName}.vault.azure.net/"),
    new DefaultAzureCredential()
);
```

**Generate Strong Key**:
```bash
# PowerShell - Generate 256-bit key
[Convert]::ToBase64String([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(64))

# Linux/Mac - Generate 256-bit key
openssl rand -base64 64
```

#### 2. **HTTPS Enforcement** 🔴
**Current State**: `RequireHttpsMetadata = false` in JWT configuration

**Action Required**:
```csharp
// In DependencyInjection.cs - Change to:
x.RequireHttpsMetadata = true; // Enforce HTTPS in production
```

**Also add HTTPS redirection** in `Program.cs`:
```csharp
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}
```

#### 3. **Connection String Security** 🔴
**Current State**: Connection strings in config files

**Action Required**:
```bash
# Use environment variables
export ConnectionStrings__PMSDBConnection="Server=...;Database=...;User Id=...;Password=...;TrustServerCertificate=False;Encrypt=True;"

# Or use Azure Key Vault / AWS Secrets Manager
```

#### 4. **Rate Limiting** 🟡
**Current State**: No rate limiting on auth endpoints

**Action Required**:
```bash
# Install package
dotnet add package AspNetCoreRateLimit
```

```csharp
// In Program.cs
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "POST:/api/Auth/*",
            Period = "1m",
            Limit = 5, // 5 requests per minute
        }
    };
});
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddInMemoryRateLimiting();

// In middleware
app.UseIpRateLimiting();
```

---

## 🔍 Security Monitoring

### **Log Monitoring**
Monitor these log patterns for security issues:

#### 1. **Token Reuse Attempts**
```
Token refresh failed: Refresh token already used or revoked for user {UserId}. Potential security breach!
```
**Action**: Investigate user account, check for compromised credentials

#### 2. **JTI Mismatch**
```
Token refresh failed: JTI mismatch for user {UserId}. Potential token theft!
```
**Action**: All user tokens are automatically revoked. Notify user to re-login.

#### 3. **Failed Refresh Attempts**
```
Token refresh failed: [reason]
```
**Action**: Track frequency per user/IP. Implement alerting for unusual patterns.

### **Database Monitoring**
Query to find suspicious activity:
```sql
-- Find tokens that were used after being marked as used (security breach indicator)
SELECT * FROM Security_RefreshToken
WHERE IsUsed = 1 AND ExpiryDate > GETUTCDATE()
ORDER BY CreatedDate DESC;

-- Find users with multiple active tokens (normal for multiple devices)
SELECT UserId, COUNT(*) as ActiveTokens
FROM Security_RefreshToken
WHERE IsUsed = 0 AND IsRevoked = 0 AND ExpiryDate > GETUTCDATE()
GROUP BY UserId
HAVING COUNT(*) > 5; -- Alert if > 5 devices

-- Find recently revoked tokens (check for patterns)
SELECT UserId, COUNT(*) as RevokedCount
FROM Security_RefreshToken
WHERE IsRevoked = 1 AND CreatedDate > DATEADD(day, -7, GETUTCDATE())
GROUP BY UserId
ORDER BY RevokedCount DESC;
```

---

## 🚀 Production Deployment Steps

### **Pre-Deployment**
1. ✅ Generate new production secret keys (minimum 64 characters)
2. ✅ Store secrets in Azure Key Vault or AWS Secrets Manager
3. ✅ Configure HTTPS certificates
4. ✅ Set `RequireHttpsMetadata = true`
5. ✅ Implement rate limiting
6. ✅ Set up log aggregation (e.g., Application Insights, Seq, ELK)
7. ✅ Configure connection strings via environment variables
8. ✅ Review CORS settings (remove `localhost` entries)

### **Deployment**
```bash
# Set environment
export ASPNETCORE_ENVIRONMENT=Production

# Set secrets via environment variables (don't store in files!)
export JwtSettings__SecretKey="<your-production-key>"
export ConnectionStrings__PMSDBConnection="<your-production-connection>"

# Run migrations
dotnet ef database update

# Start application
dotnet PMSBackend.API.dll
```

### **Post-Deployment Validation**
```bash
# 1. Test login
curl -X POST "https://your-api.com/api/Auth/Login" \
  -H "Content-Type: application/json" \
  -d '{"userName":"test","authType":1,"password":"test"}'

# 2. Test token refresh
curl -X POST "https://your-api.com/api/Auth/refresh-token" \
  -H "Content-Type: application/json" \
  -d '{"accessToken":"...","refreshToken":"..."}'

# 3. Test protected endpoint
curl -X GET "https://your-api.com/api/User/GetAll" \
  -H "Authorization: Bearer <access-token>"

# 4. Verify HTTPS (should NOT work)
curl -X POST "http://your-api.com/api/Auth/Login" # Should redirect or fail
```

---

## 📊 Additional Security Best Practices

### **1. Token Lifetime Strategy**
| Environment | Access Token | Refresh Token | Rationale |
|-------------|--------------|---------------|-----------|
| Development | 120 min | 7 days | Convenience for testing |
| Staging | 90 min | 14 days | Balance testing/security |
| Production | **15-60 min** | **7-30 days** | Security best practice |

**Recommendation**: Reduce production access token to 15-30 minutes for maximum security.

### **2. CORS Configuration**
**Current**: Allows specific IPs/domains
**Production**: Remove all development URLs
```csharp
.WithOrigins(
    "https://your-production-domain.com",
    "https://your-mobile-app-domain.com"
)
```

### **3. Secure Cookie Storage (Frontend)**
If using cookies for token storage:
```javascript
// Use HttpOnly, Secure, SameSite cookies
document.cookie = `accessToken=${token}; Secure; HttpOnly; SameSite=Strict`;
```

### **4. Token Cleanup Job**
Implement a background job to clean up expired tokens:
```csharp
// Create a background service
public class TokenCleanupService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Delete tokens expired > 30 days ago
            await CleanupExpiredTokens();
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}
```

### **5. Multi-Factor Authentication (MFA)**
Consider adding MFA for sensitive operations:
- Account changes
- Password resets
- Large transactions
- Administrative actions

---

## 🔧 Environment-Specific Settings

### **Development**
```json
{
  "JwtSettings": {
    "ExpiryMinutes": 120,
    "RefreshTokenExpiryDays": 7
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  }
}
```

### **Production**
```json
{
  "JwtSettings": {
    "ExpiryMinutes": 30,
    "RefreshTokenExpiryDays": 30
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "PMSBackend": "Information"
    }
  }
}
```

---

## 📞 Security Incident Response

### **If Tokens Are Compromised**

#### 1. **Immediate Actions**
```bash
# Revoke all tokens for affected user
POST /api/Auth/revoke-all-user-tokens
{
  "userId": 123
}

# Force password reset
# Notify user via email/SMS

# Review access logs
SELECT * FROM AuditLog 
WHERE UserId = 123 
AND Timestamp > '<incident-time>'
ORDER BY Timestamp DESC;
```

#### 2. **System-Wide Breach**
```bash
# Generate new secret key
export JwtSettings__SecretKey="<new-key>"

# Restart application (invalidates all tokens)
# Force all users to re-login

# Review all activity during breach period
```

#### 3. **Post-Incident**
- Review and update security measures
- Conduct security audit
- Update documentation
- Train team on security best practices

---

## 📚 Additional Resources

- [OWASP JWT Security Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/JSON_Web_Token_for_Java_Cheat_Sheet.html)
- [JWT.io - Token Decoder/Debugger](https://jwt.io/)
- [Microsoft Identity Platform Best Practices](https://docs.microsoft.com/en-us/azure/active-directory/develop/identity-platform-integration-checklist)

---

## ✅ Summary

### **What's Been Fixed**
1. ✅ ClockSkew configuration corrected
2. ✅ Added comprehensive logging
3. ✅ Implemented automatic security responses
4. ✅ Added RevokeAllUserTokens endpoint
5. ✅ Cleaned up dead code
6. ✅ Fixed production configuration

### **What Needs Your Attention**
1. 🔴 Generate and store production secret keys securely
2. 🔴 Enable HTTPS enforcement for production
3. 🔴 Configure production connection strings
4. 🟡 Implement rate limiting
5. 🟡 Set up log monitoring and alerting
6. 🟢 Consider reducing access token lifetime in production

---

**Last Updated**: October 28, 2025
**Version**: 1.0
**Status**: Ready for Production (with required configurations)

