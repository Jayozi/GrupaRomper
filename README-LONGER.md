# 🚍 Aplikacja do zarządzania rozkładem jazdy – .NET MAUI

**Platforma:** .NET MAUI  
**Baza danych:** SQLite  
**Typ aplikacji:** Wieloplatformowa (desktop, Android)

---

## 📌 Opis projektu

Aplikacja została stworzona w celu umożliwienia użytkownikom przeglądania rozkładów jazdy autobusów, a administratorom – zarządzania kontami użytkowników oraz flotą pojazdów. Projekt zawiera kompletne mechanizmy bezpieczeństwa (hashowanie haseł, walidacje danych), przejrzysty interfejs oraz obsługę wyjątków i operacji na plikach.

---

## 🧩 Technologie

| Warstwa       | Narzędzie / biblioteka     |
|---------------|-----------------------------|
| UI            | XAML (.NET MAUI)           |
| Logika        | C# (.NET 7 / .NET 8)       |
| Baza danych   | SQLite (lokalna)           |
| UI/UX         | MAUI Controls              |
| Bezpieczeństwo| Hashowanie haseł (SHA-256) |

---

## 🛠️ Funkcjonalności

### ✅ Logowanie i użytkownicy
- Logowanie z weryfikacją hasła (hash SHA-256)
- Rozróżnienie ról: administrator (`adm123`) i zwykły użytkownik
- Przechowywanie zalogowanego użytkownika (`LoggedInUser`)
- Obsługa błędnych danych logowania z komunikatem

### 🧑‍💼 Panel użytkownika
- Wyświetlanie danych osobowych (imię, nazwisko, e-mail)
- Zmiana hasła z walidacją złożoności
- Przeglądanie rozkładu jazdy (planowane rozszerzenie)

### 🔐 Bezpieczeństwo
- Hasła nie są przechowywane w postaci jawnej (SHA-256)
- Złożoność hasła: 8–20 znaków, duża litera, mała litera, cyfra
- Zabezpieczenie przed usunięciem konta, na którym użytkownik jest zalogowany

### 🧑‍🔧 Panel administratora
- Dodawanie, edytowanie, usuwanie użytkowników
- Walidacja pól (brak pustych, poprawność e-mail, unikalność adresu)
- Podgląd i modyfikacja floty autobusów
- Eksport danych użytkowników i autobusów do pliku tekstowego `.txt`
- Import danych z pliku `.txt` (z zabezpieczeniem przed błędnymi danymi)
- Obsługa wyjątków z czytelnymi komunikatami

---

## 📁 Struktura katalogów

```
projekt/
├── Pages/
│   ├── loginPage.xaml(.cs)         # Logowanie
│   ├── AdminPanelPage.xaml(.cs)    # Panel administratora
│   ├── UserPanelPage.xaml(.cs)     # Panel użytkownika
│   ├── AdminBusPage.xaml(.cs)      # Zarządzanie autobusami
├── Helpers/
│   ├── PasswordHelper.cs           # Hashowanie i walidacja haseł
├── Models/
│   ├── User.cs                     # Klasa użytkownika
│   ├── Bus.cs                      # Klasa autobusu
├── Database/
│   ├── Database.cs                 # Operacje na SQLite
├── Resources/
├── projekt.csproj
├── README.md
└── App.xaml(.cs)
```

---

## 🔄 Operacje na danych

| Funkcja           | Format     | Opis                                                         |
|-------------------|------------|--------------------------------------------------------------|
| Eksport danych    | `.txt`     | Tworzy plik z listą użytkowników/autobusów                  |
| Import danych     | `.txt`     | Wczytuje dane z pliku i aktualizuje bazę danych             |
| Baza danych       | SQLite     | Zapis lokalny danych, inicjalizacja przy uruchomieniu       |
| Walidacja danych  | C#         | Każda operacja sprawdza poprawność pól i składnię e-maili   |

---

## ✅ Walidacja hasła

Implementacja `SprawdzPoprawnoscHasla` sprawdza:

- Czy hasło ma od 8 do 20 znaków
- Czy zawiera co najmniej:
  - 1 dużą literę
  - 1 małą literę
  - 1 cyfrę

```csharp
public static bool SprawdzPoprawnoscHasla(string haslo)
{
    if (string.IsNullOrWhiteSpace(haslo)) return false;
    if (haslo.Length < 8 || haslo.Length > 20) return false;

    bool hasUpper = false, hasLower = false, hasDigit = false;
    foreach (char c in haslo)
    {
        if (char.IsUpper(c)) hasUpper = true;
        else if (char.IsLower(c)) hasLower = true;
        else if (char.IsDigit(c)) hasDigit = true;
    }

    return hasUpper && hasLower && hasDigit;
}
```

---

## 🧪 Dane testowe

| Typ           | Email               | Hasło      | Uprawnienia |
|---------------|---------------------|------------|-------------|
| Administrator | `admin@example.com` | `Admin123` | `adm123`    |
| Użytkownik    | `user@example.com`  | `User1234` | `null`      |

---

## 🚀 Uruchomienie projektu

1. Otwórz projekt w Visual Studio 2022 lub nowszym (z obsługą .NET MAUI).
2. Zainstaluj brakujące zależności (np. SQLite-net).
3. Ustaw `projekt` jako projekt startowy.
4. Uruchom na emulatorze Androida lub jako aplikację desktopową.
5. Po uruchomieniu zaloguj się za pomocą danych testowych.

---

## 📌 Możliwe rozszerzenia

- Rejestracja nowych użytkowników z poziomu UI
- System powiadomień push lub e-mail
- Synchronizacja z zewnętrznym API rozkładów jazdy
- Historia logowań / zmiany hasła
- Tryb offline z pełną synchronizacją danych

---

## 👨‍💻 Autor

Autorzy: 
rystian Koza  
Mateusz Janiczek  
Jakub Michałek  
Rok: **2025**

---

## 📃 Licencja

Projekt przeznaczony wyłącznie do celów edukacyjnych.
