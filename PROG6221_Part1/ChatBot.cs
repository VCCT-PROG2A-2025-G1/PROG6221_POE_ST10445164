using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PROG6221_Part1
{
    class ChatBot
    {
        // Stores the Username
        private String Username;

        // Chatbot Conversation
        public void Conversation()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enter Your Username: ");
            Username = Console.ReadLine();

            // Check if the username is not empty or whitespace
            while (string.IsNullOrWhiteSpace(Username))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Name cannot be empty. Enter a Valid Username: ");
                Username = Console.ReadLine();
            }
            // Welcoming the user with the Username
            PrintWithTextDelay($"\nHello {Username}, I'm here to help you stay safe online!\n");

            bool continueChat = true;

            // Start chatbot while loop to handle user questions
            while (continueChat)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\nAsk me a question or type 'exit' to quit: ");
                Console.ForegroundColor = ConsoleColor.White;

                string Userinput = Console.ReadLine().ToLower();

                // Handle empty or invalid input
                if (string.IsNullOrWhiteSpace(Userinput))
                {
                    PrintWithTextDelay("I didn't fully understand that. Please reword your question.");
                    continue;
                }
                // Handles user input
                switch (Userinput)
                {
                    case string s when s.Contains("exit"):
                        PrintWithTextDelay("Stay safe online. Goodbye!");
                        continueChat = false;
                        break;
                    case string s when s.Contains("how are you"):
                        PrintWithTextDelay("I'm only code, but I'm functioning perfectly Thanks!");
                        break;
                    case string s when s.Contains("purpose"):
                        PrintWithTextDelay("I'm here to help you learn how to protect yourself online.");
                        break;
                    case string s when s.Contains("password"):
                        PrintWithTextDelay("Make use of complicated passwords that combine symbols, numbers, and letters.");
                        break;
                    case string s when s.Contains("phishing"):
                        PrintWithTextDelay("Steer clear of links from senders you don't know. Always make sure to check the URL.");
                        break;
                    case string s when s.Contains("browsing"):
                        PrintWithTextDelay("Make sure websites use HTTPS and avoid suspicious popups.");
                        break;
                    case string s when s.Contains("what can i ask"):
                        PrintWithTextDelay("You can ask about password safety, phishing, and safe browsing tips.");
                        break;
                    default:
                        PrintWithTextDelay("I did not understand that. Could you rephrase?");
                        break;
                }
            }
        }
        // Prints a message, character per character, with a typing delay.
        static void PrintWithTextDelay(string message, int delay = 30)
        {
            foreach (char c in message)
            {
                // Prints each character
                Console.Write(c);
                // Waits for a short time before printing the next character
                Thread.Sleep(delay); 
            }
            // Moves to the next line after the message is printed
            Console.WriteLine(); 
        }
    }
}


