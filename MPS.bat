@echo off
if "%1" equ "" (
    echo Usage: MPS.bat [options] [file]
    exit /b 0
)

if "%2" equ "" (
    echo Usage: MPS.bat [options] [file]
    exit /b 0
)
dotnet new %1 -o %2
dotnet sln add %2/%2.csproj
echo "Added %2 to solution"