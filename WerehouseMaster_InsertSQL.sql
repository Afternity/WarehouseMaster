-- Скрипт заполнения базы данных WarehouseMasterDB тестовыми данными
USE WarehouseMasterDB;
GO

-- Очистка таблиц (если нужно)
DELETE FROM Orders;
DELETE FROM Employees;
DELETE FROM Locations;
DELETE FROM Pallets;
DELETE FROM Products;
DELETE FROM Suppliers;
GO

-- Заполнение таблицы Suppliers (Поставщики) - 10 записей
INSERT INTO Suppliers (Id, CompanyName, ContactPerson, Phone, Email, Address)
VALUES
    (NEWID(), 'ООО "Рога и копыта"', 'Иван Иванов', '+7-999-123-45-67', 'sales@roga-i-kopyta.ru', 'г. Москва, ул. Промышленная, д. 1'),
    (NEWID(), 'ЗАО "МеталлПрофи"', 'Петр Петров', '+7-999-234-56-78', 'info@metalprofi.ru', 'г. Санкт-Петербург, пр. Заводской, д. 25'),
    (NEWID(), 'ОАО "Электросила"', 'Сергей Сергеев', '+7-999-345-67-89', 'contact@electrosila.com', 'г. Екатеринбург, ул. Энергетиков, д. 10'),
    (NEWID(), 'ИП "СтройМатериалы"', 'Анна Смирнова', '+7-999-456-78-90', 'anna@stroymat.ru', 'г. Новосибирск, ул. Строителей, д. 15'),
    (NEWID(), 'ТД "ТехноИмпорт"', 'Дмитрий Козлов', '+7-999-567-89-01', 'order@technoimport.com', 'г. Казань, пр. Технический, д. 8'),
    (NEWID(), 'ООО "ХимПром"', 'Ольга Новикова', '+7-999-678-90-12', 'info@khimprom.ru', 'г. Нижний Новгород, ул. Химическая, д. 5'),
    (NEWID(), 'АО "Машинострой"', 'Владимир Морозов', '+7-999-789-01-23', 'sales@mashinstroy.com', 'г. Самара, ул. Машиностроителей, д. 12'),
    (NEWID(), 'ООО "Деревообработка"', 'Елена Волкова', '+7-999-890-12-34', 'elena@derevo.ru', 'г. Воронеж, ул. Лесная, д. 30'),
    (NEWID(), 'ЗАО "Пищепром"', 'Алексей Лебедев', '+7-999-901-23-45', 'order@foodprom.ru', 'г. Ростов-на-Дону, ул. Пищевая, д. 7'),
    (NEWID(), 'ООО "ТекстильГрупп"', 'Наталья Ковалева', '+7-999-012-34-56', 'natalia@textilegroup.ru', 'г. Уфа, ул. Ткацкая, д. 18');
GO

-- Заполнение таблицы Employees (Сотрудники) - 10 записей
INSERT INTO Employees (Id, FullName, Email, Password)
VALUES
    (NEWID(), 'Александр Васильев', 'alex.vasiliev@warehouse.ru', 'password123'),
    (NEWID(), 'Мария Петрова', 'maria.petrova@warehouse.ru', 'password123'),
    (NEWID(), 'Андрей Соколов', 'andrey.sokolov@warehouse.ru', 'password123'),
    (NEWID(), 'Екатерина Иванова', 'ekaterina.ivanova@warehouse.ru', 'password123'),
    (NEWID(), 'Денис Кузнецов', 'denis.kuznetsov@warehouse.ru', 'password123'),
    (NEWID(), 'Ольга Смирнова', 'olga.smirnova@warehouse.ru', 'password123'),
    (NEWID(), 'Михаил Попов', 'mikhail.popov@warehouse.ru', 'password123'),
    (NEWID(), 'Анна Федорова', 'anna.fedorova@warehouse.ru', 'password123'),
    (NEWID(), 'Павел Морозов', 'pavel.morozov@warehouse.ru', 'password123'),
    (NEWID(), 'Ирина Николаева', 'irina.nikolaeva@warehouse.ru', 'password123');
GO

-- Заполнение таблицы Locations (Локации) - 10 записей
INSERT INTO Locations (Id, Name, Code, Width, Length, Height)
VALUES
    (NEWID(), 'Сектор А-1', 'A1', 10.0, 5.0, 3.0),
    (NEWID(), 'Сектор А-2', 'A2', 8.0, 4.0, 3.0),
    (NEWID(), 'Сектор Б-1', 'B1', 12.0, 6.0, 4.0),
    (NEWID(), 'Сектор Б-2', 'B2', 15.0, 8.0, 4.0),
    (NEWID(), 'Сектор В-1', 'C1', 6.0, 4.0, 3.0),
    (NEWID(), 'Сектор В-2', 'C2', 10.0, 5.0, 3.5),
    (NEWID(), 'Холодильная камера 1', 'HC1', 8.0, 6.0, 3.0),
    (NEWID(), 'Холодильная камера 2', 'HC2', 12.0, 8.0, 3.0),
    (NEWID(), 'Сектор Д-1', 'D1', 20.0, 10.0, 5.0),
    (NEWID(), 'Сектор Д-2', 'D2', 15.0, 7.0, 4.0);
GO

-- Заполнение таблицы Pallets (Паллеты) - 10 записей
INSERT INTO Pallets (Id, Name, Barcode, Weight, Length)
VALUES
    (NEWID(), 'Паллета 001', 'PAL001', 25.0, 1.2),
    (NEWID(), 'Паллета 002', 'PAL002', 30.0, 1.2),
    (NEWID(), 'Паллета 003', 'PAL003', 20.0, 0.8),
    (NEWID(), 'Паллета 004', 'PAL004', 35.0, 1.5),
    (NEWID(), 'Паллета 005', 'PAL005', 28.0, 1.2),
    (NEWID(), 'Паллета 006', 'PAL006', 22.0, 1.0),
    (NEWID(), 'Паллета 007', 'PAL007', 40.0, 1.8),
    (NEWID(), 'Паллета 008', 'PAL008', 26.0, 1.2),
    (NEWID(), 'Паллета 009', 'PAL009', 32.0, 1.4),
    (NEWID(), 'Паллета 010', 'PAL010', 24.0, 1.1);
GO

-- Заполнение таблицы Products (Продукты) - 10 записей
INSERT INTO Products (Id, Name, Description, Price, Type)
VALUES
    (NEWID(), 'Стальной лист 2мм', 'Стальной лист толщиной 2мм', 1500.00, 'Металлопрокат'),
    (NEWID(), 'Медь катодная', 'Медь катодная высшей очистки', 65000.00, 'Цветные металлы'),
    (NEWID(), 'Кабель ВВГ 3х2.5', 'Кабель силовой ВВГ 3х2.5мм?', 85.50, 'Электротехника'),
    (NEWID(), 'Цемент М500', 'Цемент марки М500 в мешках 50кг', 450.00, 'Строительные материалы'),
    (NEWID(), 'Доска сосновая', 'Доска сосновая 50х100х6000мм', 12000.00, 'Пиломатериалы'),
    (NEWID(), 'Полипропилен гранулы', 'Гранулы полипропилена для литья', 95.00, 'Полимеры'),
    (NEWID(), 'Аккумулятор 12V', 'Автомобильный аккумулятор 12V 60Ah', 4200.00, 'Автозапчасти'),
    (NEWID(), 'Мясо говяжье', 'Говядина охлажденная', 650.00, 'Продукты питания'),
    (NEWID(), 'Хлопковая ткань', 'Ткань хлопковая белая 150см', 350.00, 'Текстиль'),
    (NEWID(), 'Лак мебельный', 'Лак для мебели прозрачный 5л', 2800.00, 'ЛКМ');
GO

-- Заполнение таблицы Orders (Заказы) - 20 записей
DECLARE @Supplier1 UNIQUEIDENTIFIER, @Supplier2 UNIQUEIDENTIFIER, @Supplier3 UNIQUEIDENTIFIER, @Supplier4 UNIQUEIDENTIFIER, @Supplier5 UNIQUEIDENTIFIER;
DECLARE @Employee1 UNIQUEIDENTIFIER, @Employee2 UNIQUEIDENTIFIER, @Employee3 UNIQUEIDENTIFIER, @Employee4 UNIQUEIDENTIFIER, @Employee5 UNIQUEIDENTIFIER;
DECLARE @Location1 UNIQUEIDENTIFIER, @Location2 UNIQUEIDENTIFIER, @Location3 UNIQUEIDENTIFIER, @Location4 UNIQUEIDENTIFIER, @Location5 UNIQUEIDENTIFIER;
DECLARE @Pallet1 UNIQUEIDENTIFIER, @Pallet2 UNIQUEIDENTIFIER, @Pallet3 UNIQUEIDENTIFIER, @Pallet4 UNIQUEIDENTIFIER, @Pallet5 UNIQUEIDENTIFIER;
DECLARE @Product1 UNIQUEIDENTIFIER, @Product2 UNIQUEIDENTIFIER, @Product3 UNIQUEIDENTIFIER, @Product4 UNIQUEIDENTIFIER, @Product5 UNIQUEIDENTIFIER;

-- Получаем ID из вставленных записей
SELECT TOP 1 @Supplier1 = Id FROM Suppliers;
SELECT TOP 1 @Supplier2 = Id FROM Suppliers ORDER BY CompanyName DESC;
SELECT TOP 1 @Supplier3 = Id FROM Suppliers WHERE CompanyName LIKE '%Металл%';
SELECT TOP 1 @Supplier4 = Id FROM Suppliers WHERE CompanyName LIKE '%Электро%';
SELECT TOP 1 @Supplier5 = Id FROM Suppliers WHERE CompanyName LIKE '%Строй%';

SELECT TOP 1 @Employee1 = Id FROM Employees;
SELECT TOP 1 @Employee2 = Id FROM Employees ORDER BY FullName DESC;
SELECT TOP 1 @Employee3 = Id FROM Employees WHERE FullName LIKE '%Мария%';
SELECT TOP 1 @Employee4 = Id FROM Employees WHERE FullName LIKE '%Андрей%';
SELECT TOP 1 @Employee5 = Id FROM Employees WHERE FullName LIKE '%Екатерина%';

SELECT TOP 1 @Location1 = Id FROM Locations;
SELECT TOP 1 @Location2 = Id FROM Locations ORDER BY Name DESC;
SELECT TOP 1 @Location3 = Id FROM Locations WHERE Name LIKE '%А-2%';
SELECT TOP 1 @Location4 = Id FROM Locations WHERE Name LIKE '%Б-1%';
SELECT TOP 1 @Location5 = Id FROM Locations WHERE Name LIKE '%Холод%';

SELECT TOP 1 @Pallet1 = Id FROM Pallets;
SELECT TOP 1 @Pallet2 = Id FROM Pallets ORDER BY Name DESC;
SELECT TOP 1 @Pallet3 = Id FROM Pallets WHERE Name LIKE '%003%';
SELECT TOP 1 @Pallet4 = Id FROM Pallets WHERE Name LIKE '%004%';
SELECT TOP 1 @Pallet5 = Id FROM Pallets WHERE Name LIKE '%005%';

SELECT TOP 1 @Product1 = Id FROM Products;
SELECT TOP 1 @Product2 = Id FROM Products ORDER BY Name DESC;
SELECT TOP 1 @Product3 = Id FROM Products WHERE Name LIKE '%Кабель%';
SELECT TOP 1 @Product4 = Id FROM Products WHERE Name LIKE '%Цемент%';
SELECT TOP 1 @Product5 = Id FROM Products WHERE Name LIKE '%Доска%';

INSERT INTO Orders (Id, DateTime, ProductCount, PalletCount, TotalPrice, PalletId, ProductId, EmployeeId, LocationId, SupplierId)
VALUES
    (NEWID(), DATEADD(DAY, -10, GETDATE()), 100, 5, 150000.00, @Pallet1, @Product1, @Employee1, @Location1, @Supplier1),
    (NEWID(), DATEADD(DAY, -9, GETDATE()), 50, 3, 3250000.00, @Pallet2, @Product2, @Employee2, @Location2, @Supplier2),
    (NEWID(), DATEADD(DAY, -8, GETDATE()), 200, 8, 17100.00, @Pallet3, @Product3, @Employee3, @Location3, @Supplier3),
    (NEWID(), DATEADD(DAY, -7, GETDATE()), 40, 2, 18000.00, @Pallet4, @Product4, @Employee4, @Location4, @Supplier4),
    (NEWID(), DATEADD(DAY, -6, GETDATE()), 15, 1, 180000.00, @Pallet5, @Product5, @Employee5, @Location5, @Supplier5),
    (NEWID(), DATEADD(DAY, -5, GETDATE()), 80, 4, 7600.00, @Pallet1, @Product3, @Employee1, @Location2, @Supplier3),
    (NEWID(), DATEADD(DAY, -4, GETDATE()), 25, 2, 105000.00, @Pallet2, @Product5, @Employee2, @Location3, @Supplier5),
    (NEWID(), DATEADD(DAY, -3, GETDATE()), 60, 3, 27000.00, @Pallet3, @Product4, @Employee3, @Location4, @Supplier4),
    (NEWID(), DATEADD(DAY, -2, GETDATE()), 120, 6, 180000.00, @Pallet4, @Product1, @Employee4, @Location5, @Supplier1),
    (NEWID(), DATEADD(DAY, -1, GETDATE()), 30, 2, 1950000.00, @Pallet5, @Product2, @Employee5, @Location1, @Supplier2),
    (NEWID(), GETDATE(), 75, 4, 7125.00, @Pallet1, @Product3, @Employee1, @Location3, @Supplier3),
    (NEWID(), GETDATE(), 20, 1, 84000.00, @Pallet2, @Product5, @Employee2, @Location4, @Supplier5),
    (NEWID(), GETDATE(), 45, 2, 20250.00, @Pallet3, @Product4, @Employee3, @Location5, @Supplier4),
    (NEWID(), GETDATE(), 90, 5, 135000.00, @Pallet4, @Product1, @Employee4, @Location1, @Supplier1),
    (NEWID(), GETDATE(), 35, 2, 2275000.00, @Pallet5, @Product2, @Employee5, @Location2, @Supplier2),
    (NEWID(), DATEADD(HOUR, 1, GETDATE()), 110, 6, 10450.00, @Pallet1, @Product3, @Employee1, @Location4, @Supplier3),
    (NEWID(), DATEADD(HOUR, 2, GETDATE()), 18, 1, 75600.00, @Pallet2, @Product5, @Employee2, @Location5, @Supplier5),
    (NEWID(), DATEADD(HOUR, 3, GETDATE()), 55, 3, 24750.00, @Pallet3, @Product4, @Employee3, @Location1, @Supplier4),
    (NEWID(), DATEADD(HOUR, 4, GETDATE()), 95, 5, 142500.00, @Pallet4, @Product1, @Employee4, @Location2, @Supplier1),
    (NEWID(), DATEADD(HOUR, 5, GETDATE()), 28, 2, 1820000.00, @Pallet5, @Product2, @Employee5, @Location3, @Supplier2);
GO

PRINT 'База данных успешно заполнена тестовыми данными!';
PRINT 'Suppliers: 10 записей';
PRINT 'Employees: 10 записей';
PRINT 'Locations: 10 записей';
PRINT 'Pallets: 10 записей';
PRINT 'Products: 10 записей';
PRINT 'Orders: 20 записей';