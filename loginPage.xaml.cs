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
            string email = emailEntry.Text;       
            string password = passwordEntry.Text; 

            // Znajdź użytkownika o podanym emailu
            var user = userManager.ListaUzytkownikow.FirstOrDefault(u => u.Email == email);

            // Sprawdź, czy hasło jest poprawne (porównanie z zahashowanym hasłem)
            if (user != null && PasswordHelper.VerifyPassword(password, user.Haslo))
            {
                LoggedInUser = user;  // Zapisz użytkownika jako zalogowanego

                // Przekieruj na odpowiedni panel w zależności od uprawnień
                if (user.Uprawnienia == "adm123")
                {
                    await Navigation.PushAsync(new AdminPanelPage());
                }
                else
                {
                    await Navigation.PushAsync(new UserPanelPage());
                }
            }
            else
            {
                ErrorLabel.Text = "Niepoprawny e-mail lub hasło!";
            }
        }
    }
}
