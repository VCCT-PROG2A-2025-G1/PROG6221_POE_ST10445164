// Samuel Sossen
// ST10445164
// Group 1

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot_GUI_POE
{
    public class CyberTask
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public override string ToString()
        {
            return $"{Title}" + (ReminderDate != null ? $" (Remind on {ReminderDate:yyyy-MM-dd})" : "");
        }

    }
}
// ------------------------------------------------------------END OF FILE-----------------------------------------------------------------------
