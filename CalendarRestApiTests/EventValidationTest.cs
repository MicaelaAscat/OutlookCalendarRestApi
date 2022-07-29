using CalendarRestApi.Models;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace CalendarRestApiTests
{
    public class EventValidationTest
    {
        [Fact]
        public void DateRangeValidationEventDto()
        {
            var eventDto = new EventDto()
            {
                Subject = "Test",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(-1),
                Timezone = "UTC"
            };
            Assert.Throws<ValidationException>(() => eventDto.Validate());
        }

        [Fact]
        public void TimezoneValidationEventDto()
        {
            var eventDto = new EventDto()
            {
                Subject = "Test",
                Start = DateTime.Now,
                End = DateTime.Now,
                Timezone = "AAA"
            };
            Assert.Throws<ValidationException>(() => eventDto.Validate());
        }
        [Fact]
        public void TimezoneRequiredValidationEventDto()
        {
            var eventDto = new EventDto()
            {
                Subject = "Test",
                Start = DateTime.Now,
                End = DateTime.Now
            };
            Assert.Throws<ValidationException>(() => eventDto.Validate());
        }
    }
}