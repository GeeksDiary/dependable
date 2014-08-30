﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Dependable.Persistence
{
    public class InMemoryPersistenceStore : IPersistenceStore
    {
        readonly ConcurrentDictionary<Guid, Job> _items =
            new ConcurrentDictionary<Guid, Job>();

        public Job Load(Guid id)
        {
            return _items.Values.SingleOrDefault(i => i.Id == id);
        }

        public Job LoadBy(Guid correlationId)
        {
            return _items.Values.SingleOrDefault(i => i.CorrelationId == correlationId);
        }

        public IEnumerable<Job> LoadBy(JobStatus status)
        {
            return _items.Values.Where(i => i.Status == status);
        }

        public void Store(Job job)
        {
            _items.TryAdd(job.Id, job);
        }

        public void Store(IEnumerable<Job> jobs)
        {
            foreach(var j in jobs)
                Store(j);
        }

        public IEnumerable<Job> LoadSuspended(Type type, int max)
        {
            return _items.Values.Where(i => i.Type == type && i.Suspended).Take(max);
        }

        public int CountSuspended(Type type)
        {            
            return _items.Values.Count(i => i.Type == type && i.Suspended);
        }

        public void Dispose()
        {
        }
    }
}