using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QR_Code_Reader.Models.VM
{
    public class PatientSearchViewModel
    {
        [Required(ErrorMessage = "Doldurulmalidir!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Doldurulmalidir!")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Doldurulmalidir!")]
        public string IdCartNumber { get; set; }
    }
}
