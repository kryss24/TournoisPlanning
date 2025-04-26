using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournoisPlanning.Models
{
    public class Equipes
    {
        public int Id { get; set; }
        public int TournoiId { get; set; }
        public string Nom { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        //public string Logo { get; set; }
        public String Telephone { get; set; }
    }
}
