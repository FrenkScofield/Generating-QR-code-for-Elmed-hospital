using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace QR_Code_Reader.Models.VM
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Doldurulmalidir!"), EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Doldurulmalidir!"), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
