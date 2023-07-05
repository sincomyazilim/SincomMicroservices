using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.Messages.PublisherEvent//190
{
    public class CourseNameChangedEvent
    {
        
        public string CourseId { get; set; }
        public string UpdateName { get; set; }
    }
}
