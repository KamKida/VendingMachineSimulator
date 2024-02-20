using VendingMachine.DefaultContinueFunctions;
using VendingMachine.SQLcomunication;

namespace VendingMachine.DateObjectsWorkers
{
   

    internal class WorkerObj
    {

        private Dictionary<DateTime, string> workerActions = new Dictionary<DateTime, string>();

        public int workerId;
        public string workerNameSurname;
        public string login;
        public string password;

        public WorkerObj(int Id, string nameSurname, string login, string password)
        {
            this.workerId = Id;
            this.workerNameSurname = nameSurname;
            this.login = login;
            this.password = password;
        }
        /// <summary>
        /// Shows all values of: 'WorkerObj'.
        /// </summary>

        public void SayHello()
        {
            Console.WriteLine($"Worker id: {this.workerId}.\n" +
                             $"Worker name and surname: {this.workerNameSurname}. \n" +
                             $"Worker login: {this.login}.\n" +
                             $"Worker password: {this.password}. \n");
        }

        /// <summary>
        /// Add new action to dictionary of actions that worker did. Date of action is key, action description is value.
        /// </summary>
        /// <param name="action">Description of action as string.</param>
  

        public void AddAction(string action)
        {
            DateTime dateOfAction = DateTime.Now;



            if (workerActions.ContainsKey(dateOfAction))
            {
                return;
            }
            else if (!workerActions.ContainsKey(dateOfAction))
            {
                workerActions.Add(dateOfAction, action);
            }

        }

        /// <summary>
        /// Sends new date to date base ,based on: 'workerActions' dictionary that is created right away with new 'Worker Obj' and shuts down aplication.
        /// </summary>


        public void EndOperationsWorker()
        {

            if (workerActions.Count == 0)
            {
                AddAction("Did nothing. ");
            }
            foreach (var action in workerActions)
            {
                ISQLcomunication.SendDate($"INSERT INTO [Workers History] VALUES('{action.Key.ToString("yyyy-MM-dd")}', {this.workerId}, '{action.Value}');");
            }
            Console.WriteLine($"Thank you for your work: {this.workerNameSurname} . ");
            Console.WriteLine(" ");
            Console.WriteLine("To shut down application press any key. \n");
            Console.ReadKey();
            Environment.Exit(0);
        }

        /// <summary>
        /// Changes values in 'WorkerObj' based on options .
        /// </summary>
        /// <param name="option">Depending on option chosen variables will be changed. Options are: "name and surname" for: name and surname, "login" for: login, "password" for password.</param>
        /// <param name="newDate">Its new value that will be put into place of old value.</param>
        /// <param name="workerDif">Its for 'AddAction()' method if 'WorkerObj' you are editing values on is diffrent than the one you are using.</param>

        public void EditDate(string option, string newDate, WorkerObj workerDif)
        {
            if (option == "name and surname")
            {
                string oldNameSurname = this.workerNameSurname;
                this.workerNameSurname = newDate;
                workerDif.AddAction($"Changed worker name and surname from : {oldNameSurname} ,to: {this.workerNameSurname} for worker with id: {this.workerId}. ");
                ISQLcomunication.SendDate($"UPDATE Workers SET [Name And Surname] = '{this.workerNameSurname}' WHERE [Worker ID] = {this.workerId}");
            }
            else if (option == "login")
            {
                string oldLogin = this.workerNameSurname;
                this.login = newDate;
                workerDif.AddAction($"Changed worker login from : {oldLogin} ,to: {this.login} for worker with id: {this.workerId}. ");
                ISQLcomunication.SendDate($"UPDATE Workers SET [Worker Login] = '{this.login}' WHERE [Worker ID] = {this.workerId}");
            }
            else if (option == "password")
            {
                string oldPassword = this.workerNameSurname;
                this.password = newDate;
                workerDif.AddAction($"Changed worker name and surname from : {oldPassword} ,to: {this.password} for worker with id: {this.workerId}. ");
                ISQLcomunication.SendDate($"UPDATE Workers SET [Worker Password] = '{this.password}' WHERE [Worker ID] = {this.workerId}");
            }

        }

    }
}
