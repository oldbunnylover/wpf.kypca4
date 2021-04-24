USE master;
CREATE database KP_Library

use KP_Library;
create table Users 
(    
	username nvarchar(255) primary key,  
    role int default 0,
	fullname nvarchar(255) not null,
	password nvarchar(255) not null,
	blocked bit default 0
 )

create table Genres
(
	genreID int primary key IDENTITY(0,1),
	genrename varchar(255) not null
)

create table Books 
(    
	bookID int primary key IDENTITY(0,1),  
    bookname nvarchar(255) not null,
	description nvarchar(2000) not null,
	filepath nvarchar(1000) not null unique,
	applyed bit default 0,
	genreID int foreign key references dbo.Genres(genreID)
)

create table SavedBooks 
(    
	username nvarchar(255) foreign key references dbo.Users(username),
	bookID int foreign key references dbo.Books(bookID),
	primary key(username, bookID)
)

create table BooksAuthors 
(    
	username nvarchar(255) foreign key references dbo.Users(username),
	bookID int foreign key references dbo.Books(bookID),
	primary key(username, bookID)
)

drop table SavedBooks
drop table BooksAuthors
drop table Books
drop table Users
drop table Genres 