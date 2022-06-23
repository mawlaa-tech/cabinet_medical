using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microservice.Models;

namespace Microservice.DbContexts
{
    public partial class CabinetContext : DbContext
    {
        public CabinetContext()
        {
        }

        public CabinetContext(DbContextOptions<CabinetContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActeMedical> ActeMedicaux { get; set; } = null!;
        public virtual DbSet<Admin> Admins { get; set; } = null!;
        public virtual DbSet<DossierFinancier> DossierFinanciers { get; set; } = null!;
        public virtual DbSet<DossierMedical> DossierMedicaux { get; set; } = null!;
        public virtual DbSet<Medecin> Medecins { get; set; } = null!;
        public virtual DbSet<Patient> Patients { get; set; } = null!;
        public virtual DbSet<PlageHoraire> PlageHoraires { get; set; } = null!;
        public virtual DbSet<RendezVous> RendezVous { get; set; } = null!;
        public virtual DbSet<Specialite> Specialites { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Cabinet;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActeMedical>(entity =>
            {
                entity.ToTable("acte_medical");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Libelle)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("libelle");

                entity.Property(e => e.SpecialiteId).HasColumnName("specialite_id");

                entity.HasOne(d => d.Specialite)
                    .WithMany(p => p.ActeMedicaux)
                    .HasForeignKey(d => d.SpecialiteId)
                    .HasConstraintName("FK8brb14928onw358u2x3xcgxgm");
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("admin");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("role");
            });

            modelBuilder.Entity<DossierFinancier>(entity =>
            {
                entity.ToTable("dossier_financier");

                entity.HasIndex(e => e.PatientId, "UQ__dossier___4D5CE4770121DCE4")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NumDossier)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("num_dossier");

                entity.Property(e => e.PatientId).HasColumnName("patient_id");

                entity.HasOne(d => d.Patient)
                    .WithOne(p => p.DossierFinancier)
                    .HasForeignKey<DossierFinancier>(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKn46sjyin2rm3o7onesf5ypye0");
            });

            modelBuilder.Entity<DossierMedical>(entity =>
            {
                entity.ToTable("dossier_medical");

                entity.HasIndex(e => e.PatientId, "UQ__dossier___4D5CE477B364EF3E")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NumDossier)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("num_dossier");

                entity.Property(e => e.PatientId).HasColumnName("patient_id");

                entity.HasOne(d => d.Patient)
                    .WithOne(p => p.DossierMedical)
                    .HasForeignKey<DossierMedical>(d => d.PatientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKhr7ar2qbk7e6j9l2s8ey8lerb");
            });

            modelBuilder.Entity<Medecin>(entity =>
            {
                entity.ToTable("medecin");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.NomMedecin)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nom_medecin");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.PrenomMedecin)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("prenom_medecin");

                entity.Property(e => e.Role)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("role");

                entity.HasMany(d => d.Specialites)
                    .WithMany(p => p.Medecins)
                    .UsingEntity<Dictionary<string, object>>(
                        "MedecinSpecialite",
                        l => l.HasOne<Specialite>().WithMany().HasForeignKey("SpecialiteId").OnDelete(DeleteBehavior.Cascade).HasConstraintName("FKect93bn1mx1eer9dygro2qqxl"),
                        r => r.HasOne<Medecin>().WithMany().HasForeignKey("MedecinId").OnDelete(DeleteBehavior.Cascade).HasConstraintName("FKorhfqpvf30ir3eqji1xper78w"),
                        j =>
                        {
                            j.HasKey("MedecinId", "SpecialiteId").HasName("PK__medecin___7F0CD86B9DBDEF23");

                            j.ToTable("medecin_specialite");

                            j.IndexerProperty<long>("MedecinId").HasColumnName("medecin_id");

                            j.IndexerProperty<long>("SpecialiteId").HasColumnName("specialite_id");
                        });
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("patient");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DatNaisPat)
                    .HasColumnType("date")
                    .HasColumnName("dat_nais_pat");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.NomPat)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nom_pat");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.PhonePat)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("phone_pat");

                entity.Property(e => e.PrenPat)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("pren_pat");

                entity.Property(e => e.Role)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("role");

                entity.Property(e => e.SexPat)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("sex_pat");
            });

            modelBuilder.Entity<PlageHoraire>(entity =>
            {
                entity.ToTable("plage_horaire");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActeMedicalId).HasColumnName("acte_medical_id");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.HeureDebut).HasColumnName("heure_debut");

                entity.Property(e => e.HeureTer).HasColumnName("heure_ter");

                entity.Property(e => e.MedecinId).HasColumnName("medecin_id");

                entity.Property(e => e.SpecialiteId).HasColumnName("specialite_id");

                entity.HasOne(d => d.ActeMedical)
                    .WithMany(p => p.PlageHoraires)
                    .HasForeignKey(d => d.ActeMedicalId)
                    .HasConstraintName("FKi44vbbd0f3lvqpwlxwn69skb9");

                entity.HasOne(d => d.Medecin)
                    .WithMany(p => p.PlageHoraires)
                    .HasForeignKey(d => d.MedecinId)
                    .HasConstraintName("FKaukbkcdfs9hmaof9eg9bmhor4");

                entity.HasOne(d => d.Specialite)
                    .WithMany(p => p.PlageHoraires)
                    .HasForeignKey(d => d.SpecialiteId)
                    .HasConstraintName("FKje2feb755oyo88c9qekdnhtn2");
            });

            modelBuilder.Entity<RendezVous>(entity =>
            {
                entity.ToTable("rendez_vous");

                entity.HasIndex(e => e.PlageHoraireId, "UQ__rendez_v__7D9FA5C29491E5ED")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PatientId).HasColumnName("patient_id");

                entity.Property(e => e.PlageHoraireId).HasColumnName("plage_horaire_id");

                entity.HasOne(d => d.Patient)
                    .WithMany(p => p.RendezVous)
                    .HasForeignKey(d => d.PatientId)
                    .HasConstraintName("FKjfqx91ipif2aimip15soh8kbm");

                entity.HasOne(d => d.PlageHoraire)
                    .WithOne(p => p.RendezVous)
                    .HasForeignKey<RendezVous>(d => d.PlageHoraireId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK65icp6skpylqrx7x76kj8ukaw");
            });

            modelBuilder.Entity<Specialite>(entity =>
            {
                entity.ToTable("specialite");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nom)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("nom");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
