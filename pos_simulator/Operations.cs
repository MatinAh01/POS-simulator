using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pos_simulator
{
    public class Operations : IOperations
    {
        public Transactions Transactions { get; set; }
        public Operations(Transactions transactions)
        {
            Transactions = transactions;
        }
        public void DisplayAllTransaction()
        {
            string filePath = @"C:\Users\Matin\Desktop\transactions.txt";
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 1; i <= lines.Length; i++)
            {
                Console.WriteLine(lines[i-1]);
                if (i % 3 == 0)
                {
                    Console.WriteLine("--------------------------");
                }
            }
        }
        public void SaveAllTransactions(List<string> transactions)
        {
            string filePath = @"C:\Users\Matin\Desktop\transactions.txt";
            using (StreamWriter writer = new StreamWriter(filePath, append:true))
            {
                foreach (string transaction in transactions)
                {
                    writer.WriteLine(transaction);
                }
            }
        }
        public void NewTransaction()
        {
            List<string> transaction = new List<string>();
            try
            {
                List<string> userInputs = new List<string>();

                // Price input
                Console.WriteLine("Enter the price:");
                string priceInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(priceInput))
                {
                    throw new ArgumentException("Price cannot be empty.");
                }
                Transactions.Price = priceInput;
                transaction.Add(Transactions.Price);

                // Display available cards and select one
                Console.WriteLine("Select a card number:");
                List<string> cardNumbers = DisplayAllCardsNumber();

                if (cardNumbers == null || cardNumbers.Count == 0)
                {
                    Console.WriteLine("No card numbers available.");
                    return;
                }

                // Validate card number input
                if (!int.TryParse(Console.ReadLine(), out int cardNumberInput) || cardNumberInput < 1 || cardNumberInput > cardNumbers.Count)
                {
                    Console.WriteLine("Invalid card number selection.");
                    return;
                }

                // Add the selected card number to userInputs
                Transactions.CardNumber = cardNumbers[cardNumberInput - 1];
                userInputs.Add(Transactions.CardNumber);
                transaction.Add(Transactions.CardNumber);

                // Expiration date input
                Console.WriteLine("Enter the expiry date (yymm):");
                string exDateInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(exDateInput))
                {
                    throw new ArgumentException("Expiration date cannot be empty.");
                }
                userInputs.Add(exDateInput);

                // CVV2 input
                Console.WriteLine("Enter the CVV2:");
                string cvv2 = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(cvv2))
                {
                    throw new ArgumentException("CVV2 cannot be empty.");
                }
                userInputs.Add(cvv2);

                // Dynamic password input
                Console.WriteLine("Enter the dynamic password:");
                string dynamicPass = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(dynamicPass))
                {
                    throw new ArgumentException("Dynamic password cannot be empty.");
                }
                userInputs.Add(dynamicPass);

                // Validate card details and dynamic pass
                bool isCardValid = ValidationCardDetails(userInputs);
                bool isPassValid = ValidationDynamicPass(userInputs);

                if (isCardValid && isPassValid)
                {
                    Transactions.Status = "Successful transaction";
                }
                else
                {
                    Transactions.Status = "Unsuccessful transaction";
                }
                transaction.Add(Transactions.Status);

                // Output transaction status
                Console.WriteLine(Transactions.Status);
                SaveAllTransactions(transaction);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Error: Invalid input format.");
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Error: Card selection out of range.");
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // General exception handler for unexpected errors
                Console.WriteLine("An unexpected error occurred.");
                Console.WriteLine(ex.Message);
            }
            
        }
        public bool ValidationCardDetails(List<string> userInput)
        {
            List<string> AllCardsData = GetAllCardsData();
            // Ensure the input lists have valid sizes
            if (userInput.Count != 4)
            {
                Console.WriteLine("Invalid input sizes.");
                return false;
            }
            // Iterate through the getAllCardDetails list in groups of three (card number, expire date, cvv2)
            for (int i = 0; i < AllCardsData.Count; i += 3)
            {
                string cardNumber = AllCardsData[i];
                string expireDate = AllCardsData[i + 1];
                string cvv2 = AllCardsData[i + 2];

                // Check if the card number matches
                if (userInput[0] == cardNumber)
                {
                    // If the card number matches, check the expire date and CVV2
                    if (userInput[1] == expireDate && userInput[2] == cvv2)
                    {
                        // All values match
                        return true;
                    }
                    else
                    {
                        // Card number matches, but expire date or CVV2 is different
                        Console.WriteLine("the expire Date or cvv2 is incorrect!");
                        return false;
                    }
                }
            }
            return false;
        }
        public bool ValidationDynamicPass(List<string> userInput)
        {
            List<string> dynamicPassFileData = GetDynamicPassFileData();
            const int maxAttempts = 3;
            int attempts = 0;

            // Iterate through the dynamicPassFileData list in groups of two (card number, dynamic pass)
            for (int i = 0; i < dynamicPassFileData.Count; i += 2)
            {
                string cardNumber = dynamicPassFileData[i];
                string DynamicPass = dynamicPassFileData[i + 1];

                // Check if the card number matches
                if (userInput[0] == cardNumber)
                {
                    // Loop to allow up to 3 attempts for the dynamic pass
                    while (attempts < maxAttempts)
                    {
                        // Check if the dynamic pass is correct
                        if (userInput[3] == DynamicPass)
                        {
                            return true;
                        }
                        else
                        {
                            // Increment the attempt count
                            attempts++;

                            // If the user has remaining attempts, prompt them to try again
                            if (attempts < maxAttempts)
                            {
                                Console.WriteLine("The dynamic pass is incorrect. Please try again:");
                                userInput[3] = Console.ReadLine(); // Update the dynamic pass in userInput
                            }
                        }
                    }

                    // If the user has reached the max attempts
                    Console.WriteLine("You have exceeded the maximum number of attempts for the dynamic pass.");
                    return false;
                }
            }

            // If no matching card number is found, return false
            Console.WriteLine("Card number not found.");
            return false;
        }
        public List<string> DisplayAllCardsNumber()
        {
            // Call the GetAllCards method to retrieve the list of card numbers
            List<string> cardsData = GetAllCardsData();
            List<string> cardsNumber = new List<string>();
            // Check if the list is not empty before printing
            if (cardsData.Count == 0)
            {
                Console.WriteLine("No cards found.");
            }
            else
            {
                Console.WriteLine("List of Card Numbers:");
                int index = 1;
                for (int i = 0; i < cardsData.Count; i += 3)
                {
                    cardsNumber.Add(cardsData[i]);
                    Console.WriteLine($"{index}. {cardsData[i]}");
                    index++;
                }
            }
            return cardsNumber;
        }
        private static List<string> GetAllCardsData()
        {
            string filePath = @"C:\Users\Matin\Desktop\cardsData.txt";
            var listOfCardNumber = new List<string>();

            try
            {
                // Ensure the file exists before attempting to read
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Error: File not found.");
                    return listOfCardNumber;
                }

                // Read the file line by line and collect card numbers (every 3rd line)
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    // int lineNumber = 0;

                    while ((line = reader.ReadLine()) != null)
                    {
                        listOfCardNumber.Add(line.Trim());
                        // if (lineNumber % 3 == 0)  // Assuming card number is every 3rd line
                        // {
                        //     listOfCardNumber.Add(line.Trim());  // Add the card number
                        // }
                        // lineNumber++;
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Error: The file was not found.");
                Console.WriteLine(ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error: A problem occurred while reading the file.");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred.");
                Console.WriteLine(ex.Message);
            }

            return listOfCardNumber;
        }
        private static List<string> GetDynamicPassFileData()
        {
            string filePath = @"C:\Users\Matin\Desktop\dynamicPass.txt";
            var dynamicPassFileData = new List<string>();

            try
            {
                // Ensure the file exists before attempting to read
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Error: File not found.");
                    return dynamicPassFileData;
                }

                // Read the file line by line and collect card numbers (every 3rd line)
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    // int lineNumber = 0;

                    while ((line = reader.ReadLine()) != null)
                    {
                        dynamicPassFileData.Add(line.Trim());
                        // if (lineNumber % 3 == 0)  // Assuming card number is every 3rd line
                        // {
                        //     listOfCardNumber.Add(line.Trim());  // Add the card number
                        // }
                        // lineNumber++;
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Error: The file was not found.");
                Console.WriteLine(ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error: A problem occurred while reading the file.");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred.");
                Console.WriteLine(ex.Message);
            }
            return dynamicPassFileData;
        }
    }
}