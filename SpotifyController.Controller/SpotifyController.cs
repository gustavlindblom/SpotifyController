using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Local;
using SpotifyAPI.Local.Enums;
using SpotifyAPI.Local.Models;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyController.Controller.Models;

namespace SpotifyController.Controller
{
    public class SpotifyController
    {

        private readonly SpotifyLocalAPI _client;
        private SpotifyWebAPI _webApi;

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _accessToken;

        public SpotifyController(string clientId, string clientSecret, string accessToken = null)
        {
            _client = new SpotifyLocalAPI();

            _clientId = clientId;
            _clientSecret = clientSecret;
            _accessToken = accessToken;
        }

        public bool AuthenticateUser()
        {
            if (!string.IsNullOrEmpty(_accessToken))
                AuthenitcateWebApi();
            else
                GetUserPermissions();

            return _webApi != null;
        }

        private async void GetUserPermissions()
        {
            WebAPIFactory webApiFactory = new WebAPIFactory("http://localhost", 8000, _clientId, Scope.Streaming, TimeSpan.FromSeconds(20));
            _webApi = new SpotifyWebAPI();
            try
            {
                _webApi = await webApiFactory.GetWebApi();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            File.WriteAllText("D:\\access_token.txt", _webApi.AccessToken);
        }

        private void AuthenitcateWebApi()
        {
            _webApi = new SpotifyWebAPI()
            {
                TokenType = "Bearer",
                AccessToken = _accessToken,
                UseAuth = true,
                UseAutoRetry = true
            };
        }

        public bool Connect()
        {
            return _client.Connect();
        }

        public TrackModel CurrentTrack()
        {
            var track = _client.GetStatus().Track;

            return new TrackModel()
            {
                Title = track.TrackResource.Name,
                Length = track.Length,
                Uri = track.TrackResource.Uri
            };
        }

        public AlbumModel CurrentAlbum()
        {
            var album = _client.GetStatus().Track.AlbumResource;

            var albumModel = new AlbumModel() {Title = album.Name, Uri = album.Uri};
            albumModel.ReleaseDate = DateTime.Parse(_webApi.GetAlbum(albumModel.SpotifyId).ReleaseDate);

            return albumModel;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
