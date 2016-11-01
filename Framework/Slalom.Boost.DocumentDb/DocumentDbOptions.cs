using System;
using System.Linq;

namespace Slalom.Boost.DocumentDb
{
    public class DocumentDbOptions
    {
        public string Host { get; set; }

        public int Port { get; set; } = 10250;

        public string Database { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Collection { get; set; }
    }
}
