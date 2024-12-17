@echo off
REM "command" -c WebContext --output-dir Migrations/SQLServer
REM "command" -c PostgreSQLWebContext --output-dir Migrations/PostgreSQL

REM Check if there is only one parameter
if "%~1" equ "" (
    echo Error: No paramter provided
    exit /b 1
)

if "%~2" neq "" (
    echo Error: Too many parameters provided
    exit /b 1
)

dotnet ef migrations add %~1 -c SQLServerWebContext --output-dir Migrations/SQLServer
dotnet ef migrations add %~1 -c PostgreSQLWebContext --output-dir Migrations/PostgreSQL