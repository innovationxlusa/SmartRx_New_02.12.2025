IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [PMSUsers] (
    [Id] bigint NOT NULL IDENTITY,
    [UserCode] nvarchar(max) NOT NULL,
    [UserName] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NULL,
    [MobileNo] nvarchar(max) NOT NULL,
    [GoogleId] nvarchar(max) NULL,
    [FacebookId] nvarchar(max) NULL,
    [TwitterId] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [AuthMethod] int NOT NULL,
    [EmployeeId] int NULL,
    [EmployeeCode] nvarchar(max) NULL,
    [Gender] int NOT NULL,
    [DateOfBirth] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [CreatedById] bigint NOT NULL,
    [ModifiedDate] datetime2 NOT NULL,
    [ModifiedById] bigint NOT NULL,
    CONSTRAINT [PK_PMSUsers] PRIMARY KEY ([Id])
);

CREATE TABLE [PatientProfiles] (
    [Id] bigint NOT NULL IDENTITY,
    [PatientCode] nvarchar(max) NOT NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [NickName] nvarchar(max) NULL,
    [Age] nvarchar(max) NOT NULL,
    [DateOfBirth] datetime2 NOT NULL,
    [Gender] int NOT NULL,
    [BloodGroup] int NOT NULL,
    [VitalIds] bigint NOT NULL,
    [PhoneNumber] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [ProfilePhoto] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [Area] nvarchar(max) NOT NULL,
    [District] nvarchar(max) NOT NULL,
    [PostalCode] nvarchar(max) NOT NULL,
    [Country] nvarchar(max) NOT NULL,
    [EmergencyContact] int NOT NULL,
    [MaritalStatus] int NOT NULL,
    [IsActive] int NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [CreatedById] bigint NOT NULL,
    [ModifiedDate] datetime2 NOT NULL,
    [ModifiedById] bigint NOT NULL,
    CONSTRAINT [PK_PatientProfiles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PatientProfiles_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PatientProfiles_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [PMSUsers] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Roles] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [IsSelfService] bit NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [CreatedById] bigint NOT NULL,
    [ModifiedDate] datetime2 NOT NULL,
    [ModifiedById] bigint NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Roles_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Roles_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [PMSUsers] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [UserWiseFolders] (
    [Id] bigint NOT NULL IDENTITY,
    [FolderHierarchy] bigint NOT NULL,
    [FolderID] nvarchar(max) NOT NULL,
    [FolderName] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [UserId] bigint NOT NULL,
    [PatientProfileId] bigint NULL,
    [CreatedDate] datetime2 NOT NULL,
    [CreatedById] bigint NOT NULL,
    [ModifiedDate] datetime2 NOT NULL,
    [ModifiedById] bigint NOT NULL,
    CONSTRAINT [PK_UserWiseFolders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserWiseFolders_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserWiseFolders_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserWiseFolders_PMSUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [PMSUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserWiseFolders_PatientProfiles_PatientProfileId] FOREIGN KEY ([PatientProfileId]) REFERENCES [PatientProfiles] ([Id])
);

CREATE TABLE [Vitals] (
    [Id] bigint NOT NULL IDENTITY,
    [VitalName] nvarchar(max) NOT NULL,
    [MeasurementUnitId] int NOT NULL,
    [PatientProfileId] bigint NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [CreatedById] bigint NOT NULL,
    [ModifiedDate] datetime2 NOT NULL,
    [ModifiedById] bigint NOT NULL,
    CONSTRAINT [PK_Vitals] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Vitals_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Vitals_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Vitals_PatientProfiles_PatientProfileId] FOREIGN KEY ([PatientProfileId]) REFERENCES [PatientProfiles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [UserRoles] (
    [UserId] bigint NOT NULL,
    [RoleId] bigint NOT NULL,
    [Id] bigint NOT NULL IDENTITY,
    [CreatedDate] datetime2 NOT NULL,
    [CreatedById] bigint NOT NULL,
    [ModifiedDate] datetime2 NOT NULL,
    [ModifiedById] bigint NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserRoles_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserRoles_PMSUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [PMSUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Prescription_UploadedPrescription] (
    [Id] bigint NOT NULL IDENTITY,
    [PrescriptionCode] nvarchar(max) NOT NULL,
    [PatientProfileId] bigint NULL,
    [IsExistingPatient] bit NULL,
    [HasExistingRelative] bit NULL,
    [RelativePatientIds] nvarchar(max) NULL,
    [FileName] nvarchar(max) NOT NULL,
    [FilePath] nvarchar(max) NOT NULL,
    [UserId] bigint NOT NULL,
    [FolderId] bigint NOT NULL,
    [IsSmartRxRequested] bit NULL,
    [IsLocked] bit NULL,
    [LockedById] bigint NULL,
    [LockedDate] datetime2 NULL,
    [IsReported] bit NULL,
    [ReportById] bigint NULL,
    [ReportDate] datetime2 NULL,
    [ReportReason] nvarchar(max) NULL,
    [ReportDetails] nvarchar(max) NULL,
    [IsRecommended] bit NULL,
    [RecommendedById] bigint NULL,
    [RecommendedDate] datetime2 NULL,
    [IsApproved] bit NULL,
    [ApprovedById] bigint NULL,
    [ApprovedDate] datetime2 NULL,
    [IsCompleted] bit NULL,
    [CompletedById] bigint NULL,
    [CompletedDate] datetime2 NULL,
    [CreatedDate] datetime2 NOT NULL,
    [CreatedById] bigint NOT NULL,
    [ModifiedDate] datetime2 NOT NULL,
    [ModifiedById] bigint NOT NULL,
    CONSTRAINT [PK_Prescription_UploadedPrescription] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Prescription_UploadedPrescription_PMSUsers_ApprovedById] FOREIGN KEY ([ApprovedById]) REFERENCES [PMSUsers] ([Id]),
    CONSTRAINT [FK_Prescription_UploadedPrescription_PMSUsers_CompletedById] FOREIGN KEY ([CompletedById]) REFERENCES [PMSUsers] ([Id]),
    CONSTRAINT [FK_Prescription_UploadedPrescription_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Prescription_UploadedPrescription_PMSUsers_LockedById] FOREIGN KEY ([LockedById]) REFERENCES [PMSUsers] ([Id]),
    CONSTRAINT [FK_Prescription_UploadedPrescription_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Prescription_UploadedPrescription_PMSUsers_RecommendedById] FOREIGN KEY ([RecommendedById]) REFERENCES [PMSUsers] ([Id]),
    CONSTRAINT [FK_Prescription_UploadedPrescription_PMSUsers_ReportById] FOREIGN KEY ([ReportById]) REFERENCES [PMSUsers] ([Id]),
    CONSTRAINT [FK_Prescription_UploadedPrescription_PMSUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [PMSUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Prescription_UploadedPrescription_PatientProfiles_PatientProfileId] FOREIGN KEY ([PatientProfileId]) REFERENCES [PatientProfiles] ([Id]),
    CONSTRAINT [FK_Prescription_UploadedPrescription_UserWiseFolders_FolderId] FOREIGN KEY ([FolderId]) REFERENCES [UserWiseFolders] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_PatientProfiles_CreatedById] ON [PatientProfiles] ([CreatedById]);

CREATE INDEX [IX_PatientProfiles_ModifiedById] ON [PatientProfiles] ([ModifiedById]);

CREATE INDEX [IX_Prescription_UploadedPrescription_ApprovedById] ON [Prescription_UploadedPrescription] ([ApprovedById]);

CREATE INDEX [IX_Prescription_UploadedPrescription_CompletedById] ON [Prescription_UploadedPrescription] ([CompletedById]);

CREATE INDEX [IX_Prescription_UploadedPrescription_CreatedById] ON [Prescription_UploadedPrescription] ([CreatedById]);

CREATE INDEX [IX_Prescription_UploadedPrescription_FolderId] ON [Prescription_UploadedPrescription] ([FolderId]);

CREATE INDEX [IX_Prescription_UploadedPrescription_LockedById] ON [Prescription_UploadedPrescription] ([LockedById]);

CREATE INDEX [IX_Prescription_UploadedPrescription_ModifiedById] ON [Prescription_UploadedPrescription] ([ModifiedById]);

CREATE INDEX [IX_Prescription_UploadedPrescription_PatientProfileId] ON [Prescription_UploadedPrescription] ([PatientProfileId]);

CREATE INDEX [IX_Prescription_UploadedPrescription_RecommendedById] ON [Prescription_UploadedPrescription] ([RecommendedById]);

CREATE INDEX [IX_Prescription_UploadedPrescription_ReportById] ON [Prescription_UploadedPrescription] ([ReportById]);

CREATE INDEX [IX_Prescription_UploadedPrescription_UserId] ON [Prescription_UploadedPrescription] ([UserId]);

CREATE INDEX [IX_Roles_CreatedById] ON [Roles] ([CreatedById]);

CREATE INDEX [IX_Roles_ModifiedById] ON [Roles] ([ModifiedById]);

CREATE INDEX [IX_UserRoles_CreatedById] ON [UserRoles] ([CreatedById]);

CREATE INDEX [IX_UserRoles_ModifiedById] ON [UserRoles] ([ModifiedById]);

CREATE INDEX [IX_UserRoles_RoleId] ON [UserRoles] ([RoleId]);

CREATE INDEX [IX_UserWiseFolders_CreatedById] ON [UserWiseFolders] ([CreatedById]);

CREATE INDEX [IX_UserWiseFolders_ModifiedById] ON [UserWiseFolders] ([ModifiedById]);

CREATE INDEX [IX_UserWiseFolders_PatientProfileId] ON [UserWiseFolders] ([PatientProfileId]);

CREATE INDEX [IX_UserWiseFolders_UserId] ON [UserWiseFolders] ([UserId]);

CREATE INDEX [IX_Vitals_CreatedById] ON [Vitals] ([CreatedById]);

CREATE INDEX [IX_Vitals_ModifiedById] ON [Vitals] ([ModifiedById]);

CREATE INDEX [IX_Vitals_PatientProfileId] ON [Vitals] ([PatientProfileId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250504194355_All table create first commit', N'9.0.1');

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Vitals]') AND [c].[name] = N'ModifiedDate');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Vitals] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Vitals] ALTER COLUMN [ModifiedDate] datetime2 NULL;

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Vitals]') AND [c].[name] = N'ModifiedById');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Vitals] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Vitals] ALTER COLUMN [ModifiedById] bigint NULL;

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserWiseFolders]') AND [c].[name] = N'ModifiedDate');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [UserWiseFolders] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [UserWiseFolders] ALTER COLUMN [ModifiedDate] datetime2 NULL;

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserWiseFolders]') AND [c].[name] = N'ModifiedById');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [UserWiseFolders] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [UserWiseFolders] ALTER COLUMN [ModifiedById] bigint NULL;

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserRoles]') AND [c].[name] = N'ModifiedDate');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [UserRoles] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [UserRoles] ALTER COLUMN [ModifiedDate] datetime2 NULL;

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserRoles]') AND [c].[name] = N'ModifiedById');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [UserRoles] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [UserRoles] ALTER COLUMN [ModifiedById] bigint NULL;

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Roles]') AND [c].[name] = N'ModifiedDate');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Roles] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Roles] ALTER COLUMN [ModifiedDate] datetime2 NULL;

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Roles]') AND [c].[name] = N'ModifiedById');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Roles] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Roles] ALTER COLUMN [ModifiedById] bigint NULL;

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Prescription_UploadedPrescription]') AND [c].[name] = N'ModifiedDate');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [Prescription_UploadedPrescription] ALTER COLUMN [ModifiedDate] datetime2 NULL;

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Prescription_UploadedPrescription]') AND [c].[name] = N'ModifiedById');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [Prescription_UploadedPrescription] ALTER COLUMN [ModifiedById] bigint NULL;

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PatientProfiles]') AND [c].[name] = N'ModifiedDate');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [PatientProfiles] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [PatientProfiles] ALTER COLUMN [ModifiedDate] datetime2 NULL;

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PatientProfiles]') AND [c].[name] = N'ModifiedById');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [PatientProfiles] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [PatientProfiles] ALTER COLUMN [ModifiedById] bigint NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250504194539_base data optional', N'9.0.1');

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserWiseFolders]') AND [c].[name] = N'FolderID');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [UserWiseFolders] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [UserWiseFolders] DROP COLUMN [FolderID];

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250504195828_FolderId column removed from UserWiseFolder', N'9.0.1');

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Vitals]') AND [c].[name] = N'CreatedDate');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [Vitals] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [Vitals] ALTER COLUMN [CreatedDate] datetime2 NULL;

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Vitals]') AND [c].[name] = N'CreatedById');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [Vitals] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [Vitals] ALTER COLUMN [CreatedById] bigint NULL;

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserWiseFolders]') AND [c].[name] = N'CreatedDate');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [UserWiseFolders] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [UserWiseFolders] ALTER COLUMN [CreatedDate] datetime2 NULL;

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserWiseFolders]') AND [c].[name] = N'CreatedById');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [UserWiseFolders] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [UserWiseFolders] ALTER COLUMN [CreatedById] bigint NULL;

DECLARE @var17 sysname;
SELECT @var17 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserRoles]') AND [c].[name] = N'CreatedDate');
IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [UserRoles] DROP CONSTRAINT [' + @var17 + '];');
ALTER TABLE [UserRoles] ALTER COLUMN [CreatedDate] datetime2 NULL;

DECLARE @var18 sysname;
SELECT @var18 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserRoles]') AND [c].[name] = N'CreatedById');
IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [UserRoles] DROP CONSTRAINT [' + @var18 + '];');
ALTER TABLE [UserRoles] ALTER COLUMN [CreatedById] bigint NULL;

DECLARE @var19 sysname;
SELECT @var19 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Roles]') AND [c].[name] = N'CreatedDate');
IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [Roles] DROP CONSTRAINT [' + @var19 + '];');
ALTER TABLE [Roles] ALTER COLUMN [CreatedDate] datetime2 NULL;

DECLARE @var20 sysname;
SELECT @var20 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Roles]') AND [c].[name] = N'CreatedById');
IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [Roles] DROP CONSTRAINT [' + @var20 + '];');
ALTER TABLE [Roles] ALTER COLUMN [CreatedById] bigint NULL;

DECLARE @var21 sysname;
SELECT @var21 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Prescription_UploadedPrescription]') AND [c].[name] = N'CreatedDate');
IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [' + @var21 + '];');
ALTER TABLE [Prescription_UploadedPrescription] ALTER COLUMN [CreatedDate] datetime2 NULL;

DECLARE @var22 sysname;
SELECT @var22 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Prescription_UploadedPrescription]') AND [c].[name] = N'CreatedById');
IF @var22 IS NOT NULL EXEC(N'ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [' + @var22 + '];');
ALTER TABLE [Prescription_UploadedPrescription] ALTER COLUMN [CreatedById] bigint NULL;

DECLARE @var23 sysname;
SELECT @var23 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PatientProfiles]') AND [c].[name] = N'CreatedDate');
IF @var23 IS NOT NULL EXEC(N'ALTER TABLE [PatientProfiles] DROP CONSTRAINT [' + @var23 + '];');
ALTER TABLE [PatientProfiles] ALTER COLUMN [CreatedDate] datetime2 NULL;

DECLARE @var24 sysname;
SELECT @var24 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PatientProfiles]') AND [c].[name] = N'CreatedById');
IF @var24 IS NOT NULL EXEC(N'ALTER TABLE [PatientProfiles] DROP CONSTRAINT [' + @var24 + '];');
ALTER TABLE [PatientProfiles] ALTER COLUMN [CreatedById] bigint NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250504200410_FolderId column removed from UserWiseFolder 1', N'9.0.1');

COMMIT;
GO

