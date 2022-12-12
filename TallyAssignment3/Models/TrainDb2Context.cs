using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TallyAssignment3.Models;

public partial class TrainDb2Context : DbContext
{

    public TrainDb2Context(DbContextOptions<TrainDb2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudId).HasName("PK_StudentId");

            entity.ToTable("Student");

            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Class)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
