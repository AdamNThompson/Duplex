using System;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
using Duplex.Infrastructure.Aspectable;
using System.Reflection;

namespace Duplex.Infrastructure
{
    // fuckin magic happen here
    public class Container
    {

        private CompositionContainer _container;
        private static readonly Lazy<Container> instance = new Lazy<Container>(() => new Container());

        private Container()
        {

        }

        public static Container Instance
        {
            get
            {
                return instance.Value;
            }
        }


        public T Resolve<T>()
        {
            return _container.GetExport<T>().Value;
        }



        public void Configure()
        {
            Func<ComposablePartCatalog> catalogResolver = () =>
            {
                var aggCat = new AggregateCatalog();
                aggCat.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
                return aggCat;
            };
            var provider = new AspectProvider(catalogResolver);
            _container = new CompositionContainer(provider);
            provider.SourceProvider = _container;
        }
    }
}
