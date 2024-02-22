using System.Data.SqlClient;
using VendingMachine.DateObjectsWorkers;
using VendingMachine.DefaultContinueFunctions;
using VendingMachine.SQLcomunication;

namespace VendingMachine.WorkerMenu.SubMenus
{
    internal class WorkersWorkerMenu :DefaultOptions
    {
        List<WorkerObj> workersList = ISQLcomunication.GetList(GetWorkersList, "Workers");



        /// <summary>
        /// This is logic behind workers options menu. Uses switch statements based on user input using: 'WorkersMenuLook()' method.
        /// </summary>
        /// <param name="name">Name of worker currently using worker menu.</param>
        /// <param name="worker">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>
        public void WorkersMenuInterface(string name, WorkerObj worker)
        {
            Console.Clear();
            string workerInput = WorkersMenuLook(name);
            bool workersToken = true;
            bool firstRun = true;


            if (workerInput.ToLower() != "x")
            {
                while (workersToken == true)
                { 
                if (firstRun != true) { 
                Console.Clear();
                workerInput = WorkersMenuLook(name);
                    }
                firstRun = false;

                    switch (workerInput.ToLower())
                    {
                        case "s":
                            Console.Clear();
                            ShowWorkers();
                            worker.SendAction("Looked at workers list. ");
                            Console.WriteLine("----------------------\n");
                            ToContinue();
                            break;
                        case "a":
                            Console.Clear();
                            AddNewWorker(worker);
                            break;
                        case "d":
                            Console.Clear();
                            DeleteWorker(worker);
                            break;
                        case "e":
                            Console.Clear();
                            EditWorkerMenuInterface(worker);
                            break;
                        case "x":
                            workersToken = false;
                            break;
                        default:
                            DefaultChoice(workerInput);
                            break;
                    }
                }
            }

        }

        /// <summary>
        /// Has two functions. One: workers options menu look and asks user for input.
        /// </summary>
        /// <returns>Input of client used in: 'WorkersMenuInterface()' method.</returns>
        private string WorkersMenuLook(string name)
        {
            string options = $"s: Show all workers. \n" +
                             $"a: Add new worker. \n" +
                             $"d: Delete a worker.\n" +
                             $"e: Edit worker date. ";

            DefaultMenu("WORKER OPTIONS", name, options, true, true);
            string workerInput = Console.ReadLine();
            return workerInput;

        }

        /// <summary>
        /// Allows worker to add new worker.
        /// </summary>
        /// <param name="worker">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>>
        private void AddNewWorker(WorkerObj worker)
        {
            
            DefaultMenu("-","", "Input new worker information.",false,true);
            Console.Write("Input name and surname: ");
            string newNameSurname = Console.ReadLine();
            if(newNameSurname.ToLower() == "x") { Console.Clear(); Console.WriteLine("Add new worker operation was stopped.\n"); ToContinue(); return; }
            Console.WriteLine(" ");
            Console.Write("Input login: ");
            string newLogin = Console.ReadLine();
            if (newLogin.ToLower() == "x") { Console.Clear(); Console.WriteLine("Add new worker operation was stopped.\n"); ToContinue(); return; }
            Console.WriteLine(" ");
            Console.Write("Input password: ");
            string newPassword = Console.ReadLine();
            if (newPassword.ToLower() == "x") { Console.Clear(); Console.WriteLine("Add new worker operation was stopped.\n"); ToContinue(); return; }
            Console.WriteLine(" ");
            ISQLcomunication.SendData($"INSERT INTO Workers VALUES ('{newNameSurname}', '{newLogin}', '{newPassword}')");
            worker.SendAction("Added new worker.\n");
            workersList = ISQLcomunication.GetList(GetWorkersList, "Workers");
            
            ToContinue();
        }

        /// <summary>
        /// Public static method used to get list of workers.
        /// </summary>
        /// <returns>New 'WorkerObj' object that is automatically added to list called: 'workersList'.</returns>
        public static WorkerObj GetWorkersList(SqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            string nameSurname = reader.GetString(1);
            string login = reader.GetString(2);
            string password = reader.GetString(3);

            return new WorkerObj(id,nameSurname,login,password);
        }


        /// <summary>
        /// Shows all workers on: 'workerslist' list.
        /// </summary>
        private void ShowWorkers()
        {
            foreach(WorkerObj worker in workersList)
            {
                worker.SayHello();
            }
            
        }

        /// <summary>
        /// Allows worker to delete other workers.
        /// </summary>
        /// <param name="worker">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>
        private void DeleteWorker(WorkerObj worker)
        {
            bool deleteToken = true;

            string options = "Input ID of a worker you want to delete. ";

            while (deleteToken == true) { 
                Console.Clear();
                ShowWorkers();
                Console.WriteLine(" ");
                DefaultMenu("-"," ", options, false, true);
                string worId = Console.ReadLine();

                if (worId.ToLower() == "x") { deleteToken = false; break; }
                if (IsThereAWorker(worId) == true)
                    {
                        Console.Clear();
                        Console.WriteLine("Are you sure you want to delete this worker: \n");
                        GetWorkerId(int.Parse(worId));
                        Console.WriteLine(" ");
                        Console.WriteLine("y: for yes.");
                        Console.WriteLine("n: for no.");
                        Console.WriteLine(" ");
                        Console.Write("Input: ");
                        string choice = Console.ReadLine();
                        if (choice.ToLower() == "n")
                        {
                            Console.Clear();
                            Console.WriteLine("Worker was not deleted. \n");
                            ToContinue();
                            deleteToken = false;
                            break;
                        }
                        else if (choice.ToLower() == "y")
                        {
                            Console.Clear();
                            worker.SendAction($"Deleted worker with id: {worId} from worker list. ");
                            RemoveWorker(int.Parse(worId));
                            ISQLcomunication.SendData($"delete Workers where [Worker ID] = {int.Parse(worId)}");
                            Console.WriteLine("Worker has been deleted. \n");
                            ToContinue();
                            deleteToken = false;
                            break;
                        }
                        else { DefaultChoice(choice); }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"There is no ID: '{worId}' on workers list. \n");
                        ToContinue();

                    }
                
               
 
                
            }
        }

        /// <summary>
        /// Checks if there is worker with ID inputed by worker.
        /// </summary>
        /// <param name="id">ID of worker you are looking for.</param>
        /// <returns> 'true' if there is a worker, 'false' if there is no worker with inputed ID.</returns>
        private bool IsThereAWorker(string id)
        {
            foreach(var worker in workersList)
            {
                try
                {
                    if (worker.workerId == int.Parse(id))
                    {
                        return true;
                    }
                }
                catch (Exception ex) { return false; }
            }
            return false;
        }


        /// <summary>
        /// Shows only one worker from: 'workersList' list.
        /// </summary>
        /// <param name="id">ID of worker you want to display informations about.</param>
        private void GetWorkerId(int id)
        {
            foreach (var worker in workersList)
            {
                if (worker.workerId == id)
                {
                    worker.SayHello();
                }
            }
        }


        /// <summary>
        /// Shows you chosen information of worker.
        /// </summary>
        /// <param name="id">ID of worker you want to display information about.</param>
        /// <param name="option">To get name and surname: 'name and surname', login: 'login', password: 'password'.</param>
        /// <returns>Information you chose.</returns>
        private string GetWorkerDate(int id, string option)
        {
            foreach (var worker in workersList)
            {
                if (worker.workerId == id)
                {
                    if(option == "name and surname") { return worker.workerNameSurname; }
                    if(option == "login") { return worker.login; }
                    if(option == "password") { return worker.password; }
                }
            }   return null;
        }

        /// <summary>
        /// Used to remove worker from: 'workersList' list.
        /// </summary>
        /// <param name="id">ID of worker you want to remove.</param>
        private void RemoveWorker(int id)
        {
            for (int i = workersList.Count - 1; i >= 0; i--)
            {
                if (workersList[i].workerId == id)
                {
                    workersList.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Used to change information/s about worker.
        /// </summary>
        /// <param name="id">ID of worker whose information/s about you want to change.</param>
        /// <param name="option">To change name and surname: 'name and surname', login: 'login', password: 'password'.</param>
        /// <param name="newDate">New informations you want worker to have.</param>
        /// <param name="workerDif">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>
        private void EditWorker(int id, string option,string newDate, WorkerObj workerDif)
        {
            foreach (var worker in workersList)
            {
                if (worker.workerId == id)
                {
                    worker.EditDate(option,newDate, workerDif);
                }
            }
        }


        /// <summary>
        /// This is logic behind worker edit menu. Uses switch statements based on user input using: 'EditWorkerMenuLook()' method.
        /// </summary>
        /// <param name="worker">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>
        private void EditWorkerMenuInterface(WorkerObj worker)
        {
            bool editToken = true;

            string options = "Input ID of a worker you want to edit information about. ";

            while (editToken == true) { 
            
                Console.Clear();
                ShowWorkers();
                Console.WriteLine(" ");
                DefaultMenu("-", " ", options, false, true);
                string worId = Console.ReadLine();

                if (worId.ToLower() == "x") { editToken = false; break; }
                if (IsThereAWorker(worId) == true)
                {
                    Console.Clear();
                    GetWorkerId(int.Parse(worId));
                    string workerInput = EditWorkerMenuLook();

                    switch (workerInput.ToLower())
                    {
                        case "a":
                            AllDate(int.Parse(worId),worker);
                            break;
                        case "n":
                            EditMethod(int.Parse(worId), "name and surname", worker);
                            break;
                        case "l":
                            EditMethod(int.Parse(worId), "login",worker);
                            break;
                        case "p":
                            EditMethod(int.Parse(worId), "password", worker);
                            break;
                        case "x":
                            break;
                        default:
                            DefaultChoice(workerInput);
                            break;
                    }

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"There is no ID: {worId} on workers list. \n");
                    ToContinue();

                }


            }
        }

        /// <summary>
        /// Has two functions. One: worker edit menu look and asks user for input.
        /// </summary>
        /// <returns>Input of client used in: 'EditWorkerMenuInterface()' method.</returns>
        private string EditWorkerMenuLook()
        {
            string options = "What date about worker you want to edit? \n" +
                             "a: All date. \n"+
                             "n: Name and surname. \n" +
                             "l: Login. \n" +
                             "p: Password. ";
            DefaultMenu("-"," ",options,false,true);
            string choice =Console.ReadLine();
            return choice;

        }

        /// <summary>
        /// Method used to change all informations about worker.
        /// </summary>
        /// <param name="id">ID of worker whose informations about you want to change.</param>
        /// <param name="worker">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>
        private void AllDate(int id, WorkerObj worker)
        {
            List<string> list = new List<string>() { "name and surname", "login", "password" };

            foreach (string s in list)
            {
                EditMethod(id, s, worker);
            }

        }

        /// <summary>
        /// Method used to get new informations about worker and changing them using: 'EditWorker()' method. 
        /// </summary>
        /// <param name="id">ID of worker whose information about you want to change.</param>
        /// <param name="option">To change name and surname: 'name and surname', login: 'login', password: 'password'.</param>
        /// <param name="worker">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>
        private void EditMethod(int id, string option,WorkerObj worker)
        {   
            
                Console.Clear();
                Console.WriteLine($"Current {option}: {GetWorkerDate(id, option)}. \n");
                Console.WriteLine("x: End operations.\n");
                Console.Write($"Input new {option}: ");
                string newDate = Console.ReadLine();
                if (newDate.ToLower() == "x") { Console.Clear(); Console.WriteLine($"Worker {option} was not changed. \n"); ToContinue(); return; }
                Console.Clear() ;
                EditWorker(id,option, newDate,worker);
                Console.WriteLine($"Worker {option} is now: {GetWorkerDate(id, option)}.\n");
                ToContinue();
             
        }

        
        

    }
}
