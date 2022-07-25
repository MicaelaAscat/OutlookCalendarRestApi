using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;

namespace OutlookCalendarRestApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Events")]
    public class OutlookCalendarController : ControllerBase
    {
        private readonly GraphServiceClient _graphServiceClient;
        private readonly ILogger<OutlookCalendarController> _logger;

        public OutlookCalendarController(ILogger<OutlookCalendarController> logger, GraphServiceClient graphServiceClient)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;
        }

        [HttpGet]
        public async Task<IUserEventsCollectionPage> GetEvents()
        {
            IUserEventsCollectionPage events = await _graphServiceClient.Me.Events.Request().GetAsync();
            return events;
        }

        [HttpPost]
        public async Task<Event> CreateEvent([FromBody] Event @event)
        {
            await _graphServiceClient.Me.Events
                .Request()
                .Header("Prefer", "outlook.timezone=\"Pacific Standard Time\"")
                .AddAsync(@event);
            return @event;
        }

        [HttpPatch("{id}")]
        public async Task<Event> UpdateEvent(string id, [FromBody] Event @event)
        {
            await _graphServiceClient.Me.Events[id]
                .Request()
                .UpdateAsync(@event);

            return @event;
        }

        [HttpDelete("{id}")]
        public async Task<string> DeleteEvent(string id)
        {
            await _graphServiceClient.Me.Events[id].Request().DeleteAsync();
            return id;
        }
    }
}