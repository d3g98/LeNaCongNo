create table dmathang
(
	id varchar(36) primary key,
	code varchar(50) ,
	name nvarchar(255),
	dongia int
);
create table dkhachhang
(
	id varchar(36) primary key,
	code varchar(50) ,
	name nvarchar(255) ,
	dienthoai varchar(10),
	diachi nvarchar(255),
);
create table dbanggia
(
	id varchar(36) primary key,
	name nvarchar(255) ,
	tungay date,
	denngay date
)
create table dbanggiachitiet
(
	id varchar(36) primary key,
	dbanggiaid varchar(36),
	dmathangid varchar(36),
	duoi1kg int,
	tu1kgtrolen int
)
create table tdonhang
(
	id varchar(36) primary key,
	ngay date,
	name nvarchar(255) ,
	dkhachhangid varchar(36),
	note nvarchar(1000),
	tongcong int
)
create table tdonhangchitiet
(
	id varchar(36) primary key,
	tdonhangid varchar(36),
	dmathangid varchar(36),
	soluong decimal(18,2),
	dongia int,
	thanhtien int
)
create table tthanhtoan
(
	id varchar(36) primary key,
	ngay date,
	name nvarchar(255) ,
	dkhachhangid varchar(36),
	tongcong int,
	note nvarchar(1000) 
)