using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PROG6221_Part1;

namespace Chatbot_GUI_POE
{
    public partial class MainWindow : Window
    {
        private List<string> userTopics = new List<string>();
        private string lastTopic = "";
        private Dictionary<string, List<string>> keywordResponses;
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
            if (string.IsNullOrWhiteSpace(userInput)) return;

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

            // Default
            return "I didn't quite get that. Can you rephrase?";
        }

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
