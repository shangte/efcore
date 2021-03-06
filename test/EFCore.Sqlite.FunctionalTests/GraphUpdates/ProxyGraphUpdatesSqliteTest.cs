﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable RCS1102 // Make class static.
namespace Microsoft.EntityFrameworkCore
{
    public class ProxyGraphUpdatesSqliteTest
    {
        public abstract class ProxyGraphUpdatesSqliteTestBase<TFixture> : ProxyGraphUpdatesTestBase<TFixture>
            where TFixture : ProxyGraphUpdatesSqliteTestBase<TFixture>.ProxyGraphUpdatesSqliteFixtureBase, new()
        {
            protected ProxyGraphUpdatesSqliteTestBase(TFixture fixture)
                : base(fixture)
            {
            }

            protected override void UseTransaction(DatabaseFacade facade, IDbContextTransaction transaction)
                => facade.UseTransaction(transaction.GetDbTransaction());

            public abstract class ProxyGraphUpdatesSqliteFixtureBase : ProxyGraphUpdatesFixtureBase
            {
                public TestSqlLoggerFactory TestSqlLoggerFactory => (TestSqlLoggerFactory)ListLoggerFactory;
                protected override ITestStoreFactory TestStoreFactory => SqliteTestStoreFactory.Instance;
            }
        }

        public class LazyLoading : ProxyGraphUpdatesSqliteTestBase<LazyLoading.ProxyGraphUpdatesWithLazyLoadingSqliteFixture>
        {
            public LazyLoading(ProxyGraphUpdatesWithLazyLoadingSqliteFixture fixture)
                : base(fixture)
            {
            }

            protected override bool DoesLazyLoading => true;
            protected override bool DoesChangeTracking => false;

            public class ProxyGraphUpdatesWithLazyLoadingSqliteFixture : ProxyGraphUpdatesSqliteFixtureBase
            {
                protected override string StoreName { get; } = "ProxyGraphLazyLoadingUpdatesTest";

                public override DbContextOptionsBuilder AddOptions(DbContextOptionsBuilder builder)
                    => base.AddOptions(builder.UseLazyLoadingProxies());

                protected override IServiceCollection AddServices(IServiceCollection serviceCollection)
                    => base.AddServices(serviceCollection.AddEntityFrameworkProxies());
            }
        }

        public class ChangeTracking : ProxyGraphUpdatesSqliteTestBase<ChangeTracking.ProxyGraphUpdatesWithChangeTrackingSqliteFixture>
        {
            public ChangeTracking(ProxyGraphUpdatesWithChangeTrackingSqliteFixture fixture)
                : base(fixture)
            {
            }

            protected override bool DoesLazyLoading => false;
            protected override bool DoesChangeTracking => true;

            public class ProxyGraphUpdatesWithChangeTrackingSqliteFixture : ProxyGraphUpdatesSqliteFixtureBase
            {
                protected override string StoreName { get; } = "ProxyGraphChangeTrackingUpdatesTest";

                public override DbContextOptionsBuilder AddOptions(DbContextOptionsBuilder builder)
                    => base.AddOptions(builder.UseChangeDetectionProxies());

                protected override IServiceCollection AddServices(IServiceCollection serviceCollection)
                    => base.AddServices(serviceCollection.AddEntityFrameworkProxies());
            }
        }

        public class ChangeTrackingAndLazyLoading : ProxyGraphUpdatesSqliteTestBase<ChangeTrackingAndLazyLoading.ProxyGraphUpdatesWithChangeTrackingAndLazyLoadingSqliteFixture>
        {
            public ChangeTrackingAndLazyLoading(ProxyGraphUpdatesWithChangeTrackingAndLazyLoadingSqliteFixture fixture)
                : base(fixture)
            {
            }

            protected override bool DoesLazyLoading => true;
            protected override bool DoesChangeTracking => true;

            public class ProxyGraphUpdatesWithChangeTrackingAndLazyLoadingSqliteFixture : ProxyGraphUpdatesSqliteFixtureBase
            {
                protected override string StoreName { get; } = "ProxyGraphChangeTrackingAndLazyLoadingUpdatesTest";

                public override DbContextOptionsBuilder AddOptions(DbContextOptionsBuilder builder)
                    => base.AddOptions(builder.UseChangeDetectionProxies().UseLazyLoadingProxies());

                protected override IServiceCollection AddServices(IServiceCollection serviceCollection)
                    => base.AddServices(serviceCollection.AddEntityFrameworkProxies());
            }
        }
    }
}
