using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace Duplex.Infrastructure.Aspectable
{
    public class AspectProvider : ExportProvider, IDisposable
    {

        private readonly CatalogExportProvider _exportProvider;

        public AspectProvider(Func<ComposablePartCatalog> catalogResolver)
        {
            _exportProvider = new CatalogExportProvider(catalogResolver());
            
            //support recomposition
            _exportProvider.ExportsChanged += (s, e) => OnExportsChanged(e);
            _exportProvider.ExportsChanging += (s, e) => OnExportsChanging(e);
        }


        public ExportProvider SourceProvider
        {
            get
            {
                return _exportProvider.SourceProvider;
            }
            set
            {
                _exportProvider.SourceProvider = value;
            }
        }


        protected override IEnumerable<Export> GetExportsCore(
            ImportDefinition definition, AtomicComposition atomicComposition)
        {
            var exports = _exportProvider.GetExports(definition, atomicComposition);
            return exports.Select(export => new Export(export.Definition, () => GetValue(export)));
        }

        private object GetValue(Export innerExport)
        {
            var value = innerExport.Value;
            if (innerExport.Metadata.Any(x => x.Key == "AreAspectsEnabled"))
            {
                var specificMetadata = 
                    innerExport.Metadata.Single(x => x.Key == "AreAspectsEnabled");

                return (Boolean)specificMetadata.Value == true ? AspectProxy.Factory(value) : value;
            }
            return value;
        }


        public void Dispose()
        {
            _exportProvider.Dispose();
        }
    }
}
