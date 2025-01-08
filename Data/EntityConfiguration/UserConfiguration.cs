using BlogManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogManagement.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {

            //set varchar(50) Firstname
            builder.Property(s => s.Firstname)
                .HasColumnType("varchar(50)");

            builder.Property(s => s.Middlename)
                .HasColumnType("varchar(50)");

            //set varchar(50) Lastname
            builder.Property(s => s.Lastname)
                .HasColumnType("varchar(50)");

            //set varchar(50) Lastname
            builder.Property(s => s.Email)
                .HasColumnType("varchar(50)");

            //set unique and name unique for Email
            builder.HasIndex(x => x.Email)
                .HasName("uq_email")
                .IsUnique();

            //set varchar(50) Lastname
            builder.Property(s => s.PasswordHash)
                .HasColumnType("varchar(32)");

            //set varchar(15) Lastname
            builder.Property(s => s.Mobile)
                .HasColumnType("varchar(15)")
                .IsRequired();

            //set unique and name unique for moblie
            builder.HasIndex(x => x.Mobile)
                .HasName("uq_mobile")
                ;

            //set  RegisteredAt datetime notnull
            builder.Property(s => s.RegisteredAt)
                .IsRequired();

            //set Intro tinytext 
            builder.Property(s => s.Intro)
                 .HasColumnType("nvarchar(255)");

            //set Profile text 
            builder.Property(s => s.Profile)
                 .HasColumnType("text");

            //set NameIdentifier text 
            builder.Property(s => s.NameIdentifier)
                 .HasColumnType("varchar(255)");
            //set IImagePath text 
            builder.Property(s => s.ImagePath)
                 .HasColumnType("varchar(255)");
        }
    }
}
