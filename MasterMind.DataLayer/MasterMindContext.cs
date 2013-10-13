using MasterMind.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterMind.DataLayer
{
    public class MasterMindContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AccociatedContact> AccociatedContacts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Reminder> Reminders { get; set; }

        public MasterMindContext()
            : base ("MasterMindDb")
        {

        }
    }
}
