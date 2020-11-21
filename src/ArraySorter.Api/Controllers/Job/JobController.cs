using ArraySorter.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using ArraySorter.ReadModels;
using System.Collections.Generic;

namespace ArraySorter.Api.Controllers.Job
{
    [ApiController]
    [Route(Constants.Routes.Jobs)]
    public class JobController : ControllerBase
    {
        private readonly ILogger<JobController> _logger;

        private readonly ICommandHandler<EnqueueCommand> _handler;
        private readonly IJobQueries _readModel;

        public JobController(
            ILogger<JobController> logger,
            ICommandHandler<EnqueueCommand> handler,
            IJobQueries queries)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _handler = handler ?? throw new ArgumentNullException(nameof(handler));

            _readModel = queries ?? throw new ArgumentNullException(nameof(queries));
        }

        [HttpPost]
        public async Task<OkObjectResult> Enqueue([FromBody] Request request)
        {
            var id = Guid.NewGuid();

            _logger.LogInformation($"Enqueueing Job with id {id}");

            await _handler.Handle(request.ToCommand(id));

            return Ok(id);
        }

        [HttpGet]
        public IEnumerable<ReadModels.Job> Overview()
        {
            _logger.LogInformation("Getting Overview");

            var jobs = _readModel.Get();

            return jobs;
        }

        [HttpGet("{id}")]
        public ReadModels.Job Get([FromRoute] Guid id)
        {
            _logger.LogInformation($"Getting Job {id}");

            var job = _readModel.Get(id);

            return job;
        }
    }
}
