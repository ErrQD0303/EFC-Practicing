@echo off
:: Script to scaffold the model from the database

@REM echo Scaffold Model from SQLServer
@REM echo dotnet ef dbcontext scaffold -o ../EFC-Models/ScaffoldModel/SQLServer -d "Data Source=172.26.144.55,1433;Initial Catalog=WebDB;User ID=sa;Password=Admin@123;TrustServerCertificate=True" "Microsoft.EntityFrameworkCore.SqlServer"
@REM dotnet ef dbcontext scaffold -o ../EFC-Models/ScaffoldModel/SQLServer -d "Data Source=172.26.144.55,1433;Initial Catalog=WebDB;User ID=sa;Password=Admin@123;TrustServerCertificate=True" "Microsoft.EntityFrameworkCore.SqlServer"

@REM echo Scaffold Model from PostgreSQL
@REM echo dotnet ef dbcontext scaffold -o ../EFC-Models/ScaffoldModel/SQLServer -d "Data Source=172.26.144.55,1433;Initial Catalog=WebDB;User ID=sa;Password=Admin@123;TrustServerCertificate=True" "Microsoft.EntityFrameworkCore.SqlServer"
@REM dotnet ef dbcontext scaffold -o ../EFC-Models/ScaffoldModel/PostgreSQL -d "User ID=postgres;Password=shinichi;Host=172.26.144.55;Port=5433;Database=WebDB" "Npgsql.EntityFrameworkCore.PostgreSQL"

echo Scaffold Model from SQLServer for xtlab
echo 'dotnet ef dbcontext scaffold -o ./ScaffoldModel -d "Data Source=172.26.144.55,1433;Initial Catalog=xtlab;User ID=sa;Password=Admin@123;TrustServerCertificate=True" "Microsoft.EntityFrameworkCore.SqlServer"'
dotnet ef dbcontext scaffold -o ./ScaffoldModel -d "Data Source=172.26.144.55,1433;Initial Catalog=xtlab;User ID=sa;Password=Admin@123;TrustServerCertificate=True" "Microsoft.EntityFrameworkCore.SqlServer"