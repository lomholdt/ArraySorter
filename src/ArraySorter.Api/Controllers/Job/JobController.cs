using ArraySorter.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ArraySorter.ReadModels;

namespace ArraySorter.Api.Controllers.Job
{
    [ApiController]
    [Route(Constants.Routes.Jobs)]
    public class JobController : ControllerBase
    {
        private readonly ILogger<JobController> _logger;

        private readonly ICommandHandler<EnqueueCommand> _handler;
        private readonly IJobQueries _queries;

        public JobController(
            ILogger<JobController> logger,
            ICommandHandler<EnqueueCommand> handler,
            IJobQueries queries)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _handler = handler ?? throw new ArgumentNullException(nameof(handler));

            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
        }

        [HttpPost]
        public async Task<OkObjectResult> Enqueue([FromBody] Request request)
        {
            var id = Guid.NewGuid();

            _logger.LogInformation($"Enqueueing Job with id {id}");

            await _handler.Handle(request.ToCommand(id));

            return Ok($"{nameof(Enqueue)} with id {id}");
        }

        [HttpGet]
        public OkObjectResult Overview()
        {
            _logger.LogInformation("Getting Overview");

            var jobs = _queries.Get();

            return Ok(jobs);
        }

        [HttpGet("{id}")]
        public OkObjectResult Get([FromRoute] Guid id)
        {
            _logger.LogInformation($"Getting Job {id}");

            var job = _queries.Get(id);

            return Ok(job);
        }
    }
}
