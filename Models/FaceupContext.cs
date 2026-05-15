using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Faceup.Models;

public partial class FaceupContext : DbContext
{
    public FaceupContext(DbContextOptions<FaceupContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Posts__3214EC07FFE9EF67");

            entity.Property(e => e.ImageUrl).HasMaxLength(500);

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Posts_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07E43FC93C");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
