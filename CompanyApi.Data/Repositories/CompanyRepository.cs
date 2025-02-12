using CompanyApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyApi.Data.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly CompanyDbContext _context;

        public CompanyRepository(CompanyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Company company)
        {
            await _context.Companies.AddAsync(company);
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<Company> GetByIdAsync(int id)
        {
            return await _context.Companies.FindAsync(id);
        }

        public async Task<Company> GetByIsinAsync(string isin)
        {
            return await _context.Companies.FirstOrDefaultAsync(c => c.Isin == isin);
        }

        public async Task UpdateAsync(Company company)
        {
            _context.Companies.Update(company);
        }

        public async Task<bool> IsIsinExistsAsync(string isin)
        {
            return await _context.Companies.AnyAsync(c => c.Isin == isin);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
