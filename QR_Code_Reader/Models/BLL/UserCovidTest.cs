using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Code_Reader.Models.BLL
{
    public class UserCovidTest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDay { get; set; }
        public string CovidCode { get; set; }
    }
}
