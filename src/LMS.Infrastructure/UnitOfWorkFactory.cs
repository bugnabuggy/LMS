using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Core;
using LMS.Infrastructure;
using Microsoft.Data.Entity;

namespace LMS.Services
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private ModelContext _context;

        public UnitOfWorkFactory(ModelContext context)
        {
            _context = context;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(_context);
        }
    }
}
