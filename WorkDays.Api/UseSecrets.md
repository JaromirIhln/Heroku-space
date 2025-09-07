# Práce s User Secrets v projektu WorkDays.Api

User Secrets slouží k bezpečnému ukládání citlivých údajů (např. connection stringů) během vývoje, aniž byste je museli ukládat do zdrojového kódu nebo konfiguračních souborů.

## Aktivace User Secrets

Ve složce projektu `WorkDays.Api` spusťte:

```sh
dotnet user-secrets init
```

Tím se do souboru `.csproj` přidá sekce `<UserSecretsId>`.

## Nastavení connection stringu

Přidejte connection string do User Secrets:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=workdays;Username=postgres;Password=heslo"
```

> Nahraďte hodnoty podle svého prostředí.

## Použití v aplikaci

V `Program.cs` použijte:

```csharp
builder.Configuration.GetConnectionString("DefaultConnection")
```
V  `appsettings.json`  použijte:
```json
"DefaultConnection": ""
```

## Další informace

- [Oficiální dokumentace Microsoft: Safe storage of app secrets in development in ASP.NET Core](https://learn.microsoft.com/aspnet/core/security/app-secrets)

---

**Odkaz na tento soubor je uveden v README.md v sekci nastavení connection stringu.**
