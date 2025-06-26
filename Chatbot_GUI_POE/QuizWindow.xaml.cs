using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chatbot_GUI_POE
{
    /// <summary>
    /// Interaction logic for QuizWindow.xaml
    /// </summary>
    public partial class QuizWindow : Window
    {
        private List<QuizQuestion> questions = new List<QuizQuestion>();
        private int currentQuestion = 0;
        private int score = 0;

        public int FinalScore => score;
        public int TotalQuestions => questions.Count;

        public QuizWindow()
        {
            InitializeComponent();
            ChatbotState.LastQuizScore = -1;
            ChatbotState.LastQuizTotal = 0;
            LoadQuestions();
            ShowQuestion();
        }

        // Questions for Quiz
        private void LoadQuestions()
        {
            questions.Add(new QuizQuestion
            {
                Question = "What does 2FA stand for?",
                Options = new[] { "a) Two-Factor Authentication", "b) Fast Access", "c) File Authorization", "d) Final Agreement" },
                CorrectAnswer = 'a'
            });

            questions.Add(new QuizQuestion
            {
                Question = "Which of these is a strong password?",
                Options = new[] { "a) password123", "b) 12345678", "c) T9@kLp$8", "d) Samuel123%%" },
                CorrectAnswer = 'c'
            });

            questions.Add(new QuizQuestion
            {
                Question = "What should you check before clicking a link in an email?",
                Options = new[] { "a) The color of the email", "b) The sender's address", "c) Font style", "d) Image attached" },
                CorrectAnswer = 'b'
            });
            questions.Add(new QuizQuestion
            {
                Question = "What is phishing?",
                Options = new[] { "a) A type of fishing", "b) A cyber attack to steal information", "c) A social media trend", "d) A way to talk to someone new" },
                CorrectAnswer = 'b'
            });
            questions.Add(new QuizQuestion
            {
                Question = "You should never share your password with anyone.",
                Options = new[] { "a) True", "b) False" },
                CorrectAnswer = 'a'
            });
            questions.Add(new QuizQuestion
            {
                Question = "You should change your password immediately if you suspect a data breach.",
                Options = new[] { "a) True", "b) False" },
                CorrectAnswer = 'a'
            });

            questions.Add(new QuizQuestion
            {
                Question = "Public Wi-Fi is always safe if it doesn't require a password.",
                Options = new[] { "a) True", "b) False" },
                CorrectAnswer = 'b'
            });

            questions.Add(new QuizQuestion
            {
                Question = "Antivirus software guarantees 100% protection from all threats.",
                Options = new[] { "a) True", "b) False" },
                CorrectAnswer = 'b'
            });
            questions.Add(new QuizQuestion
            {
                Question = "What is the purpose of a firewall?",
                Options = new[] { "a) To block unwanted traffic", "b) To speed up your internet", "c) To store files",  },
                CorrectAnswer = 'a'
            });
            questions.Add(new QuizQuestion
            {
                Question = "Public Wi-Fi is always safe if it doesn't require a password.",
                Options = new[] { "a) True", "b) False" },
                CorrectAnswer = 'b'
            });
            questions.Add(new QuizQuestion
            {
                Question = "Antivirus software guarantees 100% protection from all threats.",
                Options = new[] { "a) True", "b) False" },
                CorrectAnswer = 'b'
            });
            questions.Add(new QuizQuestion
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
            questions.Add(new QuizQuestion
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
            questions.Add(new QuizQuestion
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
        }

        private void ShowQuestion()
        {
            if (currentQuestion < questions.Count)
            {
                var q = questions[currentQuestion];
                QuizQuestionText.Text = $"Q{currentQuestion + 1}: {q.Question}\n{string.Join("\n", q.Options)}";
            }
            else
            {
                QuizQuestionText.Text = $" Quiz complete! You scored {score}/{questions.Count}.";
                ChatbotState.LastQuizScore = score;
                ChatbotState.LastQuizTotal = questions.Count;
            }
        }


        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuestion >= questions.Count) return;

            var button = (Button)sender;
            char selected = Convert.ToChar(button.Tag.ToString());

            var current = questions[currentQuestion];

            string feedback;

            if (selected == current.CorrectAnswer)
            {
                score++;
                feedback = "✅ Correct!";
            }
            else
            {
                feedback = $"❌ Incorrect. The correct answer was '{current.CorrectAnswer.ToString().ToUpper()}'.";
            }

            MessageBox.Show(feedback, "Answer Feedback", MessageBoxButton.OK, MessageBoxImage.Information);

            currentQuestion++;
            ShowQuestion();
        }

        private void ExitQuiz_Click(object sender, RoutedEventArgs e)
        {
            // Store final values when exiting
            ChatbotState.LastQuizScore = score;
            ChatbotState.LastQuizTotal = currentQuestion; // How many questions were answered

            this.Close(); // Return to MainWindow
        }

    }
}

