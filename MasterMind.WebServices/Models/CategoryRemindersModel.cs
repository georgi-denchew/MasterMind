using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MasterMind.WebServices.Models
{
    [DataContract]
    public class CategoryRemindersModel
    {
        [DataMember(Name="id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "reminders")]
        public IEnumerable<ReminderViewModel> Reminders { get; set; }
    }
}