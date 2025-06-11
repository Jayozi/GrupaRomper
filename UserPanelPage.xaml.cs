using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace projekt
{
    public partial class UserPanelPage : ContentPage
    {
        public UserPanelPage()
        {
            try
            {
                InitializeComponent();
                BindingContext = this;
            }
            catch (Exception ex)
            {
                DisplayAlert("Błąd", $"Wystąpił błąd podczas inicjalizacji: {ex.Message}", "OK");
            }
        }

        public List<KeyValuePair<string, string>> UserDetails { get; set; }

        private async void OnViewSchedule(object sender, EventArgs e)
        {
            try
            {
                await DisplayAlert("Rozkład jazdy", "Tutaj będzie rozkład jazdy.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", $"Nie udało się wyświetlić rozkładu jazdy: {ex.Message}", "OK");
            }
        }

        private void OnSettings(object sender, EventArgs e)
        {
            try
            {
                if (loginPage.LoggedInUser == null)
                {
                    DisplayAlert("Błąd", "Nie jesteś zalogowany!", "OK");
                    return;
                }

                UserDetailsPanel.IsVisible = true;
                BackButton.IsVisible = true;

                UserDetails = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Imię", loginPage.LoggedInUser.Imie),
                    new KeyValuePair<string, string>("Nazwisko", loginPage.LoggedInUser.Nazwisko),
                    new KeyValuePair<string, string>("Email", loginPage.LoggedInUser.Email)
                };

                UserDetailsList.ItemsSource = UserDetails;

                if (sender is Button settingsBtn)
                    settingsBtn.IsVisible = false;
            }
            catch (Exception ex)
            {
                DisplayAlert("Błąd", $"Wystąpił błąd podczas ładowania danych użytkownika: {ex.Message}", "OK");
            }
        }

        private void OnHideUserButton(object sender, EventArgs e)
        {
            try
            {
                UserDetailsPanel.IsVisible = false;
                BackButton.IsVisible = false;
                settingsButton.IsVisible = true;
            }
            catch (Exception ex)
            {
                DisplayAlert("Błąd", $"Wystąpił błąd przy ukrywaniu danych użytkownika: {ex.Message}", "OK");
            }
        }

        private async void OnChangePassword(object sender, EventArgs e)
        {
            string newPassword = await DisplayPromptAsync("Edycja uzytkownika", "Podaj nowe haslo");

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                // Użytkownik anulował lub nic nie wpisał, po prostu wyjdź z metody
                return;
            }

            string hashedPassword = PasswordHelper.HashPassword(newPassword);

            if (loginPage.LoggedInUser.ZmienHaslo(hashedPassword))
            {
                await DisplayAlert("Udane!", "Pomyslnie zmienione haslo", "Ok");
            }
            else
            {
                await DisplayAlert("Blad!", "Haslo nie spelnia wymagan", "Ok");
            }
        }


        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
                await Navigation.PopToRootAsync();
        }

        private async void OnManageBuses(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new ManageBusesPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", $"Nie udało się otworzyć strony zarządzania autobusami: {ex.Message}", "OK");
            }
        }
    }
}
