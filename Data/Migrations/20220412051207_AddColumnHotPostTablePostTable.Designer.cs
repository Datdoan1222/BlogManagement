﻿// <auto-generated />
using System;
using BlogManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BlogManagement.Migrations
{
    [DbContext(typeof(BlogManagementDBContext))]
    [Migration("20220412051207_AddColumnHotPostTablePostTable")]
    partial class AddColumnHotPostTablePostTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.15")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BlogManagement.Data.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<string>("MetaTitle")
                        .HasColumnType("nvarchar(100)");

                    b.Property<long?>("ParentId")
                        .HasColumnType("bigint");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(75)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId")
                        .HasDatabaseName("idx_category_parent");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("BlogManagement.Data.Post", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AuthorId")
                        .HasColumnType("bigint");

                    b.Property<string>("Content")
                        .HasColumnType("ntext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("HotPost")
                        .HasColumnType("bit");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MetaTile")
                        .HasColumnType("nvarchar(100)");

                    b.Property<long?>("ParentId")
                        .HasColumnType("bigint");

                    b.Property<byte>("Published")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint")
                        .HasDefaultValue((byte)0);

                    b.Property<DateTime?>("PublishedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Summary")
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(75)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ParentId")
                        .HasDatabaseName("idx_post_parent");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("BlogManagement.Data.PostCategory", b =>
                {
                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<long>("PostId")
                        .HasColumnType("bigint");

                    b.HasKey("CategoryId", "PostId");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("idx_pc_category");

                    b.HasIndex("PostId")
                        .HasDatabaseName("idx_pc_post");

                    b.ToTable("PostCategories");
                });

            modelBuilder.Entity("BlogManagement.Data.PostComment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("ParentId")
                        .HasColumnType("bigint");

                    b.Property<long>("PostId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Published")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("PublishedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("TotalVotes")
                        .HasColumnType("int");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ParentId")
                        .HasDatabaseName("idx_comment_parent");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("PostComments");
                });

            modelBuilder.Entity("BlogManagement.Data.PostMeta", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<long>("PostId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("PostId")
                        .HasDatabaseName("idx_meta_post");

                    b.HasIndex("PostId", "Key")
                        .IsUnique()
                        .HasDatabaseName("uq_post_meta");

                    b.ToTable("PostMetas");
                });

            modelBuilder.Entity("BlogManagement.Data.PostTag", b =>
                {
                    b.Property<long>("PostId")
                        .HasColumnType("bigint");

                    b.Property<long>("TagId")
                        .HasColumnType("bigint");

                    b.HasKey("PostId", "TagId");

                    b.HasIndex("PostId")
                        .HasDatabaseName("idx_pt_post");

                    b.HasIndex("TagId")
                        .HasDatabaseName("idx_pt_tag");

                    b.ToTable("PostTags");
                });

            modelBuilder.Entity("BlogManagement.Data.Tag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<string>("MetaTitle")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(75)");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("BlogManagement.Data.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Firstname")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Intro")
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("datetime2");

                    b.Property<string>("Lastname")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Middlename")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasColumnType("varchar(15)");

                    b.Property<string>("NameIdentifier")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("varchar(32)");

                    b.Property<string>("Profile")
                        .HasColumnType("text");

                    b.Property<DateTime>("RegisteredAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("uq_email")
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("Mobile")
                        .HasDatabaseName("uq_mobile");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BlogManagement.Data.Category", b =>
                {
                    b.HasOne("BlogManagement.Data.Category", "Parents")
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .HasConstraintName("fk_category_parent")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Parents");
                });

            modelBuilder.Entity("BlogManagement.Data.Post", b =>
                {
                    b.HasOne("BlogManagement.Data.User", "Users")
                        .WithMany("Posts")
                        .HasForeignKey("AuthorId")
                        .HasConstraintName("fk_blog_user")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BlogManagement.Data.Post", "PostParent")
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("PostParent");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("BlogManagement.Data.PostCategory", b =>
                {
                    b.HasOne("BlogManagement.Data.Category", "Categories")
                        .WithMany("PostCategorys")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("fk_pc_category")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogManagement.Data.Post", "Posts")
                        .WithMany("Categories")
                        .HasForeignKey("PostId")
                        .HasConstraintName("fk_pc_post")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categories");

                    b.Navigation("Posts");
                });

            modelBuilder.Entity("BlogManagement.Data.PostComment", b =>
                {
                    b.HasOne("BlogManagement.Data.PostComment", "Parents")
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("BlogManagement.Data.Post", "Posts")
                        .WithMany("PostComments")
                        .HasForeignKey("PostId")
                        .HasConstraintName("fk_comment_post")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BlogManagement.Data.User", "Users")
                        .WithMany("PostComments")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_comment_user")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Parents");

                    b.Navigation("Posts");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("BlogManagement.Data.PostMeta", b =>
                {
                    b.HasOne("BlogManagement.Data.Post", "Posts")
                        .WithMany("PostMetas")
                        .HasForeignKey("PostId")
                        .HasConstraintName("fk_meta_post")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Posts");
                });

            modelBuilder.Entity("BlogManagement.Data.PostTag", b =>
                {
                    b.HasOne("BlogManagement.Data.Post", "Posts")
                        .WithMany("PostTags")
                        .HasForeignKey("PostId")
                        .HasConstraintName("fk_pt_post")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("BlogManagement.Data.Tag", "Tags")
                        .WithMany("TagsPost")
                        .HasForeignKey("TagId")
                        .HasConstraintName("fk_pt_Tag")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Posts");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("BlogManagement.Data.Category", b =>
                {
                    b.Navigation("PostCategorys");
                });

            modelBuilder.Entity("BlogManagement.Data.Post", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("PostComments");

                    b.Navigation("PostMetas");

                    b.Navigation("PostTags");
                });

            modelBuilder.Entity("BlogManagement.Data.Tag", b =>
                {
                    b.Navigation("TagsPost");
                });

            modelBuilder.Entity("BlogManagement.Data.User", b =>
                {
                    b.Navigation("PostComments");

                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
