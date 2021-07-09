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
using QRCoder;

namespace QR_Code_Reader.Controllers
{
    //[Authorize(Policy = "QrCodeonly")]
    [Authorize(Roles = "Admin")]

    public class QRCodeController : Controller
    {
        private readonly MyContext _context;
        public QRCodeController(MyContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(_context.UserCovidTests.ToList());
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserCovidTest userCovidTest)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] array = Encoding.ASCII.GetBytes(userCovidTest.ToString());
                string qrInfo = $"Adi Soyadi - {userCovidTest.Name} {userCovidTest.Surname} ,Dogum tarixi - {userCovidTest.BirthDay.Date.ToString("dd/MM/yyyy")} ,Test cavabi - {userCovidTest.CovidCode}";
                QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(qrInfo, QRCodeGenerator.ECCLevel.Q);
                QRCode qRCode = new QRCode(qRCodeData);

                using (Bitmap bitmap = qRCode.GetGraphic(20))
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    //ViewBag.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    ViewBag.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    //var qrCode = TempData["qrInfo"];


                }

                _context.UserCovidTests.Add(userCovidTest);
                _context.SaveChanges();

            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ShowUserInfo(int id)
        {
            var user = await _context.UserCovidTests.FindAsync(id);
            using (MemoryStream ms = new MemoryStream())
            {
                string getQrInfo = $"Adi Soyadi - {user.Name} {user.Surname} ,Dogum tarixi - {user.BirthDay.Date.ToString("dd/MM/yyyy")} ,Test cavabi - {user.CovidCode}";

                QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
                QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(getQrInfo, QRCodeGenerator.ECCLevel.Q);
                QRCode qRCode = new QRCode(qRCodeData);

                using (Bitmap bitmap = qRCode.GetGraphic(20))
                {
                    bitmap.Save(ms, ImageFormat.Png);
                    //ViewBag.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    ViewBag.QRCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    //var qrCode = TempData["qrInfo"];


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
   
    }
}
