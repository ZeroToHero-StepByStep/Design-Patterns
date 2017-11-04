using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;


/*
 Single Responsibility Principle: A typical class is responsible for one thing,
 and only has one reason to change.
*/
namespace Single_Responsibility_Principle
{
    public class Journal
    {
        private readonly List<string> entries = new List<string>();
        private static int count = 0;

        public int AddEntry(string text)
        {
            entries.Add($"{++count}:{text}");
            return count;       //memento 
        }

        public void RemoveAt(int index)
        {
            entries.RemoveAt(index);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, entries);
        }


        // breaks single responsibility principle
        //        public void Save(string filename)
        //        {
        //            File.WriteAllText(filename,ToString());
        //        }
        //
        //
        //        public void Load(string filename)
        //        {
        //            
        //        }
        //
        //        public void Load(Url url)
        //        {
        //            
        //        }

    }


    // handles the responsibility of persisting objects
    public class Persistence
    {
        public void SaveToFile(Journal journal, string filename, bool overwrite = false)
        {
            if (overwrite || !File.Exists(filename))
                File.WriteAllText(filename, journal.ToString());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var j = new Journal();
            j.AddEntry("I cried today");
            j.AddEntry("I ate a bug");
            Console.WriteLine(j);

            var p = new Persistence();
            var filename = @"D:\temp\journal.txt";
            p.SaveToFile(j, filename);
            Process.Start(filename);

        }
    }
}
