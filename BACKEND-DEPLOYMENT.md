# Backend Deployment Configuration

## 🏗️ .NET 8 API Deployment Options

### Option 1: Azure App Service (Recommended)

#### Prerequisites
```bash
# Install Azure CLI
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
az login
```

#### Deploy to Azure
```bash
cd Backend/MedicineWebsite
az webapp up --runtime "DOTNET:8.0" --name your-medicine-api --resource-group your-resource-group
```

#### Configuration
Add these application settings in Azure:
- `ASPNETCORE_ENVIRONMENT`: Production
- `ConnectionStrings__DefaultConnection`: Your database connection string
- `JwtSettings__Secret`: Your JWT secret key
- `JwtSettings__Issuer`: Your domain
- `JwtSettings__Audience`: Your domain

### Option 2: Railway

#### Deploy via Railway
1. Visit [railway.app](https://railway.app)
2. Click "Deploy from GitHub repo"
3. Select your repository
4. Railway will auto-detect .NET project
5. Set root directory to `Backend/MedicineWebsite`

#### Environment Variables
Add in Railway dashboard:
```
ASPNETCORE_ENVIRONMENT=Production
PORT=8080
```

### Option 3: Render

#### Deploy via Render
1. Visit [render.com](https://render.com)
2. Create new "Web Service"
3. Connect your repository
4. Configure:
   - **Root Directory:** `Backend/MedicineWebsite`
   - **Build Command:** `dotnet publish -c Release -o out`
   - **Start Command:** `dotnet out/MedicineWebsite.API.dll`

### Option 4: Docker Deployment

#### Create Dockerfile
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "MedicineWebsite.API/MedicineWebsite.API.csproj"
RUN dotnet build "MedicineWebsite.API/MedicineWebsite.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MedicineWebsite.API/MedicineWebsite.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MedicineWebsite.API.dll"]
```

## 🔧 Production Configuration Updates

### Update Program.cs for Production
Add these configurations for production deployment:
```csharp
// For production, you'll want to replace InMemory database
// with a real database like SQL Server, PostgreSQL, or MySQL

if (builder.Environment.IsProduction())
{
    // Use real database
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}
else
{
    // Keep InMemory for development
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("MedicineDb"));
}
```

### Environment Variables Needed
- `ConnectionStrings__DefaultConnection`
- `JwtSettings__Secret`
- `JwtSettings__Issuer`
- `JwtSettings__Audience`
- `ASPNETCORE_ENVIRONMENT=Production`

## 📡 API Endpoints Available
Once deployed, your API will be available at:
- `GET /api/medicines` - Get all medicines
- `GET /api/medicines/search` - Search medicines
- `GET /api/medicines/{id}` - Get medicine by ID

## 🔗 Update Frontend After Backend Deployment
1. Note your deployed backend URL (e.g., `https://your-api.azurewebsites.net`)
2. Update `Frontend/medicine-frontend/src/app/environment/environment.prod.ts`:
   ```typescript
   export const environment = {
     production: true,
     apiUrl: 'https://your-api.azurewebsites.net/api'
   };
   ```
3. Rebuild and redeploy frontend
