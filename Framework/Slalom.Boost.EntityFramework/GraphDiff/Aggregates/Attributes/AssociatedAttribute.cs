using System;

namespace Slalom.Boost.EntityFramework.GraphDiff.Aggregates.Attributes
{
    /// <summary>Marks this property as associated by the parent type or by the chosen AggregateType</summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AssociatedAttribute : AggregateDefinitionAttribute
    {
    }
}
