using System.Data.SqlClient;
using VendingMachine.DateObjectsWorkers;

namespace VendingMachine.SQLcomunication
{
    internal interface ISQLcomunication
    {
        private const string serverName = "DESKTOP-1R7C1KM"; // <--- Name of a server.
        private const string databaseName = "VendingMachine"; // <--- Name of datebase.

        private const string connectionString = $"Data Source={serverName};Initial Catalog={databaseName};Integrated Security=True;"; //<-- Connection string.


        /// <summary>
        /// Used to create list of objects.
        /// </summary>
        /// <param name="objectBuilder">Hear you need to send public static method with all columns of table you want to be used to create object that will be added to list.</param>
        /// <param name="tableName">Name of table you want date from.</param>
        /// <returns>List of objects.</returns>
        public static List<T> GetList<T>(Func<SqlDataReader, T> objectBuilder, string tableName)
        {
            string query = $"SELECT * FROM {tableName}";
            List<T> resultList = new List<T>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    T item = objectBuilder(reader);
                                    resultList.Add(item);
                                }
                            }
                            else
                            {
                                Console.WriteLine("There is no data in the table. \n");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Something went wrong while trying to download data: {ex.Message}  \n");
                }

                return resultList;
            }
        }

        /// <summary>
        /// Sends query to sql datebase table with changes/new information.
        /// </summary>
        /// <param name="queary">Full sql query with changes/new information and table name.</param>
        public static void SendDate(string query)
        {
            string insertQuery = query;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();


                    try
                    {
                        using (SqlCommand command = new SqlCommand(insertQuery, connection))
                        {

                            command.ExecuteNonQuery();
                        }


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"There was an error while trying to send date to datebase. {ex.Message} \n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was error while trying to connect to datebase. \n");
            }
        }

        /// <summary>
        /// Shows date from datebase without creating new list or object.
        /// </summary>
        /// <param name="objectBuilder">Hear you need to send public static method with all columns of table you want to be displayed.</param>
        /// <param name="query">Full sql query with question to get informations.</param>
        public static void GetDate<T>( Func<SqlDataReader, T> objectBuilder, string query)
        {
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    T item = objectBuilder(reader);

                                    Console.WriteLine(item);
                                }
                            }
                            else
                            {
                                Console.WriteLine("There is no data in the table. \n");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Something went wrong while trying to download data: {ex.Message} \n");
                }

               
            }
        }

        /// <summary>
        /// Used to create new: 'WorkerObj' object.
        /// </summary>
        /// <param name="workerId">ID of worker you want information about from sql database. </param>
        /// <returns>New: 'WorkerObj' object. </returns>
        public static WorkerObj GetWorker(int workerId)
        {
            string query = $"SELECT [Name And Surname], [Worker Login], [Worker Password] FROM Workers WHERE [Worker ID] = {workerId}";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string name = reader.GetString(0);
                                    string login = reader.GetString(1);
                                    string password = reader.GetString(2);

                                    WorkerObj obj = new WorkerObj(workerId, name, login, password);
                                    return obj;
                                }
                            }
                            else
                            {
                                Console.WriteLine("There is no data in the table. \n");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Something went wrong while trying to download data: {ex.Message} \n");
                }

                return null;
            }
        }

        

    } 
}