using Microsoft.EntityFrameworkCore;
using WarehouseMaster.Domain.Models;
using WarehouseMaster.Persistence.Data.DbContexts;
using WarehouseMaster.WPF.Common.Profiles;
using WarehouseMaster.WPF.Windows;
using WarehouseMaster.WPF.Common.ContractModels;

namespace WarehouseMaster.Tests.WinodowTests.ProfileWindowTests
{
    public class ProfileWindowUpdateTests : IDisposable
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
        public async Task ProfileUpdateAsync_ValidData_UpdatesSuccessfully()
        {
            // Arrange
            _context = CreateInMemoryContext();

            var originalEmployee = new Employee
            {
                Id = Guid.NewGuid(),
                FullName = "Иван Иванов",
                Email = "ivan@warehouse.ru",
                Password = "oldpassword"
            };

            await _context.Employees.AddAsync(originalEmployee);
            await _context.SaveChangesAsync();

            // Устанавливаем профиль
            EmployeeProfile.Profile = originalEmployee;

            var window = new ProfileWindow();
            var contextField = typeof(ProfileWindow).GetField("_context",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            contextField.SetValue(window, _context);

            var profileDto = new ProfileUpdateDto
            {
                Login = "ivan.new@warehouse.ru",
                Password = "newpassword123"
            };

            // Act
            var updateMethod = typeof(ProfileWindow).GetMethod("ProfileUpdateAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            await (Task)updateMethod.Invoke(window, new object[] { profileDto });

            // Assert
            var updatedEmployee = await _context.Employees.FindAsync(originalEmployee.Id);
            Assert.NotNull(updatedEmployee);
            Assert.Equal("ivan.new@warehouse.ru", updatedEmployee.Email);
            Assert.Equal("newpassword123", updatedEmployee.Password);
            Assert.Equal("Иван Иванов", updatedEmployee.FullName); // FullName не должен измениться
        }

        [StaFact]
        public async Task ProfileUpdateAsync_NullProfile_ShowsWarning()
        {
            // Arrange
            _context = CreateInMemoryContext();
            var window = new ProfileWindow();

            var contextField = typeof(ProfileWindow).GetField("_context",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            contextField.SetValue(window, _context);

            // Устанавливаем null профиль
            EmployeeProfile.Profile = null;

            var profileDto = new ProfileUpdateDto
            {
                Login = "test@warehouse.ru",
                Password = "password"
            };

            // Act
            var updateMethod = typeof(ProfileWindow).GetMethod("ProfileUpdateAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            await (Task)updateMethod.Invoke(window, new object[] { profileDto });

            // Assert - Проверяем что метод не падает
            var employeesCount = await _context.Employees.CountAsync();
            Assert.Equal(0, employeesCount);
        }

        [StaFact]
        public async Task ProfileUpdateAsync_NonExistentProfile_ShowsError()
        {
            // Arrange
            _context = CreateInMemoryContext();
            var window = new ProfileWindow();

            var contextField = typeof(ProfileWindow).GetField("_context",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            contextField.SetValue(window, _context);

            // Устанавливаем профиль с несуществующим ID
            EmployeeProfile.Profile = new Employee
            {
                Id = Guid.NewGuid(),
                FullName = "Несуществующий",
                Email = "nonexistent@warehouse.ru",
                Password = "password"
            };

            var profileDto = new ProfileUpdateDto
            {
                Login = "updated@warehouse.ru",
                Password = "newpassword"
            };

            // Act
            var updateMethod = typeof(ProfileWindow).GetMethod("ProfileUpdateAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            await (Task)updateMethod.Invoke(window, new object[] { profileDto });

            // Assert
            var employeesCount = await _context.Employees.CountAsync();
            Assert.Equal(0, employeesCount);
        }

        public void Dispose()
        {
            _context?.Dispose();
            EmployeeProfile.Profile = null;
        }
    }
}
