//using System;
//using System.Data.Entity;
//using System.Linq;
//using System.Reflection;
//using System.Threading.Tasks;
//using Slalom.Boost.Commands;
//using Slalom.Boost.RuntimeBinding;

//namespace Slalom.Boost.EntityFramework.Aspects
//{
//    public abstract class EntityFrameworkAuditStore : IAuditStore
//    {
//        private readonly DbContext _context;

//        protected EntityFrameworkAuditStore(DbContext context)
//        {
//            _context = context;
//        }

//        public bool CanRead { get; } = false;

//        public Task SaveAsync<TResponse>(Command<TResponse> command, CommandResult<TResponse> result)
//        {
//            var audit = new CommandAudit(command, result);

//            _context.Set<CommandAudit>().Add(audit);

//            _context.SaveChanges();

//            return Task.FromResult(0);
//        }

//        public IQueryable<Commands.CommandAudit> Find()
//        {
//            throw new NotSupportedException();
//        }
//    }
//}