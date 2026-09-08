create table Genders(
	Id int primary key identity(1,1),
	Name nvarchar(255)
);

create table Clients(
	Id int primary key identity(1,1),
	LastName nvarchar(255) not null,
	FirstName nvarchar(255) not null,
	MiddleName nvarchar(255),
	GenderId int foreign key references Genders(Id) not null,
	Phone nvarchar(50),
	Birthday date,
	Email nvarchar(100),
	RegDay date
);

create table Services(
	Id int primary key identity(1,1),
	Name nvarchar(255) not null,
	Image nvarchar(255),
	Duration int not null,
	Cost decimal(10,2),
	Discount int
);

create table ServiceClient(
	Id int primary key identity(1,1),
	ServiceId int foreign key references Services(Id) not null,
	StartDate datetime,
	ClientId int foreign key references Clients(Id) not null
);

create table Manufacturers(
	Id int primary key identity(1,1),
	Name nvarchar(100) not null,
	StartDate date
);

create table Products(
	Id int primary key identity(1,1),
	Title nvarchar(100) not null,
	Cost money not null,
	Description nvarchar(max),
	MainImagePath nvarchar(1000),
	IsActive bit not null,
	ManufacturerId int foreign key references Manufacturers(Id)
);

create table AttachedProducts(
	MainProductId int foreign key references Products(Id) not null,
	AttachedProductId int foreign key references Products(Id) not null,
	constraint PK_AttachedProduct primary key (MainProductId, AttachedProductId)
);

create table ProductPhotos(
	Id int primary key identity(1,1),
	ProductId int foreign key references Products(Id) not null,
	PhotoPath nvarchar(1000) not null
);

create table ProductSales(
	Id int primary key identity(1,1),
	SaleDate datetime not null,
	ProductId int foreign key references Products(Id) not null,
	Quantity int not null,
	ClientServiceId int foreign key references ServiceClient(Id)
);

create table Tags(
	Id int primary key identity(1,1),
	Title nvarchar(30) not null,
	Color nchar(6) not null
);

create table TagOfClients(
	ClientId int foreign key references Clients(Id) not null,
	TagId int foreign key references Tags(Id) not null,
	constraint PK_TagOfClient primary key (ClientId, TagId)
);

create table ServicePhotos(
	Id int primary key identity(1,1),
	ServiceId int foreign key references Services(Id) not null,
	PhotoPath nvarchar(1000) not null
);

create table DocumentsByService(
	Id int primary key identity(1,1),
	ClientServiceId int foreign key references ServiceClient(Id) not null,
	DocumentPath nvarchar(1000) not null
);