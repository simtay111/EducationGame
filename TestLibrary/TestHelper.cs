using System.Diagnostics;
using DataLayer.Mappings;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace TestLibrary
{
    public class TestHelper
    {
        private static ISessionFactory _sessionFactory;


        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    Trace.WriteLine("Creating new factory");
                    InitializeSessionFactory();
                }

                Trace.WriteLine("Returning New Factory");

                return _sessionFactory;
            }
        }

        private static void InitializeSessionFactory()
        {
            _sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                              .ConnectionString(
                @"integrated security=sspi;data source=(local);initial catalog=EducationGame;")
                              .ShowSql()
                )
                .Mappings(m =>
                          m.FluentMappings.AddFromAssemblyOf<StoryMapping>().Conventions
           .Add(FluentNHibernate.Conventions.Helpers.DefaultLazy.Never()))

                .ExposeConfiguration(cfg => new SchemaExport(cfg)

                                                .Create(true, true)).ExposeConfiguration(cfg => cfg.SetProperty(
                                        Environment.CurrentSessionContextClass,
                                        "web"))
                .BuildSessionFactory();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static void FlushAndDispose()
        {
            var currentSession = SessionFactory.GetCurrentSession();
            Trace.WriteLine("Disposing");
            currentSession.Flush();
            SessionFactory.GetCurrentSession().Dispose();
        }

        public static ISession GetCurrentSession()
        {
            Trace.WriteLine("GETTING CURRENT SESSION");
             var session = SessionFactory.GetCurrentSession();
            return session;
        }
    }
}