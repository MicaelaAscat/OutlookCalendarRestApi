using System;
using System.ComponentModel.DataAnnotations;

namespace CodaCalendarRestApi.Models
{
    public class EventDto
    {
        public string? Id { get; set; }
        [Required]
        public string? Subject { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        [Required]
        public string? Timezone { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Body { get; set; }
        [RegularExpression(@"((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([;])*)*",
          ErrorMessage = "Please enter one or more email addresses separated by a semi-colon (;)")]
        public string? Attendees { get; set; }

        public void Validate()
        {
            if(Start > End)
            {
                throw new ValidationException("Event start date should be before than event end date");
            }
            try
            {
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(Timezone);
            }
            catch(Exception ex)
            {
                throw new ValidationException(string.Format("Invalid timezone: {0}", ex.Message));
            }
        }
    }
}
