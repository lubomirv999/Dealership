namespace Dealership.Data
{
    using Microsoft.Extensions.Configuration;
    using System.Data.SqlClient;

    public static class DealershipDbContextExtentions
    {
        public static bool DatabaseExists(IConfiguration configuration)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
