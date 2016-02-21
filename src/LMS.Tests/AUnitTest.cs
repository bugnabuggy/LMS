using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Core.Models;
using LMS.Services;

namespace LMS.Tests
{
    public abstract class AUnitTest
    {
        protected FakeRepository<Goal> _goalRepository;

        protected FakeRepository<UserArea> _userAreaRepository;

        protected AppContext _context;

        public AUnitTest()
        {
            _context = new AppContext();
            _context.TimeZoneId = "Bangladesh Standard Time";
            _userAreaRepository = new FakeRepository<UserArea>();
            _goalRepository = new FakeRepository<Goal>();
        }
    }
}
