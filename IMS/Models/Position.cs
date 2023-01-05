using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteNetExtensions.Attributes;

namespace IMF.Models
{
    public class Position
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [OneToOne(CascadeOperations = CascadeOperation.All)]
        public Symbol Symobl { get; set; }

        [ForeignKey(typeof(Account))]
        public int AccountId { get; set; }

        /// <summary>
        /// SOD,Executed and Reserved are cash notional based
        /// </summary>
        public float Sod { get; set; }
        public float Executed { get; set; }
        public float Reserved { get; set; }

    }
}
