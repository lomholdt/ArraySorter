using System.Collections.Generic;
using System.Linq;

namespace ArraySorter.Api.Controllers.Job
{
  public record Request
  {
      public IEnumerable<int> Input { get; init; } = Enumerable.Empty<int>();
  }
}
