using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TFA.Api.Models;
using TFA.Domain.UseCases.CreateForum;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Domain.UseCases.GetForums;
using TFA.Domain.UseCases.GetTopics;

namespace TFA.Api.Controllers
{
    [ApiController]
    [Route("forums")]
    public class ForumController : ControllerBase
    {
        private readonly ILogger<ForumController> _logger;

        public ForumController(ILogger<ForumController> logger)
        {
            _logger = logger;
        }


        [HttpPost(Name = nameof(GetForums))]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(typeof(Forum), 201)]
        public async Task<IActionResult> CreateForum(
            [FromBody] CreateForumCommand command,
            [FromServices] ICreateForumStorage storage,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var createdForum = await storage.Create(command.Title, cancellationToken);
            
            return CreatedAtRoute(nameof(GetForums), mapper.Map<Forum>(createdForum));
        }


        [HttpGet(Name = nameof(GetForums))]
        [ProducesResponseType(typeof(Forum[]), 200)]
        public async Task<IActionResult> GetForums(
            [FromServices] IGetForumsUseCase getForumsUseCase,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var forums = await getForumsUseCase.Execute(cancellationToken);

            return Ok(mapper.Map<Forum[]>(forums));
        }



        [HttpPost("{forumId:Guid}/topics")]
        [ProducesResponseType(403)]
        [ProducesResponseType(410)]
        [ProducesResponseType(typeof(Topic), 201)]
        public async Task<IActionResult> CreateTopic([FromRoute] Guid forumId,
            [FromBody] string title,
            [FromServices] ICreateTopicUseCase createTopicUseCase,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var createTopicCommand = new CreateTopicCommand(forumId, title);
            var topic = await createTopicUseCase.Execute(createTopicCommand, cancellationToken);

            return CreatedAtRoute(nameof(GetForums), mapper.Map<Topic>(topic));
        }

        [HttpGet("{forumId:Guid}/topics")]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetTopics(
            [FromRoute] Guid forumId,
            [FromQuery] int skip,
            [FromQuery] int take,
            [FromServices] IGetTopicsUseCase getTopicsUseCase,
            [FromServices] IMapper mapper,
            CancellationToken cancellationToken)
        {
            var query = new GetTopicsQuery(forumId, skip, take);

            var (topics, totalCount) = await getTopicsUseCase.Execute(query, cancellationToken);

            return Ok(
                new 
                { 
                    topics = topics.Select(mapper.Map<Topic>),
                    totalCount 
                });
        }

    }
}