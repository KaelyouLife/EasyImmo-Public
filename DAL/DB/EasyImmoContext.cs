using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.DB;

public partial class EasyImmoContext : DbContext
{
    public EasyImmoContext()
    {
    }

    public EasyImmoContext(DbContextOptions<EasyImmoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Adresse> Adresses { get; set; }

    public virtual DbSet<Bien> Biens { get; set; }

    public virtual DbSet<DocumentBien> DocumentBiens { get; set; }

    public virtual DbSet<Evenement> Evenements { get; set; }

    public virtual DbSet<HistoriqueStatutBien> HistoriqueStatutBiens { get; set; }

    public virtual DbSet<Personne> Personnes { get; set; }

    public virtual DbSet<PhotoBien> PhotoBiens { get; set; }

    public virtual DbSet<RelationBienPersonne> RelationBienPersonnes { get; set; }

    public virtual DbSet<RelationEvenementPersonne> RelationEvenementPersonnes { get; set; }

    public virtual DbSet<RoleEvenementPersonne> RoleEvenementPersonnes { get; set; }

    public virtual DbSet<StatutBien> StatutBiens { get; set; }

    public virtual DbSet<StatutTransaction> StatutTransactions { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TypeBien> TypeBiens { get; set; }

    public virtual DbSet<TypeDocument> TypeDocuments { get; set; }

    public virtual DbSet<TypeEvenement> TypeEvenements { get; set; }

    public virtual DbSet<TypeTransaction> TypeTransactions { get; set; }

    public virtual DbSet<Utilisateur> Utilisateurs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Adresse>(entity =>
        {
            entity.HasKey(e => e.AdresseId).HasName("PK__Adresse__C3F7F274F9EF5601");

            entity.ToTable("Adresse");

            entity.Property(e => e.Boite).HasMaxLength(10);
            entity.Property(e => e.CodePostal).HasMaxLength(20);
            entity.Property(e => e.Commune).HasMaxLength(255);
            entity.Property(e => e.Numero).HasMaxLength(10);
            entity.Property(e => e.Rue).HasMaxLength(255);
        });

        modelBuilder.Entity<Bien>(entity =>
        {
            entity.HasKey(e => e.BienId).HasName("PK__Bien__F7061A5B72964559");

            entity.ToTable("Bien", tb => tb.HasTrigger("TRG_Bien_StatutHistorique"));

            entity.HasIndex(e => e.AdresseId, "IX_Bien_Adresse");

            entity.HasIndex(e => e.StatutBienId, "IX_Bien_Statut");

            entity.HasIndex(e => e.TypeBienId, "IX_Bien_Type");

            entity.HasIndex(e => e.Reference, "UQ__Bien__062B9EB89ACF1AC7").IsUnique();

            entity.Property(e => e.AnneeConstruction).HasMaxLength(10);
            entity.Property(e => e.ChargesMensuelles).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Chauffage).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(700);
            entity.Property(e => e.Isolation).HasMaxLength(50);
            entity.Property(e => e.LoyerMensuel).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Peb)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.PrixVente).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Reference).HasMaxLength(50);
            entity.Property(e => e.SurfaceHabitable).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.SurfaceTotale).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TitreAnnonce).HasMaxLength(150);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Adresse).WithMany(p => p.Biens)
                .HasForeignKey(d => d.AdresseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bien_Adresse");

            entity.HasOne(d => d.StatutBien).WithMany(p => p.Biens)
                .HasForeignKey(d => d.StatutBienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bien_StatutBien");

            entity.HasOne(d => d.TypeBien).WithMany(p => p.Biens)
                .HasForeignKey(d => d.TypeBienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Bien_TypeBien");
        });

        modelBuilder.Entity<DocumentBien>(entity =>
        {
            entity.HasKey(e => e.DocumentBienId).HasName("PK__Document__346364CAE1C3531F");

            entity.ToTable("DocumentBien");

            entity.HasIndex(e => e.BienId, "IX_DocumentBien_Bien");

            entity.Property(e => e.Chemin).HasMaxLength(250);
            entity.Property(e => e.Description).HasMaxLength(50);

            entity.HasOne(d => d.Bien).WithMany(p => p.DocumentBiens)
                .HasForeignKey(d => d.BienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentBien_Bien");

            entity.HasOne(d => d.TypeDocument).WithMany(p => p.DocumentBiens)
                .HasForeignKey(d => d.TypeDocumentId)
                .HasConstraintName("FK_DocumentBien_TypeDocument");
        });

        modelBuilder.Entity<Evenement>(entity =>
        {
            entity.HasKey(e => e.EvenementId).HasName("PK__Evenemen__327074B0B0C71A26");

            entity.ToTable("Evenement");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.EstAccompli).HasDefaultValue(false);

            entity.HasOne(d => d.Bien).WithMany(p => p.Evenements)
                .HasForeignKey(d => d.BienId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Evenement_Bien");

            entity.HasOne(d => d.TypeEvenement).WithMany(p => p.Evenements)
                .HasForeignKey(d => d.TypeEvenementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Evenement_TypeEvenement");
        });

        modelBuilder.Entity<HistoriqueStatutBien>(entity =>
        {
            entity.HasKey(e => e.HistoriqueStatutBienId).HasName("PK__Historiq__DB781FD1C7ABB2F2");

            entity.ToTable("HistoriqueStatutBien");

            entity.Property(e => e.DateChangement)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Bien).WithMany(p => p.HistoriqueStatutBiens)
                .HasForeignKey(d => d.BienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistoriqueStatutBien_Bien");

            entity.HasOne(d => d.StatutBien).WithMany(p => p.HistoriqueStatutBiens)
                .HasForeignKey(d => d.StatutBienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistoriqueStatutBien_StatutBien");
        });

        modelBuilder.Entity<Personne>(entity =>
        {
            entity.HasKey(e => e.PersonneId).HasName("PK__Personne__BFD588A892DA9CA3");

            entity.ToTable("Personne");

            entity.HasIndex(e => e.Email, "UQ__Personne__A9D10534F7473A06").IsUnique();

            entity.HasIndex(e => e.Telephone, "UQ__Personne__D9FEB744E7FBE982").IsUnique();

            entity.Property(e => e.CompteBancaire).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Nom).HasMaxLength(50);
            entity.Property(e => e.Prenom).HasMaxLength(50);
            entity.Property(e => e.Sexe)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Telephone).HasMaxLength(15);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Adresse).WithMany(p => p.Personnes)
                .HasForeignKey(d => d.AdresseId)
                .HasConstraintName("FK_Personne_Adresse");
        });

        modelBuilder.Entity<PhotoBien>(entity =>
        {
            entity.HasKey(e => e.PhotoBienId).HasName("PK__PhotoBie__583A2BF7B414FAED");

            entity.ToTable("PhotoBien");

            entity.HasIndex(e => e.BienId, "IX_PhotoBien_Bien");

            entity.Property(e => e.Chemin).HasMaxLength(250);
            entity.Property(e => e.EstPrincipale).HasDefaultValue(false);

            entity.HasOne(d => d.Bien).WithMany(p => p.PhotoBiens)
                .HasForeignKey(d => d.BienId)
                .HasConstraintName("FK_PhotoBien_Bien");
        });

        modelBuilder.Entity<RelationBienPersonne>(entity =>
        {
            entity.HasKey(e => e.RelationBienPersonneId).HasName("PK__Relation__4E1707B350B82B2A");

            entity.ToTable("RelationBienPersonne");

            entity.Property(e => e.Role).HasMaxLength(30);

            entity.HasOne(d => d.Bien).WithMany(p => p.RelationBienPersonnes)
                .HasForeignKey(d => d.BienId)
                .HasConstraintName("FK_RelBienPersonne_Bien");

            entity.HasOne(d => d.Personne).WithMany(p => p.RelationBienPersonnes)
                .HasForeignKey(d => d.PersonneId)
                .HasConstraintName("FK_RelBienPersonne_Personne");
        });

        modelBuilder.Entity<RelationEvenementPersonne>(entity =>
        {
            entity.HasKey(e => e.RelationEvenementPersonneId).HasName("PK__Relation__56D8EC818F4D3F38");

            entity.ToTable("RelationEvenementPersonne");

            entity.Property(e => e.Commentaire).HasMaxLength(400);

            entity.HasOne(d => d.Evenement).WithMany(p => p.RelationEvenementPersonnes)
                .HasForeignKey(d => d.EvenementId)
                .HasConstraintName("FK_RelEvenementPersonne_Evenement");

            entity.HasOne(d => d.Personne).WithMany(p => p.RelationEvenementPersonnes)
                .HasForeignKey(d => d.PersonneId)
                .HasConstraintName("FK_RelEvenementPersonne_Personne");

            entity.HasOne(d => d.RoleEvenementPersonne).WithMany(p => p.RelationEvenementPersonnes)
                .HasForeignKey(d => d.RoleEvenementPersonneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RelEvenementPersonne_Role");
        });

        modelBuilder.Entity<RoleEvenementPersonne>(entity =>
        {
            entity.HasKey(e => e.RoleEvenementPersonneId).HasName("PK__RoleEven__CF2F40B37D92D691");

            entity.ToTable("RoleEvenementPersonne");

            entity.Property(e => e.Libelle).HasMaxLength(50);
        });

        modelBuilder.Entity<StatutBien>(entity =>
        {
            entity.HasKey(e => e.StatutBienId).HasName("PK__StatutBi__BC95458C45EED80E");

            entity.ToTable("StatutBien");

            entity.Property(e => e.Libelle).HasMaxLength(50);
        });

        modelBuilder.Entity<StatutTransaction>(entity =>
        {
            entity.HasKey(e => e.StatutTransactionId).HasName("PK__StatutTr__BA1CB40682B99571");

            entity.ToTable("StatutTransaction");

            entity.Property(e => e.Libelle).HasMaxLength(255);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A6B44F2D642");

            entity.ToTable("Transaction");

            entity.HasIndex(e => e.BienId, "IX_Transaction_Bien");

            entity.HasIndex(e => e.TypeTransactionId, "IX_Transaction_Type");

            entity.Property(e => e.DateTransaction).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Montant).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Reference).HasMaxLength(255);

            entity.HasOne(d => d.Beneficiaire).WithMany(p => p.TransactionBeneficiaires)
                .HasForeignKey(d => d.BeneficiaireId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_Beneficiaire");

            entity.HasOne(d => d.Bien).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.BienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_Bien");

            entity.HasOne(d => d.Document).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK_Transaction_Document");

            entity.HasOne(d => d.Payeur).WithMany(p => p.TransactionPayeurs)
                .HasForeignKey(d => d.PayeurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_Payeur");

            entity.HasOne(d => d.StatutTransaction).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.StatutTransactionId)
                .HasConstraintName("FK_Transaction_Statut");

            entity.HasOne(d => d.TypeTransaction).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.TypeTransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transaction_Type");
        });

        modelBuilder.Entity<TypeBien>(entity =>
        {
            entity.HasKey(e => e.TypeBienId).HasName("PK__TypeBien__FB3658602600F6E7");

            entity.ToTable("TypeBien");

            entity.Property(e => e.Libelle).HasMaxLength(50);
        });

        modelBuilder.Entity<TypeDocument>(entity =>
        {
            entity.HasKey(e => e.TypeDocumentId).HasName("PK__TypeDocu__65AC7F8CDB031783");

            entity.ToTable("TypeDocument");

            entity.Property(e => e.Libelle).HasMaxLength(255);
        });

        modelBuilder.Entity<TypeEvenement>(entity =>
        {
            entity.HasKey(e => e.TypeEvenementId).HasName("PK__TypeEven__7B1DFC54795CE3DA");

            entity.ToTable("TypeEvenement");

            entity.Property(e => e.Libelle).HasMaxLength(50);
        });

        modelBuilder.Entity<TypeTransaction>(entity =>
        {
            entity.HasKey(e => e.TypeTransactionId).HasName("PK__TypeTran__372EE83D2BA09841");

            entity.ToTable("TypeTransaction");

            entity.Property(e => e.Libelle).HasMaxLength(50);
        });

        modelBuilder.Entity<Utilisateur>(entity =>
        {
            entity.HasKey(e => e.UtilisateurId).HasName("PK__Utilisat__6CB6ADFF63B984B2");

            entity.ToTable("Utilisateur");

            entity.HasIndex(e => e.NomUtilisateur, "UQ__Utilisat__49EDB0E56A8367AB").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MotDePasse).HasMaxLength(255);
            entity.Property(e => e.NomUtilisateur).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
