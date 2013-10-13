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
    public class CategoriesController : BaseApiController
    {
        public CategoriesController()
            : base(new MasterMindContextFactory())
        {
        }

        public CategoriesController(IDbContextFactory<DbContext> contextFactory)
            : base(contextFactory)
        {
        }

        [HttpGet, ActionName("all")]
        public IQueryable<CategoryViewModel> GetAll()
        {
            var response = this.PerformOperationAndHandleExceptions(() =>
                {
                    var context = this.ContextFactory.Create();

                    var categories = context.Set<Category>().Select(c =>
                        new CategoryViewModel
                        {
                            Id = c.Id,
                            Name = c.Name
                        });
                    categories = categories.OrderBy(c => c.Name);

                    return categories;
                });

            return response;
        }

        [HttpGet, ActionName("events")]
        public CategoryEventsModel GetEventsById(int id,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            return this.PerformOperationAndHandleExceptions(() =>
                {
                    var context = this.ContextFactory.Create();
                    var user = context.Set<User>().FirstOrDefault(u => u.SessionKey == sessionKey);

                    if (user == null)
                    {
                        throw new InvalidOperationException("Invalid user");
                    }

                    var category = context.Set<Category>().Include("Events").FirstOrDefault(c => c.Id == id);

                    if (category == null)
                    {
                        throw new InvalidOperationException("Invalid category");
                    }



                    var result = new CategoryEventsModel()
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Events =
                                from ev in category.Events
                                where ev.UserId == user.Id
                                select new EventViewModel
                                {
                                    Id = ev.Id,
                                    CategoryName = ev.Category.Name,
                                    Description = ev.Description,
                                    Duration = ev.Duration,
                                    StartDate = ev.StartTime,
                                    Latitude = ev.Latitude,
                                    Longitude = ev.Longitude,
                                    Name = ev.Name,
                                    AccociatedContacts =
                                                        from ac in ev.AccociatedContacts
                                                        select new AccociatedContactViewModel
                                                        {
                                                            DisplayName = ac.DisplayName,
                                                            PhoneNumber = ac.PhoneNumber
                                                        }
                                }
                    };

                    return result;
                });
        }


        [HttpGet, ActionName("reminders")]
        public CategoryRemindersModel GetRemindersById(int id,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            return this.PerformOperationAndHandleExceptions(() =>
            {
                var context = this.ContextFactory.Create();
                var user = context.Set<User>().FirstOrDefault(u => u.SessionKey == sessionKey);

                if (user == null)
                {
                    throw new InvalidOperationException("Invalid user");
                }

                var category = context.Set<Category>().Include("Events").FirstOrDefault(c => c.Id == id);

                if (category == null)
                {
                    throw new InvalidOperationException("Invalid category");
                }

                var result = new CategoryRemindersModel()
                {
                    Id = category.Id,
                    Name = category.Name,
                    Reminders = 
                                from r in category.Reminders
                                where r.UserId == user.Id
                                select new ReminderViewModel
                                {
                                    Name = r.Name,
                                    Description = r.Description,
                                    ReminderImage = r.ReminderImage,
                                    ToBeCompletedOn = r.ToBeCompletedOn,
                                    Id = r.Id,
                                    AccociatedContacts = 
                                                        from ac in r.AccociatedContacts
                                                        select new AccociatedContactViewModel
                                                        {
                                                            DisplayName = ac.DisplayName,
                                                            PhoneNumber = ac.PhoneNumber
                                                        }
                                }
                };

                return result;
            });
        }
    }
}