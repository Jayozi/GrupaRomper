using SQLite;
using projekt.Models;
using System.Collections.Generic;

namespace projekt
{
    internal class Database
    {
        // ----------------- KONFIGURACJA ŚCIEŻKI DO BAZY --------------------

        private static readonly string fullPath;
        static Database()
        {
#if WINDOWS
            // Na Windows: ścieżka do folderu bazy ręcznie zdefiniowana
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string relativePath = Path.Combine(basePath, "..", "..", "..", "..", "..", "Resources", "Database");
            fullPath = Path.GetFullPath(relativePath);
#else
            // Na Android i iOS baza jest w katalogu AppData
            fullPath = Path.Combine(FileSystem.AppDataDirectory);
#endif
        }

        // Tworzy nowe połączenie do pliku bazy użytkowników
        public static SQLiteConnection Connection()
        {
            return new SQLiteConnection(Path.Combine(fullPath, "database.db"));
        }

        // Inicjalizuje bazę - tworzy tabelę użytkowników jeśli nie istnieje
        public static void Initialize()
        {
            using var connection = CreateConnection();
            connection.CreateTable<User>();
        }

        // Pobiera listę wszystkich użytkowników z bazy
        public static List<User> GetUsers()
        {
            using (var conn = new SQLiteConnection(Path.Combine(fullPath, "database.db")))
            {
                return conn.Table<User>().ToList();
            }
        }

        // Wstawia nowego użytkownika do bazy (używa nowego połączenia)
        public static void InsertUserConnection(User user)
        {
            SQLiteConnection connection = CreateConnection();
            InsertUser(connection, user);
        }

        // Usuwa użytkownika z bazy i z listy użytkowników w pamięci
        public static void DeleteUserConnetion(User user)
        {
            SQLiteConnection connection = CreateConnection();
            DeleteUser(connection, user);
        }

        // Aktualizuje dane użytkownika w bazie
        public static void UpdateUserConnetion(User user)
        {
            UpdateUser(CreateConnection(), user);
        }

        // Tworzy nowe połączenie do bazy użytkowników
        public static SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection(Path.Combine(fullPath, "database.db"));
        }

        // Wczytuje użytkowników z bazy i dodaje ich do listy w pamięci (userManager)
        static void ReadData(SQLite.SQLiteConnection connection)
        {
            var usersFromDb = connection.Table<User>().ToList();

            foreach (var user in usersFromDb)
            {
                var existingUser = userManager.ListaUzytkownikow.FirstOrDefault(u => u.Email == user.Email);

                if (existingUser == null)
                {
                    userManager.AddUser(user);
                }
            }
        }

        // Metoda do wczytania użytkowników z bazy do pamięci
        public static void LoadUsers()
        {
            using var connection = CreateConnection();
            ReadData(connection);
        }

        // Wstawia użytkownika do bazy (przez przekazane połączenie)
        static void InsertUser(SQLite.SQLiteConnection connection, User user)
        {
            connection.Insert(user);
        }

        // Usuwa użytkownika z bazy i z pamięci
        static void DeleteUser(SQLite.SQLiteConnection connection, User user)
        {
            connection.Delete(user);

            var userInList = userManager.ListaUzytkownikow.FirstOrDefault(u => u.Email == user.Email);
            if (userInList != null)
                userManager.ListaUzytkownikow.Remove(userInList);
        }

        // Aktualizuje użytkownika w bazie
        static void UpdateUser(SQLite.SQLiteConnection connection, User user)
        {
            connection.Update(user);
        }


        // ----------------- BAZA AUTOBUSÓW --------------------

        // Tworzy połączenie do bazy autobusów (oddzielna baza plikowa)
        private static SQLite.SQLiteConnection CreateBusConnection()
        {
            var connection = new SQLite.SQLiteConnection($"{fullPath}\\BusesDataBase.db");
            return connection;
        }

        // Wczytuje wszystkie autobusy z bazy
        public static List<Bus> LoadBuses()
        {
            using var connection = CreateBusConnection();
            return connection.Table<Bus>().ToList();
        }

        // Dodaje nowy autobus do bazy i zwraca jego ID
        public static int InsertBus(Bus bus)
        {
            using var connection = CreateBusConnection();
            int rows = connection.Insert(bus);
            return bus.ID;
        }

        // Usuwa autobus z bazy
        public static void DeleteBus(Bus bus)
        {
            using var connection = CreateBusConnection();
            connection.Delete(bus);
        }

        // Aktualizuje dane autobusu w bazie
        public static void UpdateBus(Bus bus)
        {
            using var connection = CreateBusConnection();
            connection.Update(bus);
        }
    }
}
