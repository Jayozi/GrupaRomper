using Microsoft.Maui.Controls;
using System.Text.RegularExpressions;

namespace projekt
{
    public partial class registerPage : ContentPage
    {
        public registerPage()
        {
            InitializeComponent();
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnRegisterUPButtonClicked(object sender, EventArgs e)
        {
            ErrorLabel.Text = string.Empty;
            string name = NameEntry.Text;
            string surname = SurnameEntry.Text;
            string email = emailEntry.Text;
            string password = PasswordEntry.Text;
            string uprawnienia = PermissionEntry.Text;
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ErrorLabel.Text = "Wszystkie pola musza byc wypelnione";
                return;
            }
            if (userManager.CzyIstniejEmail(email))
            {
                ErrorLabel.Text = "Istnieje juz konto z takim adresem email!";
                return;
            }

            if (userManager.SprawdzPoprawnoscHasla(password) == false || userManager.SprawdzPoprawnoscEmail(email) == false)
            {
                ErrorLabel.Text = "Niepoprawny format emailu albo hasla";
                return;
            }

            if(uprawnienia != "adm123")
            {
                uprawnienia = "user";
            }

            User newUser = new User(name, surname, email, password, uprawnienia);

            Database.InsertUserConnection(newUser);

            userManager.AddUser(newUser);

            Task.Delay(500).Wait();

            NameEntry.Text = string.Empty;
            SurnameEntry.Text = string.Empty;
            emailEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
            PermissionEntry.Text = string.Empty;

            await Navigation.PopToRootAsync();

        }

    }
}





