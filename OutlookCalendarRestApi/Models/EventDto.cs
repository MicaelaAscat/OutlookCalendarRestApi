using System.ComponentModel.DataAnnotations;

namespace CalendarRestApi.Models
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
    }
}
