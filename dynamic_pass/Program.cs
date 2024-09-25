using dynamic_pass;

internal class Program
{
    private static void Main(string[] args)
    {
        CreaditCard creaditCard = new CreaditCard();
        IOperations operations = new Operations(creaditCard);
        bool exit = false;
        Console.Clear();
        Console.WriteLine("welcome!");
        while (!exit)
        {
            Console.WriteLine("\n\tDynamic pass operations");
            Console.WriteLine("\n1-display all credit cards");
            Console.WriteLine("2-create dynamic pass");
            Console.WriteLine("3-create new credit card");
            Console.WriteLine("4-Edit existing cards");
            Console.WriteLine("\nplease enter the number of desired opearation");
            var input = Console.ReadLine();
            Console.Clear();
            switch (input)
            {
                case "1":
                    operations.DisplayAllCards();
                    break;
                case "2":
                    operations.CreateDynamicPass();
                    break;
                case "3":
                    operations.CreateNewCreditCard();
                    break;
                case "4":
                    Console.WriteLine("\n\tedit existing credit cards");



                    Console.WriteLine("\n1-edit credit card");
                    Console.WriteLine("2-delete credit card");
                    var editInput = Console.ReadLine();
                    Console.Clear();
                    switch (editInput)
                    {
                        case "1":
                            operations.EditCard();
                            break;
                        case "2":
                            operations.DeleteCard();
                            break;
                    }
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