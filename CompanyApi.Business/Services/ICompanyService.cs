using CompanyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyApi.Business.Services
{
    /// <summary>
    /// Defines the contract for company-related operations such as creating, retrieving, and updating companies.
    /// </summary>
    public interface ICompanyService
    {
        /// <summary>
        /// Asynchronously creates a new company.
        /// </summary>
        /// <param name="company">The <see cref="Company"/> object containing the details of the company to be created.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing the created <see cref="Company"/>.</returns>
        Task<Company> CreateCompanyAsync(Company company);

        /// <summary>
        /// Asynchronously retrieves a company by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the company.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing the <see cref="Company"/> if found; otherwise, <c>null</c>.</returns>
        Task<Company> GetCompanyByIdAsync(int id);

        /// <summary>
        /// Asynchronously retrieves a company by its ISIN (International Securities Identification Number).
        /// </summary>
        /// <param name="isin">The ISIN of the company.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing the <see cref="Company"/> if found; otherwise, <c>null</c>.</returns>
        Task<Company> GetCompanyByIsinAsync(string isin);

        /// <summary>
        /// Asynchronously retrieves all companies.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing a collection of <see cref="Company"/> objects.</returns>
        Task<IEnumerable<Company>> GetAllCompaniesAsync();

        /// <summary>
        /// Asynchronously updates the details of an existing company.
        /// </summary>
        /// <param name="id">The unique identifier of the company to be updated.</param>
        /// <param name="company">The <see cref="Company"/> object containing the updated company details.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, containing the updated <see cref="Company"/>.</returns>
        Task<Company> UpdateCompanyAsync(int id, Company company);
    }

}
