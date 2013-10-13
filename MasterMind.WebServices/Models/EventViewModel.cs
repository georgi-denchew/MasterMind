using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MasterMind.WebServices.Models
{
    [DataContract]
    public class EventViewModel
    {

        [DataMember(Name="id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "latitude")]
        public double? Latitude { get; set; }

        [DataMember(Name = "longitude")]
        public double? Longitude { get; set; }

        [DataMember(Name = "categoryId")]
        public string CategoryId { get; set; }

        [DataMember(Name = "categoryName")]
        public string CategoryName { get; set; }

        [DataMember(Name = "startDate")]
        public DateTime StartDate { get; set; }

        [DataMember(Name="duration")]
        public DateTime Duration { get; set; }

        [DataMember(Name="contacts")]
        public IEnumerable<AccociatedContactViewModel> AccociatedContacts { get; set; }
    }
}