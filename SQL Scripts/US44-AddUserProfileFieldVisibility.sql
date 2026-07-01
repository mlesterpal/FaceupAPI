IF COL_LENGTH('dbo.Users', 'BioVisibility') IS NULL
BEGIN
    ALTER TABLE dbo.Users ADD BioVisibility NVARCHAR(20) NOT NULL CONSTRAINT DF_Users_BioVisibility DEFAULT 'Public';
END
GO

IF COL_LENGTH('dbo.Users', 'AddressVisibility') IS NULL
BEGIN
    ALTER TABLE dbo.Users ADD AddressVisibility NVARCHAR(20) NOT NULL CONSTRAINT DF_Users_AddressVisibility DEFAULT 'Public';
END
GO

IF COL_LENGTH('dbo.Users', 'WorkVisibility') IS NULL
BEGIN
    ALTER TABLE dbo.Users ADD WorkVisibility NVARCHAR(20) NOT NULL CONSTRAINT DF_Users_WorkVisibility DEFAULT 'Public';
END
GO

IF COL_LENGTH('dbo.Users', 'HighSchoolVisibility') IS NULL
BEGIN
    ALTER TABLE dbo.Users ADD HighSchoolVisibility NVARCHAR(20) NOT NULL CONSTRAINT DF_Users_HighSchoolVisibility DEFAULT 'Public';
END
GO

IF COL_LENGTH('dbo.Users', 'CollegeVisibility') IS NULL
BEGIN
    ALTER TABLE dbo.Users ADD CollegeVisibility NVARCHAR(20) NOT NULL CONSTRAINT DF_Users_CollegeVisibility DEFAULT 'Public';
END
GO

IF COL_LENGTH('dbo.Users', 'HobbiesVisibility') IS NULL
BEGIN
    ALTER TABLE dbo.Users ADD HobbiesVisibility NVARCHAR(20) NOT NULL CONSTRAINT DF_Users_HobbiesVisibility DEFAULT 'Public';
END
GO

IF COL_LENGTH('dbo.Users', 'PhoneVisibility') IS NULL
BEGIN
    ALTER TABLE dbo.Users ADD PhoneVisibility NVARCHAR(20) NOT NULL CONSTRAINT DF_Users_PhoneVisibility DEFAULT 'Public';
END
GO

IF COL_LENGTH('dbo.Users', 'GenderVisibility') IS NULL
BEGIN
    ALTER TABLE dbo.Users ADD GenderVisibility NVARCHAR(20) NOT NULL CONSTRAINT DF_Users_GenderVisibility DEFAULT 'Public';
END
GO

IF COL_LENGTH('dbo.Users', 'BirthDateVisibility') IS NULL
BEGIN
    ALTER TABLE dbo.Users ADD BirthDateVisibility NVARCHAR(20) NOT NULL CONSTRAINT DF_Users_BirthDateVisibility DEFAULT 'Public';
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.check_constraints
    WHERE name = 'CK_Users_ProfileVisibilityValues'
)
BEGIN
    ALTER TABLE dbo.Users ADD CONSTRAINT CK_Users_ProfileVisibilityValues CHECK (
        BioVisibility IN ('Public', 'Private')
        AND AddressVisibility IN ('Public', 'Private')
        AND WorkVisibility IN ('Public', 'Private')
        AND HighSchoolVisibility IN ('Public', 'Private')
        AND CollegeVisibility IN ('Public', 'Private')
        AND HobbiesVisibility IN ('Public', 'Private')
        AND PhoneVisibility IN ('Public', 'Private')
        AND GenderVisibility IN ('Public', 'Private')
        AND BirthDateVisibility IN ('Public', 'Private')
    );
END
GO
