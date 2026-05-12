$ErrorActionPreference = "Stop"

Write-Host "== Reincarnate CI Build =="

git submodule update --init --recursive

dotnet --info
dotnet restore
dotnet build RobustTemplate.sln --configuration Debug --no-restore

Write-Host "Build succeeded."