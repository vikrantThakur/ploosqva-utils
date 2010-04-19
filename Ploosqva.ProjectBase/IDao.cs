using System.Collections.Generic;
using NHibernate.Criterion;

namespace Ploosqva.ProjectBase
{
    public interface IDao<T, IdT>
    {
        IList<T> FindAll();
        IList<T> FindAll(int firstRow, int maxRows);
        T FindById(IdT id);
        T Create(T instance);
        void Update(T instance);
        void Delete(T instance);
        void DeleteAll();
        void Save(T instance);
        IList<T> FindAll(ICriterion[] criterias);
        IList<T> FindAll(ICriterion[] criterias, int firstRow, int maxRows);
        IList<T> FindAll(ICriterion[] criterias, Order[] sortItems);
        IList<T> FindAll(ICriterion[] criterias, Order[] sortItems, int firstRow, int maxRows);
        IList<T> FindAllWithCustomQuery(string queryString);
        IList<T> FindAllWithCustomQuery(string queryString, int firstRow, int maxRows);
        IList<T> FindAllWithNamedQuery(string namedQuery);
        IList<T> FindAllWithNamedQuery(string namedQuery, int firstRow, int maxRows);
        void InitializeLazyProperties(T instance);
        void InitializeLazyProperty(T instance, string propertyName);
    }
}