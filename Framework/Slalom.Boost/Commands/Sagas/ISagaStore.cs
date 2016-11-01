using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Slalom.Boost.Commands.Sagas
{
    [GeneratedCode("TODO", "1")]
    public interface ISagaStore
    {
        TSaga Find<TSaga>(Guid id) where TSaga : ISaga;

        void Save<TSaga>(TSaga instance) where TSaga : ISaga;

        bool Exists<TSaga>(Expression<Func<TSaga, bool>> filter) where TSaga : ISaga;
    }
}