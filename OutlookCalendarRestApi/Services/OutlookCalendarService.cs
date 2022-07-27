using CalendarRestApi.Models;
using Microsoft.Graph;

namespace CalendarRestApi.Services
{
    public class OutlookCalendarService : ICalendarService
    {
        private const int EventsPageSize = 100;

        private readonly GraphServiceClient _graphServiceClient;

        public OutlookCalendarService(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }
        public async Task<EventDto> CreateEvent(EventDto eventDto)
        {
            Event graphEvent = OutlookCalndarEventMapper.fromDto(eventDto);
            var newEvent = await _graphServiceClient.Me.Events
                .Request()
                .AddAsync(graphEvent);
            return OutlookCalndarEventMapper.toDto(newEvent);
        }

        public async Task<string> DeleteEvent(string id)
        {
            await _graphServiceClient.Me.Events[id].Request().DeleteAsync();
            return id;
        }

        public async Task<IList<EventDto>> GetEvents()
        {
            IUserEventsCollectionPage graphEvents = await _graphServiceClient.Me.Events
                                                                                .Request()
                                                                                .Header("Prefer", "outlook.timezone=\"UTC\"")
                                                                                .Select("subject,body,attendees,start,end")
                                                                                .OrderBy("start/dateTime")
                                                                                .Top(EventsPageSize).GetAsync();

            IList<Event> allEvents;
            // Handle case where there are more than EventsPageSize
            if (graphEvents.NextPageRequest != null)
            {
                allEvents = new List<Event>();
                // Create a page iterator to iterate over subsequent pages
                var pageIterator = PageIterator<Event>.CreatePageIterator(
                    _graphServiceClient, graphEvents,
                    (e) =>
                    {
                        allEvents.Add(e);
                        return true;
                    }
                );
                await pageIterator.IterateAsync();
            }
            else
            {
                // If only one page, just use the result
                allEvents = graphEvents.CurrentPage;
            }
            return allEvents.Select(ge => OutlookCalndarEventMapper.toDto(ge)).ToList();
        }

        public async Task<EventDto> UpdateEvent(string id, EventDto eventDto)
        {
            if (!string.IsNullOrEmpty(eventDto.Id) && !string.Equals(id, eventDto.Id))
            {
                throw new ServiceException(new Error() { Message = "No se puede modificar el id del evento original." });
            }

            Event graphEvent = OutlookCalndarEventMapper.fromDto(eventDto);
            await _graphServiceClient.Me.Events[id]
                .Request()
                .UpdateAsync(graphEvent);

            return OutlookCalndarEventMapper.toDto(graphEvent); ;
        }
    }
}
