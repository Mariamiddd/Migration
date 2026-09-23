using System;
using System.Collections.Generic;
using System.Text;

namespace MovieDomain.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();

    }
}
