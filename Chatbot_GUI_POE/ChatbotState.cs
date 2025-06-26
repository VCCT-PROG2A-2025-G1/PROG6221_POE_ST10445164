using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Chatbot_GUI_POE
{
    public static class ChatbotState
    {
        public static int LastQuizScore { get; set; } = -1;
        public static int LastQuizTotal { get; set; } = 0;

        public static List<CyberTask> TaskList { get; set; } = new List<CyberTask>();
        public static List<string> ActivityLog { get; set; } = new List<string>();

    }

    public enum UserIntent
    {
        None,
        AddReminder,
        AddTask,
        StartQuiz,
        GetSummary,
        Unknown
    }

    public class NLPProcessor
    {
        public UserIntent DetectIntent(string userInput)
        {
            string input = userInput.ToLower();

            if (input.Contains("remind") || input.Contains("reminder") || input.Contains("notify"))
                return UserIntent.AddReminder;

            if (input.Contains("task") || input.Contains("to do") || input.Contains("todo") || input.Contains("add task"))
                return UserIntent.AddTask;

            if (input.Contains("quiz") || input.Contains("test") || input.Contains("question"))
                return UserIntent.StartQuiz;

            if (input.Contains("what have you done") || input.Contains("summary") || input.Contains("recent actions"))
                return UserIntent.GetSummary;

            return UserIntent.Unknown;
        }

        public string ExtractDescription(string userInput, UserIntent intent)
        {
            string input = userInput.ToLower();

            switch (intent)
            {
                case UserIntent.AddReminder:
                    int idx = input.IndexOf("remind me to");
                    if (idx >= 0)
                        return userInput.Substring(idx + "remind me to".Length).Trim();
                    break;

                case UserIntent.AddTask:
                    idx = input.IndexOf("add a task to");
                    if (idx >= 0)
                        return userInput.Substring(idx + "add a task to".Length).Trim();
                    idx = input.IndexOf("add task to");
                    if (idx >= 0)
                        return userInput.Substring(idx + "add task to".Length).Trim();
                    break;
            }

            return userInput; // fallback
        }

    }


}

