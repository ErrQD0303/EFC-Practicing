@echo off

set params=
@rem loop through all parameters to form a single string
:loop
set context=%1
shift

@rem check if we have reached the end of the parameters
if "%context%" equ "" (
    goto end
)
if "%params%" equ "" (
    set params=%context%
) else (
    set params=%params% %context%
)
goto loop

:end

echo Migration's Scripts for SQLServer:
echo "dotnet ef migrations script %params% -c SQLServerWebContext"
dotnet ef migrations script %params% -c SQLServerWebContext
echo Migration's Scripts for PostgreSQL:
echo "dotnet ef migrations script %params% -c PostgreSQLWebContext"
dotnet ef migrations script %params% -c PostgreSQLWebContext