@echo off

REM Check if there is only one parameter
if "%~1" neq "" (
    echo Error: Too many parameters provided
    exit /b 1
)

dotnet ef database drop -f -c SQLServerWebContext
dotnet ef database drop -f -c PostgreSQLWebContext