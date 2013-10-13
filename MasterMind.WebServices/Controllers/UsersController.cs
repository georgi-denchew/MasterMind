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
    public class UsersController : BaseApiController
    {
        private const int SessionKeyLength = 50;

        private readonly Random rand = new Random();
        private const string SessionKeyChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public UsersController()
            : base(new MasterMindContextFactory())
        {
        }

        public UsersController(IDbContextFactory<DbContext> contextFactory)
            : base(contextFactory)
        {
        }

        [HttpPost, ActionName("register")]
        public HttpResponseMessage PostRegisterUser(UserModel model)
        {
            return this.PerformOperationAndHandleExceptions(() =>
            {
                var usernameToLower = model.Username.ToLower();
                var context = this.ContextFactory.Create();
                using (context)
                {
                    var usersDbSet = context.Set<User>();
                    var entity = usersDbSet
                        .FirstOrDefault(usr => usr.Username == usernameToLower);
                    if (entity != null)
                    {
                        string responseMessage = "Username already taken";

                        HttpResponseMessage errResponse = this.Request.CreateErrorResponse(HttpStatusCode.Conflict, responseMessage);
                        throw new HttpResponseException(errResponse);
                    }
                    entity = new User()
                    {
                        Username = model.Username.ToLower(),
                        AuthenticationCode = model.AuthCode
                    };
                    usersDbSet.Add(entity);
                    context.SaveChanges();
                    return this.PostLoginUser(model);
                }
            });
        }

        [HttpPost, ActionName("login")]
        public HttpResponseMessage PostLoginUser(UserModel model)
        {
            return this.PerformOperationAndHandleExceptions(() =>
            {
                var context = this.ContextFactory.Create();
                using (context)
                {
                    var usernameToLower = model.Username.ToLower();
                    var usersDbSet = context.Set<User>();
                    var entity = usersDbSet.SingleOrDefault(usr => usr.Username == usernameToLower && usr.AuthenticationCode == model.AuthCode);
                    if (entity == null)
                    {
                        var errResponse = this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid username or password");
                        throw new HttpResponseException(errResponse);
                    }

                    entity.SessionKey = this.GenerateSessionKey(entity.Id);

                    context.SaveChanges();
                    var responseModel = new LoginResponseModel()
                    {
                        Nickname = entity.Username,
                        SessionKey = entity.SessionKey
                    };

                    var response = this.Request.CreateResponse(HttpStatusCode.Accepted, responseModel);
                    return response;
                }
            });
        }

        [HttpGet, ActionName("logout")]
        public HttpResponseMessage LogoutUser([ValueProvider(typeof(HeaderValueProviderFactory<string>))] string sessionKey)
        {
            return this.PerformOperationAndHandleExceptions(() =>
                {
                    var context = this.ContextFactory.Create();
                    var user = this.LoginUser(sessionKey, context);

                    user.SessionKey = null;
                    context.SaveChanges();

                    return new HttpResponseMessage(HttpStatusCode.OK);
                });
        }

        private string GenerateSessionKey(int userId)
        {
            StringBuilder keyBuilder = new StringBuilder(50);
            keyBuilder.Append(userId);
            while (keyBuilder.Length < SessionKeyLength)
            {
                var index = rand.Next(SessionKeyChars.Length);
                keyBuilder.Append(SessionKeyChars[index]);
            }
            return keyBuilder.ToString();
        }
    }
}