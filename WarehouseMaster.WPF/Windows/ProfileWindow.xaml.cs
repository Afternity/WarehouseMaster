using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Windows;
using WarehouseMaster.Domain.Models;
using WarehouseMaster.Persistence.Data.DbContexts;
using WarehouseMaster.WPF.Common.Consts;
using WarehouseMaster.WPF.Common.ContractModels;
using WarehouseMaster.WPF.Common.Profiles;


namespace WarehouseMaster.WPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        private readonly WarehouseMasterDbContext _context;

        private ProfileUpdateDto _profileUpdateDto = new ProfileUpdateDto();

        public ProfileWindow()
        {
            InitializeComponent();
            _context = new WarehouseMasterDbContext();
        }

        private async void Window_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            FullNameTextBlock.Text = EmployeeProfile.Profile.FullName;
            EmailTextBlock.Text = EmployeeProfile.Profile.Email;
            PasswordTextBlock.Text = EmployeeProfile.Profile.Password;

            MyOrderCoutn.Text = $"{await GetMyOrderByEmployeeProfileId()}";
            MySupplierCount.Text = $"{await GetMySupplierByEmployeeProfileId()}";
        }

        private async void EmployeeUpdate_Click(
            object sender,
            RoutedEventArgs e)
        {
            var profile = new ProfileUpdateDto()
            {
                Login = EmailTextBlock.Text,
                Password = PasswordTextBlock.Text,
            };

            await ProfileUpdateAsync(profile);
        }

        private void OpenAuthWindow_Click(object sender, RoutedEventArgs e)
        {
            var window = new AuthWindow();
            window.Show();

            var windows = Application.Current.Windows.OfType<Window>().ToList();

            foreach(var item  in windows) 
                if (item is not AuthWindow)
                    item.Close();
        }

        private async Task ProfileUpdateAsync(ProfileUpdateDto profileDto)
        {
            try
            {
                using var tokenSource = new CancellationTokenSource(
                    OperationConts.OperationTimeLimit);

               var entity = await _context.Employees
                    .FirstOrDefaultAsync(employee =>
                        employee.Id == EmployeeProfile.Profile.Id,
                        tokenSource.Token);

                if (entity == null)
                {
                    MessageBox.Show(MessageConst.GetByIdError);
                    return;
                }

                entity.Email = profileDto.Login;
                entity.Password = profileDto.Password;
                _context.Update(entity);
                await _context.SaveChangesAsync(tokenSource.Token);

                EmployeeProfile.Profile = entity;

                MessageBox.Show(MessageConst.UpdateSuccessful);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show(MessageConst.OperationTimeLimit);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{MessageConst.OperationError} {ex.Message}");
            }
        }

        private async Task<int> GetMyOrderByEmployeeProfileId()
        {
            try
            {
                using var tokenSource = new CancellationTokenSource(
                    OperationConts.OperationTimeLimit);

                var orderCount = await _context.Orders
                    .AsNoTracking()
                    .Where(order =>
                        order.EmployeeId == EmployeeProfile.Profile.Id)
                    .CountAsync(tokenSource.Token);

                return orderCount;

            }
            catch (OperationCanceledException)
            {
                MessageBox.Show(MessageConst.OperationTimeLimit);
                return -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{MessageConst.OperationError} {ex.Message}");
                return -1;
            }
        }

        private async Task<int> GetMySupplierByEmployeeProfileId()
        {
            try
            {
                using var tokenSource = new CancellationTokenSource(
                    OperationConts.OperationTimeLimit);

                var orderCount = await _context.Orders
                    .AsNoTracking()
                    .Include(order => order.Supplier)
                    .Include(order => order.Employee)
                    .Where(order =>
                        order.EmployeeId == EmployeeProfile.Profile.Id)
                    .Select(order =>
                        order.SupplierId)
                    .Distinct()
                    .CountAsync(tokenSource.Token);

                return orderCount;

            }
            catch (OperationCanceledException)
            {
                MessageBox.Show(MessageConst.OperationTimeLimit);
                return -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{MessageConst.OperationError} {ex.Message}");
                return -1;
            }
        }

    }
}
