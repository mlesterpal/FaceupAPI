IF EXISTS (
    SELECT 1
    FROM sys.check_constraints
    WHERE name = 'CK_Users_ProfileVisibilityValues'
)
BEGIN
    ALTER TABLE dbo.Users DROP CONSTRAINT CK_Users_ProfileVisibilityValues;
END
GO

ALTER TABLE dbo.Users ADD CONSTRAINT CK_Users_ProfileVisibilityValues CHECK (
    BioVisibility IN ('Public', 'Private', 'Pinned')
    AND AddressVisibility IN ('Public', 'Private', 'Pinned')
    AND WorkVisibility IN ('Public', 'Private', 'Pinned')
    AND HighSchoolVisibility IN ('Public', 'Private', 'Pinned')
    AND CollegeVisibility IN ('Public', 'Private', 'Pinned')
    AND HobbiesVisibility IN ('Public', 'Private', 'Pinned')
    AND PhoneVisibility IN ('Public', 'Private', 'Pinned')
    AND GenderVisibility IN ('Public', 'Private', 'Pinned')
    AND BirthDateVisibility IN ('Public', 'Private', 'Pinned')
);
GO
