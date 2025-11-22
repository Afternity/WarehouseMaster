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
    /// Логика взаимодействия для PalletPage.xaml
    /// </summary>
    public partial class PalletPage
        : Page
    {
        private readonly WarehouseMasterDbContext _context = new WarehouseMasterDbContext();

        private ObservableCollection<Pallet> _palletsList = [];

        public PalletPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            await GetAllAsync();
        }

        private async void GetAllPallet_Click(
            object sender,
            RoutedEventArgs e)
        {
            await GetAllAsync();
        }

        private async void CreatePallet_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddNameTextBox.Text) &&
                decimal.TryParse(AddWeightTextBox.Text, out decimal weight) &&
                decimal.TryParse(AddLengthTextBox.Text, out decimal length))
                return;

            var newPallet = new Pallet()
            {
                Name = AddNameTextBox.Text,
                Barcode = AddBarcodeTextBox.Text,
                Weight = decimal.Parse(AddWeightTextBox.Text),
                Length = decimal.Parse(AddLengthTextBox.Text)
            };

            await CreateAsync(newPallet);
        }

        private async void DeletePallet_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (DataGridUI.SelectedItem is not Pallet selected)
                return;

            await DeleteAsync(selected);
        }

        private async void UpdatePallet_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (DataGridUI.SelectedItem is not Pallet selected)
                return;

            if (!string.IsNullOrWhiteSpace(WorkerNameTextBox.Text) &&
                !decimal.TryParse(WorkerWeightTextBox.Text, out decimal weight) &&
                !decimal.TryParse(WorkerLengthTextBox.Text, out decimal length))
                return;

            var entity = new Pallet()
            {
                Id = selected.Id,
                Name = WorkerNameTextBox.Text,
                Barcode = WorkerBarcodeTextBox.Text,
                Weight = decimal.Parse(WorkerWeightTextBox.Text),
                Length = decimal.Parse(WorkerLengthTextBox.Text)
            };

            await UpdateAsync(entity);
        }

        private void DataGridUI_SelectionChanged(
           object sender,
           SelectionChangedEventArgs e)
        {
            if (DataGridUI.SelectedItem is not Pallet selected)
                return;

            WorkerIdTextBox.Text = selected.Id.ToString();
            WorkerNameTextBox.Text = selected.Name;
            WorkerBarcodeTextBox.Text = selected.Barcode;
            WorkerWeightTextBox.Text = selected.Weight.ToString();
            WorkerLengthTextBox.Text = selected.Length.ToString();
        }

        public async Task CreateAsync(Pallet model)
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

                var entity = new Pallet()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Barcode = model.Barcode,
                    Weight = model.Weight,
                    Length = model.Length
                };

                await _context.Pallets.AddAsync(entity, tokenSourse.Token);
                await _context.SaveChangesAsync(tokenSourse.Token);

                await GetAllAsync();

                AddNameTextBox.Text = "";
                AddBarcodeTextBox.Text = "";
                AddWeightTextBox.Text = "";
                AddLengthTextBox.Text = "";

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

        public async Task DeleteAsync(Pallet model)
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

                var entity = await _context.Pallets
                    .FirstOrDefaultAsync(entity => 
                        entity.Id == model.Id,
                        tokenSourse.Token);

                if (entity == null)
                {
                    MessageBox.Show(MessageConst.GetByIdError);
                    return;
                }

                _context.Pallets.Remove(entity);
                await _context.SaveChangesAsync(tokenSourse.Token);

                await GetAllAsync();

                WorkerIdTextBox.Text = "";
                WorkerNameTextBox.Text = "";
                WorkerBarcodeTextBox.Text = "";
                WorkerWeightTextBox.Text = "";
                WorkerLengthTextBox.Text = "";

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

        public async Task UpdateAsync(Pallet model)
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

                var entity = await _context.Pallets
                     .FirstOrDefaultAsync(entity =>
                        entity.Id == model.Id,
                        tokenSourse.Token);

                if (entity == null)
                {
                    MessageBox.Show(MessageConst.GetByIdError);
                    return;
                }

                entity.Name = model.Name;
                entity.Barcode = model.Barcode;
                entity.Weight = model.Weight;
                entity.Length = model.Length;

                _context.Pallets.Update(entity);
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

                var entities = await _context.Pallets
                     .AsNoTracking()
                     .ToListAsync(tokenSourse.Token);

                if (entities.Count == 0)
                {
                    MessageBox.Show(MessageConst.GetAllError);
                    return;
                }

                _palletsList = new ObservableCollection<Pallet>(entities);
                DataGridUI.ItemsSource = _palletsList;

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
