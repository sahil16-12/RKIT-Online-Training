CREATE DATABASE week2db;
USE week2db;

-- ================================
-- Create Tables
-- ================================

-- Students table
CREATE TABLE students (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50),
    created_at DATETIME
);

-- Marks table
CREATE TABLE marks (
    id INT AUTO_INCREMENT PRIMARY KEY,
    student_id INT,
    score INT,
    FOREIGN KEY (student_id) REFERENCES students(id)
);

-- Customers table
CREATE TABLE customers (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50),
    city VARCHAR(50)
);

-- Suppliers table
CREATE TABLE suppliers (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50),
    city VARCHAR(50)
);

-- ================================
-- Insert Sample Data
-- ================================

INSERT INTO students (name) VALUES ('Alice'), ('Bob'), ('Charlie'), ('David');
INSERT INTO marks (student_id, score) VALUES
(1, 85), (1, 78), (2, 90), (3, 65), (4, 88);

INSERT INTO customers (name, city) VALUES
('Cust1', 'New York'), ('Cust2', 'Chicago'), ('Cust3', 'Boston');

INSERT INTO suppliers (name, city) VALUES
('Supp1', 'Boston'), ('Supp2', 'Chicago'), ('Supp3', 'Dallas');

-- ================================
-- Subqueries Examples
-- ================================

-- Nested Subquery
SELECT name 
FROM students 
WHERE id IN (
  SELECT student_id 
  FROM marks 
  WHERE score > 80
);

-- Correlated Subquery
SELECT s.name, s.id
FROM students s
WHERE EXISTS (
  SELECT 1
  FROM marks m
  WHERE m.student_id = s.id AND m.score > 70
);

-- ================================
-- UNION and UNION ALL Examples
-- ================================

-- UNION
SELECT city FROM customers
UNION
SELECT city FROM suppliers;

-- UNION ALL
SELECT city FROM customers
UNION ALL
SELECT city FROM suppliers;

-- ================================
-- Stored Procedure Examples
-- ================================

DELIMITER $$
CREATE PROCEDURE get_students()
BEGIN
   SELECT * FROM students;
END $$

CREATE PROCEDURE get_marks(IN student_id INT, OUT avg_marks FLOAT)
BEGIN
   SELECT AVG(score) INTO avg_marks 
   FROM marks 
   WHERE marks.student_id = student_id;
END $$
DELIMITER ;

CALL get_students();

CALL get_marks(1,@total);

select @total;

-- ================================
-- Trigger Example
-- ================================

DELIMITER $$
CREATE TRIGGER before_insert_students
BEFORE INSERT ON students
FOR EACH ROW
BEGIN
   SET NEW.created_at = NOW();
END $$
DELIMITER ;

-- ================================
-- Function Example
-- ================================

DELIMITER $$
CREATE FUNCTION get_total_marks(s_id INT) RETURNS INT
DETERMINISTIC
BEGIN
   DECLARE total INT;
   SELECT SUM(score) INTO total FROM marks WHERE student_id = s_id;
   RETURN total;
END $$
DELIMITER ;

-- Usage example
SELECT name, get_total_marks(id) AS total_score FROM students;

-- ================================
-- View Example
-- ================================

CREATE VIEW high_scorers AS
SELECT s.name, m.score
FROM students s
JOIN marks m ON s.id = m.student_id
WHERE m.score > 80;

-- Using the view
SELECT * FROM high_scorers;


-- ================================
-- Backup and Restore Examples
-- ================================

-- Logical Backup using mysqldump (From terminal)
-- mysqldump -u root -p *** > week2db_backup.sql

-- Restore Backup (from terminal)
-- mysql -u root -p *** < week2db_backup.sql

-- ================================
-- EXPLAIN Examples for Query Optimization
-- ================================

-- Without Index: This will cause a full table scan (type = ALL)
EXPLAIN SELECT * FROM students WHERE name = 'Alice';

-- Create an index on 'name' column to optimize search
CREATE INDEX idx_name ON students(name);

-- With Index: Now MySQL can use the index (type = ref)
EXPLAIN SELECT * FROM students WHERE name = 'Alice';

-- Create another index on 'score' column of marks
CREATE INDEX idx_score ON marks(score);

-- Query using range search on score (type = range)
EXPLAIN SELECT * FROM marks WHERE score > 80;


-- ================================
-- Infinite Trigger Prevention Using is_auto Column
-- ================================

-- Table A with is_auto flag
CREATE TABLE A (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50),
    is_auto BOOLEAN DEFAULT 0
);

-- Table B with is_auto flag
CREATE TABLE B (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50),
    is_auto BOOLEAN DEFAULT 0
);

DELIMITER $$

-- Trigger: insert into B when a manual row is added to A

CREATE TRIGGER trg_a_insert
AFTER INSERT ON A
FOR EACH ROW
BEGIN
    IF NEW.is_auto = 0 THEN
        INSERT INTO B (name, is_auto) VALUES (CONCAT('From A: ', NEW.name), 1);
    END IF;
END $$

-- Trigger: insert into A when a manual row is added to B

CREATE TRIGGER trg_b_insert
AFTER INSERT ON B
FOR EACH ROW
BEGIN
    IF NEW.is_auto = 0 THEN
        INSERT INTO A (name, is_auto) VALUES (CONCAT('From B: ', NEW.name), 1);
    END IF;
END $$
DELIMITER ;

-- Insert into A (manual, is_auto=0 by default).
-- This will insert into B (is_auto=1).
INSERT INTO A (name) VALUES ('Manual Insert A');

-- Insert into B (manual).
INSERT INTO B (name) VALUES ('Manual Insert B');

-- Check results
SELECT * FROM A;
SELECT * FROM B;



-- Trigger to automatically append student name with course name on update student

USE studentdb;

DELIMITER //
CREATE TRIGGER trg_author_insert
AFTER UPDATE ON students
FOR EACH ROW
BEGIN
UPDATE COURSES SET course_name = CONCAT(course_name, '-', NEW.name) WHERE course_id = NEW.course_id;
END //

DELIMITER ;

SELECT * FROM STUDENTS;
SELECT * FROM COURSES;

UPDATE students set name='sahil' WHERE student_id = 7;
