using System.Xml.Linq;
using VendingMachine.DateObjectsWorkers;
using VendingMachine.DefaultContinueFunctions;
using VendingMachine.SQLcomunication;

namespace VendingMachine.WorkerMenu.SubMenus
{
    internal class ProductsWorkerMenu : ProductMenuWorkerMethods
    {
        


        /// <summary>
        /// This is logic behind worker products menu. Uses switch statements based on user input using: 'MainProuctsMenuLook()' method.
        /// </summary>
        /// <param name="name">Name of a worker using worker menu.</param>>
        ///  <param name="worker">'WorkerObj' object used for adding actions to worker using: 'AddAction()' method.</param>
        public void MainProductMenuInterface(string name, WorkerObj worker)
        {

            Console.Clear();
            string workerInput = MainProuctsMenuLook(name);
            bool productToken = true;
            bool firstRun = true;

            if (workerInput.ToLower() != "x")
            {
                while (productToken == true)
            {
                if (firstRun != true)
                {
                    Console.Clear();
                    workerInput = MainProuctsMenuLook(name);
                }
                firstRun = false;

                
                    switch (workerInput.ToLower())
                    {
                        case "s":
                            Console.Clear();
                            ShowProducts();
                            worker.AddAction("Looked at products in wending machine. ");
                            Console.WriteLine("----------------------");
                            ToContinue();
                            break;
                        case "l":
                            Console.Clear();
                            if (AreThereProductLessThenTen() == true)
                            {
                                ProductsLessThenTenInterface(name, worker);
                            }
                            else { Console.WriteLine("All products are at max capicity (10). \n"); ToContinue(); }
                            break;
                        case "e":
                            EditProductInterface(worker);
                            break;
                        case "d":
                            AddDeleteMenuInterface(worker);
                            break;
                        case "x":
                            productToken = false;
                            break;
                        default:
                            DefaultChoice(workerInput);
                            break;
                    }

                }
            }
        }

        /// <summary>
        /// Has two functions. One: determines worker products menu look and asks user for input.
        /// </summary>
        /// <returns>Input of client used in: 'MainProductMenuInterface()' method.</returns>
        private string MainProuctsMenuLook(string name)
        {
            string options = "s: Show all products in vending machine. \n" +
                             "l: Show products with less then maximum amount (ten) in slot of vending machine. \n" +
                             "e: Edit product in vending machine. \n" +
                             "d: Delete or add product to vending machine.";
            DefaultMenu("PRODUCT OPTIONS",name, options, true, true);
            string workerInput = Console.ReadLine();
            return workerInput;

        }

        /// <summary>
        /// This is logic behind worker products menu with amount less then max (10). Uses switch statements based on user input using: 'ProductsLessThenTenMenuLook()' method.
        /// </summary>
        /// <param name="name">Name of a worker using worker menu.</param>>
        ///  <param name="worker">'WorkerObj' object used for adding actions to worker using: 'AddAction()' method.</param>

        private void ProductsLessThenTenInterface(string name, WorkerObj worker) {
            bool productLessToken = true;

            while (productLessToken == true)
            {
                Console.Clear();
                ShowProductsLessThenTen();
                string workerInput = ProductsLessThenTenMenuLook(name);

                switch (workerInput.ToLower())
                {
                    case "a":
                        Console.Clear();
                        RefillProduct("1", 0, worker);
                        Console.WriteLine("All products have been refiled to full capicity.  You will be set back to Products Menu. \n");
                        productLessToken = false;
                        ToContinue();
                        break;
                    case "o":
                        Console.Clear();
                        OneProductLessThenTenInterface(name, worker);
                        break;
                    case "x":
                        productLessToken = false;
                        break;
                    default:
                        DefaultChoice(workerInput);
                        break;
                }

            }

        }

        /// <summary>
        /// Has two functions. One: determines worker products menu with amount less then max (10) look and asks user for input.
        /// </summary>
        /// <returns>Input of client used in: 'ProductsLessThenTenInterface()' method.</returns>
        private string ProductsLessThenTenMenuLook(string name)
        {
            string options = "a: Refill all products in vending machine to full (10). \n"
                           + "o: Refill one product in vending machin . ";

            DefaultMenu("REFIL PRODUCT OPTIONS", name, options, true,true);

            string workerInput = Console.ReadLine();
            return workerInput;
            

        }

        /// <summary>
        /// This is logic behind worker one product menu with amount less then max (10). Uses switch statements based on user input using: 'OneProductRefilMenuLook()' method.
        /// </summary>
        /// <param name="name">Name of a worker using worker menu.</param>>
        ///  <param name="worker">'WorkerObj' object used for adding actions to worker using: 'AddAction()' method.</param>

        private void OneProductLessThenTenInterface(string name, WorkerObj worker)
        {
            bool oneProductLessToken = true;

            while (oneProductLessToken == true)
            {
                Console.Clear();
                
                ShowProductsLessThenTen();

                
                int productId = SearchForProduct("refill");
                if (productId == 0) { oneProductLessToken = false; break; }

                Console.Clear();
                string productName = ShowOneProduct(productId);

                string workerInput = OneProductRefilMenuLook(name);

                switch (workerInput.ToLower())
                {
                    case "f":
                        Console.Clear();
                        RefillProduct("1", productId,worker);
                        Console.WriteLine($"Product: {productName} has been filled to full capacity (10). \n");
                        ToContinue();
                        break;
                    case "c":
                        Console.Clear();
                        RefillProduct("2", productId,worker);
                        break;
                    case "x":
                        oneProductLessToken = false;
                        break;
                    default:
                        DefaultChoice(workerInput);
                        break;
                }

            }

        }

        // <summary>
        /// Has two functions. One: determines worker one products menu with amount less then max (10) look and asks user for input.
        /// </summary>
        /// <returns>Input of client used in: 'OneProductLessThenTenInterface()' method.</returns>
        private string OneProductRefilMenuLook(string name)
        {
            string options = "f: Refill product to full capicity (10). \n" +
                             "c: Refil product to capcity lower then ten. ";

            DefaultMenu("ONE PRODUCT REFIL OPTIONS", name, options, true,true);
            string workerInput = Console.ReadLine();
            return workerInput;

        }


        /// <summary>
        /// This is logic behind worker edit product menu . Uses switch statements based on user input using: 'EditProductMenuLook()' method.
        /// </summary>
        ///  <param name="worker">'WorkerObj' object used for adding actions to worker using: 'AddAction()' method.</param>

        private void EditProductInterface(WorkerObj worker)
        {
            bool editToken = true;
            while (editToken == true)
            {
                Console.Clear();
                ShowProducts();
                
                int productId = SearchForProduct("edit");
               
                if (productId == 0) { editToken = false; break; }
                Console.Clear() ;
                string productName = ShowOneProduct(productId);

                 string workerInput = EditProductMenuLook();
                 switch (workerInput.ToLower()) {
                    case "d":
                        EditALL(productId, worker);
                        break;
                    case "n":
                         EditProduct(productId, "name",worker);
                    break;
                 case "p":
                         EditProduct(productId, "price", worker);
                        break;
                case "a":
                        EditProduct(productId, "amount", worker);
                        break;
                case "x":
                     editToken= false;
                     break;
                 default:
                     DefaultChoice(workerInput);
                        break;
                 }

             
            }

        }

        /// <summary>
        /// Allows worker to change all values of product.
        /// </summary>
        /// <param name="id">Id/place in vending machine of product worker wants values to change.</param>
        /// <param name="worker">'WorkerObj' object used for adding actions to worker using: 'AddAction()' method.</param>
        private void EditALL(int id, WorkerObj worker) {
            List<string> options = new List<string>() { "name", "price", "amount" };

            foreach (string option in options)
            {
                EditProduct(id,option,worker);
            }
        }

        // <summary>
        /// Has two functions. One: determines worker edit product menu look and asks user for input.
        /// </summary>
        /// <returns>Input of client used in: 'EditProductInterface()' method.</returns>
        private string EditProductMenuLook()
        {

            string options = "Which information about product you want to change: \n" +
                             "d: All date.\n"+
                             "n: Name. \n" +
                             "p: Price. \n" +
                             "a: Amount. ";

            DefaultMenu("EDIT MENU"," ",options, false, true);
            string workerInput = Console.ReadLine();
            return workerInput;
        }


        /// <summary>
        ///  This is logic behind worker add or delete product menu . Uses switch statements based on user input using: 'AddDeleteMenuLook()' method.
        /// </summary>
        /// <param name="worker">'WorkerObj' object used for adding actions to worker using: 'AddAction()' method.</param>
        private void AddDeleteMenuInterface(WorkerObj worker)
        {
            bool addDeleteToken  = true;
            while (addDeleteToken == true)
            {
                Console.Clear();
                string workerInput = AddDeleteMenuLook();
                
                switch(workerInput.ToLower())
                {
                    case "a":
                        AddProduct(worker);
                        break;
                    case"d":
                        DeleteProduct(worker);
                        break;
                    case "x":
                        addDeleteToken = false;
                        break;
                    default:
                        DefaultChoice(workerInput);
                        break;

                }
            }
        }

        // <summary>
        /// Has two functions. One: determines worker add or delete product menu look and asks user for input.
        /// </summary>
        /// <returns>Input of client used in: 'AddDeleteMenuInterface()' method.</returns>
        private string AddDeleteMenuLook()
        {
            string options = "a: Add. \n" +
                             "d: Delete." ;


            DefaultMenu("ADD AND DELETE MENU", " ", options, false,true);
            string workerInput = Console.ReadLine();
            return workerInput;
        }


    }
}
