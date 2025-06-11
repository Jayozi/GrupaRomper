using Microsoft.Maui.Controls;

namespace projekt
{
    public partial class loginPage : ContentPage
    {
        public static User? LoggedInUser;  // Przechowuje zalogowanego użytkownika
        public loginPage()
        {
            InitializeComponent(); 
        }

        // Obsługa kliknięcia przycisku "Wstecz" - powrót do poprzedniej strony
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // Obsługa kliknięcia przycisku "Zaloguj się"
        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            try
            {
                string email = emailEntry.Text;
                string password = passwordEntry.Text;

                var user = userManager.ListaUzytkownikow.FirstOrDefault(u => u.Email == email);

                if (user != null && PasswordHelper.VerifyPassword(password, user.Haslo))
                {
                    LoggedInUser = user;

                    if (user.Uprawnienia == "adm123")
                        await Navigation.PushAsync(new AdminPanelPage());
                    else
                        await Navigation.PushAsync(new UserPanelPage());
                }
                else
                {
                    ErrorLabel.Text = "Niepoprawny e-mail lub hasło!";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", $"Wystąpił błąd podczas logowania: {ex.Message}", "OK");
            }
        }

    }
}
