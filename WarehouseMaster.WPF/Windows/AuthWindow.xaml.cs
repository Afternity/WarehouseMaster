using System.Windows;
using WarehouseMaster.Persistence.Data.DbContexts;
using WarehouseMaster.WPF.Common.ContractModels;
using WarehouseMaster.WPF.Common.Consts;
using Microsoft.EntityFrameworkCore;
using WarehouseMaster.WPF.Common.Profiles;

namespace WarehouseMaster.WPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        private readonly WarehouseMasterDbContext _context;

        private AuthDto _authDto = new AuthDto();

        public AuthWindow()
        {
            _context = new WarehouseMasterDbContext();

            InitializeComponent();

            LoginTextBlock.Text = "alex.vasiliev@warehouse.ru";
            PassworkTextBlock.Text = "password123";
        }

        private async void Auth_Click(
            object sender,
            RoutedEventArgs e)
        {
            _authDto = new AuthDto()
            {
                Login = LoginTextBlock.Text,
                Password = PassworkTextBlock.Text
            };

            await AuthAsync(_authDto);
        }

        private async Task AuthAsync(
            AuthDto authDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(authDto.Login) ||
                    string.IsNullOrWhiteSpace(authDto.Password))
                {
                    MessageBox.Show(MessageConst.NotEmptyAndNull);
                    return;
                }

                var tokenSource = new CancellationTokenSource(
                    OperationConts.OperationTimeLimit);

                var profile = await _context.Employees
                    .FirstOrDefaultAsync(profile =>
                        profile.Password == authDto.Password
                        && profile.Email == authDto.Login,
                        tokenSource.Token);

                if (profile == null)
                {
                    MessageBox.Show(MessageConst.GetByIdError);
                    return;
                }

                EmployeeProfile.Profile = profile;

                MessageBox.Show(MessageConst.GetByIdSuccessful);

                await Task.Delay(1000);

                MainWindow();
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

        private void MainWindow()
        {
            var window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
