using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Model
    {
        public string username { get; }
        public string text { get; }
        public DateTime date { get; }

        public Model(string username, string text, DateTime date)
        {
            this.username = username;
            this.text = text;
            this.date = date;
        }
    }
}
