using System;

namespace BankTeller
{
    public class MainMenu
    {
        public static int Show()
        {
            Console.Clear();
            Console.WriteLine ("WELCOME TO THE NSS COHORT COMMAND LINE TOOL");
            Console.WriteLine ("**************************************");
            Console.WriteLine ("Let's set up the database first...");
            Console.WriteLine ("1. Add server-side language");
            Console.Write ("> ");
            ConsoleKeyInfo enteredKey = Console.ReadKey();
            Console.WriteLine("");
            return int.Parse(enteredKey.KeyChar.ToString());
        }
    }
}