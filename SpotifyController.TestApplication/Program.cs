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

            Console.WriteLine($"Current album: {album.Title}\n" +
                              $"Release date: {album.ReleaseDate:yyyy-MM-dd}\n" +
                              $"ID: {album.SpotifyId}");

            Console.ReadKey();
        }
    }
}