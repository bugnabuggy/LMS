using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Core;

namespace LMS.Tests
{
    public class UnitOfWorkFactoryFake : IUnitOfWorkFactory
    {
        public IUnitOfWork Create()
        {
            return new FakeUnitOfWork();
        }
    }

    public class FakeUnitOfWork : IUnitOfWork
    {
        public void Dispose()
        {
        }

        public void SaveChanges()
        {
        }
    }
}
