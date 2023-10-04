using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Music_Portal.Models;
using Music_Portal.Repository;

namespace Music_Portal.Controllers
{
    public class HomeController : Controller
    {
        IRepository repo;
        IWebHostEnvironment _appEnvironment;

        public HomeController(IRepository r, IWebHostEnvironment appEnvironment)
        {
            repo = r;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var musicportalContext = await repo.GetSongList();
            return View(musicportalContext);
        }

        public IActionResult CreateSong()
        {
            ViewData["Style_id"] = new SelectList(repo.GetStyles(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSong([Bind("Name, Year, Style_id, Album, Clip")] Song song, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                if (uploadedFile != null)
                {
                    string path = "\\Clips\\" + uploadedFile.FileName; 

                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    song.Clip = path;
                }
                repo.CreateSong(song);
                await repo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(song);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel logon)
        {
            if (ModelState.IsValid)
            {
                if (await repo.GetUserList() == null)
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }
                var users = repo.GetUsers();
                if (users == null)
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }
                var item = users.First();
                if (item.Access_Level == -1)
                {
                    ModelState.AddModelError("", "Account not verified");
                    return View(logon);
                }
                HttpContext.Session.SetString("Login", Convert.ToString(item.Access_Level));
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Registration()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegisterModel reg)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.Name = reg.FirstName;
                user.Surname = reg.LastName;
                user.Login = reg.Login;
                user.Email = reg.Email;
                user.Password = reg.Password;
                user.Access_Level = -1;
                repo.CreateUser(user);
                await repo.Save();
                return RedirectToAction("Login", "Home");
            }

            return View(reg);
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }
    }
}
