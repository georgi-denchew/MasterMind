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
    public class RemindersController : BaseApiController
    {
        public RemindersController()
            : base(new MasterMindContextFactory())
        {
        }

        public RemindersController(IDbContextFactory<DbContext> contextFactory)
            : base(contextFactory)
        {
        }


        [HttpPost, ActionName("create")]
        public HttpResponseMessage CreateReminder([FromBody] ReminderViewModel reminderModel,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            return this.PerformOperationAndHandleExceptions(() =>
            {
                var context = this.ContextFactory.Create();
                var user = this.LoginUser(sessionKey, context);
                var category = context.Set<Category>().Find(Convert.ToInt32(reminderModel.CategoryId));

                if (category == null)
                {
                    throw new InvalidOperationException("Invalid Category");
                }

                var newReminder = new Reminder()
                {
                    Name = reminderModel.Name,
                    User = user,
                    ToBeCompletedOn = reminderModel.ToBeCompletedOn,
                    Description = reminderModel.Description,
                    ReminderImage = reminderModel.ReminderImage
                };

                var contacts = this.AddOrUpdateContacts(reminderModel.AccociatedContacts, context);

                foreach (var contact in contacts)
                {
                    newReminder.AccociatedContacts.Add(contact);
                }

                category.Reminders.Add(newReminder);
                context.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.Created);
            });
        }
    }
}