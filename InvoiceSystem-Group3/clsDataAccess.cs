using System;
using System.Data;
using System.Data.OleDb;
using InvoiceSystem_Group3;

namespace InvoiceSystem_Group3
{

    /// <summary>
    /// Provides methods for interacting with a Microsoft Access database, including executing SQL queries and
    /// retrieving results. This class establishes and manages a connection to the database.
    /// </summary>
    /// <remarks>The <see cref="clsDataAccess"/> class is designed to work with a Microsoft Access database
    /// file named "Invoice.accdb" located in the current working directory. It supports executing SQL statements for
    /// data retrieval, scalar operations, and non-query commands such as INSERT, UPDATE, and DELETE.  The class uses
    /// the Microsoft Access Database Engine (ACE) OLEDB provider for database connectivity. Ensure that the required
    /// database engine is installed on the system where this class is used.  <para> This class is not thread-safe. If
    /// multiple threads need to access the database, ensure proper synchronization or use separate instances of <see
    /// cref="clsDataAccess"/> for each thread. </para></remarks>
    public class clsDataAccess
    {
        private readonly OleDbConnection conn;

        /// <summary>
        /// Initializes a new instance of the <see cref="clsDataAccess"/> class and establishes a connection to the
        /// database.
        /// </summary>
        /// <remarks>This constructor attempts to create a connection to the "Invoice.accdb" database
        /// located in the current working directory. The connection string uses the Microsoft Access Database Engine
        /// (ACE) OLEDB provider.</remarks>
        /// <exception cref="Exception">Thrown if an error occurs while initializing the database connection. The exception message includes details
        /// about the underlying error.</exception>
        public clsDataAccess()
        {
            try
            {
                string dbPath = $"{Environment.CurrentDirectory}\\Invoice.accdb";
                string sConn = $"Provider=Microsoft.ACE.OLEDB.16.0;Data Source={dbPath};";
                conn = new OleDbConnection(sConn);
            }
            catch (Exception ex)
            {
                throw new Exception("Error initalizing database connection." + ex.Message);
            }
        }


        /// <summary>
        /// Executes the specified SQL statement and retrieves the result as a <see cref="DataSet"/>.
        /// </summary>
        /// <param name="sSQL">The SQL query string to execute. Must be a valid SELECT statement.</param>
        /// <param name="iRetVal">An output parameter that receives the number of rows affected by the query.  This value is set to 0 if no
        /// rows are returned.</param>
        /// <returns>A <see cref="DataSet"/> containing the result of the SQL query. The <see cref="DataSet"/> will be empty if
        /// no rows are returned.</returns>
        /// <exception cref="Exception">Thrown when an error occurs while executing the SQL query. The exception message includes details about the
        /// error.</exception>
        public DataSet ExecuteSQLStatement(string sSQL, ref int iRetVal)
        {
            DataSet ds = new DataSet();

            try
            {
                OleDbDataAdapter da = new OleDbDataAdapter(sSQL, conn);
                iRetVal = da.Fill(ds);
            }
            catch(Exception ex)
            {
                throw new Exception($"Database SELECT error: {ex.Message}");

            }


            return ds;
        }


        /// <summary>
        /// Executes a SQL statement that does not return data (INSERT, UPDATE, DELETE).        /// </summary>
        /// <param name="sSQL"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string ExecuteScalarSQL(string sSQL)
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(sSQL, conn);
                object result = cmd.ExecuteScalar();
                conn.Close();

                return result?.ToString() ?? string.Empty;
            }
            catch(Exception ex)
            {
                conn.Close();
                throw new Exception($"Database SCALAR error: {ex.Message}");
            }

            
        }

        /// <summary>
        /// Executes a non-query SQL statement against the database and returns the number of rows affected.
        /// </summary>
        /// <param name="sSQL">The SQL statement to execute. This should be a valid non-query SQL command, such as an INSERT, UPDATE, or
        /// DELETE statement.</param>
        /// <returns>The number of rows affected by the SQL statement.</returns>
        /// <exception cref="Exception">Thrown if an error occurs while executing the SQL statement. The exception message includes details about
        /// the error.</exception>
        public int ExecuteNonQuery(string sSQL)
        {
            try
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(sSQL, conn);
                int rows = cmd.ExecuteNonQuery();
                conn.Close();

                return rows;
            }
            catch(Exception ex)
            {
                conn.Close();
                throw new Exception($"Database NONQUERY error: {ex.Message}");
            }
        }

        public void TestConnection()
        {
            try
            {
                conn.Open();
                System.Windows.MessageBox.Show("Connection successful!", "Database", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                conn.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Connection failed: {ex.Message}", "Database", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }


    }
}


