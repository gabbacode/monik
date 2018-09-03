﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Monik.Common;
using Monik.Service;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Moq.Language.Flow;

namespace Monik.Test
{
    [TestClass]
    public abstract class CacheBaseTests<TCache, TEntity>
        where TCache : CacheBase<TEntity>
        where TEntity : ICacheEntity, new()
    {
        protected Mock<IRepository> Repo;
        protected Mock<ISourceInstanceCache> SourceCache;
        protected Mock<IMonik> Monik;
        protected TCache Cache;

        public abstract TCache CreateCache(IRepository repository, ISourceInstanceCache cache, IMonik monik);
        public abstract ISetup<IRepository, long> SetupRepoMaxId(Mock<IRepository> r);
        public abstract ISetup<IRepository, List<TEntity>> SetupRepoLast(Mock<IRepository> r);
        public abstract ISetup<IRepository> SetupRepoWriteEntities(Mock<IRepository> r);

        [TestInitialize]
        public void SetUp()
        {
            Repo = new Mock<IRepository>();
            SourceCache = new Mock<ISourceInstanceCache>();
            Monik = new Mock<IMonik>();
            Cache = CreateCache(Repo.Object, SourceCache.Object, Monik.Object);
        }

        [TestMethod]
        public void LastLogId_WhenCreate_Zero()
        {


            Assert.AreEqual(0, Cache.LastId);
        }

        [TestMethod]
        public void Add_AddingLog_WillIncrementID()
        {
            var log = new TEntity();

            Cache.Add(log);

            Assert.AreEqual(1, Cache.LastId);
        }

        [TestMethod]
        public void Add_AddingLog_WillSetLastIDToLogEntity()
        {
            var log = new TEntity {ID = 0};

            Cache.Add(log);

            Assert.AreEqual(Cache.LastId, log.ID);
        }

        [TestMethod]
        public void OnStart_Run_LoadsLastID()
        {
            SetupRepoMaxId(Repo).Returns(999);
            SetupRepoLast(Repo).Returns(new List<TEntity>());

            Cache.OnStart();

            Assert.AreEqual(999, Cache.LastId);
        }

        [TestMethod]
        public void Flush_OnRun_WillWritePendingLogs()
        {
            IEnumerable<TEntity> writtenValues = null;
            SetupRepoWriteEntities(Repo)
                .Callback((IEnumerable<TEntity> values) => writtenValues = values.ToList());
            var log = new TEntity();
            Cache.Add(log);

            Cache.Flush();

            Assert.IsTrue(new[] { log }.SequenceEqual(writtenValues));
        }

        [TestMethod]
        public void Flush_AfterRun_WillClearPendingLogs()
        {
            IEnumerable<TEntity> pendingLogs = null;
            SetupRepoWriteEntities(Repo)
                .Callback((IEnumerable<TEntity> values) => pendingLogs = values);
            var log = new TEntity();
            Cache.Add(log);

            Cache.Flush();

            Assert.AreEqual(0, pendingLogs.Count());
        }
    }
}