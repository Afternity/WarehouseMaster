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
    /// Логика взаимодействия для LocationPage.xaml
    /// </summary>
    public partial class LocationPage 
        : Page
    {
        private readonly WarehouseMasterDbContext _context = new WarehouseMasterDbContext();

        private ObservableCollection<Location> _locationsList = [];

        public LocationPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            await GetAllAsync();
        }

        private async void GetAllLocation_Click(
            object sender,
            RoutedEventArgs e)
        {
            await GetAllAsync();
        }

        private async void CreateLocation_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(AddCodeTextBox.Text) ||
                !decimal.TryParse(AddWidthTextBox.Text, out decimal width) ||
                !decimal.TryParse(AddLengthTextBox.Text, out decimal length) ||
                !decimal.TryParse(AddHeightTextBox.Text, out decimal height))
                return;

            var entity = new Location()
            {
                Name = AddNameTextBox.Text,
                Code = AddCodeTextBox.Text,
                Width = decimal.Parse(AddWidthTextBox.Text),
                Length = decimal.Parse(AddLengthTextBox.Text),
                Height = decimal.Parse(AddHeightTextBox.Text)
            };

            await CreateAsync(entity);
        }

        private async void DeleteLocation_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (DataGridUI.SelectedItem is not Location selected)
                return;

            await DeleteAsync(selected);
        }

        private async void UpdateLocation_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (DataGridUI.SelectedItem is not Location selected)
                return;

            if (string.IsNullOrWhiteSpace(WorkerNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(WorkerCodeTextBox.Text) ||
                !decimal.TryParse(WorkerWidthTextBox.Text, out decimal width) ||
                !decimal.TryParse(WorkerLengthTextBox.Text, out decimal length) ||
                !decimal.TryParse(WorkerHeightTextBox.Text, out decimal height))
                return;

            var entity = new Location()
            {
                Id = selected.Id,
                Name = WorkerNameTextBox.Text,
                Code = WorkerCodeTextBox.Text,
                Width = decimal.Parse(WorkerWidthTextBox.Text),
                Length = decimal.Parse(WorkerLengthTextBox.Text),
                Height = decimal.Parse(WorkerHeightTextBox.Text)
            };

            await UpdateAsync(entity);
        }

        private void DataGridUI_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (DataGridUI.SelectedItem is not Location selected)
                return;

            WorkerIdTextBox.Text = selected.Id.ToString();
            WorkerNameTextBox.Text = selected.Name;
            WorkerCodeTextBox.Text = selected.Code;
            WorkerWidthTextBox.Text = selected.Width.ToString();
            WorkerLengthTextBox.Text = selected.Length.ToString();
            WorkerHeightTextBox.Text = selected.Height.ToString();
        }

        public async Task CreateAsync(
            Location model)
        {
            try
            {
                if (model == null ||
                    string.IsNullOrWhiteSpace(model.Name) ||
                    string.IsNullOrWhiteSpace(model.Code))
                {
                    MessageBox.Show(MessageConst.NotEmptyAndNull);
                    return;
                }

                using var tokenSourse = new CancellationTokenSource(
                    OperationConts.OperationTimeLimit);

                var entity = new Location()
                {
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Code = model.Code,
                    Width = model.Width,
                    Length = model.Length,
                    Height = model.Height
                };

                await _context.Locations.AddAsync(
                    entity,
                    tokenSourse.Token);
                await _context.SaveChangesAsync(
                    tokenSourse.Token);

                await GetAllAsync();

                AddNameTextBox.Text = "";
                AddCodeTextBox.Text = "";
                AddWidthTextBox.Text = "0";
                AddLengthTextBox.Text = "0";
                AddHeightTextBox.Text = "0";

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
            Location model)
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

                var entity = await _context.Locations
                    .FirstOrDefaultAsync(entity =>
                        entity.Id == model.Id,
                        tokenSourse.Token);

                if (entity == null)
                {
                    MessageBox.Show(MessageConst.GetByIdError);
                    return;
                }

                _context.Locations.Remove(entity);
                await _context.SaveChangesAsync(tokenSourse.Token);

                await GetAllAsync();

                WorkerIdTextBox.Text = "";
                WorkerNameTextBox.Text = "";
                WorkerCodeTextBox.Text = "";
                WorkerWidthTextBox.Text = "";
                WorkerLengthTextBox.Text = "";
                WorkerHeightTextBox.Text = "";

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
            Location model)
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

                var entity = await _context.Locations
                     .FirstOrDefaultAsync(entity =>
                        entity.Id == model.Id,
                        tokenSourse.Token);

                if (entity == null)
                {
                    MessageBox.Show(MessageConst.GetByIdError);
                    return;
                }

                entity.Name = model.Name;
                entity.Code = model.Code;
                entity.Width = model.Width;
                entity.Length = model.Length;
                entity.Height = model.Height;

                _context.Locations.Update(entity);
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

                var entities = await _context.Locations
                     .AsNoTracking()
                     .ToListAsync(tokenSourse.Token);

                if (entities.Count == 0)
                {
                    MessageBox.Show(MessageConst.GetAllError);
                    return;
                }

                _locationsList = new ObservableCollection<Location>(entities);
                DataGridUI.ItemsSource = _locationsList;
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
