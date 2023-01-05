using Newtonsoft.Json.Linq;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace IMF.Models
{
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public float Cash { get; set; }
      
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Position>  Positions { get; set; }

    }

}
