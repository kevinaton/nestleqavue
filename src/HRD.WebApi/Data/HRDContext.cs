using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using HRD.WebApi.Data.Entities;

namespace HRD.WebApi.Data
{
    public partial class HRDContext : DbContext
    {
        public HRDContext()
        {
        }

        public HRDContext(DbContextOptions<HRDContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DropDownItem> DropDownItems { get; set; }
        public virtual DbSet<DropDownType> DropDownTypes { get; set; }
        public virtual DbSet<Hrd> Hrds { get; set; }
        public virtual DbSet<Hrddc> Hrddcs { get; set; }
        public virtual DbSet<Hrdfc> Hrdfcs { get; set; }
        public virtual DbSet<Hrdnote> Hrdnotes { get; set; }
        public virtual DbSet<Hrdpo> Hrdpos { get; set; }
        public virtual DbSet<HrdtestCost> HrdtestCosts { get; set; }
        public virtual DbSet<LaborCost> LaborCosts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Security> Securities { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<TestCost> TestCosts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<HrdMicro> HrdMicros { get; set; }
        public virtual DbSet<RawMaterial> RawMaterials { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=localhost\\MSSQLDB;Initial Catalog=HRD;User=sa;Password=Super_Encrypted_Passw0rd!");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DropDownItem>(entity =>
            {
                entity.ToTable("DropDownItem");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.DropDownType)
                    .WithMany(p => p.DropDownItems)
                    .HasForeignKey(d => d.DropDownTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DropDownI__DropD__286302EC");
            });

            modelBuilder.Entity<DropDownType>(entity =>
            {
                entity.ToTable("DropDownType");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Hrd>(entity =>
            {
                entity.ToTable("HRD");

                entity.Property(e => e.AdditionalDescription).HasMaxLength(500);

                entity.Property(e => e.ApprovedByDistroyedWhen).HasColumnType("datetime");

                entity.Property(e => e.ApprovedByDistroyedWho).HasMaxLength(50);

                entity.Property(e => e.ApprovedByPlantControllerWhen).HasColumnType("datetime");

                entity.Property(e => e.ApprovedByPlantControllerWho).HasMaxLength(50);

                entity.Property(e => e.ApprovedByPlantManagerWho).HasMaxLength(50);

                entity.Property(e => e.ApprovedByQawhen)
                    .HasColumnType("datetime")
                    .HasColumnName("ApprovedByQAWhen");

                entity.Property(e => e.ApprovedByQawho)
                    .HasMaxLength(50)
                    .HasColumnName("ApprovedByQAWho");

                entity.Property(e => e.ApprovedPlantManagerQawhen)
                    .HasColumnType("datetime")
                    .HasColumnName("ApprovedPlantManagerQAWhen");

                entity.Property(e => e.Area).HasMaxLength(50);

                entity.Property(e => e.AreaIfOther).HasMaxLength(50);

                entity.Property(e => e.SMIVendorBatch).HasMaxLength(50);

                entity.Property(e => e.Bumanager)
                    .HasMaxLength(50)
                    .HasColumnName("BUManager");

                entity.Property(e => e.CasesHeld).HasMaxLength(50);

                entity.Property(e => e.Classification).HasMaxLength(50);

                entity.Property(e => e.CodingDetails).HasMaxLength(10);

                entity.Property(e => e.CodingType).HasMaxLength(20);

                entity.Property(e => e.Comments).HasMaxLength(500);

                entity.Property(e => e.ContinuousRun).HasMaxLength(10);

                entity.Property(e => e.CostofProductonHold).HasColumnType("money");

                entity.Property(e => e.DateCompleted).HasColumnType("datetime");

                entity.Property(e => e.DateHeld).HasColumnType("datetime");

                entity.Property(e => e.DateOfResample).HasColumnType("datetime");

                entity.Property(e => e.DateReceived).HasColumnType("datetime");

                entity.Property(e => e.DateReleased).HasColumnType("datetime");

                entity.Property(e => e.DateofDisposition).HasColumnType("datetime");

                entity.Property(e => e.DayCode).HasMaxLength(7);

                entity.Property(e => e.DayOfWeek).HasMaxLength(50);

                entity.Property(e => e.Dcdate)
                    .HasColumnType("datetime")
                    .HasColumnName("DCDate");

                entity.Property(e => e.Dcuser)
                    .HasMaxLength(50)
                    .HasColumnName("DCUser");

                entity.Property(e => e.DetailedDescription).HasMaxLength(int.MaxValue);

                entity.Property(e => e.Equipment).HasMaxLength(50);

                entity.Property(e => e.EquipmentIfOther).HasMaxLength(50);

                entity.Property(e => e.Fcdate)
                    .HasColumnType("datetime")
                    .HasColumnName("FCDate");

                entity.Property(e => e.Fcuser)
                    .HasMaxLength(50)
                    .HasColumnName("FCUser");

                entity.Property(e => e.FertDescription).HasMaxLength(50);

                entity.Property(e => e.Fmtype)
                    .HasMaxLength(50)
                    .HasColumnName("FMType");

                entity.Property(e => e.Ftqcases).HasColumnName("FTQCases");

                entity.Property(e => e.Globenum)
                    .HasMaxLength(10)
                    .HasColumnName("GLOBENum");

                entity.Property(e => e.Gstdrequired).HasColumnName("GSTDRequired");

                entity.Property(e => e.HoldCategory).HasMaxLength(50);

                entity.Property(e => e.HoldConcern).HasMaxLength(50);

                entity.Property(e => e.HoldSubCategory).HasMaxLength(50);

                entity.Property(e => e.HourCode).HasMaxLength(100);

                entity.Property(e => e.HrdcompletedBy)
                    .HasMaxLength(50)
                    .HasColumnName("HRDCompletedBy");

                entity.Property(e => e.IfYesAffectedProduct).HasMaxLength(50);

                entity.Property(e => e.InspectorsName).HasMaxLength(50);

                entity.Property(e => e.IsFm).HasColumnName("IsFM");

                entity.Property(e => e.IsHrd).HasColumnName("IsHRD");

                entity.Property(e => e.IsNr).HasColumnName("IsNR");

                entity.Property(e => e.IsSmi).HasColumnName("IsSMI");

                entity.Property(e => e.LaborHours).HasColumnType("numeric(10, 4)");

                entity.Property(e => e.LastChange).HasColumnType("datetime");

                entity.Property(e => e.LastChangeUser).HasMaxLength(100);

                entity.Property(e => e.Lcl)
                    .HasMaxLength(50)
                    .HasColumnName("LCL");

                entity.Property(e => e.Line).HasMaxLength(50);

                entity.Property(e => e.LineSupervisor).HasMaxLength(50);

                entity.Property(e => e.MaterialNumber).HasMaxLength(50);

                entity.Property(e => e.MeatComponent).HasMaxLength(50);

                entity.Property(e => e.NonFtqcases).HasColumnName("NonFTQCases");

                entity.Property(e => e.Nrcategory)
                    .HasMaxLength(50)
                    .HasColumnName("NRCategory");

                entity.Property(e => e.Originator).HasMaxLength(50);

                entity.Property(e => e.OtherHrdaffected).HasColumnName("OtherHRDAffected");

                entity.Property(e => e.OtherHrdnum)
                    .HasMaxLength(50)
                    .HasColumnName("OtherHRDNum");

                entity.Property(e => e.PcocontactedImmediately)
                    .HasMaxLength(50)
                    .HasColumnName("PCOContactedImmediately");

                entity.Property(e => e.PestType).HasMaxLength(50);

                entity.Property(e => e.Physical5DaysGstd).HasColumnName("Physical5DaysGSTD");

                entity.Property(e => e.PiecesTotal).HasMaxLength(50);

                entity.Property(e => e.Plant)
                    .HasMaxLength(4)
                    .IsFixedLength();

                entity.Property(e => e.Pos)
                    .HasMaxLength(50)
                    .HasColumnName("POs");

                entity.Property(e => e.ProductAdultered).HasMaxLength(50);

                entity.Property(e => e.Qacomments).HasColumnName("QAComments");

                entity.Property(e => e.FMVendorBatch).HasMaxLength(50);

                entity.Property(e => e.RawMaterialDescription).HasMaxLength(50);

                entity.Property(e => e.ReasonAction).HasMaxLength(50);

                entity.Property(e => e.Response).HasMaxLength(50);

                entity.Property(e => e.FMSource).HasMaxLength(50);

                entity.Property(e => e.ReworkApprovedBy).HasMaxLength(50);

                entity.Property(e => e.ReworkApprovedDate).HasColumnType("datetime");

                entity.Property(e => e.ReworkComplete).HasColumnType("datetime");

                entity.Property(e => e.ReworkCompletedBy).HasMaxLength(50);

                entity.Property(e => e.ReworkStarted).HasColumnType("datetime");

                entity.Property(e => e.Rohmaterial)
                    .HasMaxLength(50)
                    .HasColumnName("ROHMaterial");

                entity.Property(e => e.SauceType).HasMaxLength(50);

                entity.Property(e => e.Schedule).HasMaxLength(50);

                entity.Property(e => e.Shift).HasMaxLength(50);

                entity.Property(e => e.ShortDescription).HasMaxLength(75);

                entity.Property(e => e.Size).HasMaxLength(50);

                entity.Property(e => e.StarchType).HasMaxLength(50);

                entity.Property(e => e.TagNumber).HasMaxLength(50);

                entity.Property(e => e.Tagged).HasMaxLength(50);

                entity.Property(e => e.TestingCost).HasColumnType("money");

                entity.Property(e => e.TlforFu)
                    .HasMaxLength(50)
                    .HasColumnName("TLforFU");

                entity.Property(e => e.VeggieComponent).HasMaxLength(50);

                entity.Property(e => e.VendorName).HasMaxLength(50);

                entity.Property(e => e.VendorNumber).HasMaxLength(50);

                entity.Property(e => e.VendorSiteNumber).HasMaxLength(50);

                entity.Property(e => e.When).HasMaxLength(50);

                entity.Property(e => e.WhenOther).HasMaxLength(50);

                entity.Property(e => e.WhereFound).HasMaxLength(50);

                entity.Property(e => e.YearHeld).HasMaxLength(4);

                entity.Property(e => e.YearOfIncident).HasMaxLength(4);
            });

            modelBuilder.Entity<Hrddc>(entity =>
            {
                entity.ToTable("HRDDC");

                entity.Property(e => e.Hrdid).HasColumnName("HRDId");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Hrd)
                    .WithMany(p => p.Hrddcs)
                    .HasForeignKey(d => d.Hrdid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HRDDC__HRDId__30F848ED");
            });

            modelBuilder.Entity<Hrdfc>(entity =>
            {
                entity.ToTable("HRDFC");

                entity.Property(e => e.Hrdid).HasColumnName("HRDId");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Hrd)
                    .WithMany(p => p.Hrdfcs)
                    .HasForeignKey(d => d.Hrdid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HRDFC__HRDId__2E1BDC42");
            });

            modelBuilder.Entity<Hrdnote>(entity =>
            {
                entity.ToTable("HRDNotes");

                entity.Property(e => e.Category).HasMaxLength(50);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.FileName).HasMaxLength(400);

                entity.Property(e => e.Hrdid).HasColumnName("HRDId");

                entity.Property(e => e.Path).HasMaxLength(400);

                entity.Property(e => e.Size).HasMaxLength(20);

                entity.Property(e => e.UserId)
                    .HasMaxLength(50)
                    .HasColumnName("UserID");

                entity.HasOne(d => d.Hrd)
                    .WithMany(p => p.Hrdnotes)
                    .HasForeignKey(d => d.Hrdid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HRDNotes__HRDId__398D8EEE");
            });

            modelBuilder.Entity<Hrdpo>(entity =>
            {
                entity.ToTable("HRDPO");

                entity.Property(e => e.Hrdid).HasColumnName("HRDId");

                entity.Property(e => e.Ponumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("PONumber");

                entity.HasOne(d => d.Hrd)
                    .WithMany(p => p.Hrdpos)
                    .HasForeignKey(d => d.Hrdid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HRDPO__HRDId__2B3F6F97");
            });

            modelBuilder.Entity<HrdtestCost>(entity =>
            {
                entity.ToTable("HRDTestCost");

                entity.Property(e => e.Cost).HasColumnType("money");

                entity.Property(e => e.Hrdid).HasColumnName("HRDId");

                entity.Property(e => e.TestName).HasMaxLength(50);

                entity.HasOne(d => d.Hrd)
                    .WithMany(p => p.HrdtestCosts)
                    .HasForeignKey(d => d.Hrdid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HRDTestCo__HRDId__4222D4EF");
            });

            modelBuilder.Entity<LaborCost>(entity =>
            {
                entity.HasKey(e => e.Year);

                entity.ToTable("LaborCost");

                entity.Property(e => e.Year)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.LaborCostValue)
                    .HasColumnType("smallmoney")
                    .HasColumnName("LaborCost");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.CostPerCase).HasColumnType("money");

                entity.Property(e => e.Country)
                    .HasMaxLength(2)
                    .IsFixedLength();

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Gpn)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("GPN")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NoBbdate).HasColumnName("NoBBDate");

                entity.Property(e => e.Year)
                    .IsRequired()
                    .HasMaxLength(4)
                    .HasDefaultValueSql("((0))")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Security>(entity =>
            {
                entity.ToTable("Security");

                entity.Property(e => e.Administrator).HasDefaultValueSql("((0))");

                entity.Property(e => e.Bumgrp)
                    .HasColumnName("BUMGrp")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DeleteHrd)
                    .HasColumnName("DeleteHRD")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.EditHrdapproveRework)
                    .HasColumnName("EditHRDApproveRework")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Gstdgrp)
                    .HasColumnName("GSTDGrp")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.GstdnotificationGrp)
                    .HasColumnName("GSTDNotificationGrp")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LaborCostList).HasDefaultValueSql("((0))");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Notification).HasDefaultValueSql("((0))");

                entity.Property(e => e.Physical5Grp).HasDefaultValueSql("((0))");

                entity.Property(e => e.PhysicalGrp).HasDefaultValueSql("((0))");

                entity.Property(e => e.ProdMaintenanceList).HasDefaultValueSql("((0))");

                entity.Property(e => e.ProdTlgrp)
                    .HasColumnName("ProdTLGrp")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Qamanager)
                    .HasColumnName("QAManager")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.QaprodRelGrp)
                    .HasColumnName("QAProdRelGrp")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Qatgrp)
                    .HasColumnName("QATGrp")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ReworkMembersGrp).HasDefaultValueSql("((0))");

                entity.Property(e => e.Type).HasMaxLength(5);
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(25);

                entity.Property(e => e.Type).HasMaxLength(10);

                entity.Property(e => e.Value).HasMaxLength(255);
            });

            modelBuilder.Entity<TestCost>(entity =>
            {
                entity.ToTable("TestCost");

                entity.Property(e => e.TestCostValue)
                    .HasColumnType("money")
                    .HasColumnName("TestCost");

                entity.Property(e => e.TestName).HasMaxLength(50);

                entity.Property(e => e.Year).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.UserId)
                    .HasMaxLength(50)
                    .HasColumnName("UserID");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasData(
                    new Role { Id = 1, Name = "Admin", DisplayName="Admin" },
                    new Role { Id = 2, Name = "DataEntry", DisplayName = "Data Entry" },
                    new Role { Id = 3, Name = "ReportViewer", DisplayName = "Report Viewer" });
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId })
                    .HasName("PK__UserRole__AF2760ADD67758F1");

                entity.ToTable("UserRole");
            });

            modelBuilder.Entity<HrdMicro>(entity =>
            {
                entity.ToTable("HRDMicro");

                entity.Property(e => e.HrdId).HasColumnName("HRDId");

                entity.Property(e => e.Organism).HasMaxLength(50);

                entity.HasOne(d => d.Hrd)
                    .WithMany(p => p.HrdMicros)
                    .HasForeignKey(d => d.HrdId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<RawMaterial>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("RawMaterials");

                entity.Property(e => e.Id)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Description)
                    .HasColumnName("Description");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
