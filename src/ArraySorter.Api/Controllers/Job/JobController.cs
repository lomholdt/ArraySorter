using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ArraySorter.Api.Controllers.Job
{
    [ApiController]
    [Route(Constants.Routes.Jobs)]
    public class JobController : ControllerBase
    {
        private readonly ILogger<JobController> _logger;

        public JobController(ILogger<JobController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<OkObjectResult> Enqueue([FromBody] Request request)
        {
            // Enqueue job as a CQRS command

            await Task.CompletedTask;

            _logger.LogInformation("Enqueuing Job");

            return Ok(nameof(Enqueue));
        }

        [HttpGet]
        public async Task<OkObjectResult> Overview()
        {
            // Get from read model

            await Task.CompletedTask;

            _logger.LogInformation("Getting Overview");

            return Ok(nameof(Overview));
        }

        [HttpGet("{id}")]
        public async Task<OkObjectResult> Get([FromRoute] Guid id)
        {
            // Get from read model

            await Task.CompletedTask;

            _logger.LogInformation($"Getting Job {id}");

            return Ok(nameof(Get));
        }
    }
}
