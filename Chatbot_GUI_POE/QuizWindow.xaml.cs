// Samuel Sossen
// ST10445164
// Group 1

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
        //--------------------------------------------------------------------------------------------------------------------------
        // Constructor
        //--------------------------------------------------------------------------------------------------------------------------
        public QuizWindow()
        {
            InitializeComponent();
            ChatbotState.LastQuizScore = -1;
            ChatbotState.LastQuizTotal = 0;
            LoadQuestions();
            ShowQuestion();
        }
        //--------------------------------------------------------------------------------------------------------------------------
        // Questions for Quiz
        //--------------------------------------------------------------------------------------------------------------------------
        private void LoadQuestions()
        {
            questions.Add(new QuizQuestion
            {
                Question = "What does 2FA stand for?",
                Options = new[] { "a) Two-Factor Authentication", "b) Fast Access", "c) File Authorization", "d) Final Agreement" },
                CorrectAnswer = 'a',
                Explanation = "2FA adds extra security by requiring a second login step."
            });
            questions.Add(new QuizQuestion
            {
                Question = "Which of these is a strong password?",
                Options = new[] { "a) password123", "b) 12345678", "c) T9@kLp$8", "d) Samuel123%%" },
                CorrectAnswer = 'c',
                Explanation = "A strong password includes a mix of letters, numbers, and symbols."
            });
            questions.Add(new QuizQuestion
            {
                Question = "What should you check before clicking a link in an email?",
                Options = new[] { "a) The color of the email", "b) The sender's address", "c) Font style", "d) Image attached" },
                CorrectAnswer = 'b',
                Explanation = "Always verify the sender's address to avoid phishing attacks."
            });
            questions.Add(new QuizQuestion
            {
                Question = "What is phishing?",
                Options = new[] { "a) A type of fishing", "b) A cyber attack to steal information", "c) A social media trend", "d) A way to talk to someone new" },
                CorrectAnswer = 'b',
                Explanation = "Phishing tricks you into giving away personal data."
            });
            questions.Add(new QuizQuestion
            {
                Question = "You should never share your password with anyone.",
                Options = new[] { "a) True", "b) False" },
                CorrectAnswer = 'a',
                Explanation = "Sharing your password can lead to unauthorized access to your accounts."
            });
            questions.Add(new QuizQuestion
            {
                Question = "You should change your password immediately if you suspect a data breach.",
                Options = new[] { "a) True", "b) False" },
                CorrectAnswer = 'a',
                Explanation = "Changing your password helps protect your account from unauthorized access."
            });
            questions.Add(new QuizQuestion
            {
                Question = "Public Wi-Fi is always safe if it doesn't require a password.",
                Options = new[] { "a) True", "b) False" },
                CorrectAnswer = 'b',
                Explanation = "Public Wi-Fi can be unsafe as anyone can access it"
            });
            questions.Add(new QuizQuestion
            {
                Question = "Antivirus software guarantees 100% protection from all threats.",
                Options = new[] { "a) True", "b) False" },
                CorrectAnswer = 'b',
                Explanation = "No antivirus can guarantee complete protection, it's just one layer of security."
            });
            questions.Add(new QuizQuestion
            {
                Question = "What is the purpose of a firewall?",
                Options = new[] { "a) To block unwanted traffic", "b) To speed up your internet", "c) To store files","d) Protects you from email attacks"  },
                CorrectAnswer = 'a',
                Explanation = "A firewall monitors and controls incoming and outgoing network traffic based on security rules."
            });
            questions.Add(new QuizQuestion
            {
                Question = "What is the purpose of a VPN?",
                Options = new[] { "a) Speed up your internet", "b) Encrypt your internet connection", "c) Improve your computer's performance", "d) Block pop-up ads"  },
                CorrectAnswer = 'b',
                Explanation = "VPNs protect your data by encrypting it."
            });
            questions.Add(new QuizQuestion
            {
                Question = "Which type of attack tricks users into revealing personal information?",
                Options = new[] { "a) Phishing", "b) Brute-force attack", "c) DDoS attack", "d) Spoofing" },
                CorrectAnswer = 'a',
                Explanation = "Phishing attacks use fake emails or websites to trick users into giving information."
            });
            questions.Add(new QuizQuestion
            {
                Question = "What is the purpose of a CAPTCHA?",
                Options = new[] { "a) To verify human users", "b) To speed up website loading", "c) To block ads", "d) To encrypt data" },
                CorrectAnswer = 'a',
                Explanation = "CAPTCHAs are used to ensure that the user is human and not a bot."
            });
            questions = questions.OrderBy(q => Guid.NewGuid()).ToList();
        }
        //--------------------------------------------------------------------------------------------------------------------------
        // This is a method to show the questions for the quiz it allows there to be multiple choice and true or false questions
        //--------------------------------------------------------------------------------------------------------------------------
        private void ShowQuestion()
        {
            if (currentQuestion < questions.Count)
            {
                var q = questions[currentQuestion];
                QuizQuestionText.Text = $"Q{currentQuestion + 1}: {q.Question}\n\n{string.Join("\n", q.Options)}";

               
                if (q.Options.Length == 2)
                {
                    TrueFalsePanel.Visibility = Visibility.Visible;
                    MultipleChoicePanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    TrueFalsePanel.Visibility = Visibility.Collapsed;
                    MultipleChoicePanel.Visibility = Visibility.Visible;

                   
                    OptionAButton.Content = q.Options.Length > 0 ? q.Options[0] : "";
                    OptionBButton.Content = q.Options.Length > 1 ? q.Options[1] : "";
                    OptionCButton.Content = q.Options.Length > 2 ? q.Options[2] : "";
                    OptionDButton.Content = q.Options.Length > 3 ? q.Options[3] : "";
                }
            }
            else
            {
                
                QuizQuestionText.Text = $" Quiz complete! You scored {score}/{questions.Count}.";

                ChatbotState.LastQuizScore = score;
                ChatbotState.LastQuizTotal = questions.Count;

                TrueFalsePanel.Visibility = Visibility.Collapsed;
                MultipleChoicePanel.Visibility = Visibility.Collapsed;
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------
        // This is a method for the answer button and replys with a feed back after answering
        //--------------------------------------------------------------------------------------------------------------------------
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
                feedback = $"✅ Correct! \n\nExplanation: {current.Explanation}";
            }
            else
            {
                feedback = $"❌ Incorrect. The correct answer was '{current.CorrectAnswer.ToString().ToUpper()}'.";
            }

            MessageBox.Show(feedback, "Answer Feedback", MessageBoxButton.OK, MessageBoxImage.Information);

            currentQuestion++;
            ShowQuestion();
        }
        //--------------------------------------------------------------------------------------------------------------------------
        // Exiting the Quiz
        //--------------------------------------------------------------------------------------------------------------------------
        private void ExitQuiz_Click(object sender, RoutedEventArgs e)
        {
            // Store final values when exiting
            ChatbotState.LastQuizScore = score;
            ChatbotState.LastQuizTotal = currentQuestion; 

            this.Close(); 
        }

    }
}
//----------------------------------------------------------------END OF FILE---------------------------------------------------------

