using CalendarRestApi.Models;
using Microsoft.Graph;

namespace CalendarRestApi.Services
{
    public class OutlookCalendarService : ICalendarService
    {
        private const int EventsPageSize = 50;

        private readonly GraphServiceClient _graphServiceClient;

        public OutlookCalendarService(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }
        public async Task<EventDto> CreateEvent(EventDto eventDto)
        {
            Event graphEvent = OutlookCalndarEventMapper.FromDto(eventDto);
            var newEvent = await _graphServiceClient.Me.Events
                .Request()
                .AddAsync(graphEvent);
            return OutlookCalndarEventMapper.ToDto(newEvent);
        }

        public async Task<string> DeleteEvent(string id)
        {
            await _graphServiceClient.Me.Events[id].Request().DeleteAsync();
            return id;
        }

        public async Task<IList<EventDto>> GetEvents()
        {   //just returning the first page for this poc
            IUserEventsCollectionPage graphEvents = await _graphServiceClient.Me.Events
                                                                                .Request()
                                                                                .Header("Prefer", "outlook.timezone=\"UTC\"")
                                                                                .Select("subject,body,attendees,start,end")
                                                                                .OrderBy("start/dateTime desc")
                                                                                .Top(EventsPageSize).GetAsync();

            return graphEvents.Select(ge => OutlookCalndarEventMapper.ToDto(ge)).ToList();
        }

        public async Task<EventDto> UpdateEvent(string id, EventDto eventDto)
        {
            if(!string.IsNullOrEmpty(eventDto.Id) && !string.Equals(id, eventDto.Id))
            {
                throw new ServiceException(new Error() { Message = "Can't change event id!" });
            }

            Event graphEvent = OutlookCalndarEventMapper.FromDto(eventDto);
            await _graphServiceClient.Me.Events[id]
                                        .Request()
                                        .UpdateAsync(graphEvent);

            return OutlookCalndarEventMapper.ToDto(graphEvent); ;
        }
    }
}
