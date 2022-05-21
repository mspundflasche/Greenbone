using MySql.Data;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

namespace GreenboneApp.Data
{
    public class DBAccess
    {
        public void SaveDataInDatabase<T>(string _sql, T _parameters, string _connectionstring)
        {
            using (IDbConnection connection = new MySqlConnection(_connectionstring))
            {
                connection.Execute(_sql, _parameters);
            }
        }

        public List<T> LoadDataFromDatabase<T, U>(string _sql, U _parameters, string _connectionstring)
        {
            using(IDbConnection connection = new MySqlConnection(_connectionstring))
            {
                var data = connection.Query<T>(_sql, _parameters);

                return data.ToList();
            }
        }
    }
}
