using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WarehouseMaster.Domain.Models;
using WarehouseMaster.Persistence.Data.DbContexts;
using WarehouseMaster.WPF.Common.Consts;

namespace WarehouseMaster.WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage 
        : Page
    {
        private readonly WarehouseMasterDbContext _context = new WarehouseMasterDbContext();

        private ObservableCollection<Employee> _employeesList = [];

        public EmployeePage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            await GetAllAsync();
        }

        private async void GetAllEmployee_Click(
            object sender,
            RoutedEventArgs e)
        {
            await GetAllAsync();
        }

        private async void CreateEmployee_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddFullNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(AddEmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(AddPasswordTextBox.Text))
                return;

            var entity = new Employee()
            {
                FullName = AddFullNameTextBox.Text,
                Email = AddEmailTextBox.Text,
                Password = AddPasswordTextBox.Text
            };

            await CreateAsync(entity);
        }

        private async void DeleteEmployee_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (DataGridUI.SelectedItem is not Employee selected)
                return;

            await DeleteAsync(selected);
        }

        private async void UpdateEmployee_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (DataGridUI.SelectedItem is not Employee selected)
                return;

            if (string.IsNullOrWhiteSpace(WorkerFullNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(WorkerEmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(WorkerPasswordTextBox.Text))
                return;

            var entity = new Employee()
            {
                Id = selected.Id,
                FullName = WorkerFullNameTextBox.Text,
                Email = WorkerEmailTextBox.Text,
                Password = WorkerPasswordTextBox.Text
            };

            await UpdateAsync(entity);
        }

        private void DataGridUI_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (DataGridUI.SelectedItem is not Employee selected)
                return;

            WorkerIdTextBox.Text = selected.Id.ToString();
            WorkerFullNameTextBox.Text = selected.FullName;
            WorkerEmailTextBox.Text = selected.Email;
            WorkerPasswordTextBox.Text = selected.Password;
        }

        public async Task CreateAsync(
            Employee model)
        {
            try
            {
                if (model == null ||
                    string.IsNullOrWhiteSpace(model.FullName) ||
                    string.IsNullOrWhiteSpace(model.Email) ||
                    string.IsNullOrWhiteSpace(model.Password))
                {
                    MessageBox.Show(MessageConst.NotEmptyAndNull);
                    return;
                }

                using var tokenSourse = new CancellationTokenSource(
                    OperationConts.OperationTimeLimit);

                var entity = new Employee()
                {
                    Id = Guid.NewGuid(),
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password
                };

                await _context.Employees.AddAsync(
                    entity,
                    tokenSourse.Token);
                await _context.SaveChangesAsync(
                    tokenSourse.Token);

                await GetAllAsync();

                AddFullNameTextBox.Text = "";
                AddEmailTextBox.Text = "";
                AddPasswordTextBox.Text = "";

                MessageBox.Show(MessageConst.OperationSuccessful);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show(MessageConst.OperationTimeLimit);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{MessageConst.OperationError}{ex.Message}");
            }
        }

        public async Task DeleteAsync(
            Employee model)
        {
            try
            {
                if (model == null ||
                    model.Id == Guid.Empty)
                {
                    MessageBox.Show(MessageConst.NotEmptyAndNull);
                    return;
                }

                using var tokenSourse = new CancellationTokenSource(
                    OperationConts.OperationTimeLimit);

                var entity = await _context.Employees
                    .FirstOrDefaultAsync(entity =>
                        entity.Id == model.Id,
                        tokenSourse.Token);

                if (entity == null)
                {
                    MessageBox.Show(MessageConst.GetByIdError);
                    return;
                }

                _context.Employees.Remove(entity);
                await _context.SaveChangesAsync(tokenSourse.Token);

                await GetAllAsync();

                WorkerIdTextBox.Text = "";
                WorkerFullNameTextBox.Text = "";
                WorkerEmailTextBox.Text = "";
                WorkerPasswordTextBox.Text = "";

                MessageBox.Show(MessageConst.DeleteSuccessful);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show(MessageConst.OperationTimeLimit);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{MessageConst.OperationError}{ex.Message}");
            }
        }

        public async Task UpdateAsync(
            Employee model)
        {
            try
            {
                if (model == null ||
                    model.Id == Guid.Empty)
                {
                    MessageBox.Show(MessageConst.NotEmptyAndNull);
                    return;
                }

                using var tokenSourse = new CancellationTokenSource(
                    OperationConts.OperationTimeLimit);

                var entity = await _context.Employees
                     .FirstOrDefaultAsync(entity =>
                        entity.Id == model.Id,
                        tokenSourse.Token);

                if (entity == null)
                {
                    MessageBox.Show(MessageConst.GetByIdError);
                    return;
                }

                entity.FullName = model.FullName;
                entity.Email = model.Email;
                entity.Password = model.Password;

                _context.Employees.Update(entity);
                await _context.SaveChangesAsync(tokenSourse.Token);

                await GetAllAsync();

                MessageBox.Show(MessageConst.UpdateSuccessful);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show(MessageConst.OperationTimeLimit);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{MessageConst.OperationError}{ex.Message}");
            }
        }

        public async Task GetAllAsync()
        {
            try
            {
                using var tokenSourse = new CancellationTokenSource(
                    OperationConts.OperationTimeLimit);

                var entities = await _context.Employees
                     .AsNoTracking()
                     .ToListAsync(tokenSourse.Token);

                if (entities.Count == 0)
                {
                    MessageBox.Show(MessageConst.GetAllError);
                    return;
                }

                _employeesList = new ObservableCollection<Employee>(entities);
                DataGridUI.ItemsSource = _employeesList;
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show(MessageConst.OperationTimeLimit);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{MessageConst.OperationError}{ex.Message}");
            }
        }
    }
}
