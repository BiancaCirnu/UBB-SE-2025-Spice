CREATE TABLE UserSessions (
    session_id INT PRIMARY KEY IDENTITY(1,1), 
    user_id INT NOT NULL,
    created_at DATETIME NOT NULL DEFAULT GETDATE(),  
    expires_at DATETIME NOT NULL,
    last_activity_time DATETIME NOT NULL DEFAULT GETDATE(), 
    is_active BIT NOT NULL DEFAULT 1,
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);
