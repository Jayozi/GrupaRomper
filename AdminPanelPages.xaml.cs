using Microsoft.Maui.Controls;
using SQLite;
using System.Threading.Tasks;

namespace projekt
{
    public partial class AdminPanelPage : ContentPage
    {
        public AdminPanelPage()
        {
            InitializeComponent();
            try
            {
                loadUsers();
            }
            catch (System.Exception ex)
            {
                DisplayAlert("Błąd", $"Nie udało się załadować użytkowników: {ex.Message}", "OK");
            }
        }

        // Odświeża listę użytkowników wyświetlanych w interfejsie
        private void loadUsers()
        {
            try
            {
                userManager.ListaUzytkownikow.Clear();
                Database.LoadUsers();

                UserList.ItemsSource = null;
                UserList.ItemsSource = userManager.ListaUzytkownikow;
            }
            catch (System.Exception ex)
            {
                DisplayAlert("Błąd", $"Błąd podczas ładowania użytkowników: {ex.Message}", "OK");
            }
        }



        // Obsługuje zmianę hasła dla zalogowanego użytkownika
        private async void OnChangePassword(object sender, EventArgs e)
        {
            string newPassword = await DisplayPromptAsync("Edycja uzytkownika", "Podaj nowe haslo");

            if (string.IsNullOrWhiteSpace(newPassword))
                return;  // Anulowano lub pusty input

            string hashedPassword = PasswordHelper.HashPassword(newPassword);

            if (loginPage.LoggedInUser.ZmienHaslo(hashedPassword))
                await DisplayAlert("Udane!", "Pomyslnie zmienione haslo", "Ok");
            else
                await DisplayAlert("Blad!", "Haslo nie spelnia wymagan", "Ok");
        }
        // Pokazuje panel zarządzania użytkownikami i odświeża listę
        private void OnManageUserClicked(object sender, System.EventArgs e)
        {
            try
            {
                userManagementPanel.IsVisible = !userManagementPanel.IsVisible;

                if (userManagementPanel.IsVisible)
                    loadUsers();
            }
            catch (System.Exception ex)
            {
                DisplayAlert("Błąd", $"Nie udało się otworzyć panelu zarządzania użytkownikami: {ex.Message}", "OK");
            }
        }

        // Usuwa wskazanego użytkownika po potwierdzeniu, z wyjątkiem aktualnie zalogowanego
        private async void OnDeleteUser(object sender, System.EventArgs e)
        {
            try
            {
                if (loginPage.LoggedInUser == null)
                {
                    await DisplayAlert("Błąd!", "Nie jesteś zalogowany!", "OK");
                    return;
                }

                if (sender is not Button button)
                    return;

                if (button.CommandParameter is not User userToDelete)
                    return;

                bool confirm = await DisplayAlert("Usuń użytkownika", $"Czy na pewno chcesz usunąć {userToDelete.Email}?", "Tak", "Nie");
                if (!confirm) return;

                if (userToDelete.Email != loginPage.LoggedInUser.Email)
                {
                    Database.DeleteUserConnetion(userToDelete);
                    OnPropertyChanged(nameof(userManager.ListaUzytkownikow));
                    loadUsers();
                }
                else
                {
                    await DisplayAlert("Błąd!", "Nie możesz usunąć użytkownika, na którego jesteś zalogowany!", "OK");
                }
            }
            catch (System.Exception ex)
            {
                await DisplayAlert("Błąd", $"Nie udało się usunąć użytkownika: {ex.Message}", "OK");
            }
        }

        // Edytuje dane wybranego użytkownika (imię, nazwisko, hasło)
        private async void OnEditUser(object sender, System.EventArgs e)
        {
            try
            {
                if (sender is not Button button)
                    return;

                if (button.CommandParameter is not User userToEdit)
                    return;

                string newName = await DisplayPromptAsync("Edycja użytkownika", $"Podaj nowe imię dla {userToEdit.Imie}", initialValue: userToEdit.Imie);
                if (string.IsNullOrWhiteSpace(newName)) return;

                string newSurname = await DisplayPromptAsync("Edycja użytkownika", $"Podaj nowe nazwisko dla {userToEdit.Nazwisko}", initialValue: userToEdit.Nazwisko);
                if (string.IsNullOrWhiteSpace(newSurname)) return;

                string newPassword = await DisplayPromptAsync("Edycja użytkownika", $"Podaj nowe hasło dla {userToEdit.Email}", initialValue: "");
                string finalPassword = string.IsNullOrWhiteSpace(newPassword)
                    ? userToEdit.Haslo
                    : PasswordHelper.HashPassword(newPassword);

                userToEdit.EdytujDane(newName, newSurname, finalPassword);
                loadUsers();
            }
            catch (System.Exception ex)
            {
                await DisplayAlert("Błąd", $"Nie udało się edytować użytkownika: {ex.Message}", "OK");
            }
        }

        // Pokazuje formularz do dodania nowego użytkownika
        private void OnAddUser(object sender, System.EventArgs e)
        {
            try
            {
                userAddForm.IsVisible = true;
            }
            catch (System.Exception ex)
            {
                DisplayAlert("Błąd", $"Nie udało się pokazać formularza: {ex.Message}", "OK");
            }
        }

        // Dodaje nowego użytkownika do bazy po walidacji danych i resetuje formularz
        private async void OnAddUserToDatabase(object sender, System.EventArgs e)
        {
            try
            {
                string name = NameEntry.Text;
                string surname = SurnameEntry.Text;
                string email = EmailEntry.Text;
                string password = PasswordEntry.Text;
                string admPass = AdmPassEntry.Text;

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) ||
                    string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    await DisplayAlert("Błąd", "Wszystkie pola są wymagane!", "OK");
                    return;
                }

                string hashedPassword = PasswordHelper.HashPassword(password);
                User newUser = new User(name, surname, email, hashedPassword, admPass);

                Database.InsertUserConnection(newUser);
                userManagementPanel.IsVisible = false;

                loadUsers();

                UserList.ItemsSource = null;
                UserList.ItemsSource = userManager.ListaUzytkownikow;

                await DisplayAlert("Sukces", "Użytkownik został dodany", "OK");
                userAddForm.IsVisible = false;

                // Czyszczenie formularza
                NameEntry.Text = string.Empty;
                SurnameEntry.Text = string.Empty;
                EmailEntry.Text = string.Empty;
                PasswordEntry.Text = string.Empty;
                AdmPassEntry.Text = string.Empty;
            }
            catch (System.Exception ex)
            {
                await DisplayAlert("Błąd", $"Nie udało się dodać użytkownika: {ex.Message}", "OK");
            }
        }

        // Przechodzi do strony zarządzania rozkładem jazdy autobusów
        private async void OnViewSchedule(object sender, System.EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new AdminBusPage());
            }
            catch (System.Exception ex)
            {
                await DisplayAlert("Błąd", $"Nie udało się otworzyć strony rozkładu jazdy: {ex.Message}", "OK");
            }
        }

        private async void OnViewBusManagement(object sender, System.EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new AdminBusPage());
            }
            catch (System.Exception ex)
            {
                await DisplayAlert("Błąd", $"Nie udało się otworzyć strony zarządzania busami: {ex.Message}", "OK");
            }
        }

        // Powrót do strony głównej 
        private async void OnBackButtonClicked(object sender, System.EventArgs e)
        {
                await Navigation.PopToRootAsync();
        }
    }
}