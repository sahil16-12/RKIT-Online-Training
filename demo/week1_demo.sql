-- Week 1 MySQL Practice Queries

-- ===============================
-- DDL: CREATE TABLES
-- ===============================
CREATE DATABASE organization_db;
USE organization_db;

CREATE TABLE departments (
  dept_id INT AUTO_INCREMENT PRIMARY KEY,
  dept_name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE employees (
  emp_id INT AUTO_INCREMENT PRIMARY KEY,
  full_name VARCHAR(100) NOT NULL,
  dept_id INT,
  salary DECIMAL(10,2) DEFAULT 0,
  hired_on DATE,
  CONSTRAINT fk_dept FOREIGN KEY (dept_id) REFERENCES departments(dept_id)
);

-- ===============================
-- DML: INSERT DATA
-- ===============================

-- Insert into departments
INSERT INTO departments (dept_name) VALUES
('Engineering'),
('Human Resources'),
('Sales'),
('Marketing');

-- Insert employees
INSERT INTO employees (full_name, dept_id, salary, hired_on) VALUES
('Alice', 1, 60000, '2023-08-01'),
('Bob', 1, 55000, '2023-07-15'),
('Charlie', 2, 45000, '2023-06-20'),
('Diana', 3, 50000, '2023-05-10'),
('Eve', NULL, 40000, '2023-04-05');

-- Update examples
UPDATE employees SET salary = salary * 1.10 WHERE dept_id = 1;
UPDATE employees SET salary = 70000 WHERE emp_id = 2;

-- Delete examples
DELETE FROM employees WHERE emp_id = 5;
-- TRUNCATE example
-- TRUNCATE TABLE employees;

-- ===============================
-- DQL: SELECT QUERIES
-- ===============================

-- Basic SELECT
SELECT * FROM employees;
SELECT emp_id, full_name AS name, salary FROM employees;

-- WHERE examples
SELECT * FROM employees WHERE salary > 50000;
SELECT * FROM employees WHERE dept_id = 1 AND salary BETWEEN 40000 AND 80000;
SELECT * FROM employees WHERE full_name LIKE 'A%';
SELECT * FROM employees WHERE dept_id IS NULL;

-- ORDER BY and LIMIT
SELECT full_name, salary FROM employees ORDER BY salary DESC LIMIT 3;
SELECT full_name, salary FROM employees ORDER BY salary DESC LIMIT 2 OFFSET 2;

-- GROUP BY and HAVING
SELECT d.dept_name, COUNT(e.emp_id) AS emp_count
FROM departments d
LEFT JOIN employees e ON d.dept_id = e.dept_id
GROUP BY d.dept_name;

SELECT d.dept_name, AVG(e.salary) AS avg_salary
FROM departments d
JOIN employees e ON d.dept_id = e.dept_id
GROUP BY d.dept_name
HAVING AVG(e.salary) > 50000;

-- ===============================
-- JOINS
-- ===============================

-- INNER JOIN
SELECT e.emp_id, e.full_name, d.dept_name
FROM employees e
INNER JOIN departments d ON e.dept_id = d.dept_id;

-- LEFT JOIN
SELECT e.emp_id, e.full_name, d.dept_name
FROM employees e
LEFT JOIN departments d ON e.dept_id = d.dept_id;

-- RIGHT JOIN
SELECT e.emp_id, e.full_name, d.dept_name
FROM employees e
RIGHT JOIN departments d ON e.dept_id = d.dept_id;

-- SELF JOIN example (employees managing other employees, hypothetical)
-- (Assume manager_id column exists; for demo only)
-- SELECT e1.full_name AS employee, e2.full_name AS manager
-- FROM employees e1
-- JOIN employees e2 ON e1.manager_id = e2.emp_id;

-- Find departments with no employees
SELECT d.dept_name
FROM departments d
LEFT JOIN employees e ON d.dept_id = e.dept_id
WHERE e.emp_id IS NULL;

-- ===============================
-- AGGREGATE FUNCTIONS
-- ===============================

SELECT SUM(salary) AS total_payroll FROM employees;
SELECT COUNT(*) AS total_employees FROM employees;
SELECT COUNT(dept_id) AS employees_with_dept FROM employees;
SELECT AVG(salary) AS avg_salary FROM employees;
SELECT MIN(salary) AS min_salary, MAX(salary) AS max_salary FROM employees;

-- Combined per department
SELECT d.dept_name,
       COUNT(e.emp_id) AS emp_count,
       SUM(e.salary) AS total_salary,
       AVG(e.salary) AS avg_salary
FROM departments d
LEFT JOIN employees e ON d.dept_id = e.dept_id
GROUP BY d.dept_name;
