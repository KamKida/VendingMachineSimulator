using System.Data.SqlClient;
using VendingMachine.SQLcomunication;
using VendingMachine.DateObjectsWorkers;
using VendingMachine.MenuIterfaceUser;
using VendingMachine.DefaultContinueFunctions;

namespace VendingMachine.WorkerMenuFile
{
    internal class CheckerWorker : DefaultOptions
    {
        /// <summary>
        /// List of IDs of workers in sql table: 'Workers'.
        /// </summary>
       public  List<string> workersIdList = ISQLcomunication.GetList(GetWorkersIdList, "Workers");
         
        private DateTime startChecker = DateTime.Now;

        public static int MainId;


        /// <summary>
        /// Public static method used to get workers IDs using: 'GetList()' method. 
        /// </summary>
        /// <returns>ID that is automatically added to: 'workerIdList' list. </returns>
        public static string GetWorkersIdList(SqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            

            return id.ToString();
        }


        
        /// <summary>
        /// Login method that asks user for login and password and checks if they are the same as login and password of worker they are trying to login as.
        /// </summary>
        /// <param name="id">ID of worker someone is trying to login as.</param>
        public void loginCheck(int id) {

            WorkerObj worker = ISQLcomunication.GetWorker(id);
            int attempts = 3;
            bool loginSuccses = false;
            

            while(attempts > 0){
                Console.Clear();
                if (attempts == 1) { Console.WriteLine("!!!!This is your last chance!!!! \n"); }
                else
                {
                    Console.WriteLine($"You have: {attempts} attemps to login.\n");
                }

                Console.Write("Pleas enter your login: ");
                string log = Console.ReadLine();
                Console.WriteLine(" ");
                Console.Write("Pleas eneter your password: ");
                string pas = Console.ReadLine();

                if (log == worker.login && pas == worker.password){  loginSuccses = true; break; }
                else { Console.Clear(); Console.WriteLine("Wrong login or password. \n"); attempts--; ToContinue();  }
            
            }

            if(loginSuccses == false) {
                DateTime endChecker = DateTime.Now;
                TimeSpan timeOfFailedLogin = endChecker - startChecker;

                ISQLcomunication.SendDate($"INSERT INTO [Workers History] VALUES ('{this.startChecker.ToString("yyyy-MM-dd")}', null, 'Failed login attempt with id: {id}')");
                Console.WriteLine("You dont have anny more attemps. Ending aplication in 5 seconds.");
                Thread.Sleep(5000);

                Environment.Exit(0);

            }

            else if(loginSuccses == true)
            {
                MainId = id;
                ClientMenu.adminUse = true;
                Console.Clear();
                Console.WriteLine("Login was succesful. In a second you will be send to worker menu.");
                Thread.Sleep(2000);
                

            }

        }

        
        
    }
}
