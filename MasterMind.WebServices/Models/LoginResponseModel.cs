using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MasterMind.WebServices.Models
{
    [DataContract]
    public class LoginResponseModel
    {
        [DataMember(Name = "sessionKey")]
        public string SessionKey { get; set; }

        [DataMember(Name = "displayName")]
        public string Nickname { get; set; }
    }
}