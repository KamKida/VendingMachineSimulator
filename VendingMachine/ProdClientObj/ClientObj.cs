using VendingMachine.DefaultContinueFunctions;
using VendingMachine.SQLcomunication;

namespace VendingMachine.ProdClientObj
{
    internal class ClientObj: DefaultOptions
    {
        private DateTime startOfTransactions = DateTime.Now;
        public decimal userFunds = 0;
        public decimal moneySpend = 0;
        private bool broughtSomething = false;
        private Dictionary<int, (int amount, string name) > purchases = new Dictionary<int, (int amount, string name)>();


        /// <summary>
        /// Used at the beginning of application to set user funds. Checks if funds are in correct format and in not negative amount using: 'FundChecker()' method.
        /// </summary>
        public void SetUserFunds()
        {
            FundChecker("Pleas input how much money you have: "); 
        }


        /// <summary>
        /// Used to add funds for client. Checks if funds are in correct format and not negative amount using: 'FundChecker()' method.
        /// </summary>
        public void AddFunds()
        {
            FundChecker("How much $ would you like to add to your funds? : ");
        }


        /// <summary>
        /// Check if client tries to input negative number or something that is not a number.
        /// </summary>
        /// <param name="mesage">Used to show message client will see before asking for his input.</param>
        private void FundChecker(string mesage)
        {
            while (true)
            {
                Console.Clear();
                Console.Write(mesage);
                try
                {
                    decimal newUserFunds = decimal.Parse(Console.ReadLine());

                    if (newUserFunds < 0)
                    {
                        Console.Clear();
                        Console.WriteLine("You cannot input negative decimal number. Try again.\n");
                        ToContinue();
                        

                    }
                    else { this.userFunds += newUserFunds; break; }
                }
                catch (Exception e)
                {
                    Console.Clear();
                    Console.WriteLine("You did not input decimal number. Try again. \n");
                    ToContinue();



                }

            }
        }


        /// <summary>
        /// Changes client funds in: 'userFunds' and: 'moneySpend' values based on 'money' paramiter. Adds: 'productId' to dictionary called: 'purchases' if it is not on it yet. Sets key as: 'productID' and values as: amount = 1, and name: 'name' paramiter.
        /// If: 'productId' is in dictionary it will increase its amount by 1.
        /// </summary>
        /// <param name="money">Amount of money client gonna spend on product. </param>
        /// <param name="productID">Id/place of a product that gonna be brought. </param>
        /// <param name="name">Name of a product that was brought. </param>
        public void TransactonMechanism(decimal money, int productID, string name) {
           this.broughtSomething = true;

            userFunds -= money;
            moneySpend += money;

            if (purchases.ContainsKey(productID))
            {
                int currentAmount = purchases[productID].amount;
                currentAmount++;
                string currentName = purchases[productID].name;

                purchases[productID] = (currentAmount, currentName);
            }
            else
            {
                purchases.Add(productID, (1, name));
            }

        }

        /// <summary>
        /// Shows all products client has brought. If client has brought nothig it will show: 'Nothing. '.
        /// </summary>
        private void PurchasesList()
        {
            int i = 1;
            Console.WriteLine($"You have brought: \n");
            if (purchases.Count() == 0) { Console.WriteLine("Nothing. \n"); }
            foreach (var product in purchases)
            {
                
                Console.WriteLine($"{i}. {product.Value.name} in amount of: {product.Value.amount}. \n");
                i++;
                                  
            }
        }

        /// <summary>
        /// Method to send new date/changes to sql database based on: 'moneySpend' value and: 'purchases' dictionary .
        /// </summary>

        public void SentSQLChanges()
        {
            if (this.broughtSomething == true) { 
                foreach (var pro in purchases)
                {

                    ISQLcomunication.SendDate($"UPDATE Products SET [Amount in Machine] = [Amount in Machine] - {pro.Value.amount} WHERE [Product ID] = {pro.Key}");
                }

                DateTime endTime = DateTime.Now;

                TimeSpan timeOfTransaction = endTime - startOfTransactions;

                string moneySpentFormatted = this.moneySpend.ToString().Replace(',', '.');

                ISQLcomunication.SendDate($"INSERT INTO Transactions VALUES ('{startOfTransactions.ToString("yyyy-MM-dd")}', '{timeOfTransaction}', {moneySpentFormatted})");

                ISQLcomunication.SendDate($"UPDATE [Money In Machine] SET [Current Money In Machine] = [Current Money In Machine] + {moneySpentFormatted} WHERE ID = 1");
            }
        }


        /// <summary>
        /// Shows what client has brought, what he spend, sends new date/changes to sql database and ends application.
        /// </summary>

        public void EndOperationsClient()
        {
            Console.Clear ();

            SentSQLChanges();

            Console.WriteLine($"Thank you for using our vending machine. \n");

                                PurchasesList();

            Console.WriteLine($"You have spend: {moneySpend} $.");
            Console.WriteLine(" ");
            Console.WriteLine("To shut down application press any key. \n");
            Console.ReadKey();
            Environment.Exit (0);
        }
        
       
    }
}
