DROP DATABASE MasterPiece

USE master;
-- Create the database
CREATE DATABASE MasterPiece;
GO

-- Use the newly created database
USE MasterPiece;
GO




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
    Note NVARCHAR(MAX),
	PaymentStatus NVARCHAR(50) DEFAULT 'Unpaid'
);

-- Create the Tests table
CREATE TABLE Tests (
    Test_ID INT PRIMARY KEY identity(1,1),
    Test_Name NVARCHAR(MAX),
    Alternative_Name NVARCHAR(MAX) null,
	Components NVARCHAR(MAX),  -- Store components as a comma-separated string
    Normal_Range NVARCHAR(MAX),
	Unit nvarchar (max),
    Description NVARCHAR(MAX),
    Price DECIMAL(10, 2),
    Inventory DECIMAL(10, 2),
    Sample_Type nvarchar(max),
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
	Total_price decimal(10, 2),
	Amount_paid DECIMAL(10, 2),
    Billing_ID INT,
    Status NVARCHAR(MAX),
);

CREATE TABLE Appointments_Tests (
	ID INT PRIMARY KEY IDENTITY(1,1),
    Appointment_ID BIGINT,
    Test_ID INT,
    FOREIGN KEY (Appointment_ID) REFERENCES Appointments(ID)ON DELETE CASCADE,
    FOREIGN KEY (Test_ID) REFERENCES Tests(Test_ID)ON DELETE CASCADE,
);


-- Create the Packages table
CREATE TABLE Packages (
    Package_ID INT PRIMARY KEY IDENTITY(1,1),
    Package_Name NVARCHAR(MAX) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(10, 2),
    Picture NVARCHAR(MAX) -- Path or URL to the package image
);

-- Create the Package_Tests table
CREATE TABLE Package_Tests (
    ID INT PRIMARY KEY IDENTITY(1,1),
    Package_ID INT,
    Test_ID INT,
    FOREIGN KEY (Package_ID) REFERENCES Packages(Package_ID) ON DELETE CASCADE,
    FOREIGN KEY (Test_ID) REFERENCES Tests(Test_ID) ON DELETE CASCADE
);

-- Create the Lab_Tech table
CREATE TABLE Lab_Tech (
    Tech_ID INT PRIMARY KEY identity(1,1),
    Name NVARCHAR(MAX),
    Email NVARCHAR(MAX),
    Password nvarchar(MAX),
    Status NVARCHAR(MAX)
);



-- Create the Test_Order table
CREATE TABLE Test_Order (
    Order_ID INT PRIMARY KEY identity(1,1),
    Patient_ID INT,
    Date DATE,
    Tech_ID INT,
	Total_Price DECIMAL(10, 2),
	Discount_Persent int DEFAULT 0,
	Amount_Paid DECIMAL(10, 2),
    Status NVARCHAR(MAX),
    FOREIGN KEY (Patient_ID) REFERENCES Patients(Patient_ID)ON DELETE CASCADE,
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
    FOREIGN KEY (Order_ID) REFERENCES Test_Order(Order_ID)ON DELETE CASCADE,
    FOREIGN KEY (Test_ID) REFERENCES Tests(Test_ID)ON DELETE CASCADE
);

CREATE TABLE Contact (
    contact_id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(max),
    email NVARCHAR(max),
    sub NVARCHAR(max),
    message NVARCHAR(max),
    sent_date DATE,
    admin_response NVARCHAR(max),
    response_date DATE,
    status INT
);

-- Create the Feedback table
CREATE TABLE Feedback (
    Feedback_ID INT PRIMARY KEY IDENTITY(1,1), -- Unique identifier for feedback
    Patient_ID INT, -- Foreign key referencing Patients table
    Message NVARCHAR(MAX), -- Feedback message from the patient
    Status NVARCHAR(50) DEFAULT 'Pending', -- Status of the feedback (e.g., Pending, Approved, Rejected)
    FOREIGN KEY (Patient_ID) REFERENCES Patients(Patient_ID) -- Establishing the foreign key relationship
);

CREATE TABLE ChatRooms (
    ChatRoom_ID INT PRIMARY KEY IDENTITY(1,1),
    LabTech_ID INT FOREIGN KEY REFERENCES Lab_Tech(Tech_ID),
    Patient_ID INT FOREIGN KEY REFERENCES Patients(Patient_ID),
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE ChatMessages (
    ChatMessage_ID INT PRIMARY KEY IDENTITY(1,1),
    ChatRoom_ID INT FOREIGN KEY REFERENCES ChatRooms(ChatRoom_ID), -- New column linking to ChatRoom
    SenderId INT, -- Can be Patient_ID or Tech_ID
    MessageText NVARCHAR(MAX),
    SentAt DATETIME DEFAULT GETDATE(),
    SenderType NVARCHAR(MAX) -- 'Patient' or 'LabTech'
);



INSERT INTO Patients (Full_Name, Date_Of_Birth, Gender, Marital_Status, Nationality, Phone_Number, Home_Address, Note)
VALUES ('John Doe', '1985-06-15', 'Male', 'Married', 'American', 1234567890, '123 Main St, New York', 'No allergies');

INSERT INTO Patients (Full_Name, Date_Of_Birth, Gender, Marital_Status, Nationality, Phone_Number, Home_Address, Note)
VALUES ('Jane Smith', '1990-11-30', 'Female', 'Single', 'British', 98765430, '456 Elm St, London', 'Allergic to penicillin');

INSERT INTO Tests (Test_Name, Alternative_Name, Components, Normal_Range, Unit, Description, Price, Inventory, Sample_Type, Expiration_Date)
VALUES ('Complete Blood Count', 'CBC', 'WBC,RBC,Platelets,Hemoglobin,Hematocrit,MCV,MCH,MCHC', '4.5-11.0,4.7-6.1,150-400,13.8-17.2,40.7-50.3,80-100,27-31,32-36', 'x10^9/L,x10^12/L,x10^9/L,g/dL,%,fL,pg,g/dL', 'A test used to evaluate overall health.', 50.00, 100, 'Blood', '2025-12-31');

INSERT INTO Tests (Test_Name, Alternative_Name, Components, Normal_Range, Unit, Description, Price, Inventory, Sample_Type, Expiration_Date)
VALUES ('Liver Function Test', 'LFT', 'ALT,AST,ALP,Bilirubin', '10-40,8-38,45-115,0.1-1.2', 'U/L,U/L,U/L,mg/dL', 'Tests to assess liver function.', 80.00, 80, 'Blood', '2026-06-30');

INSERT INTO Appointments (ID, Full_Name, Gender, Date_Of_Birth, Email_Address, Phone_Number, Home_Address, Date_Of_Appo, Total_price, Amount_paid, Billing_ID, Status)
VALUES (1, 'John Doe', 'Male', '1985-06-15', 'john.doe@example.com', 1234567890, '123 Main St, New York', '2024-01-15', 130.00, 130.00, 1001, 'Completed');

INSERT INTO Appointments (ID, Full_Name, Gender, Date_Of_Birth, Email_Address, Phone_Number, Home_Address, Date_Of_Appo, Total_price, Amount_paid, Billing_ID, Status)
VALUES (2, 'Jane Smith', 'Female', '1990-11-30', 'jane.smith@example.com', 9876543210, '456 Elm St, London', '2024-01-20', 200.00, 100.00, 1002, 'Pending');

INSERT INTO Appointments_Tests (Appointment_ID, Test_ID)
VALUES (1, 1);  -- John Doe's appointment with CBC test

INSERT INTO Appointments_Tests (Appointment_ID, Test_ID)
VALUES (2, 2);  -- Jane Smith's appointment with Liver Function Test (LFT)

INSERT INTO Lab_Tech (Name, Email, Password, Status)
VALUES ('Dr. Alice Brown', 'alice.brown@lab.com', 'password123', 'Active');

INSERT INTO Lab_Tech (Name, Email, Password, Status)
VALUES ('Dr. Bob Green', 'bob.green@lab.com', 'securepass456', 'Active');

INSERT INTO Test_Order (Patient_ID, Date, Tech_ID, Total_Price, Discount_Persent, Amount_Paid, Status)
VALUES (1, '2024-01-15', 1, 130.00, 10, 117.00, 'Completed');  -- 10% discount applied

INSERT INTO Test_Order (Patient_ID, Date, Tech_ID, Total_Price, Discount_Persent, Amount_Paid, Status)
VALUES (2, '2024-01-20', 2, 200.00, 0, 100.00, 'Pending');

INSERT INTO Test_Order_Tests (Order_ID, Test_ID, Result, Date_Of_Result, Comment, Status)
VALUES (1, 1, 'Normal', '2024-01-16', 'All values within normal range.', 'Completed');

INSERT INTO Test_Order_Tests (Order_ID, Test_ID, Result, Date_Of_Result, Comment, Status)
VALUES (2, 2, 'Elevated AST and ALT', '2024-01-21', 'Requires further investigation.', 'Pending');

-- Insert first package
INSERT INTO Packages (Package_Name, Description, Price, Picture)
VALUES ('Basic Health Checkup', 'This package includes basic health screening tests.', 99.99, '/images/basic_health_checkup.png');

-- Insert second package
INSERT INTO Packages (Package_Name, Description, Price, Picture)
VALUES ('Comprehensive Health Screening', 'This package offers a complete set of tests for in-depth health evaluation.', 199.99, '/images/comprehensive_health_screening.png');


-- Insert 1st record into Contact table
INSERT INTO Contact (name, email, sub, message, sent_date, admin_response, response_date, status)
VALUES ('John Doe', 'john.doe@example.com', 'Product Inquiry', 'I would like to know more about your product.', '2024-09-01', NULL, NULL, 0);

-- Insert 2nd record into Contact table
INSERT INTO Contact (name, email, sub, message, sent_date, admin_response, response_date, status)
VALUES ('Jane Smith', 'jane.smith@example.com', 'Support Request', 'I am facing an issue with my account.', '2024-09-05', 'We are looking into it.', '2024-09-06', 1);

-- Insert 1st record into Feedback table
INSERT INTO Feedback (Patient_ID, Message, Status)
VALUES (1, 'Great service! Really satisfied with the care provided.', 'Approved');

-- Insert 2nd record into Feedback table
INSERT INTO Feedback (Patient_ID, Message, Status)
VALUES (2, 'The appointment scheduling process needs improvement.', 'Pending');
