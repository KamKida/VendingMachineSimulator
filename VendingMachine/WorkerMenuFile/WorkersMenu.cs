using VendingMachine.DateObjectsWorkers;
using VendingMachine.DefaultContinueFunctions;
using VendingMachine.SQLcomunication;
using VendingMachine.WorkerMenu.SubMenus;
using VendingMachine.WorkerMenuFile.SubMenus;

namespace VendingMachine.WorkerMenuFile
{
    internal class WorkersMenu : DefaultOptions
    {
        WorkerObj worker = CheckerWorker.worker;
        TransactionWorkerMenu transactionWorkerMenu = new TransactionWorkerMenu();
        WorkersHistoryMenu workerHistoruMenu = new WorkersHistoryMenu();
        WorkersWorkerMenu workerMenu = new WorkersWorkerMenu();
        MoneyWorkerMenu moneyMenu = new MoneyWorkerMenu();
        ProductsWorkerMenu productMenu = new ProductsWorkerMenu();


        /// <summary>
        /// This is logic behind worker main menu. Uses switch statements based on user input using: 'WorkerMenuLook()' method.
        /// </summary>

        public void WorkerMenuInterface()
        {

            while (true)
            {

                

                Console.Clear();
                string workerInput = WorkerMenuLook(worker.workerNameSurname);
                

                switch (workerInput.ToLower())
                {
                    case "t":
                        transactionWorkerMenu.TransactionsMenuInterface(worker.workerNameSurname, worker);
                        break;
                    case "h":
                        workerHistoruMenu.WorkersHistoryMenuInterface(worker);
                        break;
                    case "w":
                        workerMenu.WorkersMenuInterface(worker.workerNameSurname, worker);
                        break;
                    case "m":
                        moneyMenu.MoneyMenuInterface(worker.workerNameSurname, worker);
                        break;
                    case "p":
                        productMenu.MainProductMenuInterface(worker.workerNameSurname, worker);
                        break;
                    case "x":
                        Console.Clear();
                        worker.EndOperationsWorker();
                        break;
                    default:
                        DefaultChoice(workerInput);
                        break;
                }
            }

        }


        /// <summary>
        /// Has two functions. One: determines worker main menu look and asks user for input.
        /// </summary>
        /// <returns>Input of client used in: ' WorkerMenuInterface()' method.</returns>

        private string WorkerMenuLook(string name)
        {

            string options = $"t: Show history of transactions. \n" +
                             $"h: Show history of workrs. \n"+
                             $"w: Operations with workers. \n" +
                             $"m: Operations with money in vending machine. \n" +
                             $"p: Operations on products. ";
            DefaultMenu("WORKER MAIN MENU", name, options, true, true);
            string workerInput = Console.ReadLine();
            return workerInput;

        }


    }

}

