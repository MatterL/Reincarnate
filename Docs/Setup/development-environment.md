````powershell
@'
# Development Environment

## Project

Reincarnate, based on RobustToolboxTemplate.

## Required tools

- Git
- Python 3.x
- .NET SDK matching current Robust/SS14 setup guidance or this repo's global.json if present
- Rider, Visual Studio, or VS Code with C# tooling
- YAML tooling

## Clone

```powershell
git clone --recurse-submodules https://github.com/space-wizards/RobustToolboxTemplate.git Reincarnate
cd Reincarnate
```

## Submodules

```powershell
git submodule update --init --recursive
```

## Build

```powershell
dotnet restore .\RobustTemplate.sln
dotnet build .\RobustTemplate.sln -c Debug --no-restore
```

## Run server

```powershell
dotnet run --project Content.Server
```

## Run client

```powershell
dotnet run --project Content.Client
```

## Local connect

Use `localhost` or `127.0.0.1` in the client direct connect flow.
'@ | Set-Content .\Docs\Setup\development-environment.md -Encoding UTF8
````