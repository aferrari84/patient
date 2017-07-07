using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Models.Maps
{
    [ExcludeFromCodeCoverage]
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.Deleted)
                .HasColumnName("IsDeleted");


            this.ToTable("Users");
        }
    }
}
