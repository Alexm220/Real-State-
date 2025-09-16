# Getting Started - Real Estate Management System

This guide will help you quickly set up and run the Real Estate Management System on your local machine.

## 🚀 Quick Start (5 minutes)

### Step 1: Install Prerequisites
1. **Install .NET 8 SDK**: https://dotnet.microsoft.com/download/dotnet/8.0
2. **Install Node.js 18+**: https://nodejs.org/
3. **Install MongoDB**: https://www.mongodb.com/try/download/community (or use MongoDB Atlas)

### Step 2: Start MongoDB
```bash
# If using local MongoDB
mongod --dbpath /path/to/your/data/directory

# Or start MongoDB service (Windows)
net start MongoDB

# Or start MongoDB service (macOS/Linux)
sudo systemctl start mongod
```

### Step 3: Run the Backend API
```bash
cd Backend
dotnet restore
dotnet run --project RealState.API
```
✅ API will be available at: `https://localhost:7001`

### Step 4: Seed Sample Data
```bash
# In a new terminal, seed the database with sample properties
curl -X POST https://localhost:7001/api/seed/seed
```

### Step 5: Run the Frontend
```bash
cd Frontend/real-estate-web
npm install
npm run dev
```
✅ Web app will be available at: `http://localhost:3000`

## 🎯 What You'll See

### API Documentation
- Visit `https://localhost:7001/swagger` for interactive API documentation
- Test all endpoints directly from the Swagger UI

### Web Application
- **Home Page**: Browse all properties with filtering options
- **Property Details**: Click any property to see detailed information
- **Responsive Design**: Works on desktop, tablet, and mobile

## 🔧 Configuration

### Backend Configuration
Edit `Backend/RealState.API/appsettings.json`:
```json
{
  "MongoDbSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "RealStateDB"
  }
}
```

### Frontend Configuration
Edit `Frontend/real-estate-web/.env.local`:
```
NEXT_PUBLIC_API_URL=https://localhost:7001/api
```

## 🧪 Testing the Application

### 1. Test API Endpoints
```bash
# Get all properties
curl https://localhost:7001/api/properties

# Get properties with filters
curl "https://localhost:7001/api/properties?name=luxury&minPrice=1000000"

# Get specific property
curl https://localhost:7001/api/properties/{property-id}
```

### 2. Test Frontend Features
1. **Property Listing**: Filter by name, address, and price range
2. **Property Details**: Click on any property card
3. **Pagination**: Navigate through multiple pages of results
4. **Responsive Design**: Resize browser window to test mobile view

### 3. Run Unit Tests
```bash
cd Backend
dotnet test
```

## 📊 Sample Data Overview

The seeded database includes:
- **6 Properties** in different locations (NYC, Miami, San Francisco, Chicago, Austin, Seattle)
- **4 Property Owners** with complete profiles
- **11 Property Images** with placeholder images
- **6 Transaction Records** showing property history

## 🛠️ Development Commands

### Backend Development
```bash
# Run in development mode
dotnet run --project RealState.API

# Run tests
dotnet test

# Build for production
dotnet build -c Release
```

### Frontend Development
```bash
# Development server
npm run dev

# Build for production
npm run build

# Start production server
npm start

# Lint code
npm run lint
```

## 🔍 Key Features to Explore

### Advanced Property Filtering
- **Name Search**: Type "luxury" to find luxury properties
- **Address Search**: Type "New York" to find NYC properties
- **Price Range**: Set min/max prices to filter by budget
- **Combined Filters**: Use multiple filters simultaneously

### Property Details
- **Image Gallery**: Multiple images per property
- **Owner Information**: Complete owner profiles
- **Transaction History**: Property valuation and sale records
- **Responsive Layout**: Optimized for all screen sizes

### API Features
- **RESTful Design**: Standard HTTP methods and status codes
- **Pagination**: Efficient handling of large datasets
- **Error Handling**: Comprehensive error responses
- **Documentation**: Interactive Swagger documentation

## 🚨 Troubleshooting

### Common Issues

1. **Port Already in Use**
   ```bash
   # Change ports in configuration files if needed
   # Backend: launchSettings.json
   # Frontend: package.json scripts
   ```

2. **MongoDB Connection Failed**
   ```bash
   # Verify MongoDB is running
   mongosh # Should connect successfully
   ```

3. **CORS Issues**
   ```bash
   # Ensure frontend URL matches CORS policy in backend
   # Check Program.cs CORS configuration
   ```

4. **Package Installation Issues**
   ```bash
   # Clear npm cache
   npm cache clean --force
   
   # Delete node_modules and reinstall
   rm -rf node_modules package-lock.json
   npm install
   ```

## 📈 Next Steps

### Extend the Application
1. **Add Authentication**: Implement user login/registration
2. **Property Management**: Add CRUD operations for properties
3. **Image Upload**: Implement real image upload functionality
4. **Advanced Search**: Add more filter options (property type, bedrooms, etc.)
5. **Maps Integration**: Add Google Maps for property locations
6. **Favorites**: Allow users to save favorite properties

### Production Deployment
1. **Database**: Set up MongoDB Atlas for cloud database
2. **Backend**: Deploy to Azure App Service or AWS
3. **Frontend**: Deploy to Vercel or Netlify
4. **Monitoring**: Add application monitoring and logging

## 📞 Support

If you encounter any issues:
1. Check the troubleshooting section above
2. Review the logs in the terminal/console
3. Verify all prerequisites are installed correctly
4. Ensure MongoDB is running and accessible

Happy coding! 🎉
