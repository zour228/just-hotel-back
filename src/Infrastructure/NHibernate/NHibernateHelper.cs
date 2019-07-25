using System;
using System.Linq;
using app.Common;
using app.DependencyInjection;
using app.Domain.Entity;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace app.Infrastructure.NHibernate
{
    public class NHibernateHelper : AbstractAssemblyAware
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                lock (new object())
                {
                    if (_sessionFactory == null)
                    {
                        CompileSessionFactory();
                    }
                }

                return _sessionFactory;
            }
        }

        public static void Boot()
        {
            CompileSessionFactory();
        }

        private static void CompileSessionFactory()
        {
            var fluentConfiguration = Fluently
                .Configure()
                .Database(PostgreSQLConfiguration
                    .PostgreSQL82
                    //Понятия не имею что это и для чего. Но без этого не работает ¯\_(ツ)_/¯
                    .Raw("hbm2ddl.keywords", "none")
                    .ConnectionString(DbAccessor.ConnectionString));

            RegisterEntitiesRecursively(fluentConfiguration, typeof(AbstractEntity));

            _sessionFactory = fluentConfiguration
//                .ExposeConfiguration(cfg => new SchemaUpdate(cfg)
//                    .Execute(false, true))
                .BuildSessionFactory();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        /// <summary>
        /// Добавит всех наследников абстракного класса в конфигурацию Nhibernate (рекурсивно)
        /// </summary>
        /// <param name="configuration">Конфигурация Fluent NHibernate</param>
        /// <param name="class">Тип абстракного класса</param>
        private static void RegisterEntitiesRecursively(FluentConfiguration configuration, Type @class)
        {
            var entities = GetAssembly()
                .DefinedTypes
                .Where(type => type.IsSubclassOf(@class));

            foreach (var entity in entities)
            {
                if (entity.IsAbstract)
                {
                    RegisterEntitiesRecursively(configuration, entity);
                }
                else
                {
                    configuration.Mappings(cfg => cfg.FluentMappings.AddFromAssembly(entity.Assembly));
                }
            }
        }
    }
}