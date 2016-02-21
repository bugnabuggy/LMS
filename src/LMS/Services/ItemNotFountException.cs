using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.Services
{
    public class ItemNotFountException : Exception
    {
        public ItemNotFountException(string message)
            :base(message)
        {
            
        }
    }
}
