using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsAppAdmin.Models
{
    public class Movie : BaseEntity
    {
        [Required]
        public string MovieName { get; set; }
        [Required]
        public string MovieDescription { get; set; }
        [Required]
        public string MovieImage { get; set; }
        [Required]
        public double Rating { get; set; }

        public virtual List<Ticket>? Tickets { get; set; }
    }
}
