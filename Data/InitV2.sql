
USE master;
GO

-- Xóa database cũ nếu đã tồn tại để tránh lỗi khi chạy lại script
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'AcademicDB')
BEGIN
    ALTER DATABASE AcademicDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE AcademicDB;
END
GO

-- Tạo mới Database với cấu hình File dung lượng
CREATE DATABASE AcademicDB
ON PRIMARY 
(
    NAME = Academic_Data,
    FILENAME = 'D:\DO AN HOC PHAN LOP\DOT NET(HK2-2026)\DataLOGPRIMARY\Academic_Data.mdf', 
    SIZE = 50MB,          -- Kích thước khởi tạo ban đầu cho dữ liệu
    MAXSIZE = 500MB,      -- Giới hạn tối đa để bảo vệ ổ đĩa
    FILEGROWTH = 10MB     -- Tự động tăng thêm mỗi lần 10MB khi bị đầy
)
LOG ON
(
    NAME = Academic_Log,
    FILENAME = 'D:\DO AN HOC PHAN LOP\DOT NET(HK2-2026)\DataLOGPRIMARY\Academic_Log.ldf',
    SIZE = 20MB,          -- Kích thước khởi tạo ban đầu cho file Log
    MAXSIZE = 100MB,      -- Giới hạn tối đa cho file Log
    FILEGROWTH = 5MB      -- Tự động tăng thêm mỗi lần 5MB
);
GO

USE AcademicDB;
GO
-----============================CREATE TABLE================================--------------
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(256) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Role NVARCHAR(20) CHECK (Role IN ('Admin', 'Lecturer', 'Student')) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    IsActive BIT DEFAULT 1
);

-- 2. Students: Chi tiết thông tin học viên (Liên kết 1-1 với Users)
CREATE TABLE Students (
    StudentID INT PRIMARY KEY FOREIGN KEY REFERENCES Users(UserID),
    Cohort NVARCHAR(20) NOT NULL,
    Major NVARCHAR(100) NOT NULL
);

-- 3. Semesters: Quản lý học kỳ
CREATE TABLE Semesters (
    SemesterID INT IDENTITY(1,1) PRIMARY KEY,
    SemesterName NVARCHAR(50) NOT NULL, -- VD: HK1 - 2025-2026
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    IsActive BIT DEFAULT 0
);

-- 4. Courses: Danh mục môn học
CREATE TABLE Courses (
    CourseID INT IDENTITY(1,1) PRIMARY KEY,
    CourseCode NVARCHAR(20) UNIQUE NOT NULL,
    CourseName NVARCHAR(150) NOT NULL,
    Credits INT NOT NULL CHECK (Credits > 0)
);

-- 5. Offerings: Các lớp học phần được mở (Hỗ trợ OfferingBrowserView)
CREATE TABLE Offerings (
    OfferingID INT IDENTITY(1,1) PRIMARY KEY,
    CourseID INT FOREIGN KEY REFERENCES Courses(CourseID),
    SemesterID INT FOREIGN KEY REFERENCES Semesters(SemesterID),
    LecturerID INT FOREIGN KEY REFERENCES Users(UserID),
    ClassCode NVARCHAR(50) NOT NULL,
    MaxCapacity INT NOT NULL,
    Deadline DATE NOT NULL,
    Status NVARCHAR(20) CHECK (Status IN ('Open', 'Closed')) DEFAULT 'Open'
);

-- 6. Enrollments: Đăng ký học phần (Hỗ trợ EnrollmentManagerView)
CREATE TABLE Enrollments (
    EnrollmentID INT IDENTITY(1,1) PRIMARY KEY,
    OfferingID INT FOREIGN KEY REFERENCES Offerings(OfferingID),
    StudentID INT FOREIGN KEY REFERENCES Students(StudentID),
    EnrollDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20) CHECK (Status IN ('Enrolled', 'Dropped', 'Pending')) DEFAULT 'Enrolled',
    CONSTRAINT UQ_Student_Offering UNIQUE (StudentID, OfferingID)
);

-- 7. GradeComponents: Thành phần điểm của môn học (Hỗ trợ GradeEntryView)
CREATE TABLE GradeComponents (
    ComponentID INT IDENTITY(1,1) PRIMARY KEY,
    CourseID INT FOREIGN KEY REFERENCES Courses(CourseID),
    ComponentName NVARCHAR(50) NOT NULL, -- Giữa kỳ, Thực hành, Cuối kỳ
    Weight DECIMAL(5,2) NOT NULL CHECK (Weight > 0 AND Weight <= 100)
);

-- 8. Grades: Lưu chi tiết điểm
CREATE TABLE Grades (
    GradeID INT IDENTITY(1,1) PRIMARY KEY,
    EnrollmentID INT FOREIGN KEY REFERENCES Enrollments(EnrollmentID),
    ComponentID INT FOREIGN KEY REFERENCES GradeComponents(ComponentID),
    Score DECIMAL(4,2) CHECK (Score >= 0 AND Score <= 10),
    GradeStatus NVARCHAR(20) CHECK (GradeStatus IN ('Draft', 'Submitted', 'Approved', 'Published')) DEFAULT 'Draft',
    RecheckNote NVARCHAR(MAX) NULL, -- Ghi chú phúc khảo (Hỗ trợ RecheckRequestView)
    LastModified DATETIME DEFAULT GETDATE(),
    CONSTRAINT UQ_Enrollment_Component UNIQUE (EnrollmentID, ComponentID)
);

-- 9. TranscriptSnapshots: Chụp lịch sử GPA
CREATE TABLE TranscriptSnapshots (
    SnapshotID INT IDENTITY(1,1) PRIMARY KEY,
    StudentID INT FOREIGN KEY REFERENCES Students(StudentID),
    SemesterID INT FOREIGN KEY REFERENCES Semesters(SemesterID),
    TotalCredits INT NOT NULL,
    TermGPA DECIMAL(4,2) NOT NULL,
    CumulativeGPA DECIMAL(4,2) NOT NULL,
    SnapshotDate DATETIME DEFAULT GETDATE()
);

--=========================Insert Value============================================--

-- Thêm Semesters
INSERT INTO Semesters (SemesterName, StartDate, EndDate, IsActive) VALUES 
('HK1 - 2024-2025', '2024-09-01', '2025-01-15', 0),
('HK1 - 2025-2026', '2025-09-01', '2026-01-15', 1);

-- Thêm Users (2 Giảng viên, 3 Học viên)
INSERT INTO Users (Username, PasswordHash, FullName, Role, Email) VALUES 
('gv_nguyenvan', 'hash1', N'Nguyễn Văn A', 'Lecturer', 'nva@edu.vn'),
('gv_lethi', 'hash2', N'Lê Thị B', 'Lecturer', 'ltb@edu.vn'),
('hv_tran01', 'hash3', N'Trần Học C', 'Student', 'thc@student.edu.vn'),
('hv_pham02', 'hash4', N'Phạm Văn D', 'Student', 'pvd@student.edu.vn'),
('hv_hoang03', 'hash5', N'Hoàng Thị E', 'Student', 'hte@student.edu.vn');

-- Thêm Students
INSERT INTO Students (StudentID, Cohort, Major) VALUES 
(3, 'K24', N'Công nghệ thông tin'),
(4, 'K24', N'Khoa học máy tính'),
(5, 'K25', N'Hệ thống thông tin');

-- Thêm Courses
INSERT INTO Courses (CourseCode, CourseName, Credits) VALUES 
('IT601', N'Học máy nâng cao', 3),
('IT602', N'Khai phá dữ liệu', 3),
('IT603', N'Cơ sở dữ liệu phân tán', 4),
('IT604', N'Mạng nơ-ron nhân tạo', 3),
('IT605', N'Thị giác máy tính', 4);

-- Thêm GradeComponents (Chuẩn cấu trúc 30-20-50 như trong GradeEntryView)
INSERT INTO GradeComponents (CourseID, ComponentName, Weight) VALUES 
(1, N'Giữa kỳ', 30), (1, N'Thực hành', 20), (1, N'Cuối kỳ', 50),
(2, N'Giữa kỳ', 30), (2, N'Thực hành', 20), (2, N'Cuối kỳ', 50),
(3, N'Giữa kỳ', 40), (3, N'Cuối kỳ', 60),
(4, N'Giữa kỳ', 30), (4, N'Thực hành', 20), (4, N'Cuối kỳ', 50),
(5, N'Giữa kỳ', 30), (5, N'Thực hành', 20), (5, N'Cuối kỳ', 50);

-- Thêm Offerings
INSERT INTO Offerings (CourseID, SemesterID, LecturerID, ClassCode, MaxCapacity, Deadline, Status) VALUES 
(1, 2, 1, 'IT601-01', 30, '2025-09-15', 'Open'),
(2, 2, 2, 'IT602-01', 40, '2025-09-15', 'Open'),
(3, 1, 1, 'IT603-01', 35, '2024-09-15', 'Closed');

-- Thêm Enrollments (Đăng ký học)
INSERT INTO Enrollments (OfferingID, StudentID, Status) VALUES 
(1, 3, 'Enrolled'), (1, 4, 'Enrolled'), (1, 5, 'Enrolled'), -- Lớp 1 có 3 SV
(2, 3, 'Enrolled'), (2, 4, 'Enrolled');                      -- Lớp 2 có 2 SV

-- Thêm Grades (Điểm) với các trạng thái khác nhau
-- Lớp IT601 (Offering 1) - Trạng thái Approved
INSERT INTO Grades (EnrollmentID, ComponentID, Score, GradeStatus) VALUES 
(1, 1, 8.5, 'Approved'), (1, 2, 9.0, 'Approved'), (1, 3, 7.5, 'Approved'), -- Học viên 3
(2, 1, 4.0, 'Approved'), (2, 2, 5.0, 'Approved'), (2, 3, 4.5, 'Approved'), -- Học viên 4 (Fail)
(3, 1, 9.5, 'Approved'), (3, 2, 8.5, 'Approved'), (3, 3, 9.0, 'Approved'); -- Học viên 5

-- Lớp IT602 (Offering 2) - Trạng thái Draft
INSERT INTO Grades (EnrollmentID, ComponentID, Score, GradeStatus) VALUES 
(4, 4, 7.0, 'Draft'), (4, 5, 8.0, 'Draft'), -- HV 3 chưa có điểm cuối kỳ
(5, 4, 3.5, 'Draft'), (5, 5, 4.0, 'Draft'); -- HV 4


--=============================Trigger===============================--
-- Rule 1: Học viên không được đăng ký nếu Offerings.Status = 'Closed'
CREATE TRIGGER trg_CheckOfferingStatus
ON Enrollments
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT 1 
        FROM inserted i
        JOIN Offerings o ON i.OfferingID = o.OfferingID
        WHERE o.Status = 'Closed' AND i.Status = 'Enrolled'
    )
    BEGIN
        RAISERROR ('Không thể đăng ký. Lớp học phần này đã đóng (Closed).', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;
GO

-- Rule 2 & 3: Quy trình duyệt điểm và chặn sửa điểm sau khi Published (trừ khi có RecheckNote)
CREATE TRIGGER trg_EnforceGradeWorkflow
ON Grades
AFTER UPDATE
AS
BEGIN
    -- Ngăn chặn sửa điểm trực tiếp khi đã Published mà không có RecheckNote
    IF EXISTS (
        SELECT 1
        FROM deleted d
        JOIN inserted i ON d.GradeID = i.GradeID
        WHERE d.GradeStatus = 'Published' 
          AND d.Score <> i.Score 
          AND (i.RecheckNote IS NULL OR d.RecheckNote = i.RecheckNote)
    )
    BEGIN
        RAISERROR ('Điểm đã được Công bố (Published). Chỉ được phép sửa nếu cập nhật kèm Ghi chú phúc khảo (RecheckNote).', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Ép buộc luồng: Draft -> Submitted -> Approved -> Published
    IF EXISTS (
        SELECT 1
        FROM deleted d
        JOIN inserted i ON d.GradeID = i.GradeID
        WHERE 
            (d.GradeStatus = 'Draft' AND i.GradeStatus NOT IN ('Draft', 'Submitted')) OR
            (d.GradeStatus = 'Submitted' AND i.GradeStatus NOT IN ('Submitted', 'Approved', 'Draft')) OR
            (d.GradeStatus = 'Approved' AND i.GradeStatus NOT IN ('Approved', 'Published', 'Submitted'))
    )
    BEGIN
        RAISERROR ('Sai quy trình duyệt điểm. Luồng hợp lệ: Draft -> Submitted -> Approved -> Published.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;
--==========================================View===================================--
CREATE OR ALTER VIEW view_students_register_for_course
AS
    SELECT e.EnrollmentID, e.OfferingID, st.*, o.ClassCode, o.*, c.*, e.Status
    FROM Enrollments e
    JOIN Offerings o ON e.OfferingID=o.OfferingID
    JOIN Students st ON st.StudentID=e.StudentID
    JOIN Courses c ON c.CourseID = o.CourseID


CREATE OR ALTER VIEW view_StudentCourseRegistration
AS
SELECT 
    e.EnrollmentID,
    u.FullName AS StudentName,        -- Lấy họ tên từ bảng Users
    s.Cohort,                          -- Khóa học (VD: K24)
    s.Major,                           -- Ngành học
    c.CourseCode,                      -- Mã môn học (VD: IT601)
    c.CourseName,                      -- Tên môn học / Học phần
    c.Credits,                         -- Số tín chỉ của môn
    o.ClassCode,                       -- Mã lớp học phần (VD: IT601-01)
    e.EnrollDate,                      -- Ngày đăng ký
    e.Status AS RegistrationStatus     -- Trạng thái đăng ký (Enrolled, Dropped...)
FROM Enrollments e
JOIN Students s ON e.StudentID = s.StudentID
JOIN Users u ON s.StudentID = u.UserID         
JOIN Offerings o ON e.OfferingID = o.OfferingID
JOIN Courses c ON o.CourseID = c.CourseID;

GO