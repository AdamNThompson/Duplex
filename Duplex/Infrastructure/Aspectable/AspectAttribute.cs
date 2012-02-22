using System;
using System.ComponentModel.Composition;

namespace Duplex.Infrastructure.Aspectable
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AspectAttribute : ExportAttribute
    {
        public AspectAttribute(Type contractType) : base(contractType)
        {
            AreAspectsEnabled = true;
        }
        public bool AreAspectsEnabled { get; set; }
    }
}
