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

            // Sprawdza czy długość hasła jest między 5 a 14 znaków 
            if (haslo.Length >= 5 && haslo.Length <= 14)
            {
                // Sprawdza, czy jest przynajmniej jedna duża i jedna mała litera
                if (haslo.Any(char.IsUpper) && haslo.Any(char.IsLower))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
