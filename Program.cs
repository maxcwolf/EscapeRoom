using System;

namespace EscapeRoom
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create an instance of the database interface
            DatabaseInterface db = new DatabaseInterface();

            int choice;

            do
            {
                // Show the main menu
                choice = Menu.ShowMainMenu();

                switch (choice)
                {
                    // Menu option 1: Adding child
                    case 1:
                        System.Console.WriteLine("MM - PRESSED 1");
                        Console.ReadKey();
                        break;

                    // Menu option 2: Adding toy
                    case 2:
                        Menu.ShowDBMenu(db);
                        break;
                }
            } while (choice != 3);
        }
    }
}