using Accommodation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Models.Interfaces
{
    public interface IAccommodationContext : IDisposable, IObjectContextAdapter
    {
        #region DBSet Properties
        IDbSet<Lodging> Lodgings { get; set; }
        IDbSet<Location> Locations { get; set; }
        IDbSet<State> States { get; set; }
        IDbSet<Advertising> Advertisings { get; set; }
        IDbSet<Photo> Photos { get; set; }
        IDbSet<AccommodationType> AccommodationTypes { get; set; }
        IDbSet<ServiceType> ServiceTypes { get; set; }
        IDbSet<Service> Services { get; set; }
        IDbSet<Tourism> Tourisms { get; set; }
        IDbSet<PublishType> PublishTypes { get; set; }

        #endregion

        #region EntityFramework Properties and Methods
        DbContextConfiguration Configuration { get; }
        Database Database { get; }

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        void DisposeTransaction();

        DbChangeTracker ChangeTracker { get; }

        int SaveChanges();
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        DbEntityEntry Entry(object entity);
        IDbSet<T> Set<T>() where T : class;
        int ExecuteSqlCommand(string sql, params object[] parameters);
        void DetectChanges();
        void SetUnchanged(object entity);
        System.Data.Entity.Infrastructure.DbEntityEntry UpdateEntries(object dbEntity, object entity);

        bool EnableCreationProxy { set; }
        #endregion
    }
}
