using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace WebsiteMonitorApplication.Services.Tests
{
    public class MemoryQueryable<T> : IQueryable<T>, IAsyncEnumerable<T> where T : class
    {
        private readonly ObservableCollection<T> _insertedEntities;
        private readonly ObservableCollection<T> _updatedEntities;
        private readonly ObservableCollection<T> _deletedEntities;
        private readonly ObservableCollection<T> _unchangedEntities;

        private readonly IQueryable _queryable;

        public MemoryQueryable()
        {
            _insertedEntities = new ObservableCollection<T>();
            _updatedEntities = new ObservableCollection<T>();
            _deletedEntities = new ObservableCollection<T>();
            _unchangedEntities = new ObservableCollection<T>();

            _queryable = _unchangedEntities.AsQueryable();
        }

        public void Add(T entity)
        {
            _insertedEntities.Add(entity);
        }

        public void UpdateOnSubmit(T entity)
        {
            _unchangedEntities.Remove(entity);
            _updatedEntities.Add(entity);
        }

        public void Remove(T entity)
        {
            _unchangedEntities.Remove(entity);
            _deletedEntities.Add(entity);
        }

        public void Submit()
        {
            foreach (var _insertedEntity in _insertedEntities)
            {
                _unchangedEntities.Add(_insertedEntity);
            }

            foreach (var _updatedEntity in _updatedEntities)
            {
                _unchangedEntities.Add(_updatedEntity);
            }

            _deletedEntities.Clear();
            _insertedEntities.Clear();
            _updatedEntities.Clear();
        }

        Type IQueryable.ElementType
        {
            get { return _queryable.ElementType; }
        }

        Expression IQueryable.Expression
        {
            get { return _queryable.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new AsyncQueryProviderWrapper<T>(_queryable.Provider); }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _unchangedEntities.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _unchangedEntities.GetEnumerator();
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new AsyncEnumeratorWrapper<T>(_unchangedEntities.GetEnumerator());
        }

        public IAsyncEnumerator<T> GetEnumerator()
        {
            return new AsyncEnumeratorWrapper<T>(_unchangedEntities.GetEnumerator());
        }
    }

    public class AsyncQueryProviderWrapper<T> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal AsyncQueryProviderWrapper(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new AsyncEnumerableQuery<T>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new AsyncEnumerableQuery<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            return new AsyncEnumerableQuery<TResult>(expression);
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }
    }

    public class AsyncEnumerableQuery<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable
    {
        public AsyncEnumerableQuery(IEnumerable<T> enumerable)
            : base(enumerable)
        {
        }

        public AsyncEnumerableQuery(Expression expression)
            : base(expression)
        {
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new AsyncEnumeratorWrapper<T>(this.AsEnumerable().GetEnumerator());
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new AsyncQueryProviderWrapper<T>(this); }
        }

        public IAsyncEnumerator<T> GetEnumerator()
        {
            return new AsyncEnumeratorWrapper<T>(this.AsEnumerable().GetEnumerator());
        }
    }

    public class AsyncEnumeratorWrapper<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public AsyncEnumeratorWrapper(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        //object IDbAsyncEnumerator.Current
        //{
        //    get { return Current; }
        //}
    }
}