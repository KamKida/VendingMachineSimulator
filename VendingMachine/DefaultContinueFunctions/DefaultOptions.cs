using VendingMachine.SQLcomunication;

namespace VendingMachine.DefaultContinueFunctions
{
    internal class DefaultOptions
    {
        /// <summary>
        /// Used to continue operations of application after showing result/s of operation.
        /// </summary>
        public void ToContinue()
        {
            Console.WriteLine("To continue press any key. \n");
            Console.ReadKey();
        }

        /// <summary>
        /// Used to inform user that whatever he input is not on list of options.
        /// </summary>
        /// <param name="input">Input of a user that was incorrect.</param>
        public void DefaultChoice(string input)
        {
            Console.Clear();
            Console.WriteLine($"You have input something that is not on options list. Your input: '{input}' .\n");
            ToContinue();
        }

        /// <summary>
        /// Pattern used to create menus.
        /// </summary>
        /// <param name="nameMenu">Is the title of menu. Will always be in upper cases.</param>
        /// <param name="name">Is a name of worker using menu. </param>
        /// <param name="options">Will show options you want to present for worker/client.</param>
        /// <param name="admin">If: 'true' worker/client name will be shown as: "Hello: {name}." after title. If: 'false' will be hidden.</param>
        /// <param name="xOption">If: 'true' choice to end current operation wil be shown as: "x: End operations."  after your options. If: 'false' will be hidden.</param>
        public void DefaultMenu(string nameMenu, string name, string options, bool admin,bool xOption)
        {
            Console.WriteLine($"-----------{nameMenu.ToUpper()}-----------");
            Console.WriteLine(" ");
            if (admin == true) { Console.WriteLine($"Hello: {name}."); }
            Console.WriteLine(" ");
            Console.WriteLine($"{options}");
            if (xOption == true) { Console.WriteLine("x: End operations. \n"); }
            Console.Write("Input: ");
        }

        /// <summary>
        /// Yes/No option menu pattern.
        /// </summary>
        /// <param name="mesage">What will happen if he presses: 'yes' .</param>

        public void YesNo(string mesage)
        {
            string options = $"{mesage}? \n" +
                                 "y: For yes. \n" +
                                 "n: For no. \n";
            DefaultMenu("-", "", options, false, false);
        }
    }
}
