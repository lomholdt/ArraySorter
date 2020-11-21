using ArraySorter.Domain;
using System;
using System.Threading.Tasks;

namespace ArraySorter.Application
{
    public class EnqueueCommandHandler : ICommandHandler<EnqueueCommand>
    {
        private readonly ISystemTime _systemTime;

        private readonly IJobRepository _repository;

        public EnqueueCommandHandler(ISystemTime systemTime, IJobRepository repository)
        {
            _systemTime = systemTime ?? throw new ArgumentNullException(nameof(systemTime));

            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(EnqueueCommand command)
        {
            if (command.Id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty");
            }

            var job = new Job(command.Id);

            job.Enqueue(command.Input, _systemTime);

            _repository.Save(job);

            await Task.CompletedTask;
        }
    }
}
