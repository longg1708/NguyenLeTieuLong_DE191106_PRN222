-- Tạo database
CREATE DATABASE DBTravelCenter;
GO
USE DBTravelCenter;
GO

-- Bảng Trip
CREATE TABLE Trip (
    TripID INT IDENTITY(1,1) PRIMARY KEY,
    Code VARCHAR(30) NOT NULL UNIQUE,
    Destination NVARCHAR(200) NOT NULL,
    Price DECIMAL(12,2) NOT NULL CHECK (Price >= 0),
    Status VARCHAR(20) NOT NULL CHECK (Status IN ('Available','Booked'))
);

-- Bảng Customer (thêm Password)
CREATE TABLE Customer (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    Code VARCHAR(30) NOT NULL UNIQUE,
    FullName NVARCHAR(150) NOT NULL,
    Email VARCHAR(200) UNIQUE,
    Age INT CHECK (Age >= 0),
    Password NVARCHAR(100) NOT NULL,   -- thêm cột Password
    Role VARCHAR(20) NOT NULL DEFAULT 'User' CHECK (Role IN ('User', 'Admin'))  -- thêm cột Role
);

-- Bảng Booking
CREATE TABLE Booking (
    BookingID INT IDENTITY(1,1) PRIMARY KEY,
    TripID INT NOT NULL,
    CustomerID INT NOT NULL,
    BookingDate DATE NOT NULL DEFAULT (GETDATE()),
    Status VARCHAR(20) NOT NULL CHECK (Status IN ('Pending','Confirmed','Cancelled')),
    FOREIGN KEY (TripID) REFERENCES Trip(TripID),
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID)
);

-- Dữ liệu mẫu
INSERT INTO Trip (Code, Destination, Price, Status) VALUES
('TRP-PAR-01', N'Paris', 2150.00, 'Available'),
('TRP-TYO-02', N'Tokyo', 1890.00, 'Available'),
('TRP-NYC-03', N'New York', 1590.00, 'Booked'),
('TRP-PAR-04', N'Paris', 1990.00, 'Available');

INSERT INTO Customer (Code, FullName, Email, Age, Password, Role) VALUES
('CUS-001', N'Nguyen Van A', 'a.nguyen@example.com', 28, '123456', 'User'),
('CUS-002', N'Tran Thi B',   'b.tran@example.com',   24, 'password1', 'User'),
('CUS-003', N'Le Van C',     'c.le@example.com',     31, 'abc@123', 'User'),
('CUS-004', N'Tran Van D', 'd.tran@example.com', 30, '123', 'Admin');

INSERT INTO Booking (TripID, CustomerID, BookingDate, Status) VALUES
(1, 1, '2026-02-04', 'Pending'),
(2, 2, '2026-01-15', 'Confirmed'),
(3, 3, '2026-02-01', 'Cancelled'),
(1, 2, '2026-02-10', 'Pending'),  
(4, 1, '2026-01-20', 'Pending');
