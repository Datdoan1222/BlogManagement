using BlogManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogManagement.EntityConfiguration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //foreign key authorId vs category name fk_pc_category
            builder.HasOne<Category>(p => p.Parents)
                .WithMany()
                .HasForeignKey(p => p.ParentId)
                .HasConstraintName("fk_category_parent")
                .OnDelete(DeleteBehavior.NoAction);

            //add index for author Id INDEX idx_pc_category
            builder.HasIndex(p => p.ParentId)
                .HasName("idx_category_parent")
                ;

            //set Title varchar 75 not null
            builder.Property(t => t.Title)
                .HasColumnType("nvarchar(75)")
                .IsRequired();

            //set metaTitle nvarchar(100)
            builder.Property(t => t.MetaTitle)
                .HasColumnType("nvarchar(100)");

            //set Slug VARCHAR(100) NOT NULL,
            builder.Property(t => t.Slug)
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            //set  type text Content and deault null
            builder.Property(s => s.Content)
                .HasColumnType("text");
        }
    }
}
