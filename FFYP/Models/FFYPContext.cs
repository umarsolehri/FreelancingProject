using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace FFYP.Models
{
    public class FFYPContext:IdentityDbContext<ApplicationUser>
    {
        public FFYPContext() : base("name=FFYPContext", throwIfV1Schema: false) { }

        public DbSet<Project> Project { get; set; }
        public DbSet<SiteUser> SiteUser { get; set; }
        public DbSet<Job> Job { get; set; }
        public DbSet<Biding> Biding { get; set; }
        public DbSet<BidsList> BidsList { get; set; }
        public DbSet<JobApproved> JobApproved { get; set; }
        public DbSet<FavPro> FavPro { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Configure primary key for pro status from project
            modelBuilder.Entity<Biding>()
                .HasKey(p => p.BidingID)
                .HasRequired(p => p.SiteUser)
                .WithMany(p=>p.Biding)
                .HasForeignKey(p=>p.SiteUserID);
            modelBuilder.Entity<Biding>()
                .HasKey(p => p.BidingID)
                .HasRequired(p => p.Project)
                .WithMany(p => p.Biding)
                .HasForeignKey(p => p.ProjectID).WillCascadeOnDelete(false);
            




        }

        public static FFYPContext Create()
        {
            return new FFYPContext();
        }
    }
}