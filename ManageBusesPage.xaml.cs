using Microsoft.Maui.Controls;
using projekt.Models;
using System.Collections.Generic;
using System;

namespace projekt
{
    public partial class ManageBusesPage : ContentPage
    {
        // Lista autobusów wyświetlanych i zarządzanych na stronie
        public List<Bus> Buses { get; set; }
        public ManageBusesPage()
        {
            InitializeComponent();   
            LoadBuses();        
            BindingContext = this;  
        }
        private void LoadBuses()
        {
            Buses = Database.LoadBuses();       
            BusesCollectionView.ItemsSource = Buses; 
        }

        // Obsługa kliknięcia przycisku "Powrót"
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 1)
            {
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Informacja", "Brak poprzedniej strony.", "OK");
            }
        }
    }
}
