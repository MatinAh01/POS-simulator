using System;
using System.Transactions;
using pos_simulator;

internal class Program
{
    private static void Main(string[] args)
    {
        Transactions transaction= new Transactions();
        IOperations operations= new Operations(transaction);
        bool exit = false;
        Console.Clear();
        Console.WriteLine("welcome!");
        while (!exit)
        {
            Console.WriteLine("\n\tPos operations");
            Console.WriteLine("\n1-new transaction");
            Console.WriteLine("2-display all transactions");
            var input = Console.ReadLine();
            Console.Clear();
            switch (input)
            {
                case "1":
                operations.NewTransaction();
                    break;
                case "2":
                operations.DisplayAllTransaction();
                    break;


            }
            Console.WriteLine("do you want continue or not?(Y/N)");
            string userInput = Console.ReadLine();
            Console.Clear();
            if (userInput.ToLower() == "yes" || userInput.ToLower() == "y")
            {
                exit = false;
            }
            else exit = true;
        }
    }
}