namespace LMS.Core
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}