using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dynamic_pass
{
    public class Operations : IOperations
    {
        public CreaditCard creaditCard;
        public Operations(CreaditCard card)
        {
            creaditCard = card;
        }
        public void DisplayAllCards()
        {
            // Call the GetAllCards method to retrieve the list of card numbers
            List<string> cardNumbers = GetAllCards();

            // Check if the list is not empty before printing
            if (cardNumbers.Count == 0)
            {
                Console.WriteLine("No cards found.");
            }
            else
            {
                Console.WriteLine("List of Card Numbers:");
                for (int i = 0; i < cardNumbers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {cardNumbers[i]}");
                }
            }
        }
        public async Task CreateDynamicPass()
        {
            List<string> listOfCardNumber = GetAllCards();
            DisplayAllCards();
            Console.WriteLine("please select a card");
            int cardIndex = int.Parse(Console.ReadLine());
            cardIndex--;
            Random random = new Random();
            int dynamicPass = random.Next(1000000, 10000000);
            string filePath = @"C:\Users\Matin\Desktop\dynamicPass.txt";
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                bool exist = lines.Contains(listOfCardNumber[cardIndex]);
                // save the dynamic pass for specific card into file
                using (StreamWriter writer = new StreamWriter(filePath, append: true))
                {
                    if (!exist) // check the file doesn't have dynamic pass for specific card currently
                    {
                        writer.WriteLine(listOfCardNumber[cardIndex]);
                        writer.WriteLine(dynamicPass);
                        Console.WriteLine("dynamic pass create successfully");
                    }
                    else
                        Console.WriteLine("It is currently not possible to create a password");
                        Console.WriteLine($"the dynamic pass is : {dynamicPass}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            await Task.Delay(TimeSpan.FromMinutes(2)); // The dynamic password is valid for 2 minutes
            RemoveDynamicPass(filePath, dynamicPass.ToString());
        }
        public void CreateNewCreditCard()
        {
            string filePath = @"C:\Users\Matin\Desktop\cardsData.txt";

            try
            {
                // Read the existing lines from the file
                string[] lines = File.ReadAllLines(filePath);
                string[] updatedLines = new string[lines.Length + 3];

                // Get user input for credit card details
                Console.WriteLine("Enter the card number:");
                creaditCard.CardNumber = Console.ReadLine();

                Console.WriteLine("Enter the expiration date (yymm):");
                creaditCard.ExDate = Console.ReadLine();

                Console.WriteLine("Enter the card CVV2:");
                creaditCard.CVV2 = Console.ReadLine();

                for (int i = 0; i < lines.Length; i++)
                {
                    updatedLines[i] = lines[i].Trim();
                }

                // Add new card details to the array
                updatedLines[lines.Length] = creaditCard.CardNumber;
                updatedLines[lines.Length + 1] = creaditCard.ExDate;
                updatedLines[lines.Length + 2] = creaditCard.CVV2;

                // Write the updated lines back to the file
                File.WriteAllLines(filePath, updatedLines);
                Console.WriteLine("the new card created successfully");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Error: The file was not found.");
                Console.WriteLine(ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error: A problem occurred while reading or writing the file.");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred.");
                Console.WriteLine(ex.Message);
            }
        }
        public void DeleteCard()
        {
            List<string> listOfCardNumber = GetAllCards();
            if (listOfCardNumber == null || listOfCardNumber.Count == 0)
            {
                Console.WriteLine("No cards available to delete.");
                return;
            }

            try
            {
                // Display cards to user
                Console.WriteLine("Please select a card:");
                for (int i = 0; i < listOfCardNumber.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {listOfCardNumber[i]}");
                }

                // Get and validate user input
                if (!int.TryParse(Console.ReadLine(), out int cardIndex) || cardIndex < 1 || cardIndex > listOfCardNumber.Count)
                {
                    Console.WriteLine("Invalid selection. Please enter a valid number.");
                    return;
                }
                cardIndex--;  // Adjust to 0-based index

                string filePath = @"C:\Users\Matin\Desktop\cardsData.txt";

                // Check if file exists
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Error: Card data file not found.");
                    return;
                }

                string[] lines = File.ReadAllLines(filePath);

                // Find the index of the line containing the selected card number
                int lineIndex = Array.FindIndex(lines, line => line.Trim() == listOfCardNumber[cardIndex]);

                if (lineIndex == -1 || lineIndex + 2 >= lines.Length)
                {
                    Console.WriteLine("Error: Unable to find the card in the file.");
                    return;
                }

                // Create a new list excluding the 3 lines to be removed (CardNumber, ExDate, CVV2)
                List<string> updatedLines = new List<string>();
                for (int i = 0; i < lines.Length; i++)
                {
                    if (i < lineIndex || i > lineIndex + 2)
                    {
                        updatedLines.Add(lines[i]);
                    }
                }

                // Write the updated content back to the file
                File.WriteAllLines(filePath, updatedLines);
                Console.Clear();
                Console.WriteLine("The selected card was successfully deleted from the list.");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Error: The file was not found.");
                Console.WriteLine(ex.Message);
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("Error: Invalid card selection.");
                Console.WriteLine(ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error: A problem occurred while reading or writing the file.");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred.");
                Console.WriteLine(ex.Message);
            }
        }
        public void EditCard()
        {
            List<string> listOfCardNumber = GetAllCards();
            if (listOfCardNumber == null || listOfCardNumber.Count == 0)
            {
                Console.WriteLine("No cards available to delete.");
                return;
            }

            try
            {
                // Display cards to user
                Console.WriteLine("Please select a card:");
                for (int i = 0; i < listOfCardNumber.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {listOfCardNumber[i]}");
                }

                // Get and validate user input
                if (!int.TryParse(Console.ReadLine(), out int cardIndex) || cardIndex < 1 || cardIndex > listOfCardNumber.Count)
                {
                    Console.WriteLine("Invalid selection. Please enter a valid number.");
                    return;
                }
                cardIndex--;  // Adjust to 0-based index

                string filePath = @"C:\Users\Matin\Desktop\cardsData.txt";

                // Check if file exists
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Error: Card data file not found.");
                    return;
                }

                string[] lines = File.ReadAllLines(filePath);

                // Find the index of the line containing the selected card number
                int lineIndex = Array.FindIndex(lines, line => line.Trim() == listOfCardNumber[cardIndex]);

                if (lineIndex == -1 || lineIndex + 2 >= lines.Length)
                {
                    Console.WriteLine("Error: Unable to find the card in the file.");
                    return;
                }

                // Get user input for edit credit card details
                Console.WriteLine("Enter the card number:");
                creaditCard.CardNumber = Console.ReadLine();

                Console.WriteLine("Enter the expiration date (yymm):");
                creaditCard.ExDate = Console.ReadLine();

                Console.WriteLine("Enter the card CVV2:");
                creaditCard.CVV2 = Console.ReadLine();
                List<string> newCardDetails = [creaditCard.CardNumber, creaditCard.ExDate, creaditCard.CVV2];

                // Create a new list excluding the 3 lines to be removed (CardNumber, ExDate, CVV2)
                List<string> updatedLines = new List<string>();
                int j = 0;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (i < lineIndex || i > lineIndex + 2)
                    {
                        updatedLines.Add(lines[i]);
                    }
                    else
                    {
                        updatedLines.Add(newCardDetails[j]);
                        j++;
                    }
                }

                // Write the updated content back to the file
                File.WriteAllLines(filePath, updatedLines);
                Console.Clear();
                Console.WriteLine("The selected card was successfully edited.");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Error: The file was not found.");
                Console.WriteLine(ex.Message);
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine("Error: Invalid card selection.");
                Console.WriteLine(ex.Message);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error: A problem occurred while reading or writing the file.");
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred.");
                Console.WriteLine(ex.Message);
            }
        }
        private static List<string> GetAllCards()
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
                    int lineNumber = 0;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (lineNumber % 3 == 0)  // Assuming card number is every 3rd line
                        {
                            listOfCardNumber.Add(line.Trim());  // Add the card number
                        }
                        lineNumber++;
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
        private static void RemoveDynamicPass(string filePath, string dynamicPass)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                // Find the index of the line containing the dynamic pass
                int lineIndex = Array.FindIndex(lines, line => line.Trim() == dynamicPass);

                // Check if the line was found
                if (lineIndex >= 0)
                {
                    // Remove the line with the dynamic pass and the line before it, if it exists
                    int startLineIndex = Math.Max(0, lineIndex - 1); // Get the line before or 0 if at the start
                    int linesToRemove = lineIndex == 0 ? 1 : 2; // Remove only one line if the dynamic pass is the first line

                    // Create a new list excluding the lines to be removed
                    string[] updatedLines = new string[lines.Length - linesToRemove];
                    int currentIndex = 0;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (i < startLineIndex || i > lineIndex)
                        {
                            updatedLines[currentIndex] = lines[i];
                            currentIndex++;
                        }
                    }

                    // Write the updated content back to the file
                    File.WriteAllLines(filePath, updatedLines);

                }
                else
                {
                    Console.WriteLine($"The dynamic pass {dynamicPass} was not found in the file.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while removing lines from the file: {ex.Message}");
            }
        }
    }
}