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
            try
            {
                _logger.LogInformation("call GetEvents");
                var eventDtos = await _calendarService.GetEvents();
                return eventDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("ERROR GetEvents: {0}", ex.Message));
                throw;
            }
        }

        [HttpPost]
        public async Task<EventDto> CreateEvent([FromBody] EventDto newEvent)
        {
            try
            {
                _logger.LogInformation("call CreateEvent");
                var eventDto = await _calendarService.CreateEvent(newEvent);
                return eventDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("ERROR CreateEvent: {0}", ex.Message));
                throw;
            }
        }

        [HttpPatch("{id}")]
        public async Task<EventDto> UpdateEvent(string id, [FromBody] EventDto updatedEvent)
        {
            try
            {
                _logger.LogInformation("call UpdateEvent {ID}", id);
                var eventDto = await _calendarService.UpdateEvent(id, updatedEvent);
                return eventDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("ERROR UpdateEvent: {0}", ex.Message));
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<string> DeleteEvent(string id)
        {
            try
            {
                _logger.LogInformation("call DeleteEvent {ID}", id);
                var deletedEventId = await _calendarService.DeleteEvent(id);
                return deletedEventId;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("ERROR DeleteEvent: {0}", ex.Message));
                throw;
            }
        }
    }
}