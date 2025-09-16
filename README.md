# Real Estate Management System

A full-stack real estate application built with .NET 8, MongoDB, and Next.js.

## Architecture

### Backend (.NET 8 Web API)
- Clean Architecture with separation of concerns
- Repository pattern for data access
- MongoDB integration
- Comprehensive error handling
- NUnit testing

### Frontend (Next.js)
- Responsive property listing
- Advanced filtering (name, address, price range)
- Property detail views
- Modern UI/UX

### Database (MongoDB)
- Property management
- Owner information
- Property images
- Property transaction history

## Project Structure

```
RealState/
├── Backend/
│   ├── RealState.API/          # Web API project
│   ├── RealState.Core/         # Domain entities and interfaces
│   ├── RealState.Infrastructure/ # Data access and external services
│   ├── RealState.Application/   # Business logic and services
│   └── RealState.Tests/        # Unit tests
├── Frontend/
│   └── real-estate-web/       # Next.js application
└── README.md
```

## Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 18+
- MongoDB (local or cloud)

### Backend Setup
1. Navigate to Backend directory
2. Run `dotnet restore`
3. Update connection string in appsettings.json
4. Run `dotnet run --project RealState.API`

### Frontend Setup
1. Navigate to Frontend/real-estate-web
2. Run `npm install`
3. Update API endpoint in configuration
4. Run `npm run dev`

## API Endpoints

- `GET /api/properties` - Get all properties with optional filters
- `GET /api/properties/{id}` - Get property by ID
- `GET /api/owners/{id}` - Get owner information

## Features

- Property listing with pagination
- Advanced search and filtering
- Responsive design
- Property detail views
- Owner information display
- Image gallery for properties
