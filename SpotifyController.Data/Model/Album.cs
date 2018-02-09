using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyController.Data.Model
{
    [Table(Name = "Album")]
    public class Album
    {
        [Column(Name = "ID", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        public int Id { get; set; }

        [Column(Name = "Title", DbType = "VARCHAR")]
        public string Title { get; set; }

        [Column(Name = "Rating", DbType = "INTEGER")]
        public int Rating { get; set; }

        [Column(Name = "ReleaseDate", DbType = "DATE")]
        public DateTime ReleaseDate { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
