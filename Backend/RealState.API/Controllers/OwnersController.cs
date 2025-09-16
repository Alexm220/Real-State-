using Microsoft.AspNetCore.Mvc;
using RealState.Application.Interfaces;
using RealState.Core.DTOs;

namespace RealState.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OwnersController : ControllerBase
{
    private readonly IOwnerService _ownerService;

    public OwnersController(IOwnerService ownerService)
    {
        _ownerService = ownerService;
    }

    /// <summary>
    /// Get owner by ID
    /// </summary>
    /// <param name="id">Owner ID</param>
    /// <returns>Owner information</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<OwnerDto>> GetOwner(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { message = "Owner ID is required" });

            var owner = await _ownerService.GetOwnerByIdAsync(id);
            
            if (owner == null)
                return NotFound(new { message = "Owner not found" });

            return Ok(owner);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the owner", error = ex.Message });
        }
    }

    /// <summary>
    /// Get owner by IdOwner
    /// </summary>
    /// <param name="idOwner">Owner business ID</param>
    /// <returns>Owner information</returns>
    [HttpGet("by-id-owner/{idOwner}")]
    public async Task<ActionResult<OwnerDto>> GetOwnerByIdOwner(string idOwner)
    {
        try
        {
            if (string.IsNullOrEmpty(idOwner))
                return BadRequest(new { message = "Owner ID is required" });

            var owner = await _ownerService.GetOwnerByIdOwnerAsync(idOwner);
            
            if (owner == null)
                return NotFound(new { message = "Owner not found" });

            return Ok(owner);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the owner", error = ex.Message });
        }
    }

    /// <summary>
    /// Create a new owner
    /// </summary>
    /// <param name="ownerDto">Owner data</param>
    /// <returns>Created owner</returns>
    [HttpPost]
    public async Task<ActionResult<OwnerDto>> CreateOwner([FromBody] OwnerDto ownerDto)
    {
        try
        {
            if (ownerDto == null)
                return BadRequest(new { message = "Owner data is required" });

            var createdOwner = await _ownerService.CreateOwnerAsync(ownerDto);
            return CreatedAtAction(nameof(GetOwner), new { id = createdOwner.Id }, createdOwner);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the owner", error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing owner
    /// </summary>
    /// <param name="id">Owner ID</param>
    /// <param name="ownerDto">Updated owner data</param>
    /// <returns>Updated owner</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<OwnerDto>> UpdateOwner(string id, [FromBody] OwnerDto ownerDto)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { message = "Owner ID is required" });

            if (ownerDto == null)
                return BadRequest(new { message = "Owner data is required" });

            var updatedOwner = await _ownerService.UpdateOwnerAsync(id, ownerDto);
            
            if (updatedOwner == null)
                return NotFound(new { message = "Owner not found" });

            return Ok(updatedOwner);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the owner", error = ex.Message });
        }
    }

    /// <summary>
    /// Delete an owner
    /// </summary>
    /// <param name="id">Owner ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOwner(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest(new { message = "Owner ID is required" });

            var result = await _ownerService.DeleteOwnerAsync(id);
            
            if (!result)
                return NotFound(new { message = "Owner not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the owner", error = ex.Message });
        }
    }
}
