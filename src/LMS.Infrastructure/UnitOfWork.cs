using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using LMS.Core;
using Microsoft.Data.Entity;

namespace LMS.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContext _context;

        public UnitOfWork(ModelContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            //Do nothing
        }
    }
}
