using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyController.Data.Context;
using SpotifyController.Data.Model;
using System.IO;

namespace SpotifyController.TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string accessToken = null;
            if (File.Exists("D:\\access_token.txt"))
                accessToken = File.ReadAllText("D:\\access_token.txt");

            var controller = new Controller.SpotifyController(ConfigurationManager.AppSettings.Get("Spotify.Client.ID"), string.Empty, accessToken);

            controller.AuthenticateUser();
            controller.Connect();

            var album = controller.CurrentAlbum();

            controller.PlayStateChanged += Controller_PlayStateChanged;
            controller.TrackChanged += Controller_TrackChanged;
            controller.VolumeChanged += Controller_VolumeChanged;
            controller.TrackTimeChanged += Controller_TrackTimeChanged;

            Console.WriteLine($"Current album: {album.Title}\n" +
                              $"Release date: {album.ReleaseDate:yyyy-MM-dd}\n" +
                              $"ID: {album.SpotifyId}");

            Console.ReadKey();
        }

        private static void Controller_TrackTimeChanged(object sender, Controller.SpotifyController.TrackTimeChangeEventArgs e)
        {
        }

        private static void Controller_VolumeChanged(object sender, Controller.SpotifyController.VolumeChangeEventArgs e)
        {
        }

        private static void Controller_TrackChanged(object sender, Controller.SpotifyController.TrackChangeEventArgs e)
        {
            Console.Clear();
            Console.WriteLine($"Now playing: {e.CurrentTrack.Title}");
        }

        private static void Controller_PlayStateChanged(object sender, Controller.SpotifyController.PlayStateEventArgs e)
        {
        }
    }
}