using SqlKata.Execution;
using SqlKata.Compilers;
using MySql.Data.MySqlClient;
using dotenv.net;

namespace RESTful.Database
{
  public class DatabaseConnection
  {
    public DatabaseConnection() { }
    public QueryFactory Connect()
    {
      DotEnv.Load();
      var connection = new MySqlConnection(
        System.Environment.GetEnvironmentVariable("DB_CONNECTION")
      );
      var compiler = new SqlServerCompiler();
      return new QueryFactory(connection, compiler);
    }
  }
}
