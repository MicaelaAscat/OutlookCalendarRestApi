using CodaCalendarRestApi.Models;
using CodaCalendarRestApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CodaCalendarRestApi.Controllers
{
    [Authorize]
    [ApiController]
    [AuthorizeForScopes(Scopes = new[] { "access_as_user" })]
    [Route("[controller]")]
    public class CalendarController :ControllerBase
    {
        private readonly ICalendarService _calendarService;
        private readonly ILogger<CalendarController> _logger;

        public CalendarController(ILogger<CalendarController> logger, ICalendarService calendarService)
        {
            _logger = logger;
            _calendarService = calendarService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<EventDto>>> GetEvents()
        {
            try
            {
                _logger.LogInformation("call GetEvents");
                var eventDtos = await _calendarService.GetEvents();
                return Ok(eventDtos);
            }
            catch(Exception ex)
            {
                var errorMessage = string.Format("ERROR GetEvents: {0}", ex.Message);
                _logger.LogError(errorMessage);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<EventDto>> CreateEvent([FromBody] EventDto newEvent)
        {
            try
            {
                _logger.LogInformation("call CreateEvent");
                var eventDto = await _calendarService.CreateEvent(newEvent);
                return Ok(eventDto);
            }
            catch(ValidationException ex)
            {
                var errorMessage = string.Format("ERROR CreateEvent: {0}", ex.Message);
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }
            catch(Exception ex)
            {
                var errorMessage = string.Format("ERROR CreateEvent: {0}", ex.Message);
                _logger.LogError(errorMessage);
                throw;
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<EventDto>> UpdateEvent(string id, [FromBody] EventDto updatedEvent)
        {
            try
            {
                _logger.LogInformation("call UpdateEvent {ID}", id);
                var eventDto = await _calendarService.UpdateEvent(id, updatedEvent);
                return Ok(eventDto);
            }
            catch(ValidationException ex)
            {
                var errorMessage = string.Format("ERROR UpdateEvent: {0}", ex.Message);
                _logger.LogError(errorMessage);
                return BadRequest(errorMessage);
            }
            catch(Exception ex)
            {
                var errorMessage = string.Format("ERROR UpdateEvent: {0}", ex.Message);
                _logger.LogError(errorMessage);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteEvent(string id)
        {
            try
            {
                _logger.LogInformation("call DeleteEvent {ID}", id);
                var deletedEventId = await _calendarService.DeleteEvent(id);
                return Ok(deletedEventId);
            }
            catch(Exception ex)
            {
                var errorMessage = string.Format("ERROR DeleteEvent: {0}", ex.Message);
                _logger.LogError(errorMessage);
                throw;
            }
        }
    }
}
