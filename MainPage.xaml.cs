using Microsoft.Maui.Controls;

namespace projekt
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            Database.Initialize();    
            Database.Connection();    
            Database.LoadUsers();     
            InitializeComponent();   
        }

        // Obsługa kliknięcia przycisku logowania - przejście do strony logowania
        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new loginPage());
        }

        // Obsługa kliknięcia przycisku rejestracji - przejście do strony rejestracji
        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new registerPage());
        }
    }
}
