#!/bin/bash

# Medicine Website Deployment Script

echo "🏥 Medicine Website Deployment Script"
echo "====================================="

# Check if we're in the right directory
if [ ! -d "Frontend/medicine-frontend" ]; then
    echo "❌ Error: Please run this script from the root directory of your project"
    exit 1
fi

# Navigate to frontend directory
cd Frontend/medicine-frontend

echo "📦 Installing dependencies..."
npm install

echo "🔨 Building for production..."
npm run build

# Check if build was successful
if [ $? -eq 0 ]; then
    echo "✅ Build successful!"
    echo ""
    echo "📁 Build files are located at: dist/medicine-frontend/browser"
    echo ""
    echo "🚀 Ready for deployment!"
    echo ""
    echo "Next steps:"
    echo "1. Deploy to Netlify:"
    echo "   - Option A: Drag and drop the 'dist/medicine-frontend/browser' folder to Netlify"
    echo "   - Option B: Use Netlify CLI: netlify deploy --prod --dir=dist/medicine-frontend/browser"
    echo ""
    echo "2. Deploy your backend to a cloud service (Azure, Railway, or Render)"
    echo ""
    echo "3. Update Frontend/medicine-frontend/src/app/environment/environment.prod.ts with your backend URL"
    echo ""
    echo "4. Rebuild and redeploy frontend with updated API URL"
else
    echo "❌ Build failed! Please check the errors above."
    exit 1
fi
