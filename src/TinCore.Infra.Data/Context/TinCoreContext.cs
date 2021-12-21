using TinCore.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace TinCore.Infra.Data.Context
{
    public class TinCoreContext : DbContext
    {
        private readonly IHostingEnvironment _env;

        public TinCoreContext(IHostingEnvironment env)
        {
            _env = env;
        }

        #region Location entities
        public DbSet<VLocation> VLocation { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Neighbourhood> Neighbourhood { get; set; }
        public DbSet<LocationProgram> LocationProgram { get; set; }

        #endregion Location entities

        #region entity related entities
        public DbSet<TinEntity> Entity { get; set; }
        public DbSet<TinContact> TinContact { get; set; }
        public DbSet<Subscription> Subscription { get; set; }
        public DbSet<TinAttribute> TinAttribute { get; set; }
        public DbSet<SubscriptionTemplate> SubscriptionTemplate { get; set; }
        public DbSet<EntitySubscriptionTracking> EntitySubscriptionTracking { get; set; }

        #endregion entity related entities


        #region Relation entities
        public DbSet<VRelation> VRelation { get; set; }
        public DbSet<LocationCategoryRelation> LocationCategoryRelation { get; set; }
        public DbSet<RelationEntityRelation> RelationEntityRelation { get; set; }
        public DbSet<LocationEntityRelation> LocationEntityRelation { get; set; }
        #endregion Relation entities

        #region classification entities
        public DbSet<VClassification> VClassification { get; set; }
        public DbSet<Classification> Classification { get; set; }
        public DbSet<ClassificationType> ClassificationType { get; set; }
        #endregion classification entities

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new GeoCityMap());
                        
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // get the configuration from the app settings
            var config = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();


            // define the database to use
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }
    }
}
