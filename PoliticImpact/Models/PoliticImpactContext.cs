using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace PoliticImpact.Models
{
    public class PoliticImpactContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, add the following
        // code to the Application_Start method in your Global.asax file.
        // Note: this will destroy and re-create your database with every model change.
        // 
        // System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<PoliticImpact.Models.PoliticImpactContext>());

        public DbSet<PoliticImpact.Models.CaseItem> CaseItems { get; set; }

        public PoliticImpactContext()
        { 
           System.Data.Entity.Database.SetInitializer(
             new System.Data.Entity.DropCreateDatabaseIfModelChanges<PoliticImpact.Models.PoliticImpactContext>());
           System.Data.Entity.Database.DefaultConnectionFactory = new SqlCeConnectionFactory("System.Data.SqlServerCe.4.0");
        }

        public DbSet<PoliticImpact.Models.CaseSignUp> CaseSignUps { get; set; }

        public DbSet<PoliticImpact.Models.CaseCategory> CaseCategories { get; set; }

        public DbSet<PoliticImpact.Models.CaseVoting> CaseVotings { get; set; }

        public DbSet<PoliticImpact.Models.CaseVote> CaseVotes { get; set; }

        public DbSet<PoliticImpact.Models.CaseLike> CaseLikes { get; set; }

        public DbSet<PoliticImpact.Models.CaseComment> CaseComments { get; set; }

        public DbSet<PoliticImpact.Models.RecieverResponse> RecieverResponses { get; set; }

        public DbSet<PoliticImpact.Models.CaseImage> CaseImages { get; set; }

        public DbSet<PoliticImpact.Models.UserGroup> UserGroups { get; set; }
    }
}