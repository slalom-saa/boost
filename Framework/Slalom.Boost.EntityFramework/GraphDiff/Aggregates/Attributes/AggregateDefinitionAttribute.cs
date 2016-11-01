using System;

namespace Slalom.Boost.EntityFramework.GraphDiff.Aggregates.Attributes
{
    public class AggregateDefinitionAttribute : Attribute
    {
        /// <summary>The aggregate type who the ownership/association refers to.</summary>
        public Type AggregateType { get; set; }
    }
}
