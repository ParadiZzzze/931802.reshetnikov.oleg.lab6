using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication8.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PostCategory> PostCategories { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<PostComment> PostComments { get; set; }

        public DbSet<PostAttachment> PostAttachments { get; set; }

        public DbSet<ForumCategory> ForumCategorys { get; set; }

        public DbSet<Forum> Forums { get; set; }

        public DbSet<ForumTopic> ForumTopics { get; set; }

        public DbSet<ForumMessage> ForumMessages { get; set; }

        public DbSet<ForumMessageAttachment> ForumMessageAttachments { get; set; }

        public DbSet<Folder> Folders { get; set; }

        public DbSet<File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Post>().HasOne(x => x.Creator).WithMany().OnDelete(DeleteBehavior.Restrict);
            builder.Entity<PostComment>().HasOne(x => x.Creator).WithMany().OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ForumTopic>().HasOne(x => x.ApplicationUser).WithMany().OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ForumMessage>().HasOne(x => x.ApplicationUser).WithMany().OnDelete(DeleteBehavior.Restrict);


            //builder.Entity<OneToOneFirst>()
            //.HasRequired(x => x.Second)                      для 1 к 1
            //.WithRequiredPrincipal(x => x.First);
        }
    }
}
