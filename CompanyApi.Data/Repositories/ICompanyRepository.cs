using CompanyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyApi.Data.Repositories
{
    public interface ICompanyRepository
    {
        Task<Company> GetByIdAsync(int id);
        Task<Company> GetByIsinAsync(string isin);
        Task<IEnumerable<Company>> GetAllAsync();
        Task AddAsync(Company company);
        Task UpdateAsync(Company company);
        Task<bool> IsIsinExistsAsync(string isin);
        Task SaveChangesAsync();
    }
}
