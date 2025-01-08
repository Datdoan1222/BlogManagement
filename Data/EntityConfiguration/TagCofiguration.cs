using BlogManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogManagement.EntityConfiguration
{
    public class TagCofiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            //set varchar(75) Title
            builder.Property(s => s.Title)
                .HasColumnType("nvarchar(75)")
                .IsRequired();

            //set varchar(100) MetaTitle
            builder.Property(s => s.MetaTitle)
                .HasColumnType("nvarchar(100)");

            //set varchar(100) Slug
            builder.Property(s => s.Slug)
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            //set text content
            builder.Property(s => s.Content)
                .HasColumnType("text");

        }
    }
}
