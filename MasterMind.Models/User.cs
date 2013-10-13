using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMind.Models
{

    // EVENTS - USE GEOLOCATION |||| REMINDERS - USE CONTACTS
    public class User
    {
        public User()
        {
            this.Reminders = new HashSet<Reminder>();
            this.Events = new HashSet<Event>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string AuthenticationCode { get; set; }

        public string SessionKey { get; set; }

        public virtual ICollection<Reminder> Reminders { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}
