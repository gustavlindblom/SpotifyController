using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyController.Controller.Models
{
    public class AlbumModel
    {
        public string Title { get; set; }
        public string Uri { get; set; }
        public string SpotifyId => Uri.Replace("spotify:album:", string.Empty);
        public DateTime ReleaseDate { get; set; }
    }
}
