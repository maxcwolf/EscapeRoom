using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace EscapeRoom
{
    public class Menu
    {
        public static int ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("WELCOME TO THE NSS COHORT COMMAND LINE TOOL");
            Console.WriteLine("**************************************");
            Console.WriteLine("1. Show Cohort Information");
            Console.WriteLine("2. Add Info To Database");
            Console.WriteLine("3. Exit");
            Console.Write("> ");

            ConsoleKeyInfo enteredKey = Console.ReadKey();
            Console.WriteLine("");
            return int.Parse(enteredKey.KeyChar.ToString());
        }
        public static int DisplayDBMenu()
        {
            Console.Clear();
            Console.WriteLine("ADD DATA TO THE DATABASE");
            Console.WriteLine("**************************************");
            Console.WriteLine("1. ADD SERVER SIDE LANGUAGE");
            Console.WriteLine("2. ADD INSTRUCTOR");
            Console.WriteLine("3. ADD COHORT");
            Console.WriteLine("4. ADD STUDENT");
            Console.WriteLine("5. ADD INSTRUCTOR TO COHORT");
            Console.WriteLine("6. BACK TO MAIN MENU");
            Console.Write("> ");

            ConsoleKeyInfo enteredKey = Console.ReadKey();
            Console.WriteLine("");
            return int.Parse(enteredKey.KeyChar.ToString());
        }

        public static void ShowDBMenu(DatabaseInterface db)
        {

            int choice;

            do
            {
                choice = Menu.DisplayDBMenu();
                switch (choice)
                {
                    // Menu option 1: Adding account
                    case 1:
                        {
                            Console.Clear();
                            Console.WriteLine("ADD PRIMARY TRACK SERVER SIDE LANGUAGE : ");
                            Console.Write("> ");
                            string language = Console.ReadLine();
                            db.Insert($@"
                            INSERT INTO Language
                            (Id, Language)
                            VALUES
                            (null, '{language}')
                        ");
                            break;
                        }

                    // Menu option 2: Deposit money
                    case 2:
                        {
                            Console.Clear();
                            Console.WriteLine("ADD INSTRUCTOR : ");
                            Console.Write("> ");
                            string name = Console.ReadLine();
                            db.Insert($@"
                            INSERT INTO Instructors
                            (Id, Name)
                            VALUES
                            (null, '{name}')
                        ");
                            break;
                        }

                    case 3:
                        {
                            Console.Clear();
                            Console.WriteLine("ADD COHORT NAME : ");
                            Console.Write("> ");
                            string cohort = Console.ReadLine();
                            // Display all languages
                            int languageId = Menu.ListLanguages(db);
                            db.Insert($@"
                            INSERT INTO Cohort
                            (Id, Cohort, LanguageId)
                            VALUES
                            (null, '{cohort}', {languageId})
                        ");
                            break;
                        }

                    case 4:
                        {
                            // Logic here
                            Console.Clear();
                            Console.WriteLine("ADD STUDENT NAME : ");
                            Console.Write("> ");
                            string name = Console.ReadLine();
                            // Display all languages
                            int cohortId = Menu.ListCohorts(name, db);
                            db.Insert($@"
                            INSERT INTO Student
                            (Id, Name, CohortId)
                            VALUES
                            (null, '{name}', {cohortId})
                        ");
                            break;
                        }

                    case 5:
                        {
                            Console.Clear();
                            (int Id, string Name) instructors = Menu.ListInstructors(db);
                            int cohortId = Menu.ListCohorts(instructors.Name, db);

                            db.Insert($@"
                                INSERT INTO CohortInstructors
                                (Id, CohortId, InstructorsId)
                                VALUES
                                (null, '{cohortId}', {instructors.Id})
                            ");

                            // Logic here
                            break;
                        }

                }
            } while (choice != 6);
        }

        public static int ListLanguages(DatabaseInterface db)
        {
            string query = "SELECT * FROM Language";
            List<(int Id, string Langage)> LanguageList = new List<(int, string)>();

            db.Query(query,
               (SqliteDataReader handler) =>
               {
                   while (handler.Read())
                   {
                       LanguageList.Add((handler.GetInt32(0), handler.GetString(1)));
                   }
               });

            Console.Clear();
            Console.WriteLine("Choose Server Side Language :");
            Console.WriteLine("Enter ID Number");
            Console.WriteLine("*****************************");

            LanguageList.ForEach(x =>
            {
                Console.WriteLine($"{x.Id}: {x.Langage}");
            });

            Console.Write("> ");
            ConsoleKeyInfo enteredKey = Console.ReadKey();
            Console.WriteLine("");

            return int.Parse(enteredKey.KeyChar.ToString());
        }

        public static int ListCohorts(string name, DatabaseInterface db)
        {
            string query = "SELECT Id, Cohort FROM Cohort";
            List<(int Id, string Name)> CohortList = new List<(int, string)>();

            db.Query(query,
               (SqliteDataReader handler) =>
               {
                   while (handler.Read())
                   {
                       CohortList.Add((handler.GetInt32(0), handler.GetString(1)));
                   }
               });

            Console.Clear();
            if (name.Equals(""))
            {
                Console.WriteLine($"Choose Cohort From The List :");
            }
            else
            {
                Console.WriteLine($"Choose {name}'s Cohort :");
            }
            Console.WriteLine("Enter ID Number");
            Console.WriteLine("*****************************");

            CohortList.ForEach(x =>
            {
                Console.WriteLine($"{x.Id}: {x.Name}");
            });

            Console.Write("> ");
            string enteredKey = Console.ReadLine();
            Console.WriteLine("");

            return int.Parse(enteredKey);
        }

        public static (int Id, string Name) ListInstructors(DatabaseInterface db)
        {
            string query = "SELECT * FROM Instructors";
            List<(int Id, string Name)> InstructorsList = new List<(int, string)>();

            db.Query(query,
               (SqliteDataReader handler) =>
               {
                   while (handler.Read())
                   {
                       InstructorsList.Add((handler.GetInt32(0), handler.GetString(1)));
                   }
               });

            Console.Clear();
            Console.WriteLine($"Select Instructor to add to Cohort :");
            Console.WriteLine("Enter ID Number");
            Console.WriteLine("*****************************");

            InstructorsList.ForEach(x =>
            {
                Console.WriteLine($"{x.Id}: {x.Name}");
            });

            Console.Write("> ");
            string enteredKey = Console.ReadLine();
            Console.WriteLine("");

            int choice = int.Parse(enteredKey);

            return InstructorsList.Find(x => x.Id == choice);
        }

        public static void DisplayCohortInfo(DatabaseInterface db)
        {
            int cohortId = ListCohorts("", db);

            string cohortName = "";
            HashSet<string> instructorNames = new HashSet<string>();
            HashSet<string> studentNames = new HashSet<string>();
            string language = "";

            db.Query($@"
                SELECT Cohort.Cohort, Language.Language, Instructors.Name, Student.Name FROM Cohort
                JOIN Language
                ON Cohort.LanguageId = Language.Id
                JOIN CohortInstructors
                On Cohort.Id = CohortInstructors.CohortId
                JOIN Instructors
                On CohortInstructors.InstructorsId = Instructors.Id
                JOIN Student
                ON Cohort.Id = Student.CohortId
                WHERE Cohort.Id = {cohortId}
            ", (SqliteDataReader handler) =>
            {
                while (handler.Read())
                {
                    cohortName = handler.GetString(0);
                    language = handler.GetString(1);
                    instructorNames.Add(handler.GetString(2));
                    studentNames.Add(handler.GetString(3));
                }
            });

            Console.Clear();
            Console.WriteLine($"COHORT : {cohortName}");
            Console.WriteLine("*****************");
            Console.WriteLine($"LANGUAGE : {language}");
            Console.WriteLine("*****************");
            Console.Write("INSTRUCTORS : ");
            foreach (var inst in instructorNames)
            {
                Console.Write($"{inst} ");
            }
            Console.WriteLine("");
            Console.WriteLine("*****************");
            Console.WriteLine("");
            Console.WriteLine("STUDENTS : ");
            foreach (var student in studentNames)
            {
                Console.WriteLine($"{student} ");
            }
            Console.WriteLine("");
            Console.WriteLine("Press Any Key To Return To Main Menu");
            Console.ReadKey();

        }
    }
}