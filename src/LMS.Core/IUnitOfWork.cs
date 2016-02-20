using System;

namespace LMS.Core
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}
