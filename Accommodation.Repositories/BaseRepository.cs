using System;
using System.Collections;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Accommodation.Interfaces.Services;
using Accommodation.Models;
using Accommodation.Models.Interfaces;

namespace Accommodation.Repositories
{
    public class BaseRepository
    {
        private IAccommodationContext _context;

        private ICurrentUserService _userService;
        private const string InsertName = "Insert";
        private const string UpdateName = "Update";
        private const string DeleteName = "Delete";

        public virtual IAccommodationContext Context
        {
            get
            {
                return _context;
            }
            set
            {
                if (value != null)
                {
                    _context = value;
                }
            }
        }

        public BaseRepository(IAccommodationContext context, ICurrentUserService userService)
        {
            _context = context;
            _userService = userService;
        }


        public virtual T Insert<T>(T entity, bool autoLoading = false) where T : EntityBase
        {

            try
            {
                string new_entity = SerializeEntity<T>(entity);

                var dbSet = (IDbSet<T>)_context.Set<T>();
                entity.Created = DateTime.Now;
                dbSet.Add(entity);
                _context.SaveChanges();

                LogRecord<T>(new_entity, InsertName);
                if (autoLoading)
                {
                    return AutoLoadReferences<T>(entity);
                }
                return entity;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public virtual T InsertUser<T>(T entity) where T : IdentityUser
        {

            try
            {
                var dbSet = (IDbSet<T>)_context.Set<T>();

                dbSet.Add(entity);
                _context.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public virtual bool Update<T>(T entity, bool compositeKey = false, int failedAttemps = 0) where T : EntityBase
        {
            try
            {
                string new_entity = string.Empty;
                string old_entity = string.Empty;

                if (failedAttemps < 3)
                {
                    bool needToSave = false;
                    var dbSet = _context.Set<T>();

                    _context.EnableCreationProxy = false;
                    T dbEntry = compositeKey ? dbSet.Find(entity.IdentityID(compositeKey)) : dbSet.Find(entity.IdentityID());
                    old_entity = SerializeEntity<T>(dbEntry);
                    _context.EnableCreationProxy = true;

                    if (dbEntry != null)
                    {
                        entity.Created = dbEntry.Created;
                        entity.Updated = DateTime.Now;
                        new_entity = SerializeEntity<T>(entity);
                        var entry = Context.UpdateEntries(dbEntry, entity);
                        needToSave = true;
                    }

                    if (needToSave)
                    {
                        _context.SaveChanges();
                    }

                    LogRecord<T>(new_entity, UpdateName, old_entity);
                    return needToSave;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public virtual bool UpdateUser<T>(T entity, int failedAttemps = 0) where T : IdentityUser
        {
            try
            {
                if (failedAttemps < 3)
                {
                    bool needToSave = false;
                    var dbSet = _context.Set<T>();

                    T dbEntry = dbSet.Find(entity.Id);

                    if (dbEntry != null)
                    {
                        Context.UpdateEntries(dbEntry, entity);
                        needToSave = true;
                    }

                    if (needToSave)
                    {
                        _context.SaveChanges();
                    }

                    return needToSave;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public virtual T AutoLoadReferences<T>(T entity) where T : EntityBase
        {
            try
            {
                var ObjectSet = Context.ObjectContext.CreateObjectSet<T>();

                // Get entity set for current entity type
                var entitySet = ObjectSet.EntitySet;
                // Get name of the entity's navigation properties
                Type type = entity.GetType();
                var properties = type.GetProperties();

                var _propertyNames = ((EntityType)entitySet.ElementType).NavigationProperties.Select(p => p.Name).ToArray();

                foreach (var property in properties.Where(x => _propertyNames.Contains(x.Name)))
                {
                    var propertyType = property.PropertyType;

                    bool isCollection = propertyType.GetInterfaces().Any(x => x == typeof(IEnumerable)) &&
                                        !propertyType.Equals(typeof(string));
                    if (isCollection)
                    {
                        Context.Entry(entity).Collection(property.Name).Load();
                    }
                    else if ((!propertyType.IsValueType && !propertyType.Equals(typeof(string))))
                    {
                        Context.Entry(entity).Reference(property.Name).Load();
                    }
                }
                return entity;
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public virtual IQueryable<T> OrderBy<T>(IQueryable<T> list, string column, bool desc)
        {
            try
            {
                return list.OrderBy(column, desc);
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public virtual bool Delete<T>(T entity, bool compositeKey = false, int failedAttemps = 0) where T : EntityBase
        {
            try
            {
                string new_entity = string.Empty;
                string old_entity = string.Empty;

                if (failedAttemps < 3)
                {
                    bool needToSave = false;
                    var dbSet = _context.Set<T>();

                    _context.EnableCreationProxy = false;
                    T dbEntry = compositeKey ? dbSet.Find(entity.IdentityID(compositeKey)) : dbSet.Find(entity.IdentityID());
                    old_entity = SerializeEntity<T>(dbEntry);
                    _context.EnableCreationProxy = true;

                    if (dbEntry != null)
                    {

                        entity.Deleted = true;
                        entity.Updated = DateTime.Now;
                        new_entity = SerializeEntity<T>(entity);
                        var entry = Context.UpdateEntries(dbEntry, entity);
                        needToSave = true;
                    }

                    if (needToSave)
                    {
                        _context.SaveChanges();
                    }

                    LogRecord<T>(new_entity, DeleteName, old_entity);

                    return needToSave;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        public virtual bool LogRecord<T>(string new_entity, string operation, string old_entity = null) where T : EntityBase
        {
            string typeName = typeof(T).Name;

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                return false;
            }
            return true;
        }

        public virtual string SerializeEntity<T>(T entity)
        {
            try
            {
                return JsonConvert.SerializeObject(entity, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            catch (Exception ex)
            {

                ErrorManager.ErrorHandler.HandleError(ex);
                var type = entity.GetType();
                return string.Format("Wasn't able to serialize the entity {0}", type.ToString());
            }
        }
    }
}
