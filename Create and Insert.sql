/*CREATE DATABASE Fernlea;

USE Fernlea;*/


CREATE TABLE Priority (
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Priority_Type VARCHAR(255)
);

CREATE TABLE Task (
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Title VARCHAR(255) NOT NULL,
    Description VARCHAR(1000),
    Date_Due DATE NOT NULL,
    Priority_Id INT NOT NULL,
    Done BIT NOT NULL DEFAULT 0,
	CONSTRAINT fk_Priority_Id FOREIGN KEY(Priority_Id) REFERENCES Priority(Id)
);

CREATE TABLE Note (
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Task_Id INT NOT NULL,
    Content VARCHAR(1000),
    FOREIGN KEY(Task_Id) REFERENCES Task(Id)
);


INSERT INTO Priority(Priority_Type) VALUES
('Low'),
('Medium'),
('High'),
('Immediate');


INSERT INTO Task(Title, Description, Date_Due, Priority_Id) VALUES
('To Do Program', 'Lets you keep track of tasks, mark as done, delete and add notes to them', '2018-06-18', 3),
('Order milk', '', '2018-07-20', 1),
('Order pizza', 'Need food!', '2018-06-15', 4),
('fix car', 'needs an oil change', '2018-06-28', 2);


INSERT INTO Note(Task_Id, Content) VALUES
(1, 'Using ASP.NET MVC with MySQL and the entity framework'),
(3, 'jumbo with pepperoni');