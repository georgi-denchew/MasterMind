using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MasterMind.WebServices.Models
{
    [DataContract]
    public class ReminderViewModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "toBeCompletedOn")]
        public DateTime ToBeCompletedOn { get; set; }

        [DataMember(Name = "categoryId")]
        public int CategoryId { get; set; }

        [DataMember(Name = "reminderImage")]
        public byte[] ReminderImage { get; set; }

        [DataMember(Name = "contacts")]
        public IEnumerable<AccociatedContactViewModel> AccociatedContacts { get; set; }
    }
}