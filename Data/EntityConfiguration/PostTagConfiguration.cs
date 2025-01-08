using BlogManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogManagement.EntityConfiguration
{
    public class PostTagConfiguration : IEntityTypeConfiguration<PostTag>
    {
        public void Configure(EntityTypeBuilder<PostTag> builder)
        {
            //set primary key for postid and tagid
            builder.HasKey(k => new { k.PostId, k.TagId });

            //foreign key PostId vs Post
            builder.HasOne<Post>(p => p.Posts)
                .WithMany(p => p.PostTags)
                .HasForeignKey(s => s.PostId)
                .HasConstraintName("fk_pt_post")
                .OnDelete(DeleteBehavior.NoAction);

            //foreign key authorId vs post
            builder.HasOne<Tag>(p => p.Tags)
                .WithMany(p => p.TagsPost)
                .HasForeignKey(s => s.TagId)
                .HasConstraintName("fk_pt_Tag")
                .OnDelete(DeleteBehavior.NoAction);

            //set index for tagid and postid
            builder.HasIndex(p => p.TagId)
                .HasName("idx_pt_tag");

            builder.HasIndex(p => p.PostId)
                .HasName("idx_pt_post");
        }
    }
}
