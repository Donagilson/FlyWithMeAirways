using Microsoft.Data.SqlClient;
using System.Configuration;

namespace AirlineManagement.Repository
{
    public class AdminRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["CsWinSql"].ConnectionString;

        public bool Login(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Administrator WHERE Username=@user AND Password=@pass", conn);
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@pass", password);

                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
    }
}
