using VendingMachine.DateObjectsWorkers;
using VendingMachine.DefaultContinueFunctions;
using VendingMachine.MenuIterface;
using VendingMachine.ProdClientObj;
using VendingMachine.SQLcomunication;

namespace VendingMachine.WorkerMenu.SubMenus
{
    internal class ProductMenuWorkerMethods : ProductDisplay 
    {
        /// <summary>
        /// Shows one product based on his ID/place in vending machine.
        /// </summary>
        /// <param name="place">IDD/place in vending machine of product.</param>
        /// <returns>Name of product.</returns>
        public string ShowOneProduct(int place)
        {
            foreach (var product in products)
            {
                if (product.place == place)
                {
                    product.Introduce();
                    return product.name;
                }
            }
            return null;
        }


        /// <summary>
        /// Menu allowing changes to products in veding machine.
        /// </summary>
        /// <param name="place">ID/place in vending machine of product you want to edit.</param>
        /// <param name="dateName">Depending on choice allows to change one value of product. Options are: 'name' for name, 'price' for price, 'amount' for amount.</param>
        /// <param name="worker">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>
        public void EditProduct(int place, string dateName, WorkerObj worker)
        {
            foreach (var product in products)
            {
                if (product.place == place)
                {
                    Console.Clear();
                    if (dateName == "name")
                    {
                        Console.WriteLine($"Curent name of product: {product.name}. \n");
                        Console.WriteLine("x: End operations. \n");
                        Console.Write("Input new name: ");
                        string newName = Console.ReadLine();
                        if (newName.ToLower() == "x") { Console.Clear(); Console.WriteLine($"Name of a product: '{product.name}' was not changed. \n"); ToContinue(); break; }
                        else
                        {
                            Console.Clear();
                            worker.SendAction($"Changed product name: {product.name} ,to: {newName}.  ");
                            product.ChangeDate("name", newName);
                            Console.WriteLine($"New name of product: {product.name}. \n");
                            ToContinue();
                        }
                    }
                    else if (dateName == "price")
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine($"Curent price of product: {product.price} $. \n");
                            Console.WriteLine("x:. End operations. \n");
                            Console.Write("Input new price: ");
                            string newPrice = Console.ReadLine();
                            if (newPrice.ToLower() == "x") { Console.Clear(); Console.WriteLine($"Price of a product: '{product.name}' was not changed. \n"); ToContinue(); break; }
                            else 
                            {
                                try
                                {
                                    if(decimal.Parse(newPrice) >= 0 ) {
                                        if (decimal.Parse(newPrice) < 7)
                                        {
                                            Console.Clear();
                                            decimal oldPrice =product.price;
                                            product.ChangeDate("price", newPrice);
                                            Console.WriteLine($"New price of product: {product.name} is now: {product.price} $. \n");
                                            worker.SendAction($"Changed product : {product.name} price: {oldPrice} $ ,to: {product.price} $.  ");
                                            ToContinue();
                                            break;
                                        }
                                        else { Console.Clear(); Console.WriteLine("New price cant be bigger than 7,00 $.  \n"); ToContinue(); }
                                    }
                                    else { Console.Clear(); Console.WriteLine("New price cant be less than zero $.  \n"); ToContinue(); }
                                }
                                catch(Exception e) { Console.Clear(); Console.WriteLine($"You did not input decimal number: {newPrice}. \n"); ToContinue(); }
                            }
                        }
                    }
                    else if (dateName == "amount")
                    {
 
                            while (true)
                            {
                                Console.Clear();
                                Console.WriteLine($"Curent amount of product: {product.amount} . \n");
                                Console.WriteLine("x: End operations. \n");
                                Console.Write("Input new amount: ");
                                string newAmount = Console.ReadLine();
                                if (newAmount.ToLower() == "x") { Console.Clear(); Console.WriteLine($"Amount of a product: '{product.name}' was not changed. \n"); ToContinue(); break; }
                            try
                                {
                                    if (int.Parse(newAmount) >= 0)
                                    {
                                        if (int.Parse(newAmount) <= 10)
                                        {
                                            Console.Clear();
                                            worker.SendAction($"Changed product: {product.name} amount: {product.amount} ,to: {newAmount}.  ");
                                            product.ChangeDate("amount", newAmount);
                                            Console.WriteLine($"New amount of product: {product.name} is now: {product.amount} . \n");
                                            ToContinue();
                                            break;
                                        }
                                        else
                                        {
                                            Console.Clear();
                                            Console.WriteLine("New amount cant be bigger than ten. \n");
                                            ToContinue();
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("New amount cant be less than zero. \n");
                                        ToContinue();
                                        continue;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.Clear();
                                    Console.WriteLine($"You have input something diffrent then number: {newAmount}. \n"); ToContinue(); continue;
                                }
                            }
                        
                    }
                }
            }
        }

                
        /// <summary>
        /// Shows products that have less then maximum (10) amount in vending machine.
        /// </summary>
        
        public void ShowProductsLessThenTen()
        {
            foreach (var product in products)
            {
                if (product.amount < 10)
                {
                    if (product.name != "EMPTY")
                    {
                        product.Introduce();
                    }
                }
            }
        }

        /// <summary>
        /// Checks if there are any products with less than maximum (10) amount in vending machine.
        /// </summary>
        /// <returns>'true' if there is at least one, 'false' if there are none.</returns>

        public bool AreThereProductLessThenTen()
        {
            foreach (var product in products)
            {
                if (product.amount < 10 && product.name != "EMPTY")
                {
                    
                    return true;

                }
            }
            return false;
        }

        /// <summary>
        /// Refils product/s to said amount.
        /// </summary>
        /// <param name="workerChoice">Choices: '1' full refill, '2' partial refill with said amount. </param>
        /// <param name="productId">ID/place in vending machine of product you want to refill.</param>
        /// <param name="worker">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>
        public void RefillProduct(string workerChoice, int productId, WorkerObj worker)
        {
            if (workerChoice == "1" && productId == 0)
            {
                worker.SendAction("Refilled all products in vending machine to full capacity. ");
                foreach (var product in products)
                {
                    if (product.name != "EMPTY")
                    {
                        product.ChangeDate("amount", 10.ToString());
                    }
                }
            }
            else if (workerChoice == "1" && productId != 0)
            {
                foreach (var product in products)
                {
                    if (product.place == productId)
                    {
                        worker.SendAction($"Refilled product: {product.name} to full slot capacity. ");
                        product.ChangeDate("amount", 10.ToString());
                    }
                }
            }
            else if (workerChoice == "2" && productId != 0)
            {
                bool amountToken = true;
                while (amountToken == true)
                {

                    Console.Clear();
                    ShowOneProduct(productId);
                    string options = "Input amount that you wanna refill product with:";

                    DefaultMenu("-", " ", options, false,true);
                    string newAmount = Console.ReadLine();

                    try
                    {
                        foreach (var product in products)
                        {

                            if (product.place == productId)
                            {
                                if ((int.Parse(newAmount) + product.amount) <= 10)
                                {

                                    Console.Clear();
                                    worker.SendAction($"Refilled product: {product.name} to capacity: {(int.Parse(newAmount) + product.amount)}. ");
                                    product.ChangeDate("amount",((int.Parse(newAmount) + product.amount).ToString()));
                                    Console.WriteLine($"Amount of product: '{product.name}' is now: {product.amount}. \n");
                                    ToContinue();
                                    amountToken = false;
                                    break;
                                }
                                else if ((int.Parse(newAmount) + product.amount) > 10)
                                {
                                    bool amountTokenTwo = true;
                                    while (amountTokenTwo == true)
                                    {
                                        Console.Clear();
                                        YesNo($"You can not add: {newAmount} to product: '{product.name}' capacity: {product.amount} because its gonna be more then slot capacity (ten). \n" +
                                              $"Would you like to set amount of product: '{product.name}' to full");
                                        string capaChoice = Console.ReadLine();
                                        if (capaChoice.ToLower() == "n")
                                        {
                                            Console.Clear();
                                            Console.WriteLine($"Amount of product: '{product.name}' was not changed. \n");
                                            ToContinue();
                                            amountTokenTwo = false;
                                            break;
                                        }
                                        else if (capaChoice.ToLower() == "y")
                                        {
                                            Console.Clear();
                                            product.ChangeDate("amount", 10.ToString());
                                            worker.SendAction($"Refilled product: {product.name} to full slot capacity. ");
                                            Console.WriteLine($"Amount of product: '{product.name}' was set to full. \n");
                                            ToContinue();
                                            amountTokenTwo = false;
                                            break;
                                        }
                                        else
                                        {
                                            DefaultChoice(newAmount);
                                            continue;
                                        }

                                    }
                                }
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        if (newAmount.ToLower() == "x") { break; }
                        DefaultChoice(newAmount);
                    }
                }

            }

        }

        /// <summary>
        /// Searches if product exists.
        /// </summary>
        /// <param name="action">Depending on action there will be different message for worker: 'edit' for editing product, 'refill' for refiling product/s, 'delete' for deleting product. </param>
        /// <returns>ID/place in vending machine of product.</returns>
        public int SearchForProduct(string action)
        {


            bool resultFound = false;
            while (true)
            {
                string workerInput = "0";
                if (action == "edit")
                {
                    Console.Clear();
                    ShowProducts();
                    string options = "Input place of product you want to change informations about. ";
                    DefaultMenu("-", " ", options, false, true);
                    workerInput = Console.ReadLine();
                }
                else if (action == "refill")
                {
                    Console.Clear();
                    ShowProductsLessThenTen();
                    string options = "Input place of product you want to increas amount in machine. ";
                    DefaultMenu("-", " ", options, false,true);
                    workerInput = Console.ReadLine();
                }
                else if (action == "delete")
                {
                    Console.Clear();
                    ShowProducts();
                    string options = "Input place of product you want to delete. ";
                    DefaultMenu("-", " ", options, false, true);
                    workerInput = Console.ReadLine();
                }
                try
                {
                    foreach (var product in products)
                    {
                        if (product.place == int.Parse(workerInput))
                        {
                            if (product.name != "EMPTY") {
                                resultFound = true;
                                return int.Parse(workerInput);
                            }

                        }
                    }

                    if (!resultFound)
                    {
                        Console.Clear();
                        Console.WriteLine($"There is no product with plae: {workerInput}, in vending machine. \n");
                        ToContinue();
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    if (workerInput.ToLower() == "x") { return 0; }
                    Console.Clear();
                    Console.WriteLine($"You have input something diffrent then number: {workerInput}. \n"); ToContinue(); continue;
                }
            }

        }

        /// <summary>
        /// Adds new product in place of product with name: 'EMPTY'. If there is already 10 products in machine with name different than: 'EMPTY' it won't  allow to add new product. 
        /// Product takes place of first instance of product with name: 'EMPTY'.
        /// </summary>
        /// <param name="worker">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>
        public void AddProduct(WorkerObj worker)
        {
            int occupiedSlots = 0;
            foreach (var product in products)
            {
                if(product.name != "EMPTY")
                {
                    occupiedSlots += 1;
                }

            }
            if (occupiedSlots < 10)
            {
                Console.Clear ();
                decimal price = 0;
                int amount = 0;
                Console.Write("Input new product name: ");
                string name = Console.ReadLine();
                Console.WriteLine(" ");
                bool priceToken = true;
                while (priceToken == true)
                {
                    Console.Clear();
                    Console.Write("Input new product price: ");
                    try
                    {
                       
                        price = decimal.Parse(Console.ReadLine());
                        if(price >= 0)
                        {
                            if(price < 7)
                            {
                                priceToken = false;
                                break;
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Price cant be bigger then 7 $. \n");
                                ToContinue();
                                continue;
                            }

                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Price cant be lower then zero $. \n");
                            ToContinue();
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Clear();
                        Console.WriteLine($"You have input something diffrent then a decimal number. Try again.\n");
                        ToContinue();
                        continue;
                    }
                }

                bool amountToken = true;
                while (amountToken == true)
                {
                    Console.Clear();
                    Console.Write("Input new product amount: ");
                    try
                    {

                        amount = int.Parse(Console.ReadLine());
                        if (amount >= 0)
                        {
                            if (amount <= 10)
                            {
                                amountToken = false;
                                break;
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Amount cant be bigger then 10. \n");
                                ToContinue();
                                continue;
                            }

                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Amount cant be lower then zero . \n");
                            ToContinue();
                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Clear();
                        Console.WriteLine($"You have input something diffrent then a number. Try again. \n");
                        ToContinue();
                        continue;
                    }
                }
                int lastPlace = 0;
                bool emptyToken = false;
                    
                    foreach(var product in products)
                {
                    lastPlace = product.place;
                    if (product.name == "EMPTY")
                    {
                        Console.Clear();
                        emptyToken = true;
                        product.ChangeDate("name", name);
                        product.ChangeDate("amount", amount.ToString());
                        product.ChangeDate("price", price.ToString());
                        worker.SendAction($"Added new product: {product.name} to slot: {product.place}. ");
                        Console.WriteLine("New product: \n");
                        product.Introduce();
                        Console.WriteLine($"Was addet to slot nr: {product.place}. \n");
                        ToContinue() ;

                        break;
                    }
                    
                }
                if (!emptyToken)
                {
                    Console.Clear();
                    ProductObj newProduct = new ProductObj((lastPlace + 1), name, amount, price);
                    worker.SendAction($"Added new product: {newProduct.name} to slot: {newProduct.place}. ");
                    products.Add(newProduct);
                    Console.WriteLine("New product: \n");
                    newProduct.Introduce();
                    Console.WriteLine($"Was addet to slot nr: {newProduct.place}. \n");
                    ToContinue();
                }


            }
            else
            {
                Console.WriteLine("There is no more space in vending machine for a new products. \n");
                ToContinue();
            }
        }

        /// <summary>
        /// Allows worker to delete product from list by changing his name to: 'EMPTY' , price and amount values to zero.
        /// </summary>
        /// <param name="worker">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>
        public void DeleteProduct(WorkerObj worker)
        {
            
            int productId = SearchForProduct("delete");
            bool deleteToken = true;
            while(deleteToken == true)
            {
                Console.Clear();
                ShowOneProduct(productId);
                YesNo("Are you sure you want to delete this product");
                string workerInput = Console.ReadLine();
                switch (workerInput.ToLower())
                {
                   
                    case "y":
                        foreach(var product in products)
                        {
                            if(product.place == productId)
                            {
                                Console.Clear();
                                Console.WriteLine("Product: \n");
                                ShowOneProduct(productId);
                                Console.WriteLine("Was deleted. \n");
                                worker.SendAction($"Deleted product: {product.name} from slot: {product.place}. ");
                                product.ChangeDate("deleted", " ");
                                ToContinue();
                                deleteToken = false;
                                break;
                            }
                        }
                        break;
                  
                    case "n":
                        Console.Clear();
                        Console.WriteLine("Product: \n");
                        ShowOneProduct(productId);
                        Console.WriteLine("Was not deleted. \n");
                        ToContinue();
                        deleteToken = false;
                        break;
                    default:
                        DefaultChoice(workerInput);
                        break;

                }
                
            }

        }
    }
}
