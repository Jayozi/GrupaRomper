# 🚌 Aplikacja do zarządzania autobusami – .NET MAUI

---

## 1. Opis projektu

**Nazwa aplikacji:** Zarządzanie autobusami (Bus Management App)  
**Cel aplikacji:**  
Aplikacja służy do zarządzania trasami autobusowymi. Umożliwia rejestrację użytkowników, logowanie z podziałem na role (administrator i zwykły użytkownik) oraz pozwala administratorowi na dodawanie, edycję i usuwanie tras autobusowych.  

**Problem do rozwiązania:**  
Brak łatwego narzędzia do zarządzania rozkładami jazdy autobusów w małej organizacji.  
Aplikacja dostarcza prosty i intuicyjny interfejs do zarządzania danymi o trasach oraz zapewnia bezpieczeństwo dostępu przez uwierzytelnianie i nadawanie uprawnień.

---

## 2. Architektura aplikacji

### Diagram architektury

```
[User Interface - XAML Pages]
          │
          ▼
[ViewModels / Code-behind (logika UI)]
          │
          ▼
[Database Layer - SQLite + serwisy]
          │
          ▼
[Modele danych (Bus, User)]
```

### Opis modułów

- **Moduł logowania i rejestracji:** Obsługuje tworzenie kont użytkowników, walidację danych i zabezpieczenie haseł (hash PBKDF2 + salt).
- **Moduł zarządzania użytkownikami:** Przechowuje dane o użytkownikach oraz role (admin/user).
- **Moduł zarządzania autobusami:** Pozwala administratorowi przeglądać, dodawać, edytować i usuwać trasy autobusów.
- **Baza danych:** Lokalna baza SQLite, w której przechowywane są wszystkie dane aplikacji.

---

## 3. Opis funkcji i funkcjonalności

### Rejestracja i logowanie

- Użytkownik podaje e-mail, nazwę użytkownika i hasło.
- Hasło jest hashowane i solone przed zapisem do bazy.
- W przypadku podania poprawnego kodu administratora użytkownik otrzymuje rolę admina.
- Możliwość logowania i weryfikacji hasła.

### Panel administratora (AdminBusPage)

- Wyświetlanie listy autobusów w kolekcji.
- Możliwość edycji danych autobusu: nazwa, trasa (skąd, dokąd), godzina, cena biletu.
- Usuwanie tras z potwierdzeniem.
- Dodawanie nowych tras poprzez formularz.

### Panel użytkownika

- Podgląd dostępnych tras autobusowych (bez możliwości edycji).

---

## 4. Instrukcja instalacji i uruchomienia

### Wymagania

- Visual Studio 2022+ z zainstalowanym workloadem **.NET MAUI**
- Emulator Androida lub urządzenie z Androidem/iOS
- System operacyjny: Windows 10+ lub macOS z odpowiednim środowiskiem
- .NET 7 SDK lub nowszy

### Kroki uruchomienia

1. Sklonuj repozytorium:
   ```bash
   git clone https://github.com/twoj-login/projekt-autobusy.git
   cd projekt-autobusy
   ```

2. Otwórz projekt w Visual Studio (`.sln`).

3. Zbuduj projekt (Ctrl+Shift+B).

4. Uruchom na emulatorze lub urządzeniu fizycznym (F5).

---

## 5. Dokumentacja kodu

### Klasy główne

- **Bus.cs**  
  Model danych przechowujący informacje o trasie autobusu:  
  `ID, Nazwa, Skad, Dokad, Godzina, CenaBiletu`

- **User.cs**  
  Model użytkownika zawierający:  
  `ID, Username, Email, HashedPassword, Salt, Role`

- **Database.cs**  
  Statyczna klasa odpowiedzialna za interakcję z bazą SQLite – wczytywanie, zapisywanie, usuwanie tras i użytkowników.

- **AdminBusPage.xaml.cs**  
  Logika strony zarządzania autobusami:  
  - Ładowanie listy tras  
  - Obsługa edycji i usuwania  
  - Dodawanie nowej trasy  
  - Aktualizacja UI

### Przykład kluczowej metody (dodawanie autobusu):

```csharp
private void OnAddBusClicked(object sender, EventArgs e)
{
    string nazwa = NewBusName.Text;
    string skad = NewBusFrom.Text;
    string dokad = NewBusTo.Text;
    string godzina = NewBusTime.Time.ToString(@"hh\:mm");
    
    if (!double.TryParse(NewBusPrice.Text, out double cena))
    {
        DisplayAlert("Błąd", "Nieprawidłowa cena biletu.", "OK");
        return;
    }

    Bus newBus = new Bus(nazwa, skad, dokad, godzina, cena);
    int newId = Database.InsertBus(newBus);
    newBus.ID = newId;

    // Czyszczenie formularza i odświeżenie listy
    NewBusName.Text = "";
    NewBusFrom.Text = "";
    NewBusTo.Text = "";
    NewBusTime.Time = new TimeSpan(0, 0, 0);
    NewBusPrice.Text = "";

    LoadBuses();
    DisplayAlert("Sukces", "Dodano nowy autobus.", "OK");
}
```

---

## 6. Przykłady użycia

### Scenariusz 1: Dodanie nowej trasy przez administratora

1. Zaloguj się jako administrator.
2. Wypełnij pola „Nazwa”, „Skąd”, „Dokąd”, wybierz godzinę i wpisz cenę biletu.
3. Kliknij „Dodaj autobus”.
4. Nowa trasa pojawi się na liście.

### Scenariusz 2: Edycja istniejącej trasy

1. Kliknij przycisk „Edytuj” obok wybranego autobusu.
2. Zmień dane w polach edycji.
3. Kliknij „Zapisz zmiany”.
4. Zmiany zostaną zapisane i widoczne na liście.

### Scenariusz 3: Usuwanie trasy

1. Kliknij przycisk „Usuń” obok trasy.
2. Potwierdź usunięcie w oknie dialogowym.
3. Trasa zostanie usunięta z bazy i z listy.

---

## 7. Błędy i ich obsługa

- **Nieprawidłowa cena biletu:**  
  Wyświetlany jest alert z informacją o błędzie i blokowane jest dodanie autobusu.

- **Brak połączenia z bazą danych:**  
  W przypadku problemów z odczytem lub zapisem danych aplikacja wyświetla komunikat błędu.

- **Niepoprawne dane logowania:**  
  Użytkownik zostaje poinformowany o błędnym emailu lub haśle.

- **Wyjątki:**  
  Obsługiwane są typowe wyjątki związane z operacjami na bazie SQLite i interakcją z UI; aplikacja wyświetla komunikaty i zapobiega awariom.

---

## 8. Wnioski i przyszłe usprawnienia

### Co działa dobrze

- Intuicyjny i responsywny interfejs
- Bezpieczne hashowanie haseł i walidacja użytkowników
- Prosta obsługa CRUD tras autobusowych przez administratora

### Trudności

- Synchronizacja stanu UI po zmianach w liście tras (wymagała ręcznego odświeżania)
- Zarządzanie widocznością panelu edycji na poziomie każdego elementu CollectionView

### Możliwe usprawnienia

- Dodanie mechanizmu rezerwacji miejsc i sprzedaży biletów
- Integracja z zewnętrznym API rozkładów jazdy
- Obsługa wielu platform z różnymi widokami (desktop/mobile)
- Zaimplementowanie testów jednostkowych i UI
- Przeniesienie danych do chmury dla synchronizacji między urządzeniami

---

# 📄 Licencja

MIT License – projekt edukacyjny, do użytku niekomercyjnego z zachowaniem informacji o autorze.

---

*Dziękuję za zapoznanie się z dokumentacją. W razie pytań lub sugestii proszę o kontakt.*  
🚍💨

