using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsApp.Domain.Identity;

namespace TicketsApp.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<TicketsAppUser> GetAll();
        TicketsAppUser Get(string? id);
        void Insert(TicketsAppUser entity);
        void Update(TicketsAppUser entity);
        void Delete(TicketsAppUser entity);
    }
}
