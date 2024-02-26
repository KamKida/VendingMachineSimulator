using System.Data.SqlClient;
using VendingMachine.DefaultContinueFunctions;
using VendingMachine.MenuIterface;
using VendingMachine.ProdClientObj;
using VendingMachine.SQLcomunication;
using VendingMachine.WorkerMenuFile;


namespace VendingMachine.MenuIterfaceUser
{
    internal class ClientMenu: ProductDisplay
    {
        ClientObj newClient = new ClientObj();
        CheckerWorker checkerWorker = new CheckerWorker();
        public static bool adminUse = false;

        /// <summary>
        /// This is logic behind client menu. Uses switch statements based on user input using: 'UserMenuLook()' method.
        /// </summary>
        public void UserMenuInterface()
        {
            newClient.SetUserFunds();

            while (true)
            {
                Console.Clear();
                ShowProducts();
                string userInput = UserMenuLook();

                if (checkerWorker.workersIdList.Contains(userInput))
                {
                    Console.Clear();
                    checkerWorker.loginCheck(int.Parse(userInput));
                    break;
                }
                else
                {

                    switch (userInput.ToLower())
                    {
                        case "a":
                            Console.Clear();
                            newClient.AddFunds();
                            break;
                        case "x":
                            newClient.EndOperationsClient();
                            break;
                        default:
                            BuyProduct(userInput);
                            break;
                    }
                }
                

            }

        }

        /// <summary>
        /// Has two functions. One: determines client menu look and asks user for input.
        /// </summary>
        /// <returns>Input of client used in: 'UserMenuInterface()' method.</returns>
        private string UserMenuLook()
    {

            string options = $"You have: {Math.Round(newClient.userFunds, 2)} $. \n" +
                             $"You spend: {newClient.moneySpend} $. \n" +
                             $"a: Add funds. " ;
                 DefaultMenu("MENU", " ", options, false, true);
                string userInput = Console.ReadLine();
                return userInput;
            
    }

        /// <summary>
        /// Allows client to buy product after checking if client has enough money and if there is enough product in vending machine.
        /// </summary>
        public void BuyProduct(string userInput)
        {
            bool noResult = false;
            try
            {
                foreach (var product in products)
                {
                    if (product.place == int.Parse(userInput))
                    {
                        noResult = true;
                        if (product.price <= newClient.userFunds)
                        {
                            if (product.amount > 0)
                            {
                                product.ChangeDate("amount", (product.amount - 1).ToString()); 
                                newClient.TransactonMechanism(product.price, product.place, product.name);
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine($"Sorry, there is no more: '{product.name}' in vending machine. \n");
                                ToContinue();
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine($"Sorry you are short: {product.price - newClient.userFunds} $ to buy: '{product.name}'. \n");
                            ToContinue();
                        }
                    }
                }
                if (!noResult)
                {
                    Console.Clear();
                    Console.WriteLine($"There is no product with place: '{userInput}' in machine. \n");
                    ToContinue();
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                DefaultChoice(userInput);
            }
        }
    }
}
