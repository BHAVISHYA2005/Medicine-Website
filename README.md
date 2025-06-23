# Medicine Website Platform

A comprehensive online medicine platform connecting patients with nearby pharmacies.

## 🚀 Deployment Guide

### Frontend Deployment to Netlify

#### Option 1: Deploy via Netlify Web Interface (Recommended)

1. **Prepare your repository:**
   ```bash
   # Make sure your code is committed to Git
   git add .
   git commit -m "Prepare for Netlify deployment"
   git push origin main
   ```

2. **Deploy on Netlify:**
   - Visit [netlify.com](https://netlify.com) and sign in
   - Click "New site from Git"
   - Connect your GitHub/GitLab/Bitbucket repository
   - Select your medicine website repository
   - Configure build settings:
     - **Build command:** `npm run build`
     - **Publish directory:** `dist/medicine-frontend/browser`
     - **Base directory:** `Frontend/medicine-frontend`
   - Click "Deploy site"

#### Option 2: Deploy via Netlify CLI

1. **Install Netlify CLI:**
   ```bash
   npm install -g netlify-cli
   ```

2. **Login to Netlify:**
   ```bash
   netlify login
   ```

3. **Deploy from the frontend directory:**
   ```bash
   cd Frontend/medicine-frontend
   npm run build
   netlify deploy --prod --dir=dist/medicine-frontend/browser
   ```

### Backend Deployment Options

Since your backend is built with .NET 8, here are deployment options:

#### Option 1: Azure App Service (Recommended for .NET)
1. Install Azure CLI
2. Create Azure App Service
3. Deploy using `az webapp up`

#### Option 2: Railway
1. Visit [railway.app](https://railway.app)
2. Connect your repository
3. Railway will auto-detect .NET and deploy

#### Option 3: Render
1. Visit [render.com](https://render.com)
2. Create new Web Service
3. Connect repository and configure build settings

### Environment Configuration

After deploying your backend, update the frontend environment:

1. **Update production environment:**
   ```typescript
   // src/app/environment/environment.prod.ts
   export const environment = {
     production: true,
     apiUrl: 'https://your-deployed-backend-url.com/api'
   };
   ```

2. **Rebuild and redeploy frontend:**
   ```bash
   npm run build
   netlify deploy --prod --dir=dist/medicine-frontend/browser
   ```

## 🛠️ Local Development

### Frontend
```bash
cd Frontend/medicine-frontend
npm install
npm start
```

### Backend
```bash
cd Backend/MedicineWebsite
dotnet restore
dotnet run --project MedicineWebsite.API
```

## 📋 Features Implemented

- ✅ Angular 20 frontend with SSR
- ✅ .NET 8 Web API backend
- ✅ 6-layer N-Tier architecture
- ✅ Entity Framework Core with in-memory database
- ✅ JWT authentication setup
- ✅ Bootstrap 5 responsive UI
- ✅ Medicine search and filtering
- ✅ Pharmacy listings
- ✅ Shopping cart functionality

## 🔄 Next Steps

1. Deploy backend to cloud service
2. Update frontend API URLs
3. Implement authentication service
4. Add Stripe payment integration
5. Add Cloudinary image upload
6. Implement email services
7. Add database migrations for production

## 🌐 Live Demo

- **Frontend:** Will be available at your Netlify URL
- **Backend:** Will be available at your deployed backend URL

## 📞 Support

For deployment issues, refer to:
- [Netlify Documentation](https://docs.netlify.com/)
- [Angular Deployment Guide](https://angular.io/guide/deployment)
- [.NET Deployment Guide](https://docs.microsoft.com/en-us/dotnet/core/deploying/)
