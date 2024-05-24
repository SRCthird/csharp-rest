# ASP.NET Core Web API with MySQL, SqlKata, and EF Core Migrations

This project takes the basic webapi template from dotnet and turns it into a full functioning CRUD API using ASP.NET Core with a MySQL database. 
It uses SqlKata to construct queries and Entity Framework Core for database migrations.

## Prerequisites

Before you begin, ensure you have the following installed:
- .NET 8.0 SDK or later
- MySQL/MariaDB Server
- Visual Studio 2019 (or dotnet-sdk and aspnet-runtime if you prefer to use the command line) 
- EF Core CLI tools

## Getting Started

To get the project up and running on your local machine, follow these steps:

1. **Clone the Repository**

   ```bash
   git clone https://github.com/SRCthird/csharp-rest.git
   cd csharp-rest
   ```
2. **Set up the Database**

   Ensure that MySQL or MariaDB is running on your machine. Create a new database for this project.

   ```sql
   CREATE DATABASE yourdbname;
   ```

3. **Update Database Connection**

   Open `.env` and update the connection string with your database details:

   ```bash
     DB_CONNECTION="server=localhost;port=3306;database=yourdbname;user=yourusername;password=yourpassword"
   ```

4. **Run Migrations**

   Execute the following command to apply migrations to the database:

   ```bash
   dotnet ef database update
   ```

5. **Run the Application**

   Start the server using the .NET CLI:

   ```bash
   dotnet run
   ```

   The API will be available at `http://localhost:5000`.

## Project Structure

- `Controllers/` - Contains API controllers that handle HTTP requests.
- `Database/Models/` - Contains entity classes.
- `Database/connection.cs` - Includes the application DbContext.
- `Database/migration.cs` - Includes the EF Core migrations configuration, and the DBSets.
- `Migrations/` - Contains EF Core migrations files.

## Using SqlKata

This project uses SqlKata to build SQL queries, the connection is made in the DatabaseConnection class. Here's an example of a query in a repository method:

```csharp
var result = _db.Connect().Query("WeatherForecasts")
    .Where("TemperatureC", ">", 25)
    .Get<WeatherForecast>();
```

## CRUD Operations

This API supports the following CRUD operations:

- **Create**: `POST /weatherforecast`
- **Read**: `GET /weatherforecast` and `GET /weatherforecast/{id}`
- **Update**: `PATCH /weatherforecast/{id}`
- **Delete**: `DELETE /weatherforecast/{id}`

## Contributing

Contributions are welcome. Feel free to fork the repository and submit pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE) file for details.

## Acknowledgements

- ASP.NET Core Team
- SqlKata Documentation
- Entity Framework Core Team
