using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterMind.Models
{
    public class Event
    {
        public Event()
        {
            this.AccociatedContacts = new HashSet<AccociatedContact>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime Duration { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<AccociatedContact> AccociatedContacts { get; set; }
    }
}