using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MasterMind.WebServices.Models
{
    [DataContract]
    public class AccociatedContactViewModel
    {
        [DataMember(Name="displayName")]
        public string DisplayName { get; set; }

        [DataMember(Name = "phoneNumber")]
        public int? PhoneNumber { get; set; }
    }
}
