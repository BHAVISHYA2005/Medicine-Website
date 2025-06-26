# Backend Deployment Configuration

## Environment Files

### `.env` (Production - Railway)
- Used for Railway deployment
- Contains only production environment variables
- **Important:** PORT is NOT set here - Railway assigns it dynamically

### `.env.local` (Local Development)
- Used for local development only
- Contains local configuration including PORT=5000
- **Never commit this file** - it's in .gitignore

## Railway Deployment

Railway will automatically:
1. Detect the `railway.json` configuration
2. Assign a dynamic PORT environment variable
3. Use the start command: `dotnet run --project MedicineWebsite.API --urls http://0.0.0.0:$PORT`

## Local Development

For local development, copy `.env.local` to your local environment or use:

```bash
dotnet run --project MedicineWebsite.API --urls http://localhost:5000
```

## Environment Variables

### Required for Production:
- `ASPNETCORE_ENVIRONMENT=Production`
- `PORT` (auto-assigned by Railway)

### Optional (add as needed):
- `DATABASE_URL` (when using external database)
- `JWT_SECRET`
- `CORS_ORIGINS`
- `EMAIL_*` settings for email functionality

## Port Configuration

- **Local Development:** Uses PORT=5000 from `.env.local`
- **Railway Deployment:** Uses dynamic PORT assigned by Railway
- **Docker:** Uses PORT from environment or defaults to 80

This ensures no port conflicts during deployment while maintaining local development convenience.
