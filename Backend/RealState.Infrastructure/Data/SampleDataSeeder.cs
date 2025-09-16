using MongoDB.Driver;
using RealState.Core.Entities;
using RealState.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace RealState.Infrastructure.Data;

public class SampleDataSeeder
{
    private readonly IMongoCollection<Owner> _owners;
    private readonly IMongoCollection<Property> _properties;
    private readonly IMongoCollection<PropertyImage> _propertyImages;
    private readonly IMongoCollection<PropertyTrace> _propertyTraces;

    public SampleDataSeeder(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);

        _owners = mongoDatabase.GetCollection<Owner>(mongoDbSettings.Value.OwnersCollectionName);
        _properties = mongoDatabase.GetCollection<Property>(mongoDbSettings.Value.PropertiesCollectionName);
        _propertyImages = mongoDatabase.GetCollection<PropertyImage>(mongoDbSettings.Value.PropertyImagesCollectionName);
        _propertyTraces = mongoDatabase.GetCollection<PropertyTrace>(mongoDbSettings.Value.PropertyTracesCollectionName);
    }

    public async Task SeedDataAsync()
    {
        // Check if data already exists
        var existingOwnersCount = await _owners.CountDocumentsAsync(FilterDefinition<Owner>.Empty);
        if (existingOwnersCount > 0)
        {
            Console.WriteLine("Sample data already exists. Skipping seeding.");
            return;
        }

        Console.WriteLine("Seeding sample data...");

        // Create sample owners
        var owners = new List<Owner>
        {
            new Owner
            {
                IdOwner = "OWNER001",
                Name = "John Smith",
                Address = "123 Main Street, New York, NY 10001",
                Photo = "https://via.placeholder.com/150x150?text=JS",
                Birthday = new DateTime(1980, 5, 15)
            },
            new Owner
            {
                IdOwner = "OWNER002",
                Name = "Sarah Johnson",
                Address = "456 Oak Avenue, Los Angeles, CA 90210",
                Photo = "https://via.placeholder.com/150x150?text=SJ",
                Birthday = new DateTime(1975, 8, 22)
            },
            new Owner
            {
                IdOwner = "OWNER003",
                Name = "Michael Brown",
                Address = "789 Pine Road, Chicago, IL 60601",
                Photo = "https://via.placeholder.com/150x150?text=MB",
                Birthday = new DateTime(1985, 12, 3)
            },
            new Owner
            {
                IdOwner = "OWNER004",
                Name = "Emily Davis",
                Address = "321 Elm Street, Miami, FL 33101",
                Photo = "https://via.placeholder.com/150x150?text=ED",
                Birthday = new DateTime(1990, 3, 18)
            }
        };

        await _owners.InsertManyAsync(owners);

        // Create sample properties
        var properties = new List<Property>
        {
            new Property
            {
                IdProperty = "PROP001",
                Name = "Luxury Downtown Apartment",
                Address = "100 Central Park West, New York, NY 10023",
                Price = 2500000,
                CodeInternal = "NYC001",
                Year = 2020,
                IdOwner = "OWNER001"
            },
            new Property
            {
                IdProperty = "PROP002",
                Name = "Modern Beach House",
                Address = "500 Ocean Drive, Miami Beach, FL 33139",
                Price = 1800000,
                CodeInternal = "MIA001",
                Year = 2019,
                IdOwner = "OWNER004"
            },
            new Property
            {
                IdProperty = "PROP003",
                Name = "Victorian Family Home",
                Address = "200 Maple Street, San Francisco, CA 94102",
                Price = 3200000,
                CodeInternal = "SF001",
                Year = 1905,
                IdOwner = "OWNER002"
            },
            new Property
            {
                IdProperty = "PROP004",
                Name = "Contemporary Loft",
                Address = "150 Industrial Way, Chicago, IL 60622",
                Price = 850000,
                CodeInternal = "CHI001",
                Year = 2018,
                IdOwner = "OWNER003"
            },
            new Property
            {
                IdProperty = "PROP005",
                Name = "Suburban Ranch House",
                Address = "75 Willow Lane, Austin, TX 78701",
                Price = 650000,
                CodeInternal = "AUS001",
                Year = 2015,
                IdOwner = "OWNER001"
            },
            new Property
            {
                IdProperty = "PROP006",
                Name = "Penthouse Suite",
                Address = "888 Skyline Boulevard, Seattle, WA 98101",
                Price = 4500000,
                CodeInternal = "SEA001",
                Year = 2021,
                IdOwner = "OWNER002"
            }
        };

        await _properties.InsertManyAsync(properties);

        // Create sample property images
        var propertyImages = new List<PropertyImage>
        {
            // Images for PROP001
            new PropertyImage { IdPropertyImage = "IMG001", IdProperty = "PROP001", File = "https://via.placeholder.com/800x600?text=Luxury+Apartment+1", Enabled = true },
            new PropertyImage { IdPropertyImage = "IMG002", IdProperty = "PROP001", File = "https://via.placeholder.com/800x600?text=Luxury+Apartment+2", Enabled = true },
            
            // Images for PROP002
            new PropertyImage { IdPropertyImage = "IMG003", IdProperty = "PROP002", File = "https://via.placeholder.com/800x600?text=Beach+House+1", Enabled = true },
            new PropertyImage { IdPropertyImage = "IMG004", IdProperty = "PROP002", File = "https://via.placeholder.com/800x600?text=Beach+House+2", Enabled = true },
            
            // Images for PROP003
            new PropertyImage { IdPropertyImage = "IMG005", IdProperty = "PROP003", File = "https://via.placeholder.com/800x600?text=Victorian+Home+1", Enabled = true },
            new PropertyImage { IdPropertyImage = "IMG006", IdProperty = "PROP003", File = "https://via.placeholder.com/800x600?text=Victorian+Home+2", Enabled = true },
            
            // Images for PROP004
            new PropertyImage { IdPropertyImage = "IMG007", IdProperty = "PROP004", File = "https://via.placeholder.com/800x600?text=Contemporary+Loft+1", Enabled = true },
            
            // Images for PROP005
            new PropertyImage { IdPropertyImage = "IMG008", IdProperty = "PROP005", File = "https://via.placeholder.com/800x600?text=Ranch+House+1", Enabled = true },
            
            // Images for PROP006
            new PropertyImage { IdPropertyImage = "IMG009", IdProperty = "PROP006", File = "https://via.placeholder.com/800x600?text=Penthouse+1", Enabled = true },
            new PropertyImage { IdPropertyImage = "IMG010", IdProperty = "PROP006", File = "https://via.placeholder.com/800x600?text=Penthouse+2", Enabled = true },
            new PropertyImage { IdPropertyImage = "IMG011", IdProperty = "PROP006", File = "https://via.placeholder.com/800x600?text=Penthouse+3", Enabled = true }
        };

        await _propertyImages.InsertManyAsync(propertyImages);

        // Create sample property traces
        var propertyTraces = new List<PropertyTrace>
        {
            new PropertyTrace
            {
                IdPropertyTrace = "TRACE001",
                DateSale = new DateTime(2023, 6, 15),
                Name = "Initial Purchase",
                Value = 2500000,
                Tax = 125000,
                IdProperty = "PROP001"
            },
            new PropertyTrace
            {
                IdPropertyTrace = "TRACE002",
                DateSale = new DateTime(2022, 3, 10),
                Name = "Market Valuation",
                Value = 1800000,
                Tax = 90000,
                IdProperty = "PROP002"
            },
            new PropertyTrace
            {
                IdPropertyTrace = "TRACE003",
                DateSale = new DateTime(2023, 1, 20),
                Name = "Renovation Assessment",
                Value = 3200000,
                Tax = 160000,
                IdProperty = "PROP003"
            },
            new PropertyTrace
            {
                IdPropertyTrace = "TRACE004",
                DateSale = new DateTime(2023, 8, 5),
                Name = "Recent Sale",
                Value = 850000,
                Tax = 42500,
                IdProperty = "PROP004"
            },
            new PropertyTrace
            {
                IdPropertyTrace = "TRACE005",
                DateSale = new DateTime(2023, 4, 12),
                Name = "Property Assessment",
                Value = 650000,
                Tax = 32500,
                IdProperty = "PROP005"
            },
            new PropertyTrace
            {
                IdPropertyTrace = "TRACE006",
                DateSale = new DateTime(2023, 9, 30),
                Name = "Luxury Purchase",
                Value = 4500000,
                Tax = 225000,
                IdProperty = "PROP006"
            }
        };

        await _propertyTraces.InsertManyAsync(propertyTraces);

        Console.WriteLine($"Successfully seeded:");
        Console.WriteLine($"- {owners.Count} owners");
        Console.WriteLine($"- {properties.Count} properties");
        Console.WriteLine($"- {propertyImages.Count} property images");
        Console.WriteLine($"- {propertyTraces.Count} property traces");
    }

    public async Task ClearDataAsync()
    {
        Console.WriteLine("Clearing existing data...");
        
        await _propertyTraces.DeleteManyAsync(FilterDefinition<PropertyTrace>.Empty);
        await _propertyImages.DeleteManyAsync(FilterDefinition<PropertyImage>.Empty);
        await _properties.DeleteManyAsync(FilterDefinition<Property>.Empty);
        await _owners.DeleteManyAsync(FilterDefinition<Owner>.Empty);
        
        Console.WriteLine("Data cleared successfully.");
    }
}
