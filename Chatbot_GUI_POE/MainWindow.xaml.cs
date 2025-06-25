using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<QuizQuestion> quizQuestions = new List<QuizQuestion>();
        private int currentQuestionIndex = 0;
        private int quizScore = 0;
        private bool quizActive = false;
        private string username = "User";

        public MainWindow()
        {
            InitializeComponent();
            InitializeBot();
        }

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

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string userInput = UserInput.Text.Trim().ToLower(); 

            if (string.IsNullOrWhiteSpace(userInput))
                return;

            AppendToChat($"You: {userInput}"); 
            UserInput.Clear(); 

            string botResponse = GetBotResponse(userInput); 
            AppendToChat($"Bot: {botResponse}"); 
        }


        private void AppendToChat(string message)
        {
            ChatOutput.Text += message + Environment.NewLine;
        }


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

            // Step 3: Task Assistant - Add Task
            if (input.StartsWith("add task"))
            {
                // Attempt to extract date
                DateTime? reminderDate = null;
                string raw = input.Replace("add task", "").Trim();

                // Try to find a date pattern (e.g., "on 2025-07-01")
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
                

                // Add to activity log
                string logEntry = $"Task added: '{taskTitle}'";
                if (reminderDate != null)
                    logEntry += $" (Reminder set for {reminderDate:yyyy-MM-dd})";

                activityLog.Add(logEntry);

                if (reminderDate != null)
                    return $"Task added: {taskTitle} with a reminder on {reminderDate:yyyy-MM-dd}.";
                else
                    return $"Task added: {taskTitle}. You can optionally set a date using 'on YYYY-MM-DD'.";
            }

            // Step 3: Task Assistant - Show Tasks
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

            // Quiz Feature Part 3
            if (quizActive)
            {
                if (input.Length == 1 && "abc".Contains(input))
                {
                    char userAnswer = input[0];
                    var currentQuestion = quizQuestions[currentQuestionIndex];

                    string feedback = userAnswer == currentQuestion.CorrectAnswer
                        ? "Correct!" : $"Incorrect. The correct answer was '{currentQuestion.CorrectAnswer}'.";

                    if (userAnswer == currentQuestion.CorrectAnswer)
                        quizScore++;

                    currentQuestionIndex++;

                    if (currentQuestionIndex < quizQuestions.Count)
                    {
                        var next = quizQuestions[currentQuestionIndex];
                        return $"{feedback}\n\nQuestion {currentQuestionIndex + 1}: {next.Question}\n{string.Join("\n", next.Options)}";
                    }
                    else
                    {
                        quizActive = false;
                        activityLog.Add($"Quiz completed - {quizScore}/{quizQuestions.Count} correct.");
                        return $"{feedback}\n\n🎉 Quiz complete! Your score: {quizScore}/{quizQuestions.Count}";
                    }
                }
                else
                {
                    return "Please answer with 'a', 'b', or 'c'.";
                }
            }

            // Start quiz trigger
            if (input == "start quiz")
            {
                InitializeQuiz();
                var q = quizQuestions[0];
                return $"🧠 Starting Cybersecurity Quiz!\n\nQuestion 1: {q.Question}\n{string.Join("\n", q.Options)}";
            }

            // Default Response
            return "I didn't quite get that. Can you rephrase?";
        }
        // Quiz Feature Part 3
        private void InitializeQuiz()
        {
            quizQuestions.Clear();
            currentQuestionIndex = 0;
            quizScore = 0;

            quizQuestions.Add(new QuizQuestion
            {
                Question = "What does 2FA stand for?",
                Options = new[] { "a) Two-Factor Authentication", "b) Fast Access", "c) File Authorization" },
                CorrectAnswer = 'a'
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "Which of these is a strong password?",
                Options = new[] { "a) password123", "b) 12345678", "c) T9@kLp$8" },
                CorrectAnswer = 'c'
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "What should you check before clicking a link in an email?",
                Options = new[] { "a) The color of the email", "b) The sender's address", "c) Font style" },
                CorrectAnswer = 'b'
            });
            quizQuestions.Add(new QuizQuestion
            {
                Question = "What is phishing?",
                Options = new[] { "a) A type of fishing", "b) A cyber attack to steal information", "c) A social media trend" },
                CorrectAnswer = 'b'
            });
            quizQuestions.Add(new QuizQuestion
            {
                Question = "You should never share your password with anyone.",
                Options = new[] { "a) True", "b) False" },
                CorrectAnswer = 'a'
            });
            quizQuestions.Add(new QuizQuestion
            {
                Question = "You should change your password immediately if you suspect a data breach.",
                Options = new[] { "a) True", "b) False" },
                CorrectAnswer = 'a'
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "Public Wi-Fi is always safe if it doesn't require a password.",
                Options = new[] { "a) True", "b) False" },
                CorrectAnswer = 'b'
            });

            quizQuestions.Add(new QuizQuestion
            {
                Question = "Antivirus software guarantees 100% protection from all threats.",
                Options = new[] { "a) True", "b) False" },
                CorrectAnswer = 'b'
            });
            quizQuestions.Add(new QuizQuestion
            {
                Question = "What is the purpose of a firewall?",
                Options = new[] { "a) To block unwanted traffic", "b) To speed up your internet", "c) To store files" },
                CorrectAnswer = 'a'
            });
            quizQuestions.Add(new QuizQuestion
            {
                Question = "Public Wi-Fi is always safe if it doesn't require a password.",
                Options = new[] { "a) True", "b) False" },
                CorrectAnswer = 'b'
            });
            quizQuestions.Add(new QuizQuestion
            {
                Question = "Antivirus software guarantees 100% protection from all threats.",
                Options = new[] { "a) True", "b) False" },
                CorrectAnswer = 'b'
            });
            quizQuestions.Add(new QuizQuestion
            {
                Question = "Which of the following is the most secure password?",
                Options = new[] {
                    "a) johndoe123",
                    "b) 12345678",
                    "c) P@55w0rd!",
                    "d) MyDog'sName"
                                    },
                CorrectAnswer = 'c'
            });
            quizQuestions.Add(new QuizQuestion
            {
                Question = "What is the purpose of a VPN?",
                Options = new[] {
                    "a) Speed up your internet",
                    "b) Encrypt your internet connection",
                    "c) Improve your computer's performance",
                    "d) Block pop-up ads"
                                            },
                CorrectAnswer = 'b'
            });
            quizQuestions.Add(new QuizQuestion
            {
                Question = "Which type of attack tricks users into revealing personal information?",
                Options = new[] {
                     "a) Phishing",
                     "b) Brute-force attack",
                     "c) DDoS attack",
                     "d) Spoofing"
                                    },
                CorrectAnswer = 'a'
            });


            quizActive = true;
        }

        // Sentiment Detection Method Part 2
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
