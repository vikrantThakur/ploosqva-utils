using System.Collections.Generic;
using Castle.Facilities.NHibernateIntegration.Components.Dao;
using NHibernate.Criterion;

namespace Ploosqva.ProjectBase.NHibernate
{
    public abstract class AbstractNHibernateDao<T, IdT> : IDao<T, IdT>
    {
        protected readonly INHibernateGenericDao nHibernateGenericDao;

        protected AbstractNHibernateDao(INHibernateGenericDao nHibernateGenericDao)
        {
            this.nHibernateGenericDao = nHibernateGenericDao;
        }

        #region Packaging of IGenericDao

        public IList<T> FindAll()
        {
            return new List<T>((T[])nHibernateGenericDao.FindAll(typeof(T)));
        }
        public IList<T> FindAll(int firstRow, int maxRows)
        {
            return new List<T>((T[])nHibernateGenericDao.FindAll(typeof (T), firstRow, maxRows));
        }
        public T FindById(IdT id)
        {
            return (T)nHibernateGenericDao.FindById(typeof(T), id);
        }
        public T Create(T instance)
        {
            return (T)nHibernateGenericDao.Create(instance);
        }
        public void Update(T instance)
        {
            nHibernateGenericDao.Update(instance);    
        }
        public void Delete(T instance)
        {
            nHibernateGenericDao.Delete(instance);
        }
        public void DeleteAll()
        {
            nHibernateGenericDao.DeleteAll(typeof(T));
        }
        public void Save(T instance)
        {
            nHibernateGenericDao.Save(instance);
        }

        #endregion

        #region Packaging  of INHibernateGenericDao

        public IList<T> FindAll(ICriterion[] criterias)
        {
            return new List<T>((T[])nHibernateGenericDao.FindAll(typeof(T), criterias));
        }
        public IList<T> FindAll(ICriterion[] criterias, int firstRow, int maxRows)
        {
            return new List<T>((T[])nHibernateGenericDao.FindAll(typeof(T), criterias, firstRow, maxRows));
        }
        public IList<T> FindAll(ICriterion[] criterias, Order[] sortItems)
        {
            return new List<T>((T[])nHibernateGenericDao.FindAll(typeof(T), criterias, sortItems));
        }
        public IList<T> FindAll(ICriterion[] criterias, Order[] sortItems, int firstRow, int maxRows)
        {
            return new List<T>((T[])nHibernateGenericDao.FindAll(typeof(T), criterias, sortItems, firstRow, maxRows));
        }
        public IList<T> FindAllWithCustomQuery(string queryString)
        {
            return new List<T>((T[])nHibernateGenericDao.FindAllWithCustomQuery(queryString));
        }
        public IList<T> FindAllWithCustomQuery(string queryString, int firstRow, int maxRows)
        {
            return new List<T>((T[])nHibernateGenericDao.FindAllWithCustomQuery(queryString, firstRow, maxRows));
        }
        public IList<T> FindAllWithNamedQuery(string namedQuery)
        {
            return new List<T>((T[])nHibernateGenericDao.FindAllWithNamedQuery(namedQuery));
        }
        public IList<T> FindAllWithNamedQuery(string namedQuery, int firstRow, int maxRows)
        {
            return new List<T>((T[])nHibernateGenericDao.FindAllWithNamedQuery(namedQuery, firstRow, maxRows));
        }
        public void InitializeLazyProperties(T instance)
        {
            nHibernateGenericDao.InitializeLazyProperties(instance);
        }
        public void InitializeLazyProperty(T instance, string propertyName)
        {
            nHibernateGenericDao.InitializeLazyProperty(instance, propertyName);
        }

        #endregion
    }
}