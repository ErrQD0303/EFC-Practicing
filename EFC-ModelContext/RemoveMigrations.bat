@echo off
REM "command" -c WebContext --output-dir Migrations/SQLServer
REM "command" -c PostgreSQLWebContext --output-dir Migrations/PostgreSQL

REM Check if there is only one parameter
if "%~1" neq "" (
    echo Error: Too many parameters provided
    exit /b 1
)

dotnet ef migrations remove -c SQLServerWebContext
dotnet ef migrations remove -c PostgreSQLWebContext