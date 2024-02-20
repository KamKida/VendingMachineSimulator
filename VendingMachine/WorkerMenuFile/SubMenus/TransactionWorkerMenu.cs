using System.Data.SqlClient;
using VendingMachine.SQLcomunication;
using VendingMachine.DefaultContinueFunctions;
using VendingMachine.DateObjectsWorkers;


namespace VendingMachine.WorkerMenu.SubMenus
{
    internal class TransactionWorkerMenu : DefaultOptions
    {




        /// <summary>
        /// This is logic behind worker transactions history menu. Uses switch statements based on user input using: 'TransactionMenuLook()' method.
        /// </summary>
        /// <param name="name">Name of worker currently using worker menu.</param>
        /// <param name="worker">'WorkerObj' object used for send actions to database using: 'SendAction()' method.</param>
        public void TransactionsMenuInterface(string name, WorkerObj worker)
        {
            Console.Clear();
            string workerInput = TransactionMenuLook(name);
            bool transactionsToken = true;
            bool firstRun = true;

            if (workerInput.ToLower() != "x")
            {
                while (transactionsToken == true)
                {
                    if (firstRun != true)
                    {
                        Console.Clear();
                        workerInput = TransactionMenuLook(name);
                    }
                    firstRun = false;


                    switch (workerInput.ToLower())
                    {
                        case "t":
                            Console.Clear();
                            ISQLcomunication.GetDate(TransactionsInfo, "select top 3 [Date Of Transaction], [Lenght Of transaction], Profits from Transactions order by [Transaction ID] desc");
                            worker.SendAction("Looked at last three transactions. ");
                            ToContinue();
                            break;
                        case "a":
                            Console.Clear();
                            ISQLcomunication.GetDate(TransactionsInfo, "select [Date Of Transaction], [Lenght Of transaction], Profits from Transactions order by [Transaction ID] desc");
                            worker.SendAction("Looked at all transactions. ");
                            ToContinue();
                            break;
                        case "c":
                            Console.Clear();
                            DeleteTransacionhistory(worker);
                            break;
                        case "x":
                            transactionsToken = false;
                            break;
                        default:
                            DefaultChoice(workerInput);
                            break;
                    }

                }
            }

        }

        // <summary>
        /// Has two functions. One: determines worker transactions history menu look and asks user for input.
        /// </summary>
        /// <returns>Input of client used in: 'TransactionsMenuInterface()' method.</returns>
        private string TransactionMenuLook(string name)
        {
            string options = $"t: Show last three transactions. \n" +
                             $"a: Show all transactions. \n" +
                             $"c: Delete all history of transactions.";

            DefaultMenu("TRANSACTIONS MENU", name, options, true, true);
            string workerInput = Console.ReadLine();
            return workerInput;
        }

        /// <summary>
        /// Public static method used to show transactions done on vending machine.
        /// </summary>
        /// <returns>String used in: 'GetDate()' method to display results.</returns>
        public static string TransactionsInfo(SqlDataReader reader)
        {
            DateTime date = reader.GetDateTime(0);
            TimeSpan time = reader.GetTimeSpan(1);
            decimal profits = reader.GetDecimal(2);

            return $"Date of transaction: {date.ToString("dd-MM-yyyy")}. \n" +
                   $"Tntransaction length: {time}. \n" +
                   $"Profits from transaction: {Math.Round(profits, 2)} $. \n ";
                                   
        }

        private void DeleteTransacionhistory(WorkerObj worker)
        {
            Console.Clear();
            ISQLcomunication.GetDate(TransactionsInfo, "select [Date Of Transaction], [Lenght Of transaction], Profits from Transactions order by [Transaction ID] desc");
            YesNo("Are you sure you want to delete entire transactions history");
            string choice = Console.ReadLine();
            if (choice.ToLower() == "n")
            {
                Console.Clear();
                Console.WriteLine("Entire transactions history was not deleted. \n");
                ToContinue();
            }
            else if (choice.ToLower() == "y")
            {
                Console.Clear();
                worker.SendAction("Deleted entire history of transactions. ");
                ISQLcomunication.SendDate("DELETE Transactions");
                Console.WriteLine("Full history of transaction has been deleted. \n");
                ToContinue();
            }
            else
            {
                DefaultChoice(choice);
            }
        }
        
    }
}
