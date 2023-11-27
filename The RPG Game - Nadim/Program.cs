using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_RPG_Game___Nadim
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string theDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(theDirectory, "Map.txt");
            string lines = File.ReadAllText(path);
            Console.WriteLine();

            Console.WriteLine(lines);
   
        }
    }
}
