CREATE TABLE t_Company (
Id int identity(1,1) primary key,
[Name] varchar(255),
Adress varchar(255),
City varchar(255),
Country varchar(255),
DateRegister Datetime DEFAULT(GETDATE()))