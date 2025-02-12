using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyApi.Business
{
    /// <summary>
    /// Contains error messages used throughout the application.
    /// </summary>
    public static class ErrorMessages
    {
        public const string InvalidIsinFormat = "ISIN format is invalid. The first two characters must be letters.";
        public const string DuplicateIsin = "A company with the same ISIN already exists.";
        public const string ErrorCreatingCompany = "An error occurred while creating the company.";
        public const string ErrorRetrievingCompanies = "An error occurred while retrieving companies.";
        public const string ErrorRetrievingCompany = "An error occurred while retrieving the company.";
        public const string ErrorUpdatingCompany = "An error occurred while updating the company.";
        public const string CompanyNotFound = "Company not found.";
    }
}
