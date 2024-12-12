# Instruction:

## To learn about Entity FrameworkCore with SQLServer and PostgreSQL simultaenously:

1. Set Connection Strings:

- Go to EFC-Main/appsettings.json and then change the connection strings inside of it

2. Choose which databases you want to use:

- Go to Examples/ClassExamples.cs file
- Go to ClassExamples static class
- Choose the "Databases" you want to work with by setting the connection you pass in the \_unitOfWork variable:

* If you want to only use SQLServer, simply pass in "new EFSQLServerConnection()"
* If you want to only use PostgreSQL, simply pass in "new EFPostgresqlConnection()

- If you want to use both of them, pass in "new EFSQLServerConnection(), new EFPostgresqlConnection()"

3. Restore projects's dependencies:

- Run `dotnet restore` to restore all dependencies

4. Run the program:

- `cd EFC-Main`
- `dotnet run`
