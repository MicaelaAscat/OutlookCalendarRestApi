using CodaCalendarRestApi.Models;
using CodaCalendarRestApi.Services;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CalendarRestApiTests
{
    public class OutlookCalendarEventMapperTest
    {
        [Fact]
        public void ToDtoMapping()
        {
            var graphEvent = new Event()
            {
                Id = "123456",
                Subject = "Meeting",
                Start = new DateTimeTimeZone
                {
                    DateTime = DateTime.Now.ToString("o"),
                    TimeZone = "UTC"
                },
                End = new DateTimeTimeZone
                {
                    DateTime = DateTime.Now.ToString("o"),
                    TimeZone = "UTC"
                },
                Body = new ItemBody
                {
                    ContentType = BodyType.Text,
                    Content = "<html><h1>Coda test</h1></html>"
                },
                Attendees = new List<Attendee>() {
                new Attendee
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = "hola@hotmail.com"
                    },
                    Type = AttendeeType.Required
                },
                new Attendee
                {
                    EmailAddress = new EmailAddress
                    {
                        Address = "chau@hotmail.com"
                    },
                    Type = AttendeeType.Required
                } }
            };

            var eventDto = OutlookCalendarEventMapper.ToDto(graphEvent);

            Assert.Equal("123456", eventDto.Id);
            Assert.Equal("Meeting", eventDto.Subject);
            Assert.Equal(DateTime.Parse(graphEvent.Start.DateTime), eventDto.Start);
            Assert.Equal("UTC", eventDto.Timezone);
            Assert.Equal(DateTime.Parse(graphEvent.End.DateTime), eventDto.End);
            Assert.Equal("UTC", eventDto.Timezone);
            Assert.Equal("<html><h1>Coda test</h1></html>", eventDto.Body);
            Assert.Equal("hola@hotmail.com;chau@hotmail.com", eventDto.Attendees);
        }

        [Fact]
        public void FromDtoMapping()
        {
            var eventDto = new EventDto()
            {
                Id = "123456",
                Subject = "Meeting",
                Start = DateTime.Now,
                End = DateTime.Now,
                Timezone = "UTC",
                Body = "<html><h1>Coda test</h1></html>",
                Attendees = "hola@hotmail.com;chau@hotmail.com"
            };

            var graphEvent = OutlookCalendarEventMapper.FromDto(eventDto);

            Assert.Equal("123456", graphEvent.Id);
            Assert.Equal("Meeting", graphEvent.Subject);
            Assert.Equal(eventDto.Start.ToString("o"), graphEvent.Start.DateTime);
            Assert.Equal("UTC", graphEvent.Start.TimeZone);
            Assert.Equal(eventDto.End.ToString("o"), graphEvent.End.DateTime);
            Assert.Equal("UTC", graphEvent.End.TimeZone);
            Assert.Equal("<html><h1>Coda test</h1></html>", graphEvent.Body.Content);
            Assert.Equal(2, graphEvent.Attendees.Count());
        }
    }
}