ALTER TABLE Users
ADD 
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    Password NVARCHAR(255),
    Gender NVARCHAR(20),
    BirthDate DATE;

ALTER TABLE Users
DROP COLUMN Name;