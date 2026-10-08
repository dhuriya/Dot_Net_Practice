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
SELECT * FROM Students