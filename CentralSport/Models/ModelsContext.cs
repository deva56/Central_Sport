namespace CentralSportV1._0._1.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelsContext : DbContext
    {
        public ModelsContext()
            : base("name=MySQLServerDefaultConnection")
        {
        }

        public virtual DbSet<admin> admin { get; set; }
        public virtual DbSet<gradovi> gradovi { get; set; }
        public virtual DbSet<images> images { get; set; }
        public virtual DbSet<korisnik> korisnik { get; set; }
        public virtual DbSet<odabranapoduzećaindex> odabranapoduzećaindex { get; set; }
        public virtual DbSet<poduzeće> poduzeće { get; set; }
        public virtual DbSet<sportovi> sportovi { get; set; }
        public virtual DbSet<županije> županije { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<admin>()
                .Property(e => e.idAdmin)
                .IsUnicode(false);

            modelBuilder.Entity<admin>()
                .Property(e => e.imeAdmin)
                .IsUnicode(false);

            modelBuilder.Entity<gradovi>()
                .Property(e => e.idGradovi)
                .IsUnicode(false);

            modelBuilder.Entity<gradovi>()
                .Property(e => e.imeGradovi)
                .IsUnicode(false);

            modelBuilder.Entity<images>()
                .Property(e => e.imageId)
                .IsUnicode(false);

            modelBuilder.Entity<images>()
                .Property(e => e.imageIdByPoduzeće)
                .IsUnicode(false);

            modelBuilder.Entity<images>()
                .Property(e => e.imagePathOnDisk)
                .IsUnicode(false);

            modelBuilder.Entity<korisnik>()
                .Property(e => e.idKorisnik)
                .IsUnicode(false);

            modelBuilder.Entity<korisnik>()
                .Property(e => e.imeKorisnik)
                .IsUnicode(false);

            modelBuilder.Entity<korisnik>()
                .Property(e => e.prezimeKorisnik)
                .IsUnicode(false);

            modelBuilder.Entity<korisnik>()
                .Property(e => e.kratkiOpisKorisnik)
                .IsUnicode(false);

            modelBuilder.Entity<korisnik>()
                .Property(e => e.registracijskiEmailKorisnik)
                .IsUnicode(false);

            modelBuilder.Entity<korisnik>()
                .Property(e => e.kontaktEmailKorisnik)
                .IsUnicode(false);

            modelBuilder.Entity<korisnik>()
                .Property(e => e.kontaktTelefonKorisnik)
                .IsUnicode(false);

            modelBuilder.Entity<korisnik>()
                .Property(e => e.gradKorisnik)
                .IsUnicode(false);

            modelBuilder.Entity<korisnik>()
                .Property(e => e.županijaKorisnik)
                .IsUnicode(false);

            modelBuilder.Entity<korisnik>()
                .Property(e => e.ulicaKorisnik)
                .IsUnicode(false);

            modelBuilder.Entity<korisnik>()
                .Property(e => e.putanjaDoProfilneSlike)
                .IsUnicode(false);

            modelBuilder.Entity<korisnik>()
                .Property(e => e.korisničkoImeKorisnik)
                .IsUnicode(false);

            modelBuilder.Entity<odabranapoduzećaindex>()
                .Property(e => e.idOdabranaPoduzećaIndex)
                .IsUnicode(false);

            modelBuilder.Entity<odabranapoduzećaindex>()
                .Property(e => e.imePoduzeća)
                .IsUnicode(false);

            modelBuilder.Entity<odabranapoduzećaindex>()
                .Property(e => e.putanjaDoSlike)
                .IsUnicode(false);

            modelBuilder.Entity<odabranapoduzećaindex>()
                .Property(e => e.opisPoduzeća)
                .IsUnicode(false);

            modelBuilder.Entity<poduzeće>()
                .Property(e => e.idPoduzeće)
                .IsUnicode(false);

            modelBuilder.Entity<poduzeće>()
                .Property(e => e.imePoduzeće)
                .IsUnicode(false);

            modelBuilder.Entity<poduzeće>()
                .Property(e => e.opisPoduzeće)
                .IsUnicode(false);

            modelBuilder.Entity<poduzeće>()
                .Property(e => e.kontaktTelefon)
                .IsUnicode(false);

            modelBuilder.Entity<poduzeće>()
                .Property(e => e.kontaktEmail)
                .IsUnicode(false);

            modelBuilder.Entity<poduzeće>()
                .Property(e => e.primarnaDjelatnostPoduzeće)
                .IsUnicode(false);

            modelBuilder.Entity<poduzeće>()
                .Property(e => e.gradPoduzeće)
                .IsUnicode(false);

            modelBuilder.Entity<poduzeće>()
                .Property(e => e.županijaPoduzeće)
                .IsUnicode(false);

            modelBuilder.Entity<poduzeće>()
                .Property(e => e.ulicaPoduzeće)
                .IsUnicode(false);

            modelBuilder.Entity<poduzeće>()
                .Property(e => e.korisničkoImePoduzeće)
                .IsUnicode(false);

            modelBuilder.Entity<poduzeće>()
                .HasMany(e => e.images)
                .WithRequired(e => e.poduzeće)
                .HasForeignKey(e => e.imageIdByPoduzeće);

            modelBuilder.Entity<sportovi>()
                .Property(e => e.idSportovi)
                .IsUnicode(false);

            modelBuilder.Entity<sportovi>()
                .Property(e => e.imeSporta)
                .IsUnicode(false);

            modelBuilder.Entity<županije>()
                .Property(e => e.idŽupanija)
                .IsUnicode(false);

            modelBuilder.Entity<županije>()
                .Property(e => e.imeŽupanije)
                .IsUnicode(false);
        }
    }
}
