CREATE DATABASE Diversity
GO
USE Diversity
GO

-- Tạo bảng Vai Trò
CREATE TABLE Roles (
    RoleID INT IDENTITY PRIMARY KEY,
    RoleName VARCHAR(50) NOT NULL
);

-- Tạo bảng Người Dùng
CREATE TABLE Users (
    UserID INT IDENTITY PRIMARY KEY,
    Username VARCHAR(50) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    RoleID INT,
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
);

-- Tạo bảng Khách Hàng
CREATE TABLE Customers (
    CustomerID INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Address NVARCHAR(255),
    PhoneNumber NVARCHAR(20),
    Email NVARCHAR(255)
);

-- Tạo bảng Nhà Cung Cấp
CREATE TABLE Suppliers (
    SupplierID INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Address NVARCHAR(255),
    PhoneNumber NVARCHAR(20),
    Email NVARCHAR(255)
);

-- Tạo bảng Sản Phẩm
CREATE TABLE Products (
    ProductID INT IDENTITY PRIMARY KEY,
    ProductName NVARCHAR(255) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    ProductImage NVARCHAR(MAX),
    SupplierID INT,
    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID)
);

INSERT INTO Products (ProductName, Price)
VALUES 
('iPhone 14', 999.99),
('Samsung Galaxy S23', 899.99),
('MacBook Pro 16', 2399.99),
('Dell XPS 13', 1299.99),
('Men''s T-Shirt', 19.99),
('Women''s Jeans', 49.99);



-- Tạo bảng Đơn Hàng
CREATE TABLE Orders (
    OrderID INT IDENTITY PRIMARY KEY,
    CustomerID INT,
    OrderDate DATE NOT NULL,
    TotalAmount DECIMAL(10, 2) NOT NULL,
	ProductImage NVARCHAR(MAX), 
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

-- Tạo bảng Chi Tiết Đơn Hàng
CREATE TABLE OrderDetails (
    OrderDetailID INT IDENTITY PRIMARY KEY,
    OrderID INT,
    ProductID INT,
    Quantity INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    ProductImage NVARCHAR(MAX), 
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

-- Tạo bảng Nhân Viên
CREATE TABLE Employees (
    EmployeeID INT IDENTITY PRIMARY KEY,
    UserID INT,
    Name NVARCHAR(255) NOT NULL,
    Position NVARCHAR(255),
    PhoneNumber NVARCHAR(20),
    Email NVARCHAR(255),
    Photo NVARCHAR(255),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE ProductReviews (
    ReviewID INT IDENTITY PRIMARY KEY,
    ProductID INT,
    CustomerID INT,
    Rating INT,
    Comment NVARCHAR(MAX),
    ReviewDate DATE,
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

-- Tạo bảng Lịch Sử Đơn Hàng
CREATE TABLE OrderHistory (
    OrderHistoryID INT IDENTITY PRIMARY KEY,
    OrderID INT,
    Status NVARCHAR(50),
    StatusDate DATETIME,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID)
);

-- Tạo bảng Mục Đơn Hàng
CREATE TABLE OrderItems (
    OrderItemID INT IDENTITY PRIMARY KEY,
    ProductName NVARCHAR(255) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    Quantity INT NOT NULL,
    ProductImage NVARCHAR(MAX)
);

CREATE TABLE DiscountCodes (
    DiscountCodeID INT IDENTITY PRIMARY KEY,
    Code NVARCHAR(50) NOT NULL,
    DiscountAmount DECIMAL(10, 2) NOT NULL,
    ValidFrom DATE NOT NULL,
    ValidUntil DATE NOT NULL
);

INSERT INTO DiscountCodes (Code, DiscountAmount, ValidFrom, ValidUntil)
VALUES
('SUMMER2024', 20.00, '2024-06-01', '2024-08-31'),
('WELCOME10', 10.00, '2024-01-01', '2024-12-31');


