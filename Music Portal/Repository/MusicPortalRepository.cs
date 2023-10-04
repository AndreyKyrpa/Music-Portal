using Microsoft.EntityFrameworkCore;
using Music_Portal.Models;
using System.Numerics;
using System.Collections;

namespace Music_Portal.Repository
{
    public class MusicPortalRepository : IRepository
    {
        private readonly MusicPortalContext _context;

        public MusicPortalRepository(MusicPortalContext context)
        {
            _context = context;
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        //User
        public async Task<List<User>> GetUserList()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public async Task CreateUser(User c)
        {
            await _context.Users.AddAsync(c);
        }

        public void UpdateUser(User c)
        {
            _context.Entry(c).State = EntityState.Modified;
        }

        public async Task DeleteUser(int id)
        {
            User? c = await _context.Users.FindAsync(id);
            if (c != null)
                _context.Users.Remove(c);
        }
        public bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //Style
        public async Task<List<Style>> GetStyleList()
        {
            return await _context.Styles.ToListAsync();
        }
        public async Task<Style> GetStyle(int id)
        {
            return await _context.Styles.FindAsync(id);
        }

        public async Task CreateStyle(Style c)
        {
            await _context.Styles.AddAsync(c);
        }

        public void UpdateStyle(Style c)
        {
            _context.Entry(c).State = EntityState.Modified;
        }

        public async Task DeleteStyle(int id)
        {
            Style? c = await _context.Styles.FindAsync(id);
            if (c != null)
                _context.Styles.Remove(c);
        }

        public bool StyleExists(int id)
        {
            return (_context.Styles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public IEnumerable<Style> GetStyles()
        {
            return _context.Styles.ToList();
        }

        //Song
        public async Task<List<Song>> GetSongList()
        {
            return await _context.Songs.ToListAsync();
        }
        public async Task<Song> GetSong(int id)
        {
            return await _context.Songs.FindAsync(id);
        }

        public async Task CreateSong(Song c)
        {
            await _context.Songs.AddAsync(c);
        }

        public void UpdateSong(Song c)
        {
            _context.Entry(c).State = EntityState.Modified;
        }

        public async Task DeleteSong(int id)
        {
            Song? c = await _context.Songs.FindAsync(id);
            if (c != null)
                _context.Songs.Remove(c);
        }

        public IEnumerable<Song> GetAllSongs()
        {
            return _context.Songs.ToList();
        }

        public bool SongExists(int id)
        {
            return (_context.Songs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}