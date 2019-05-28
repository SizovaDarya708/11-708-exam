using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Exam.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Exam.Controllers
{
    public class UserFilesController : Controller
    {
        private readonly RUNContext db;
        private readonly IHostingEnvironment _appEnvironment;

        public UserFilesController(RUNContext context, IHostingEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index(int? Id)
        {

            if (Id == null)
            {
                User user = HomeController.GetUser(db, HttpContext);
                ViewBag.login = user.Id;
                ViewBag.Owner = true;
            }
            else
            {
                ViewBag.login = Id;
                ViewBag.Owner = false;
            }

            IQueryable<Models.File> users = db.Files.Include(u => u.User);
            return View(await users.AsNoTracking().ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFile = await db.Files
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userFile == null)
            {
                return NotFound();
            }

            return View(userFile);
        }

        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(db.Users, "Id", "Id");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Path,Time,UserId")] Models.File userFile, string text)
        {
            if (ModelState.IsValid)
            {
                User user = HomeController.GetUser(db, HttpContext);
                var time = DateTime.Now;
                var filename = userFile.Name + "_" + time.ToShortDateString().Replace('.', '-') + '-' + time.ToLongTimeString().Replace(':', '-') + ".txt";
                userFile.Time = time;
                    userFile.Link = "/Files/" + filename;
                db.Add(userFile);//в бд
                SaveToFile(text, filename);//в папку
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(db.Users, "Id", "Id", userFile.User);
            return View(userFile);
        }

        private void SaveToFile(string text, string filename)
        {
            string path = "/Files/" + filename;

            try
            {
                using (StreamWriter sw = new StreamWriter(_appEnvironment.WebRootPath + path, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(text);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFile = await db.Files.FindAsync(id);
            var realname = userFile.Link.Remove(0, 7).ToString();
            GetFile(realname);
            if (userFile == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(db.Users, "Id", "Id", userFile.User);
            return View(userFile);
        }

        private string ToShortName(string fileName, int count)
        {
            return fileName.Remove(fileName.Length - count, count);
        }

        private void GetFile(string fileName)
        {
            ViewData["fileName"] = fileName;
            try
            {
                var path = "/Files/" + fileName;
                using (StreamReader sr = new StreamReader(_appEnvironment.WebRootPath + path))
                {
                    ViewData["fileTXT"] = sr.ReadToEnd();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFile = await db.Files
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userFile == null)
            {
                return NotFound();
            }

            return View(userFile);
        }

        // POST: UserFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userFile = await db.Files.FindAsync(id);
            var path = _appEnvironment.WebRootPath + userFile.Link;
            RemoveFile(path);
            db.Files.Remove(userFile);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public FileResult Download(string filename)
        {
            var path = _appEnvironment.WebRootPath + filename;
            // Объект Stream
            FileStream fs = new FileStream(path, FileMode.Open);
            string file_type = "application/txt";
            string file_name = ToShortName(filename) + ".txt";
            return File(fs, file_type, file_name);
        }

        private string ToShortName(string fileName)
        {
            return fileName.Remove(fileName.Length - 24, 24).Remove(0, 7);
        }

        private bool UserFileExists(int id)
        {
            return db.Files.Any(e => e.Id == id);
        }

        private void RemoveFile(string path) => System.IO.File.Delete(path);
    }

}