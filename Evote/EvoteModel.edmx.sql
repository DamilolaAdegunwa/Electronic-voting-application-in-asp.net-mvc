
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/16/2019 19:20:19
-- Generated from EDMX file: C:\Users\DAMILOLA\source\repos\Evote\Evote\EvoteModel.edmx
-- --------------------------------------------------
CREATE DATABASE [hr];
GO
SET QUOTED_IDENTIFIER OFF;
GO
USE [hr];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Committee_member_EligibleMember]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Committee_member] DROP CONSTRAINT [FK_Committee_member_EligibleMember];
GO
IF OBJECT_ID(N'[dbo].[FK_Contestant_EligibleMember]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Contestant] DROP CONSTRAINT [FK_Contestant_EligibleMember];
GO
IF OBJECT_ID(N'[dbo].[FK_Contestant_ToVotingSession]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Contestant] DROP CONSTRAINT [FK_Contestant_ToVotingSession];
GO
IF OBJECT_ID(N'[dbo].[FK_voteLog_Contestant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[voteLog] DROP CONSTRAINT [FK_voteLog_Contestant];
GO
IF OBJECT_ID(N'[dbo].[FK_voteLog_EligibleMember]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[voteLog] DROP CONSTRAINT [FK_voteLog_EligibleMember];
GO
IF OBJECT_ID(N'[dbo].[FK_voteLog_VotingSession]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[voteLog] DROP CONSTRAINT [FK_voteLog_VotingSession];
GO
IF OBJECT_ID(N'[dbo].[FK_VotingSession_Position]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VotingSession] DROP CONSTRAINT [FK_VotingSession_Position];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[activityLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[activityLog];
GO
IF OBJECT_ID(N'[dbo].[Admin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Admin];
GO
IF OBJECT_ID(N'[dbo].[BI]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BI];
GO
IF OBJECT_ID(N'[dbo].[Committee_member]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Committee_member];
GO
IF OBJECT_ID(N'[dbo].[Contestant]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contestant];
GO
IF OBJECT_ID(N'[dbo].[EligibleMember]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EligibleMember];
GO
IF OBJECT_ID(N'[dbo].[membershipType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[membershipType];
GO
IF OBJECT_ID(N'[dbo].[Position]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Position];
GO
IF OBJECT_ID(N'[dbo].[post]', 'U') IS NOT NULL
    DROP TABLE [dbo].[post];
GO
IF OBJECT_ID(N'[dbo].[voteLog]', 'U') IS NOT NULL
    DROP TABLE [dbo].[voteLog];
GO
IF OBJECT_ID(N'[dbo].[VotingSession]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VotingSession];
GO
IF OBJECT_ID(N'[dbo].[winner]', 'U') IS NOT NULL
    DROP TABLE [dbo].[winner];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'activityLogs'
CREATE TABLE [dbo].[activityLogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [fullname] varchar(max)  NULL,
    [membership_number] varchar(max)  NULL,
    [datetime] datetime  NULL,
    [contestantId] varchar(max)  NULL
);
GO

-- Creating table 'Admins'
CREATE TABLE [dbo].[Admins] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] varchar(max)  NULL,
    [password] varchar(max)  NULL
);
GO

-- Creating table 'BIs'
CREATE TABLE [dbo].[BIs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [totalVote] int  NULL,
    [totalContestants] int  NULL
);
GO

-- Creating table 'Committee_member'
CREATE TABLE [dbo].[Committee_member] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [cmvn] varchar(max)  NULL,
    [MemberId] int  NULL
);
GO

-- Creating table 'Contestants'
CREATE TABLE [dbo].[Contestants] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [VotingSessionId] int  NULL,
    [manifesto] varchar(max)  NULL,
    [CampaignPictureName] varchar(max)  NULL,
    [CampaignPictureContent] varbinary(max)  NULL,
    [CampaignPictureFileType] varchar(max)  NULL,
    [voteNumber] int  NULL,
    [BIId] int  NULL,
    [percentageVote] decimal(18,2)  NULL,
    [PositionId] int  NULL,
    [status] bit  NULL,
    [MemberId] int  NULL,
    [email] varchar(max)  NULL,
    [password] varchar(max)  NULL
);
GO

-- Creating table 'EligibleMembers'
CREATE TABLE [dbo].[EligibleMembers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [fullname] varchar(max)  NULL,
    [phone_number] varchar(max)  NULL,
    [membership_number] varchar(max)  NULL,
    [email] varchar(max)  NULL,
    [password] varchar(max)  NULL,
    [confirmPassword] varchar(max)  NULL,
    [hash] varchar(max)  NULL,
    [vote_status] varchar(max)  NULL,
    [ImageName] varchar(max)  NULL,
    [ImageContent] varbinary(max)  NULL,
    [ImageFileType] varchar(max)  NULL
);
GO

-- Creating table 'membershipTypes'
CREATE TABLE [dbo].[membershipTypes] (
    [Id] int  NOT NULL,
    [Type] varchar(max)  NULL
);
GO

-- Creating table 'Positions'
CREATE TABLE [dbo].[Positions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Post] varchar(max)  NULL,
    [Description] varchar(max)  NULL
);
GO

-- Creating table 'posts'
CREATE TABLE [dbo].[posts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [postTitle] varchar(max)  NULL,
    [postDescription] varchar(max)  NULL,
    [postImageName] varchar(max)  NULL,
    [postImageContent] varbinary(max)  NULL,
    [postImageFileType] varchar(max)  NULL,
    [startDate] datetime  NULL,
    [closeDate] datetime  NULL,
    [numberOfContestants] int  NULL,
    [voteStatus] bit  NULL
);
GO

-- Creating table 'voteLogs'
CREATE TABLE [dbo].[voteLogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [datetime] datetime  NULL,
    [contestantId] int  NULL,
    [VotingSessionId] int  NULL,
    [memberId] int  NULL
);
GO

-- Creating table 'VotingSessions'
CREATE TABLE [dbo].[VotingSessions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [postImageName] varchar(max)  NULL,
    [postImageContent] varbinary(max)  NULL,
    [postImageFileType] varchar(max)  NULL,
    [startDate] datetime  NULL,
    [startTime] time  NULL,
    [closeDate] datetime  NULL,
    [closeTime] time  NULL,
    [startDateTime] datetime  NULL,
    [closeDateTime] datetime  NULL,
    [PositionId] int  NULL,
    [status] bit  NULL,
    [moment] varchar(max)  NULL,
    [BIId] int  NULL,
    [VotingSessionName] varchar(max)  NULL
);
GO

-- Creating table 'winners'
CREATE TABLE [dbo].[winners] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContestantId] varchar(max)  NULL,
    [BIId] varchar(max)  NULL,
    [date] datetime  NULL,
    [time] time  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'activityLogs'
ALTER TABLE [dbo].[activityLogs]
ADD CONSTRAINT [PK_activityLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Admins'
ALTER TABLE [dbo].[Admins]
ADD CONSTRAINT [PK_Admins]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BIs'
ALTER TABLE [dbo].[BIs]
ADD CONSTRAINT [PK_BIs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Committee_member'
ALTER TABLE [dbo].[Committee_member]
ADD CONSTRAINT [PK_Committee_member]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Contestants'
ALTER TABLE [dbo].[Contestants]
ADD CONSTRAINT [PK_Contestants]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EligibleMembers'
ALTER TABLE [dbo].[EligibleMembers]
ADD CONSTRAINT [PK_EligibleMembers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'membershipTypes'
ALTER TABLE [dbo].[membershipTypes]
ADD CONSTRAINT [PK_membershipTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Positions'
ALTER TABLE [dbo].[Positions]
ADD CONSTRAINT [PK_Positions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'posts'
ALTER TABLE [dbo].[posts]
ADD CONSTRAINT [PK_posts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'voteLogs'
ALTER TABLE [dbo].[voteLogs]
ADD CONSTRAINT [PK_voteLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VotingSessions'
ALTER TABLE [dbo].[VotingSessions]
ADD CONSTRAINT [PK_VotingSessions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'winners'
ALTER TABLE [dbo].[winners]
ADD CONSTRAINT [PK_winners]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [MemberId] in table 'Committee_member'
ALTER TABLE [dbo].[Committee_member]
ADD CONSTRAINT [FK_Committee_member_EligibleMember]
    FOREIGN KEY ([MemberId])
    REFERENCES [dbo].[EligibleMembers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Committee_member_EligibleMember'
CREATE INDEX [IX_FK_Committee_member_EligibleMember]
ON [dbo].[Committee_member]
    ([MemberId]);
GO

-- Creating foreign key on [MemberId] in table 'Contestants'
ALTER TABLE [dbo].[Contestants]
ADD CONSTRAINT [FK_Contestant_EligibleMember]
    FOREIGN KEY ([MemberId])
    REFERENCES [dbo].[EligibleMembers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Contestant_EligibleMember'
CREATE INDEX [IX_FK_Contestant_EligibleMember]
ON [dbo].[Contestants]
    ([MemberId]);
GO

-- Creating foreign key on [VotingSessionId] in table 'Contestants'
ALTER TABLE [dbo].[Contestants]
ADD CONSTRAINT [FK_Contestant_ToVotingSession]
    FOREIGN KEY ([VotingSessionId])
    REFERENCES [dbo].[VotingSessions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Contestant_ToVotingSession'
CREATE INDEX [IX_FK_Contestant_ToVotingSession]
ON [dbo].[Contestants]
    ([VotingSessionId]);
GO

-- Creating foreign key on [contestantId] in table 'voteLogs'
ALTER TABLE [dbo].[voteLogs]
ADD CONSTRAINT [FK_voteLog_Contestant]
    FOREIGN KEY ([contestantId])
    REFERENCES [dbo].[Contestants]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_voteLog_Contestant'
CREATE INDEX [IX_FK_voteLog_Contestant]
ON [dbo].[voteLogs]
    ([contestantId]);
GO

-- Creating foreign key on [memberId] in table 'voteLogs'
ALTER TABLE [dbo].[voteLogs]
ADD CONSTRAINT [FK_voteLog_EligibleMember]
    FOREIGN KEY ([memberId])
    REFERENCES [dbo].[EligibleMembers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_voteLog_EligibleMember'
CREATE INDEX [IX_FK_voteLog_EligibleMember]
ON [dbo].[voteLogs]
    ([memberId]);
GO

-- Creating foreign key on [PositionId] in table 'VotingSessions'
ALTER TABLE [dbo].[VotingSessions]
ADD CONSTRAINT [FK_VotingSession_Position]
    FOREIGN KEY ([PositionId])
    REFERENCES [dbo].[Positions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VotingSession_Position'
CREATE INDEX [IX_FK_VotingSession_Position]
ON [dbo].[VotingSessions]
    ([PositionId]);
GO

-- Creating foreign key on [VotingSessionId] in table 'voteLogs'
ALTER TABLE [dbo].[voteLogs]
ADD CONSTRAINT [FK_voteLog_VotingSession]
    FOREIGN KEY ([VotingSessionId])
    REFERENCES [dbo].[VotingSessions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_voteLog_VotingSession'
CREATE INDEX [IX_FK_voteLog_VotingSession]
ON [dbo].[voteLogs]
    ([VotingSessionId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------