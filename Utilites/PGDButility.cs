
using System.Data;
using Npgsql;
using server.Models;

namespace server.Utilities;

public class PGDBUtilityAPI {

    public static ILogger log;

    public static string connectionString = CUtility.getConfig().GetConnectionString("PostgresConnStringAPI");

    public static string GetConnectionString(){

        if(connectionString == null || connectionString == string.Empty){

            connectionString = CUtility.getConfig().GetConnectionString("PostgresConnStringAPI");
        }

        return connectionString;
    }

    public static void resetConnectionString(){
        connectionString = string.Empty;
    }

    public static DataTable GetDataTable(string query){
        string conString = GetConnectionString();
        DataTable dt = null;

        using (NpgsqlConnection postgresConn = new NpgsqlConnection(conString))
        {
            IDbDataAdapter daPgsql = new NpgsqlDataAdapter(query, postgresConn);

            try{
                DataSet dsPg = new DataSet();
                log.LogDebug(query);
                daPgsql.Fill(dsPg);
                if(dsPg.Tables.Count>0){
                    dt = dsPg.Tables[0];
                }
            }
            catch(Exception ex){
                log.LogError("Query:{0} ::> {1}||{2}", query, ex.Message, ex.StackTrace);
            }
            finally{
                if(postgresConn.State == ConnectionState.Open){
                    postgresConn.Close();
                }
            }
            
        }
        return dt;
    }

    public static int ExecuteCommand(string query){

        Int32 rowAffected = -1;
        string conString = GetConnectionString();

        using (NpgsqlConnection postgresConn = new NpgsqlConnection(conString)){

            try{
                NpgsqlCommand pgCommand = new NpgsqlCommand(query, postgresConn);
                pgCommand.Connection.Open();
                log.LogDebug(query);
                rowAffected = pgCommand.ExecuteNonQuery();
                pgCommand.Connection.Close();
            }
            catch(Exception ex){
                log.LogError("Query:{0} ::> {1}||{2}", query, ex.Message, ex.StackTrace);
            }
            finally{
                if(postgresConn.State == ConnectionState.Open){
                    postgresConn.Close();
                }
            }
        }
        return rowAffected;

    }


    public static string GetStringFromTable(string query){
        string retval = string.Empty;
        try{
            DataTable dt = GetDataTable(query);
            if(dt != null && dt.Rows.Count>0){
                retval = dt.Rows[0][0].ToString();

            }
            
        }
        catch(Exception ex){
            log.LogError("Query:{0} ::>> {1} || {2}", query, ex.Message, ex.StackTrace);
        }
        return retval;
    }


//Get Column Data of Single Row
    public static TokenModel getRowFromTable(string query)
    {
        string connectionString = GetConnectionString(); // Replace with your actual connection string

        // SQL command to retrieve a single row (example query)
        string sqlCommandText = query;
        TokenModel tokenModel = null;

        // Create and open a connection
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            // Create a command with the SQL query and the connection
            using (NpgsqlCommand command = new NpgsqlCommand(sqlCommandText, connection))
            {
                // Execute the SQL command and retrieve a data reader
                try
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    // Check if there are rows
                    if (reader.HasRows)
                    {
                        // Read the first row
                        reader.Read();

                        // Access data from columns by column name or index
                        string Name = reader.GetString(reader.GetOrdinal("name"));
                        string Department = reader.GetString(reader.GetOrdinal("department"));
                        tokenModel = new TokenModel()
                        {
                            name = Name,
                            role = Department
                        };

                    }
                }
                }
                catch(Exception ex){
                log.LogError("Query:{0} ::> {1}||{2}", query, ex.Message, ex.StackTrace);
                }
                finally{
                    if(connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }
        return tokenModel;
    }


//Run the Update Query for Dashboard
    public static bool RunUpdateQuery(string query)
    {
        string connectionString = GetConnectionString();
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                try{
                    command.ExecuteNonQuery();
                }
                catch(Exception ex){
                    log.LogError("Query:{0} ::> {1}||{2}", query, ex.Message, ex.StackTrace);
                    return false;
                }
                finally{
                    if(connection.State == ConnectionState.Open){
                        connection.Close();
                    }
                    
                }
            }
        }
        return true;
    }


//Get All Json Data From Dashboard
    public static List<string> GetDashboardJsonData(string query){
        // string retval = string.Empty;
        List<string> list = new List<string>();
        try{
            DataTable dt = GetDataTable(query);
            if(dt != null && dt.Rows.Count>0){
                for(int i = 0; i < 3 ; i++)
                {
                    string retval = dt.Rows[0][i].ToString();
                    list.Add(retval);
                }

            }
            
        }
        catch(Exception ex){
            log.LogError("Query:{0} ::>> {1} || {2}", query, ex.Message, ex.StackTrace);
        }
        return list;
    }



    //Get User Detail From Table
    public static UserModel GetUserFromTable(string query)
    {
        string connectionString = GetConnectionString();
        string sqlCommandText = query;
        UserModel userModel = null;
        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(sqlCommandText, connection))
            {
                try
                {
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();

                        string Name = reader.GetString(reader.GetOrdinal("name"));
                        int age = (int)reader.GetValue(reader.GetOrdinal("age"));
                        // string adhaar = reader.GetString(reader.GetOrdinal("adhaar"));
                        string adhaar = null; // Initialize adhaar variable with null

                        int adhaarOrdinal = reader.GetOrdinal("adhaar");
                        
                        if (!reader.IsDBNull(adhaarOrdinal))
                        {
                            adhaar = reader.GetString(adhaarOrdinal);
                        }
                        string email = reader.GetString(reader.GetOrdinal("email"));
                        string gender = reader.GetString(reader.GetOrdinal("gender"));
                        userModel = new UserModel()
                        {
                            name = Name,
                            age = age,
                            adhaar = adhaar,
                            email = email,
                            gender = gender

                        };

                    }
                }
                }
                catch(Exception ex){
                log.LogError("Query:{0} ::> {1}||{2}", query, ex.Message, ex.StackTrace);
                }
                finally{
                    if(connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
                
            }
        }
        return userModel;
    }



}
