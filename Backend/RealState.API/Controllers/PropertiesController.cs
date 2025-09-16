using Microsoft.AspNetCore.Mvc;
using RealState.Application.Interfaces;
using RealState.Core.DTOs;

namespace RealState.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService _propertyService;

    public PropertiesController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    /// <summary>
    /// Get all properties with optional filtering
    /// </summary>
    /// <param name="filter">Filter parameters for properties</param>
    /// <returns>Paginated list of properties</returns>
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<PropertyDto>>> GetProperties([FromQuery] PropertyFilterDto filter)
    {
        try
        {
            var result = await _propertyService.GetPropertiesAsync(filter);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving properties", error = ex.Message });
        }
    }

    /// <summary>
    /// Get property by ID with detailed information
    /// </summary>
    /// <param name="id">Property ID</param>
    /// <returns>Property details including images and traces</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PropertyDetailDto>> GetProperty(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { message = "Property ID is required" });

            var property = await _propertyService.GetPropertyByIdAsync(id);
            
            if (property == null)
                return NotFound(new { message = "Property not found" });

            return Ok(property);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the property", error = ex.Message });
        }
    }

    /// <summary>
    /// Create a new property
    /// </summary>
    /// <param name="propertyDto">Property data</param>
    /// <returns>Created property</returns>
    [HttpPost]
    public async Task<ActionResult<PropertyDto>> CreateProperty([FromBody] PropertyDto propertyDto)
    {
        try
        {
            if (propertyDto == null)
                return BadRequest(new { message = "Property data is required" });

            var createdProperty = await _propertyService.CreatePropertyAsync(propertyDto);
            return CreatedAtAction(nameof(GetProperty), new { id = createdProperty.Id }, createdProperty);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the property", error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing property
    /// </summary>
    /// <param name="id">Property ID</param>
    /// <param name="propertyDto">Updated property data</param>
    /// <returns>Updated property</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<PropertyDto>> UpdateProperty(string id, [FromBody] PropertyDto propertyDto)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { message = "Property ID is required" });

            if (propertyDto == null)
                return BadRequest(new { message = "Property data is required" });

            var updatedProperty = await _propertyService.UpdatePropertyAsync(id, propertyDto);
            
            if (updatedProperty == null)
                return NotFound(new { message = "Property not found" });

            return Ok(updatedProperty);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the property", error = ex.Message });
        }
    }

    /// <summary>
    /// Delete a property
    /// </summary>
    /// <param name="id">Property ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProperty(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { message = "Property ID is required" });

            var result = await _propertyService.DeletePropertyAsync(id);
            
            if (!result)
                return NotFound(new { message = "Property not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the property", error = ex.Message });
        }
    }
}
