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
            InitializeComponent();
            BindingContext = this;
        }

        public List<KeyValuePair<string, string>> UserDetails { get; set; }

        private void OnViewSchedule(object sender, EventArgs e)
        {
            DisplayAlert("Rozkład jazdy", "Tutaj będzie rozkład jazdy.", "OK");
        }

        private void OnSettings(object sender, EventArgs e)
        {
            UserDetailsPanel.IsVisible = true;
            BackButton.IsVisible = true;

            UserDetails = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Imię", loginPage.LoggedInUser.Imie),
                new KeyValuePair<string, string>("Nazwisko", loginPage.LoggedInUser.Nazwisko),
                new KeyValuePair<string, string>("Email", loginPage.LoggedInUser.Email)
            };

            UserDetailsList.ItemsSource = UserDetails;

            (sender as Button).IsVisible = false;
        }

        private void OnHideUserButton(object sender, EventArgs e)
        {
            UserDetailsPanel.IsVisible = false;
            BackButton.IsVisible = false;
            settingsButton.IsVisible = true;
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

        // 🔹 Nowa metoda do zarządzania autobusami
        private async void OnManageBuses(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ManageBusesPage());
        }
    }
}
