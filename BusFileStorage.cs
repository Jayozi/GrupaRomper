using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using projekt.Models;

namespace projekt
{
    public static class BusFileStorage
    {
        private static readonly string FolderName = "Buses";

        // Ścieżka do folderu Dokumenty użytkownika + podfolder Buses
        private static readonly string FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), FolderName);

        // Pełna ścieżka do pliku buses.json w tym folderze
        private static readonly string FilePath = Path.Combine(FolderPath, "buses.json");

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true, // ładny format JSON
        };

        public static async Task SaveBusesAsync(List<Bus> buses)
        {
            try
            {
                // Tworzymy folder, jeśli nie istnieje
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }

                string json = JsonSerializer.Serialize(buses, JsonOptions);
                await File.WriteAllTextAsync(FilePath, json);
            }
            catch (Exception ex)
            {
                // Możesz tu zrobić logowanie, albo po prostu wyrzucić dalej
                throw new Exception($"Błąd zapisu pliku: {ex.Message}", ex);
            }
        }

        public static async Task<List<Bus>> LoadBusesAsync()
        {
            try
            {
                if (!File.Exists(FilePath))
                    return new List<Bus>();

                string json = await File.ReadAllTextAsync(FilePath);
                var buses = JsonSerializer.Deserialize<List<Bus>>(json, JsonOptions);
                return buses ?? new List<Bus>();
            }
            catch (Exception ex)
            {
                // Jeśli plik jest uszkodzony, możesz go usunąć lub zwrócić pustą listę
                // File.Delete(FilePath); // opcjonalnie usuń uszkodzony plik
                throw new Exception($"Błąd odczytu pliku: {ex.Message}", ex);
            }
        }
    }
}
