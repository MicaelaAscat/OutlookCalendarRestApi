using CalendarRestApi.Models;
using Microsoft.Graph;

namespace CalendarRestApi.Services
{
    public static class OutlookCalndarEventMapper
    {
        public static Event FromDto(EventDto eventDto)
        {
            var graphEvent = new Event
            {
                Id = eventDto.Id,
                Subject = eventDto.Subject,
                Start = new DateTimeTimeZone
                {
                    DateTime = eventDto.Start.ToString("o"),
                    TimeZone = eventDto.Timezone
                },
                End = new DateTimeTimeZone
                {
                    DateTime = eventDto.End.ToString("o"),
                    TimeZone = eventDto.Timezone
                }
            };

            if(!string.IsNullOrEmpty(eventDto.Body))
            {
                graphEvent.Body = new ItemBody
                {
                    ContentType = BodyType.Text,
                    Content = eventDto.Body
                };
            }
            if(!string.IsNullOrEmpty(eventDto.Attendees))
            {
                var attendees =
                    eventDto.Attendees.Split(';', StringSplitOptions.RemoveEmptyEntries);

                if(attendees.Length > 0)
                {
                    var attendeeList = new List<Attendee>();
                    foreach(var attendee in attendees)
                    {
                        attendeeList.Add(new Attendee
                        {
                            EmailAddress = new EmailAddress
                            {
                                Address = attendee
                            },
                            Type = AttendeeType.Required
                        });
                    }

                    graphEvent.Attendees = attendeeList;
                }
            }
            return graphEvent;
        }

        public static EventDto ToDto(Event graphEvent)
        {
            return new EventDto()
            {
                Id = graphEvent.Id,
                Subject = graphEvent.Subject,
                Start = DateTime.Parse(graphEvent.Start.DateTime),
                End = DateTime.Parse(graphEvent.End.DateTime),
                Timezone = graphEvent.Start.TimeZone,
                Body = graphEvent.Body != null ? graphEvent.Body.Content : null,
                Attendees = graphEvent.Attendees != null ?
                            string.Join(";", graphEvent.Attendees.Select(a => a.EmailAddress != null ? a.EmailAddress.Address : ""))
                            : null
            };
        }
    }
}
