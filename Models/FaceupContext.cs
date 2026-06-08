using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Faceup.Models;

public partial class FaceupContext : DbContext
{
    public FaceupContext()
    {
    }

    public FaceupContext(DbContextOptions<FaceupContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Friendship> Friendships { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Share> Shares { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Friendship>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Friendsh__3214EC07529B2CA5");

            entity.HasIndex(e => new { e.ReceiverId, e.Status }, "IX_Friendships_Receiver_Status");

            entity.HasIndex(e => new { e.RequesterId, e.Status }, "IX_Friendships_Requester_Status");

            entity.HasIndex(e => new { e.RequesterId, e.ReceiverId }, "UX_Friendships_RequesterReceiver")
                .IsUnique()
                .HasFilter("([Status] IN ('Pending', 'Accepted'))");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Receiver).WithMany(p => p.FriendshipReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Friendships_Receiver");

            entity.HasOne(d => d.Requester).WithMany(p => p.FriendshipRequesters)
                .HasForeignKey(d => d.RequesterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Friendships_Requester");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Likes__3214EC07B69D1EF0");

            entity.HasIndex(e => e.PostId, "IX_Likes_PostId");

            entity.HasIndex(e => new { e.PostId, e.UserId }, "UX_Likes_PostId_UserId").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Post).WithMany(p => p.Likes)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK_Likes_Posts");

            entity.HasOne(d => d.User).WithMany(p => p.Likes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Likes_Users");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Posts__3214EC07FFE9EF67");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ImageUrl).HasMaxLength(500);

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Posts_Users");
        });

        modelBuilder.Entity<Share>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shares__3214EC07E5948C41");

            entity.HasIndex(e => e.PostId, "IX_Shares_PostId").IsUnique();

            entity.HasIndex(e => new { e.PostId, e.UserId }, "IX_Shares_PostId_UserId").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Post).WithOne(p => p.Share)
                .HasForeignKey<Share>(d => d.PostId)
                .HasConstraintName("FK_Shares_PostId");

            entity.HasOne(d => d.User).WithMany(p => p.Shares)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shares_UserId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07E43FC93C");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.ProfilePicture).HasMaxLength(500);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
