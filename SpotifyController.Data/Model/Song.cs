using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyController.Data.Model
{
    [Table(Name = "Song")]
    public class Song
    {
        [Column(Name = "ID", IsPrimaryKey = true, IsDbGenerated = true, DbType = "INTEGER")]
        public int Id { get; set; }

        [Column(Name = "Title", DbType = "VARCHAR")]
        public string Title { get; set; }

        [Column(Name = "Length", DbType = "INTEGER")]
        public int Length { get; set; }

        [Column(Name = "Rating", DbType = "INTEGER")]
        public int Rating { get; set; }

        public virtual Artist Artist { get; set; }
        public virtual Album Album { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
