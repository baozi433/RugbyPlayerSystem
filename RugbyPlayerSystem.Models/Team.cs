using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RugbyPlayerSystem.Models
{
    public class Team
    {
        [Key]
        public string Name { get; set; }
        public string Ground { get; set; }
        public string Coach { get; set; }
        public int FoundedYear { get; set; }
        public string Region { get; set; }
        public string? UnionName { get; set; }
    }
}
