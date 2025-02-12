using CompanyApi.Data;
using CompanyApi.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyApi.Business.Services;
using CompanyApi.Data.Repositories;

namespace CompanyApi.Business
{
    /// <summary>
    /// Provides services for managing company data, including creating, retrieving, and updating companies.
    /// </summary>
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ILogger<CompanyService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyService"/> class.
        /// </summary>
        /// <param name="companyRepository">The repository used to interact with company data storage.</param>
        /// <param name="logger">The logger used for logging operations and errors.</param>
        public CompanyService(ICompanyRepository companyRepository, ILogger<CompanyService> logger)
        {
            _companyRepository = companyRepository;
            _logger = logger;
        }

        /// <summary>
        /// Asynchronously creates a new company with validation for ISIN format and duplicates.
        /// </summary>
        /// <param name="company">The <see cref="Company"/> object containing the details of the company to be created.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing the created <see cref="Company"/>.</returns>
        /// <exception cref="ArgumentException">Thrown when the ISIN format is invalid or a duplicate ISIN is found.</exception>
        /// <exception cref="Exception">Thrown when an error occurs while creating the company.</exception>
        public async Task<Company> CreateCompanyAsync(Company company)
        {
            // Validate ISIN: first two characters must be letters.
            if (string.IsNullOrWhiteSpace(company.Isin) ||
                company.Isin.Length < 2 ||
                !char.IsLetter(company.Isin[0]) ||
                !char.IsLetter(company.Isin[1]))
            {
                _logger.LogError(ErrorMessages.InvalidIsinFormat);
                throw new ArgumentException(ErrorMessages.InvalidIsinFormat);
            }

            // Prevent duplicate ISIN entries.
            if (await _companyRepository.IsIsinExistsAsync(company.Isin))
            {
                _logger.LogError(ErrorMessages.DuplicateIsin);
                throw new ArgumentException(ErrorMessages.DuplicateIsin);
            }

            try
            {
                await _companyRepository.AddAsync(company);
                await _companyRepository.SaveChangesAsync();
                return company;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorCreatingCompany);
                throw new Exception(ErrorMessages.ErrorCreatingCompany, ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves all companies from the repository.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing a collection of <see cref="Company"/> objects.</returns>
        /// <exception cref="Exception">Thrown when an error occurs while retrieving the companies.</exception>
        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            try
            {
                return await _companyRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorRetrievingCompanies);
                throw new Exception(ErrorMessages.ErrorRetrievingCompanies, ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves a company by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the company.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing the <see cref="Company"/> if found.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the company is not found.</exception>
        /// <exception cref="Exception">Thrown when an error occurs while retrieving the company.</exception>
        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            try
            {
                var company = await _companyRepository.GetByIdAsync(id);
                if (company == null)
                {
                    _logger.LogWarning(ErrorMessages.CompanyNotFound);
                    throw new KeyNotFoundException(ErrorMessages.CompanyNotFound);
                }
                return company;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorRetrievingCompany);
                throw new Exception(ErrorMessages.ErrorRetrievingCompany, ex);
            }
        }

        /// <summary>
        /// Asynchronously retrieves a company by its ISIN (International Securities Identification Number).
        /// </summary>
        /// <param name="isin">The ISIN of the company.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing the <see cref="Company"/> if found.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the company is not found.</exception>
        /// <exception cref="Exception">Thrown when an error occurs while retrieving the company.</exception>
        public async Task<Company> GetCompanyByIsinAsync(string isin)
        {
            try
            {
                var company = await _companyRepository.GetByIsinAsync(isin);
                if (company == null)
                {
                    _logger.LogWarning(ErrorMessages.CompanyNotFound);
                    throw new KeyNotFoundException(ErrorMessages.CompanyNotFound);
                }
                return company;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorRetrievingCompany);
                throw new Exception(ErrorMessages.ErrorRetrievingCompany, ex);
            }
        }

        /// <summary>
        /// Asynchronously updates an existing company's details.
        /// </summary>
        /// <param name="id">The unique identifier of the company to be updated.</param>
        /// <param name="updatedCompany">The <see cref="Company"/> object containing the updated company details.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing the updated <see cref="Company"/>.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the company to be updated is not found.</exception>
        /// <exception cref="ArgumentException">Thrown when the ISIN format is invalid or a duplicate ISIN is found.</exception>
        /// <exception cref="Exception">Thrown when an error occurs while updating the company.</exception>
        public async Task<Company> UpdateCompanyAsync(int id, Company updatedCompany)
        {
            try
            {
                var company = await _companyRepository.GetByIdAsync(id);
                if (company == null)
                {
                    _logger.LogWarning(ErrorMessages.CompanyNotFound);
                    throw new KeyNotFoundException(ErrorMessages.CompanyNotFound);
                }

                // If the ISIN is being changed, validate and ensure it’s not already taken.
                if (!string.Equals(company.Isin, updatedCompany.Isin, StringComparison.OrdinalIgnoreCase))
                {
                    if (await _companyRepository.IsIsinExistsAsync(updatedCompany.Isin))
                    {
                        _logger.LogError(ErrorMessages.DuplicateIsin);
                        throw new ArgumentException(ErrorMessages.DuplicateIsin);
                    }
                    if (string.IsNullOrWhiteSpace(updatedCompany.Isin) ||
                        updatedCompany.Isin.Length < 2 ||
                        !char.IsLetter(updatedCompany.Isin[0]) ||
                        !char.IsLetter(updatedCompany.Isin[1]))
                    {
                        _logger.LogError(ErrorMessages.InvalidIsinFormat);
                        throw new ArgumentException(ErrorMessages.InvalidIsinFormat);
                    }
                }

                // Update the company properties.
                company.Name = updatedCompany.Name;
                company.Exchange = updatedCompany.Exchange;
                company.Ticker = updatedCompany.Ticker;
                company.Isin = updatedCompany.Isin;
                company.Website = updatedCompany.Website;

                await _companyRepository.UpdateAsync(company);
                await _companyRepository.SaveChangesAsync();

                return company;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorUpdatingCompany);
                throw new Exception(ErrorMessages.ErrorUpdatingCompany, ex);
            }
        }
    }
}
