using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyController.Data.Model
{
    [Table(Name = "Genre")]
    public class Genre
    {
        [Column(Name = "ID", IsPrimaryKey = true, IsDbGenerated = true, DbType = "INTEGER")]
        public int Id { get; set; }

        [Column(Name = "Name", DbType = "VARCHAR")]
        public string Name { get; set; }
    }
}
