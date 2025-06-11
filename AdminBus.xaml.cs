using Microsoft.Maui.Controls;
using projekt.Models;
using System;
using System.Collections.Generic;
using static projekt.Database;

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
            try
            {
                Buses = Database.LoadBuses();
                BusesCollectionView.ItemsSource = null;
                BusesCollectionView.ItemsSource = Buses;
            }
            catch (Exception ex)
            {
                DisplayAlert("Błąd", $"Nie udało się załadować autobusów: {ex.Message}", "OK");
            }
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
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Bus bus)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"UpdateBus called for ID={bus.ID}, Nazwa={bus.Nazwa}");
                    Database.UpdateBus(bus);

                    bus.OriginalNazwa = bus.Nazwa;
                    bus.OriginalSkad = bus.Skad;
                    bus.OriginalDokad = bus.Dokad;

                    await DisplayAlert("Sukces", "Zapisano zmiany.", "OK");
                    LoadBuses();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Błąd", $"Nie udało się zapisać zmian: {ex.Message}", "OK");
                }
            }
        }

        // Obsługa dodawania nowego autobusu do bazy danych
        private async void OnAddBusClicked(object sender, EventArgs e)
        {
            try
            {
                string nazwa = NewBusName.Text;
                string skad = NewBusFrom.Text;
                string dokad = NewBusTo.Text;
                string godzina = NewBusTime.Time.ToString(@"hh\:mm");

                if (!double.TryParse(NewBusPrice.Text, out double cena))
                {
                    await DisplayAlert("Błąd", "Nieprawidłowa cena biletu.", "OK");
                    return;
                }

                Bus newBus = new Bus(nazwa, skad, dokad, godzina, cena);
                int newId = Database.InsertBus(newBus);
                newBus.ID = newId;

                NewBusName.Text = "";
                NewBusFrom.Text = "";
                NewBusTo.Text = "";
                NewBusTime.Time = new TimeSpan(0, 0, 0);
                NewBusPrice.Text = "";

                LoadBuses();

                await DisplayAlert("Sukces", "Dodano nowy autobus.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", $"Nie udało się dodać autobusu: {ex.Message}", "OK");
            }
        }

        // Obsługa usuwania autobusu po potwierdzeniu
        private async void OnDeleteBusClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Bus bus)
            {
                bool confirm = await DisplayAlert("Potwierdzenie", $"Czy na pewno chcesz usunąć trasę {bus.Nazwa}?", "Tak", "Nie");
                if (confirm)
                {
                    try
                    {
                        Database.DeleteBus(bus);
                        LoadBuses();
                        await DisplayAlert("Sukces", "Trasa została usunięta.", "OK");
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Błąd", $"Nie udało się usunąć trasy: {ex.Message}", "OK");
                    }
                }
            }
        }

        private async void OnExportBusesClicked(object sender, EventArgs e)
        {
            try
            {
                await BusFileStorage.SaveBusesAsync(Buses);
                await DisplayAlert("Sukces", "Lista autobusów została wyeksportowana.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", $"Nie udało się wyeksportować autobusów: {ex.Message}", "OK");
            }
        }

        private async void OnImportBusesClicked(object sender, EventArgs e)
        {
            try
            {
                int imported = await BusImporter.ImportBusesFromFileAsync();
                await DisplayAlert("Import zakończony", $"Dodano {imported} nowych autobusów.", "OK");
                LoadBuses();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Błąd", $"Nie udało się zaimportować autobusów: {ex.Message}", "OK");
            }
        }

        // Obsługa przycisku powrotu – nawigacja wstecz, jeśli to możliwe
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
                await Navigation.PopAsync();
        }
    }
}
