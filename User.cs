using SQLite;

namespace projekt
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        [Column("User_id")]
        public int Id { get; set; } 

        [Column("name")]
        public string Imie { get; set; } 

        [Column("surname")]
        public string Nazwisko { get; set; }  

        [Column("email")]
        public string Email { get; set; }  

        [Column("password")]
        public string Haslo { get; set; } 

        [Column("admPass")]
        public string Uprawnienia { get; set; }  

        // Bezparametrowy konstruktor wymagany przez bibliotekę SQLite-net
        public User()
        {
        }

        // Konstruktor do wygodnego tworzenia nowego użytkownika w kodzie
        public User(string imie, string nazwisko, string email, string haslo, string uprawnienia)
        {
            Imie = imie;
            Nazwisko = nazwisko;
            Email = email;
            Haslo = haslo;
            Uprawnienia = uprawnienia;
        }

        // Sprawdza, czy podane hasło jest zgodne z zapisanym (prosta metoda porównawcza)
        public bool SprawdzHaslo(string podaneHaslo)
        {
            return Haslo == podaneHaslo;
        }

        // Sprawdza, czy podany email jest zgodny z tym przypisanym do użytkownika
        public bool SprawdzEmail(string podanyEmail)
        {
            return Email == podanyEmail;
        }

        // Zwraca aktualne uprawnienia użytkownika
        public string SprawdzUprawnienia()
        {
            return Uprawnienia;
        }

        // Aktualizuje dane użytkownika i zapisuje zmiany w bazie
        public void EdytujDane(string imie, string nazwisko, string haslo)
        {
            Imie = imie;
            Nazwisko = nazwisko;
            Haslo = haslo;

            Database.UpdateUserConnetion(this);
        }

        // Próbuje zmienić hasło, jeśli spełnia wymagania - zwraca true jeśli się udało
        public bool ZmienHaslo(string noweHaslo)
        {
            if (userManager.SprawdzPoprawnoscHasla(noweHaslo))
            {
                Haslo = noweHaslo;
                Database.UpdateUserConnetion(this);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
