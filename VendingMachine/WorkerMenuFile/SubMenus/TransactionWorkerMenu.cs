﻿using System.Data.SqlClient;
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
        /// <param name="worker">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>
        public void TransactionsMenuInterface( WorkerObj worker)
        {
            Console.Clear();
            string workerInput = TransactionMenuLook();
            bool transactionsToken = true;
            bool firstRun = true;

            if (workerInput.ToLower() != "x")
            {
                while (transactionsToken == true)
                {
                    if (firstRun != true)
                    {
                        Console.Clear();
                        workerInput = TransactionMenuLook();
                    }
                    firstRun = false;


                    switch (workerInput.ToLower())
                    {
                        case "t":
                            Console.Clear();
                            ISQLcomunication.GetData(TransactionsInfo, "select [Date Of Transaction], [Lenght Of transaction], Profits from Transactions where [Date Of Transaction] = (select max([Date Of Transaction]) from Transactions)order by [Transaction ID] desc");
                            worker.SendAction("Looked at transactions from last time vending machine was used. ");
                            Console.WriteLine("----------------------");
                            ToContinue();
                            break;
                        case "a":
                            Console.Clear();
                            ISQLcomunication.GetData(TransactionsInfo, "select [Date Of Transaction], [Lenght Of transaction], Profits from Transactions order by [Transaction ID] desc");
                            worker.SendAction("Looked at all transactions. ");
                            Console.WriteLine("----------------------");
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
        private string TransactionMenuLook()
        {
            string options = $"t: Show transactions from last time vending machine was used. \n" +
                             $"a: Show all transactions. \n" +
                             $"c: Delete all history of transactions.";

            DefaultMenu("TRANSACTIONS MENU"," ", options, false, true);
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
                   $"Transaction length: {time}. \n" +
                   $"Profits from transaction: {Math.Round(profits, 2)} $. \n ";
                                   
        }

        /// <summary>
        /// Method with decision if worker wants to delete entire transactions history.
        /// </summary>
        /// <param name="worker">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>

        private void DeleteTransacionhistory(WorkerObj worker)
        {
            Console.Clear();
            ISQLcomunication.GetData(TransactionsInfo, "select [Date Of Transaction], [Lenght Of transaction], Profits from Transactions order by [Transaction ID] desc");
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
                ISQLcomunication.SendData("DELETE Transactions");
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
