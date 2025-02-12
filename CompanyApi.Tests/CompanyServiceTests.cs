using Xunit;
using Moq;
using CompanyApi.Business;
using CompanyApi.Data;
using CompanyApi.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using CompanyApi.Data.Repositories;

namespace CompanyApi.Tests
{
    /// <summary>
    /// Unit tests for the <see cref="CompanyService"/> class, ensuring all business logic works as expected.
    /// </summary>
    public class CompanyServiceTests
    {
        private readonly CompanyService _companyService;
        private readonly Mock<ICompanyRepository> _companyRepoMock = new();
        private readonly Mock<ILogger<CompanyService>> _loggerMock = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyServiceTests"/> class with mocked dependencies.
        /// </summary>
        public CompanyServiceTests()
        {
            _companyService = new CompanyService(_companyRepoMock.Object, _loggerMock.Object);
        }

        /// <summary>
        /// Tests that creating a company with an invalid ISIN throws an <see cref="ArgumentException"/>.
        /// </summary>
        [Fact]
        public async Task CreateCompanyAsync_WithInvalidIsin_ThrowsArgumentException()
        {
            var company = new Company
            {
                Name = "Test Company",
                Exchange = "Test Exchange",
                Ticker = "TST",
                Isin = "12INVALID"  // First two characters are not letters
            };

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _companyService.CreateCompanyAsync(company));
            Assert.Equal(ErrorMessages.InvalidIsinFormat, ex.Message);
        }

        /// <summary>
        /// Tests that creating a company with a duplicate ISIN throws an <see cref="ArgumentException"/>.
        /// </summary>
        [Fact]
        public async Task CreateCompanyAsync_WithDuplicateIsin_ThrowsArgumentException()
        {
            var company = new Company
            {
                Name = "Duplicate ISIN Company",
                Exchange = "NYSE",
                Ticker = "DUP",
                Isin = "US1234567890"
            };

            _companyRepoMock.Setup(repo => repo.IsIsinExistsAsync(company.Isin)).ReturnsAsync(true);

            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _companyService.CreateCompanyAsync(company));
            Assert.Equal(ErrorMessages.DuplicateIsin, ex.Message);
        }

        /// <summary>
        /// Tests that a company is successfully created when provided with valid data.
        /// </summary>
        [Fact]
        public async Task CreateCompanyAsync_WithValidData_ReturnsCreatedCompany()
        {
            var company = new Company
            {
                Name = "Valid Company",
                Exchange = "NASDAQ",
                Ticker = "VAL",
                Isin = "US9876543210"
            };

            _companyRepoMock.Setup(repo => repo.IsIsinExistsAsync(company.Isin)).ReturnsAsync(false);
            _companyRepoMock.Setup(repo => repo.AddAsync(company)).Returns(Task.CompletedTask);
            _companyRepoMock.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _companyService.CreateCompanyAsync(company);

            Assert.NotNull(result);
            Assert.Equal(company.Name, result.Name);
            Assert.Equal(company.Isin, result.Isin);
        }

        /// <summary>
        /// Tests that retrieving a company by an invalid ID throws a <see cref="KeyNotFoundException"/>.
        /// </summary>
        [Fact]
        public async Task GetCompanyByIdAsync_WithInvalidId_ThrowsKeyNotFoundException()
        {
            int invalidId = 999;
            _companyRepoMock.Setup(repo => repo.GetByIdAsync(invalidId))
                            .ReturnsAsync((Company)null);

            var ex = await Assert.ThrowsAsync<Exception>(() => _companyService.GetCompanyByIdAsync(invalidId));

            Assert.NotNull(ex.InnerException);
            Assert.IsType<KeyNotFoundException>(ex.InnerException);
            Assert.Equal(ErrorMessages.CompanyNotFound, ex.InnerException.Message);
        }


        /// <summary>
        /// Tests that retrieving a company by a valid ID returns the correct company.
        /// </summary>
        [Fact]
        public async Task GetCompanyByIdAsync_WithValidId_ReturnsCompany()
        {
            var company = new Company
            {
                Id = 1,
                Name = "Test Company",
                Isin = "US1234567890"
            };

            _companyRepoMock.Setup(repo => repo.GetByIdAsync(company.Id)).ReturnsAsync(company);

            var result = await _companyService.GetCompanyByIdAsync(company.Id);

            Assert.NotNull(result);
            Assert.Equal(company.Id, result.Id);
            Assert.Equal(company.Name, result.Name);
        }      

        /// <summary>
        /// Tests that updating a company with valid data returns the updated company.
        /// </summary>
        [Fact]
        public async Task UpdateCompanyAsync_WithValidData_ReturnsUpdatedCompany()
        {
            var existingCompany = new Company
            {
                Id = 1,
                Name = "Old Name",
                Isin = "US1111111111"
            };

            var updatedCompany = new Company
            {
                Name = "New Name",
                Exchange = "NASDAQ",
                Ticker = "NEW",
                Isin = "US1111111111", // Same ISIN, no duplication issue
                Website = "https://newcompany.com"
            };

            _companyRepoMock.Setup(repo => repo.GetByIdAsync(existingCompany.Id)).ReturnsAsync(existingCompany);
            _companyRepoMock.Setup(repo => repo.UpdateAsync(existingCompany)).Returns(Task.CompletedTask);
            _companyRepoMock.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _companyService.UpdateCompanyAsync(existingCompany.Id, updatedCompany);

            Assert.NotNull(result);
            Assert.Equal(updatedCompany.Name, result.Name);
            Assert.Equal(updatedCompany.Website, result.Website);
        }
    }
}
