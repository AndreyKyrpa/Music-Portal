using Music_Portal.Models;
using System.Collections;

namespace Music_Portal.Repository
{
    public interface IRepository
    {
        Task Save();

        //User

        Task<List<User>> GetUserList();
        Task<User> GetUser(int id);
        public IEnumerable<User> GetUsers();
        public IEnumerable<User> GetUsersLogin(LoginModel logon);
        Task CreateUser(User item);
        void UpdateUser(User item);
        Task DeleteUser(int id);
        public bool UserExists(int id);

        //Style

        Task<List<Style>> GetStyleList();
        Task<Style> GetStyle(int id);
        Task CreateStyle(Style item);
        void UpdateStyle(Style item);
        Task DeleteStyle(int id);
        public bool StyleExists(int id);
        public IEnumerable<Style> GetStyles();

        //Song

        Task<List<Song>> GetSongList();
        Task<Song> GetSong(int id);
        Task CreateSong(Song item);
        void UpdateSong(Song item);
        Task DeleteSong(int id);
        public IEnumerable<Song> GetAllSongs();
        public bool SongExists(int id);
    }
}
