namespace Uspelite.Data.Repositories
{
    using System;
    using System.Linq;
    using Common.Contracts;

    public interface IRepository<T> : IDisposable where T : class, IBaseModel
    {
        IQueryable<T> All();

        T GetById(object id);

        void Add(T entity);

        void Update(T entity);

        IQueryable<T> AllWithDeleted();

        void HardDelete(T entity);

        void HardDelete(object id);

        void Delete(object Id);

        void Detach(T entity);

        int SaveChanges();
    }
}
