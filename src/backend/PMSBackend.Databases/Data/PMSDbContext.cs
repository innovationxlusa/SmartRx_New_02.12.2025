using Microsoft.EntityFrameworkCore;
using PMSBackend.Domain.Entities;

namespace PMSBackend.Databases.Data
{
    public class PMSDbContext : DbContext
    {
        //DbConnector connector;
        public PMSDbContext(DbContextOptions<PMSDbContext> options) : base(options)
        {

        }

        public DbSet<SmartRxUserEntity> Security_PMSUsers { get; set; }
        public DbSet<SmartRxRoleEntity> Security_Roles { get; set; }
        public DbSet<SmartRxUserRoleEntity> Security_UserRoles { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<Prescription_UploadEntity> Prescription_UploadedPrescription { get; set; }
        public DbSet<Prescription_UserWiseFolderEntity> Prescription_UserWiseFolders { get; set; }


        public DbSet<Configuration_DistrictEntity> Configuration_District { get; set; }
        public DbSet<Configuration_PoliceStationEntity> Configuration_PoliceStation { get; set; }
        public DbSet<Configuration_CityEntity> Configuration_City { get; set; }
        public DbSet<Configuration_CountryEntity> Configuration_Country { get; set; }

        public DbSet<Configuration_HospitalEntity> Configuration_Hospital { get; set; }

        public DbSet<Configuration_DepartmentSectionEntity> Configuration_DepartmentSection { get; set; }
        public DbSet<Configuration_DepartmentEntity> Configuration_Department { get; set; }
        public DbSet<Configuration_DesignationEntity> Configuration_Designation { get; set; }


        public DbSet<Configuration_UnitEntity> Configuration_Unit { get; set; }
        public DbSet<Configuration_VitalEntity> Configuration_Vital { get; set; }
        //public DbSet<Configuration_ChamberEntity> Configuration_Chamber { get; set; }
        public DbSet<Configuration_PrescriptionSectionsEntity> Prescription_Scetion { get; set; }
        public DbSet<Configuration_ChiefComplaintEntity> Configuration_ChiefComplaint { get; set; }
        public DbSet<Configuration_DoctorEntity> Configuration_Doctor { get; set; }
        public DbSet<Configuration_DoctorChamberEntity> Configuration_DoctorChamber { get; set; }
        public DbSet<Configuration_EducationEntity> Configuration_Education { get; set; }
        public DbSet<Configuration_SmartRxAcronymEntity> Configuration_Acronym { get; set; }
        public DbSet<Configuration_AdviceFAQEntity> Configuration_AdviceFAQ { get; set; }
        public DbSet<Configuration_TagsEntity> Configuration_Tags { get; set; }
        public DbSet<Configuration_VitalFAQEntity> Configuration_VitalFAQ { get; set; }
        public DbSet<Configuration_InvestigationFAQEntitiy> Configuration_InvestigationFAQ { get; set; }
        public DbSet<Configuration_UserActivityEntity> Configuration_UserActivity { get; set; }
        public DbSet<Configuration_RewardRuleEntity> Configuration_RewardRule { get; set; }


        public DbSet<Configuration_MedicineDosageFormEntity> Configuration_MedicineDosageForm { get; set; }
        public DbSet<Configuration_MedicineGenericEntity> Configuration_MedicineGeneric { get; set; }
        public DbSet<Configuration_MedicineBrandEntity> Configuration_MedicineBrand { get; set; }
        public DbSet<Configuration_MedicineFAQEntity> Configuration_MedicineFAQ { get; set; }
        public DbSet<Configuration_MedicineManufactureInfoEntity> Configuration_MedicineManufactureInfo { get; set; }
        public DbSet<Configuration_MedicineEntity> Configuration_Medicine { get; set; }
        public DbSet<Configuration_InvestigationEntity> Configuration_Investigation { get; set; }
        public DbSet<Configuration_DiagnosisCenterWiseTestEntity> Configuration_DiagnosisCenterWiseTest { get; set; }


        public DbSet<SmartRx_PatientProfileEntity> Smartrx_PatientProfile { get; set; }
        public DbSet<SmartRx_MasterEntity> Smartrx_Master { get; set; }
        public DbSet<SmartRx_PatientChiefComplaintEntity> Smartrx_ChiefComplaint { get; set; }
        public DbSet<SmartRx_PatientVitalsEntity> Smartrx_Vital { get; set; }
        public DbSet<SmartRx_PatientDoctorEntity> Smartrx_Doctor { get; set; }
        public DbSet<SmartRx_PatientHistoryEntity> Smartrx_PatientHistory { get; set; }
        public DbSet<SmartRx_PatientRelativesEntity> Smartrx_PatientRelatives { get; set; }
        public DbSet<SmartRx_ReferredConsultantEntity> Smartrx_ReferredConsultant { get; set; }
        public DbSet<SmartRx_PatientMedicineEntity> SmartRx_PatientMedicine { get; set; }
        public DbSet<SmartRx_PatientInvestigationEntity> SmartRx_PatientInvestigation { get; set; }
        public DbSet<SmartRx_PatientAdviceEntity> SmartRx_PatientAdvice { get; set; }
        public DbSet<SmartRx_PatientWishlistEntity> SmartRx_PatientWishList { get; set; }
        public DbSet<SmartRx_PatientOtherExpenseEntity> SmartRx_PatientOtherExpenses { get; set; }

        public DbSet<Configuration_RewardBadgeEntity> Configuration_RewardBadge { get; set; }
        public DbSet<Configuration_Reward> Configuration_Reward { get; set; }
        public DbSet<SmartRx_PatientReward> SmartRx_PatientReward { get; set; }
        public DbSet<SmartRx_RewardPointConversionsEntity> SmartRx_RewardPointConversions { get; set; }
        public DbSet<Configuration_Settings> Configuration_Settings { get; set; }
        public DbSet<SmartRx_RewardTransactionEntity> SmartRx_RewardTransaction { get; set; }
        public DbSet<SmartRx_Reward_UserBalancesEntity> SmartRx_Reward_UserBalances { get; set; }
        public DbSet<SmartRx_UserRewardBadgeEntity> SmartRx_UserRewardBadge { get; set; }


        


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //        .UseSqlServer("PMSDBConnection")
        //        .ConfigureWarnings(warnings =>
        //            warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SmartRxUserEntity>().Ignore(x => x.CreatedBy);
            modelBuilder.Entity<SmartRxUserEntity>().Ignore(x => x.ModifiedBy);

            // RefreshToken configuration
            modelBuilder.Entity<RefreshTokenEntity>()
                .HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SmartRxRoleEntity>()
                .HasOne(x => x.CreatedBy) // navigation property
                .WithMany() // or WithMany(x => x.UserRolesCreated) if reverse nav exists
                .HasForeignKey("CreatedById") // assumes a property CreatedById exists
                .OnDelete(DeleteBehavior.Restrict); // optional
            modelBuilder.Entity<SmartRxRoleEntity>()
                .HasOne(x => x.ModifiedBy) // navigation property
                .WithMany() // or WithMany(x => x.UserRolesCreated) if reverse nav exists
                .HasForeignKey("ModifiedById") // assumes a property CreatedById exists
                .OnDelete(DeleteBehavior.Restrict); // optional

            // Many-to-Many User <-> Role (through UserRole)
            modelBuilder.Entity<SmartRxUserRoleEntity>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<SmartRxUserRoleEntity>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<SmartRxUserRoleEntity>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
            modelBuilder.Entity<SmartRxUserRoleEntity>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRxUserRoleEntity>()
                .HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey("ModifiedById")
                .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<UserRoleEntity>().ToTable("UserRoles");           



            modelBuilder.Entity<Configuration_VitalEntity>()
                .HasOne(s => s.Unit)
                .WithMany()
                .HasForeignKey(s => s.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Configuration_VitalEntity>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Configuration_VitalEntity>()
                .HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey("ModifiedById")
                .OnDelete(DeleteBehavior.Restrict);



            //modelBuilder.Entity<Prescription_UserWiseFolderEntity>()
            //    .HasOne(ur => ur.User)
            //    .WithMany(u => u.UserWiseFolders)
            //    .HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<Prescription_UserWiseFolderEntity>()
              .HasOne(s => s.PatientProfile)
              .WithMany()
              .HasForeignKey(s => s.PatientId)
              .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Prescription_UserWiseFolderEntity>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Prescription_UserWiseFolderEntity>()
                .HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey("ModifiedById")
                .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Prescription_UserWiseFolderEntity>()
            //   .HasMany(f => f.SubFolders)
            //   .WithOne(f => f.ParentFolder)
            //   .HasForeignKey(f => f.ParentFolderId)
            //   .OnDelete(DeleteBehavior.Restrict);
            // modelBuilder.Entity<UserWiseFolderEntity>().ToTable("UserWiseFolders");


            //modelBuilder.Entity<Configuration_DoctorEducationDegreeEntity>()
            //   .HasOne(s => s.Doctor)
            //   .WithMany()
            //   .HasForeignKey(s => s.DoctorId)
            //   .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Configuration_DoctorEducationDegreeEntity>()
            //  .HasOne(s => s.Education)
            //  .WithMany()
            //  .HasForeignKey(s => s.EducationId)
            //  .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Configuration_DoctorEducationDegreeEntity>()
            //    .HasOne(x => x.CreatedBy)
            //    .WithMany()
            //    .HasForeignKey("CreatedById")
            //    .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Configuration_DoctorEducationDegreeEntity>()
            //    .HasOne(x => x.ModifiedBy)
            //    .WithMany()
            //    .HasForeignKey("ModifiedById")
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Configuration_DepartmentEntity>()
               .HasOne(s => s.DepartmentSection)
               .WithMany()
               .HasForeignKey(s => s.SectionId)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Configuration_DepartmentEntity>()
              .HasOne(s => s.Hospital)
              .WithMany()
              .HasForeignKey(s => s.HospitalId)
              .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Configuration_DepartmentEntity>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Configuration_DepartmentEntity>()
                .HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey("ModifiedById")
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Configuration_DoctorChamberEntity>()
               .HasOne(s => s.Doctor)
               .WithMany()
               .HasForeignKey(s => s.DoctorId)
               .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Configuration_DoctorChamberEntity>()
            //   .HasOne(s => s.Chamber)
            //   .WithMany()
            //   .HasForeignKey(s => s.ChamberId)
            //   .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Configuration_DoctorChamberEntity>()
                .HasOne(s => s.Department)
                .WithMany()
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Configuration_DoctorChamberEntity>()
                .HasOne(s => s.DepartmentSection)
                .WithMany()
                .HasForeignKey(s => s.DepartmentSectionId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Configuration_DoctorChamberEntity>()
                .HasOne(s => s.Hospital)
                .WithMany()
                .HasForeignKey(s => s.HospitalId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Configuration_DoctorChamberEntity>()
              .HasOne(s => s.DoctorDesignationInChamber)
              .WithMany()
              .HasForeignKey(s => s.DoctorDesignationInChamberId)
              .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Configuration_DoctorChamberEntity>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Configuration_DoctorChamberEntity>()
                .HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey("ModifiedById")
                .OnDelete(DeleteBehavior.Restrict);


            //modelBuilder.Entity<Configuration_HospitalEntity>()
            //    .HasMany(h => h.DepartmentSections)
            //    .WithOne(ds => ds.Hospital)
            //    .HasForeignKey(ds => ds.HospitalId);

            modelBuilder.Entity<Configuration_HospitalEntity>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Configuration_HospitalEntity>()
                .HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey("ModifiedById")
                .OnDelete(DeleteBehavior.Restrict); // optional

            modelBuilder.Entity<Configuration_DepartmentSectionEntity>()
                .HasMany(ds => ds.Departments)
                .WithOne(d => d.DepartmentSection)
                .HasForeignKey(d => d.SectionId);
            modelBuilder.Entity<Configuration_DepartmentSectionEntity>()
               .HasOne(x => x.CreatedBy)
               .WithMany()
               .HasForeignKey("CreatedById")
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Configuration_DepartmentSectionEntity>()
                .HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey("ModifiedById")
                .OnDelete(DeleteBehavior.Restrict); // optional

            //modelBuilder.Entity<Configuration_DepartmentEntity>()
            //    .HasMany(d => d.Chambers)
            //    .WithOne(c => c.Department)
            //    .HasForeignKey(c => c.DepartmentId);
            modelBuilder.Entity<Configuration_DepartmentEntity>()
               .HasOne(x => x.CreatedBy)
               .WithMany()
               .HasForeignKey("CreatedById")
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Configuration_DepartmentEntity>()
                .HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey("ModifiedById")
                .OnDelete(DeleteBehavior.Restrict); // optional


            modelBuilder.Entity<SmartRx_PatientProfileEntity>()
                .HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict); // or .NoAction()
            modelBuilder.Entity<SmartRx_PatientProfileEntity>()
                .HasOne(p => p.PoliceStation)
                .WithMany()
                .HasForeignKey(p => p.PoliceStationId)
                .OnDelete(DeleteBehavior.Restrict); // or .NoAction()
            modelBuilder.Entity<SmartRx_PatientProfileEntity>()
                .HasOne(p => p.ModifiedBy)
                .WithMany()
                .HasForeignKey(p => p.ModifiedById)
                .OnDelete(DeleteBehavior.Restrict); // or .NoAction()


            modelBuilder.Entity<Prescription_UploadEntity>()
                .HasOne(p => p.SmartRx)
                .WithMany()
                .HasForeignKey(p => p.SmartRxId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prescription_UploadEntity>()
                .HasOne(p => p.PatientProfile)
                .WithMany()
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.Restrict); // Or .NoAction()         
            modelBuilder.Entity<Prescription_UploadEntity>()
               .HasOne(p => p.Folder)
               .WithMany()
               .HasForeignKey(p => p.FolderId)
               .OnDelete(DeleteBehavior.Restrict); // Or .NoAction()
            modelBuilder.Entity<Prescription_UploadEntity>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Or .NoAction()
            modelBuilder.Entity<Prescription_UploadEntity>()
                .HasOne(p => p.ApprovedBy)
                .WithMany()
                .HasForeignKey(p => p.ApprovedById)
                .OnDelete(DeleteBehavior.Restrict); // or .NoAction()
            modelBuilder.Entity<Prescription_UploadEntity>()
                .HasOne(p => p.CompletedBy)
                .WithMany()
                .HasForeignKey(p => p.CompletedById)
                .OnDelete(DeleteBehavior.Restrict); // or .NoAction()
            modelBuilder.Entity<Prescription_UploadEntity>()
                .HasOne(p => p.LockedBy)
                .WithMany()
                .HasForeignKey(p => p.LockedById)
                .OnDelete(DeleteBehavior.Restrict); // or .NoAction()
            modelBuilder.Entity<Prescription_UploadEntity>()
                .HasOne(p => p.RecommendedBy)
                .WithMany()
                .HasForeignKey(p => p.RecommendedById)
                .OnDelete(DeleteBehavior.Restrict); // or .NoAction()
            modelBuilder.Entity<Prescription_UploadEntity>()
                .HasOne(p => p.ReportBy)
                .WithMany()
                .HasForeignKey(p => p.ReportById)
                .OnDelete(DeleteBehavior.Restrict); // or .NoAction()
            modelBuilder.Entity<Prescription_UploadEntity>()
               .HasOne(x => x.CreatedBy)
               .WithMany()
               .HasForeignKey("CreatedById")
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Prescription_UploadEntity>()
                .HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey("ModifiedById")
                .OnDelete(DeleteBehavior.Restrict); // optional


            modelBuilder.Entity<SmartRx_MasterEntity>()
                .HasOne(p => p.Prescription)
                .WithMany()
                .HasForeignKey(p => p.PrescriptionId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_MasterEntity>()
                .HasOne(p => p.PatientProfile)
                .WithMany()
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_MasterEntity>()
                .HasOne(x => x.ReportBy)
                .WithMany()
                .HasForeignKey("ReportById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_MasterEntity>()
                .HasOne(x => x.LockedBy)
                .WithMany()
                .HasForeignKey("LockedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_MasterEntity>()
                .HasOne(x => x.RecommendedBy)
                .WithMany()
                .HasForeignKey("RecommendedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_MasterEntity>()
                .HasOne(x => x.ApprovedBy)
                .WithMany()
                .HasForeignKey("ApprovedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_MasterEntity>()
                .HasOne(x => x.CompletedBy)
                .WithMany()
                .HasForeignKey("CompletedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_MasterEntity>()
                .HasOne(x => x.RejectedBy)
                .WithMany()
                .HasForeignKey("RejectedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientProfileEntity>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientProfileEntity>()
                .HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey("ModifiedById")
                .OnDelete(DeleteBehavior.Restrict); // optional
            modelBuilder.Entity<SmartRx_MasterEntity>()
                .Property(e => e.DiscountPercentageOnMedicineByDoctor)
                .HasPrecision(5, 2);


            //modelBuilder.Entity<SmartRx_PatientProfileEntity>()
            //    .HasMany(p => p.Prescriptions)
            //    .WithOne(p => p.PatientProfile)
            //    .HasForeignKey(p => p.PatientId)
            //    .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientProfileEntity>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientProfileEntity>()
                .HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey("ModifiedById")
                .OnDelete(DeleteBehavior.Restrict); // optional


            //modelBuilder.Entity<SmartRx_PatientChiefComplaintEntity>()
            //    .HasOne(x => x.Configuration_ChiefComplaint)
            //    .WithMany()
            //    .HasForeignKey("ChiefComplaintId")
            //    .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientChiefComplaintEntity>()
                .HasOne(x => x.SmartRx_Master)
                .WithMany()
                .HasForeignKey("SmartRxMasterId")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientChiefComplaintEntity>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientChiefComplaintEntity>()
                .HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey("ModifiedById")
                .OnDelete(DeleteBehavior.Restrict); // optional


            modelBuilder.Entity<SmartRx_PatientDoctorEntity>()
                .HasOne(x => x.ChamberFeeMeasurementUnit)
                .WithMany()
                .HasForeignKey("ChamberFeeMeasurementUnitId")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientDoctorEntity>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientDoctorEntity>()
                .HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey("ModifiedById")
                .OnDelete(DeleteBehavior.Restrict); // optional


            modelBuilder.Entity<SmartRx_PatientVitalsEntity>()
               .HasOne(pv => pv.SmartRxMaster)
               .WithMany(p => p.PatientVitals)
               .HasForeignKey(pv => pv.SmartRxMasterId)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientVitalsEntity>()
               .HasOne(x => x.PatientProfile)
               .WithMany()
               .HasForeignKey("PatientId")
               .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<SmartRx_PatientVitalsEntity>()
            //   .HasOne(pv => pv.Vital)
            //   .WithMany(v => v.PatientVitals)
            //   .HasForeignKey(pv => pv.VitalId)
            //   .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<SmartRx_PatientVitalsEntity>()
              .HasOne(x => x.Prescription)
              .WithMany()
              .HasForeignKey("PrescriptionId")
              .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientVitalsEntity>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientVitalsEntity>()
                .HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey("ModifiedById")
                .OnDelete(DeleteBehavior.Restrict); // optional


            modelBuilder.Entity<SmartRx_PatientRelativesEntity>()
              .HasOne(x => x.PatientProfile)
              .WithMany()
              .HasForeignKey("PatientId")
              .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientRelativesEntity>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientRelativesEntity>()
                .HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey("ModifiedById")
                .OnDelete(DeleteBehavior.Restrict); // optional


            modelBuilder.Entity<SmartRx_ReferredConsultantEntity>()
                .HasOne(x => x.SmartRxMaster)
                .WithMany()
                .HasForeignKey("SmartRxMasterId")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_ReferredConsultantEntity>()
                .HasOne(x => x.ReferredConsultant)
                .WithMany()
                .HasForeignKey("ReferredConsultantId")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_ReferredConsultantEntity>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_ReferredConsultantEntity>()
                .HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey("ModifiedById")
                .OnDelete(DeleteBehavior.Restrict); // optional



            modelBuilder.Entity<Configuration_CityEntity>()
                .HasOne(c => c.Country)
                .WithMany(country => country.Cities)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<SmartRx_PatientWishlistEntity>()
                .HasOne(p => p.PatientMedicine)
                .WithMany()
                .HasForeignKey(p => p.PatientWishlistMedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SmartRx_PatientInvestigationEntity>()
               .HasOne(p => p.InvestigationTest)
               .WithMany()
               .HasForeignKey(p => p.TestId)
               .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<SmartRx_PatientInvestigationEntity>()
            //  .HasOne(p => p.TestCenters)
            //  .WithMany()
            //  .HasForeignKey(p => p.TestCenterId)
            //  .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientInvestigationEntity>()
              .HasOne(p => p.DiagnosticCenterWiseTest)
              .WithMany()
              .HasForeignKey(p => p.DiagnosticCenterWiseTestId)
              .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientInvestigationEntity>()
             .HasOne(p => p.PriceUnit)
             .WithMany()
             .HasForeignKey(p => p.PriceUnitId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Configuration_DiagnosisCenterWiseTestEntity>()
            .HasOne(p => p.DiagnosticTestCenter)
            .WithMany()
            .HasForeignKey(p => p.TestCenterId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Configuration_DiagnosisCenterWiseTestEntity>()
              .HasOne(p => p.DiagnosticTest)
              .WithMany()
              .HasForeignKey(p => p.TestId)
              .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<SmartRx_PatientOtherExpenseEntity>()
                .HasOne(e => e.Prescription)
                .WithMany(m => m.SmartRx_PatientOtherExpense)
                .HasForeignKey(e => e.PrescriptionId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientOtherExpenseEntity>()
                 .HasOne(e => e.SmartRxMaster)
                 .WithMany(m => m.SmartRx_PatientOtherExpense)
                 .HasForeignKey(e => e.SmartRxMasterId)
                 .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientOtherExpenseEntity>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_PatientOtherExpenseEntity>()
                .HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey("ModifiedById")
                .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<SmartRx_PatientReward>()
            //   .HasOne(p => p.User)
            //   .WithMany()
            //   .HasForeignKey(p => p.UserId)
            //   .OnDelete(DeleteBehavior.NoAction);  // ✅ Prevents cascade cycles

            modelBuilder.Entity<SmartRx_PatientReward>()
                .HasOne(p => p.PatientProfile)
                .WithMany()
                .HasForeignKey(p => p.PatientId)
                .OnDelete(DeleteBehavior.NoAction);  // ✅ Prevents cascade cycles
            modelBuilder.Entity<SmartRx_PatientReward>()
              .HasOne(b => b.CreatedBy)
              .WithMany()
              .HasForeignKey(b => b.CreatedById)
              .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<SmartRx_PatientReward>()
                .HasOne(b => b.ModifiedBy)
                .WithMany()
                .HasForeignKey(b => b.ModifiedById)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Configuration_RewardBadgeEntity>()
                .HasOne(b => b.CreatedBy)
                .WithMany()
                .HasForeignKey(b => b.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Configuration_RewardBadgeEntity>()
                .HasOne(b => b.ModifiedBy)
                .WithMany()
                .HasForeignKey(b => b.ModifiedById)
                .OnDelete(DeleteBehavior.NoAction);

           

            modelBuilder.Entity<SmartRx_RewardPointConversionsEntity>()
                .HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey(x => x.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<SmartRx_RewardPointConversionsEntity>()
                .HasOne(x => x.ModifiedBy)
                .WithMany()
                .HasForeignKey(x => x.ModifiedById)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);


        }
    }
}
