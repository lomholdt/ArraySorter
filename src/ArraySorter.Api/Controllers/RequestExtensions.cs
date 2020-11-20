using ArraySorter.Application;
using System;

namespace ArraySorter.Api.Controllers.Job
{
    public static class RequestExtensions
    {
        public static EnqueueCommand ToCommand(this Request self, Guid id) =>
            new EnqueueCommand(id, self.Input);
    }
}
