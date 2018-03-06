using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using PIMS.DataAccess;
using PIMS.Entities;
using System.Collections.Generic;

namespace PIMS.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<ChurchDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
       
        }
        protected override void Seed(ChurchDBContext context)
        {
           
        }
    }
}
