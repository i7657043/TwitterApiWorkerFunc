using System;

namespace TOFunction.Models
{
    public class CustomTweetDto
    {
        public string Category { get; set; }
        public string Location { get; set; }
        public DateTime Deadline { get; set; }
        public ScheduleTime ScheduleTime { get; set; }
    }
}
