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
            loadUsers();
        }

        // Odświeża listę użytkowników wyświetlanych w interfejsie
        private void loadUsers()
        {
            userManager.ListaUzytkownikow.Clear(); 
            Database.LoadUsers();                  

            UserList.ItemsSource = null;            
            UserList.ItemsSource = userManager.ListaUzytkownikow; 
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
        private void OnManageUserClicked(object sender, EventArgs e)
        {
            // Pokaż panel zarządzania użytkownikami
            userManagementPanel.IsVisible = !userManagementPanel.IsVisible;

            // Załaduj listę użytkowników
            if (userManagementPanel.IsVisible)
                loadUsers();
        }

        // Usuwa wskazanego użytkownika po potwierdzeniu, z wyjątkiem aktualnie zalogowanego
        private async void OnDeleteUser(object sender, EventArgs e)
        {
            if (loginPage.LoggedInUser == null)
            {
                await DisplayAlert("Blad!", "Nie jestes zalogowany!", "Ok");
                return;
            }

            var button = sender as Button;
            var userToDelete = button?.CommandParameter as User;

            if (userToDelete == null) return;

            bool confirm = await DisplayAlert("Usun uzytkownika", $"Czy na pewno chcesz usunac {userToDelete.Email}?", "Tak", "Nie");
            if (!confirm) return;

            if (userToDelete.Email != loginPage.LoggedInUser.Email)
            {
                Database.DeleteUserConnetion(userToDelete);
                OnPropertyChanged(nameof(userManager.ListaUzytkownikow));
                loadUsers();
            }
            else
                await DisplayAlert("Blad!", "Nie mozesz usunac uzytkownika na ktorego jestes zalogowany!", "Ok");
        }

        // Edytuje dane wybranego użytkownika (imię, nazwisko, hasło)
        private async void OnEditUser(object sender, EventArgs e)
        {
            var button = sender as Button;
            var userToEdit = button?.CommandParameter as User;

            if (userToEdit == null) return;

            string newName = await DisplayPromptAsync("Edycja uzytkownika", $"Podaj nowe imie dla {userToEdit.Imie}", initialValue: userToEdit.Imie);
            if (string.IsNullOrWhiteSpace(newName)) return;

            string newSurname = await DisplayPromptAsync("Edycja uzytkownika", $"Podaj nowe nazwisko dla {userToEdit.Nazwisko}", initialValue: userToEdit.Nazwisko);
            if (string.IsNullOrWhiteSpace(newSurname)) return;

            string newPassword = await DisplayPromptAsync("Edycja użytkownika", $"Podaj nowe hasło dla {userToEdit.Email}", initialValue: "");
            string finalPassword = string.IsNullOrWhiteSpace(newPassword)
                ? userToEdit.Haslo  // jeśli puste, nie zmieniamy hasła
                : PasswordHelper.HashPassword(newPassword);

            userToEdit.EdytujDane(newName, newSurname, finalPassword);
            loadUsers();
        }

        // Pokazuje formularz do dodania nowego użytkownika
        private void OnAddUser(object sender, EventArgs e)
        {
            userAddForm.IsVisible = true;
        }

        // Dodaje nowego użytkownika do bazy po walidacji danych i resetuje formularz
        private async void OnAddUserToDatabase(object sender, EventArgs e)
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

        // Przechodzi do strony zarządzania rozkładem jazdy autobusów
        private async void OnViewSchedule(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminBusPage());
        }

        private async void OnViewBusManagement(object sender, EventArgs e)
        {
            // Przykładowo, zakładam że zarządzanie busami to nowa strona AdminBusPage
            await Navigation.PushAsync(new AdminBusPage());
        }


        // Powrót do strony głównej 
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }

}
