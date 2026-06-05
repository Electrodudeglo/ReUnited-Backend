# ReUnited-Backend

ASP.NET Core backend for the ReUnited application using the MVC pattern.

ReUnited is a lost-and-found application designed to help match people with their lost belongings.

## Tech Stack

- C# / .NET8
- ASP.NET Core
- Supabase Database
- Supabase Auth

## Prerequisites
- Visual Studio 2022
- .NET 8 SDK

## Instructions

Clone this repository. Open ReUnited-Backend.slnx in it's root directory using Visual Studio 2022. 

Run the application with:
```
dotnet run
```

## Configuration
Add your own supabase information in appsettings.Development.json

```
"ConnectionStrings": {
    "DefaultConnection" : "your-connection-string"
    "SupabaseDb": "your-connection-string"
  },
  "Supabase": {
    "URL": "https://your-project.supabase.co",
    "Key": "your-secret-key",
    "Issuer": "your-issuer",
    "Audience": "your-audience"
  }
```
If dotnet user secrets has not beeen initialized:
```
dotnet user-secrets init
```
Use dotnet user secrets
```
dotnet user-secrets set "Supabase:Url" "https://your-project.supabase.co"
dotnet user-secrets set "Supabase:Bucket" "LostItems"
dotnet user-secrets set "Supabase:ApiKey" "your-secret-key"
```