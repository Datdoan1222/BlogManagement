using BlogManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogManagement.EntityConfiguration
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {

            //set title varchar(75) and not null
            builder.Property(p => p.Title)
                .HasColumnType("nvarchar(75)")
                .IsRequired();

            //set title varchar(75) 
            builder.Property(p => p.MetaTile)
                .HasColumnType("nvarchar(100)");

            //set Slug varchar(100) and not null
            builder.Property(p => p.Slug)
                .HasColumnType("nvarchar(100)")
                .IsRequired();
            //set Summary tinytext 
            builder.Property(p => p.Summary)
                .HasColumnType("nvarchar(255)");

            //set CreatedAt datetime not null
            builder.Property(s => s.CreatedAt)
                .IsRequired();

            //set Published tinyint(1) and not null default 0
            builder.Property(p => p.Published)
                .HasColumnType("bit");

            
            //set text Content
            builder.Property(s => s.Content)
                .HasColumnType("ntext");

            //foreign key authorId vs user
            builder.HasOne<User>(p => p.Users)
                .WithMany(p => p.Posts)
                .HasForeignKey(s => s.AuthorId)
                .HasConstraintName("fk_blog_user")
                .OnDelete(DeleteBehavior.NoAction);
            //set parentid is null

            //foreign key authorId vs user
            builder.HasOne<Post>(p => p.PostParent)
                .WithMany()
                .HasForeignKey(p => p.ParentId)
                .OnDelete(DeleteBehavior.NoAction);

            // add index for author Id
            builder.HasIndex(p => p.ParentId)
                .HasName("idx_post_parent");

        }
    }
}
