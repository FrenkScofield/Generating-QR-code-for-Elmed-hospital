using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Code_Reader.Models.BLL
{
    public class UserCovidTest
    {
        public int Id { get; set; }
        public string NameSurname { get; set; }
        public string FhaterName { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public string Department { get; set; }
        public string OnRequest { get; set; }
        public string TimeOfIssue { get; set; }
        public DateTime AtTheTimeOfApproval { get; set; }
        public string Doctor { get; set; }
        public string Indicators { get; set; }
        public string Result { get; set; }
        public string Norm { get; set; }
        public string IdCardNumber { get; set; }
    }
}
