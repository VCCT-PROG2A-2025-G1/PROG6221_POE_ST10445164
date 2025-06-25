// Samuel Sossen
// ST10445164
// Group 1

// Reference:
// https://copilot.microsoft.com/chats/7bpPFQBmHtPLdFNcEuYrd

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
        private List<string> UserTopic = new List<string>();
        private Dictionary<string, List<string>> KeywordResponses;
        private Random random = new Random();
        private string lastTopic = "";
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
            // Initialize the dictionary to store keyword responses
            KeywordResponses = new Dictionary<string, List<string>>
            {
                ["phishing"] = new List<string>
                {
                "Avoid clicking on links from unknown senders.",
                "Always verify email addresses carefully.",
                "Look out for any type of typos and strange URLs that a person emails you."
                },
                ["password"] = new List<string>
                {
                "Use strong passwords with numbers and special characters, Avoid using personal details in your passwords.",
                "Avoid reusing passwords across multiple accounts.",
                "Consider using a password manager to make sure you won't forget what password is for what account."
                },
                ["privacy"] = new List<string>
                {
                    "Regularly review your social media privacy settings.",
                    "Avoid oversharing personal information online.",
                    "Use privacy-focused search engines and browsers."
                },
            };


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
                // Checks for the user interest in a topic
                if (Userinput.Contains("interested in")|| Userinput.Contains("favourite topic"))
                {
                    int index = Userinput.IndexOf("interested in");
                    string topic = Userinput.Substring(index + 13).Trim().TrimEnd('.', '!', '?');

                    if (!string.IsNullOrWhiteSpace(topic) && !UserTopic.Contains(topic))
                    {
                        UserTopic.Add(topic);
                        PrintWithTextDelay($"Great! I'll remember that you're interested in {topic}. It's a crucial part of staying safe online.");
                    }
                    else if (UserTopic.Contains(topic))
                    {
                        PrintWithTextDelay($"You've already mentioned you're interested in {topic}.");
                    }
                }
                // keyword recognition for random responses
                foreach (var keyword in KeywordResponses.Keys)
                {
                    if (Userinput.Contains(keyword))
                    {
                        lastTopic = keyword;
                        var responses = KeywordResponses[keyword];
                        string selected = responses[random.Next(responses.Count)];
                        PrintWithTextDelay(selected);
                        // Memory feature for a topic
                        if (UserTopic.Contains(keyword))
                        {
                            PrintWithTextDelay($"As someone interested in {keyword}, it is important to take proactive steps to secure your online presence.");
                        }
                        goto ContinueLoop;
                    }
                }
                // Handles user input
                switch (Userinput)
                {
                    case string input when input.Contains("tell me more") || input.Contains("can you explain"):
                        if (!string.IsNullOrEmpty(lastTopic) && KeywordResponses.ContainsKey(lastTopic))
                        {
                            var responses = KeywordResponses[lastTopic];
                            string followUp = responses[random.Next(responses.Count)];
                            PrintWithTextDelay($"Sure! Here's more about {lastTopic}: {followUp}");
                        }
                        else
                        {
                            PrintWithTextDelay("I'm not sure what you're referring to. Could you specify the topic?");
                        }
                        break;
                    // Part 2 Sentiment Detection
                    case string inputs when inputs.Contains("worried") || inputs.Contains("anxious") || inputs.Contains("concerned") || inputs.Contains("uneasy") || inputs.Contains("nervous")
                      || inputs.Contains("frustrated") || inputs.Contains("annoyed") || inputs.Contains("irritated") || inputs.Contains("upset") || inputs.Contains("discouraged")
                      || inputs.Contains("curious") || inputs.Contains("interested") || inputs.Contains("inquiring") || inputs.Contains("wondering") || inputs.Contains("intrigued"):
                        HandleSentimentalWords(Userinput);
                        break;
                    // Part 1 Keyword Detection
                    case string inputs when inputs.Contains("exit"):
                        PrintWithTextDelay($"Stay safe online {Username}. Goodbye!");
                        continueChat = false;
                        break;
                    case string inputs when inputs.Contains("how are you"):
                        PrintWithTextDelay("I'm only code, but I'm functioning perfectly Thanks!");
                        break;
                    case string inputs when inputs.Contains("purpose"):
                        PrintWithTextDelay($"I'm here to help you {Username} to learn how to protect yourself online.");
                        break;
                    case string inputs when inputs.Contains("browsing"):
                        PrintWithTextDelay("Make sure websites use HTTPS and avoid suspicious popups.");
                        break;
                    case string inputs when inputs.Contains("what can i ask"):
                        PrintWithTextDelay($"{Username} you can ask about password safety, phishing, What is Two-Factor Authetication (2fa), VPN's, and safe browsing tips.");
                        break;
                    case string inputs when inputs.Contains("2fa") || inputs.Contains("two-factor"):
                        PrintWithTextDelay("Two-factor authentication adds a second layer of security to your account, usually by sending a code to your phone.");
                        break;
                    case string inputs when inputs.Contains("vpn"):
                        PrintWithTextDelay("A VPN (Virtual Private Network) encrypts your internet connection to protect your data and privacy.");
                        break;
                    // Let the user ask what the bot remembers
                    case string inputs when inputs.Contains("what do you remember") || inputs.Contains("remember me") || inputs.Contains("favorite topic"):
                        if (UserTopic.Count > 0)
                        {
                            string allTopics = string.Join(", ", UserTopic);
                            PrintWithTextDelay($"You have mentioned that you are interested in: {allTopics}.");
                        }
                        else
                        {
                            PrintWithTextDelay("You haven't mentioned any specific topics yet.");
                        }
                        break;
                    default:
                        PrintWithTextDelay("I did not understand that. Could you rephrase?");
                        break;
                }
                ContinueLoop:; 
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
        // Handles sentiment detection based on user input.
        private void HandleSentimentalWords(string input)
        {
            string[] worriedWords = { "worried", "anxious", "concerned", "uneasy", "nervous" };
            string[] frustratedWords = { "frustrated", "annoyed", "irritated", "upset", "discouraged" };
            string[] curiousWords = { "curious", "questioning", "inquiring", "wondering", "intrigued" };

            string detectedWord = worriedWords.FirstOrDefault(word => input.Contains(word));
            if (detectedWord != null)
            {
                PrintWithTextDelay($"It's okay to feel {detectedWord}. Cyber threats can be scary, but I'm here to help.");
                return;
            }

            detectedWord = frustratedWords.FirstOrDefault(word => input.Contains(word));
            if (detectedWord != null)
            {
                PrintWithTextDelay($"I'm sorry you're feeling {detectedWord}. Let’s go step-by-step to stay safe online.");
                return;
            }

            detectedWord = curiousWords.FirstOrDefault(word => input.Contains(word));
            if (detectedWord != null)
            {
                PrintWithTextDelay($"I love that you're {detectedWord}! Ask me anything about staying safe online.");
                return;
            }
        }

    }
}
//-----------------------------------------------END OF FILE------------------------------------------------


