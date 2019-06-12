using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SecurityApplication.DataAccess;
using SecurityApplication.Models;
using SecurityApplication.Services;

namespace SecurityApplication
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void SignInButtonClick(object sender, RoutedEventArgs e)
        {
            var login = loginTextBox.Text;

            var password = passwordBox.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            signInButton.IsEnabled = false;
            registrationButton.IsEnabled = false;

            using (var context = new DataAccess.AppContext())
            {
                var user = await GetUserFromLogin(context, login);
                if (user == null || !SecurityHasher.VerifyPassword(password, user.Password))
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
                else
                {
                    new CabinetWindow().Show();
                    Close();
                }
            }

            signInButton.IsEnabled = true;
            registrationButton.IsEnabled = true;
        }

        private Task<User> GetUserFromLogin(DataAccess.AppContext context, string login)
        {
            return Task.Run(() =>
            {
                return context.Users.SingleOrDefault(seatchingUser => seatchingUser.Login == login);
            });
        }


        private void RegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();

            registrationWindow.Show();
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
