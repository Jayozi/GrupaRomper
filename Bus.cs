using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;

namespace projekt.Models
{
    [Table("BusesDataBase")]  // Mapa klasy do tabeli w SQLite
    public class Bus : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Metoda wywoływana, gdy zmienia się właściwość, aby UI mogło się odświeżyć
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private int id;
        [PrimaryKey, AutoIncrement]  
        public int ID
        {
            get => id;
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        private string nazwa;
        public string Nazwa
        {
            get => nazwa;
            set
            {
                if (nazwa != value)
                {
                    nazwa = value;
                    OnPropertyChanged();
                }
            }
        }

        private string skad;
        public string Skad
        {
            get => skad;
            set
            {
                if (skad != value)
                {
                    skad = value;
                    OnPropertyChanged();
                }
            }
        }

        private string dokad;
        public string Dokad
        {
            get => dokad;
            set
            {
                if (dokad != value)
                {
                    dokad = value;
                    OnPropertyChanged();
                }
            }
        }

        private string godzina;
        public string Godzina
        {
            get => godzina;
            set
            {
                if (godzina != value)
                {
                    godzina = value;
                    OnPropertyChanged();
                }
            }
        }

        private double cenaBiletu;
        public double CenaBiletu
        {
            get => cenaBiletu;
            set
            {
                if (cenaBiletu != value)
                {
                    cenaBiletu = value;
                    OnPropertyChanged();
                }
            }
        }

        // Pola do przechowywania oryginalnych wartości (nie zapisywane w bazie)
        [Ignore]
        public string OriginalNazwa { get; set; }
        [Ignore]
        public string OriginalSkad { get; set; }
        [Ignore]
        public string OriginalDokad { get; set; }

        // Bezparametrowy konstruktor wymagany przez SQLite-net
        public Bus()
        {
        }

        // Konstruktor do wczytywania rekordu z bazy (z ID)
        public Bus(int ID, string nazwa, string skad, string dokad, string godzina, double cena)
        {
            id = ID;
            Nazwa = nazwa;
            Skad = skad;
            Dokad = dokad;
            Godzina = godzina;
            CenaBiletu = cena;
        }

        // Konstruktor do tworzenia nowych rekordów (bez ID)
        public Bus(string nazwa, string skad, string dokad, string godzina, double cena)
        {
            Nazwa = nazwa;
            Skad = skad;
            Dokad = dokad;
            Godzina = godzina;
            CenaBiletu = cena;
        }
    }
}
