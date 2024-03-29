using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatHubProject.Application.Model
{
    public class Server : IEntity<int>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Server() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Server(string name, User creator, string imageFilename)
        {
            Name = name;
            User = creator;
            ImageFilename = imageFilename;
        }

        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Name { get; set; }
        public string ImageFilename { get; set; }

        public User User { get; set; }

    }
}
