using System.Threading.Tasks;

namespace ArraySorter.Application
{
    public interface ICommandHandler<TCommand> where TCommand : Command
    {
        Task Handle(TCommand command);
    }
}
