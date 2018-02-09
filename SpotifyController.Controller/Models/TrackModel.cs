using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyController.Controller.Models
{
    public class TrackModel
    {
        public string Title { get; set; }
        public int Length { get; set; }
        public string Uri { get; set; }
        public string SpotifyId => Uri.Replace("spotify:track:", string.Empty);
    }
}
