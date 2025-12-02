# Quick Start: Get JWT Token for Postman

## 🚀 Get Your Token in 3 Steps

### Option A: Direct Token Generation (Fastest - For Testing Only)
**Best for:** Quick testing when you know the UserId

**Postman Request:**
- **Method:** `POST`
- **URL:** `http://localhost:7000/api/Auth/generate-token`
- **Headers:** 
  - `Content-Type: application/json`
- **Body (JSON):**
```json
{
  "userId": 1
}
```

**User IDs:**
- `1` = Super Admin
- `2` = Admin  
- `3` = Entry User

---

### Option B: Login to Get Token (Recommended - Production-like)

### Step 1: Start Your Backend
```bash
cd PMSBackend
dotnet run
```
Backend will run at: `http://localhost:7000` (or port configured in launchSettings.json)

### Step 2: Login to Get Token

**Postman Request:**
- **Method:** `POST`
- **URL:** `http://localhost:7000/api/Auth/Login`
- **Headers:** 
  - `Content-Type: application/json`
- **Body (JSON):**
```json
{
  "userName": "superadmin",
  "authType": 1,
  "password": "1234"
}
```

**Response:**
```json
{
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "random-base64-string",
    "accessTokenExpiry": "2025-01-28T10:30:00Z",
    "tokenType": "Bearer"
  }
}
```

### Step 3: Use Token in Protected Endpoints

**Add Authorization Header:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 📝 Example: Get All Users

**Postman Request:**
- **Method:** `GET`
- **URL:** `http://localhost:7000/api/User`
- **Headers:**
  - `Authorization: Bearer YOUR_ACCESS_TOKEN_HERE`

## 🔄 When Token Expires (after 2 hours)

**Refresh Token Request:**
- **Method:** `POST`
- **URL:** `http://localhost:7000/api/Auth/refresh-token`
- **Body (JSON):**
```json
{
  "accessToken": "your-expired-token",
  "refreshToken": "your-refresh-token"
}
```

## 👥 Available Test Users

| Username | Password |
|----------|----------|
| superadmin | 1234 |
| admin | 1234 |
| entryuser | 1234 |

## 🛠️ Quick Tips

1. **Copy the entire token** - don't miss any characters
2. **Use Bearer prefix** - `Bearer YOUR_TOKEN`
3. **Token expires in 2 hours** - use refresh endpoint
4. **Check server logs** if login fails
5. **Verify user exists** in database if authentication fails

## 📚 More Details

For comprehensive guide, see: [POSTMAN_JWT_TESTING_GUIDE.md](POSTMAN_JWT_TESTING_GUIDE.md)

## ❌ Common Issues

**"401 Unauthorized"**
- Token expired → Use refresh-token endpoint
- Missing Bearer prefix → Add `Bearer ` before token
- Token corrupted → Login again to get new token

**"Invalid User!"**
- Wrong username/password
- User not active in database
- User doesn't exist

**"Token has been revoked"**
- Refresh token was manually revoked
- User logged out
- Security measure triggered

## 🔐 Security Notes

- Never commit tokens to git
- Use HTTPS in production
- Refresh tokens last 7 days
- Store tokens securely (httpOnly cookies in browsers)

