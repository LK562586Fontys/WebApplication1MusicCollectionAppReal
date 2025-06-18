using DataAccessLayer;
using Interfaces;
using LogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using WebApplication1MusicCollectionAppReal.Pages.ViewModels;

namespace WebApplication1MusicCollectionAppReal.Pages
{
    public class Song : PageModel
    {
        private readonly IUserService _userService;
        private readonly IPlaylistService _playlistService;
        private readonly ISongService _songService;
        private readonly ISongRepository _songRepository;
        private readonly IPlaylistRepository _playlistRepository;
        public SongViewModel viewModel { get; set; }
        public AccountViewModel userPlaylists { get; set; }
        private string ErrorMessage { get; set; }
        public static LogicLayer.Song CurrentSong { get; set; }
        public static LogicLayer.Playlist CurrentPlaylist { get; set; }
        private static User CurrentUser { get; set; }
        public List<Playlist> Playlists { get; set; }
        public Song(IUserService userService, IPlaylistService playlistService, ISongService songService, IPlaylistRepository playlistRepository, ISongRepository songRepository) 
        {
            _userService = userService;
            _playlistService = playlistService;
            _songService = songService;
            _playlistRepository = playlistRepository;
            _songRepository = songRepository;
        }
        public void OnGet(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                // Not logged in — redirect to login
                Response.Redirect("/Login");
                return;
            }

            // Set the current user
            CurrentUser = (User)_userService.GetUserById((int)userId);
            CurrentSong = (LogicLayer.Song)_songService.GetSongById(id);
            if (CurrentSong == null) 
            {
                Response.Redirect("/Error");
                return;
            }
            CurrentSong.GetSpecificSong(CurrentSong.ID);
            LoadSongData();
        }
        public void LoadSongData() 
        {
            viewModel = new SongViewModel { 
                ID = CurrentSong.ID,
                Name = CurrentSong.Name,
                DateReleased = CurrentSong.DateReleased,
                Weight = CurrentSong.Weight,
                Album = (Playlist)CurrentSong.Album,
                Artist = (User)CurrentSong.Artist,
            };
            var cookie = Request.Cookies["PlaylistOrder"];
            Playlists = CurrentUser.LoadPlaylists(_playlistRepository, _songRepository, cookie);
            userPlaylists = new AccountViewModel { 
                ID = CurrentUser.ID,
                Playlists = CurrentUser.userPlaylist
            };
        }

        public IActionResult OnPostAddSongToPlaylist() 
        {
            try
            {
                CurrentPlaylist.AddSong(CurrentSong.ID);
                return RedirectToPage();
            }
            catch (ArgumentException ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
            catch (Exception) 
            {
                ErrorMessage = "An unexpected error has occurred please try again later";
                return Page();
            }
        }
		public IActionResult OnPostUpdateWeight(int Weight)
		{
			CurrentSong.ChangeSongWeight(CurrentSong.ID, Weight);
			return RedirectToPage();
		}
        public IActionResult OnPostRemoveSongFromPlaylist()
        {
            try 
            {
                CurrentPlaylist.RemoveSong(CurrentSong.ID);
                return RedirectToPage();
            }
            catch (ArgumentException ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
            catch (Exception)
            {
                ErrorMessage = "An unexpected error has occurred please try again later";
                return Page();
            }
        }
    }
}
