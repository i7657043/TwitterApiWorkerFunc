using System;

namespace TOFunction
{
    public class Tweet
    {
        public string Category { get; set; }
        public string Location { get; set; }
        public DateTime Deadline { get; set; }
        public ScheduleTime ScheduleTime { get; set; }
    }
}
