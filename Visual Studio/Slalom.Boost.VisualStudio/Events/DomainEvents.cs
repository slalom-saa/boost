using System;
using System.Collections.Generic;
using System.Linq;

namespace Slalom.Boost.VisualStudio.Events
{
    public static class DomainEvents
    {
        private static List<Delegate> actions;

        public static void Register<T>(Action<T> callback) where T : IDomainEvent
        {
            if (actions == null)
            {
                actions = new List<Delegate>();
            }

            actions.Add(callback);
        }

        public static void ClearCallbacks()
        {
            actions = null;
        }

        public static void Raise<T>(T args) where T : IDomainEvent
        {
            if (actions != null)
            {
                foreach (var action in actions.ToList())
                {
                    if (action is Action<T>)
                    {
                        ((Action<T>)action)(args);
                    }
                }
            }
            foreach (var handler in Application.Current.Container.ResolveAll((typeof(IHandleEvent<>).MakeGenericType(args.GetType()))))
            {
                try
                {
                    ((dynamic)handler).Handle(args);
                }
                catch (Exception exception)
                {
                    BoostOutputWindow.WriteLine(exception.ToString());
                }
            }
        }
    }
}