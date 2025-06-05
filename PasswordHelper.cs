using System.Security.Cryptography;

namespace projekt
{
    public static class PasswordHelper
    {
        // Tworzy bezpieczny, zasolony hash hasła metodą PBKDF2 z SHA256
        public static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16); // generuje 16 bajtów soli (128-bit)
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256); // 100k iteracji
            byte[] hash = pbkdf2.GetBytes(32); // 32 bajty (256-bit) hash

            // łączy sól i hash w jedną tablicę bajtów (48 bajtów)
            byte[] hashBytes = new byte[48];
            Array.Copy(salt, 0, hashBytes, 0, 16); // sól na początku
            Array.Copy(hash, 0, hashBytes, 16, 32); // hash zaraz po soli

            // zwraca wynik jako base64, gotowy do przechowywania
            return Convert.ToBase64String(hashBytes);
        }

        // Weryfikuje czy podane hasło zgadza się z hashem (sprawdza po soli i hash)
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword); // rozkoduj base64
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16); // wyciągnij sól z pierwszych 16 bajtów

            // wygeneruj hash podanego hasła używając wyciągniętej soli
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32);

            // porównaj każdy bajt wygenerowanego hash z oryginalnym
            for (int i = 0; i < 32; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false; // jeśli różni się choć jeden bajt, hasła się nie zgadzają

            return true; // hasła są takie same
        }
    }
}
