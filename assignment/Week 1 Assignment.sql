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




