using System;
using System.CodeDom;
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
        public event EventHandler<PlayStateEventArgs> PlayStateChanged;
        public event EventHandler<TrackChangeEventArgs> TrackChanged;
        public event EventHandler<VolumeChangeEventArgs> VolumeChanged;
        public event EventHandler<TrackTimeChangeEventArgs> TrackTimeChanged;

        private readonly SpotifyLocalAPI _client;
        private SpotifyWebAPI _webApi;

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _accessToken;

        private readonly string _testId = "06qjd7U01Ma8ZtY8B30Lim";

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

            if (_webApi.GetTrack(_testId).HasError())
                GetUserPermissions();
        }

        public bool Connect()
        {
            if (!_client.Connect())
                return false;

            _client.ListenForEvents = true;
            _client.OnPlayStateChange += ClientOnPlayStateChange;
            _client.OnTrackChange += ClientOnTrackChange;
            _client.OnTrackTimeChange += ClientOnTrackTimeChange;
            _client.OnVolumeChange += ClientOnVolumeChange;
            return true;
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

        #region EVENT_HANDLERS
        private void ClientOnVolumeChange(object sender, SpotifyAPI.Local.VolumeChangeEventArgs volumeChangeEventArgs)
        {
            VolumeChanged?.Invoke(this, (VolumeChangeEventArgs)volumeChangeEventArgs);
        }

        private void ClientOnTrackTimeChange(object sender, SpotifyAPI.Local.TrackTimeChangeEventArgs trackTimeChangeEventArgs)
        {
            TrackTimeChanged?.Invoke(this, (TrackTimeChangeEventArgs)trackTimeChangeEventArgs);
        }

        private void ClientOnTrackChange(object sender, SpotifyAPI.Local.TrackChangeEventArgs trackChangeEventArgs)
        {
            TrackChanged?.Invoke(this, (TrackChangeEventArgs)trackChangeEventArgs);
        }

        private void ClientOnPlayStateChange(object sender, SpotifyAPI.Local.PlayStateEventArgs playStateEventArgs)
        {
            PlayStateChanged?.Invoke(this, (PlayStateEventArgs)playStateEventArgs);
        }
        #endregion

        #region EVENT_ARGS_WRAPPERS

        public class PlayStateEventArgs : EventArgs
        {
            public bool Playing { get; set; }

            public static explicit operator PlayStateEventArgs(SpotifyAPI.Local.PlayStateEventArgs args)
            {
                return new PlayStateEventArgs() {Playing = args.Playing};
            }
        }

        public class TrackChangeEventArgs : EventArgs
        {

            public TrackModel PreviousTrack { get; set; }
            public TrackModel CurrentTrack { get; set; }

            public static explicit operator TrackChangeEventArgs(SpotifyAPI.Local.TrackChangeEventArgs args)
            {
                return new TrackChangeEventArgs()
                {
                    PreviousTrack = new TrackModel()
                    {
                        Length = args.OldTrack.Length,
                        Title = args.OldTrack.TrackResource.Name,
                        Uri = args.OldTrack.TrackResource.Uri
                    },
                    CurrentTrack = new TrackModel()
                    {
                        Length = args.NewTrack.Length,
                        Title = args.NewTrack.TrackResource.Name,
                        Uri = args.NewTrack.TrackResource.Uri
                    }
                };
            }
        }

        public class TrackTimeChangeEventArgs : EventArgs
        {
            public double TrackTime { get; set; }

            public static explicit operator TrackTimeChangeEventArgs(SpotifyAPI.Local.TrackTimeChangeEventArgs args)
            {
                return new TrackTimeChangeEventArgs() {TrackTime = args.TrackTime};
            }
        }

        public class VolumeChangeEventArgs : EventArgs
        {

            public double PreviousVolume { get; set; }
            public double CurrentVolume { get; set; }

            public static explicit operator VolumeChangeEventArgs(SpotifyAPI.Local.VolumeChangeEventArgs args)
            {
                return new VolumeChangeEventArgs()
                {
                    CurrentVolume = args.NewVolume,
                    PreviousVolume = args.OldVolume
                };
            }
        }

        #endregion
    }
}
