using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyApi.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Exchange { get; set; }
        public string Ticker { get; set; }
        public string Isin { get; set; }
        public string? Website { get; set; }
    }
}
