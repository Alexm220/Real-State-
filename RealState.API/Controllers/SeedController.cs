using Microsoft.AspNetCore.Mvc;
using RealState.Infrastructure.Data;

namespace RealState.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeedController : ControllerBase
{
    private readonly SampleDataSeeder _seeder;

    public SeedController(SampleDataSeeder seeder)
    {
        _seeder = seeder;
    }

    /// <summary>
    /// Seed the database with sample data
    /// </summary>
    /// <returns>Success message</returns>
    [HttpPost("seed")]
    public async Task<ActionResult> SeedData()
    {
        try
        {
            await _seeder.SeedDataAsync();
            return Ok(new { message = "Sample data seeded successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while seeding data", error = ex.Message });
        }
    }

    /// <summary>
    /// Clear all data from the database
    /// </summary>
    /// <returns>Success message</returns>
    [HttpDelete("clear")]
    public async Task<ActionResult> ClearData()
    {
        try
        {
            await _seeder.ClearDataAsync();
            return Ok(new { message = "Data cleared successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while clearing data", error = ex.Message });
        }
    }
}
