using BasicAPI.Models;
using BasicAPI.Repository;
using BasicAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasicAPI.Controllers
{
    [Route("api/country")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IFileUploadService _uploadService;

        public CountryController(ICountryRepository countryRepository, IFileUploadService uploadService)
        {
            _countryRepository = countryRepository;
            _uploadService = uploadService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var countries = await _countryRepository.GetCountries();
                return Ok(countries);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);  
            }
        }

        [HttpGet]
        [Route("{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
            try
            {
                var country = await _countryRepository.GetCountry(Id);
                return Ok(country);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromForm] CreateCountryDTO countryDto)
        {
            try
            {
                Country newCountry = new Country()
                {
                    Name = countryDto.Name,
                    ISDCode = countryDto.ISDCode,
                    ShortCode = countryDto.ShortCode,
                    FlagUrl = ""
                };

                if(countryDto.File != null) 
                {
                    newCountry.FlagUrl = await _uploadService.SingleUpload(countryDto.File, Request);
                }

                var country = await _countryRepository.Save(newCountry);

                return Ok(country);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
