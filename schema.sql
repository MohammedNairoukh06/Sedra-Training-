/* =========================================================
   TicketDesk - schema.sql
   Week 2, Day 5 - Build the Schema
   Creates all tables with PRIMARY KEY / FOREIGN KEY constraints
   ========================================================= */

-- Drop tables if they already exist (children first, then parents)
DROP TABLE IF EXISTS TicketCategory;
DROP TABLE IF EXISTS TicketComment;
DROP TABLE IF EXISTS Ticket;
DROP TABLE IF EXISTS UserRole;
DROP TABLE IF EXISTS Category;
DROP TABLE IF EXISTS [User];
DROP TABLE IF EXISTS Role;
GO

-- =========================================================
-- Role
-- =========================================================
CREATE TABLE Role (
    RoleId      INT IDENTITY(1,1) PRIMARY KEY,
    RoleName    VARCHAR(50) NOT NULL UNIQUE
);
GO

-- =========================================================
-- User
-- =========================================================
CREATE TABLE [User] (
    UserId       INT IDENTITY(1,1) PRIMARY KEY,
    Username     VARCHAR(50)  NOT NULL UNIQUE,
    Email        VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    CreatedAt    DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- =========================================================
-- UserRole (link table: User M:N Role)
-- =========================================================
CREATE TABLE UserRole (
    UserId  INT NOT NULL,
    RoleId  INT NOT NULL,
    CONSTRAINT PK_UserRole PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_UserRole_User FOREIGN KEY (UserId)
        REFERENCES [User](UserId) ON DELETE CASCADE,
    CONSTRAINT FK_UserRole_Role FOREIGN KEY (RoleId)
        REFERENCES Role(RoleId) ON DELETE CASCADE
);
GO

-- =========================================================
-- Category
-- =========================================================
CREATE TABLE Category (
    CategoryId   INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName VARCHAR(50) NOT NULL UNIQUE
);
GO

-- =========================================================
-- Ticket (User 1:M Ticket)
-- =========================================================
CREATE TABLE Ticket (
    TicketId    INT IDENTITY(1,1) PRIMARY KEY,
    Title       VARCHAR(150) NOT NULL,
    Description VARCHAR(1000) NULL,
    Status      VARCHAR(20) NOT NULL DEFAULT 'Open'
                CHECK (Status IN ('Open', 'InProgress', 'Resolved', 'Closed')),
    CreatedAt   DATETIME NOT NULL DEFAULT GETDATE(),
    UserId      INT NOT NULL,
    CONSTRAINT FK_Ticket_User FOREIGN KEY (UserId)
        REFERENCES [User](UserId) ON DELETE CASCADE
);
GO

-- =========================================================
-- TicketComment (Ticket 1:M TicketComment)
-- =========================================================
CREATE TABLE TicketComment (
    CommentId   INT IDENTITY(1,1) PRIMARY KEY,
    TicketId    INT NOT NULL,
    UserId      INT NOT NULL,
    CommentText VARCHAR(1000) NOT NULL,
    CreatedAt   DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Comment_Ticket FOREIGN KEY (TicketId)
        REFERENCES Ticket(TicketId) ON DELETE CASCADE,
    CONSTRAINT FK_Comment_User FOREIGN KEY (UserId)
        REFERENCES [User](UserId) ON DELETE NO ACTION
);
GO

-- =========================================================
-- TicketCategory (link table: Ticket M:N Category)
-- =========================================================
CREATE TABLE TicketCategory (
    TicketId    INT NOT NULL,
    CategoryId  INT NOT NULL,
    CONSTRAINT PK_TicketCategory PRIMARY KEY (TicketId, CategoryId),
    CONSTRAINT FK_TicketCategory_Ticket FOREIGN KEY (TicketId)
        REFERENCES Ticket(TicketId) ON DELETE CASCADE,
    CONSTRAINT FK_TicketCategory_Category FOREIGN KEY (CategoryId)
        REFERENCES Category(CategoryId) ON DELETE CASCADE
);
GO
