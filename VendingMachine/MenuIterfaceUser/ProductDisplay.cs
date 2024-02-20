using VendingMachine.SQLcomunication;
using VendingMachine.ProdClientObj;
using System.Data.SqlClient;
using VendingMachine.DefaultContinueFunctions;


namespace VendingMachine.MenuIterface
{
    internal class ProductDisplay : DefaultOptions
    {
        /// <summary>
        /// List of products based on date from database.
        /// </summary>
        protected List<ProductObj> products = ISQLcomunication.GetList(BuildProductObject, "Products");


        /// <summary>
        /// Used to create list of products based on date from database.
        /// </summary>
        /// <returns> New 'ProductObj' that is automatically added to list called: 'products'. </returns>
        public static ProductObj BuildProductObject(SqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            int amount = reader.GetInt32(2);
            decimal price = reader.GetDecimal(3);

            return new ProductObj(id, name, amount, price);
        }

        /// <summary>
        /// Shows products in list of products called: 'products'. If product name is: "EMPTY" it will be seen as empty slot in vending machine and not shown.
        /// </summary>
        public void ShowProducts()
        {
            foreach (var product in products)
            {
                if (product.name != "EMPTY")
                {
                    product.Introduce();
                }
            }
        }



    }

}
