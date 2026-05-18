-- ===========================================================================
-- 1. KHỞI TẠO DATABASE VỚI ĐỊNH LƯỢNG DUNG LƯỢNG (MB)
-- ===========================================================================
USE master;
GO

-- Xóa database cũ nếu đã tồn tại để tránh lỗi khi chạy lại script
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'AcademicManagementDB')
BEGIN
    ALTER DATABASE AcademicManagementDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE AcademicManagementDB;
END
GO

-- Tạo mới Database với cấu hình File dung lượng
CREATE DATABASE AcademicManagementDB
ON PRIMARY 
(
    NAME = AcademicManagement_Data,
    FILENAME = 'C:\Study\.Net\Doan\DB_QLHT\AcademicManagement_Data.mdf', 
    SIZE = 50MB,          -- Kích thước khởi tạo ban đầu cho dữ liệu
    MAXSIZE = 500MB,      -- Giới hạn tối đa để bảo vệ ổ đĩa
    FILEGROWTH = 10MB     -- Tự động tăng thêm mỗi lần 10MB khi bị đầy
)
LOG ON
(
    NAME = AcademicManagement_Log,
    FILENAME = 'C:\Study\.Net\Doan\DB_QLHT\AcademicManagement_Log.ldf',
    SIZE = 20MB,          -- Kích thước khởi tạo ban đầu cho file Log
    MAXSIZE = 100MB,      -- Giới hạn tối đa cho file Log
    FILEGROWTH = 5MB      -- Tự động tăng thêm mỗi lần 5MB
);
GO

USE AcademicManagementDB;
GO

-- ===========================================================================
-- 2. TẠO CẤU TRÚC CÁC BẢNG (TABLES)
-- ===========================================================================

-- Bảng Sinh viên
CREATE TABLE Students (
    StudentId INT IDENTITY(1,1) PRIMARY KEY,
    StudentCode VARCHAR(20) UNIQUE NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    DateOfBirth DATE NULL,
    Email VARCHAR(100) NULL
);

-- Bảng Học kỳ
CREATE TABLE Semesters (
    SemesterId INT IDENTITY(1,1) PRIMARY KEY,
    SemesterName NVARCHAR(50) NOT NULL, -- Ví dụ: Học kỳ 1 2025-2026
    StartDate DATE NULL,
    EndDate DATE NULL
);

-- Bảng Môn học
CREATE TABLE Courses (
    CourseId INT IDENTITY(1,1) PRIMARY KEY,
    CourseCode VARCHAR(20) UNIQUE NOT NULL,
    CourseName NVARCHAR(100) NOT NULL,
    Credits INT NOT NULL DEFAULT 3
);

-- Bảng Lớp học phần
CREATE TABLE Offerings (
    OfferingId INT IDENTITY(1,1) PRIMARY KEY,
    CourseId INT NOT NULL,
    SemesterId INT NOT NULL,
    ClassCode VARCHAR(20) NOT NULL, 
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId),
    FOREIGN KEY (SemesterId) REFERENCES Semesters(SemesterId)
);

-- Bảng Đăng ký học phần (Yêu cầu gốc)
CREATE TABLE Enrollments (
    EnrollmentId INT IDENTITY(1,1) PRIMARY KEY,
    StudentId INT NOT NULL,
    OfferingId INT NOT NULL,
    EnrollStatus NVARCHAR(50) DEFAULT N'Active', 
    EnrolledAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    FOREIGN KEY (OfferingId) REFERENCES Offerings(OfferingId),
    CONSTRAINT UC_Student_Offering UNIQUE (StudentId, OfferingId)
);

-- Bảng Thành phần điểm (Yêu cầu gốc)
CREATE TABLE GradeComponents (
    ComponentId INT IDENTITY(1,1) PRIMARY KEY,
    CourseId INT NOT NULL,
    ComponentName NVARCHAR(100) NOT NULL, 
    Weight DECIMAL(5,2) NOT NULL, -- Ví dụ: 0.30 (30%), 0.70 (70%)
    MaxScore DECIMAL(5,2) DEFAULT 10.00, 
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId)
);

-- Bảng Điểm số chi tiết (Yêu cầu gốc)
CREATE TABLE Grades (
    GradeId INT IDENTITY(1,1) PRIMARY KEY,
    EnrollmentId INT NOT NULL,
    ComponentId INT NOT NULL,
    Score DECIMAL(5,2) NULL, 
    GradeStatus NVARCHAR(50) DEFAULT N'Normal', 
    FOREIGN KEY (EnrollmentId) REFERENCES Enrollments(EnrollmentId) ON DELETE CASCADE,
    FOREIGN KEY (ComponentId) REFERENCES GradeComponents(ComponentId),
    CONSTRAINT UC_Enrollment_Component UNIQUE (EnrollmentId, ComponentId)
);

-- Bảng Chốt bảng điểm theo học kỳ (Yêu cầu gốc)
CREATE TABLE TranscriptSnapshots (
    SnapshotId INT IDENTITY(1,1) PRIMARY KEY,
    StudentId INT NOT NULL,
    SemesterId INT NOT NULL,
    GPA DECIMAL(3,2) NOT NULL, 
    TotalCredits INT NOT NULL, 
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    FOREIGN KEY (SemesterId) REFERENCES Semesters(SemesterId),
    CONSTRAINT UC_Student_Semester UNIQUE (StudentId, SemesterId)
);
GO

-- ===========================================================================
-- 2.1 RULE SYSTEM
-- ===========================================================================

-- 1. Bổ sung trường Status vào bảng Offerings (Lớp học phần) để kiểm tra hạn đăng ký
ALTER TABLE Offerings 
ADD Status NVARCHAR(50) DEFAULT N'Open'; 
-- Trạng thái: 'Open' (Cho đăng ký), 'Closed' (Đóng/Hết hạn đăng ký)

-- 2. Bổ sung các trường phục vụ Phúc khảo điểm vào bảng Grades
ALTER TABLE Grades 
ADD RecheckNote NVARCHAR(500) NULL,
    IsRechecked BIT DEFAULT 0;

-- 3. Cập nhật lại dữ liệu mẫu cho trạng thái điểm của Nhóm 06 (Draft -> Submitted -> Approved -> Published)
-- Thay vì mặc định 'Normal', ta chuẩn hóa theo workflow bắt buộc của đề tài
DECLARE @ConstraintName NVARCHAR(200);

-- Sửa 'COLUMN_PROPERTY' thành 'COLUMNPROPERTY' viết liền
SELECT @ConstraintName = name
FROM sys.default_constraints
WHERE parent_object_id = OBJECT_ID('Grades') 
  AND parent_column_id = COLUMNPROPERTY(OBJECT_ID('Grades'), 'GradeStatus', 'ColumnId');

-- Nếu tìm thấy thì tiến hành xóa
IF @ConstraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE Grades DROP CONSTRAINT ' + @ConstraintName);
END
GO

ALTER TABLE Grades
ADD CONSTRAINT DF_Grades_Status DEFAULT N'Draft' FOR GradeStatus;
GO

-- ===========================================================================
-- 3. CHÈN DỮ LIỆU MẪU (SEED DATA) ĐỂ TEST HỆ THỐNG
-- ===========================================================================
SET DATEFORMAT dmy;
GO

-- Thêm học sinh (Đã đổi sang định dạng ngày/tháng/năm)
INSERT INTO Students (StudentCode, FullName, DateOfBirth, Email) VALUES
('SV001', N'Nguyễn Văn A', '15/05/2005', 'vana@gmail.com'),
('SV002', N'Trần Thị B', '20/09/2005', 'thib@gmail.com');

-- Thêm học kỳ (Đã đổi sang định dạng ngày/tháng/năm)
INSERT INTO Semesters (SemesterName, StartDate, EndDate) VALUES
(N'Học kỳ 1 2025-2026', '05/09/2025', '15/01/2026'),
(N'Học kỳ 2 2025-2026', '01/02/2026', '15/06/2026');

-- Thêm môn học
INSERT INTO Courses (CourseCode, CourseName, Credits) VALUES
('PRN211', N'Lập trình C# .NET', 3),
('DBI202', N'Hệ quản trị Cơ sở dữ liệu', 3);

-- Thêm lớp học phần (Môn học mở trong kỳ)
INSERT INTO Offerings (CourseId, SemesterId, ClassCode) VALUES
(1, 1, 'PRN211_L01'), -- C# mở vào Kỳ 1
(2, 1, 'DBI202_L01'); -- DB mở vào Kỳ 1

-- Sinh viên Đăng ký học (Enrollments)
INSERT INTO Enrollments (StudentId, OfferingId, EnrollStatus) VALUES
(1, 1, N'Active'), -- Nguyễn Văn A học lớp C#
(1, 2, N'Active'), -- Nguyễn Văn A học lớp DB
(2, 1, N'Active'); -- Trần Thị B học lớp C#

-- Cài đặt cấu trúc đầu điểm cho các môn học (GradeComponents)
-- Môn C# (Id = 1): Giữa kỳ 30%, Cuối kỳ 70%
INSERT INTO GradeComponents (CourseId, ComponentName, Weight) VALUES
(1, N'Điểm Giữa Kỳ', 0.30),
(1, N'Điểm Cuối Kỳ', 0.70);

-- Môn DB (Id = 2): Chuyên cần 10%, Thi 90%
INSERT INTO GradeComponents (CourseId, ComponentName, Weight) VALUES
(2, N'Điểm Chuyên Cần', 0.10),
(2, N'Điểm Thi Cuối Kỳ', 0.90);

-- Nhập điểm cho sinh viên (Grades)
-- Nguyễn Văn A (EnrollmentId = 1) môn C#: Giữa kỳ được 8, Cuối kỳ được 9
INSERT INTO Grades (EnrollmentId, ComponentId, Score) VALUES
(1, 1, 8.0), 
(1, 2, 9.0);

-- Trần Thị B (EnrollmentId = 3) môn C#: Giữa kỳ được 5, Cuối kỳ được 6
INSERT INTO Grades (EnrollmentId, ComponentId, Score) VALUES
(3, 1, 5.0),
(3, 2, 6.0);

-- Lưu chốt GPA kỳ 1 của Nguyễn Văn A (TranscriptSnapshots)
INSERT INTO TranscriptSnapshots (StudentId, SemesterId, GPA, TotalCredits) VALUES
(1, 1, 8.70, 6);
GO


-- ===========================================================================
-- 4. TRUY VẤN KIỂM TRA KẾT QUẢ ĐÃ LIÊN KẾT
-- ===========================================================================
SET DATEFORMAT dmy;
GO

SELECT 
    s.StudentCode AS [Mã SV],
    s.FullName AS [Tên Sinh Viên],
    CONVERT(VARCHAR, s.DateOfBirth, 103) AS [Ngày Sinh], -- Hiển thị dạng dd/mm/yyyy
    c.CourseName AS [Tên Môn Học],
    gc.ComponentName AS [Cột Điểm],
    g.Score AS [Điểm Số],
    gc.Weight AS [Trọng Số]
FROM Grades g
JOIN Enrollments e ON g.EnrollmentId = e.EnrollmentId
JOIN Students s ON e.StudentId = s.StudentId
JOIN Offerings o ON e.OfferingId = o.OfferingId
JOIN Courses c ON o.CourseId = c.CourseId
JOIN GradeComponents gc ON g.ComponentId = gc.ComponentId;


