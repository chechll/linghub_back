using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using linghub.Models;

namespace linghub.Data;

public partial class LinghubContext : DbContext
{
    public LinghubContext()
    {
    }

    public LinghubContext(DbContextOptions<LinghubContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Calendar> Calendars { get; set; }

    public virtual DbSet<Error> Errors { get; set; }

    public virtual DbSet<Text> Texts { get; set; }

    public virtual DbSet<UText> UTexts { get; set; }

    public virtual DbSet<UWord> UWords { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Word> Words { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\MSSQLSERVER01;Database=linghub;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Calendar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("XPKkalendar");

            entity.ToTable("calendar");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Datum)
                .HasColumnType("date")
                .HasColumnName("datum");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Calendars)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("R_4");
        });

        modelBuilder.Entity<Error>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("XPKerror");

            entity.ToTable("Error");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("email");
        });

        modelBuilder.Entity<Text>(entity =>
        {
            entity.HasKey(e => e.IdText).HasName("XPKText");

            entity.ToTable("TEXT");

            entity.Property(e => e.IdText).HasColumnName("id_text");
            entity.Property(e => e.Ans)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("ans");
            entity.Property(e => e.Ans1)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("ans1");
            entity.Property(e => e.Ans2)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("ans2");
            entity.Property(e => e.Ans3)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("ans3");
            entity.Property(e => e.Question)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Text1).HasColumnName("text");
            entity.Property(e => e.TextName)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("text_name");
        });

        modelBuilder.Entity<UText>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("XPKUtext");

            entity.ToTable("U_text");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdText).HasColumnName("id_text");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdTextNavigation).WithMany(p => p.UTexts)
                .HasForeignKey(d => d.IdText)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("R_6");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.UTexts)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("R_5");
        });

        modelBuilder.Entity<UWord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("XPKUword");

            entity.ToTable("U_word");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.IdWord).HasColumnName("id_word");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.UWords)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("R_7");

            entity.HasOne(d => d.IdWordNavigation).WithMany(p => p.UWords)
                .HasForeignKey(d => d.IdWord)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("R_8");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("XPKUser");

            entity.ToTable("USER");

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Admin).HasColumnName("admin");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Surname)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("surname");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("user_password");
        });

        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(e => e.IdWord).HasName("XPKWord");

            entity.ToTable("Word");

            entity.Property(e => e.IdWord).HasColumnName("id_word");
            entity.Property(e => e.Ensent)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ensent");
            entity.Property(e => e.Enword)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("enword");
            entity.Property(e => e.Uasent)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("uasent");
            entity.Property(e => e.Uaword)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("uaword");
        });
        modelBuilder.HasSequence("WordSequence");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
