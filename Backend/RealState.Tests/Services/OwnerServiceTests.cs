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
public class OwnerServiceTests
{
    private Mock<IOwnerRepository> _mockOwnerRepository;
    private IMapper _mapper;
    private OwnerService _ownerService;

    [SetUp]
    public void Setup()
    {
        _mockOwnerRepository = new Mock<IOwnerRepository>();
        
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        
        _ownerService = new OwnerService(_mockOwnerRepository.Object, _mapper);
    }

    [Test]
    public async Task GetOwnerByIdAsync_ExistingOwner_ReturnsOwner()
    {
        // Arrange
        var ownerId = "1";
        var owner = new Owner
        {
            Id = ownerId,
            IdOwner = "OWNER001",
            Name = "John Doe",
            Address = "123 Main St",
            Birthday = new DateTime(1980, 1, 1)
        };

        _mockOwnerRepository.Setup(x => x.GetOwnerByIdAsync(ownerId))
            .ReturnsAsync(owner);

        // Act
        var result = await _ownerService.GetOwnerByIdAsync(ownerId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("John Doe"));
        Assert.That(result.IdOwner, Is.EqualTo("OWNER001"));
    }

    [Test]
    public async Task GetOwnerByIdAsync_NonExistingOwner_ReturnsNull()
    {
        // Arrange
        var ownerId = "999";
        _mockOwnerRepository.Setup(x => x.GetOwnerByIdAsync(ownerId))
            .ReturnsAsync((Owner?)null);

        // Act
        var result = await _ownerService.GetOwnerByIdAsync(ownerId);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateOwnerAsync_ValidOwner_ReturnsCreatedOwner()
    {
        // Arrange
        var ownerDto = new OwnerDto
        {
            Name = "Jane Smith",
            Address = "456 Oak St",
            Birthday = new DateTime(1985, 5, 15)
        };

        var createdOwner = new Owner
        {
            Id = "1",
            IdOwner = "OWNER002",
            Name = ownerDto.Name,
            Address = ownerDto.Address,
            Birthday = ownerDto.Birthday
        };

        _mockOwnerRepository.Setup(x => x.CreateOwnerAsync(It.IsAny<Owner>()))
            .ReturnsAsync(createdOwner);

        // Act
        var result = await _ownerService.CreateOwnerAsync(ownerDto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Jane Smith"));
        Assert.That(result.Address, Is.EqualTo("456 Oak St"));
    }

    [Test]
    public async Task DeleteOwnerAsync_ExistingOwner_ReturnsTrue()
    {
        // Arrange
        var ownerId = "1";
        _mockOwnerRepository.Setup(x => x.DeleteOwnerAsync(ownerId))
            .ReturnsAsync(true);

        // Act
        var result = await _ownerService.DeleteOwnerAsync(ownerId);

        // Assert
        Assert.That(result, Is.True);
    }
}
