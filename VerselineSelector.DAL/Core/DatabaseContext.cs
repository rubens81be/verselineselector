using VerselineSelector.Domain.ChapterIV;
using VerselineSelector.Domain.Patient;

namespace VerselineSelector.DAL.Core;

public class DatabaseContext : DbContext
{
    public DbSet<VerselineEntity> Verselines => Set<VerselineEntity>();

    public DbSet<ParagraphEntity> Paragraphs => Set<ParagraphEntity>();

    public DbSet<PatientEntity> Patients => Set<PatientEntity>();

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VerselineEntity>().ToTable("SAMV2_VERSE").HasKey(c => new { c.ParagraphName, c.Sequence });
        modelBuilder.Entity<VerselineEntity>().Property(c => c.ParagraphName).HasColumnName("PARAGRAPH");
        modelBuilder.Entity<VerselineEntity>().Property(c => c.VerselineNumber).HasColumnName("NUMBER");
        modelBuilder.Entity<VerselineEntity>().Property(c => c.ParentSequence).HasColumnName("PARENT");
        modelBuilder.Entity<VerselineEntity>().Property(c => c.HasCheckbox).HasColumnName("CHECKBOX");
        modelBuilder.Entity<VerselineEntity>().Property(c => c.MinimumChecks).HasColumnName("MINCHECK");
        modelBuilder.Entity<VerselineEntity>().Property(c => c.MinimumAge).HasColumnName("MINAGEAUTHORIZED");
        modelBuilder.Entity<VerselineEntity>().Property(c => c.MinimumAgeUnit).HasColumnName("MINAGEAUTHUNIT");
        modelBuilder.Entity<VerselineEntity>().Property(c => c.MaximumAge).HasColumnName("MAXAGEAUTHORIZED");
        modelBuilder.Entity<VerselineEntity>().Property(c => c.MaximumAgeUnit).HasColumnName("MAXAGEAUTHUNIT");
        modelBuilder.Entity<VerselineEntity>().Property(c => c.AnnexMandatory).HasColumnName("OTHERADDEDDOC");
        modelBuilder.Entity<VerselineEntity>().Property(c => c.SexRestriction).HasColumnName("SEXRESTRICTED");
        modelBuilder.Entity<VerselineEntity>().Property(c => c.Text).HasColumnName("TEXT_NL");
        modelBuilder.Entity<VerselineEntity>().HasOne(c => c.Parent).WithMany(c => c.Children).HasForeignKey(c => new { c.ParagraphName, c.ParentSequence });
        modelBuilder.Entity<VerselineEntity>().HasOne(c => c.Paragraph).WithMany(c => c.Verselines).HasForeignKey(c => c.ParagraphName );

        modelBuilder.Entity<VerselineEntity>().Property(v => v.SexRestriction).HasConversion(
            v => v.Equals(SexType.Undefined) ? string.Empty : ((char)v).ToString(),
            v => string.IsNullOrEmpty(v) ? SexType.Undefined : (SexType)(int)v.ToCharArray()[0]);
        modelBuilder.Entity<VerselineEntity>().Property(v => v.MinimumAgeUnit).HasConversion(
            v => v.Equals(AgeUnitType.Undefined) ? string.Empty : ((char)v).ToString(),
            v => string.IsNullOrEmpty(v) ? AgeUnitType.Undefined : (AgeUnitType)(int)v.ToCharArray()[0]);
        modelBuilder.Entity<VerselineEntity>().Property(v => v.MaximumAgeUnit).HasConversion(
            v => v.Equals(AgeUnitType.Undefined) ? string.Empty : ((char)v).ToString(),
            v => string.IsNullOrEmpty(v) ? AgeUnitType.Undefined : (AgeUnitType)(int)v.ToCharArray()[0]);
        modelBuilder.Entity<VerselineEntity>().Property(v => v.Type).HasConversion(
            v => v.Equals(VerselineType.Undefined) ? string.Empty : ((char)v).ToString(),
            v => string.IsNullOrEmpty(v) ? VerselineType.Undefined : (VerselineType)(int)v.ToCharArray()[0]);

        modelBuilder.Entity<ParagraphEntity>().ToView("SAMV2_PARAGRAPHVIEW").HasKey(p => p.Name);
        modelBuilder.Entity<ParagraphEntity>().HasMany(p => p.Verselines).WithOne(v => v.Paragraph).HasForeignKey(d => d.ParagraphName);
        modelBuilder.Entity<ParagraphEntity>().HasMany(p => p.Documents).WithOne(p => p.Paragraph).HasForeignKey(d => d.ParagraphName);
        modelBuilder.Entity<ParagraphEntity>().Navigation(p => p.Verselines).UsePropertyAccessMode(PropertyAccessMode.Property).AutoInclude();
        modelBuilder.Entity<ParagraphEntity>().Navigation(p => p.Documents).UsePropertyAccessMode(PropertyAccessMode.Property).AutoInclude();
        modelBuilder.Entity<ParagraphEntity>().Property(p => p.Name).HasColumnName("PARAGRAPH");
        modelBuilder.Entity<ParagraphEntity>().Property(p => p.Description).HasColumnName("KEYSTRING");
        modelBuilder.Entity<ParagraphEntity>().Property(p => p.AuthorisationType).HasColumnName("AGREEMENTTYPE").HasConversion(
            v => v.Equals(AuthorisationType.Undefined) ? string.Empty : ((char?)v).ToString(),
            v => string.IsNullOrEmpty(v) ? AuthorisationType.Undefined : (AuthorisationType)(int)v.ToCharArray()[0]);
        modelBuilder.Entity<ParagraphEntity>().Property(p => p.ProcessType).HasColumnName("PROCESSTYPE").HasConversion(
            v => ((char?)v).ToString(), 
            v => string.IsNullOrEmpty(v) ? ParagaphProcessType.NotApplicable : (ParagaphProcessType)(int)v.ToCharArray()[0]);
        modelBuilder.Entity<ParagraphEntity>().Property(p => p.Version).HasColumnName("VERSION");

        modelBuilder.Entity<DocumentEntity>().ToView("SAMV2_DOCUMENTVIEW").HasKey(c => c.ParagraphName);
        modelBuilder.Entity<DocumentEntity>().HasOne(d => d.Verseline).WithMany(v => v.Documents).HasForeignKey(d => new { d.ParagraphName, d.VerselineSequence });
        modelBuilder.Entity<DocumentEntity>().HasOne(d => d.Paragraph).WithMany(p => p.Documents).HasForeignKey(d => d.ParagraphName);
        modelBuilder.Entity<DocumentEntity>().Navigation(d => d.Verseline).UsePropertyAccessMode(PropertyAccessMode.Property).AutoInclude();
        modelBuilder.Entity<DocumentEntity>().Navigation(d => d.Paragraph).UsePropertyAccessMode(PropertyAccessMode.Property).AutoInclude();
        modelBuilder.Entity<DocumentEntity>().Property(d => d.ParagraphName).HasColumnName("Paragraph");
        modelBuilder.Entity<DocumentEntity>().Property(d => d.VerselineSequence).HasColumnName("VerselineSequence");
        modelBuilder.Entity<DocumentEntity>().Property(d => d.DocumentSequence).HasColumnName("DocumentSequence");
        modelBuilder.Entity<DocumentEntity>().Property(d => d.Uri).HasColumnName("URI");
        
        modelBuilder.Entity<PatientEntity>().ToTable("DUMMY_PATIENT").HasKey(t => t.PatientNumber);
        modelBuilder.Entity<PatientEntity>().Property(p => p.DateOfBirth).HasConversion(
            v => v.ToDateTime(TimeOnly.MinValue),
            v => DateOnly.FromDateTime(v));
        modelBuilder.Entity<PatientEntity>().Property(p => p.Sex).HasConversion(
            v => v.Equals(SexType.Undefined) ? string.Empty : ((char)v).ToString(),
            v => string.IsNullOrEmpty(v) ? SexType.Undefined : (SexType)(int)v.ToCharArray()[0]);
    }
}
