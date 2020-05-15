USE [P1Context-1]
GO
--Clear the Order Details table
delete from [dbo].[OrderDetails]
GO
--Clear the Orders table
delete from [dbo].[Orders]
GO
--Clear the Products table then allow for identity inserts then add default Products
delete from [dbo].[Products]
GO
--Clear the StoreInventores table
delete from [dbo].[StoreInventories]
GO
--Clear the Stores table then allow for identity inserts then add default Stores
delete from [dbo].[Stores]
GO
--Clear the Customers table then allow for identity inserts then add default customers
delete from [dbo].[Customers]
GO
SET IDENTITY_INSERT [dbo].[Customers] ON; 

insert into [dbo].[Customers]([CustomerId], [FirstName], [LastName], [Email])
values(999, 'Admin', 'Admin', 'admin@email.com');
insert into [dbo].[Customers]([CustomerId], [FirstName], [LastName], [Email])
values(1, 'John', 'Doe', 'johndoe@email.com');
insert into [dbo].[Customers]([CustomerId], [FirstName], [LastName], [Email])
values(2, 'Adam', 'James', 'aj@email.com');
insert into [dbo].[Customers]([CustomerId], [FirstName], [LastName], [Email])
values(3, 'Jane', 'Doe', 'jdoe@email.com');
insert into [dbo].[Customers]([CustomerId], [FirstName], [LastName], [Email])
values(4, 'Don', 'Juan', 'donjuan@email.com');

SET IDENTITY_INSERT [dbo].[Customers] OFF;
GO
SET IDENTITY_INSERT [dbo].[Stores] ON;  

insert into [dbo].[Stores](StoreId, Location)
values(1, 'Seattle')
insert into [dbo].[Stores](StoreId, Location)
values(2, 'Redmond')
insert into [dbo].[Stores](StoreId, Location)
Values(3, 'Tacoma')
insert into [dbo].[Stores](StoreId, Location)
values(4, 'Olympia')
SET IDENTITY_INSERT [dbo].[Stores] OFF;  
GO 
SET IDENTITY_INSERT [dbo].[Products] ON;  
insert into [dbo].[Products] ([PoductId], ProductDescription, Price)
values(1, 'Widget', 21.99)
insert into [dbo].[Products] ([PoductId], ProductDescription, Price)
values(2, 'Whachamecallit', 5.99)
insert into [dbo].[Products] ([PoductId], ProductDescription, Price)
values(3, 'DooHicky', 7.99)
insert into [dbo].[Products] ([PoductId], ProductDescription, Price)
values(4, 'Thingy', 13.99)
insert into [dbo].[Products] ([PoductId], ProductDescription, Price)
values(5, 'Whatever', 10.99)
SET IDENTITY_INSERT [dbo].[Products] OFF;  
GO 
--Insert available products into store #1
insert into [dbo].[StoreInventories]
values(1, 1, 50)
insert into [dbo].[StoreInventories]
values(1, 3, 50)
insert into [dbo].[StoreInventories]
values(1, 4, 50)
insert into [dbo].[StoreInventories]
values(1, 5, 50)
GO
--Insert available products into store #2
insert into [dbo].[StoreInventories]
values(2, 1, 50)
insert into [dbo].[StoreInventories]
values(2, 2, 50)
insert into [dbo].[StoreInventories]
values(2, 4, 50)
insert into [dbo].[StoreInventories]
values(2, 5, 50)
GO
--Insert available products into store #3
insert into [dbo].[StoreInventories]
values(3, 1, 50)
insert into [dbo].[StoreInventories]
values(3, 2, 50)
insert into [dbo].[StoreInventories]
values(3, 3, 50)
insert into [dbo].[StoreInventories]
values(3, 5, 50)
GO
--Insert available products into store #4
insert into [dbo].[StoreInventories]
values(4, 1, 50)
insert into [dbo].[StoreInventories]
values(4, 2, 50)
insert into [dbo].[StoreInventories]
values(4, 3, 50)
insert into [dbo].[StoreInventories]
values(4, 4, 50)
GO
--Add the default orders for store 1 customer 1
SET IDENTITY_INSERT [dbo].[Orders] ON;
insert into [dbo].[Orders]([OrderId],[CusomerId],[StoreId],[OrderDateTime])
values(1, 1, 1, '04/20/2020')
insert into [dbo].[Orders]([OrderId],[CusomerId],[StoreId],[OrderDateTime])
values(2, 1, 1, '04/15/2020')
insert into [dbo].[Orders]([OrderId],[CusomerId],[StoreId],[OrderDateTime])
values(3, 1, 1, '04/19/2020')
SET IDENTITY_INSERT [dbo].[Orders] OFF;
GO
--insert the default order details for the above orders
insert into [dbo].[OrderDetails]
values(1, 1, 5, 21.99)
insert into [dbo].[OrderDetails]
values(1, 3, 2, 7.99)
GO
insert into [dbo].[OrderDetails]
values(2, 5, 5, 10.99)
GO
insert into [dbo].[OrderDetails]
values(3, 3, 1, 7.99)
insert into [dbo].[OrderDetails]
values(3, 4, 2, 13.99)
insert into [dbo].[OrderDetails]
values(3, 1, 2, 21.99)
GO

