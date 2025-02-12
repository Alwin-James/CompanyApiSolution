using Microsoft.AspNetCore.Mvc;
using CompanyApi.Business;
using CompanyApi.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using CompanyApi.Business.Services;
using Microsoft.AspNetCore.Authorization;

namespace CompanyApi.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ILogger<CompaniesController> _logger;

        public CompaniesController(ICompanyService companyService, ILogger<CompaniesController> logger)
        {
            _companyService = companyService;
            _logger = logger;
        }

        // POST: api/companies
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdCompany = await _companyService.CreateCompanyAsync(company);
                return CreatedAtAction(nameof(GetCompanyById), new { id = createdCompany.Id }, createdCompany);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorCreatingCompany);
                return StatusCode(500, ErrorMessages.ErrorCreatingCompany);
            }
        }

        // GET: api/companies/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {
            try
            {
                var company = await _companyService.GetCompanyByIdAsync(id);
                return Ok(company);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, ErrorMessages.CompanyNotFound);
                return NotFound(ErrorMessages.CompanyNotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorRetrievingCompany);
                return StatusCode(500, ErrorMessages.ErrorRetrievingCompany);
            }
        }

        // GET: api/companies/isin/{isin}
        [HttpGet("isin/{isin}")]
        public async Task<IActionResult> GetCompanyByIsin(string isin)
        {
            try
            {
                var company = await _companyService.GetCompanyByIsinAsync(isin);
                return Ok(company);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, ErrorMessages.CompanyNotFound);
                return NotFound(ErrorMessages.CompanyNotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorRetrievingCompany);
                return StatusCode(500, ErrorMessages.ErrorRetrievingCompany);
            }
        }

        // GET: api/companies
        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            try
            {
                var companies = await _companyService.GetAllCompaniesAsync();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorRetrievingCompanies);
                return StatusCode(500, ErrorMessages.ErrorRetrievingCompanies);
            }
        }

        // PUT: api/companies/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updatedCompany = await _companyService.UpdateCompanyAsync(id, company);
                return Ok(updatedCompany);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, ErrorMessages.CompanyNotFound);
                return NotFound(ErrorMessages.CompanyNotFound);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorUpdatingCompany);
                return StatusCode(500, ErrorMessages.ErrorUpdatingCompany);
            }
        }
    }
}
