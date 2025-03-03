using Bank.DTO;
using Bank.Model;
using Bank.Service.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
namespace Bank.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;
    private readonly ILogger<CityController> _logger;

    public CityController(ICityService cityService, ILogger<CityController> logger)
    {
        _cityService = cityService;
        _logger = logger;
    }

    [HttpGet]
    public Task<IQueryable<City>> GetCities()
    {
        var cities = _cityService.GetCities();
        return cities ?? throw new ArgumentNullException($"There is no City by this name {nameof(cities)}");
    }

    [HttpGet("{id:int}")]
    public async Task<City> GetCityAsync(int id)
    {
        var city = await _cityService.GetCityAsync(id);
        return city ?? throw new ArgumentNullException(nameof(id));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCityAsync([FromBody] CityModel cityModel)
    {
        if (cityModel == null)
            return BadRequest("Invalid city data.");

        City city = new()
        {
            Name = cityModel.Name
        };

        await _cityService.AddCityAsync(city);
        return Ok(city);
    }

    [HttpPut("{id:int}")]

    public async Task<IActionResult> UpdateCity(int id, [FromBody] CityModel model)
    {
        if (model == null) return BadRequest("Model cannot be null.");

        var existingCity = await _cityService.GetCity(id);
        if (existingCity == null) return NotFound("City not found.");

        existingCity.Name = model.Name;

        _cityService.UpdateCity(existingCity);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCityAsync(int id)
    {
        var city = await _cityService.GetCityAsync(id);
        if (city == null)
            return NotFound($"City with ID {id} not found.");

        await _cityService.DeleteCityAsync(id);
        return NoContent();
    }
}
