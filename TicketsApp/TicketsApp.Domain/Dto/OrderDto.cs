﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsApp.Domain.Domain;

namespace TicketsApp.Domain.Dto
{
    public class OrderDto
    {
        public List<TicketInOrder>? Tickets { get; set; }
        public double TotalPrice { get; set; }
    }
}
