using System;
using System.Diagnostics;
using Slalom.Boost.Commands;
using Swashbuckle.Swagger;

namespace Slalom.Boost.WebApi
{
    public class OmitCommandProperties : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            if ((type.BaseType?.IsGenericType ?? false) && type.BaseType?.GetGenericTypeDefinition() == typeof(Command<>))
            {
                schema.properties.Remove("id");
                schema.properties.Remove("commandType");
            }
        }
    }
}