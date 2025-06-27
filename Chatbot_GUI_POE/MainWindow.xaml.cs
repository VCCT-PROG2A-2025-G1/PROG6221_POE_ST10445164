// Samuel Sossen
// ST10445164
// Group 1

// Reference:
// https://copilot.microsoft.com/chats/7bpPFQBmHtPLdFNcEuYrd
// https://theolivenbaum.medium.com/nlp-in-c-made-easy-with-spacy-catalyst-acc93e005f3d

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using PROG6221_Part1;

namespace Chatbot_GUI_POE
{
    public partial class MainWindow : Window
    {
        private List<CyberTask> taskList = new List<CyberTask>();
        private List<string> activityLog = new List<string>();
        private List<string> userTopics = new List<string>();
        private string lastTopic = "";
        private Dictionary<string, List<string>> keywordResponses;
        private NLPProcessor nlpProcessor = new NLPProcessor();
        private CyberTask lastCreatedTask = null;
        private string username = "User";
        // ----------------------------------------------------------------------------------
        // MainWindow constructor initializes the chatbot and sets up keyword responses
        // ----------------------------------------------------------------------------------
        public MainWindow()
        {
            InitializeComponent();
            InitializeBot();
        }
        // ----------------------------------------------------------------------------------
        // Initializes the bot with predefined keyword responses
        // ----------------------------------------------------------------------------------
        private void InitializeBot()
        {
            keywordResponses = new Dictionary<string, List<string>>
            {
                ["phishing"] = new List<string>
                {
                    "Be careful of emails asking for personal details.",
                    "Verify email addresses and look for typos or strange URLs.",
                    "Never click links from unknown sources."
                },
                ["password"] = new List<string>
                {
                    "Use complex passwords with numbers and symbols.",
                    "Never reuse the same password for different accounts.",
                    "Consider using a password manager."
                },
                ["privacy"] = new List<string>
                {
                    "Avoid oversharing personal info online.",
                    "Use privacy settings on all social media platforms.",
                    "Switch to privacy-focused browsers."
                }
            };
        }
        // --------------------------------------------------------------------------------------------------------------------------
        // This method is for the Send button click in this it processes the user input, and will output anything the user inputs.
        // --------------------------------------------------------------------------------------------------------------------------
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string userInput = UserInput.Text.Trim();

            if (string.IsNullOrEmpty(userInput))
                return;

            // Exits the Program 
            if (userInput.ToLower() == "exit")
            {
                MessageBox.Show("Goodbye!");
                Application.Current.Shutdown();
                return;
            }

            AppendToChat($"You: {userInput}");
            UserInput.Clear();

            string lowerInput = userInput.ToLower();

            // Activity log 
            if (lowerInput.Contains("show activity log")
                || lowerInput.Contains("what have you done")
                || lowerInput.Contains("recent actions"))
            {
                int total = ChatbotState.ActivityLog.Count;
                if (total == 0)
                {
                    AppendToChat("Bot: No recent activities yet.");
                }
                else
                {
                    int showCount = Math.Min(10, total);
                    var logEntries = ChatbotState.ActivityLog.Skip(total - showCount).ToList();

                    StringBuilder sb = new StringBuilder("Here’s a summary of recent actions:\n");
                    for (int i = 0; i < logEntries.Count; i++)
                        sb.AppendLine($"{i + 1}. {logEntries[i]}");

                    AppendToChat("Bot: " + sb.ToString());
                }
                return; 
            }

            // For setting reminders
            if (lastCreatedTask != null && lowerInput.Contains("remind me in"))
            {
                var parts = lowerInput.Split(new[] { "remind me in" }, StringSplitOptions.None);

                if (parts.Length > 1)
                {
                    var words = parts[1].Trim().Split(' ');
                    if (int.TryParse(words[0], out int days))
                    {
                        lastCreatedTask.ReminderDate = DateTime.Today.AddDays(days);
                        string logEntry = $"[{DateTime.Now:HH:mm}] Reminder set for '{lastCreatedTask.Title}' in {days} days.";
                        ChatbotState.ActivityLog.Add(logEntry);
                        AppendToChat($"Bot: Got it! I'll remind you in {days} days.");
                        return;
                    }
                }
            }

            // NLP Intent detection 
            UserIntent intent = nlpProcessor.DetectIntent(userInput);
            string response = "";

            switch (intent)
            {
                // If the user wants to set a reminder
                case UserIntent.AddReminder:
                    {
                        string desc = nlpProcessor.ExtractDescription(userInput, intent);
                        DateTime reminderDate = DateTime.Today.AddDays(1);

                        var newTask = new CyberTask
                        {
                            Title = desc,
                            Description = KeywordDetection(desc),
                            ReminderDate = reminderDate
                        };

                        ChatbotState.TaskList.Add(newTask);
                        ChatbotState.ActivityLog.Add($"[{DateTime.Now:HH:mm}] Reminder set for '{desc}' on {reminderDate:yyyy-MM-dd}.");
                        lastCreatedTask = newTask;

                        response = $"Reminder set for '{desc}' on {reminderDate:yyyy-MM-dd}.";
                        break;
                    }

                // If the user wants to add a task
                case UserIntent.AddTask:
                    {
                        string desc = nlpProcessor.ExtractDescription(userInput, intent);

                        var newTask = new CyberTask
                        {
                            Title = desc,
                            Description = KeywordDetection(desc),
                            ReminderDate = null
                        };

                        ChatbotState.TaskList.Add(newTask);
                        ChatbotState.ActivityLog.Add($"[{DateTime.Now:HH:mm}] Task added: '{desc}' (no reminder set).");
                        lastCreatedTask = newTask;

                        response = $"Task added with the description \"{newTask.Description}\". Would you like a reminder?";
                        break;
                    }

                // If the user wants a summary of their recent actions
                case UserIntent.GetSummary:
                    {
                        if (ChatbotState.ActivityLog.Count == 0)
                        {
                            response = "No actions recorded yet.";
                        }
                        else
                        {
                            int total = ChatbotState.ActivityLog.Count;
                            int showCount = Math.Min(10, total);
                            var logEntries = ChatbotState.ActivityLog.Skip(total - showCount).ToList();

                            StringBuilder sb = new StringBuilder("Here’s a summary of recent actions:\n");
                            for (int i = 0; i < logEntries.Count; i++)
                            {
                                sb.AppendLine($"{i + 1}. {logEntries[i]}");
                            }
                            response = sb.ToString();
                        }
                        break;
                    }

                // If the user wants to start a quiz
                case UserIntent.StartQuiz:
                    {
                        QuizWindow quizWindow = new QuizWindow();
                        quizWindow.ShowDialog();

                        if (ChatbotState.LastQuizScore >= 0)
                        {
                            ChatbotState.ActivityLog.Add($"[{DateTime.Now:HH:mm}] Quiz started – {ChatbotState.LastQuizTotal} questions answered.");
                            ChatbotState.ActivityLog.Add($"[{DateTime.Now:HH:mm}] Quiz completed – {ChatbotState.LastQuizScore}/{ChatbotState.LastQuizTotal} correct.");

                            response = $"You completed the quiz and scored {ChatbotState.LastQuizScore}/{ChatbotState.LastQuizTotal}.";
                        }
                        else
                        {
                            response = "You exited the quiz before completing it.";
                        }
                        break;
                    }
                default:
                    {
                        response = GetBotResponse(userInput.ToLower());
                        break;
                    }
            }

            AppendToChat($"Bot: {response}");
        }
        // ----------------------------------------------------------------------------------
        // This method checks the title for specific keywords and returns a relevant description.
        // ----------------------------------------------------------------------------------
        private string KeywordDetection(string title)
        {
            if (title.ToLower().Contains("privacy"))
                return "Review your account privacy settings to ensure your data is protected.";
            if (title.ToLower().Contains("2fa") || title.ToLower().Contains("two-factor"))
                return "Enable two-factor authentication to secure your accounts.";
            if (title.ToLower().Contains("password"))
                return "Update your passwords using strong, unique combinations.";
            if (title.ToLower().Contains("phishing"))
                return "Learn how to detect and avoid phishing scams.";

            return $"This task is to {title.ToLower()}. Make sure it improves your cybersecurity.";
        }
        // ----------------------------------------------------------------------------------
        // This method appends the message to the chat output.
        // ----------------------------------------------------------------------------------
        private void AppendToChat(string message)
        {
            ChatOutput.Text += message + Environment.NewLine;
        }
        // ----------------------------------------------------------------------------------
        // This method generates a response from the bot based on user input.
        // ----------------------------------------------------------------------------------
        private string GetBotResponse(string input)
        {
            // Sentiment Detection
            string sentiment = DetectSentiment(input);
            if (sentiment != null)
                return sentiment;

            // Activity Log Request
            if (input.Contains("show activity log"))
            {
                if (activityLog.Count == 0)
                    return "There are no recent activities logged yet.";

                var logOutput = "Here’s a summary of recent actions:\n";
                int count = 1;
                foreach (var entry in activityLog)
                {
                    logOutput += $"{count++}. {entry}\n";
                }
                return logOutput.Trim();
            }

            // Task Assistant - Add Task
            if (input.StartsWith("add task"))
            {
                DateTime? reminderDate = null;
                string raw = input.Replace("add task", "").Trim();

                int dateIndex = raw.LastIndexOf(" on ");
                string taskTitle = raw;

                if (dateIndex > -1)
                {
                    string possibleDate = raw.Substring(dateIndex + 4).Trim();
                    taskTitle = raw.Substring(0, dateIndex).Trim();

                    if (DateTime.TryParse(possibleDate, out DateTime parsedDate))
                    {
                        reminderDate = parsedDate;
                    }
                    else
                    {
                        return $"I couldn't understand the date '{possibleDate}'. Please use the format YYYY-MM-DD.";
                    }
                }

                if (string.IsNullOrWhiteSpace(taskTitle))
                    return "Please enter a task description.";

                var newTask = new CyberTask
                {
                    Title = taskTitle,
                    Description = "No description yet",
                    ReminderDate = reminderDate
                };

                taskList.Add(newTask);

                string logEntry = $"Task added: '{taskTitle}'";
                if (reminderDate != null)
                    logEntry += $" (Reminder set for {reminderDate:yyyy-MM-dd})";

                activityLog.Add(logEntry);

                return reminderDate != null
                    ? $"Task added: {taskTitle} with a reminder on {reminderDate:yyyy-MM-dd}."
                    : $"Task added: {taskTitle}. You can optionally set a date using 'on YYYY-MM-DD'.";
            }

            // Task Assistant - Show Tasks
            if (input == "show tasks")
            {
                if (taskList.Count == 0)
                    return "You have no tasks at the moment.";

                return "Here are your tasks:\n" + string.Join("\n", taskList.Select(t => "- " + t));
            }

            // Memory Feature
            if (input.Contains("interested in"))
            {
                string topic = input.Substring(input.IndexOf("interested in") + 13).Trim();
                if (!userTopics.Contains(topic))
                {
                    userTopics.Add(topic);
                    return $"I'll remember that you're interested in {topic}. It's a crucial part of cybersecurity.";
                }
                return $"You've already told me you're interested in {topic}.";
            }

            // Topic Recall
            if (input.Contains("what do you remember"))
            {
                return userTopics.Count > 0
                    ? $"You mentioned you're interested in: {string.Join(", ", userTopics)}."
                    : "You haven't mentioned any specific topics yet.";
            }

            // Keyword Responses
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    lastTopic = keyword;
                    var responses = keywordResponses[keyword];
                    return responses[new Random().Next(responses.Count)];
                }
            }

            // Follow-up Request
            if (input.Contains("tell me more") || input.Contains("can you explain"))
            {
                if (!string.IsNullOrEmpty(lastTopic) && keywordResponses.ContainsKey(lastTopic))
                {
                    var responses = keywordResponses[lastTopic];
                    return $"Here's more about {lastTopic}: {responses[new Random().Next(responses.Count)]}";
                }
                return "Can you please clarify what you'd like me to explain?";
            }

            // Launch QuizWindow
            if (input == "start quiz")
            {
                QuizWindow quizWindow = new QuizWindow();
                quizWindow.ShowDialog();

                if (ChatbotState.LastQuizScore >= 0)
                {
                    activityLog.Add($"Quiz started – {ChatbotState.LastQuizTotal} questions answered.");
                    activityLog.Add($"Quiz completed – {ChatbotState.LastQuizScore}/{ChatbotState.LastQuizTotal} correct.");

                    string summary = $"You completed the quiz and scored {ChatbotState.LastQuizScore}/{ChatbotState.LastQuizTotal}.";
                    return summary;
                }
                else
                {
                    return "You exited the quiz before completing it.";
                }
            }

            // Default Response
            return "I didn't quite get that. Can you rephrase?";
        }
        // ----------------------------------------------------------------------------------
        // Sentiment Detection
        // ----------------------------------------------------------------------------------
        private string DetectSentiment(string input)
        {
            var worriedWords = new[] { "worried", "anxious", "concerned", "uneasy", "nervous" };
            var frustratedWords = new[] { "frustrated", "annoyed", "irritated", "upset", "discouraged" };
            var curiousWords = new[] { "curious", "interested", "inquiring", "wondering", "intrigued" };

            string word = Array.Find(worriedWords, input.Contains);
            if (word != null)
                return $"It's okay to feel {word}. Cyber threats can be scary, but I'm here to help.";

            word = Array.Find(frustratedWords, input.Contains);
            if (word != null)
                return $"Sorry you're feeling {word}. Let's take it one step at a time.";

            word = Array.Find(curiousWords, input.Contains);
            if (word != null)
                return $"You're feeling {word}? I love that! Ask me anything.";

            return null;
        }
    }
}
// ----------------------------------------------------------------------------------END OF FILE -----------------------------------------------------------------------
