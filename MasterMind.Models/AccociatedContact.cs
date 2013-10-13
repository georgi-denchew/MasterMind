using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMind.Models
{
    public class AccociatedContact
    {
        public AccociatedContact()
        {
            this.Events = new HashSet<Event>();
            this.Reminders = new HashSet<Reminder>();
        }

        public int Id { get; set; }

        public string DisplayName { get; set; }

        public int? PhoneNumber { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<Reminder> Reminders { get; set; }
    }
}