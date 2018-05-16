using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteMonitorApplication.Core.Services;

namespace WebsiteMonitorApplication.Services.Tests
{
    public class MemoryDbUoW : IUnitOfWork
    {
        private bool _ignoreDispose;

        public MemoryDbUoW(bool ignoreDispose = false)
        {
            _ignoreDispose = ignoreDispose;
        }

        private readonly Dictionary<Type, IMemoryRepository> _repositories = new Dictionary<Type, IMemoryRepository>();

        public void Dispose()
        {
            if (!_ignoreDispose)
            {
                _repositories.Clear();
            }
        }

        public IRepository<T> Get<T>() where T : class
        {
            if (!_repositories.ContainsKey(typeof(T)))
            {
                _repositories.Add(typeof(T), new MemoryRepository<T>());
            }
            return (IRepository<T>)_repositories[typeof(T)];
        }

        public void SaveChanges()
        {
            foreach (var repository in _repositories.Values)
            {
                repository.Submit();
            }
        }

        public Task SaveChangesAsync()
        {
            return new TaskFactory().StartNew(SaveChanges);
        }
    }

    public class MemoryDbUowFactory : IUnitOfWorkFactory
    {
        private readonly IUnitOfWork _uow;
        private readonly bool _ignoreDispose;

        public MemoryDbUowFactory(bool ignoreDispose = false)
        {
            _ignoreDispose = ignoreDispose;
        }

        public MemoryDbUowFactory(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IUnitOfWork Create()
        {
            if (_uow != null)
            {
                return _uow;
            }

            return new MemoryDbUoW(_ignoreDispose);
        }
    }
}