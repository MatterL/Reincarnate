#!/usr/bin/env bash
set -euo pipefail

echo "== Reincarnate CI Build =="

git submodule update --init --recursive

dotnet --info
dotnet restore
dotnet build RobustTemplate.sln --configuration Debug --no-restore

echo "Build succeeded."