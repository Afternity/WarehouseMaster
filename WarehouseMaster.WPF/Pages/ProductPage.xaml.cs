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
    /// Логика взаимодействия для ProductPage.xaml
    /// </summary>
    public partial class ProductPage : Page
    {
        private readonly WarehouseMasterDbContext _context = new WarehouseMasterDbContext();

        private ObservableCollection<Product> _productsList = [];

        public ProductPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            await GetAllAsync();
        }

        private async void GetAllProduct_Click(
            object sender,
            RoutedEventArgs e)
        {
            await GetAllAsync();
        }

        private async void CreateProduct_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddNameTextBox.Text) ||
                !decimal.TryParse(AddPriceTextBox.Text, out decimal price))
                return;

            var entity = new Product()
            {
                Name = AddNameTextBox.Text,
                Description = AddDescriptionTextBox.Text,
                Price = decimal.Parse(AddPriceTextBox.Text),
                Type = AddTypeTextBox.Text
            };

            await CreateAsync(entity);
        }

        private async void DeleteProduct_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (DataGridUI.SelectedItem is not Product selected)
                return;

            await DeleteAsync(selected);
        }

        private async void UpdateProduct_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (DataGridUI.SelectedItem is not Product selected)
                return;

            if (string.IsNullOrWhiteSpace(WorkerNameTextBox.Text) ||
                !decimal.TryParse(WorkerPriceTextBox.Text, out decimal price))
                return;

            var entity = new Product()
            {
                Id = selected.Id,
                Name = WorkerNameTextBox.Text,
                Description = WorkerDescriptionTextBox.Text,
                Price = decimal.Parse(WorkerPriceTextBox.Text),
                Type = WorkerTypeTextBox.Text
            };

            await UpdateAsync(entity);
        }

        private void DataGridUI_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (DataGridUI.SelectedItem is not Product selected)
                return;

            WorkerIdTextBox.Text = selected.Id.ToString();
            WorkerNameTextBox.Text = selected.Name;
            WorkerDescriptionTextBox.Text = selected.Description;
            WorkerPriceTextBox.Text = selected.Price.ToString();
            WorkerTypeTextBox.Text = selected.Type;
        }

        public async Task CreateAsync(
            Product model)
        {
            try
            {
                if (model == null ||
                    string.IsNullOrWhiteSpace(model.Name))
                {
                    MessageBox.Show(MessageConst.NotEmptyAndNull);
                    return;
                }

                using var tokenSourse = new CancellationTokenSource(
                    OperationConts.OperationTimeLimit);

                var entity = new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Type = model.Type
                };

                await _context.Products.AddAsync(
                    entity, 
                    tokenSourse.Token);
                await _context.SaveChangesAsync(
                    tokenSourse.Token);

                await GetAllAsync();

                AddNameTextBox.Text = "";
                AddDescriptionTextBox.Text = "";
                AddPriceTextBox.Text = "0";
                AddTypeTextBox.Text = "";

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
            Product model)
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

                var entity = await _context.Products
                    .FirstOrDefaultAsync(entity => 
                        entity.Id == model.Id, 
                        tokenSourse.Token);

                if (entity == null)
                {
                    MessageBox.Show(MessageConst.GetByIdError);
                    return;
                }

                _context.Products.Remove(entity);
                await _context.SaveChangesAsync(tokenSourse.Token);

                await GetAllAsync();

                WorkerIdTextBox.Text = "";
                WorkerNameTextBox.Text = "";
                WorkerDescriptionTextBox.Text = "";
                WorkerPriceTextBox.Text = "";
                WorkerTypeTextBox.Text = "";

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
            Product model)
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

                var entity = await _context.Products
                     .FirstOrDefaultAsync(entity =>
                        entity.Id == model.Id,
                        tokenSourse.Token);

                if (entity == null)
                {
                    MessageBox.Show(MessageConst.GetByIdError);
                    return;
                }

                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Price = model.Price;
                entity.Type = model.Type;

                _context.Products.Update(entity);
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

                var entities = await _context.Products
                     .AsNoTracking()
                     .ToListAsync(tokenSourse.Token);

                if (entities.Count == 0)
                {
                    MessageBox.Show(MessageConst.GetAllError);
                    return;
                }

                _productsList = new ObservableCollection<Product>(entities);
                DataGridUI.ItemsSource = _productsList;
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
