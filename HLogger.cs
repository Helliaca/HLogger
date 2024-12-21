using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace HLogger
{
    public class Logger
    {
        const string filepath = "logs/";

        const string delimeter = ";";

        private readonly StreamWriter file;

        private StringBuilder buffer;

        public int lineCount { get; private set; } = 0;

        public bool isClosed => file == null || file.BaseStream == null;

        public Logger(string filename, params string[] headers)
        {
            string path = filepath + filename;
            string folder = new FileInfo(path).Directory.FullName;

            buffer = new StringBuilder();

            var dir = Directory.CreateDirectory(folder);
            file = File.CreateText(path);

            foreach (var header in headers)
            {
                Log(header);
            }
            EndLine();

            Console.WriteLine("New logger created at: " + filename);
        }

        public void Log(object o)
        {
            buffer.Append(System.Convert.ToString(o, System.Globalization.CultureInfo.InvariantCulture) + delimeter);
        }

        public void Log(params object[] os)
        {
            foreach (object o in os)
            {
                buffer.Append(System.Convert.ToString(o, System.Globalization.CultureInfo.InvariantCulture) + delimeter);
            }
        }

        public void EndLine()
        {
            //EndLineAsync().Wait();
            lineCount++;
            file.WriteLine(buffer.ToString());
            buffer.Clear();
        }

        public void Close()
        {
            if (file != null)
            {
                file.Close();
                file.Dispose(); // should not be necessary
            }
        }

        ~Logger()
        {
            Close();
        }
    }

}
