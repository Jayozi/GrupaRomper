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
        private async void OnPasswordInfoClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Wymagania hasła",
                "Hasło musi mieć 8-20 znaków, zawierać co najmniej jedną dużą literę, małą literę, cyfrę oraz znak specjalny.",
                "OK");
        }

        private async void OnRegisterUPButtonClicked(object sender, EventArgs e)
        {
            try
            {
                ErrorLabel.Text = string.Empty;

                string name = NameEntry.Text?.Trim();
                string surname = SurnameEntry.Text?.Trim();
                string email = emailEntry.Text?.Trim();
                string password = PasswordEntry.Text;
                string uprawnienia = PermissionEntry.Text?.Trim();

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) ||
                    string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    ErrorLabel.Text = "Wszystkie pola muszą być wypełnione";
                    return;
                }

                if (name.Length < 3 || name.Length > 20)
                {
                    ErrorLabel.Text = "Imię musi mieć od 3 do 20 znaków";
                    return;
                }

                if (surname.Length < 3 || surname.Length > 20)
                {
                    ErrorLabel.Text = "Nazwisko musi mieć od 3 do 20 znaków";
                    return;
                }

                if (userManager.CzyIstniejEmail(email))
                {
                    ErrorLabel.Text = "Istnieje już konto z takim adresem email!";
                    return;
                }

                bool emailOk = userManager.SprawdzPoprawnoscEmail(email);
                bool passOk = userManager.SprawdzPoprawnoscHasla(password);

                if (!emailOk || !passOk)
                {
                    // Tworzymy komunikaty oddzielnie i łączymy w jeden tekst z nową linią
                    string emailError = emailOk ? string.Empty : "Niepoprawny format emailu";
                    string passwordError = passOk ? string.Empty : "Niepoprawny format hasla";

                    ErrorLabel.Text = $"{emailError}\n{passwordError}".Trim();
                    return;
                }

                if (uprawnienia != "adm123")
                {
                    uprawnienia = "user";
                }

                string hashedPassword = PasswordHelper.HashPassword(password);
                User newUser = new User(name, surname, email, hashedPassword, uprawnienia);

                Database.InsertUserConnection(newUser);
                userManager.AddUser(newUser);

                await Task.Delay(500);

                NameEntry.Text = string.Empty;
                SurnameEntry.Text = string.Empty;
                emailEntry.Text = string.Empty;
                PasswordEntry.Text = string.Empty;
                PermissionEntry.Text = string.Empty;

                await Navigation.PopToRootAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", $"Wystąpił nieoczekiwany błąd: {ex.Message}", "OK");
            }
        }



    }
}





