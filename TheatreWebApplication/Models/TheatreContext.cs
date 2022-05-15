using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TheatreWebApplication
{
    public partial class TheatreContext : DbContext
    {
        public TheatreContext()
        {
        }

        public TheatreContext(DbContextOptions<TheatreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Act> Acts { get; set; } = null!;
        public virtual DbSet<ActActor> ActActors { get; set; } = null!;
        public virtual DbSet<Actor> Actors { get; set; } = null!;
        public virtual DbSet<Producer> Producers { get; set; } = null!;
        public virtual DbSet<Scenarist> Scenarists { get; set; } = null!;
        public virtual DbSet<Session> Sessions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=EUGEN-LAPTOP; Database = Theatre; Trusted_Connection=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Act>(entity =>
            {
                entity.ToTable("Act");

                entity.Property(e => e.ActName)
                    .HasMaxLength(64)
                    .HasColumnName("Act_name");

                entity.Property(e => e.Budget).HasColumnType("money");

                entity.Property(e => e.ProducerId).HasColumnName("Producer_id");

                entity.Property(e => e.ScenaristId).HasColumnName("Scenarist_id");

                entity.HasOne(d => d.Producer)
                    .WithMany(p => p.Acts)
                    .HasForeignKey(d => d.ProducerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Act_Producer");

                entity.HasOne(d => d.Scenarist)
                    .WithMany(p => p.Acts)
                    .HasForeignKey(d => d.ScenaristId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Act_Scenarist");
            });

            modelBuilder.Entity<ActActor>(entity =>
            {
                entity.HasKey(e => new { e.ActId, e.ActorId });

                entity.ToTable("Act_Actor");

                entity.Property(e => e.ActId).HasColumnName("Act_id");

                entity.Property(e => e.ActorId).HasColumnName("Actor_id");

                entity.Property(e => e.ContractDate)
                    .HasColumnType("date")
                    .HasColumnName("Contract_date");

                entity.HasOne(d => d.Act)
                    .WithMany(p => p.ActActors)
                    .HasForeignKey(d => d.ActId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Act_Actor_Act");

                entity.HasOne(d => d.Actor)
                    .WithMany(p => p.ActActors)
                    .HasForeignKey(d => d.ActorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Act_Actor_Actor");
            });

            modelBuilder.Entity<Actor>(entity =>
            {
                entity.ToTable("Actor");

                entity.Property(e => e.ActorName)
                    .HasMaxLength(32)
                    .HasColumnName("Actor_name");

                entity.Property(e => e.CareerStart)
                    .HasColumnType("date")
                    .HasColumnName("Career_start");
            });

            modelBuilder.Entity<Producer>(entity =>
            {
                entity.ToTable("Producer");

                entity.Property(e => e.ProducerName)
                    .HasMaxLength(32)
                    .HasColumnName("Producer_name");
            });

            modelBuilder.Entity<Scenarist>(entity =>
            {
                entity.ToTable("Scenarist");

                entity.Property(e => e.ScenaristName)
                    .HasMaxLength(32)
                    .HasColumnName("Scenarist_name");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("Session");

                //entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ActId).HasColumnName("Act_id");

                entity.Property(e => e.DateAndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("Date_and_time");

                entity.HasOne(d => d.Act)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.ActId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Session_Act");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
