using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace TOFunction.Models
{
    public class CustomTweetTableEntity : TableEntity
    {        
        public CustomTweetTableEntity(string category, string location, DateTime deadline, ScheduleTime scheduleTime)
        {
            Category = category;
            Location = location;
            Deadline = deadline;
            ScheduleTime = scheduleTime.ToString();

            PartitionKey = $"{category}_{location}_{scheduleTime.ToString()}";
            RowKey = Guid.NewGuid().ToString();
        }
        
        public string Category { get; set; }
        public string Location { get; set; }
        public DateTime Deadline { get; set; }
        public string ScheduleTime { get; set; }
    }
}
