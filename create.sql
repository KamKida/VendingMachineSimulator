CREATE DATABASE VendingMachine;

USE VendingMachine;

CREATE TABLE Products (
[Product ID] INT IDENTITY(55,1) PRIMARY KEY ,
[Product Name] VARCHAR(20) NOT NULL,
[Amount In Machine] INT NOT NULL CHECK ([Amount In Machine] <=10 AND [Amount In Machine] >= 0 ),
[Product Price In $] MONEY NOT NULL CHECK([Product Price In $] <=7.00),
);


CREATE TABLE Transactions(
[Date Of Transaction] DATE NOT NULL,
[Lenght Of Transaction] TIME(0) NOT NULL,
[Profits] MONEY NOT NULL CHECK([Profits] > 0)
);

CREATE TABLE Workers(
[Worker ID] INT IDENTITY(68718,1) NOT NULL,
[Name And Surname] VARCHAR(50) NOT NULL,
[Worker Login] VARCHAR(10) NOT NULL,
[Worker Password] VARCHAR(10) NOT NULL,
PRIMARY KEY([Worker ID])
);

CREATE TABLE [Workers History](
[Date Of Login] DATE NOT NULL,
[Worker ID] INT,
[Worker Operations] VARCHAR(200) NOT NULL
);

CREATE TABLE [Money In Machine] (
ID INT IDENTITY(1,1) PRIMARY KEY,
[Current Money In Machine] MONEY, 
CHECK([Current money in machine] >=0));
