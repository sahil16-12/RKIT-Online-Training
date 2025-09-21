CREATE DATABASE studentdb;
USE studentdb;

-- o	Students (student_id, name, age, gender, course_id)
CREATE TABLE Students(
	student_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    age INT CHECK(age>=5),
    gender ENUM('Male','Female','Other'),
    course_id INT,
    FOREIGN KEY(course_id) REFERENCES Courses (course_id)
);

-- o	Courses (course_id, course_name, duration)
CREATE TABLE Courses (
	course_id INT PRIMARY KEY AUTO_INCREMENT,
    course_name VARCHAR(100) NOT NULL,
    duration INT NOT NULL -- Duration in months
);
-- o	Marks (mark_id, student_id, subject, score)
CREATE TABLE Marks(
	mark_id INT PRIMARY KEY AUTO_INCREMENT,
    student_id INT,
    score INT CHECK (score>=0 AND score<=100),
    FOREIGN KEY(student_id) REFERENCES Students(student_id)
);

-- 2.	Modify Students table to add a new column email.
ALTER TABLE Students
ADD email VARCHAR(100) UNIQUE;

-- 1.	Insert at least 5 rows into each table.
INSERT INTO Courses (course_name, duration) VALUES
('Computer Science', 48),
('Mechanical Engineering', 48),
('Electrical Engineering', 48),
('Business Administration', 36),
('Psychology', 36);

INSERT INTO Students (name, age, gender, course_id, email) VALUES
('Alice Johnson', 20, 'Female', 1, 'alice@example.com'),
('Bob Smith', 22, 'Male', 2, 'bob@example.com'),
('Charlie Brown', 21, 'Male', 3, 'charlie@example.com'),
('Diana Prince', 23, 'Female', 4, 'diana@example.com'),
('Ethan Hunt', 24, 'Male', 5, 'ethan@example.com');


INSERT INTO Marks (student_id, score) VALUES
(6, 88),
(7, 75),
(8, 82),
(9, 91),
(10, 85);

-- 2.	Update one student’s course.
UPDATE students SET course_id = 2 WHERE student_id = 6;

-- 3.	Delete a student record.
SHOW CREATE TABLE Marks; -- To see the name of foreign key contraint

ALTER TABLE MARKS DROP FOREIGN KEY marks_ibfk_1;

ALTER TABLE MARKS 
ADD CONSTRAINT marks_ibfk_1
FOREIGN KEY(student_id) REFERENCES Students(student_id)
ON DELETE CASCADE;

DELETE FROM students WHERE student_id = 6;


-- 1.	Write a query to fetch all students above age 20.

SELECT * FROM students WHERE age > 20;

-- 2.	Fetch all students ordered by name alphabetically.
SELECT * FROM students ORDER BY name;

-- 3.	Show total number of students enrolled in each course using GROUP BY.
SELECT c.course_name, COUNT(s.student_id) AS total_students
FROM Courses c
LEFT JOIN Students s ON c.course_id = s.course_id
GROUP BY c.course_id, c.course_name;

-- 4.	Show courses that have more than 2 students using HAVING.
SELECT c.course_name, COUNT(s.student_id) AS total_students
FROM Courses c
JOIN Students s ON c.course_id = s.course_id
GROUP BY c.course_id, c.course_name
HAVING COUNT(s.student_id) > 2;


-- 1.	Display students with their enrolled course names using INNER JOIN.
SELECT s.student_id,s.name,c.course_name FROM students s JOIN courses c ON s.course_id = c.course_id;

-- 2.	Display all students even if they are not enrolled in any course (LEFT JOIN).
SELECT s.name,c.course_name FROM students S LEFT JOIN courses C ON S.course_id = C.course_id;

-- 3.	Display all courses and their students (RIGHT JOIN).
SELECT c.course_id, c.course_name, c.duration,
       s.student_id, s.name, s.age, s.gender, s.email
FROM Students s
RIGHT JOIN Courses c ON s.course_id = c.course_id;

-- 4.	Find highest, lowest, and average marks per subject.
SELECT 
       MAX(score) AS highest_mark,
       MIN(score) AS lowest_mark,
       AVG(score) AS average_mark
FROM Marks;

-- 5.	Count how many male and female students exist.
SELECT gender, COUNT(*) AS total_students
FROM students 
GROUP BY gender;


-- Test/Quiz for Week 1: Write SQL queries for a small LibraryDB with Books, Authors, and Borrowers tables (to check joins, aggregates, and filtering).

-- Authors table
CREATE TABLE Authors (
    author_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL
);

-- Books table
CREATE TABLE Books (
    book_id INT PRIMARY KEY AUTO_INCREMENT,
    title VARCHAR(150) NOT NULL,
    author_id INT,
    published_year INT,
    FOREIGN KEY (author_id) REFERENCES Authors(author_id)
);

-- Borrowers table
CREATE TABLE Borrowers (
    borrower_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    book_id INT,
    borrow_date DATE,
    return_date DATE,
    FOREIGN KEY (book_id) REFERENCES Books(book_id)
);

INSERT INTO Authors (name) VALUES
('J.K. Rowling'),
('George R.R. Martin'),
('Agatha Christie'),
('J.R.R. Tolkien'),
('Chetan Bhagat');

INSERT INTO Books (title, author_id, published_year) VALUES
('Harry Potter and the Sorcerer''s Stone', 1, 1997),
('A Game of Thrones', 2, 1996),
('Murder on the Orient Express', 3, 1934),
('The Hobbit', 4, 1937),
('2 States', 5, 2009);

INSERT INTO Borrowers (name, book_id, borrow_date, return_date) VALUES
('Alice', 1, '2025-01-05', '2025-01-20'),
('Bob', 2, '2025-01-10', NULL),   -- Not yet returned
('Charlie', 3, '2025-01-15', '2025-01-25'),
('David', 1, '2025-02-01', NULL), -- Not yet returned
('Emma', 4, '2025-02-05', '2025-02-15');



-- Q1. List all books with their authors (JOIN).
SELECT b.book_id, b.title, a.name AS author_name, b.published_year
FROM Books b
JOIN Authors a ON b.author_id = a.author_id;

-- Q2. Show all books and their borrowers, even if the book hasn’t been borrowed yet (LEFT JOIN).
SELECT b.title, br.name AS borrower_name, br.borrow_date
FROM Books b
LEFT JOIN Borrowers br ON b.book_id = br.book_id;

-- Q3. Find how many books each author has written (GROUP BY).
SELECT a.name AS author_name, COUNT(b.book_id) AS total_books
FROM Authors a
LEFT JOIN Books b ON a.author_id = b.author_id
GROUP BY a.author_id, a.name;


-- Q4. Show authors who have written more than 2 books (HAVING).
SELECT a.name AS author_name, COUNT(b.book_id) AS total_books
FROM Authors a
JOIN Books b ON a.author_id = b.author_id
GROUP BY a.author_id, a.name
HAVING COUNT(b.book_id) > 2;

-- Q5. Find the most recent book borrowed (MAX on borrow_date).
SELECT b.title, br.name AS borrower_name, br.borrow_date
FROM Borrowers br
JOIN Books b ON br.book_id = b.book_id
WHERE br.borrow_date = (SELECT MAX(borrow_date) FROM Borrowers);



-- ============================================
-- Week 2 Assignments (Implementations)
-- ============================================

-- Assignment 6: Subqueries
-- 1. Find students who scored above the average score using a subquery.
SELECT s.student_id, s.name, m.score
FROM Students s
JOIN Marks m ON s.student_id = m.student_id
WHERE m.score > (SELECT AVG(score) FROM Marks);

-- 2. Get students enrolled in the same course as "John" using a correlated subquery.
SELECT s2.*
FROM Students s2
WHERE s2.course_id = (
    SELECT s1.course_id
    FROM Students s1
    WHERE s1.name = 'John'
    LIMIT 1
);

-- Correlated-subquery style (same result but correlated)
SELECT s_outer.*
FROM Students s_outer
WHERE EXISTS (
    SELECT 1
    FROM Students s_inner
    WHERE s_inner.name = 'John'
      AND s_outer.course_id = s_inner.course_id
);


-- Assignment 7: UNION & Set Ops
-- 1. List all distinct course names from Courses and from Marks (via Students->Courses) using UNION.
SELECT course_name FROM Courses
UNION
SELECT c.course_name
FROM Marks m
JOIN Students s ON m.student_id = s.student_id
JOIN Courses c ON s.course_id = c.course_id;

-- 2. Include duplicates (use UNION ALL).
SELECT course_name FROM Courses
UNION ALL
SELECT c.course_name
FROM Marks m
JOIN Students s ON m.student_id = s.student_id
JOIN Courses c ON s.course_id = c.course_id;


-- Assignment 8: Constraints & Performance
-- 1. Add a PRIMARY KEY on student_id 
-- ALTER TABLE Students ADD PRIMARY KEY (student_id);

-- 2. Add an AUTO_INCREMENT to course_id
-- ALTER TABLE courses
-- MODIFY course_id INT AUTO_INCREMENT;

-- 3. Create an INDEX on email for faster search.
CREATE INDEX idx_students_email ON Students(email);

-- 4. Prove query optimization difference using EXPLAIN with and without index.
-- Explain with index:
EXPLAIN FORMAT=JSON SELECT * FROM Students WHERE email = 'bob@example.com';

-- Assignment 9: Stored Programs
-- 1. Stored Procedure to return all students enrolled in a given course.

DELIMITER $$
CREATE PROCEDURE GetStudentsByCourse(IN in_course_id INT)
BEGIN
    SELECT * FROM Students WHERE course_id = in_course_id;
END $$
DELIMITER ;

-- Example call:
-- CALL GetStudentsByCourse(1);

-- 2. Function to calculate grade based on marks (A/B/C...).

DELIMITER $$
CREATE FUNCTION GetGrade(in_mark INT) RETURNS VARCHAR(2)
DETERMINISTIC
BEGIN
    DECLARE g VARCHAR(2);
    IF in_mark >= 90 THEN
        SET g = 'A';
    ELSEIF in_mark >= 75 THEN
        SET g = 'B';
    ELSEIF in_mark >= 60 THEN
        SET g = 'C';
    ELSEIF in_mark >= 50 THEN
        SET g = 'D';
    ELSE
        SET g = 'F';
    END IF;
    RETURN g;
END $$
DELIMITER ;

-- Example usage:
-- SELECT student_id, score, GetGrade(score) AS grade FROM Marks;


-- 3. Trigger to log deleted student records into a new table DeletedStudents.
CREATE TABLE IF NOT EXISTS DeletedStudents (
    del_id INT PRIMARY KEY AUTO_INCREMENT,
    student_id INT,
    name VARCHAR(100),
    age INT,
    gender ENUM('Male','Female','Other'),
    course_id INT,
    email VARCHAR(100),
    deleted_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);


DELIMITER $$
CREATE TRIGGER trg_log_deleted_student
AFTER DELETE ON Students
FOR EACH ROW
BEGIN
    INSERT INTO DeletedStudents(student_id, name, age, gender, course_id, email)
    VALUES (OLD.student_id, OLD.name, OLD.age, OLD.gender, OLD.course_id, OLD.email);
END $$
DELIMITER ;


-- Assignment 10: Views + Backup
-- 1. Create a View StudentCourseView that shows student name + course name.
CREATE OR REPLACE VIEW StudentCourseView AS
SELECT s.student_id, s.name AS student_name, c.course_name
FROM Students s
LEFT JOIN Courses c ON s.course_id = c.course_id;

-- 2. Query from the view to find students enrolled in "Database Systems".
SELECT * FROM StudentCourseView WHERE course_name = 'Database Systems';

-- 3. Export your database using Workbench (Backup).
-- Workbench: Server -> Data Export -> select 'studentdb' -> Export to Self-Contained File

-- 4. Restore it into a new schema StudentDB_Copy.
-- Workbench: Server → Data Import -> Select Import from Self-Contained File -> Choose the studentdb_backup.sql file -> Choose New Schema and name it StudentDB_Copy -> Click Start Import.

-- ============================================
-- Final Test/Project for Week 2: Mini Employee Payroll System
-- ============================================

USE studentdb;

-- Create tables
CREATE TABLE IF NOT EXISTS Departments (
    dept_id INT PRIMARY KEY AUTO_INCREMENT,
    dept_name VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS Employees (
    emp_id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(100) NOT NULL,
    dept_id INT,
    salary DECIMAL(12,2),
    FOREIGN KEY (dept_id) REFERENCES Departments(dept_id)
);

CREATE TABLE IF NOT EXISTS Salaries (
    sal_id INT PRIMARY KEY AUTO_INCREMENT,
    emp_id INT,
    month VARCHAR(20),
    amount DECIMAL(12,2),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (emp_id) REFERENCES Employees(emp_id)
);

-- Table to log salary insertions
CREATE TABLE IF NOT EXISTS SalaryLogs (
    log_id INT PRIMARY KEY AUTO_INCREMENT,
    sal_id INT,
    emp_id INT,
    amount DECIMAL(12,2),
    month VARCHAR(20),
    logged_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Sample data
INSERT INTO Departments (dept_name) VALUES ('Engineering'), ('HR'), ('Sales');

INSERT INTO Employees (name, dept_id, salary) VALUES
('Ravi Kumar', 1, 60000),
('Priya Singh', 2, 45000),
('Amit Sharma', 1, 70000),
('Neha Patel', 3, 52000);

INSERT INTO Salaries (emp_id, month, amount) VALUES
(1, '2025-01', 60000),
(1, '2025-02', 60000),
(2, '2025-01', 45000),
(3, '2025-01', 70000);

-- Complex join: employees + department + latest salary (example)
SELECT e.emp_id, e.name, d.dept_name, s.month, s.amount
FROM Employees e
LEFT JOIN Departments d ON e.dept_id = d.dept_id
LEFT JOIN Salaries s ON e.emp_id = s.emp_id;

-- Subquery to find employees earning above department average.
SELECT e.emp_id, e.name, e.salary, d.dept_name
FROM Employees e
JOIN Departments d ON e.dept_id = d.dept_id
WHERE e.salary > (
    SELECT AVG(salary) FROM Employees WHERE dept_id = e.dept_id
);

-- SP to calculate yearly salary for an employee (sums Salaries.amount for the emp_id)
DROP PROCEDURE IF EXISTS CalculateYearlySalary;
DELIMITER $$
CREATE PROCEDURE CalculateYearlySalary(IN in_emp_id INT, IN in_year_prefix VARCHAR(5))
BEGIN
    -- in_year_prefix like '2025' to sum months that start with '2025-'
    SELECT in_emp_id AS emp_id, SUM(amount) AS yearly_total
    FROM Salaries
    WHERE emp_id = in_emp_id AND month LIKE CONCAT(in_year_prefix, '%');
END $$
DELIMITER ;

-- Example: CALL CalculateYearlySalary(1, '2025');

-- Trigger to auto-log salary insertions into SalaryLogs.
DROP TRIGGER IF EXISTS trg_log_salary_insert;
DELIMITER $$
CREATE TRIGGER trg_log_salary_insert
AFTER INSERT ON Salaries
FOR EACH ROW
BEGIN
    INSERT INTO SalaryLogs (sal_id, emp_id, amount, month)
    VALUES (NEW.sal_id, NEW.emp_id, NEW.amount, NEW.month);
END $$
DELIMITER ;

-- Create a view for employee salary summary (total paid, latest month).
CREATE OR REPLACE VIEW EmployeeSalarySummary AS
SELECT e.emp_id, e.name,
       d.dept_name,
       IFNULL(SUM(s.amount),0) AS total_paid,
       MAX(s.month) AS latest_month
FROM Employees e
LEFT JOIN Departments d ON e.dept_id = d.dept_id
LEFT JOIN Salaries s ON e.emp_id = s.emp_id
GROUP BY e.emp_id, e.name, d.dept_name;

-- Example query from the view:
SELECT * FROM EmployeeSalarySummary;

