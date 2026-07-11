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

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<Friendship> Friendships { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Share> Shares { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comments__3214EC07D1F711F7");

            entity.HasIndex(e => e.PostId, "IX_Comments_PostId");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK_Comments_Posts");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_Users");
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Conversa__3214EC07B482112E");

            entity.HasIndex(e => new { e.User1Id, e.User2Id }, "UX_Conversations_UserPair").IsUnique();

            entity.HasIndex(e => new { e.User1Id, e.LastMessageAt }, "IX_Conversations_User1_LastMessageAt");

            entity.HasIndex(e => new { e.User2Id, e.LastMessageAt }, "IX_Conversations_User2_LastMessageAt");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.User1).WithMany(p => p.ConversationUser1s)
                .HasForeignKey(d => d.User1Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Conversations_User1");

            entity.HasOne(d => d.User2).WithMany(p => p.ConversationUser2s)
                .HasForeignKey(d => d.User2Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Conversations_User2");
        });

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

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Messages__3214EC073DE4E0A6");

            entity.HasIndex(e => new { e.ConversationId, e.CreatedAt }, "IX_Messages_ConversationId_CreatedAt");

            entity.HasIndex(e => new { e.ConversationId, e.IsRead }, "IX_Messages_ConversationId_IsRead");

            entity.HasIndex(e => e.SenderUserId, "IX_Messages_SenderUserId");

            entity.Property(e => e.Body).HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Conversation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ConversationId)
                .HasConstraintName("FK_Messages_Conversation");

            entity.HasOne(d => d.SenderUser).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Messages_SenderUser");
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

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Bio).HasMaxLength(1000);
            entity.Property(e => e.BioVisibility).HasMaxLength(20);
            entity.Property(e => e.College).HasMaxLength(200);
            entity.Property(e => e.CollegeVisibility).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.GenderVisibility).HasMaxLength(20);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(20);
            entity.Property(e => e.BirthDateVisibility).HasMaxLength(20);
            entity.Property(e => e.HighSchool).HasMaxLength(200);
            entity.Property(e => e.HighSchoolVisibility).HasMaxLength(20);
            entity.Property(e => e.Hobbies).HasMaxLength(500);
            entity.Property(e => e.HobbiesVisibility).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.PhoneVisibility).HasMaxLength(20);
            entity.Property(e => e.ProfilePicture).HasMaxLength(500);
            entity.Property(e => e.Work).HasMaxLength(200);
            entity.Property(e => e.WorkVisibility).HasMaxLength(20);
            entity.Property(e => e.AddressVisibility).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
