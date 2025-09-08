# WorkDays – Nastavení projektu (Heroku-space)

Tento soubor popisuje doporučený postup pro správné založení víceprojektového .NET řešení pro API a datovou vrstvu, aby nedošlo k vytvoření nežádoucích .sln souborů a bylo možné používat relativní using/directivy napříč projekty.

## 1. Vytvoření API projektu

```sh
dotnet new web -o WorkDays.Api -f net9.0
```

> Vytvoří ASP.NET Core Web API projekt v podsložce `WorkDays.Api`.

## 2. Vytvoření knihovny pro datovou vrstvu

```sh
dotnet new classlib -o WorkDays.Data --language c# -f net9.0
```

> Vytvoří knihovnu tříd v podsložce `WorkDays.Data` pro modely, repozitáře a služby.

## 3. Vytvoření řešení (sln) v root složce

**Tento krok je zásadní!**

```sh
dotnet new sln -n WorkDays
```

> Vytvoří řešení v rootu (`Heroku-space`).

## 4. Přidání projektů do řešení

```sh
dotnet sln WorkDays.sln add WorkDays.Api/WorkDays.Api.csproj

dotnet sln WorkDays.sln add WorkDays.Data/WorkDays.Data.csproj
```

> Přidá oba projekty do jednoho řešení. Díky tomu budou všechny `using` relativní a nevzniknou zbytečné .sln soubory v podsložkách.

## 5. Přidání reference mezi projekty

```sh
dotnet add WorkDays.Api/WorkDays.Api.csproj reference WorkDays.Data/WorkDays.Data.csproj
```

> API projekt může používat modely a služby z datové knihovny.

---

**Poznámka:**

Nikdy nespouštějte sestavení (`dotnet build`) před vytvořením hlavního řešení v rootu! Jinak se automaticky vytvoří .sln soubory v podsložkách a projekt se zbytečně rozdělí.

---

Tento postup zajistí čistou a přehlednou strukturu pro další vývoj (modely, repozitáře, služby, testy, atd.).

---

## 6. Vytvoření základní struktury složek a souborů v WorkDays.Data

- `Models/WorkDay.cs` – model pro pracovní den
- `Interfaces/IBaseRepository.cs` – generické rozhraní pro repozitáře
- `Interfaces/IWorkDayRepository.cs` – rozhraní pro repozitář WorkDay
- `Repositories/BaseRepository.cs` – základní implementace repozitáře
- `Repositories/WorkDayRepository.cs` – konkrétní repozitář pro WorkDay

## 7. Instalace NuGet balíčků

Do obou projektů (`WorkDays.Data` i `WorkDays.Api`) nainstalujte:

```sh
dotnet add WorkDays.Data/WorkDays.Data.csproj package Microsoft.EntityFrameworkCore

dotnet add WorkDays.Data/WorkDays.Data.csproj package Microsoft.EntityFrameworkCore.Tools

dotnet add WorkDays.Data/WorkDays.Data.csproj package Microsoft.EntityFrameworkCore.Design

dotnet add WorkDays.Data/WorkDays.Data.csproj package Microsoft.Extensions.Configuration

dotnet add WorkDays.Data/WorkDays.Data.csproj package Microsoft.Extensions.Configuration.Json

dotnet add WorkDays.Data/WorkDays.Data.csproj package Npgsql.EntityFrameworkCore.PostgreSQL

dotnet add WorkDays.Api/WorkDays.Api.csproj package Microsoft.EntityFrameworkCore.Tools

dotnet add WorkDays.Api/WorkDays.Api.csproj package Npgsql.EntityFrameworkCore.PostgreSQL
```

> Pro design-time factory a načítání konfigurace jsou potřeba i balíčky `Microsoft.EntityFrameworkCore.Design`, `Microsoft.Extensions.Configuration` a `Microsoft.Extensions.Configuration.Json`.

## 8. Vytvoření DbContextu

V projektu `WorkDays.Data` vytvořte soubor `AppDbContext.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using WorkDays.Data.Models;

namespace WorkDays.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<WorkDay> WorkDays { get; set; }
}
}
```

## 9. Konfigurace DbContextu v API projektu

V `Program.cs` v projektu `WorkDays.Api` přidejte konfiguraci DbContextu:

```csharp
# WorkDays Backend – Poznámky a úskalí

## Na co si dát pozor

- **Design-time soubor pro migrace**
  - Pro správné fungování EF Core migrací je dobré mít `AppDbContextFactory` v projektu, který načítá konfiguraci (connection string) i mimo běh aplikace.
  - Pokud měníš umístění `appsettings.json`, uprav i `SetBasePath` v tomto souboru.

- **Migrace a práce s databází**
  - Migrace spouštěj přes `dotnet ef migrations add <NázevMigrace>` a `dotnet ef database update` - * Globální příkazy dotnet - nutno instalovat*
    vždy ve správném projektu (většinou `WorkDays.Data`) - `dotnet cd WorkDays.Data`.
  - PM package manager ve VS2022
    ```cli
    dotnet add-migration InitialCreate
    // následně pokud je vše v pořádku
    dotnet update-database
    ```

  - Pokud narazíš na chybu s `DateTimeKind`, zkontroluj, že všechny `DateTime` hodnoty jsou v UTC (ideálně s `Z` v JSONu).
  - Pro typy času používej v modelech `TimeOnly` (ne `DateTime`), v JSONu pak formát `"HH:mm:ss"`.
  - Enumy mapuj na string v DTO, v DB může být int.

- **DTO a mapování**
  - DTO (Data Transfer Objects) oddělují vnitřní model od API – při změně modelu vždy aktualizuj i mapování v helperu.
  - Pokud přidáš nové pole (např. `note`, `tasks`, `pickup`), nezapomeň na migraci, úpravu DTO a mapování.

- **Testování API**
  - Pro testování používej `.http` soubor nebo nástroje jako Postman/Thunder Client.
  - Vždy ověř, že formát dat odpovídá očekávaným typům v backendu.
  - Pokud nechceš přidávat zbatečně balíčky, tak nejvhodnější volba je `[jméno-projektu].http`
- **Connection string a konfigurace**
  - Pro lokální vývoj používej User Secrets nebo `appsettings.Development.json`.
  - Pro produkci (např. Heroku) nastav connection string přes proměnné prostředí.

- **Obecné rady**
  - Po každé změně modelu spusť novou migraci.
  - Pokud narazíš na chybu s migrací, zkontroluj, zda je DB ve správném stavu (někdy je potřeba dropnout tabulku ručně).
  - Vždy používej UTC pro datum/čas, pokud není důvod jinak.

---

Pro detailní postupy viz sekce níže nebo samostatné soubory (`UseSecrets.md`).

## Testování API pomocí .http souboru - OpenApi

1. V kořenové složce projektu vytvoř soubor `WorkDaysApi.http` s následujícím obsahem:

```http
@WorkDays.Api_HostAddress = https://localhost:7194
@WorkDays.Api_Http_HostAddress = http://localhost:5287

GET {{WorkDays.Api_HostAddress}}/api/workday

###
POST {{WorkDays.Api_HostAddress}}/api/workday
Content-Type: application/json

{
    "date": "2025-09-06T01:00:00Z",
    "startTime": "09:00:00Z",
    "endTime": "17:00:00Z",
    "break": "00:30:00Z",
    "isHoliday": false,
    "type": 0
}
###
POST {{WorkDays.Api_HostAddress}}/api/workday
Content-Type: application/json

{
    "date": "2025-09-08T01:00:00Z",
    "startTime": "08:00:00",
    "endTime": "16:00:00",
    "break": "00:30:00",
    "isHoliday": false,
    "type": 0
}
###
@id=2
GET {{WorkDays.Api_HostAddress}}/api/workday/{{id}}


###

PUT {{WorkDays.Api_HostAddress}}/api/workday/{{id}}
Content-Type: application/json

{
    "workDayId": 5,
    "date": "2025-09-06T01:00:00Z",
    "startTime": "10:30:00",
    "endTime": "16:00:00",
    "break": "00:20:00",
    "isHoliday": false,
    "type": 0,
    "totalHours": "00:00:00"
}

###
@id=5
DELETE {{WorkDays.Api_HostAddress}}/api/workday/{{id}}
###
POST {{WorkDays.Api_HostAddress}}/api/workday
Content-Type: application/json

{
   "workDayId": 5,
    "date": "2025-09-08T01:00:00Z",
    "startTime": "10:30:00",
    "endTime": "16:00:00",
    "break": "00:20:00",
    "isHoliday": false,
    "type": 3,
    "totalHours": "00:00:00"
}

###

PUT {{WorkDays.Api_HostAddress}}/api/workday/{{id}}
Content-Type: application/json

{
    "workDayId": 1,
    "date": "2025-09-06T01:00:00Z",
    "startTime": "08:30:00",
    "endTime": "16:00:00",
    "break": "00:30:00",
    "isHoliday": false,
    "type": 0,
    "totalHours": "00:00:00"
}

###

@id=1
GET {{WorkDays.Api_HostAddress}}/api/workday/{{id}}

###
PUT {{WorkDays.Api_HostAddress}}/api/workday/{{id}}
Content-Type: application/json

{
    "workDayId": 1,
    "date": "2025-09-06T01:00:00Z",
    "startTime": "08:30:00",
    "endTime": "16:00:00",
    "break": "00:30:00",
    "isHoliday": false,
    "type": 0,
    "totalHours": "00:00:00"
}
###
# HTTP requests for WorkDays.Api
# http://localhost:5287/api/workday/1
@id=7
GET {{WorkDays.Api_Http_HostAddress}}/api/workday/{{id}}

###

# http request update http://localhost:5287/api/workday/10

PUT {{WorkDays.Api_Http_HostAddress}}/api/workday/{{id}}
Content-Type: application/json

{
    "workDayId": 7,
    "date": "2025-09-11T01:00:00Z",
    "startTime": "11:00:00",
    "endTime": "16:00:00",
    "break": "00:30:00",
    "isHoliday": false,
    "type": "ShortWorkDay"

}

###

GET {{WorkDays.Api_Http_HostAddress}}/api/workday

###
DELETE {{WorkDays.Api_Http_HostAddress}}/api/workday/{{id}}
```

---

1. Otevři tento soubor ve VS Code a použij tlačítko "Send Request" nad jednotlivými HTTP požadavky pro jejich odeslání.
2. Až budeš mít otestovány všechny endpoity, pak tvá aplikace je připravena na nasazení na HEROKU - snadné a automatizované

## Nasazení na HEROKU

- Jelikož používám skutečnou(fyzickou Db) - PostgreSQL, tak jsem nemusel přidávat do projektu žádné Add-Ons
  jako je právě PostgreSql na AWS
- Nasazení je snadné:
    1. Craete New projekt
        1a. [your-unique-app-name]
        2b. Add pipeline => Create a 'New pipe-line'
            - zvolte vhodný a výstižný název pro svou službu
            - projekt nahrajte na git, aby jste ho mohli propojit
    2. Musíte mít naistalované `Heroku Cli` - na vše vás průvoce navede
        - projekt nahrajte na git, aby jste ho mohli propojit
    3. Nastavte automatické buildy při každém pull requestu
       - znamená to, že po každé změně se váš projekt znovu sestavý
- Nyní se spustí Váš první Deploy na Heroku a  na konci na Vás čeká adresa,
   na  které běží Váš backend
