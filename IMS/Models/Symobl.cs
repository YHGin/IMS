using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMF.Models
{
    public class Symbol
    {
        [Key]
        public int Id { get; set; }
        public string Sedol { get; set; }
        public string InstrumentType { get; set; }
        public string Exchange{ get; set; }
    }
}
