using Microsoft.AspNetCore.Mvc;
using Lines.Services;

namespace Lines.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CitiesController : ControllerBase
    {
        private readonly LoadData loadData;

        public CitiesController(LoadData loadData2)
        {
            loadData = loadData2;
        }

        [HttpGet]
        public IActionResult getAllCities()
        {
            var data = loadData.CityDataDictionary();

            var result = data.Select(kvp => new
            {
                City = kvp.Key,
                Min = kvp.Value.Min,
                Max = kvp.Value.Max,
                // Count = kvp.Value.Count,
                Average = kvp.Value.Average
            });

            return Ok(result);
        }

        [HttpGet("{cityName}")]
        public IActionResult GetCity(string cityName)
        {
            if (loadData.CityDataDictionary().TryGetValue(cityName, out var data))
            {
                return Ok(new
                {
                    City = cityName,
                    Min = data.Min,
                    Max = data.Max,
                    Average = data.Average,
                    // Count = data.Count
                });
            }

            return NotFound($"City {cityName} not found!");

        }

        [HttpPost("reload")]
        public IActionResult ReloadData()
        {
            try
            {
                string filePath = "Data/measurements.txt";
                loadData.LoadFromFile(filePath);
                return Ok("Data reloaded!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error reloading data: {ex.Message}");
            }
        }

        [HttpGet("filter")]

        public IActionResult getFilteredCities([FromQuery] double minMax, [FromQuery] bool greaterThan)
        {
            var data = loadData.CityDataDictionary();

            IEnumerable<KeyValuePair<string, CityData>> filteredData;
            if (greaterThan)
            {
                filteredData = data.Where(kvp => kvp.Value.Average > minMax);
            }
            else
            {
                filteredData = data.Where(kvp => kvp.Value.Average < minMax);
            }

            var filtered = filteredData.Select(kvp => new
            {
                City = kvp.Key,
                Avg = kvp.Value.Average
            });

            return Ok(filtered);

        }

    }

}