using BlogManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogManagement.EntityConfiguration
{
    public class PostMetaConfiguration : IEntityTypeConfiguration<PostMeta>
    {
        public void Configure(EntityTypeBuilder<PostMeta> builder)
        {
            //set foreign key table PostMeta vs postid delete NoAction
            builder.HasOne(p => p.Posts)
                .WithMany(pt => pt.PostMetas)
                .HasForeignKey(m => m.PostId)
                .HasConstraintName("fk_meta_post")
                .OnDelete(DeleteBehavior.NoAction);

            //set key VARCHAR(50) NOT NULL,
            builder.Property(k => k.Key)
                .HasColumnType("nvarchar(50)")
                .IsRequired();

            //set Content text 
            builder.Property(s => s.Content)
                .HasColumnType("text");

            //index name idx_meta_post (postId ASC),
            builder.HasIndex(p => p.PostId)
                .HasName("idx_meta_post");

            //index uq_post_meta (postId ASC, key ASC) vs Unique
            builder.HasIndex(p => new { p.PostId, p.Key })
                .HasName("uq_post_meta")
                .IsUnique();

        }
    }
}
