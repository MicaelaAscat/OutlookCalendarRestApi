using CalendarRestApi.Models;
using Microsoft.Graph;

namespace CalendarRestApi.Services
{
    public class OutlookCalendarService : ICalendarService
    {
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
            IUserEventsCollectionPage graphEvents = await _graphServiceClient.Me.Events.Request().GetAsync();
            return graphEvents.Select(ge => OutlookCalndarEventMapper.toDto(ge)).ToList();
        }

        public async Task<EventDto> UpdateEvent(string id, EventDto eventDto)
        {
            Event graphEvent = OutlookCalndarEventMapper.fromDto(eventDto);
            await _graphServiceClient.Me.Events[id]
                .Request()
                .UpdateAsync(graphEvent);

            return OutlookCalndarEventMapper.toDto(graphEvent); ;
        }
    }
}
