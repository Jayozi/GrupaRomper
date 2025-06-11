using System.Text.RegularExpressions;

namespace projekt
{
    public static class userManager
    {
        // Lista przechowująca wszystkich użytkowników w aplikacji
        public static List<User> ListaUzytkownikow = new List<User>();

        // Dodaje nowego użytkownika do listy
        public static void AddUser(User user)
        {
            ListaUzytkownikow.Add(user);
        }

        // Znajduje i zwraca użytkownika na podstawie adresu email, albo null jeśli nie ma
        public static User GetUserByEmail(string email)
        {
            return ListaUzytkownikow.FirstOrDefault(u => u.Email == email);
        }

        // Sprawdza, czy podany email jest w poprawnym formacie (prosty regex)
        public static bool SprawdzPoprawnoscEmail(string email)
        {
            string wzorzec = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, wzorzec);
        }

        // Zaladowanie jeszcze raz odswiezonej listy uzytkownikow
        public static void ReloadUsersFromDatabase()
        {
            ListaUzytkownikow.Clear();
            Database.LoadUsers();
        }

        // Sprawdza, czy dany email już istnieje na liście użytkowników
        public static bool CzyIstniejEmail(string email)
        {
            return ListaUzytkownikow.Any(u => u.SprawdzEmail(email));
        }

        // Sprawdza, czy hasło spełnia minimalne wymagania: długość, duża i mała litera
        public static bool SprawdzPoprawnoscHasla(string haslo)
        {
            if (string.IsNullOrWhiteSpace(haslo))
                return false;

            if (haslo.Length < 8 || haslo.Length > 20)
                return false;

            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigit = false;

            foreach (char c in haslo)
            {
                if (char.IsUpper(c)) hasUpper = true;
                else if (char.IsLower(c)) hasLower = true;
                else if (char.IsDigit(c)) hasDigit = true;
            }

            return hasUpper && hasLower && hasDigit;
        }

    }
}
