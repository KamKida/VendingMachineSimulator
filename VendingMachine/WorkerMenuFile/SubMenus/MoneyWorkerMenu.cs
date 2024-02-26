using System.Data.SqlClient;
using VendingMachine.SQLcomunication;
using VendingMachine.DefaultContinueFunctions;
using VendingMachine.DateObjectsWorkers;

namespace VendingMachine.WorkerMenu.SubMenus
{
    internal class MoneyWorkerMenu : DefaultOptions
    {
        /// <summary>
        /// List with one argument representing current money in vending machine.
        /// </summary>
        List<decimal> moneyInMachine = ISQLcomunication.GetList(GetProfits, "[Money in Machine]");
        

        /// <summary>
        /// This is logic behind worker money menu. Uses switch statements based on user input using: 'MoneyMenuLook()' method.
        /// </summary>
        /// <param name="worker">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>
        public void MoneyMenuInterface( WorkerObj worker)
        {
            Console.Clear();
            string workerInput = MoneyMenuLook();
            bool moneyToken = true;
            bool firstRun = true;

            if(workerInput.ToLower() != "x") {
                while (moneyToken == true)
                {
                    if(firstRun != true) { 
                    Console.Clear();
                    workerInput = MoneyMenuLook();
                    }
                    firstRun = false;

                    switch (workerInput.ToLower())
                    {
                        case "s":
                            Console.Clear();
                            ShowMoney();
                            worker.SendAction("Looked at money in vending machine. ");
                            ToContinue();
                            break;
                        case "w":
                            Console.Clear();
                            WithdraAllMoney(worker);
                            ToContinue();
                            break;
                        case "x":
                            moneyToken = false;
                            break;
                        default:
                            DefaultChoice(workerInput);
                            break;
                    }
                }
            }

        }
        /// <summary>
        /// Has two functions. One: determines worker money menu look and asks user for input.
        /// </summary>
        /// <returns>Input of client used in: 'MoneyMenuInterface()' method.</returns>
        public string MoneyMenuLook()
        {
            string optins = $"s: Show money in vending machine. \n" +
                            $"w: Withdraw all money in vending machine.";

            DefaultMenu("MONEY OPTIONS", " ", optins, false,true);
            string workerInput = Console.ReadLine();
            return workerInput;
        }

        /// <summary>
        /// Public static method used to create list: 'moneyInMachine'.
        /// </summary>

        public static decimal GetProfits(SqlDataReader reader)
        {
            decimal profits = reader.GetDecimal(1);


            return   Math.Round(profits, 2);
        }

        /// <summary>
        /// Shows current money in vending machine.
        /// </summary>
        private void ShowMoney()
        {
            Console.WriteLine($"There is currently: {moneyInMachine[0]} $ in vending machine.");
            Console.WriteLine(" ");

        }

        /// <summary>
        /// Allows worker to withdraw all money from vending machine if there is any.
        /// </summary>
        /// <param name="worker">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>
        private void WithdraAllMoney(WorkerObj worker)
        {
            if(moneyInMachine[0] == 0)
            {
                Console.WriteLine("You can not withdraw money from vending machine because there is 0,00 $ insde. ");
                Console.WriteLine(" ");
                
            }
            else
            {
                while (true)
                {
                    Console.Clear();
                    YesNo($"Would you like to withdraw: {moneyInMachine[0]} $ from vending machine ");
                    string choice = Console.ReadLine();

                    if (choice.ToLower() == "y") {
                        Console.Clear();
                        worker.SendAction($"Withdraw all money: {moneyInMachine[0]} $, from vending machine. ");
                        ISQLcomunication.SendData("UPDATE [Money In Machine] SET [Current Money In Machine] = 0 WHERE ID = 1");
                        Console.WriteLine($"You have withdraw: {moneyInMachine[0]} $, from vending machine. \n");
                        moneyInMachine[0] = 0;
                        break;
                    }
                    else if (choice.ToLower() == "n") { Console.Clear(); Console.WriteLine("Moeny was not taken. \n"); break; }
                    else { Console.Clear(); DefaultChoice(choice); continue; }
                }
            }
        }
    }
}
