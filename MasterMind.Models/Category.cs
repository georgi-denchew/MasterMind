using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterMind.Models
{
    public class Category
    {
        public Category()
        {
            this.Reminders = new HashSet<Reminder>();
            this.Events = new HashSet<Event>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Reminder> Reminders { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
