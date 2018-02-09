using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyController.Data.Model
{
    [Table(Name = "Artist")]
    public class Artist
    {
        [Column(Name = "ID", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        public int Id { get; set; }

        [Column(Name = "Name", DbType = "VARCHAR")]
        public string Name { get; set; }

        public virtual Genre Genre { get; set; }
    }
}
