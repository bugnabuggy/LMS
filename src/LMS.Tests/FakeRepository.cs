using System.Collections.Generic;
using System.Linq;
using LMS.Core;

namespace LMS.Tests
{
    public class FakeRepository<TModel> : IRepository<TModel>
        where TModel : class
    {
        public IQueryable<TModel> Items => Source.AsQueryable();

        public List<TModel> Source { get; set; }

        public FakeRepository()
        {
            Source = new List<TModel>();
        }

        public TModel Add(TModel item)
        {
            Source.Add(item);

            return item;
        }

        public TModel Update(TModel item)
        {
            //Do nothing
            return item;
        }

        public void Delete(TModel item)
        {
            Source.Remove(item);
        }
    }
}
