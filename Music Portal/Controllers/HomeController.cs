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

namespace Music_Portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly MusicPortalContext _context;
        IWebHostEnvironment _appEnvironment;

        public HomeController(MusicPortalContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var musicportalContext = _context.Songs;
            return View(await musicportalContext.ToListAsync());
        }

        public IActionResult CreateSong()
        {
            ViewData["Style_id"] = new SelectList(_context.Styles, "Id", "Name");
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
                _context.Add(song);
                await _context.SaveChangesAsync();
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
        public IActionResult Login(LoginModel logon)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.ToList().Count == 0)
                {
                    ModelState.AddModelError("", "Wrong login or password!");
                    return View(logon);
                }
                var users = _context.Users.Where(a => a.Login == logon.Login);
                if (users.ToList().Count == 0)
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
        public IActionResult Registration(RegisterModel reg)
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
                _context.Add(user);
                _context.SaveChanges();
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
