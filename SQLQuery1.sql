-- Create the database
CREATE DATABASE MasterPiece;
GO

-- Use the newly created database
USE MasterPiece;
GO

-- Create the Packages table
CREATE TABLE Packages (
    Id int PRIMARY KEY identity(1,1)
);

-- Create the Billing table
CREATE TABLE Billing (
    Id int PRIMARY KEY identity(1,1)
);

-- Create the Patients table
CREATE TABLE Patients (
    Patient_ID INT PRIMARY KEY identity(1,1),
    Full_Name NVARCHAR(MAX),
    Date_Of_Birth DATE,
    Gender NVARCHAR(MAX),
    Marital_Status NVARCHAR(MAX),
    Nationality NVARCHAR(MAX),
    Phone_Number INT,
    Home_Address NVARCHAR(MAX),
    Note NVARCHAR(MAX)
);

-- Create the Tests table
CREATE TABLE Tests (
    Test_ID INT PRIMARY KEY identity(1,1),
    Test_Name NVARCHAR(MAX),
    Alternative_Name NVARCHAR(MAX) null,
    Normal_Range NVARCHAR(MAX),
    Description NVARCHAR(MAX),
    Price DECIMAL,
    Inventory DECIMAL,
    Sample_Type nvarchar,
    Expiration_Date date
);

-- Create the Appointments table
CREATE TABLE Appointments (
    ID BIGINT PRIMARY KEY,
    Full_Name NVARCHAR(MAX),
    Gender NVARCHAR(MAX),
    Date_Of_Birth DATE,
    Email_Address NVARCHAR(MAX),
    Phone_Number NVARCHAR(MAX),
    Home_Address NVARCHAR(MAX),
    Date_Of_Appo DATE,
    Tests_IDs int,
    Billing_ID int,
    Status NVARCHAR(MAX),
    FOREIGN KEY (Tests_IDs) REFERENCES Tests(Test_ID),
    FOREIGN KEY (Billing_ID) REFERENCES Billing(Id)
);

-- Create the Lab_Tech table
CREATE TABLE Lab_Tech (
    Tech_ID INT PRIMARY KEY identity(1,1),
    Name NVARCHAR(MAX),
    Email NVARCHAR(MAX),
    Password nvarchar,
    Status NVARCHAR(MAX)
);



-- Create the Test_Order table
CREATE TABLE Test_Order (
    Order_ID INT PRIMARY KEY identity(1,1),
    Patient_ID INT,
    Date DATE,
    Tech_ID INT,
    Status NVARCHAR(MAX),
    FOREIGN KEY (Patient_ID) REFERENCES Patients(Patient_ID),
    FOREIGN KEY (Tech_ID) REFERENCES Lab_Tech(Tech_ID)
);

-- Create the Test_Order_Tests table
CREATE TABLE Test_Order_Tests (
    ID INT PRIMARY KEY identity(1,1),
    Order_ID INT,
    Test_ID INT,
    Result NVARCHAR(MAX),
    Date_Of_Result DATE,
    Comment NVARCHAR(MAX),
    Status NVARCHAR(MAX),
    FOREIGN KEY (Order_ID) REFERENCES Test_Order(Order_ID),
    FOREIGN KEY (Test_ID) REFERENCES Tests(Test_ID)
);
