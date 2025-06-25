using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot_GUI_POE
{
    class CyberTask
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }

        public override string ToString()
        {
            return $"{Title} – {Description}" +
                   (ReminderDate != null ? $" (Remind on {ReminderDate.Value.ToShortDateString()})" : "");
        }
    }
}
