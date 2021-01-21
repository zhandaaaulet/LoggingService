using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoggingService
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Attachment { get; set; }

        public Message(IEnumerable<string> to, string subject, string content, string path)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x)));
            Subject = subject;
            Content = content;
            Attachment = path;
        }
    }
}
