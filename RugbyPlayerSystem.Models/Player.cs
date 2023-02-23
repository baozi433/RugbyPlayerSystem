using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RugbyPlayerSystem.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string PlaceOfBirth { get; set; }
        public string? TeamName { get; set; }
    }
}
