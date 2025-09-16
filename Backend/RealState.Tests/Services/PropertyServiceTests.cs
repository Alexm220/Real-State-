using AutoMapper;
using Moq;
using NUnit.Framework;
using RealState.Application.Mappings;
using RealState.Application.Services;
using RealState.Core.DTOs;
using RealState.Core.Entities;
using RealState.Core.Interfaces;

namespace RealState.Tests.Services;

[TestFixture]
public class PropertyServiceTests
{
    private Mock<IPropertyRepository> _mockPropertyRepository;
    private Mock<IOwnerRepository> _mockOwnerRepository;
    private IMapper _mapper;
    private PropertyService _propertyService;

    [SetUp]
    public void Setup()
    {
        _mockPropertyRepository = new Mock<IPropertyRepository>();
        _mockOwnerRepository = new Mock<IOwnerRepository>();
        
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        
        _propertyService = new PropertyService(_mockPropertyRepository.Object, _mockOwnerRepository.Object, _mapper);
    }

    [Test]
    public async Task GetPropertiesAsync_ReturnsPagedResult()
    {
        // Arrange
        var filter = new PropertyFilterDto { Page = 1, PageSize = 10 };
        var properties = new List<Property>
        {
            new Property
            {
                Id = "1",
                IdProperty = "PROP001",
                Name = "Test Property",
                Address = "123 Test St",
                Price = 100000,
                IdOwner = "OWNER001",
                Images = new List<PropertyImage>()
            }
        };
        
        var pagedResult = new PagedResultDto<Property>
        {
            Items = properties,
            TotalCount = 1,
            Page = 1,
            PageSize = 10
        };

        _mockPropertyRepository.Setup(x => x.GetPropertiesAsync(filter))
            .ReturnsAsync(pagedResult);
        
        _mockPropertyRepository.Setup(x => x.GetPropertyImagesAsync("PROP001"))
            .ReturnsAsync(new List<PropertyImage>());
        
        _mockOwnerRepository.Setup(x => x.GetOwnerByIdOwnerAsync("OWNER001"))
            .ReturnsAsync((Owner?)null);

        // Act
        var result = await _propertyService.GetPropertiesAsync(filter);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Items.Count, Is.EqualTo(1));
        Assert.That(result.TotalCount, Is.EqualTo(1));
        Assert.That(result.Items.First().Name, Is.EqualTo("Test Property"));
    }

    [Test]
    public async Task GetPropertyByIdAsync_ExistingProperty_ReturnsPropertyDetail()
    {
        // Arrange
        var propertyId = "1";
        var property = new Property
        {
            Id = propertyId,
            IdProperty = "PROP001",
            Name = "Test Property",
            Address = "123 Test St",
            Price = 100000,
            IdOwner = "OWNER001"
        };

        _mockPropertyRepository.Setup(x => x.GetPropertyByIdAsync(propertyId))
            .ReturnsAsync(property);
        
        _mockPropertyRepository.Setup(x => x.GetPropertyImagesAsync("PROP001"))
            .ReturnsAsync(new List<PropertyImage>());
        
        _mockPropertyRepository.Setup(x => x.GetPropertyTracesAsync("PROP001"))
            .ReturnsAsync(new List<PropertyTrace>());
        
        _mockOwnerRepository.Setup(x => x.GetOwnerByIdOwnerAsync("OWNER001"))
            .ReturnsAsync((Owner?)null);

        // Act
        var result = await _propertyService.GetPropertyByIdAsync(propertyId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Test Property"));
        Assert.That(result.Address, Is.EqualTo("123 Test St"));
    }

    [Test]
    public async Task GetPropertyByIdAsync_NonExistingProperty_ReturnsNull()
    {
        // Arrange
        var propertyId = "999";
        _mockPropertyRepository.Setup(x => x.GetPropertyByIdAsync(propertyId))
            .ReturnsAsync((Property?)null);

        // Act
        var result = await _propertyService.GetPropertyByIdAsync(propertyId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreatePropertyAsync_ValidProperty_ReturnsCreatedProperty()
    {
        // Arrange
        var propertyDto = new PropertyDto
        {
            Name = "New Property",
            Address = "456 New St",
            Price = 150000,
            IdOwner = "OWNER001"
        };

        var createdProperty = new Property
        {
            Id = "1",
            IdProperty = "PROP002",
            Name = propertyDto.Name,
            Address = propertyDto.Address,
            Price = propertyDto.Price,
            IdOwner = propertyDto.IdOwner
        };

        _mockPropertyRepository.Setup(x => x.CreatePropertyAsync(It.IsAny<Property>()))
            .ReturnsAsync(createdProperty);

        // Act
        var result = await _propertyService.CreatePropertyAsync(propertyDto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("New Property"));
        Assert.That(result.Price, Is.EqualTo(150000));
    }

    [Test]
    public async Task DeletePropertyAsync_ExistingProperty_ReturnsTrue()
    {
        // Arrange
        var propertyId = "1";
        _mockPropertyRepository.Setup(x => x.DeletePropertyAsync(propertyId))
            .ReturnsAsync(true);

        // Act
        var result = await _propertyService.DeletePropertyAsync(propertyId);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task DeletePropertyAsync_NonExistingProperty_ReturnsFalse()
    {
        // Arrange
        var propertyId = "999";
        _mockPropertyRepository.Setup(x => x.DeletePropertyAsync(propertyId))
            .ReturnsAsync(false);

        // Act
        var result = await _propertyService.DeletePropertyAsync(propertyId);

        // Assert
        Assert.That(result, Is.False);
    }
}
