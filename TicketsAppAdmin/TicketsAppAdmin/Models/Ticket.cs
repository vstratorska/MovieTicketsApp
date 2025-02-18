using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsAppAdmin.Models
{
    public class Ticket : BaseEntity
    {
        public double Price { get; set; }
        public Guid MovieId { get; set; }
        public Movie? Movie { get; set; }

        public DateTime? Date { get; set; }

        public virtual ICollection<TicketInShoppingCart>? TicketsInShoppingCart { get; set; }
        public virtual IEnumerable<TicketInOrder>? TicketsInOrder { get; set; }
    }
}
