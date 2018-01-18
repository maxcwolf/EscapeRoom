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
            Console.WriteLine ("1. Create cohort");
            Console.Write ("> ");
            ConsoleKeyInfo enteredKey = Console.ReadKey();
            Console.WriteLine("");
            return int.Parse(enteredKey.KeyChar.ToString());
        }
    }
}