using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsAppAdmin.Models;

namespace TicketsAppAdmin.Models
{
    public class OrderDto
    {
        public List<TicketInOrder>? Tickets { get; set; }
        public double TotalPrice { get; set; }
    }
}
