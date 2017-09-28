using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Slalom.Boost.Commands;
using CommandAudit = Slalom.Boost.EntityFramework.Aspects.CommandAudit;

namespace Slalom.Boost.EntityFramework.Logging
{
    public abstract class SqlAuditStore : IAuditStore
    {
        private readonly LoggingDbContext _context;

        protected SqlAuditStore(LoggingDbContext context)
        {
            _context = context;
        }

        public bool CanRead { get; } = false;

        public Task SaveAsync<TResponse>(Command<TResponse> command, CommandResult<TResponse> result)
        {
            var audit = new CommandAudit(command, result);

            _context.Requests.Add(new RequestEntryItem
            {
                CorrelationId = audit.CorrelationId.ToString("D"),
                EntryId = Guid.NewGuid().ToString("D"),
                Body = audit.CommandPayload,
                RequestType = command.GetType().FullName,
                //Parent = 
                Path = HttpContext.Current?.Request?.Path,
                SessionId = audit.Session,
               // SourceAddress = ,
                TimeStamp = audit.TimeStamp,
                UserName = audit.UserName,
                RequestId = audit.CommandId.ToString("D"),
                ApplicationName = audit.Application,
                Environment = audit.Environment,
                MachineName = audit.MachineName
            });

            _context.Responses.Add(new ResponseEntryItem
            {
                CorrelationId = audit.CorrelationId.ToString("D"),
                EntryId = Guid.NewGuid().ToString("D"),
                Path = HttpContext.Current?.Request?.Path,
                TimeStamp = audit.TimeStamp,
                RequestId = audit.CommandId.ToString("D"),
                ApplicationName = audit.Application,
                Environment = audit.Environment,
                MachineName = audit.MachineName,
                Build = audit.Build,
                Completed = audit.Completed,
                //Cancelled = audit.Cancelled,
                Elapsed = audit.Elapsed,
                Exception = audit.Exception?.Trim(4000),
                Function = audit.CommandType,
                IsSuccessful = audit.Successful,
                Started = audit.Started,
                ValidationErrors = (audit.ValidationMessages != null && audit.ValidationMessages.Any()) ? String.Join(";#;", audit.ValidationMessages).Trim(4000) : null,
                Version = ConfigurationManager.AppSettings["Version"]
            });

            _context.SaveChanges();


            return Task.FromResult(0);
        }

        public IQueryable<Commands.CommandAudit> Find()
        {
            throw new NotSupportedException();
        }
    }
}