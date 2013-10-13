using MasterMind.WebServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterMind.WebServices.Controllers
{
    public class CategoryEventsModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<EventViewModel> Events { get; set; }
    }
}
