@echo off

REM Check if there is only one parameter
if "%~1" neq "" (
    echo Error: Too many parameters provided
    exit /b 1
)

echo Migration List for SQLServer:
dotnet ef migrations list -c SQLServerWebContext
echo Migration List for PostgreSQL:
dotnet ef migrations list -c PostgreSQLWebContext