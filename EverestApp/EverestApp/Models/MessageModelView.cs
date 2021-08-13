using System;
using System.Collections.Generic;
using System.Text;

namespace EverestApp.Models
{
    class MessageModelView
    {
        public int NewMessagesCount { get; set; }
        public int NewMessagesCountForAll { get; set; }

        public IEnumerable<Message> Messages { get; set; }
    }
}
