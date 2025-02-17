using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsApp.Domain.Identity;

namespace TicketsApp.Domain.Domain
{
    public class Order : BaseEntity
    {
        public string OwnerId { get; set; }
        public TicketsAppUser Owner { get; set; }
        public List<TicketInOrder> TicketsInOrder { get; set; }
    }
}
