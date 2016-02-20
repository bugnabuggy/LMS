using System.Linq;

namespace LMS.Core
{
    public interface IRepository<TModel>
        where TModel : class
    {
        IQueryable<TModel> Items { get; }

        TModel Add(TModel item);

        TModel Update(TModel item);

        void Delete(TModel item);
    }
}
