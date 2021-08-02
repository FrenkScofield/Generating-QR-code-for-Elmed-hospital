using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QR_Code_Reader.Models.BLL;
using QR_Code_Reader.Models.DAL;
using QR_Code_Reader.Models.VM;
using QRCoder;

namespace QR_Code_Reader.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QRCodeController : Controller
    {
        private readonly MyContext _context;
        public QRCodeController(MyContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.UserCovidTests.OrderByDescending(x => x.Id).ToList());
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserCovidTest userCovidTest, int genderId)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] array = Encoding.ASCII.GetBytes(userCovidTest.ToString());

                string qrInfo = $"Adı Soyadı - {userCovidTest.NameSurname}, " +
                                $"Ata adı {userCovidTest.FhaterName} ," +
                                $"İstək zamanı - {userCovidTest.OnRequest} ," +
                                $"Nəticənin çıxma vaxtı - {userCovidTest.TimeOfIssue} ," +
                                $"Təsdiq zamanı - {userCovidTest.AtTheTimeOfApproval.Date.ToString("dd/MM/yyyy")} ," +
                                $"Həkim - {userCovidTest.Doctor} ," +
                                $"Yaş - {userCovidTest.Age} ," +
                                $"Cinsiyyet - {userCovidTest.Sex} ," +
                                $"Şöbə - {userCovidTest.Department} ," +
                                $"Göstəricilər - {userCovidTest.Indicators} ," +
                                $"Nəticə - {userCovidTest.Result} ," +
                                $"Norma - {userCovidTest.Norm}";

                QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(qrInfo, QRCodeGenerator.ECCLevel.Q);
                QRCode qRCode = new QRCode(qRCodeData);

                using (Bitmap bitmap = qRCode.GetGraphic(20))
                {
                    bitmap.Save(ms, ImageFormat.Png);

                    ViewBag.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());


                }
                if (Convert.ToInt32(userCovidTest.Sex) == (int)Enum.Gender.Kişi)
                {
                    userCovidTest.Sex = "Kişi";
                }
                else
                {
                    userCovidTest.Sex = "Qadın";
                }
                _context.UserCovidTests.Add(userCovidTest);
                _context.SaveChanges();

            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> ShowUserInfo(int id)
        {
            var user = await _context.UserCovidTests.FindAsync(id);
            using (MemoryStream ms = new MemoryStream())
            {
                string getQrInfo = $"Adi Soyadi - {user.NameSurname} ," +
                                   $"Ata adi -  {user.FhaterName} ," +
                                   $"Yaş -  {user.Age} ," +
                                   $"Cinsiyyet -  {user.Sex} ," +
                                   $"Şöbə -  {user.Department} ," +
                                   $"İstək zamanı - {user.OnRequest} ," +
                                   $"Nəticənin çıxma vaxtı - {user.TimeOfIssue} ," +
                                   $"Təsdiq zamanı - {user.AtTheTimeOfApproval.Date.ToString("dd/MM/yyyy")} ," +
                                   $"Həkim - {user.Doctor} ," +
                                   $" Göstəricilər - {user.Indicators} ," +
                                   $" Nəticə - {user.Result} ," +
                                   $" Norma - {user.Norm}";

                QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(getQrInfo, QRCodeGenerator.ECCLevel.Q);
                QRCode qRCode = new QRCode(qRCodeData);

                using (Bitmap bitmap = qRCode.GetGraphic(20))
                {
                    bitmap.Save(ms, ImageFormat.Png);

                    ViewBag.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());


                }
            }

            return View(user);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var Covid = await _context.UserCovidTests.FindAsync(id);
            _context.UserCovidTests.Remove(Covid);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult PatientSearch()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PatientSearch(PatientSearchViewModel patientSearchViewModel)
        {
           
            if (!ModelState.IsValid) return View(patientSearchViewModel);

            var data = await _context.UserCovidTests.FindAsync(patientSearchViewModel);

            if(data.IdCardNumber == patientSearchViewModel.IdCartNumber)
            {
                return RedirectToAction(nameof(ShowUserInfo));
            }
            return View();
            
        }

    }
}
