using Microsoft.EntityFrameworkCore;
using WarehouseMaster.Domain.Models;
using WarehouseMaster.Persistence.Data.DbContexts;
using WarehouseMaster.WPF.Common.Profiles;
using WarehouseMaster.WPF.Windows;

namespace WarehouseMaster.Tests.WinodowTests.ProfileWindowTests
{
    public class ProfileWindowMetricsTests : IDisposable
    {
        private WarehouseMasterDbContext _context;

        private WarehouseMasterDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<WarehouseMasterDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new WarehouseMasterDbContext(options);
        }

        [StaFact]
        public async Task GetMyOrderByEmployeeProfileId_WithOrders_ReturnsCorrectCount()
        {
            // Arrange
            _context = CreateInMemoryContext();

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FullName = "Тестовый сотрудник",
                Email = "test@warehouse.ru",
                Password = "password"
            };

            var supplier = new Supplier
            {
                Id = Guid.NewGuid(),
                CompanyName = "Тестовый поставщик"
            };

            await _context.Employees.AddAsync(employee);
            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();

            // Создаем заказы для сотрудника
            var orders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), EmployeeId = employee.Id, SupplierId = supplier.Id },
                new Order { Id = Guid.NewGuid(), EmployeeId = employee.Id, SupplierId = supplier.Id }
            };

            await _context.Orders.AddRangeAsync(orders);
            await _context.SaveChangesAsync();

            // Устанавливаем профиль
            EmployeeProfile.Profile = employee;

            var window = new ProfileWindow();
            var contextField = typeof(ProfileWindow).GetField("_context",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            contextField.SetValue(window, _context);

            // Act
            var orderCountMethod = typeof(ProfileWindow).GetMethod("GetMyOrderByEmployeeProfileId",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var task = (Task<int>)orderCountMethod.Invoke(window, null);
            var orderCount = await task;

            // Assert
            Assert.Equal(2, orderCount);
        }

        [StaFact]
        public async Task GetMySupplierByEmployeeProfileId_WithOrders_ReturnsCorrectCount()
        {
            // Arrange
            _context = CreateInMemoryContext();

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FullName = "Тестовый сотрудник",
                Email = "test@warehouse.ru",
                Password = "password"
            };

            var suppliers = new List<Supplier>
            {
                new Supplier { Id = Guid.NewGuid(), CompanyName = "Поставщик 1" },
                new Supplier { Id = Guid.NewGuid(), CompanyName = "Поставщик 2" },
                new Supplier { Id = Guid.NewGuid(), CompanyName = "Поставщик 3" }
            };

            await _context.Employees.AddAsync(employee);
            await _context.Suppliers.AddRangeAsync(suppliers);
            await _context.SaveChangesAsync();

            // Создаем заказы с разными поставщиками
            var orders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), EmployeeId = employee.Id, SupplierId = suppliers[0].Id },
                new Order { Id = Guid.NewGuid(), EmployeeId = employee.Id, SupplierId = suppliers[1].Id },
                new Order { Id = Guid.NewGuid(), EmployeeId = employee.Id, SupplierId = suppliers[2].Id },
                new Order { Id = Guid.NewGuid(), EmployeeId = employee.Id, SupplierId = suppliers[0].Id } // Дубликат
            };

            await _context.Orders.AddRangeAsync(orders);
            await _context.SaveChangesAsync();

            // Устанавливаем профиль
            EmployeeProfile.Profile = employee;

            var window = new ProfileWindow();
            var contextField = typeof(ProfileWindow).GetField("_context",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            contextField.SetValue(window, _context);

            // Act
            var supplierCountMethod = typeof(ProfileWindow).GetMethod("GetMySupplierByEmployeeProfileId",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var task = (Task<int>)supplierCountMethod.Invoke(window, null);
            var supplierCount = await task;

            // Assert - должно быть 3 уникальных поставщика
            Assert.Equal(3, supplierCount);
        }

        [StaFact]
        public async Task GetMyOrderByEmployeeProfileId_NoOrders_ReturnsZero()
        {
            // Arrange
            _context = CreateInMemoryContext();

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FullName = "Сотрудник без заказов",
                Email = "noorders@warehouse.ru",
                Password = "password"
            };

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            // Устанавливаем профиль
            EmployeeProfile.Profile = employee;

            var window = new ProfileWindow();
            var contextField = typeof(ProfileWindow).GetField("_context",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            contextField.SetValue(window, _context);

            // Act
            var orderCountMethod = typeof(ProfileWindow).GetMethod("GetMyOrderByEmployeeProfileId",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var task = (Task<int>)orderCountMethod.Invoke(window, null);
            var orderCount = await task;

            // Assert
            Assert.Equal(0, orderCount);
        }

        [StaFact]
        public async Task GetMySupplierByEmployeeProfileId_NoOrders_ReturnsZero()
        {
            // Arrange
            _context = CreateInMemoryContext();

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                FullName = "Сотрудник без заказов",
                Email = "noorders@warehouse.ru",
                Password = "password"
            };

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            // Устанавливаем профиль
            EmployeeProfile.Profile = employee;

            var window = new ProfileWindow();
            var contextField = typeof(ProfileWindow).GetField("_context",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            contextField.SetValue(window, _context);

            // Act
            var supplierCountMethod = typeof(ProfileWindow).GetMethod("GetMySupplierByEmployeeProfileId",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var task = (Task<int>)supplierCountMethod.Invoke(window, null);
            var supplierCount = await task;

            // Assert
            Assert.Equal(0, supplierCount);
        }

        public void Dispose()
        {
            _context?.Dispose();
            EmployeeProfile.Profile = null;
        }
    }
}
