using MasterMind.Models;
using MasterMind.WebServices.Headers;
using MasterMind.WebServices.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.ValueProviders;

namespace MasterMind.WebServices.Controllers
{
    public class EventsController : BaseApiController
    {
        public EventsController()
            : base(new MasterMindContextFactory())
        {
        }

        public EventsController(IDbContextFactory<DbContext> contextFactory)
            : base(contextFactory)
        {
        }

        [HttpPost, ActionName("create")]
        public HttpResponseMessage CreateEvent([FromBody] EventViewModel eventModel,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            return this.PerformOperationAndHandleExceptions(() =>
                {
                    var context = this.ContextFactory.Create();
                    var user = this.LoginUser(sessionKey, context);
                    var category = context.Set<Category>().Find(Convert.ToInt32(eventModel.CategoryId));

                    if (category == null)
                    {
                        throw new InvalidOperationException("Invalid Category");
                    }

                    var newEvent = new Event()
                    {
                        Name = eventModel.Name,
                        Longitude = eventModel.Longitude,
                        Latitude = eventModel.Latitude,
                        User = user,
                        StartTime = eventModel.StartDate,
                        Description = eventModel.Description,
                        Duration = eventModel.StartDate.AddHours(eventModel.Duration.Hour).AddMinutes(eventModel.Duration.Minute)
                    };

                    var contacts = this.AddOrUpdateContacts(eventModel.AccociatedContacts, context);

                    foreach (var contact in contacts)
                    {
                        newEvent.AccociatedContacts.Add(contact);
                    }

                    category.Events.Add(newEvent);
                    context.SaveChanges();
                    
                    return this.Request.CreateResponse(HttpStatusCode.Created);
                });
        }

        //private HashSet<AccociatedContact> AddOrUpdateContacts(IEnumerable<AccociatedContactViewModel> contactModels, DbContext context)
        //{
        //    var result = new HashSet<AccociatedContact>();

        //    foreach (var model in contactModels)
        //    {
        //        var existing = context.Set<AccociatedContact>()
        //            .FirstOrDefault(ac => ac.DisplayName == model.DisplayName &&
        //                ac.PhoneNumber == model.PhoneNumber);

        //        if (existing == null)
        //        {
        //            existing = new AccociatedContact()
        //            {
        //                DisplayName = model.DisplayName,
        //                PhoneNumber = model.PhoneNumber
        //            };

        //            context.Set<AccociatedContact>().Add(existing);
        //        }

        //        result.Add(existing);
        //    }

        //    return result;
        //}

        [HttpGet]
        public IQueryable GetByCategory(int categoryId,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            return this.PerformOperationAndHandleExceptions(() =>
                {
                    var context = this.ContextFactory.Create();
                    var user = this.LoginUser(sessionKey, context);

                    var events = context.Set<Event>()
                        .Where(e => e.Category.Id == categoryId && e.UserId == user.Id)
                        .Select(ev => new EventViewModel
                        {
                            Id = ev.Id,
                            CategoryName = ev.Category.Name,
                            Latitude = ev.Latitude,
                            Longitude = ev.Longitude,
                            Name = ev.Name,
                            Description = ev.Description,
                            AccociatedContacts = 
                                                from ac in ev.AccociatedContacts
                                                select new AccociatedContactViewModel 
                                                {
                                                    DisplayName = ac.DisplayName,
                                                    PhoneNumber = ac.PhoneNumber
                                                }
                        });

                    return events;
                });
        }
    }
}