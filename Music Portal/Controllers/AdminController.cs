using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Music_Portal.Models;
using Music_Portal.Repository;

namespace Music_Portal.Controllers
{
    public class AdminController : Controller
    {
        IRepository repo;
        IWebHostEnvironment _appEnvironment;

        public AdminController(IRepository r, IWebHostEnvironment appEnvironment)
        {
            repo = r;
            _appEnvironment = appEnvironment;
        }
        public async Task<IActionResult> DetailsStyle()
        {
            var styles = repo.GetStyleList();
            return View(styles);
        }
        public async Task<IActionResult> DetailsUser()
        {
            var styles = repo.GetUserList();
            return View(styles);
        }
        public IActionResult CreateStyle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStyle([Bind("Name")] Style style)
        {
            if (ModelState.IsValid)
            {
                repo.CreateStyle(style);
                await repo.Save();
                return RedirectToAction("DetailsStyle", "Admin");
            }
            return View(style);
        }

        public async Task<IActionResult> EditSong(int id)
        {
            if (id == null || repo.GetSongList == null)
            {
                return NotFound();
            }

            var song = await repo.GetSong(id);
            if (song == null)
            {
                return NotFound();
            }
            ViewData["Style_id"] = new SelectList(repo.GetAllSongs(), "Id", "Name", song.Style_id);
            return View(song);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSong(int id, [Bind("Id,Name,Year,Style_id,Album,Clip")] Song song, IFormFile uploadedFile)
        {
            if (id != song.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string path = "\\Clips\\" + uploadedFile.FileName;

                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                    song.Clip = path;
                    repo.UpdateSong(song);
                    await repo.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!repo.SongExists(song.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(song);
        }
        public async Task<IActionResult> EditUser(int id)
        {
            if (id == null || repo.GetUserList == null)
            {
                return NotFound();
            }

            var user = await repo.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, [Bind("Id,Name,Surname,Login,Email,Access_Level")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    repo.UpdateUser(user);
                    await repo.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!repo.UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("DetailsUser", "Admin");
            }
            return View(user);
        }

        public async Task<IActionResult> DeleteStyle(int id)
        {
            if (id == null || repo.GetSongList == null)
            {
                return NotFound();
            }

            var style = await repo.GetStyle(id);
            if (style == null)
            {
                return NotFound();
            }

            return View(style);
        }

        [HttpPost, ActionName("DeleteStyle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedStyle(int id)
        {
            if (repo.GetStyleList == null)
            {
                return Problem("Entity set 'MusicPortalContext.Styles'  is null.");
            }
            var style = await repo.GetStyle(id);
            if (style != null)
            {
                repo.DeleteStyle(style.Id);
            }

            await repo.Save();
            return RedirectToAction("DetailsStyle", "Admin");
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id == null || repo.GetUserList == null)
            {
                return NotFound();
            }

            var user = await repo.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedUser(int id)
        {
            if (repo.GetUserList == null)
            {
                return Problem("Entity set 'MusicPortalContext.Users'  is null.");
            }
            var user = await repo.GetUser(id);
            if (user != null)
            {
                repo.DeleteUser(user.Id);
            }

            await repo.Save();
            return RedirectToAction("DetailsUser", "Admin");
        }

        public async Task<IActionResult> DeleteSong(int id)
        {
            if (id == null || repo.GetSongList == null)
            {
                return NotFound();
            }
            var song = await repo.GetSong(id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        [HttpPost, ActionName("DeleteSong")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedSong(int id)
        {
            if (repo.GetSongList == null)
            {
                return Problem("Entity set 'MusicPortalContext.Songs'  is null.");
            }
            var song = await repo.GetSong(id);
            if (song != null)
            {
                repo.DeleteSong(song.Id);
            }

            await repo.Save();
            return RedirectToAction("Index", "Home");
        }
        
        
    }
}
