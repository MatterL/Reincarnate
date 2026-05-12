````powershell
@'
# Troubleshooting

## Git not found

Reopen PowerShell after installing Git. Check:

```powershell
git --version
where git
```

## Python not found or opens Microsoft Store

Disable Windows App Execution Aliases for Python, then install Python from python.org or winget.

Check:

```powershell
python --version
py --version
where python
where py
```

## dotnet not found

Reopen PowerShell after installing the .NET SDK. Check:

```powershell
dotnet --version
dotnet --list-sdks
where dotnet
```

## RobustToolbox folder is missing or empty

You probably cloned without submodules or downloaded a GitHub zip. Run:

```powershell
git submodule sync --recursive
git submodule update --init --recursive --jobs 8
```

## Build says assets/project.assets.json missing

Run restore again:

```powershell
dotnet restore .\RobustTemplate.sln
```

## Client cannot connect

- Make sure the server terminal is still open.
- Wait until the server is ready.
- Try `localhost`.
- Try `127.0.0.1`.
- Allow the app through Windows Firewall on private networks.
- Check whether another process is using the server port.

## Visual Studio does not show startup projects

Open `RobustTemplate.sln` as a solution. Do not open only the folder for first setup.

## OneDrive problems

Clone to `C:\Dev\Reincarnate` instead of Desktop/Documents/OneDrive.
'@ | Set-Content .\Docs\Setup\troubleshooting.md -Encoding UTF8
````