using System.Data.SqlClient;
using VendingMachine.DateObjectsWorkers;
using VendingMachine.MenuIterfaceUser;
using VendingMachine.SQLcomunication;
using VendingMachine.WorkerMenu.SubMenus;
using VendingMachine.WorkerMenuFile;
using VendingMachine.WorkerMenuFile.SubMenus;

namespace VendingMachine
{
    internal class Program
    {
        static void Main(string[] args)
        {


            while (ClientMenu.adminUse == false)
            {
                ClientMenu clientMenu = new ClientMenu();
                clientMenu.UserMenuInterface();
                while (ClientMenu.adminUse == true)
                {

                    WorkersMenu workersMenu = new WorkersMenu();
                    workersMenu.WorkerMenuInterface();


                }
            }
            



        }
    }
}
