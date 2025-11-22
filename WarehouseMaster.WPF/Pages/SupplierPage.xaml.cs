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
    /// Логика взаимодействия для SupplierPage.xaml
    /// </summary>
    public partial class SupplierPage 
        : Page
    {
        private readonly WarehouseMasterDbContext _context = new WarehouseMasterDbContext();

        private ObservableCollection<Supplier> _suppliersList = [];

        public SupplierPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            await GetAllAsync();
        }

        private async void GetAllSupplier_Click(
            object sender,
            RoutedEventArgs e)
        {
            await GetAllAsync();
        }

        private async void CreateSupplier_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddCompanyNameTextBox.Text))
                return;

            var entity = new Supplier()
            {
                CompanyName = AddCompanyNameTextBox.Text,
                ContactPerson = AddContactPersonTextBox.Text,
                Phone = AddPhoneTextBox.Text,
                Email = AddEmailTextBox.Text,
                Address = AddAddressTextBox.Text
            };

            await CreateAsync(entity);
        }

        private async void DeleteSupplier_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (DataGridUI.SelectedItem is not Supplier selected)
                return;

            await DeleteAsync(selected);
        }

        private async void UpdateSupplier_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (DataGridUI.SelectedItem is not Supplier selected)
                return;

            if (string.IsNullOrWhiteSpace(WorkerCompanyNameTextBox.Text))
                return;

            var entity = new Supplier()
            {
                Id = selected.Id,
                CompanyName = WorkerCompanyNameTextBox.Text,
                ContactPerson = WorkerContactPersonTextBox.Text,
                Phone = WorkerPhoneTextBox.Text,
                Email = WorkerEmailTextBox.Text,
                Address = WorkerAddressTextBox.Text
            };

            await UpdateAsync(entity);
        }

        private void DataGridUI_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (DataGridUI.SelectedItem is not Supplier selected)
                return;

            WorkerIdTextBox.Text = selected.Id.ToString();
            WorkerCompanyNameTextBox.Text = selected.CompanyName;
            WorkerContactPersonTextBox.Text = selected.ContactPerson;
            WorkerPhoneTextBox.Text = selected.Phone;
            WorkerEmailTextBox.Text = selected.Email;
            WorkerAddressTextBox.Text = selected.Address;
        }

        public async Task CreateAsync(
            Supplier model)
        {
            try
            {
                if (model == null ||
                    string.IsNullOrWhiteSpace(model.CompanyName))
                {
                    MessageBox.Show(MessageConst.NotEmptyAndNull);
                    return;
                }

                using var tokenSourse = new CancellationTokenSource(
                    OperationConts.OperationTimeLimit);

                var entity = new Supplier()
                {
                    Id = Guid.NewGuid(),
                    CompanyName = model.CompanyName,
                    ContactPerson = model.ContactPerson,
                    Phone = model.Phone,
                    Email = model.Email,
                    Address = model.Address
                };

                await _context.Suppliers.AddAsync(
                    entity,
                    tokenSourse.Token);
                await _context.SaveChangesAsync(
                    tokenSourse.Token);

                await GetAllAsync();

                AddCompanyNameTextBox.Text = "";
                AddContactPersonTextBox.Text = "";
                AddPhoneTextBox.Text = "";
                AddEmailTextBox.Text = "";
                AddAddressTextBox.Text = "";

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
            Supplier model)
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

                var entity = await _context.Suppliers
                    .FirstOrDefaultAsync(entity =>
                        entity.Id == model.Id,
                        tokenSourse.Token);

                if (entity == null)
                {
                    MessageBox.Show(MessageConst.GetByIdError);
                    return;
                }

                _context.Suppliers.Remove(entity);
                await _context.SaveChangesAsync(tokenSourse.Token);

                await GetAllAsync();

                WorkerIdTextBox.Text = "";
                WorkerCompanyNameTextBox.Text = "";
                WorkerContactPersonTextBox.Text = "";
                WorkerPhoneTextBox.Text = "";
                WorkerEmailTextBox.Text = "";
                WorkerAddressTextBox.Text = "";

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
            Supplier model)
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

                var entity = await _context.Suppliers
                     .FirstOrDefaultAsync(entity =>
                        entity.Id == model.Id,
                        tokenSourse.Token);

                if (entity == null)
                {
                    MessageBox.Show(MessageConst.GetByIdError);
                    return;
                }

                entity.CompanyName = model.CompanyName;
                entity.ContactPerson = model.ContactPerson;
                entity.Phone = model.Phone;
                entity.Email = model.Email;
                entity.Address = model.Address;

                _context.Suppliers.Update(entity);
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

                var entities = await _context.Suppliers
                     .AsNoTracking()
                     .ToListAsync(tokenSourse.Token);

                if (entities.Count == 0)
                {
                    MessageBox.Show(MessageConst.GetAllError);
                    return;
                }

                _suppliersList = new ObservableCollection<Supplier>(entities);
                DataGridUI.ItemsSource = _suppliersList;
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
