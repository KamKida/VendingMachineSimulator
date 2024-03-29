﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.DateObjectsWorkers;
using VendingMachine.DefaultContinueFunctions;
using VendingMachine.SQLcomunication;

namespace VendingMachine.WorkerMenuFile.SubMenus
{
    internal class WorkersHistoryMenu : DefaultOptions
    {

        List<string>workersId = ISQLcomunication.GetList(GetWorkersIDList, "[Workers History]");
        List<string>distinctWorkersId = new List<string>();

        /// <summary>
        /// This is logic behind workers history menu. Uses switch statements based on user input using: 'WorkersHistoryMenuLook()' method.
        /// </summary>
        /// <param name="worker">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>
        public void WorkersHistoryMenuInterface(WorkerObj worker)
        {
            Console.Clear();
            string workerInput = WorkersHistoryMenuLook();
            bool historyToken = true;
            bool firstRun = true;

            if (workerInput.ToLower() != "x") {
                while (historyToken == true)
                {
                    if (firstRun != true)
                    {
                        Console.Clear();
                        workerInput = WorkersHistoryMenuLook();
                    }
                    firstRun = false;

                    switch (workerInput.ToLower())
                    {
                        case "a":
                            Console.Clear();
                            ShowAllHistoryOfWorkers(worker);
                            ToContinue();
                            break;
                        case "h":
                            ShowOneWorkerHistoryMenuInterface(worker);
                            break;
                        case "f":
                            Console.Clear();
                            ShowFailedAttemptsAtLogin(worker);
                            ToContinue();
                            break;
                        case "x":
                            historyToken = false;
                            break;
                        default:
                            DefaultChoice(workerInput);
                            break;
                    }
                }
            }

        }

        /// <summary>
        /// Has two functions. One: determines workers history menu look and asks user for input.
        /// </summary>
        /// <returns>Input of client used in: 'WorkersHistoryMenuInterface()' method.</returns>


        private string WorkersHistoryMenuLook()
        {

            string options = $"a: Show all history of workers actions from most recent. \n" +
                             $"h: Show history of actions of chosen worker. \n" +
                             $"f: Show failed attemps to login. \n";
            DefaultMenu("WORKERS ACTIONS HISTORY MENU", " ", options, false, true);
            string workerInput = Console.ReadLine();
            return workerInput;

        }

        /// <summary>
        /// Public static method used to get workers IDs from: 'Workers History' table in sql datebase for: 'workersId' list.
        /// </summary>
        /// <returns>ID of worker.</returns>

        public static string GetWorkersIDList(SqlDataReader reader)
        {
            
            try
            {
                int id = reader.GetInt32(2);
                return id.ToString();
            }
            catch (Exception e)
            {
                return "NULL";
            }
           
        }

        /// <summary>
        /// Used to show full history of workers actions.
        /// </summary>
        /// <param name="worker">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>
        private void ShowAllHistoryOfWorkers(WorkerObj worker)
        {
            ISQLcomunication.GetData(GetAllHistory, "select [Date Of Login], Workers.[Name And Surname], [Workers History].[Worker ID] ,[Worker Operations] from [Workers History] left join Workers on [Workers History].[Worker ID] = Workers.[Worker ID] WHERE [Workers History].[Worker ID] IS NOT NULL ORDER BY [Worker History ID] DESC");
            worker.SendAction("Looked at all of workers history of actions. ");
            Console.WriteLine("--------------------");
           
        }

        /// <summary>
        /// Used to show failed attempts to login into workers menu.
        /// </summary>
        /// <param name="worker">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>
        private void ShowFailedAttemptsAtLogin(WorkerObj worker)
        {
            worker.SendAction("Looked at all of failed attempts to login into worker menu. ");
            ISQLcomunication.GetData(GetAllHistory, "select [Date Of Login], Workers.[Name And Surname], [Workers History].[Worker ID] ,[Worker Operations] from [Workers History]\r\nleft join Workers on [Workers History].[Worker ID] = Workers.[Worker ID] WHERE [Workers History].[Worker ID] IS NULL ORDER BY [Worker History ID] DESC");
            Console.WriteLine("--------------------");
        }

        
        /// <summary>
        /// Public static method used to display full history of workers actions.
        /// </summary>
        /// <returns>String used in 'GetDate()' method.</returns>

        public static string GetAllHistory(SqlDataReader reader)
        {
            var workerId = "0";
            var nameSurname = " ";
            DateTime date = reader.GetDateTime(0);
            try
            {
                nameSurname = reader.GetString(1);
            }
            catch (Exception e)
            {

                nameSurname = "NULL";
            }
            try
            {
                workerId = reader.GetInt32(2).ToString();
            }
            catch (Exception e) {

                workerId = "NULL";
            }
            string operation = reader.GetString(3);

            return $"Date of worker action: {date.ToString("yyyy-MM-dd")}. \n" +
                   $"Name and surname of worker: {nameSurname}. \n" +
                   $"Id of worker that did this operation: {workerId}. \n" +
                   $"Operation description: {operation} \n";
        }

        /// <summary>
        /// Shows all distinct workers IDs from: 'workersId' list.
        /// </summary>
        private void ShowAllWorkers()
        {
            distinctWorkersId = workersId.Distinct().ToList();
            Console.WriteLine("IDs of workers that are in history of workers actions: \n");
            foreach (var id in distinctWorkersId)
            { if (id != "NULL")
                {
                    Console.WriteLine($"Worker ID: {id}. \n");
                }
            }
        }

        /// <summary>
        /// Checks if worker exist on: 'Workers History' table in sql datebase.
        /// </summary>
        /// <returns>ID of worker if exists.</returns>
        private string DoWorkerExists()
        {
            bool matchFound = false;

            while (true)
            {
                Console.Clear();
                ShowAllWorkers();
                string options = "Input worker ID whoes history of actions you want to look into. ";
                                 DefaultMenu("-", " ", options,false, true);
                string input = Console.ReadLine();

                foreach (var id in distinctWorkersId)
                {
                    if (id == input)
                    {
                        return id;
                    }
                }
                if(input.ToLower() == "x")
                {
                    return "break";
                }
                if (!matchFound)
                {
                    Console.Clear() ;
                    Console.WriteLine($"There is no worker with ID in history of workers actions: '{input}'. \n");
                    ToContinue();
                }
                
            }
        }


        /// <summary>
        /// This is logic behind one worker history menu. Uses switch statements based on user input using: 'ShowOneWorkerHistoryMenuLook()' method.
        /// </summary>
        /// <param name="worker">'WorkerObj' object used for sending actions to database using: 'SendAction()' method.</param>
        private void ShowOneWorkerHistoryMenuInterface(WorkerObj worker)
        {
            bool oneWorker = true;
            
            string inputID = DoWorkerExists();
            

            while (oneWorker == true) { 
            Console.Clear();

                if (inputID == "break") { oneWorker = false; break; }

                Console.Clear();
                Console.WriteLine($"Worker history with ID: {inputID}");
                string choice = ShowOneWorkerHistoryMenuLook();

                switch (choice.ToLower())
                {
                    case "s":
                        Console.Clear();
                        worker.SendAction($"Looked at all of one worker history of actions with ID: {inputID}. ");
                        ISQLcomunication.GetData(GetAllHistory, $"select [Date Of Login], Workers.[Name And Surname], [Workers History].[Worker ID] ,[Worker Operations] from [Workers History] left join Workers on [Workers History].[Worker ID] = Workers.[Worker ID] WHERE [Workers History].[Worker ID] = {int.Parse(inputID)}  ORDER BY [Worker History ID] DESC");
                        Console.WriteLine("----------------------");
                        ToContinue();
                        break;
                    case "l":
                        Console.Clear();
                        worker.SendAction($"Looked at all of one worker history of actions with ID: {inputID} ,from last time he used workers menu. ");
                        ISQLcomunication.GetData(GetAllHistory, $"select [Date Of Login], Workers.[Name And Surname], [Workers History].[Worker ID] ,[Worker Operations] from [Workers History] left join Workers on [Workers History].[Worker ID] = Workers.[Worker ID] WHERE  [Workers History].[Worker ID] = {int.Parse(inputID)} and [Workers History].[Date Of Login] = (select max([Date Of Login])from [Workers History] where [Worker ID] = {int.Parse(inputID)}) ORDER BY [Worker History ID] DESC");
                        Console.WriteLine("----------------------");
                        ToContinue();
                        break;
                    case "x":
                        oneWorker = false;
                        break;
                    default:
                        DefaultChoice(choice);
                        break;
                }

            }
        }
        /// <summary>
        /// Has two functions. One: determines one worker history menu look and asks user for input.
        /// </summary>
        /// <returns>Input of client used in: 'ShowOneWorkerHistoryMenuInterface()' method.</returns>

        private string ShowOneWorkerHistoryMenuLook()
        {
            string options = "s: Show all history of worker actions starting from most recent. \n" +
                             "l: Show history of actions from last time worker used 'Workers menu'. ";

            DefaultMenu("-"," ", options,false, true);
            string input = Console.ReadLine();
            return input;

        }

       
    }
}
