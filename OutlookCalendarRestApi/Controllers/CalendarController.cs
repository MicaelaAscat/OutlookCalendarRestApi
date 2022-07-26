using CalendarRestApi.Models;
using CalendarRestApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;

namespace OutlookCalendarRestApi.Controllers
{
    [ApiController]
    [AuthorizeForScopes(Scopes = new[] { "access_as_user" })]
    [Route("Events")]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;
        private readonly ILogger<CalendarController> _logger;

        public CalendarController(ILogger<CalendarController> logger, ICalendarService calendarService)
        {
            _logger = logger;
            _calendarService = calendarService;
        }

        [HttpGet]
        public async Task<IList<EventDto>> GetEvents()
        {
            var eventDtos = await _calendarService.GetEvents();
            return eventDtos;
        }

        [HttpPost]
        public async Task<EventDto> CreateEvent([FromBody][Bind("Subject,Attendees,Start,End,Body")] EventDto newEvent)
        {
            var eventDto = await _calendarService.CreateEvent(newEvent);
            return eventDto;
        }

        [HttpPatch("{id}")]
        public async Task<EventDto> UpdateEvent(string id, [FromBody] EventDto updatedEvent)
        {
            var eventDto = await _calendarService.UpdateEvent(id, updatedEvent);
            return eventDto;
        }

        [HttpDelete("{id}")]
        public async Task<string> DeleteEvent(string id)
        {
            var deletedEventId = await _calendarService.DeleteEvent(id);
            return deletedEventId;
        }
    }
}