using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubProject.Application.Model
{
    public class Message : IEntity<int>
    {

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected Message() { }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.

        public Message(string text, DateTime time, User user)
        {
            Text = text;  
            Time = time;
            User = user;
        }

        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public User User { get; set; }
    }
}
