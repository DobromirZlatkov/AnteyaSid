namespace AnteyaSidOnContainers.Services.Catalog.Data.Contracts
{
    using System.Linq;

    using Common.Models;

    public interface IDeletableEntityRepository<T> : IRepository<T> where T : class, IDeletableEntity
    {
        IQueryable<T> AllWithDeleted();

        void ActualDelete(T entity);
    }
}
