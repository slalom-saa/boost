using System.Diagnostics;

namespace Slalom.Boost.EntityFramework.Extensions.Mapping
{
    /// <summary>
    /// A class representing a property map
    /// </summary>
    [DebuggerDisplay("Property: {PropertyName}, Column: {ColumnName}")]
    public class PropertyMap
    {
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        public string ColumnName { get; set; }
    }
}