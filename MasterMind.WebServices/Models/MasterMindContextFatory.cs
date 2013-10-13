using MasterMind.DataLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace MasterMind.WebServices.Models
{
    public class MasterMindContextFactory : IDbContextFactory<DbContext>
    {
        public DbContext Create()
        {
            return new MasterMindContext();
        }
    }
}