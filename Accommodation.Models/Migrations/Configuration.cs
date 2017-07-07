using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Models.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<Accommodation.Models.Context.AccommodationContext>
    {
        public Configuration()
        {
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();
            //SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Accommodation.Models.Context.AccommodationContext context)
        {
            try
            {
                //Just run the seed when the DB is empty
                if (context.Database.Exists())
                {
                    if (context.Locations.Count() < 1)
                    {
                        RunScripts(context);
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }

        private static void RunScripts(Accommodation.Models.Context.AccommodationContext context)
        {
            try
            {
                string dir = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName;
                context.Database.ExecuteSqlCommand(File.ReadAllText(Path.Combine(dir, "Fixtures/States.sql")));
                context.Database.ExecuteSqlCommand(File.ReadAllText(Path.Combine(dir, "Fixtures/Locations.sql")));
                context.Database.ExecuteSqlCommand(File.ReadAllText(Path.Combine(dir, "Fixtures/AccommodationType.sql")));
                context.Database.ExecuteSqlCommand(File.ReadAllText(Path.Combine(dir, "Fixtures/ServiceType.sql")));
                context.Database.ExecuteSqlCommand(File.ReadAllText(Path.Combine(dir, "Fixtures/TourismType.sql")));
                context.Database.ExecuteSqlCommand(File.ReadAllText(Path.Combine(dir, "Fixtures/PublishType.sql")));
                context.Database.ExecuteSqlCommand(File.ReadAllText(Path.Combine(dir, "Fixtures/Lodgings.sql")));
                context.Database.ExecuteSqlCommand(File.ReadAllText(Path.Combine(dir, "Fixtures/Services.sql")));
                context.Database.ExecuteSqlCommand(File.ReadAllText(Path.Combine(dir, "Fixtures/Tourisms.sql")));
                context.Database.ExecuteSqlCommand(File.ReadAllText(Path.Combine(dir, "Fixtures/Photos.sql")));
                context.Database.ExecuteSqlCommand(File.ReadAllText(Path.Combine(dir, "Fixtures/Advertising.sql")));
                
            }
            catch (Exception ex)
            {
                ErrorManager.ErrorHandler.HandleError(ex);
                throw ex;
            }
        }
    }
}

