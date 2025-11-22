using Microsoft.EntityFrameworkCore;
using WarehouseMaster.Domain.Models;
using WarehouseMaster.Persistence.Data.DbContexts;
using WarehouseMaster.WPF.Common.ContractModels;
using WarehouseMaster.WPF.Common.Profiles;
using WarehouseMaster.WPF.Windows;

namespace WarehouseMaster.Tests.WinodowTests.AuthWidndowTests
{
    public class AuthWindowAuthTests 
        : IDisposable
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
        public async Task AuthAsync_ValidCredentials_AuthSuccessfully()
        {
            // Arrange
            _context = CreateInMemoryContext();

            var testEmployee = new Employee
            {
                Id = Guid.NewGuid(),
                FullName = "Тестовый сотрудник",
                Email = "test@warehouse.ru",
                Password = "password123"
            };

            await _context.Employees.AddAsync(testEmployee);
            await _context.SaveChangesAsync();

            var window = new AuthWindow();
            var contextField = typeof(AuthWindow).GetField("_context",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            contextField.SetValue(window, _context);

            var authDto = new AuthDto
            {
                Login = "test@warehouse.ru",
                Password = "password123"
            };

            // Act
            var authMethod = typeof(AuthWindow).GetMethod("AuthAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            await (Task)authMethod.Invoke(window, new object[] { authDto });

            // Assert
            Assert.NotNull(EmployeeProfile.Profile);
            Assert.Equal("test@warehouse.ru", EmployeeProfile.Profile.Email);
            Assert.Equal("Тестовый сотрудник", EmployeeProfile.Profile.FullName);
        }

        [StaFact]
        public async Task AuthAsync_InvalidCredentials_ProfileRemainsNull()
        {
            // Arrange
            _context = CreateInMemoryContext();

            var testEmployee = new Employee
            {
                Id = Guid.NewGuid(),
                Email = "test@warehouse.ru",
                Password = "password123"
            };

            await _context.Employees.AddAsync(testEmployee);
            await _context.SaveChangesAsync();

            var window = new AuthWindow();
            var contextField = typeof(AuthWindow).GetField("_context",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            contextField.SetValue(window, _context);

            var authDto = new AuthDto
            {
                Login = "test@warehouse.ru",
                Password = "wrongpassword"
            };

            // Сохраняем предыдущий профиль (если есть) и очищаем
            var previousProfile = EmployeeProfile.Profile;
            EmployeeProfile.Profile = null;

            // Act
            var authMethod = typeof(AuthWindow).GetMethod("AuthAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            await (Task)authMethod.Invoke(window, new object[] { authDto });

            // Assert
            Assert.Null(EmployeeProfile.Profile);

            // Восстанавливаем предыдущий профиль
            EmployeeProfile.Profile = previousProfile;
        }

        [StaFact]
        public async Task AuthAsync_EmptyCredentials_ShowsWarning()
        {
            // Arrange
            _context = CreateInMemoryContext();
            var window = new AuthWindow();

            var contextField = typeof(AuthWindow).GetField("_context",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            contextField.SetValue(window, _context);

            var authDto = new AuthDto
            {
                Login = "",
                Password = ""
            };

            // Сохраняем предыдущий профиль
            var previousProfile = EmployeeProfile.Profile;
            EmployeeProfile.Profile = null;

            // Act
            var authMethod = typeof(AuthWindow).GetMethod("AuthAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            await (Task)authMethod.Invoke(window, new object[] { authDto });

            // Assert
            Assert.Null(EmployeeProfile.Profile);

            // Восстанавливаем предыдущий профиль
            EmployeeProfile.Profile = previousProfile;
        }

        [StaFact]
        public async Task AuthAsync_NonExistentUser_ShowsError()
        {
            // Arrange
            _context = CreateInMemoryContext();
            var window = new AuthWindow();

            var contextField = typeof(AuthWindow).GetField("_context",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            contextField.SetValue(window, _context);

            var authDto = new AuthDto
            {
                Login = "nonexistent@warehouse.ru",
                Password = "password"
            };

            // Сохраняем предыдущий профиль
            var previousProfile = EmployeeProfile.Profile;
            EmployeeProfile.Profile = null;

            // Act
            var authMethod = typeof(AuthWindow).GetMethod("AuthAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            await (Task)authMethod.Invoke(window, new object[] { authDto });

            // Assert
            Assert.Null(EmployeeProfile.Profile);

            // Восстанавливаем предыдущий профиль
            EmployeeProfile.Profile = previousProfile;
        }

        public void Dispose()
        {
            _context?.Dispose();
            // Очищаем профиль после тестов
            EmployeeProfile.Profile = null;
        }
    }
}
