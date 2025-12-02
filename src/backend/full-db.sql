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
CREATE TABLE [Security_PMSUsers] (
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
    [CreatedById] bigint NULL,
    [ModifiedDate] datetime2 NOT NULL,
    [ModifiedById] bigint NULL,
    [SmartRxUserEntityId] bigint NULL,
    CONSTRAINT [PK_Security_PMSUsers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Security_PMSUsers_Security_PMSUsers_SmartRxUserEntityId] FOREIGN KEY ([SmartRxUserEntityId]) REFERENCES [Security_PMSUsers] ([Id])
);

CREATE TABLE [Configuration_Districts] (
    [Id] bigint NOT NULL IDENTITY,
    [Code] char(2) NOT NULL,
    [Name] varchar(100) NOT NULL,
    [DivisionId] int NOT NULL,
    [CreatedDate] datetime2 NULL,
    [CreatedById] bigint NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedById] bigint NULL,
    CONSTRAINT [PK_Configuration_Districts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Configuration_Districts_Security_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [Security_PMSUsers] ([Id]),
    CONSTRAINT [FK_Configuration_Districts_Security_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [Security_PMSUsers] ([Id])
);

CREATE TABLE [Security_Roles] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [IsSelfService] bit NOT NULL,
    [CreatedDate] datetime2 NULL,
    [CreatedById] bigint NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedById] bigint NULL,
    CONSTRAINT [PK_Security_Roles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Security_Roles_Security_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Security_Roles_Security_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Configuration_Thanas] (
    [Id] bigint NOT NULL IDENTITY,
    [DistrictId] bigint NOT NULL,
    [Code] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NULL,
    [CreatedById] bigint NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedById] bigint NULL,
    CONSTRAINT [PK_Configuration_Thanas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Configuration_Thanas_Configuration_Districts_DistrictId] FOREIGN KEY ([DistrictId]) REFERENCES [Configuration_Districts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Configuration_Thanas_Security_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [Security_PMSUsers] ([Id]),
    CONSTRAINT [FK_Configuration_Thanas_Security_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [Security_PMSUsers] ([Id])
);

CREATE TABLE [UserRoles] (
    [UserId] bigint NOT NULL,
    [RoleId] bigint NOT NULL,
    [Id] bigint NOT NULL IDENTITY,
    [CreatedDate] datetime2 NULL,
    [CreatedById] bigint NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedById] bigint NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_Security_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserRoles_Security_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserRoles_Security_PMSUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Security_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Security_Roles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Prescription_SmartRx_PatientProfiles] (
    [Id] bigint NOT NULL IDENTITY,
    [PatientCode] nvarchar(max) NOT NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [NickName] nvarchar(max) NULL,
    [Age] nvarchar(max) NOT NULL,
    [DateOfBirth] datetime2 NOT NULL,
    [Gender] int NOT NULL,
    [BloodGroup] int NOT NULL,
    [PhoneNumber] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [ProfilePhoto] nvarchar(max) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [ThanaId] bigint NOT NULL,
    [CityId] int NOT NULL,
    [DistrictId] bigint NOT NULL,
    [PostalCode] nvarchar(10) NOT NULL,
    [CountryId] int NOT NULL,
    [EmergencyContact] int NOT NULL,
    [MaritalStatus] int NOT NULL,
    [IsActive] int NOT NULL,
    [CreatedDate] datetime2 NULL,
    [CreatedById] bigint NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedById] bigint NULL,
    CONSTRAINT [PK_Prescription_SmartRx_PatientProfiles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Prescription_SmartRx_PatientProfiles_Configuration_Districts_DistrictId] FOREIGN KEY ([DistrictId]) REFERENCES [Configuration_Districts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Prescription_SmartRx_PatientProfiles_Configuration_Thanas_ThanaId] FOREIGN KEY ([ThanaId]) REFERENCES [Configuration_Thanas] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Prescription_SmartRx_PatientProfiles_Security_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Prescription_SmartRx_PatientProfiles_Security_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Configuration_Vitals] (
    [Id] bigint NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [MeasurementUnitId] int NOT NULL,
    [PatientProfileId] bigint NOT NULL,
    [CreatedDate] datetime2 NULL,
    [CreatedById] bigint NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedById] bigint NULL,
    CONSTRAINT [PK_Configuration_Vitals] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Configuration_Vitals_Prescription_SmartRx_PatientProfiles_PatientProfileId] FOREIGN KEY ([PatientProfileId]) REFERENCES [Prescription_SmartRx_PatientProfiles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Configuration_Vitals_Security_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Configuration_Vitals_Security_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [UserWiseFolders] (
    [Id] bigint NOT NULL IDENTITY,
    [ParentFolderId] bigint NULL,
    [FolderHierarchy] bigint NOT NULL,
    [FolderName] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [UserId] bigint NOT NULL,
    [PatientProfileId] bigint NULL,
    [CreatedDate] datetime2 NULL,
    [CreatedById] bigint NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedById] bigint NULL,
    CONSTRAINT [PK_UserWiseFolders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserWiseFolders_Prescription_SmartRx_PatientProfiles_PatientProfileId] FOREIGN KEY ([PatientProfileId]) REFERENCES [Prescription_SmartRx_PatientProfiles] ([Id]),
    CONSTRAINT [FK_UserWiseFolders_Security_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserWiseFolders_Security_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserWiseFolders_Security_PMSUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserWiseFolders_UserWiseFolders_ParentFolderId] FOREIGN KEY ([ParentFolderId]) REFERENCES [UserWiseFolders] ([Id]) ON DELETE NO ACTION
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
    [UserWiseFolderEntityId] bigint NULL,
    [CreatedDate] datetime2 NULL,
    [CreatedById] bigint NULL,
    [ModifiedDate] datetime2 NULL,
    [ModifiedById] bigint NULL,
    CONSTRAINT [PK_Prescription_UploadedPrescription] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Prescription_UploadedPrescription_Prescription_SmartRx_PatientProfiles_PatientProfileId] FOREIGN KEY ([PatientProfileId]) REFERENCES [Prescription_SmartRx_PatientProfiles] ([Id]),
    CONSTRAINT [FK_Prescription_UploadedPrescription_Security_PMSUsers_ApprovedById] FOREIGN KEY ([ApprovedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Prescription_UploadedPrescription_Security_PMSUsers_CompletedById] FOREIGN KEY ([CompletedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Prescription_UploadedPrescription_Security_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Prescription_UploadedPrescription_Security_PMSUsers_LockedById] FOREIGN KEY ([LockedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Prescription_UploadedPrescription_Security_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Prescription_UploadedPrescription_Security_PMSUsers_RecommendedById] FOREIGN KEY ([RecommendedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Prescription_UploadedPrescription_Security_PMSUsers_ReportById] FOREIGN KEY ([ReportById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Prescription_UploadedPrescription_Security_PMSUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Prescription_UploadedPrescription_UserWiseFolders_FolderId] FOREIGN KEY ([FolderId]) REFERENCES [UserWiseFolders] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Prescription_UploadedPrescription_UserWiseFolders_UserWiseFolderEntityId] FOREIGN KEY ([UserWiseFolderEntityId]) REFERENCES [UserWiseFolders] ([Id])
);

CREATE INDEX [IX_Configuration_Districts_CreatedById] ON [Configuration_Districts] ([CreatedById]);

CREATE INDEX [IX_Configuration_Districts_ModifiedById] ON [Configuration_Districts] ([ModifiedById]);

CREATE INDEX [IX_Configuration_Thanas_CreatedById] ON [Configuration_Thanas] ([CreatedById]);

CREATE INDEX [IX_Configuration_Thanas_DistrictId] ON [Configuration_Thanas] ([DistrictId]);

CREATE INDEX [IX_Configuration_Thanas_ModifiedById] ON [Configuration_Thanas] ([ModifiedById]);

CREATE INDEX [IX_Configuration_Vitals_CreatedById] ON [Configuration_Vitals] ([CreatedById]);

CREATE INDEX [IX_Configuration_Vitals_ModifiedById] ON [Configuration_Vitals] ([ModifiedById]);

CREATE INDEX [IX_Configuration_Vitals_PatientProfileId] ON [Configuration_Vitals] ([PatientProfileId]);

CREATE INDEX [IX_Prescription_SmartRx_PatientProfiles_CreatedById] ON [Prescription_SmartRx_PatientProfiles] ([CreatedById]);

CREATE INDEX [IX_Prescription_SmartRx_PatientProfiles_DistrictId] ON [Prescription_SmartRx_PatientProfiles] ([DistrictId]);

CREATE INDEX [IX_Prescription_SmartRx_PatientProfiles_ModifiedById] ON [Prescription_SmartRx_PatientProfiles] ([ModifiedById]);

CREATE INDEX [IX_Prescription_SmartRx_PatientProfiles_ThanaId] ON [Prescription_SmartRx_PatientProfiles] ([ThanaId]);

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

CREATE INDEX [IX_Prescription_UploadedPrescription_UserWiseFolderEntityId] ON [Prescription_UploadedPrescription] ([UserWiseFolderEntityId]);

CREATE INDEX [IX_Security_PMSUsers_SmartRxUserEntityId] ON [Security_PMSUsers] ([SmartRxUserEntityId]);

CREATE INDEX [IX_Security_Roles_CreatedById] ON [Security_Roles] ([CreatedById]);

CREATE INDEX [IX_Security_Roles_ModifiedById] ON [Security_Roles] ([ModifiedById]);

CREATE INDEX [IX_UserRoles_CreatedById] ON [UserRoles] ([CreatedById]);

CREATE INDEX [IX_UserRoles_ModifiedById] ON [UserRoles] ([ModifiedById]);

CREATE INDEX [IX_UserRoles_RoleId] ON [UserRoles] ([RoleId]);

CREATE INDEX [IX_UserWiseFolders_CreatedById] ON [UserWiseFolders] ([CreatedById]);

CREATE INDEX [IX_UserWiseFolders_ModifiedById] ON [UserWiseFolders] ([ModifiedById]);

CREATE INDEX [IX_UserWiseFolders_ParentFolderId] ON [UserWiseFolders] ([ParentFolderId]);

CREATE INDEX [IX_UserWiseFolders_PatientProfileId] ON [UserWiseFolders] ([PatientProfileId]);

CREATE INDEX [IX_UserWiseFolders_UserId] ON [UserWiseFolders] ([UserId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250512094555_Initial Commit', N'9.0.1');

ALTER TABLE [Prescription_UploadedPrescription] ADD [Tag1] nvarchar(max) NULL;

ALTER TABLE [Prescription_UploadedPrescription] ADD [Tag2] nvarchar(max) NULL;

ALTER TABLE [Prescription_UploadedPrescription] ADD [Tag3] nvarchar(max) NULL;

ALTER TABLE [Prescription_UploadedPrescription] ADD [Tag4] nvarchar(max) NULL;

ALTER TABLE [Prescription_UploadedPrescription] ADD [Tag5] nvarchar(max) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250512104539_5 tag added in prescription upload table', N'9.0.1');

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Prescription_UploadedPrescription]') AND [c].[name] = N'Tag5');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Prescription_UploadedPrescription] ALTER COLUMN [Tag5] varchar(50) NULL;

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Prescription_UploadedPrescription]') AND [c].[name] = N'Tag4');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Prescription_UploadedPrescription] ALTER COLUMN [Tag4] varchar(50) NULL;

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Prescription_UploadedPrescription]') AND [c].[name] = N'Tag3');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Prescription_UploadedPrescription] ALTER COLUMN [Tag3] varchar(50) NULL;

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Prescription_UploadedPrescription]') AND [c].[name] = N'Tag2');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Prescription_UploadedPrescription] ALTER COLUMN [Tag2] varchar(50) NULL;

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Prescription_UploadedPrescription]') AND [c].[name] = N'Tag1');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Prescription_UploadedPrescription] ALTER COLUMN [Tag1] varchar(50) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250512105058_data length added', N'9.0.1');

ALTER TABLE [Configuration_Districts] DROP CONSTRAINT [FK_Configuration_Districts_Security_PMSUsers_CreatedById];

ALTER TABLE [Configuration_Districts] DROP CONSTRAINT [FK_Configuration_Districts_Security_PMSUsers_ModifiedById];

ALTER TABLE [Configuration_Thanas] DROP CONSTRAINT [FK_Configuration_Thanas_Configuration_Districts_DistrictId];

ALTER TABLE [Configuration_Thanas] DROP CONSTRAINT [FK_Configuration_Thanas_Security_PMSUsers_CreatedById];

ALTER TABLE [Configuration_Thanas] DROP CONSTRAINT [FK_Configuration_Thanas_Security_PMSUsers_ModifiedById];

ALTER TABLE [Configuration_Vitals] DROP CONSTRAINT [FK_Configuration_Vitals_Prescription_SmartRx_PatientProfiles_PatientProfileId];

ALTER TABLE [Configuration_Vitals] DROP CONSTRAINT [FK_Configuration_Vitals_Security_PMSUsers_CreatedById];

ALTER TABLE [Configuration_Vitals] DROP CONSTRAINT [FK_Configuration_Vitals_Security_PMSUsers_ModifiedById];

ALTER TABLE [Prescription_SmartRx_PatientProfiles] DROP CONSTRAINT [FK_Prescription_SmartRx_PatientProfiles_Configuration_Districts_DistrictId];

ALTER TABLE [Prescription_SmartRx_PatientProfiles] DROP CONSTRAINT [FK_Prescription_SmartRx_PatientProfiles_Configuration_Thanas_ThanaId];

ALTER TABLE [Prescription_SmartRx_PatientProfiles] DROP CONSTRAINT [FK_Prescription_SmartRx_PatientProfiles_Security_PMSUsers_CreatedById];

ALTER TABLE [Prescription_SmartRx_PatientProfiles] DROP CONSTRAINT [FK_Prescription_SmartRx_PatientProfiles_Security_PMSUsers_ModifiedById];

ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [FK_Prescription_UploadedPrescription_Prescription_SmartRx_PatientProfiles_PatientProfileId];

ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [FK_Prescription_UploadedPrescription_Security_PMSUsers_ApprovedById];

ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [FK_Prescription_UploadedPrescription_Security_PMSUsers_CompletedById];

ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [FK_Prescription_UploadedPrescription_Security_PMSUsers_CreatedById];

ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [FK_Prescription_UploadedPrescription_Security_PMSUsers_LockedById];

ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [FK_Prescription_UploadedPrescription_Security_PMSUsers_ModifiedById];

ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [FK_Prescription_UploadedPrescription_Security_PMSUsers_RecommendedById];

ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [FK_Prescription_UploadedPrescription_Security_PMSUsers_ReportById];

ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [FK_Prescription_UploadedPrescription_Security_PMSUsers_UserId];

ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [FK_Prescription_UploadedPrescription_UserWiseFolders_FolderId];

ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [FK_Prescription_UploadedPrescription_UserWiseFolders_UserWiseFolderEntityId];

ALTER TABLE [Security_Roles] DROP CONSTRAINT [FK_Security_Roles_Security_PMSUsers_CreatedById];

ALTER TABLE [Security_Roles] DROP CONSTRAINT [FK_Security_Roles_Security_PMSUsers_ModifiedById];

ALTER TABLE [UserRoles] DROP CONSTRAINT [FK_UserRoles_Security_Roles_RoleId];

ALTER TABLE [UserWiseFolders] DROP CONSTRAINT [FK_UserWiseFolders_Prescription_SmartRx_PatientProfiles_PatientProfileId];

ALTER TABLE [Security_Roles] DROP CONSTRAINT [PK_Security_Roles];

ALTER TABLE [Prescription_UploadedPrescription] DROP CONSTRAINT [PK_Prescription_UploadedPrescription];

ALTER TABLE [Prescription_SmartRx_PatientProfiles] DROP CONSTRAINT [PK_Prescription_SmartRx_PatientProfiles];

ALTER TABLE [Configuration_Vitals] DROP CONSTRAINT [PK_Configuration_Vitals];

ALTER TABLE [Configuration_Thanas] DROP CONSTRAINT [PK_Configuration_Thanas];

ALTER TABLE [Configuration_Districts] DROP CONSTRAINT [PK_Configuration_Districts];

EXEC sp_rename N'[Security_Roles]', N'Security_PMSRoles', 'OBJECT';

EXEC sp_rename N'[Prescription_UploadedPrescription]', N'Prescription_Upload', 'OBJECT';

EXEC sp_rename N'[Prescription_SmartRx_PatientProfiles]', N'Prescription_PatientProfile', 'OBJECT';

EXEC sp_rename N'[Configuration_Vitals]', N'Prescription_Vitals', 'OBJECT';

EXEC sp_rename N'[Configuration_Thanas]', N'Configuration_Thana', 'OBJECT';

EXEC sp_rename N'[Configuration_Districts]', N'Configuration_District', 'OBJECT';

EXEC sp_rename N'[Security_PMSRoles].[IX_Security_Roles_ModifiedById]', N'IX_Security_PMSRoles_ModifiedById', 'INDEX';

EXEC sp_rename N'[Security_PMSRoles].[IX_Security_Roles_CreatedById]', N'IX_Security_PMSRoles_CreatedById', 'INDEX';

EXEC sp_rename N'[Prescription_Upload].[IX_Prescription_UploadedPrescription_UserWiseFolderEntityId]', N'IX_Prescription_Upload_UserWiseFolderEntityId', 'INDEX';

EXEC sp_rename N'[Prescription_Upload].[IX_Prescription_UploadedPrescription_UserId]', N'IX_Prescription_Upload_UserId', 'INDEX';

EXEC sp_rename N'[Prescription_Upload].[IX_Prescription_UploadedPrescription_ReportById]', N'IX_Prescription_Upload_ReportById', 'INDEX';

EXEC sp_rename N'[Prescription_Upload].[IX_Prescription_UploadedPrescription_RecommendedById]', N'IX_Prescription_Upload_RecommendedById', 'INDEX';

EXEC sp_rename N'[Prescription_Upload].[IX_Prescription_UploadedPrescription_PatientProfileId]', N'IX_Prescription_Upload_PatientProfileId', 'INDEX';

EXEC sp_rename N'[Prescription_Upload].[IX_Prescription_UploadedPrescription_ModifiedById]', N'IX_Prescription_Upload_ModifiedById', 'INDEX';

EXEC sp_rename N'[Prescription_Upload].[IX_Prescription_UploadedPrescription_LockedById]', N'IX_Prescription_Upload_LockedById', 'INDEX';

EXEC sp_rename N'[Prescription_Upload].[IX_Prescription_UploadedPrescription_FolderId]', N'IX_Prescription_Upload_FolderId', 'INDEX';

EXEC sp_rename N'[Prescription_Upload].[IX_Prescription_UploadedPrescription_CreatedById]', N'IX_Prescription_Upload_CreatedById', 'INDEX';

EXEC sp_rename N'[Prescription_Upload].[IX_Prescription_UploadedPrescription_CompletedById]', N'IX_Prescription_Upload_CompletedById', 'INDEX';

EXEC sp_rename N'[Prescription_Upload].[IX_Prescription_UploadedPrescription_ApprovedById]', N'IX_Prescription_Upload_ApprovedById', 'INDEX';

EXEC sp_rename N'[Prescription_PatientProfile].[IX_Prescription_SmartRx_PatientProfiles_ThanaId]', N'IX_Prescription_PatientProfile_ThanaId', 'INDEX';

EXEC sp_rename N'[Prescription_PatientProfile].[IX_Prescription_SmartRx_PatientProfiles_ModifiedById]', N'IX_Prescription_PatientProfile_ModifiedById', 'INDEX';

EXEC sp_rename N'[Prescription_PatientProfile].[IX_Prescription_SmartRx_PatientProfiles_DistrictId]', N'IX_Prescription_PatientProfile_DistrictId', 'INDEX';

EXEC sp_rename N'[Prescription_PatientProfile].[IX_Prescription_SmartRx_PatientProfiles_CreatedById]', N'IX_Prescription_PatientProfile_CreatedById', 'INDEX';

EXEC sp_rename N'[Prescription_Vitals].[IX_Configuration_Vitals_PatientProfileId]', N'IX_Prescription_Vitals_PatientProfileId', 'INDEX';

EXEC sp_rename N'[Prescription_Vitals].[IX_Configuration_Vitals_ModifiedById]', N'IX_Prescription_Vitals_ModifiedById', 'INDEX';

EXEC sp_rename N'[Prescription_Vitals].[IX_Configuration_Vitals_CreatedById]', N'IX_Prescription_Vitals_CreatedById', 'INDEX';

EXEC sp_rename N'[Configuration_Thana].[IX_Configuration_Thanas_ModifiedById]', N'IX_Configuration_Thana_ModifiedById', 'INDEX';

EXEC sp_rename N'[Configuration_Thana].[IX_Configuration_Thanas_DistrictId]', N'IX_Configuration_Thana_DistrictId', 'INDEX';

EXEC sp_rename N'[Configuration_Thana].[IX_Configuration_Thanas_CreatedById]', N'IX_Configuration_Thana_CreatedById', 'INDEX';

EXEC sp_rename N'[Configuration_District].[IX_Configuration_Districts_ModifiedById]', N'IX_Configuration_District_ModifiedById', 'INDEX';

EXEC sp_rename N'[Configuration_District].[IX_Configuration_Districts_CreatedById]', N'IX_Configuration_District_CreatedById', 'INDEX';

ALTER TABLE [Security_PMSRoles] ADD CONSTRAINT [PK_Security_PMSRoles] PRIMARY KEY ([Id]);

ALTER TABLE [Prescription_Upload] ADD CONSTRAINT [PK_Prescription_Upload] PRIMARY KEY ([Id]);

ALTER TABLE [Prescription_PatientProfile] ADD CONSTRAINT [PK_Prescription_PatientProfile] PRIMARY KEY ([Id]);

ALTER TABLE [Prescription_Vitals] ADD CONSTRAINT [PK_Prescription_Vitals] PRIMARY KEY ([Id]);

ALTER TABLE [Configuration_Thana] ADD CONSTRAINT [PK_Configuration_Thana] PRIMARY KEY ([Id]);

ALTER TABLE [Configuration_District] ADD CONSTRAINT [PK_Configuration_District] PRIMARY KEY ([Id]);

ALTER TABLE [Configuration_District] ADD CONSTRAINT [FK_Configuration_District_Security_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [Security_PMSUsers] ([Id]);

ALTER TABLE [Configuration_District] ADD CONSTRAINT [FK_Configuration_District_Security_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [Security_PMSUsers] ([Id]);

ALTER TABLE [Configuration_Thana] ADD CONSTRAINT [FK_Configuration_Thana_Configuration_District_DistrictId] FOREIGN KEY ([DistrictId]) REFERENCES [Configuration_District] ([Id]) ON DELETE CASCADE;

ALTER TABLE [Configuration_Thana] ADD CONSTRAINT [FK_Configuration_Thana_Security_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [Security_PMSUsers] ([Id]);

ALTER TABLE [Configuration_Thana] ADD CONSTRAINT [FK_Configuration_Thana_Security_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [Security_PMSUsers] ([Id]);

ALTER TABLE [Prescription_PatientProfile] ADD CONSTRAINT [FK_Prescription_PatientProfile_Configuration_District_DistrictId] FOREIGN KEY ([DistrictId]) REFERENCES [Configuration_District] ([Id]) ON DELETE CASCADE;

ALTER TABLE [Prescription_PatientProfile] ADD CONSTRAINT [FK_Prescription_PatientProfile_Configuration_Thana_ThanaId] FOREIGN KEY ([ThanaId]) REFERENCES [Configuration_Thana] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Prescription_PatientProfile] ADD CONSTRAINT [FK_Prescription_PatientProfile_Security_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Prescription_PatientProfile] ADD CONSTRAINT [FK_Prescription_PatientProfile_Security_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Prescription_Upload] ADD CONSTRAINT [FK_Prescription_Upload_Prescription_PatientProfile_PatientProfileId] FOREIGN KEY ([PatientProfileId]) REFERENCES [Prescription_PatientProfile] ([Id]);

ALTER TABLE [Prescription_Upload] ADD CONSTRAINT [FK_Prescription_Upload_Security_PMSUsers_ApprovedById] FOREIGN KEY ([ApprovedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Prescription_Upload] ADD CONSTRAINT [FK_Prescription_Upload_Security_PMSUsers_CompletedById] FOREIGN KEY ([CompletedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Prescription_Upload] ADD CONSTRAINT [FK_Prescription_Upload_Security_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Prescription_Upload] ADD CONSTRAINT [FK_Prescription_Upload_Security_PMSUsers_LockedById] FOREIGN KEY ([LockedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Prescription_Upload] ADD CONSTRAINT [FK_Prescription_Upload_Security_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Prescription_Upload] ADD CONSTRAINT [FK_Prescription_Upload_Security_PMSUsers_RecommendedById] FOREIGN KEY ([RecommendedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Prescription_Upload] ADD CONSTRAINT [FK_Prescription_Upload_Security_PMSUsers_ReportById] FOREIGN KEY ([ReportById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Prescription_Upload] ADD CONSTRAINT [FK_Prescription_Upload_Security_PMSUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE CASCADE;

ALTER TABLE [Prescription_Upload] ADD CONSTRAINT [FK_Prescription_Upload_UserWiseFolders_FolderId] FOREIGN KEY ([FolderId]) REFERENCES [UserWiseFolders] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Prescription_Upload] ADD CONSTRAINT [FK_Prescription_Upload_UserWiseFolders_UserWiseFolderEntityId] FOREIGN KEY ([UserWiseFolderEntityId]) REFERENCES [UserWiseFolders] ([Id]);

ALTER TABLE [Prescription_Vitals] ADD CONSTRAINT [FK_Prescription_Vitals_Prescription_PatientProfile_PatientProfileId] FOREIGN KEY ([PatientProfileId]) REFERENCES [Prescription_PatientProfile] ([Id]) ON DELETE CASCADE;

ALTER TABLE [Prescription_Vitals] ADD CONSTRAINT [FK_Prescription_Vitals_Security_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Prescription_Vitals] ADD CONSTRAINT [FK_Prescription_Vitals_Security_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Security_PMSRoles] ADD CONSTRAINT [FK_Security_PMSRoles_Security_PMSUsers_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Security_PMSRoles] ADD CONSTRAINT [FK_Security_PMSRoles_Security_PMSUsers_ModifiedById] FOREIGN KEY ([ModifiedById]) REFERENCES [Security_PMSUsers] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [UserRoles] ADD CONSTRAINT [FK_UserRoles_Security_PMSRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Security_PMSRoles] ([Id]) ON DELETE CASCADE;

ALTER TABLE [UserWiseFolders] ADD CONSTRAINT [FK_UserWiseFolders_Prescription_PatientProfile_PatientProfileId] FOREIGN KEY ([PatientProfileId]) REFERENCES [Prescription_PatientProfile] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250512123150_All tables renamed', N'9.0.1');

COMMIT;
GO

