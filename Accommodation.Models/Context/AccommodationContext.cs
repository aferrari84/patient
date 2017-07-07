using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Accommodation.Models.Interfaces;
using Accommodation.Models.Maps;
using Accommodation.Models;

namespace Accommodation.Models.Context
{
    public class AccommodationContext : IdentityDbContext<User>, IAccommodationContext
    {
        private DbContextTransaction dbContextTransaction;

        static AccommodationContext()
        {
        }

        public AccommodationContext()
            : base("AccommodationContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AccommodationContext, Accommodation.Models.Migrations.Configuration>());
        }

        #region IAccommodationContext Properties Implementation
        public virtual IDbSet<Lodging> Lodgings { get; set; }
        public virtual IDbSet<State> States { get; set; }
        public virtual IDbSet<Location> Locations { get; set; }
        public virtual IDbSet<Service> Services { get; set; }
        public virtual IDbSet<Tourism> Tourisms { get; set; }
        public virtual IDbSet<Advertising> Advertisings { get; set; }
        public virtual IDbSet<Photo> Photos { get; set; }
        public virtual IDbSet<AccommodationType> AccommodationTypes { get; set; }
        public virtual IDbSet<ServiceType> ServiceTypes { get; set; }
        public virtual IDbSet<PublishType> PublishTypes { get; set; }


        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            try
            {
                modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

                #region Adding Mapping class to Model Builder
                modelBuilder.Configurations.Add(new UserMap());
               
                #endregion

                base.OnModelCreating(modelBuilder);
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        #region IAccommodationContext Methods Implementation
        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public new System.Data.Entity.Infrastructure.DbEntityEntry Entry<T>(T entity) where T : class
        {
            return base.Entry<T>(entity);
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return Database.ExecuteSqlCommand(sql, parameters);
        }

        public void DetectChanges()
        {
            ChangeTracker.DetectChanges();
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        public void SetUnchanged(object entity)
        {
            Entry(entity).State = EntityState.Unchanged;
        }

        public System.Data.Entity.Infrastructure.DbEntityEntry UpdateEntries(object dbEntity, object entity)
        {
            var entry = Entry(dbEntity);
            entry.OriginalValues.SetValues(dbEntity);
            entry.CurrentValues.SetValues(entity);
            return entry;
        }


        #endregion


        public void BeginTransaction()
        {
            dbContextTransaction = Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (dbContextTransaction != null)
            {
                dbContextTransaction.Commit();
            }
        }

        public void RollbackTransaction()
        {
            if (dbContextTransaction != null)
            {
                dbContextTransaction.Rollback();
            }
        }

        public void DisposeTransaction()
        {
            if (dbContextTransaction != null)
            {
                dbContextTransaction.Dispose();
            }
        }


        public bool EnableCreationProxy
        {
            set { base.Configuration.ProxyCreationEnabled = value; }
        }
    }

}
