using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_Server.Model
{
    public class Message
    {
        private string Username { get; set; }

        private string MessageText { get; set; }

        private string Timestamp { get; set; }

        public Message(string username, string messagetext, string timestamp)
        {
            Username = username;
            MessageText = messagetext;
            Timestamp = timestamp;
        }


    }
}
