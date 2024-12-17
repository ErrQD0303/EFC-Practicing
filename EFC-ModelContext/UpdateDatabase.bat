@echo off

REM Check if there is only one parameter
if "%~2" neq "" (
    echo Error: Too many parameters provided
    exit /b 1
)

dotnet ef database update %~1 -c SQLServerWebContext
dotnet ef database update %~1 -c PostgreSQLWebContext