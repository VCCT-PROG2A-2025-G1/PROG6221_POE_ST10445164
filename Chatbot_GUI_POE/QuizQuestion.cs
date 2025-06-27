using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot_GUI_POE
{
    public class QuizQuestion
    {
        public string Question { get; set; }
        public string[] Options { get; set; }
        public char CorrectAnswer { get; set; }
        public string Explanation { get; set; }
    }
}
// ---------------------------------------------------------------END OF FILE --------------------------------------------------------------
