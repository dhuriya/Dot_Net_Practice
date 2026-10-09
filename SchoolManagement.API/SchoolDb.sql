--- Create DB
--CREATE DATABASE SchoolDb;
CREATE TABLE Students
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    admission_number VARCHAR(12) NOT NULL,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(40) NULL,
    father_name VARCHAR(50) NOT NULL,
    mother_name VARCHAR(50) NOT NULL,
    parent_phone VARCHAR(10) NOT NULL,
    date_of_birth DATE NOT NULL,
    gender CHAR(1) NOT NULL,
    email VARCHAR(50) NOT NULL,
    phone VARCHAR(10) NOT NULL,
    address VARCHAR(MAX) NOT NULL,
    class_id VARCHAR(8) NOT NULL,
    section_id VARCHAR(10) NOT NULL,
    roll_number VARCHAR(11) NOT NULL,
    admission_date DATE NULL,
    status CHAR(1) DEFAULT 'N',
    created_at DATE DEFAULT GETDATE(),
    updated_at DATE NULL
)

GO

CREATE OR ALTER PROCEDURE dbo.ManageStudent
    @Action VARCHAR(10),
    @Id INT = NULL,
    @admission_number VARCHAR(12) = NULL,
    @first_name VARCHAR(50) = NULL,
    @last_name VARCHAR(40) = NULL,
    @father_name VARCHAR(50) = NULL,
    @mother_name VARCHAR(50) = NULL,
    @parent_phone VARCHAR(10) = NULL,
    @date_of_birth DATE = NULL,
    @gender CHAR(1) = NULL,
    @email VARCHAR(50) = NULL,
    @phone VARCHAR(10) = NULL,
    @address VARCHAR(MAX) = NULL,
    @class_id VARCHAR(8) = NULL,
    @section_id VARCHAR(10) = NULL,
    @roll_number VARCHAR(11) = NULL,
    @admission_date DATE = NULL,
    @status CHAR(1) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF UPPER(@Action) = 'INSERT'
    BEGIN
        IF @admission_number IS NULL OR @first_name IS NULL OR @father_name IS NULL
            OR @mother_name IS NULL OR @parent_phone IS NULL OR @date_of_birth IS NULL
            OR @gender IS NULL OR @email IS NULL OR @phone IS NULL OR @address IS NULL
            OR @class_id IS NULL OR @section_id IS NULL OR @roll_number IS NULL
            THROW 50001, 'Required student fields must be provided for INSERT.', 1;

        INSERT INTO Students
        (
            admission_number, first_name, last_name, father_name, mother_name,
            parent_phone, date_of_birth, gender, email, phone, address,
            class_id, section_id, roll_number, admission_date
        )
        VALUES
        (
            @admission_number, @first_name, @last_name, @father_name, @mother_name,
            @parent_phone, @date_of_birth, @gender, @email, @phone, @address,
            @class_id, @section_id, @roll_number, @admission_date
        );

        SET @Id = CONVERT(INT, SCOPE_IDENTITY());
        SELECT * FROM Students WHERE id = @Id;
        RETURN;
    END;

    IF UPPER(@Action) = 'UPDATE'
    BEGIN
        IF @Id IS NULL
            THROW 50002, 'Id must be provided for UPDATE.', 1;

        IF @admission_number IS NULL OR @first_name IS NULL OR @father_name IS NULL
            OR @mother_name IS NULL OR @parent_phone IS NULL OR @date_of_birth IS NULL
            OR @gender IS NULL OR @email IS NULL OR @phone IS NULL OR @address IS NULL
            OR @class_id IS NULL OR @section_id IS NULL OR @roll_number IS NULL
            THROW 50003, 'Required student fields must be provided for UPDATE.', 1;

        UPDATE Students
        SET first_name = @first_name,last_name = @last_name,father_name = @father_name,mother_name = @mother_name,
            parent_phone = @parent_phone,date_of_birth = @date_of_birth,gender = @gender,email = @email,phone = @phone,
            address = @address,class_id = @class_id,section_id = @section_id,roll_number = @roll_number,status = COALESCE(@status, status),
            updated_at = GETDATE()
        WHERE  admission_number = @admission_number

        SELECT * FROM Students WHERE id = @Id;
        RETURN;
    END;

    IF UPPER(@Action) = 'DELETE'
    BEGIN
        IF @Id IS NULL
            THROW 50004, 'Id must be provided for DELETE.', 1;

        DELETE FROM Students WHERE id = @Id;
        SELECT @@ROWCOUNT AS affected_rows;
        RETURN;
    END;

    IF UPPER(@Action) = 'SELECT'
    BEGIN
        IF @Id IS NULL
            SELECT * FROM Students;
        ELSE
            SELECT * FROM Students WHERE id = @Id;
        RETURN;
    END;

    THROW 50005, 'Invalid action. Use INSERT, UPDATE, DELETE, or SELECT.', 1;
END;

GO

SELECT * FROM Students