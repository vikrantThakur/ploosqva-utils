using System.Collections.Generic;
using Castle.Facilities.NHibernateIntegration;
using Castle.Facilities.NHibernateIntegration.Components.Dao;
using Castle.Services.Transaction;
using NHibernate.Criterion;

namespace Ploosqva.ProjectBase.NHibernate
{
    [Transactional]
    public abstract class AbstractNHibernateDao<T, IdT> : IDao<T, IdT>
    {
        protected readonly INHibernateGenericDao nHibernateGenericDao;
        protected readonly ISessionManager sessionManager;
        protected readonly string factoryAlias;

        protected AbstractNHibernateDao(ISessionManager sessionManager, string factoryAlias, INHibernateGenericDao nHibernateGenericDao)
        {
            this.nHibernateGenericDao = nHibernateGenericDao;
            this.factoryAlias = factoryAlias;
            this.sessionManager = sessionManager;
        }

        protected AbstractNHibernateDao(ISessionManager sessionManager, INHibernateGenericDao nHibernateGenericDao)
        {
            this.nHibernateGenericDao = nHibernateGenericDao;
            this.sessionManager = sessionManager;
        }

        #region Packaging of IGenericDao

        [Transaction(TransactionMode.NotSupported)]
        public IList<T> FindAll()
        {
            return new List<T>((T[])nHibernateGenericDao.FindAll(typeof(T)));
        }
        [Transaction(TransactionMode.NotSupported)]
        public IList<T> FindAll(int firstRow, int maxRows)
        {
            return new List<T>((T[])nHibernateGenericDao.FindAll(typeof (T), firstRow, maxRows));
        }
        [Transaction(TransactionMode.NotSupported)]
        public T FindById(IdT id)
        {
            return (T)nHibernateGenericDao.FindById(typeof(T), id);
        }
        [Transaction(TransactionMode.Requires)]
        public IdT Create(T instance)
        {
            return (IdT)nHibernateGenericDao.Create(instance);
        }
        [Transaction(TransactionMode.Requires)]
        public void Update(T instance)
        {
            nHibernateGenericDao.Update(instance);
        }
        [Transaction(TransactionMode.Requires)]
        public void Delete(T instance)
        {
            nHibernateGenericDao.Delete(instance);
        }
        [Transaction(TransactionMode.Requires)]
        public void DeleteAll()
        {
            nHibernateGenericDao.DeleteAll(typeof(T));
        }
        [Transaction(TransactionMode.Requires)]
        public void Save(T instance)
        {
            nHibernateGenericDao.Save(instance);
        }

        #endregion

        #region Packaging  of INHibernateGenericDao

        [Transaction(TransactionMode.NotSupported)]
        public IList<T> FindAll(ICriterion[] criterias)
        {
            return new List<T>((T[])nHibernateGenericDao.FindAll(typeof(T), criterias));
        }
        [Transaction(TransactionMode.NotSupported)]
        public IList<T> FindAll(ICriterion[] criterias, int firstRow, int maxRows)
        {
            return new List<T>((T[])nHibernateGenericDao.FindAll(typeof(T), criterias, firstRow, maxRows));
        }
        [Transaction(TransactionMode.NotSupported)]
        public IList<T> FindAll(ICriterion[] criterias, Order[] sortItems)
        {
            return new List<T>((T[])nHibernateGenericDao.FindAll(typeof(T), criterias, sortItems));
        }
        [Transaction(TransactionMode.NotSupported)]
        public IList<T> FindAll(ICriterion[] criterias, Order[] sortItems, int firstRow, int maxRows)
        {
            return new List<T>((T[])nHibernateGenericDao.FindAll(typeof(T), criterias, sortItems, firstRow, maxRows));
        }
        [Transaction(TransactionMode.NotSupported)]
        public IList<T> FindAllWithCustomQuery(string queryString)
        {
            return new List<T>((T[])nHibernateGenericDao.FindAllWithCustomQuery(queryString));
        }
        [Transaction(TransactionMode.NotSupported)]
        public IList<T> FindAllWithCustomQuery(string queryString, int firstRow, int maxRows)
        {
            return new List<T>((T[])nHibernateGenericDao.FindAllWithCustomQuery(queryString, firstRow, maxRows));
        }
        [Transaction(TransactionMode.NotSupported)]
        public IList<T> FindAllWithNamedQuery(string namedQuery)
        {
            return new List<T>((T[])nHibernateGenericDao.FindAllWithNamedQuery(namedQuery));
        }
        [Transaction(TransactionMode.NotSupported)]
        public IList<T> FindAllWithNamedQuery(string namedQuery, int firstRow, int maxRows)
        {
            return new List<T>((T[])nHibernateGenericDao.FindAllWithNamedQuery(namedQuery, firstRow, maxRows));
        }
        [Transaction(TransactionMode.NotSupported)]
        public void InitializeLazyProperties(T instance)
        {
            nHibernateGenericDao.InitializeLazyProperties(instance);
        }
        [Transaction(TransactionMode.NotSupported)]
        public void InitializeLazyProperty(T instance, string propertyName)
        {
            nHibernateGenericDao.InitializeLazyProperty(instance, propertyName);
        }

        #endregion
    }
}