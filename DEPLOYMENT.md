# Deployment Guide

This guide provides step-by-step instructions for deploying the Real Estate Management System.

## Prerequisites

### Backend Requirements
- .NET 8 SDK
- MongoDB (local installation or MongoDB Atlas)
- Visual Studio 2022 or VS Code with C# extension

### Frontend Requirements
- Node.js 18+ and npm
- Modern web browser

## Backend Deployment

### 1. Install .NET 8 SDK
Download and install from: https://dotnet.microsoft.com/download/dotnet/8.0

### 2. Set up MongoDB
Choose one of the following options:

#### Option A: Local MongoDB Installation
1. Download MongoDB Community Server from: https://www.mongodb.com/try/download/community
2. Install and start MongoDB service
3. Default connection string: `mongodb://localhost:27017`

#### Option B: MongoDB Atlas (Cloud)
1. Create account at https://www.mongodb.com/atlas
2. Create a new cluster
3. Get connection string from Atlas dashboard
4. Update `appsettings.json` with your connection string

### 3. Configure Application Settings
Update `Backend/RealState.API/appsettings.json`:

```json
{
  "MongoDbSettings": {
    "ConnectionString": "your-mongodb-connection-string",
    "DatabaseName": "RealStateDB",
    "PropertiesCollectionName": "Properties",
    "OwnersCollectionName": "Owners",
    "PropertyImagesCollectionName": "PropertyImages",
    "PropertyTracesCollectionName": "PropertyTraces"
  }
}
```

### 4. Build and Run Backend
```bash
cd Backend
dotnet restore
dotnet build
dotnet run --project RealState.API
```

The API will be available at: `https://localhost:7001`

### 5. Seed Sample Data (Optional)
Once the API is running, seed sample data:
```bash
curl -X POST https://localhost:7001/api/seed/seed
```

## Frontend Deployment

### 1. Install Dependencies
```bash
cd Frontend/real-estate-web
npm install
```

### 2. Configure API Endpoint
Update `Frontend/real-estate-web/.env.local`:
```
NEXT_PUBLIC_API_URL=https://localhost:7001/api
```

### 3. Build and Run Frontend
```bash
npm run dev
```

The frontend will be available at: `http://localhost:3000`

## Production Deployment

### Backend Production
1. **Publish the application:**
   ```bash
   dotnet publish -c Release -o ./publish
   ```

2. **Deploy to hosting service:**
   - Azure App Service
   - AWS Elastic Beanstalk
   - Docker container
   - IIS (Windows Server)

3. **Environment variables:**
   Set production MongoDB connection string and other settings

### Frontend Production
1. **Build for production:**
   ```bash
   npm run build
   ```

2. **Deploy to hosting service:**
   - Vercel (recommended for Next.js)
   - Netlify
   - AWS S3 + CloudFront
   - Azure Static Web Apps

## Docker Deployment (Optional)

### Backend Dockerfile
Create `Backend/Dockerfile`:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RealState.API/RealState.API.csproj", "RealState.API/"]
COPY ["RealState.Application/RealState.Application.csproj", "RealState.Application/"]
COPY ["RealState.Core/RealState.Core.csproj", "RealState.Core/"]
COPY ["RealState.Infrastructure/RealState.Infrastructure.csproj", "RealState.Infrastructure/"]
RUN dotnet restore "RealState.API/RealState.API.csproj"
COPY . .
WORKDIR "/src/RealState.API"
RUN dotnet build "RealState.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RealState.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RealState.API.dll"]
```

### Frontend Dockerfile
Create `Frontend/real-estate-web/Dockerfile`:
```dockerfile
FROM node:18-alpine AS deps
WORKDIR /app
COPY package*.json ./
RUN npm ci --only=production

FROM node:18-alpine AS builder
WORKDIR /app
COPY . .
COPY --from=deps /app/node_modules ./node_modules
RUN npm run build

FROM node:18-alpine AS runner
WORKDIR /app
ENV NODE_ENV production
COPY --from=builder /app/public ./public
COPY --from=builder /app/.next/standalone ./
COPY --from=builder /app/.next/static ./.next/static
EXPOSE 3000
ENV PORT 3000
CMD ["node", "server.js"]
```

### Docker Compose
Create `docker-compose.yml`:
```yaml
version: '3.8'
services:
  mongodb:
    image: mongo:latest
    container_name: mongodb
    ports:
      - "27017:27017"
    volumes:
      - mongodb_data:/data/db

  backend:
    build: ./Backend
    container_name: realstate-api
    ports:
      - "7001:80"
    depends_on:
      - mongodb
    environment:
      - MongoDbSettings__ConnectionString=mongodb://mongodb:27017

  frontend:
    build: ./Frontend/real-estate-web
    container_name: realstate-web
    ports:
      - "3000:3000"
    depends_on:
      - backend
    environment:
      - NEXT_PUBLIC_API_URL=http://localhost:7001/api

volumes:
  mongodb_data:
```

## Testing the Deployment

### 1. Verify Backend
- Navigate to `https://localhost:7001/swagger` for API documentation
- Test endpoints using Swagger UI

### 2. Verify Frontend
- Navigate to `http://localhost:3000`
- Test property listing and filtering
- Test property detail views

### 3. Test Integration
- Ensure frontend can communicate with backend
- Verify data is loading correctly
- Test all filtering functionality

## Troubleshooting

### Common Issues

1. **MongoDB Connection Issues**
   - Verify MongoDB is running
   - Check connection string format
   - Ensure network connectivity

2. **CORS Issues**
   - Verify CORS policy in backend
   - Check frontend API URL configuration

3. **Port Conflicts**
   - Change ports in configuration if needed
   - Ensure ports are not blocked by firewall

4. **SSL Certificate Issues**
   - Use HTTP for local development
   - Configure proper SSL certificates for production

### Logs and Debugging
- Backend logs: Check console output or configure logging
- Frontend logs: Check browser developer tools
- MongoDB logs: Check MongoDB service logs

## Security Considerations

### Production Security
1. **Use HTTPS** in production
2. **Secure MongoDB** with authentication
3. **Environment Variables** for sensitive data
4. **API Rate Limiting** to prevent abuse
5. **Input Validation** on all endpoints
6. **CORS Configuration** for specific domains only

### Monitoring
- Set up application monitoring (Application Insights, etc.)
- Configure health checks
- Monitor database performance
- Set up alerting for critical issues
