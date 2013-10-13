using MasterMind.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using MasterMind.WebServices.Models;

namespace MasterMind.WebServices.Controllers
{
    public class BaseApiController : ApiController
    {
        private IDbContextFactory<DbContext> contextFactory;

        protected IDbContextFactory<DbContext> ContextFactory
        {
            get
            {
                return this.contextFactory;
            }
        }

        public BaseApiController(IDbContextFactory<DbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        protected T PerformOperationAndHandleExceptions<T>(Func<T> operation, HttpStatusCode errStatusCode = HttpStatusCode.BadRequest, string errMessage = null)
        {
            try
            {
                return operation();
            }
            catch (Exception ex)
            {
                var errResponse = 
                    this.Request
                        .CreateErrorResponse(HttpStatusCode.BadRequest,
                            (errMessage != null) ? errMessage : ex.Message);
                throw new HttpResponseException(errResponse);
            }
        }

        protected User LoginUser(string sessionKey, DbContext context)
        {
            var user = context.Set<User>().FirstOrDefault(u => u.SessionKey == sessionKey);
            if (user == null)
            {
                throw new InvalidOperationException("User is not authenticated");
            }
            return user;
        }

        protected HashSet<AccociatedContact> AddOrUpdateContacts(IEnumerable<AccociatedContactViewModel> contactModels, DbContext context)
        {
            var result = new HashSet<AccociatedContact>();

            foreach (var model in contactModels)
            {
                var existing = context.Set<AccociatedContact>()
                    .FirstOrDefault(ac => ac.DisplayName == model.DisplayName &&
                        ac.PhoneNumber == model.PhoneNumber);

                if (existing == null)
                {
                    existing = new AccociatedContact()
                    {
                        DisplayName = model.DisplayName,
                        PhoneNumber = model.PhoneNumber
                    };

                    context.Set<AccociatedContact>().Add(existing);
                }

                result.Add(existing);
            }

            return result;
        }
    }
}