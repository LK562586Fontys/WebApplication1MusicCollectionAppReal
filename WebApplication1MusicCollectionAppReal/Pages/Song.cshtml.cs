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
        private readonly UserFactory _userService;
        private readonly PlaylistFactory _playlistService;
        private readonly SongFactory _songService;
        private readonly ISongRepository _songRepository;
        private readonly IPlaylistRepository _playlistRepository;
        public SongViewModel viewModel { get; set; }
        public AccountViewModel userPlaylists { get; set; }
        public string ErrorMessage { get; private set; }
        public static LogicLayer.Song CurrentSong { get; set; }
        public static LogicLayer.Playlist CurrentPlaylist { get; set; }
        private static User CurrentUser { get; set; }
        public List<Playlist> Playlists { get; set; }
        public Song(UserFactory userService, PlaylistFactory playlistService, SongFactory songService, IPlaylistRepository playlistRepository, ISongRepository songRepository) 
        {
            _userService = userService;
            _playlistService = playlistService;
            _songService = songService;
            _playlistRepository = playlistRepository;
            _songRepository = songRepository;
        }
        public IActionResult OnGet(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                // Not logged in — redirect to login
                return RedirectToPage ("/Login");
            }

            // Set the current user
            CurrentUser = _userService.GetUserById((int)userId);
            CurrentSong = (LogicLayer.Song)_songService.GetSongById(id);
            if (CurrentSong == null) 
            {
                return RedirectToPage("/Error", new {message = "Song Not Found"});
            }
            CurrentSong.GetSpecificSong(CurrentSong.ID);
            LoadSongData();
            return Page();
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
                AlbumPicture = ((Playlist)CurrentSong.Album)?.Photo
            };
            if (viewModel.AlbumPicture != null)
            {
                viewModel.AlbumBase64Picture = Convert.ToBase64String(viewModel.AlbumPicture);
            }
            var cookie = Request.Cookies["PlaylistOrder"];
            Playlists = CurrentUser.LoadPlaylists(cookie);
            userPlaylists = new AccountViewModel { 
                ID = CurrentUser.ID,
                Playlists = CurrentUser.userPlaylist
            };
        }

        public IActionResult OnPostAddSongToPlaylist(int playlistid) 
        {
            CurrentPlaylist = (LogicLayer.Playlist)_playlistService.GetPlaylistById(playlistid);
            try
            {
                CurrentPlaylist.AddSong(CurrentSong.ID);
                return RedirectToPage(new { id = CurrentSong.ID });
            }
            catch (ArgumentException ex)
            {
                ErrorMessage = ex.Message;
                LoadSongData();
                return Page();
            }
            catch (Exception) 
            {
                ErrorMessage = "An unexpected error has occurred please try again later";
                LoadSongData();
                return Page();
            }
        }
		public IActionResult OnPostUpdateWeight(int Weight)
		{
            try
            {
                CurrentSong.ChangeSongWeight(CurrentSong.ID, Weight);
                return RedirectToPage(new { id = CurrentSong.ID });
            }
            catch (ArgumentException ex) 
            {
                ErrorMessage = ex.Message;
                LoadSongData();
                return Page();
            }
            catch (Exception) 
            {
                ErrorMessage = "An unexpected error has occurred please try again later";
                LoadSongData();
                return Page();
            }
		}
        public IActionResult OnPostRemoveSongFromPlaylist(int playlistid)
        {
            CurrentPlaylist = (LogicLayer.Playlist)_playlistService.GetPlaylistById(playlistid);
            try 
            {
                CurrentPlaylist.RemoveSong(CurrentSong.ID);
                return RedirectToPage(new { id = CurrentSong.ID });
            }
            catch (ArgumentException ex)
            {
                ErrorMessage = ex.Message;
                LoadSongData();
                return Page();
            }
            catch (Exception)
            {
                ErrorMessage = "An unexpected error has occurred please try again later";
                LoadSongData();
                return Page();
            }
        }
    }
}
