using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Music_Portal.Models;

namespace Music_Portal.Controllers
{
    public class AdminController : Controller
    {
        private readonly MusicPortalContext _context;
        IWebHostEnvironment _appEnvironment;
        public AdminController(MusicPortalContext context, IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            _context = context;
        }
        public async Task<IActionResult> DetailsStyle()
        {
            return _context.Styles != null ?
                        View(await _context.Styles.ToListAsync()) :
                        Problem("Entity set 'MusicPortalContext.Styles'  is null.");
        }
        public async Task<IActionResult> DetailsUser()
        {
            return _context.Users != null ?
                        View(await _context.Users.ToListAsync()) :
                        Problem("Entity set 'MusicPortalContext.Users'  is null.");
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
                _context.Add(style);
                await _context.SaveChangesAsync();
                return RedirectToAction("DetailsStyle", "Admin");
            }
            return View(style);
        }

        public async Task<IActionResult> EditSong(int? id)
        {
            if (id == null || _context.Songs == null)
            {
                return NotFound();
            }

            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }
            ViewData["Style_id"] = new SelectList(_context.Styles, "Id", "Name", song.Style_id);
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
                    _context.Update(song);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongExists(song.Id))
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
        public async Task<IActionResult> EditUser(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
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
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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

        public async Task<IActionResult> DeleteStyle(int? id)
        {
            if (id == null || _context.Styles == null)
            {
                return NotFound();
            }

            var style = await _context.Styles
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.Styles == null)
            {
                return Problem("Entity set 'MusicPortalContext.Styles'  is null.");
            }
            var style = await _context.Styles.FindAsync(id);
            if (style != null)
            {
                _context.Styles.Remove(style);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("DetailsStyle", "Admin");
        }

        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.Users == null)
            {
                return Problem("Entity set 'MusicPortalContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("DetailsUser", "Admin");
        }

        public async Task<IActionResult> DeleteSong(int? id)
        {
            if (id == null || _context.Songs == null)
            {
                return NotFound();
            }
            var song = await _context.Songs
                .Include(p => p.Style)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if (_context.Songs == null)
            {
                return Problem("Entity set 'MusicPortalContext.Songs'  is null.");
            }
            var song = await _context.Songs.FindAsync(id);
            if (song != null)
            {
                _context.Songs.Remove(song);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        private bool StyleExists(int id)
        {
            return (_context.Styles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool SongExists(int id)
        {
            return (_context.Songs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
