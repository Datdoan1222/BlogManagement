using BlogManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogManagement.EntityConfiguration
{
    public class PostCommentConfiguration : IEntityTypeConfiguration<PostComment>
    {

        public void Configure(EntityTypeBuilder<PostComment> builder)
        {
            //title varchar 100 not null
            builder.Property(t => t.Title)
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            //title Published not null default 0
            builder.Property(t => t.Published)
                .HasDefaultValue(0)
                .IsRequired();

            //set text Content deault null
            builder.Property(s => s.Content)
                .HasColumnType("text");

            //parentId  null default null And Foreign key authorId vs postcomment
            builder.HasOne<PostComment>(p => p.Parents)
                .WithMany()
                .HasForeignKey(p => p.ParentId)
                .OnDelete(DeleteBehavior.NoAction);

            //foreign key 1: N Post vs PostComments ConstraintName ="fk_comment_post"
            builder.HasOne<Post>(p => p.Posts)
                .WithMany(p => p.PostComments)
                .HasForeignKey(s => s.PostId)
                .HasConstraintName("fk_comment_post")
                .OnDelete(DeleteBehavior.NoAction);
            //foreign key 1: N Post vs PostComments ConstraintName ="fk_comment_post"
            builder.HasOne<User>(p => p.Users)
                .WithMany(p => p.PostComments)
                .HasForeignKey(s => s.UserId)
                .HasConstraintName("fk_comment_user")
                .OnDelete(DeleteBehavior.NoAction);

            //add Index for Author Id
            builder.HasIndex(p => p.ParentId)
                .HasName("idx_comment_parent");
        }
    }
}
