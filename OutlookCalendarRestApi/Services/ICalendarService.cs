using CalendarRestApi.Models;

namespace CalendarRestApi.Services
{
    public interface ICalendarService
    {
        Task<IList<EventDto>> GetEvents();
        Task<EventDto> CreateEvent(EventDto eventDto);
        Task<EventDto> UpdateEvent(string id, EventDto eventDto);
        Task<string> DeleteEvent(string id);
    }
}
