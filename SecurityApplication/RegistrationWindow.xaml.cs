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
using System.Windows.Shapes;
using SecurityApplication.DataAccess;
using SecurityApplication.Models;
using SecurityApplication.Services;

namespace SecurityApplication
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private async void RegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox.Text;

            string password = passwordTextBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            registrationButton.IsEnabled = false;

            using (var context = new DataAccess.AppContext())
            {
                var user = await GetUserFromLogin(context, login);
                if (user == null)
                {
                    await CreateUser(context, new User
                    {
                        Login = login,
                        Password = SecurityHasher.HashPassword(password)
                    });
                    await context.SaveChangesAsync();

                    MessageBox.Show("Учетная запись успешно создана!");
                    Close();
                }
                else
                {
                    MessageBox.Show("Логин уже занят!");
                }
            }

            registrationButton.IsEnabled = true;
        }

        private Task CreateUser(DataAccess.AppContext context, User user)
        {
            return Task.Run(() =>
            {
                context.Users.Add(user);
            });
        }

        private Task<User> GetUserFromLogin(DataAccess.AppContext context, string login)
        {
            return Task.Run(() =>
            {
                return context.Users.SingleOrDefault(seatchingUser => seatchingUser.Login == login);
            });
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
