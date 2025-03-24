-- public class ToDoListEntry
-- {
--     public int Id { get; set; }
--     public int ToDoListId { get; set; } // Foreign key to the ToDoList table
--     public string Title { get; set; } = string.Empty;
--     public string Description { get; set; } = string.Empty;
--     public EntryStatus Status { get; set; } = EntryStatus.Active;
--     public bool IsCompleted => Status == EntryStatus.Completed;
--     public bool IsDeleted { get; set; } = false; // Soft delete flag
--     public DateTime? DeletedAt { get; set; } // Timestamp for when the item was deleted
-- }


-- public class User
-- {
--     public int Id { get; set; }
--     public string Username { get; set; } = string.Empty;
--     public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
--     public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
--     public DateTime CreatedAt { get; set; }
-- }

        -- using (var hmac = new System.Security.Cryptography.HMACSHA512())
        -- {
        --     passwordSalt = hmac.Key;
        --     passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        -- }


CREATE TABLE [Users] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash VARBINARY(64) NOT NULL,
    PasswordSalt VARBINARY(128) NOT NULL,
    CreatedAt [DateTime] DEFAULT GETDATE(),
);

select * from [Users] -- To verify the table structure

CREATE TABLE [EntryStatuses] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE
)

INSERT INTO [EntryStatuses] (Name) VALUES ('Active'), ('Completed'), ('Canceled');

-- public class ToDoList
-- {
--     public int Id { get; set; }
--     public string Title { get; set; } = string.Empty;
--     public string Description { get; set; } = string.Empty;
--     public DateTime CreatedAt { get; set; }
--     public DateTime? CompletedAt { get; set; }
--     public EntryStatus Status { get; set; } = EntryStatus.Active;
--     public bool IsCompleted => Status == EntryStatus.Completed;
--     public int UserId { get; set; } // Foreign key to the User table
--     public bool IsDeleted { get; set; } = false; // Soft delete flag
--     public DateTime? DeletedAt { get; set; } // Timestamp for when the item was deleted
-- }

CREATE TABLE [ToDoLists] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Description TEXT NOT NULL,
    CreatedAt [DateTime] DEFAULT GETDATE(),
    CompletedAt [DateTime] NULL,
    [Status] INT NOT NULL DEFAULT 1, -- Default to Active status
    UserId INT NOT NULL,
    IsDeleted BIT DEFAULT 0,
    DeletedAt DATETIME NULL,
    FOREIGN KEY (UserId) REFERENCES [Users](Id) ON DELETE CASCADE,
    FOREIGN KEY ([Status]) REFERENCES [EntryStatuses](Id) ON DELETE NO ACTION -- Assuming you want to keep the status even if the entry is deleted
)


select * from [ToDoLists] -- To verify the table structure

CREATE TABLE [ToDoListEntries] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ToDoListId INT NOT NULL,
    Title VARCHAR(255) NOT NULL,
    Description VARCHAR(4000) NOT NULL,
    [Status] INT NOT NULL DEFAULT 1, -- Default to Active status
    IsDeleted BIT DEFAULT 0,
    DeletedAt DATETIME NULL,
    FOREIGN KEY ([Status]) REFERENCES [EntryStatuses](Id) ON DELETE NO ACTION,
    FOREIGN KEY (ToDoListId) REFERENCES [ToDoLists](Id) ON DELETE CASCADE
);


SELECT Id, Username, PasswordHash, PasswordSalt FROM [Users] WHERE Username = 'bob'