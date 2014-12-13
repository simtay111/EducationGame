using System.Configuration;
using System.Diagnostics;
using DataLayer.Mappings;
using DomainLayer;
using DomainLayer.Entities;
using DomainLayer.Entities.Stories;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace DataLayer.Configuration
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;


        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    Trace.WriteLine("Creating new NHibernate Session Factory");
                    InitializeSessionFactory();
                }

                return _sessionFactory;
            }
        }

        private static void InitializeSessionFactory()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[SystemConstants.ConnectionStringName].ConnectionString;
            _sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                              .ConnectionString(connectionString)
                              .ShowSql()
                )
                .Mappings(m =>
                          m.FluentMappings.AddFromAssemblyOf<StoryMapping>().Conventions
           .Add(FluentNHibernate.Conventions.Helpers.DefaultLazy.Never()))

                .ExposeConfiguration(cfg => new SchemaExport(cfg)

                                                .Create(true, false)).ExposeConfiguration(cfg => cfg.SetProperty(
                                        Environment.CurrentSessionContextClass,
                                        "web"))
                .BuildSessionFactory();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static ISession GetCurrentSession()
        {
            var session = SessionFactory.GetCurrentSession();
            return session;
        }
    }
}