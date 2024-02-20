using VendingMachine.SQLcomunication;

namespace VendingMachine.ProdClientObj
{
    internal class ProductObj
    {
        public int place;
        public string name;
        public int amount;
        public decimal price;

        
        public ProductObj(int place, string name, int amount, decimal newPrice) { 
            this.place = place;
            this.name = name;
            this.amount = amount;
            this.price = Math.Round(newPrice, 2);
        }

        /// <summary>
        /// Shows all values of: 'ProductObj'.
        /// </summary>
        public void Introduce()
        {
            Console.WriteLine($"Name of product: {this.name}.\n" + $"Price of product: {this.price} $.\n" + $"Amount in machine: {this.amount}.\n" + $"Place in machine: {this.place}.\n ");
        }

        /// <summary>
        /// Changes values of product. 
        /// Send new values to datebase.
        /// For: 'delete' option, changes price and amount to: 0 and name to: 'EMPTY'.
        /// For: 'add' option ,if name is: 'EMPTY', sets values of product to new values. 
        /// </summary>
        /// <param name="information">Decides what kind of value will be changed. Options are: 'name' for name, 'price' for price, 'amount' for amount, 'add' for adding new produc, 'deleted' for deleting product. </param>
        /// <param name="newInformation">New information you want to be set for product.</param>

        public void ChangeDate(string information, string newInformation)
        {
            if(information == "name")
            {
                this.name = newInformation;
                ISQLcomunication.SendDate($"UPDATE Products SET [Product Name] = '{newInformation}' where [Product ID] = {this.place}");
            }
            else if(information == "price")
            {
                if (newInformation.Length == 1)
                {
                    string newPrice = newInformation + ",00";
                    this.price = decimal.Parse(newPrice);
                }
                else
                {
                    this.price = decimal.Parse(newInformation);
                }
                string newInformationFormat = this.price.ToString().Replace(',', '.');
                ISQLcomunication.SendDate($"UPDATE Products SET [Product Price In $] = '{newInformationFormat}' where [Product ID] = {this.place}");
            }
            else if (information == "amount")
            {
                this.amount = int.Parse(newInformation);
                ISQLcomunication.SendDate($"UPDATE Products SET [Amount In Machine] = '{int.Parse(newInformation)}' where [Product ID] = {this.place}");
            }
            else if(information == "deleted")
            {
                this.name = "EMPTY";
                this.amount = 0;
                this.price = 0;
                ISQLcomunication.SendDate($"UPDATE Products SET [Product Name] = '{this.name}' , [Amount In Machine] = {this.amount}, [Product Price In $] = {this.price} where [Product ID] = {this.place}");
            }
        }
        
        
    }
}
