using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        RUNContext db;
        IHostingEnvironment _appEnvironment;

        public async Task<IActionResult> UploadFiles(IFormFileCollection files, string ShortDes, string LongDes)
        {
            foreach (var file in files)
            {
                if (file != null)
                {
                    var path = "/Files/" + file.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    var newFile = new Models.File { Name = file.FileName, ShortDescription = ShortDes, FullDescription = LongDes, Link = path };
                    db.Files.Add(newFile);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]        
        public async Task<IActionResult> AddFile(IFormFileCollection uploads)
        {
            var login = HttpContext.User.Identity.Name;
            foreach (var uploadedFile in uploads)
            {
                var valid = uploadedFile.FileName.Split(".");
                if (valid[valid.Length - 1] != "txt")
                {
                    break;
                }
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                //User user = GetUser(db, HttpContext);
                Models.File file = new Models.File { Name = uploadedFile.FileName, Link = path, Time = DateTime.Now};
                db.Files.Add(file);
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AllFilesAsync()
        {
            IQueryable<Models.File> files = db.Files;
            return View(await files.AsNoTracking().ToListAsync());
        }
        public IActionResult Descriptions()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public static User GetUser(RUNContext db, HttpContext httpContext)
        {
            var login = httpContext.User.Identity.Name;
            return db.Users.FirstOrDefault(u => u.Login == login);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
