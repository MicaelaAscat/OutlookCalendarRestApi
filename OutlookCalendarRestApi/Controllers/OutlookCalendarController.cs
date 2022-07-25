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
    }
}