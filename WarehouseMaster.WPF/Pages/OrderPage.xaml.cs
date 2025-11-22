using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WarehouseMaster.Domain.Models;
using WarehouseMaster.Domain.Models.BaseModels;
using WarehouseMaster.Persistence.Data.DbContexts;
using WarehouseMaster.WPF.Common.Consts;

namespace WarehouseMaster.WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderPage.xaml
    /// </summary>
    public partial class OrderPage 
        : Page
    {
        private readonly WarehouseMasterDbContext _context = new WarehouseMasterDbContext();

        private ObservableCollection<Order> _ordersList = [];
        private ObservableCollection<Product> _productsList = [];
        private ObservableCollection<Employee> _employeesList = [];
        private ObservableCollection<Location> _locationsList = [];
        private ObservableCollection<Supplier> _suppliersList = [];
        private ObservableCollection<Pallet> _palletsList = [];

        private string _productName = string.Empty;
        private string _supplierCompany = string.Empty;
        private string _employeeName = string.Empty;

        public OrderPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            await LoadComboBoxDataAsync();
            await GetAllAsync();
        }

        private async void GetAllOrder_Click(
            object sender,
            RoutedEventArgs e)
        {
            await GetAllAsync();
        }

        private async void CreateOrder_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (!int.TryParse(AddProductCountTextBox.Text, out int productCount) ||
                !int.TryParse(AddPalletCountTextBox.Text, out int palletCount) ||
                !decimal.TryParse(AddTotalPriceTextBox.Text, out decimal totalPrice) ||
                AddProductComboBox.SelectedItem is not Product selectedProduct ||
                AddEmployeeComboBox.SelectedItem is not Employee selectedEmployee ||
                AddLocationComboBox.SelectedItem is not Location selectedLocation ||
                AddSupplierComboBox.SelectedItem is not Supplier selectedSupplier ||
                AddPalletComboBox.SelectedItem is not Pallet selectedPallet)
                return;

            var entity = new Order()
            {
                DateTime = DateTime.UtcNow,
                ProductCount = productCount,
                PalletCount = palletCount,
                TotalPrice = totalPrice,
                ProductId = selectedProduct.Id,
                EmployeeId = selectedEmployee.Id,
                LocationId = selectedLocation.Id,
                SupplierId = selectedSupplier.Id,
                PalletId = selectedPallet.Id
            };

            await CreateAsync(entity);
        }

        private async void DeleteOrder_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (DataGridUI.SelectedItem is not Order selected)
                return;

            await DeleteAsync(selected);
        }

        private async void UpdateOrder_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (DataGridUI.SelectedItem is not Order selected)
                return;

            if (!int.TryParse(WorkerProductCountTextBox.Text, out int productCount) ||
                !int.TryParse(WorkerPalletCountTextBox.Text, out int palletCount) ||
                !decimal.TryParse(WorkerTotalPriceTextBox.Text, out decimal totalPrice) ||
                WorkerProductComboBox.SelectedItem is not Product selectedProduct ||
                WorkerEmployeeComboBox.SelectedItem is not Employee selectedEmployee ||
                WorkerLocationComboBox.SelectedItem is not Location selectedLocation ||
                WorkerSupplierComboBox.SelectedItem is not Supplier selectedSupplier ||
                WorkerPalletComboBox.SelectedItem is not Pallet selectedPallet)
                return;

            var entity = new Order()
            {
                Id = selected.Id,
                DateTime = selected.DateTime,
                ProductCount = productCount,
                PalletCount = palletCount,
                TotalPrice = totalPrice,
                ProductId = selectedProduct.Id,
                EmployeeId = selectedEmployee.Id,
                LocationId = selectedLocation.Id,
                SupplierId = selectedSupplier.Id,
                PalletId = selectedPallet.Id
            };

            await UpdateAsync(entity);
        }

        private void DataGridUI_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (DataGridUI.SelectedItem is not Order selected)
                return;

            WorkerIdTextBox.Text = selected.Id.ToString();
            WorkerDateTimeTextBox.Text = selected.DateTime.ToString("dd.MM.yyyy HH:mm");
            WorkerProductCountTextBox.Text = selected.ProductCount.ToString();
            WorkerPalletCountTextBox.Text = selected.PalletCount.ToString();
            WorkerTotalPriceTextBox.Text = selected.TotalPrice.ToString("F2");

            // Установка выбранных элементов в ComboBox
            SetComboBoxSelection(WorkerProductComboBox, selected.ProductId);
            SetComboBoxSelection(WorkerEmployeeComboBox, selected.EmployeeId);
            SetComboBoxSelection(WorkerLocationComboBox, selected.LocationId);
            SetComboBoxSelection(WorkerSupplierComboBox, selected.SupplierId);
            SetComboBoxSelection(WorkerPalletComboBox, selected.PalletId);
        }

        private void SetComboBoxSelection(ComboBox comboBox, Guid selectedId)
        {
            foreach (var item in comboBox.Items)
            {
                if (item is BaseModel entity && entity.Id == selectedId)
                {
                    comboBox.SelectedItem = item;
                    return;
                }
            }
            comboBox.SelectedIndex = -1;
        }

        private async Task LoadComboBoxDataAsync()
        {
            try
            {
                using var tokenSourse = new CancellationTokenSource(OperationConts.OperationTimeLimit);

                // Загрузка данных для ComboBox
                var products = await _context.Products.AsNoTracking().ToListAsync(tokenSourse.Token);
                var employees = await _context.Employees.AsNoTracking().ToListAsync(tokenSourse.Token);
                var locations = await _context.Locations.AsNoTracking().ToListAsync(tokenSourse.Token);
                var suppliers = await _context.Suppliers.AsNoTracking().ToListAsync(tokenSourse.Token);
                var pallets = await _context.Pallets.AsNoTracking().ToListAsync(tokenSourse.Token);

                _productsList = new ObservableCollection<Product>(products);
                _employeesList = new ObservableCollection<Employee>(employees);
                _locationsList = new ObservableCollection<Location>(locations);
                _suppliersList = new ObservableCollection<Supplier>(suppliers);
                _palletsList = new ObservableCollection<Pallet>(pallets);

                // Привязка данных к ComboBox для добавления
                AddProductComboBox.ItemsSource = _productsList;
                AddEmployeeComboBox.ItemsSource = _employeesList;
                AddLocationComboBox.ItemsSource = _locationsList;
                AddSupplierComboBox.ItemsSource = _suppliersList;
                AddPalletComboBox.ItemsSource = _palletsList;

                // Привязка данных к ComboBox для редактирования
                WorkerProductComboBox.ItemsSource = _productsList;
                WorkerEmployeeComboBox.ItemsSource = _employeesList;
                WorkerLocationComboBox.ItemsSource = _locationsList;
                WorkerSupplierComboBox.ItemsSource = _suppliersList;
                WorkerPalletComboBox.ItemsSource = _palletsList;
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

        public async Task CreateAsync(
            Order model)
        {
            try
            {
                if (model == null)
                {
                    MessageBox.Show(MessageConst.NotEmptyAndNull);
                    return;
                }

                using var tokenSourse = new CancellationTokenSource();

                var entity = new Order()
                {
                    Id = Guid.NewGuid(),
                    DateTime = model.DateTime,
                    ProductCount = model.ProductCount,
                    PalletCount = model.PalletCount,
                    TotalPrice = model.TotalPrice,
                    ProductId = model.ProductId,
                    EmployeeId = model.EmployeeId,
                    LocationId = model.LocationId,
                    SupplierId = model.SupplierId,
                    PalletId = model.PalletId
                };

                await _context.Orders.AddAsync(entity, tokenSourse.Token);
                await _context.SaveChangesAsync(tokenSourse.Token);

                await GetAllAsync();

                AddProductCountTextBox.Text = "0";
                AddPalletCountTextBox.Text = "0";
                AddTotalPriceTextBox.Text = "0";
                AddProductComboBox.SelectedIndex = -1;
                AddEmployeeComboBox.SelectedIndex = -1;
                AddLocationComboBox.SelectedIndex = -1;
                AddSupplierComboBox.SelectedIndex = -1;
                AddPalletComboBox.SelectedIndex = -1;

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
            Order model)
        {
            try
            {
                if (model == null || model.Id == Guid.Empty)
                {
                    MessageBox.Show(MessageConst.NotEmptyAndNull);
                    return;
                }

                using var tokenSourse = new CancellationTokenSource(OperationConts.OperationTimeLimit);

                var entity = await _context.Orders
                    .FirstOrDefaultAsync(entity => entity.Id == model.Id, tokenSourse.Token);

                if (entity == null)
                {
                    MessageBox.Show(MessageConst.GetByIdError);
                    return;
                }

                _context.Orders.Remove(entity);
                await _context.SaveChangesAsync(tokenSourse.Token);

                await GetAllAsync();

                WorkerIdTextBox.Text = "";
                WorkerDateTimeTextBox.Text = "";
                WorkerProductCountTextBox.Text = "";
                WorkerPalletCountTextBox.Text = "";
                WorkerTotalPriceTextBox.Text = "";
                WorkerProductComboBox.SelectedIndex = -1;
                WorkerEmployeeComboBox.SelectedIndex = -1;
                WorkerLocationComboBox.SelectedIndex = -1;
                WorkerSupplierComboBox.SelectedIndex = -1;
                WorkerPalletComboBox.SelectedIndex = -1;

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
            Order model)
        {
            try
            {
                if (model == null || model.Id == Guid.Empty)
                {
                    MessageBox.Show(MessageConst.NotEmptyAndNull);
                    return;
                }

                using var tokenSourse = new CancellationTokenSource(OperationConts.OperationTimeLimit);

                var entity = await _context.Orders
                     .FirstOrDefaultAsync(entity => entity.Id == model.Id, tokenSourse.Token);

                if (entity == null)
                {
                    MessageBox.Show(MessageConst.GetByIdError);
                    return;
                }

                entity.DateTime = model.DateTime;
                entity.ProductCount = model.ProductCount;
                entity.PalletCount = model.PalletCount;
                entity.TotalPrice = model.TotalPrice;
                entity.ProductId = model.ProductId;
                entity.EmployeeId = model.EmployeeId;
                entity.LocationId = model.LocationId;
                entity.SupplierId = model.SupplierId;
                entity.PalletId = model.PalletId;

                _context.Orders.Update(entity);
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
                using var tokenSourse = new CancellationTokenSource(OperationConts.OperationTimeLimit);

                var entities = await _context.Orders
                     .Include(o => o.Product)
                     .Include(o => o.Employee)
                     .Include(o => o.Location)
                     .Include(o => o.Supplier)
                     .Include(o => o.Pallet)
                     .AsNoTracking()
                     .ToListAsync(tokenSourse.Token);

                if (entities.Count == 0)
                {
                    MessageBox.Show(MessageConst.GetAllError);
                    return;
                }

                _ordersList = new ObservableCollection<Order>(entities);
                DataGridUI.ItemsSource = _ordersList;
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

        private async void Search_Click(
            object sender, 
            RoutedEventArgs e)
        {
            _productName = productNameTextBox.Text;
            _supplierCompany = supplierCompanyTextBox.Text;
            _employeeName = employeeNameTextBox.Text;

            await SearchAsync(_productName, _supplierCompany, _employeeName);
        }

        public async Task SearchAsync(
            string productName,
            string supplierCompany,
            string employeeName)
        {
            try
            {
                using var tokenSourse = new CancellationTokenSource(OperationConts.OperationTimeLimit);

                var entities = await _context.Orders
                     .AsNoTracking()
                     .Include(o => o.Product)
                     .Include(o => o.Employee)
                     .Include(o => o.Location)
                     .Include(o => o.Supplier)
                     .Include(o => o.Pallet)
                     .Where(order =>
                         (string.IsNullOrWhiteSpace(productName) || order.Product.Name.Contains(productName))
                         && (string.IsNullOrWhiteSpace(supplierCompany) || order.Supplier.CompanyName.Contains(supplierCompany))
                         && (string.IsNullOrWhiteSpace(employeeName) || order.Employee.FullName.Contains(employeeName)))
                     .ToListAsync(tokenSourse.Token);

                if (entities.Count == 0)
                {
                    MessageBox.Show("Заказы по заданным фильтрам не найдены");
                    return;
                }

                _ordersList = new ObservableCollection<Order>(entities);
                DataGridUI.ItemsSource = _ordersList;
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
