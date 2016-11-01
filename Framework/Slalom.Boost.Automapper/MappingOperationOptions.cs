using System;
using System.Collections.Generic;

namespace Slalom.Boost.AutoMapper
{
    public class MappingOperationOptions<TSource, TDestination> : MappingOperationOptions,
        IMappingOperationOptions<TSource, TDestination>
    {
        public void BeforeMap(Action<TSource, TDestination> beforeFunction)
        {
            this.BeforeMapAction = (src, dest) => beforeFunction((TSource) src, (TDestination) dest);
        }

        public void AfterMap(Action<TSource, TDestination> afterFunction)
        {
            this.AfterMapAction = (src, dest) => afterFunction((TSource) src, (TDestination) dest);
        }
    }

    public class MappingOperationOptions : IMappingOperationOptions
    {
        public MappingOperationOptions()
        {
            this.Items = new Dictionary<string, object>();
            this.BeforeMapAction = (src, dest) => { };
            this.AfterMapAction = (src, dest) => { };
        }

        public Func<Type, object> ServiceCtor { get; private set; }
        public IDictionary<string, object> Items { get; }
        public bool DisableCache { get; set; }
        public Action<object, object> BeforeMapAction { get; protected set; }
        public Action<object, object> AfterMapAction { get; protected set; }

        public void BeforeMap(Action<object, object> beforeFunction)
        {
            this.BeforeMapAction = beforeFunction;
        }

        public void AfterMap(Action<object, object> afterFunction)
        {
            this.AfterMapAction = afterFunction;
        }

        void IMappingOperationOptions.ConstructServicesUsing(Func<Type, object> constructor)
        {
            this.ServiceCtor = constructor;
        }
    }
}