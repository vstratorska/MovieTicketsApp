using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsApp.Domain.Domain
{
    public class Movie : BaseEntity
    {
        
        public string MovieName { get; set; }
        public string Genres { get; set; }
        public string MovieDescription { get; set; }
        public string MovieImage { get; set; }
        public double Rating { get; set; }

        public virtual List<Ticket>? Tickets { get; set; }
    }
}
