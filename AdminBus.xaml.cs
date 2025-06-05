using Microsoft.Maui.Controls;
using projekt.Models;
using System;
using System.Collections.Generic;

namespace projekt
{
    public partial class AdminBusPage : ContentPage
    {
        // Lista autobusów do wyświetlenia i zarządzania na stronie
        public List<Bus> Buses { get; set; }

        // Konstruktor strony
        public AdminBusPage()
        {
            InitializeComponent();   
            LoadBuses();            
            BindingContext = this; 
        }

        // Metoda ładująca autobusy z bazy i aktualizująca widok
        private void LoadBuses()
        {
            Buses = Database.LoadBuses();
            BusesCollectionView.ItemsSource = null; 
            BusesCollectionView.ItemsSource = Buses; 
        }

        // Obsługa kliknięcia przycisku „Edytuj” – pokazuje lub ukrywa panel edycji dla konkretnego autobusu
        private void OnEditClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                var parentStack = button.Parent as StackLayout;
                if (parentStack?.Parent is StackLayout outerStack)
                {
                    foreach (var child in outerStack.Children)
                    {
                        if (child is Frame editPanel && editPanel.StyleId == "EditPanel")
                        {
                            editPanel.IsVisible = !editPanel.IsVisible;
                            break;
                        }
                    }
                }
            }
        }

        // Obsługa zapisu zmian po edycji autobusu
        private void OnSaveClicked(object sender, EventArgs e)
        {
            // Sprawdź czy przycisk posiada przypisany obiekt autobusu jako CommandParameter
            if (sender is Button button && button.CommandParameter is Bus bus)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"UpdateBus called for ID={bus.ID}, Nazwa={bus.Nazwa}");
                    Database.UpdateBus(bus);

                    bus.OriginalNazwa = bus.Nazwa;
                    bus.OriginalSkad = bus.Skad;
                    bus.OriginalDokad = bus.Dokad;

                    DisplayAlert("Sukces", "Zapisano zmiany.", "OK");
                    LoadBuses();  // Odśwież listę autobusów
                }
                catch (Exception ex)
                {
                    DisplayAlert("Błąd", $"Nie udało się zapisać zmian: {ex.Message}", "OK");
                }
            }
        }

        // Obsługa dodawania nowego autobusu do bazy danych
        private void OnAddBusClicked(object sender, EventArgs e)
        {
            // Pobierz dane z formularza
            string nazwa = NewBusName.Text;
            string skad = NewBusFrom.Text;
            string dokad = NewBusTo.Text;
            string godzina = NewBusTime.Time.ToString(@"hh\:mm");

            // Sprawdź, czy cena jest poprawna liczbowo
            if (!double.TryParse(NewBusPrice.Text, out double cena))
            {
                DisplayAlert("Błąd", "Nieprawidłowa cena biletu.", "OK");
                return;
            }

            // Utwórz nowy obiekt autobusu
            Bus newBus = new Bus(nazwa, skad, dokad, godzina, cena);

            // Dodaj go do bazy i pobierz przypisane ID
            int newId = Database.InsertBus(newBus);
            newBus.ID = newId;

            // Wyczyść pola formularza po dodaniu
            NewBusName.Text = "";
            NewBusFrom.Text = "";
            NewBusTo.Text = "";
            NewBusTime.Time = new TimeSpan(0, 0, 0);
            NewBusPrice.Text = "";

            LoadBuses(); 

            DisplayAlert("Sukces", "Dodano nowy autobus.", "OK");
        }

        // Obsługa usuwania autobusu po potwierdzeniu
        private async void OnDeleteBusClicked(object sender, EventArgs e)
        {
            // Sprawdź, czy kliknięty przycisk ma powiązany autobus
            if (sender is Button button && button.CommandParameter is Bus bus)
            {
                bool confirm = await DisplayAlert("Potwierdzenie", $"Czy na pewno chcesz usunąć trasę {bus.Nazwa}?", "Tak", "Nie");
                if (confirm)
                {
                    Database.DeleteBus(bus);
                    LoadBuses();             
                    await DisplayAlert("Sukces", "Trasa została usunięta.", "OK");
                }
            }
        }

        // Obsługa przycisku powrotu – nawigacja wstecz, jeśli to możliwe
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 1)
                await Navigation.PopAsync();
            else
                await DisplayAlert("Informacja", "Brak poprzedniej strony.", "OK");
        }
    }
}
