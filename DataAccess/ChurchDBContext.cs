using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PIMS.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PIMS.DataAccess
{
    public class ChurchDBContext : DbContext
    {
        public DbSet<Church> Churches { get; set; }
        public DbSet<Administration> Admins { get; set; }
        public DbSet<Ceremony> Ceremonies { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<PriestLeave> PriestLeave { get; set; }
        public DbSet<House>Houses { get; set; }




    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    }

}
        

}