using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accommodation.Models.Maps
{
    //[ExcludeFromCodeCoverage]
    //public class EmployeeMap : EntityTypeConfiguration<Employee>
    //{
    //    public EmployeeMap()
    //    {
    //        this.HasKey(e => e.ID);
    //        this.Property(e => e.FirstName)
    //            .HasMaxLength(50)
    //            .IsRequired();

    //        this.Property(e => e.LastName)
    //            .HasMaxLength(50)
    //            .IsRequired();

    //        this.Property(e => e.CUIL)
    //            .HasMaxLength(50)
    //            .IsRequired();

    //        this.Property(e => e.DateEnter)
    //            .IsRequired();

    //        this.Property(e => e.CivilStatus)
    //            .IsRequired();

    //        this.Property(e => e.Birthdate)
    //            .IsRequired();

    //        this.Property(e => e.Email)
    //            .HasMaxLength(200)
    //            .IsRequired();

    //        this.Property(e => e.CompanyEmail)
    //           .HasMaxLength(200)
    //           .IsOptional();

    //        this.Property(e => e.EndDate)
    //            .IsOptional();

    //        this.Property(e => e.Gender)
    //            .IsRequired();

    //        this.Property(e => e.Address)
    //            .HasMaxLength(300)
    //            .IsOptional();

    //        this.Property(e => e.Nationality)
    //            .HasMaxLength(100)
    //            .IsOptional();

    //        this.Property(e => e.PassportExp)
    //            .IsOptional();

    //        this.Property(e => e.VisaExp)
    //            .IsOptional();

    //        this.Property(e => e.Phone)
    //            .IsOptional();

    //        this.Property(e => e.Picture)
    //            .IsOptional();

    //        this.Property(e => e.Type)
    //            .IsRequired();

    //        this.Property(e => e.HoursPerDay)
    //            .IsRequired();

    //        this.Property(e => e.Billable)
    //            .IsRequired();
    //        // Common maps
    //        this.Property(x => x.Deleted)
    //            .HasColumnName("IsDeleted");

    //        this.Property(x => x.Created)
    //            .HasColumnName("Created");

    //        this.Property(x => x.Updated)
    //            .HasColumnName("Updated");

    //        this.ToTable("Employees");

    //        //this.HasRequired<Area>(u => u.Area)
    //        //    .WithRequiredDependent(c => c.AreaResponsable);

    //    }
    //}
}
