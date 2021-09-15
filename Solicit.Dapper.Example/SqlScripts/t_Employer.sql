CREATE TABLE t_Employer (
Id int identity(1,1) primary key,
IdCompany int,
[Name] varchar(255),
Adress varchar(255),
City varchar(255),
Country varchar(255),
ZipCode varchar(255),
DateRegister Datetime DEFAULT(GETDATE()))