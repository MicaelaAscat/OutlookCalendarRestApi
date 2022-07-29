using CalendarRestApi.Models;
using Microsoft.Graph;
using System.ComponentModel.DataAnnotations;

namespace CalendarRestApi.Services
{
    public class OutlookCalendarService :ICalendarService
    {
        private const int EventsPageSize = 50;

        private readonly GraphServiceClient _graphServiceClient;

        public OutlookCalendarService(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }
        public async Task<EventDto> CreateEvent(EventDto eventDto)
        {
            eventDto.Validate();
            Event graphEvent = OutlookCalendarEventMapper.FromDto(eventDto);
            var newEvent = await _graphServiceClient.Me.Events
                .Request()
                .AddAsync(graphEvent);
            return OutlookCalendarEventMapper.ToDto(newEvent);
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

            return graphEvents.Select(ge => OutlookCalendarEventMapper.ToDto(ge)).ToList();
        }

        public async Task<EventDto> UpdateEvent(string id, EventDto eventDto)
        {
            eventDto.Validate();
            if(!string.IsNullOrEmpty(eventDto.Id) && !string.Equals(id, eventDto.Id))
            {
                throw new ValidationException("Id in the url path should be the same as the id in the event body");
            }

            Event graphEvent = OutlookCalendarEventMapper.FromDto(eventDto);
            await _graphServiceClient.Me.Events[id]
                                        .Request()
                                        .UpdateAsync(graphEvent);

            return OutlookCalendarEventMapper.ToDto(graphEvent); ;
        }
    }
}
