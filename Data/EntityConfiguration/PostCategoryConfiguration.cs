using BlogManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogManagement.EntityConfiguration
{
    public class PostCategoryConfiguration : IEntityTypeConfiguration<PostCategory>
    {
        public void Configure(EntityTypeBuilder<PostCategory> builder)
        {
            //set primarykey is CategoryId and PostID
            builder.HasKey(k => new { k.CategoryId, k.PostId });

            //set foreign key AuthorId vs User ConstraintName fk_pc_category
            builder.HasOne<Category>(p => p.Categories)
                .WithMany(p => p.PostCategorys)
                .HasForeignKey(s => s.CategoryId)
                .HasConstraintName("fk_pc_category")
                .OnDelete(DeleteBehavior.Cascade);

            //set foreign key AuthorId vs Posts ConstraintName fk_pc_category
            builder.HasOne<Post>(p => p.Posts)
                .WithMany(p => p.Categories)
                .HasForeignKey(s => s.PostId)
                .HasConstraintName("fk_pc_post")
                .OnDelete(DeleteBehavior.Cascade);

            //set index  'idx_pc_category'
            builder.HasIndex(p => p.CategoryId)
                .HasName("idx_pc_category");

            //set index 'idx_pc_post'
            builder.HasIndex(p => p.PostId)
                .HasName("idx_pc_post");
        }
    }
}
