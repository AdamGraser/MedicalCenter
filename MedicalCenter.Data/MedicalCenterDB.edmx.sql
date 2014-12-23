
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 12/23/2014 08:58:42
-- Generated from EDMX file: C:\Users\Adam\Documents\GitHub\MedicalCenter\MedicalCenter.Data\MedicalCenterDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MedicalCenter];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'A_Calendars'
CREATE TABLE [dbo].[A_Calendars] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IsHoliday] bit  NOT NULL,
    [Weekday] tinyint  NOT NULL,
    [Date] datetime  NOT NULL
);
GO

-- Creating table 'A_Absences'
CREATE TABLE [dbo].[A_Absences] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [WorkerId] int  NOT NULL,
    [DateFrom] datetime  NOT NULL,
    [DateTo] datetime  NULL,
    [Type] char(1)  NOT NULL
);
GO

-- Creating table 'A_Users'
CREATE TABLE [dbo].[A_Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [WorkerId] int  NOT NULL,
    [Login] varchar(10)  NOT NULL,
    [Password] text  NOT NULL,
    [Active] datetime  NOT NULL,
    [Expires] datetime  NULL
);
GO

-- Creating table 'A_Schedules'
CREATE TABLE [dbo].[A_Schedules] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [WorkerId] int  NOT NULL,
    [ValidFrom] datetime  NOT NULL,
    [ValidTo] datetime  NULL,
    [D1From] datetime  NULL,
    [D1To] datetime  NULL,
    [D2From] datetime  NULL,
    [D2To] datetime  NULL,
    [D3From] datetime  NULL,
    [D3To] datetime  NULL,
    [D4From] datetime  NULL,
    [D4To] datetime  NULL,
    [D5From] datetime  NULL,
    [D5To] datetime  NULL,
    [D6From] datetime  NULL,
    [D6To] datetime  NULL,
    [D7From] datetime  NULL,
    [D7To] datetime  NULL
);
GO

-- Creating table 'A_DictionaryJobTitles'
CREATE TABLE [dbo].[A_DictionaryJobTitles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JobTitle] varchar(20)  NOT NULL,
    [Code] char(4)  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [New] int  NOT NULL
);
GO

-- Creating table 'A_DictionarySpecializations'
CREATE TABLE [dbo].[A_DictionarySpecializations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Specialization] varchar(20)  NOT NULL,
    [Code] char(7)  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [New] int  NOT NULL
);
GO

-- Creating table 'A_Workers'
CREATE TABLE [dbo].[A_Workers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LastName] varchar(50)  NOT NULL,
    [FirstName] varchar(25)  NOT NULL,
    [SecondName] varchar(25)  NULL,
    [BirthDate] datetime  NOT NULL,
    [Gender] bit  NOT NULL,
    [Pesel] bigint  NOT NULL,
    [JobTitle] int  NOT NULL,
    [Specialization] int  NOT NULL,
    [Street] varchar(30)  NULL,
    [BuildingNumber] varchar(5)  NOT NULL,
    [Apartment] varchar(5)  NULL,
    [PostalCode] char(6)  NOT NULL,
    [City] varchar(50)  NOT NULL,
    [Post] varchar(50)  NULL
);
GO

-- Creating table 'M_Visits'
CREATE TABLE [dbo].[M_Visits] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RegistrarId] int  NOT NULL,
    [DoctorId] int  NOT NULL,
    [PatientId] int  NOT NULL,
    [Registered] datetime  NOT NULL,
    [DateOfVisit] datetime  NOT NULL,
    [Started] datetime  NULL,
    [State] tinyint  NOT NULL,
    [Description] varchar(max)  NULL,
    [Diagnosis] varchar(max)  NULL
);
GO

-- Creating table 'M_MedicalTreatments'
CREATE TABLE [dbo].[M_MedicalTreatments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [VisitId] int  NOT NULL,
    [DoctorId] int  NOT NULL,
    [PatientId] int  NOT NULL,
    [DoerId] int  NOT NULL,
    [Ordered] datetime  NOT NULL,
    [DateOfExecution] datetime  NULL,
    [Executed] datetime  NULL,
    [State] tinyint  NOT NULL,
    [MedicalTreatment] int  NOT NULL,
    [IsPrivate] bit  NOT NULL,
    [Description] varchar(max)  NULL,
    [Result] varchar(max)  NULL,
    [Comments] varchar(max)  NULL
);
GO

-- Creating table 'M_Patients'
CREATE TABLE [dbo].[M_Patients] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LastName] varchar(50)  NOT NULL,
    [FirstName] varchar(25)  NOT NULL,
    [SecondName] varchar(25)  NULL,
    [BirthDate] datetime  NOT NULL,
    [Gender] bit  NOT NULL,
    [Pesel] bigint  NOT NULL,
    [Street] varchar(30)  NULL,
    [BuildingNumber] varchar(5)  NOT NULL,
    [Apartment] varchar(5)  NULL,
    [PostalCode] char(6)  NOT NULL,
    [City] varchar(50)  NOT NULL,
    [Post] varchar(50)  NULL,
    [IsInsured] bit  NOT NULL,
    [NfzBranch] varchar(5)  NOT NULL
);
GO

-- Creating table 'M_Prescriptions'
CREATE TABLE [dbo].[M_Prescriptions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [VisitId] int  NOT NULL,
    [Number] varchar(15)  NOT NULL
);
GO

-- Creating table 'M_L4Diseases'
CREATE TABLE [dbo].[M_L4Diseases] (
    [VisitId] int  NOT NULL,
    [Disease] int  NOT NULL,
    [L4Number] varchar(15)  NOT NULL
);
GO

-- Creating table 'M_DictionaryDiseases'
CREATE TABLE [dbo].[M_DictionaryDiseases] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] varchar(5)  NOT NULL,
    [Name] varchar(150)  NOT NULL,
    [Description] varchar(150)  NULL,
    [New] int  NOT NULL
);
GO

-- Creating table 'M_DictionaryMedicalTreatments'
CREATE TABLE [dbo].[M_DictionaryMedicalTreatments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] varchar(5)  NOT NULL,
    [Name] varchar(150)  NOT NULL,
    [Description] varchar(150)  NULL,
    [Type] char(1)  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [New] int  NOT NULL
);
GO

-- Creating table 'A_WorkersRooms'
CREATE TABLE [dbo].[A_WorkersRooms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RoomId] int  NOT NULL,
    [WorkerId] int  NOT NULL,
    [DateFrom] datetime  NOT NULL,
    [DateTo] datetime  NULL
);
GO

-- Creating table 'A_DictionaryRooms'
CREATE TABLE [dbo].[A_DictionaryRooms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Number] varchar(5)  NOT NULL,
    [ClinicId] int  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [New] int  NOT NULL
);
GO

-- Creating table 'M_DictionaryClinics'
CREATE TABLE [dbo].[M_DictionaryClinics] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(40)  NOT NULL,
    [IsNfzContracted] bit  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [New] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'A_Calendars'
ALTER TABLE [dbo].[A_Calendars]
ADD CONSTRAINT [PK_A_Calendars]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'A_Absences'
ALTER TABLE [dbo].[A_Absences]
ADD CONSTRAINT [PK_A_Absences]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'A_Users'
ALTER TABLE [dbo].[A_Users]
ADD CONSTRAINT [PK_A_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'A_Schedules'
ALTER TABLE [dbo].[A_Schedules]
ADD CONSTRAINT [PK_A_Schedules]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'A_DictionaryJobTitles'
ALTER TABLE [dbo].[A_DictionaryJobTitles]
ADD CONSTRAINT [PK_A_DictionaryJobTitles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'A_DictionarySpecializations'
ALTER TABLE [dbo].[A_DictionarySpecializations]
ADD CONSTRAINT [PK_A_DictionarySpecializations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'A_Workers'
ALTER TABLE [dbo].[A_Workers]
ADD CONSTRAINT [PK_A_Workers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'M_Visits'
ALTER TABLE [dbo].[M_Visits]
ADD CONSTRAINT [PK_M_Visits]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'M_MedicalTreatments'
ALTER TABLE [dbo].[M_MedicalTreatments]
ADD CONSTRAINT [PK_M_MedicalTreatments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'M_Patients'
ALTER TABLE [dbo].[M_Patients]
ADD CONSTRAINT [PK_M_Patients]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'M_Prescriptions'
ALTER TABLE [dbo].[M_Prescriptions]
ADD CONSTRAINT [PK_M_Prescriptions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [VisitId] in table 'M_L4Diseases'
ALTER TABLE [dbo].[M_L4Diseases]
ADD CONSTRAINT [PK_M_L4Diseases]
    PRIMARY KEY CLUSTERED ([VisitId] ASC);
GO

-- Creating primary key on [Id] in table 'M_DictionaryDiseases'
ALTER TABLE [dbo].[M_DictionaryDiseases]
ADD CONSTRAINT [PK_M_DictionaryDiseases]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'M_DictionaryMedicalTreatments'
ALTER TABLE [dbo].[M_DictionaryMedicalTreatments]
ADD CONSTRAINT [PK_M_DictionaryMedicalTreatments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'A_WorkersRooms'
ALTER TABLE [dbo].[A_WorkersRooms]
ADD CONSTRAINT [PK_A_WorkersRooms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'A_DictionaryRooms'
ALTER TABLE [dbo].[A_DictionaryRooms]
ADD CONSTRAINT [PK_A_DictionaryRooms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'M_DictionaryClinics'
ALTER TABLE [dbo].[M_DictionaryClinics]
ADD CONSTRAINT [PK_M_DictionaryClinics]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [WorkerId] in table 'A_Absences'
ALTER TABLE [dbo].[A_Absences]
ADD CONSTRAINT [FK_A_Absence_A_Worker]
    FOREIGN KEY ([WorkerId])
    REFERENCES [dbo].[A_Workers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_A_Absence_A_Worker'
CREATE INDEX [IX_FK_A_Absence_A_Worker]
ON [dbo].[A_Absences]
    ([WorkerId]);
GO

-- Creating foreign key on [WorkerId] in table 'A_Users'
ALTER TABLE [dbo].[A_Users]
ADD CONSTRAINT [FK_A_User_A_Worker]
    FOREIGN KEY ([WorkerId])
    REFERENCES [dbo].[A_Workers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_A_User_A_Worker'
CREATE INDEX [IX_FK_A_User_A_Worker]
ON [dbo].[A_Users]
    ([WorkerId]);
GO

-- Creating foreign key on [JobTitle] in table 'A_Workers'
ALTER TABLE [dbo].[A_Workers]
ADD CONSTRAINT [FK_A_Worker_A_DictionaryJobTitle]
    FOREIGN KEY ([JobTitle])
    REFERENCES [dbo].[A_DictionaryJobTitles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_A_Worker_A_DictionaryJobTitle'
CREATE INDEX [IX_FK_A_Worker_A_DictionaryJobTitle]
ON [dbo].[A_Workers]
    ([JobTitle]);
GO

-- Creating foreign key on [Specialization] in table 'A_Workers'
ALTER TABLE [dbo].[A_Workers]
ADD CONSTRAINT [FK_A_Worker_A_DictionarySpecialization]
    FOREIGN KEY ([Specialization])
    REFERENCES [dbo].[A_DictionarySpecializations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_A_Worker_A_DictionarySpecialization'
CREATE INDEX [IX_FK_A_Worker_A_DictionarySpecialization]
ON [dbo].[A_Workers]
    ([Specialization]);
GO

-- Creating foreign key on [RegistrarId] in table 'M_Visits'
ALTER TABLE [dbo].[M_Visits]
ADD CONSTRAINT [FK_M_Visit_A_Worker_RegistrarID]
    FOREIGN KEY ([RegistrarId])
    REFERENCES [dbo].[A_Workers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_M_Visit_A_Worker_RegistrarID'
CREATE INDEX [IX_FK_M_Visit_A_Worker_RegistrarID]
ON [dbo].[M_Visits]
    ([RegistrarId]);
GO

-- Creating foreign key on [DoctorId] in table 'M_Visits'
ALTER TABLE [dbo].[M_Visits]
ADD CONSTRAINT [FK_M_Visit_A_Worker_DoctorId]
    FOREIGN KEY ([DoctorId])
    REFERENCES [dbo].[A_Workers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_M_Visit_A_Worker_DoctorId'
CREATE INDEX [IX_FK_M_Visit_A_Worker_DoctorId]
ON [dbo].[M_Visits]
    ([DoctorId]);
GO

-- Creating foreign key on [PatientId] in table 'M_Visits'
ALTER TABLE [dbo].[M_Visits]
ADD CONSTRAINT [FK_M_Visit_M_Patient]
    FOREIGN KEY ([PatientId])
    REFERENCES [dbo].[M_Patients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_M_Visit_M_Patient'
CREATE INDEX [IX_FK_M_Visit_M_Patient]
ON [dbo].[M_Visits]
    ([PatientId]);
GO

-- Creating foreign key on [VisitId] in table 'M_MedicalTreatments'
ALTER TABLE [dbo].[M_MedicalTreatments]
ADD CONSTRAINT [FK_M_MedicalTreatment_M_Visit]
    FOREIGN KEY ([VisitId])
    REFERENCES [dbo].[M_Visits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_M_MedicalTreatment_M_Visit'
CREATE INDEX [IX_FK_M_MedicalTreatment_M_Visit]
ON [dbo].[M_MedicalTreatments]
    ([VisitId]);
GO

-- Creating foreign key on [DoctorId] in table 'M_MedicalTreatments'
ALTER TABLE [dbo].[M_MedicalTreatments]
ADD CONSTRAINT [FK_M_MedicalTreatment_A_Worker_DoctorId]
    FOREIGN KEY ([DoctorId])
    REFERENCES [dbo].[A_Workers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_M_MedicalTreatment_A_Worker_DoctorId'
CREATE INDEX [IX_FK_M_MedicalTreatment_A_Worker_DoctorId]
ON [dbo].[M_MedicalTreatments]
    ([DoctorId]);
GO

-- Creating foreign key on [PatientId] in table 'M_MedicalTreatments'
ALTER TABLE [dbo].[M_MedicalTreatments]
ADD CONSTRAINT [FK_M_MedicalTreatment_M_Patient]
    FOREIGN KEY ([PatientId])
    REFERENCES [dbo].[M_Patients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_M_MedicalTreatment_M_Patient'
CREATE INDEX [IX_FK_M_MedicalTreatment_M_Patient]
ON [dbo].[M_MedicalTreatments]
    ([PatientId]);
GO

-- Creating foreign key on [DoerId] in table 'M_MedicalTreatments'
ALTER TABLE [dbo].[M_MedicalTreatments]
ADD CONSTRAINT [FK_M_MedicalTreatment_A_Worker_DoerId]
    FOREIGN KEY ([DoerId])
    REFERENCES [dbo].[A_Workers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_M_MedicalTreatment_A_Worker_DoerId'
CREATE INDEX [IX_FK_M_MedicalTreatment_A_Worker_DoerId]
ON [dbo].[M_MedicalTreatments]
    ([DoerId]);
GO

-- Creating foreign key on [MedicalTreatment] in table 'M_MedicalTreatments'
ALTER TABLE [dbo].[M_MedicalTreatments]
ADD CONSTRAINT [FK_M_MedicalTreatment_M_DictionaryMedicalTreatment]
    FOREIGN KEY ([MedicalTreatment])
    REFERENCES [dbo].[M_DictionaryMedicalTreatments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_M_MedicalTreatment_M_DictionaryMedicalTreatment'
CREATE INDEX [IX_FK_M_MedicalTreatment_M_DictionaryMedicalTreatment]
ON [dbo].[M_MedicalTreatments]
    ([MedicalTreatment]);
GO

-- Creating foreign key on [VisitId] in table 'M_L4Diseases'
ALTER TABLE [dbo].[M_L4Diseases]
ADD CONSTRAINT [FK_M_L4Disease_M_Visit]
    FOREIGN KEY ([VisitId])
    REFERENCES [dbo].[M_Visits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Disease] in table 'M_L4Diseases'
ALTER TABLE [dbo].[M_L4Diseases]
ADD CONSTRAINT [FK_M_L4Disease_M_DictionaryDisease]
    FOREIGN KEY ([Disease])
    REFERENCES [dbo].[M_DictionaryDiseases]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_M_L4Disease_M_DictionaryDisease'
CREATE INDEX [IX_FK_M_L4Disease_M_DictionaryDisease]
ON [dbo].[M_L4Diseases]
    ([Disease]);
GO

-- Creating foreign key on [VisitId] in table 'M_Prescriptions'
ALTER TABLE [dbo].[M_Prescriptions]
ADD CONSTRAINT [FK_M_Prescription_M_Visit]
    FOREIGN KEY ([VisitId])
    REFERENCES [dbo].[M_Visits]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_M_Prescription_M_Visit'
CREATE INDEX [IX_FK_M_Prescription_M_Visit]
ON [dbo].[M_Prescriptions]
    ([VisitId]);
GO

-- Creating foreign key on [RoomId] in table 'A_WorkersRooms'
ALTER TABLE [dbo].[A_WorkersRooms]
ADD CONSTRAINT [FK_A_WorkersRoom_A_DictionaryRoom]
    FOREIGN KEY ([RoomId])
    REFERENCES [dbo].[A_DictionaryRooms]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_A_WorkersRoom_A_DictionaryRoom'
CREATE INDEX [IX_FK_A_WorkersRoom_A_DictionaryRoom]
ON [dbo].[A_WorkersRooms]
    ([RoomId]);
GO

-- Creating foreign key on [WorkerId] in table 'A_WorkersRooms'
ALTER TABLE [dbo].[A_WorkersRooms]
ADD CONSTRAINT [FK_A_WorkersRoom_A_Worker]
    FOREIGN KEY ([WorkerId])
    REFERENCES [dbo].[A_Workers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_A_WorkersRoom_A_Worker'
CREATE INDEX [IX_FK_A_WorkersRoom_A_Worker]
ON [dbo].[A_WorkersRooms]
    ([WorkerId]);
GO

-- Creating foreign key on [ClinicId] in table 'A_DictionaryRooms'
ALTER TABLE [dbo].[A_DictionaryRooms]
ADD CONSTRAINT [FK_A_DictionaryRoom_M_DictionaryClinic]
    FOREIGN KEY ([ClinicId])
    REFERENCES [dbo].[M_DictionaryClinics]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_A_DictionaryRoom_M_DictionaryClinic'
CREATE INDEX [IX_FK_A_DictionaryRoom_M_DictionaryClinic]
ON [dbo].[A_DictionaryRooms]
    ([ClinicId]);
GO

-- Creating foreign key on [WorkerId] in table 'A_Schedules'
ALTER TABLE [dbo].[A_Schedules]
ADD CONSTRAINT [FK_A_Schedule_A_Worker]
    FOREIGN KEY ([WorkerId])
    REFERENCES [dbo].[A_Workers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_A_Schedule_A_Worker'
CREATE INDEX [IX_FK_A_Schedule_A_Worker]
ON [dbo].[A_Schedules]
    ([WorkerId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------